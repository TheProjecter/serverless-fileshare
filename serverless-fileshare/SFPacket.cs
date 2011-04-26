using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    class SFPacket
    {
        int _type;
        byte[] _data;
        public SFPacket(byte[] message,int messageSize)
        {
            this._type = GrabType(message);
            this._data = GrabData(message,messageSize);
        }

        public SFPacket(int PacketType, byte[] sentData)
        {
            _type = PacketType;
            _data = sentData;
        }
        
        public int GetPacketType()
        {
            return _type;
        }

        public byte[] GetPacketData()
        {
            return _data;
        }

        public byte[] GetRawPacket()
        {
            byte[] total = new byte[_data.Length + 1];
            int x=0;
            total[0] =(byte) _type;
            for (x = 1; x < _data.Length+1; x++)
            {
                total[x] = _data[x-1];
            }
            return total;
        }

        private int GrabType(byte[] message)
        {

            return (int)message[0];
        }

        private byte[] GrabData(byte[] message,int messageSize)
        {
            byte[] workingBytes = new byte[messageSize-1];
            for (int x = 0; x < workingBytes.Length; x++)
            {
                workingBytes[x] = message[x+1];
            }
            return workingBytes;
        }
    }
}
