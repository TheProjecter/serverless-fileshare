using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    class PacketSorter
    {
        FileSaver _fileSaver;

        public PacketSorter()
        {
            _fileSaver = new FileSaver();
        }

        public void SortPacket(SFPacket incomingPacket)
        {
            switch(incomingPacket.GetPacketType())
            {
                case SFPacketType.IncomingFileTransfer:
                    _fileSaver.SavePacket(incomingPacket.GetPacketData());
                    break;
            }
        }
    }
}
