﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace serverless_fileshare
{
    class PacketSorter
    {
        FileSaver _fileSaver;
        MyFilesDB _myFiles;
        OutboundManager _outBoundManager;
        MovingTCPScheduler _scheduler;

        public PacketSorter(MyFilesDB myFiles,MovingTCPScheduler scheduler)
        {
            _myFiles = myFiles;
            _fileSaver = new FileSaver(_myFiles,scheduler.fileTransferDB);
            _outBoundManager = scheduler.outboundManager;
            _scheduler = scheduler;
        }

        /// <summary>
        /// Takes an incoming SFPacket and decides where to go with it and performs its action
        /// </summary>
        /// <param name="incomingPacket">Packet received from the client</param>
        public void SortPacket(SFPacket incomingPacket)
        {
            _scheduler.myNeighbors.AddNeighbor(incomingPacket._sourceIP.ToString());
            switch(incomingPacket.GetPacketType())
            {
                case SFPacketType.FileTransfer:
                    _fileSaver.SavePacket(incomingPacket.GetPacketData(),incomingPacket._sourceIP.ToString());
                    break;
                case SFPacketType.SearchForFile:
                    ArrayList result = _myFiles.SearchFor(
                        System.Text.Encoding.ASCII.GetString(incomingPacket.GetPacketData()));
                    _outBoundManager.SendFileList(result,incomingPacket._sourceIP);
                    break;
                case SFPacketType.FileList:
                    MemoryStream stream = new MemoryStream(incomingPacket.GetPacketData());
                    BinaryFormatter bf = new BinaryFormatter();
                    ArrayList searchResults = (ArrayList)bf.Deserialize(stream);
                    _scheduler.fileSearchForm.AddResults(searchResults,incomingPacket._sourceIP);
                    break;

                case SFPacketType.FileDownloadRequest:
                    //Get the fileID
                    int fileId=BitConverter.ToInt32(incomingPacket.GetPacketData(),0);
                    //Lookup the file by its ID
                    MyFile file=_myFiles.GetFileByID(fileId);
                    //Send back the file
                    _outBoundManager.SendFile(fileId, file.FileLoc, incomingPacket._sourceIP);
                    break;

                case SFPacketType.GetNeighborList:
                    _outBoundManager.SendNeigbhorList(incomingPacket._sourceIP);
                    break;
                case SFPacketType.NeighborListResponse:
                    MemoryStream nlstream = new MemoryStream(incomingPacket.GetPacketData());
                    BinaryFormatter nlbf = new BinaryFormatter();
                    ArrayList neighbors = (ArrayList)nlbf.Deserialize(nlstream);
                    foreach (Neighbor nb in neighbors)
                        _scheduler.myNeighbors.AddNeighbor(nb.IPAddress);
                    break;
                    
            }
        }
    }
}
