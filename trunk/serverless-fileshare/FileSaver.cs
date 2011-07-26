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
        MyFilesDB _myFiles;
        Hashtable pendingFileQueue;
        PendingFileTransferDB _pendingFileTransferDB;
        public FileSaver(MyFilesDB myFiles, PendingFileTransferDB pendingFileTransferDB)
        {
            _myFiles = myFiles;

            //TODO: Load pendingFileTransfers from DB
            _pendingFileTransferDB = pendingFileTransferDB;
            pendingFileQueue = new Hashtable();
        }

        /// <summary>
        /// Saves the byte[] to the given file
        /// </summary>
        /// <param name="data">byte array of the data</param>
        /// <param name="fromQueue">If the SavePacket is called from a queue then there
        /// is no need to add it again</param>
        /// <returns>returns true if the packet saved correctly</returns>
        public void SavePacket(byte[] data,String source)
        {
            int id = BitConverter.ToInt32(data, 0);
            String fileID =id +source;
            if (pendingFileQueue.ContainsKey(fileID))
            {
                ((PendingFileQueue)pendingFileQueue[fileID]).AddPacket(data);
            }
            else
            {
                PendingFileQueue pfq = new PendingFileQueue(data,id,GetFileLoc(fileID,source));
                pendingFileQueue.Add(fileID, pfq);
            }
        }

        private String GetFileLoc(String fileID,String source)
        {
            return _pendingFileTransferDB.GetPendingWithID(fileID).fileLocation;
        }
        
    }
}
