using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    public class PendingFile
    {
        public int id;
        public String fileLocation;
        public String Source;
        public PendingFile(int id,String fileLocation,String source)
        {
            this.id = id;
            this.fileLocation = fileLocation;
            this.Source = source;
        }
    }
}
