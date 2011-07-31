using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
namespace serverless_fileshare
{
    public class OutboundManager
    {
        MovingTCPScheduler _scheduler;
        ArrayList _filesToSend;
        public OutboundManager(MovingTCPScheduler scheduler)
        {
            _scheduler = scheduler;
            //TODO: Keep track of files queued to send
            _filesToSend = new ArrayList();
        }

        public void SendFile(int fileID,String file, IPAddress destination)
        {

            //ParameterizedThreadStart ts = new ParameterizedThreadStart(ThreadedSendFile);
            //Thread td = new Thread(ts);
            object[] obj = {fileID, file, destination };
            //td.Start(obj);
            ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadedSendFile), obj);
        }

        private void ThreadedSendFile(object parameters)
        {
            object[] parms = (object[])parameters;
            int fileID=(int)parms[0];
            String file = (String)parms[1];
            IPAddress destination = (IPAddress)parms[2];

            FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read);
            BinaryReader br = new BinaryReader(fs);
            long bytesRead = 0;
            long fileSize = new FileInfo(file).Length;
            int packetSize=Properties.Settings.Default.PacketDataSize;
            byte[] buffer;
            int packetsSent = 0;
            int toSkip = BitConverter.GetBytes(fileID).Length;
            //try{
                while (bytesRead+(packetSize-toSkip)<fileSize)
                {
                    buffer = br.ReadBytes(packetSize-toSkip);
                    byte[] toSend=ShiftBytes(fileID, buffer);
                    SFPacket packet = new SFPacket(SFPacketType.FileTransfer,toSend );
                    _scheduler.SendPacket(packet, destination);
                    bytesRead += buffer.Length;
                    packetsSent++;
                }
                
                if (fileSize > bytesRead)
                {
                    buffer = br.ReadBytes((int)(fileSize - bytesRead));
                    byte[] toSend = ShiftBytes(fileID, buffer);
                    SFPacket finalPacket = new SFPacket(SFPacketType.FileTransfer, toSend);
                    _scheduler.SendPacket(finalPacket, destination);
                    packetsSent++;
                }

                byte[] bytes = Encoding.ASCII.GetBytes("EOT");
                byte[] EOT = ShiftBytes(fileID, bytes);
                SFPacket eotPacket = new SFPacket(SFPacketType.FileTransfer, EOT);
                _scheduler.SendPacket(eotPacket, destination);
                packetsSent++;

                br.Close();
                System.Console.WriteLine("Packets Sent: " + packetsSent);
            /*}
            catch (Exception ex)
            {

            }
             */

        }

        /// <summary>
        /// Puts the fileID as the first byte in the byte array
        /// </summary>
        /// <param name="fileID">file id integer</param>
        /// <param name="data">byte array needing to be shifted</param>
        /// <returns></returns>
        private byte[] ShiftBytes(int fileID, byte[] data)
        {
            byte[] fileIDBytes = BitConverter.GetBytes(fileID);
            byte[] toReturn = new byte[data.Length + fileIDBytes.Length];
            int i = 0;
            for ( i= 0; i < fileIDBytes.Length; i++)
            {
                toReturn[i] = fileIDBytes[i];
            }
            int dataCount = 0;
            for (int x = i; x < toReturn.Length; x++)
            {
                toReturn[x] = data[dataCount];
                dataCount++;
            }
            return toReturn;
        }



        public void SendFileList(ArrayList files,IPAddress destination)
        {

            byte[] data=null;
            int packetSize = Properties.Settings.Default.PacketDataSize;

            while (data == null || data.Length+2 > packetSize)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, files);
                data = ms.ToArray();

                if (data.Length+2 > packetSize)
                {
                    //If there are too many items because it goes over the packet size remove
                    //the last item in it
                    files.RemoveAt(files.Count - 1);
                }
            }

            long bytesRead = 0;
            byte[] buffer;
            int packetsSent = 0;

            try
            {
                while (bytesRead + (packetSize) < data.Length)
                {
                    buffer = new byte[packetsSent];  
                    Array.Copy(data, buffer, packetsSent);
                    SFPacket packet = new SFPacket(SFPacketType.FileList, buffer);
                    _scheduler.SendPacket(packet, destination);
                    bytesRead += buffer.Length;
                    packetsSent++;
                }

                if (data.Length > bytesRead)
                {
                    byte[] toSend = new byte[data.Length];
                    Array.Copy(data,toSend,((int)(data.Length - bytesRead)));
                    SFPacket finalPacket = new SFPacket(SFPacketType.FileList, toSend);
                    _scheduler.SendPacket(finalPacket, destination);
                    packetsSent++;
                }

                System.Console.WriteLine("Packets Sent: " + packetsSent);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Sends a search request packet to given IP address
        /// </summary>
        /// <param name="query">The query to be sent</param>
        /// <param name="dest">IP address of the person you want to query</param>
        public void SendSearchRequest(String query, IPAddress dest)
        {
            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes(query);
            if (bytes.Length < Properties.Settings.Default.PacketDataSize)
            {
                SFPacket packet = new SFPacket(SFPacketType.SearchForFile, bytes);
                _scheduler.SendPacket(packet, dest);
            }
            else
            {
                throw new Exception("Search Request data size is too large for one packet: " + query);
            }
        }

        /// <summary>
        /// Sends a packet requesting a file download
        /// </summary>
        /// <param name="fileID">id of the file you want</param>
        /// <param name="dest">IPAddress of the destination</param>
        public void SendFileDownloadRequest(int fileID, IPAddress dest)
        {
            byte[] data = BitConverter.GetBytes(fileID);
            SFPacket packet=new SFPacket(SFPacketType.FileDownloadRequest,data);
            _scheduler.SendPacket(packet,dest);
            
        }

        public void SendNeighborDownloadRequest(IPAddress dest)
        {
            byte[] bytes;
            bytes = Encoding.ASCII.GetBytes("");
            if (bytes.Length < Properties.Settings.Default.PacketDataSize)
            {
                SFPacket packet = new SFPacket(SFPacketType.GetNeighborList, bytes);
                _scheduler.SendPacket(packet, dest);
            }
        }

        public void SendNeigbhorList(IPAddress dest)
        {
            byte[] data=null;
            int packetSize = Properties.Settings.Default.PacketDataSize;
            ArrayList neighbors = _scheduler.myNeighbors.GetListOfNeighbors();
            while (data == null || data.Length+2 > packetSize)
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream();
                bf.Serialize(ms, neighbors);
                data = ms.ToArray();

                if (data.Length+2 > packetSize)
                {
                    //If there are too many items because it goes over the packet size remove
                    //the first item because we hope that was received earlier
                    neighbors.RemoveAt(0);
                }
            }

            long bytesRead = 0;
            byte[] buffer;
            int packetsSent = 0;

            try
            {
                while (bytesRead + (packetSize) < data.Length)
                {
                    buffer = new byte[packetsSent];  
                    Array.Copy(data, buffer, packetsSent);
                    SFPacket packet = new SFPacket(SFPacketType.NeighborListResponse, buffer);
                    _scheduler.SendPacket(packet, dest);
                    bytesRead += buffer.Length;
                    packetsSent++;
                }

                if (data.Length > bytesRead)
                {
                    byte[] toSend = new byte[data.Length];
                    Array.Copy(data,toSend,((int)(data.Length - bytesRead)));
                    SFPacket finalPacket = new SFPacket(SFPacketType.NeighborListResponse, toSend);
                    _scheduler.SendPacket(finalPacket, dest);
                    packetsSent++;
                }

                System.Console.WriteLine("Packets Sent: " + packetsSent);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
