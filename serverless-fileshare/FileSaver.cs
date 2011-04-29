using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Collections;
namespace serverless_fileshare
{
    class FileSaver
    {
        PendingFileTransferDB _pendingFileTransferDB;
        
        Hashtable pendingFileQueue;
        public FileSaver()
        {
            //TODO: Load pendingFileTransfers from DB
            _pendingFileTransferDB = new PendingFileTransferDB();
            pendingFileQueue = new Hashtable();
        }

        /// <summary>
        /// Saves the byte[] to the given file
        /// </summary>
        /// <param name="data">byte array of the data</param>
        /// <param name="fromQueue">If the SavePacket is called from a queue then there
        /// is no need to add it again</param>
        /// <returns>returns true if the packet saved correctly</returns>
        public void SavePacket(byte[] data)
        {
            int fileID = data[0];
            if (pendingFileQueue.ContainsKey(fileID))
            {
                ((PendingFileQueue)pendingFileQueue[fileID]).AddPacket(data);
            }
            else
            {
                PendingFileQueue pfq = new PendingFileQueue(data,fileID,GetFileLoc(fileID));
                pendingFileQueue.Add(fileID, pfq);
            }
        }

        private String GetFileLoc(int fileID)
        {
            //TODO: look up fileloc in DB
            return @"C:\Test\received.mp3";
        }
        
    }
}
