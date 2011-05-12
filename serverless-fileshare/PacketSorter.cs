using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace serverless_fileshare
{
    class PacketSorter
    {
        FileSaver _fileSaver;
        MyFilesDB _myFiles;
        OutboundManager _outBoundManager;

        public PacketSorter(MyFilesDB myFiles)
        {
            _myFiles = myFiles;
            _fileSaver = new FileSaver(_myFiles);
            
        }

        /// <summary>
        /// Takes an incoming SFPacket and decides where to go with it and performs its action
        /// </summary>
        /// <param name="incomingPacket">Packet received from the client</param>
        public void SortPacket(SFPacket incomingPacket)
        {
            switch(incomingPacket.GetPacketType())
            {
                case SFPacketType.FileTransfer:
                    _fileSaver.SavePacket(incomingPacket.GetPacketData());
                    break;
                case SFPacketType.SearchForFile:
                    ArrayList result=_myFiles.SearchFor(incomingPacket.GetPacketData().ToString());
                    _outBoundManager.SendFileList(result);
                    break;
                    
            }
        }
    }
}
