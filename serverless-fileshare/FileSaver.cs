using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace serverless_fileshare
{
    class FileSaver
    {
        PendingFileTransferDB _pendingFileTransferDB;
        public FileSaver()
        {
            //TODO: Load pendingFileTransfers from DB
            _pendingFileTransferDB = new PendingFileTransferDB();
        }

        public void SavePacket(byte[] data)
        {
            int fileID=data[0];
            String fileLoc=GetFileLoc(fileID);
            FileStream fs = new FileStream(fileLoc, FileMode.Append); 
            fs.Write(data,1, data.Length);
            fs.Close();
        }

        private String GetFileLoc(int fileID)
        {
            //TODO: look up fileloc in DB
            return "testFile.txt";
        }
        
    }
}
