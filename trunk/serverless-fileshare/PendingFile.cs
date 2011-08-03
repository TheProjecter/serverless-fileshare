using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace serverless_fileshare
{
    [Serializable]
    public class PendingFile
    {
        public int id;
        public String fileLocation;
        public String Source;
        public DateTime lastPacketReceived;
        public DateTime lastRequestSent;
        public PendingFile(int id,String fileLocation,String source)
        {
            this.id = id;
            this.fileLocation = fileLocation;
            this.Source = source;
            lastPacketReceived = DateTime.Now;
            lastRequestSent = DateTime.Now;
        }

        public PendingFile(SerializationInfo info, StreamingContext ctxt)
        {
            fileLocation = (String)info.GetValue("FileLocation", typeof(String));
            Source = (String)info.GetValue("Source", typeof(String));
            id = (int)info.GetValue("id", typeof(int));
            lastPacketReceived = (DateTime)info.GetValue("lastPacketReceived", typeof(DateTime));
            lastRequestSent = (DateTime)info.GetValue("lastRequestSent", typeof(DateTime));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("id", id);
            info.AddValue("fileLocation", fileLocation);
            info.AddValue("Source", Source);
            info.AddValue("lastPacketReceived", lastPacketReceived);
            info.AddValue("lastRequestSent", lastRequestSent);
        }
    }
}
