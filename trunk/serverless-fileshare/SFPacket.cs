using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    class SFPacket
    {
        String type;
        byte[] data;
        public SFPacket(byte[] message)
        {
            this.type = GrabType(message);
            this.data = GrabData(message);
        }

        public String GetPacketType()
        {
            return type;
        }

        public byte[] GetPacketData()
        {
            return data;
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
            byte[] workingBytes = new byte[8];
            for (int x = 8; x < message.Length; x++)
            {
                workingBytes[x] = message[x];
            }
            return workingBytes;
        }
    }
}
