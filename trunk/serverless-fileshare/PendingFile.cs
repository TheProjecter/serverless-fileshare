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
        public PendingFile(int id,String fileLocation)
        {
            this.id = id;
            this.fileLocation = fileLocation;
        }
    }
}
