﻿using System;
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
    class OutboundManager
    {
        MovingTCPScheduler _scheduler;
        public OutboundManager(MovingTCPScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void SendFile(int fileID,String file, IPAddress destination)
        {

            ParameterizedThreadStart ts = new ParameterizedThreadStart(ThreadedSendFile);
            Thread td = new Thread(ts);
            object[] obj = {fileID, file, destination };
            td.Start(obj);
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

            try
            {
                while (bytesRead+(packetSize-1)<fileSize)
                {
                    buffer = br.ReadBytes(packetSize-1);
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

                br.Close();
                System.Console.WriteLine("Packets Sent: " + packetsSent);
            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Puts the fileID as the first byte in the byte array
        /// </summary>
        /// <param name="fileID">file id integer</param>
        /// <param name="data">byte array needing to be shifted</param>
        /// <returns></returns>
        private byte[] ShiftBytes(int fileID, byte[] data)
        {
            byte[] toReturn = new byte[data.Length + 1];
            toReturn[0] =(byte) fileID;
            for (int x = 1; x < data.Length+1; x++)
            {
                toReturn[x] = data[x-1];
            }
            return toReturn;
        }



        public void SendFileList(ArrayList files)
        {
            //TODO: Well crap. This needs to split an arraylist into multiple packets (if its huge). And if that happens 
            //then i'm really screwed because I have to put them back together as one arraylist... Lets make the packet
            //size big enough to allow for a good sized arraylist and drop the rest after that...
        }

        public void SendSearchRequest(String query, IPAddress dest)
        {
            BinaryFormatter bf = new BinaryFormatter();
            byte[] bytes;
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, query);
            bytes = ms.ToArray();
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
    }
}