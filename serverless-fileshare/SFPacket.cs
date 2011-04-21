using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    class SFPacket
    {
        String _type;
        byte[] _data;
        public SFPacket(byte[] message)
        {
            this._type = GrabType(message);
            this._data = GrabData(message);
        }

        public SFPacket(String PacketType, byte[] sentData)
        {
            _type = PacketType;
            _data = sentData;
        }
        
        public String GetPacketType()
        {
            return _type;
        }

        public byte[] GetPacketData()
        {
            return _data;
        }

        public byte[] GetRawPacket()
        {
            byte[] total = new byte[_data.Length + 8];
            int x=0;
            foreach (byte b in _type)
            {
                total[x] = b;
                x++; 
            }
            for (x = 8; x < _data.Length; x++)
            {
                total[x] = _data[x];
            }
            return total;
        }

        private String GrabType(byte[] message)
        {
            byte[] typeBytes = new byte[8];
            for (int x = 0; x < 8; x++)
            {
                typeBytes[x] = message[x];
            }
            ASCIIEncoding encoder = new ASCIIEncoding();
            return encoder.GetString(typeBytes);
        }

        private byte[] GrabData(byte[] message)
        {
            byte[] workingBytes = new byte[message.Length-8];
            for (int x = 8; x < message.Length-8; x++)
            {
                workingBytes[x] = message[x];
            }
            return workingBytes;
        }
    }
}
