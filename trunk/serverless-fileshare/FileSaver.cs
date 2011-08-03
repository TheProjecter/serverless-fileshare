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
                if (data.Length > 10)
                {
                    PendingFile pFile = _pendingFileTransferDB.GetPendingWithID(fileID);
                    if(pFile!=null)
                        pFile.lastPacketReceived = DateTime.Now;
                    ((PendingFileQueue)pendingFileQueue[fileID]).AddPacket(data);
                }
                else
                {
                    byte[] trimmed=new byte[data.Length-1];
                    Array.Copy(data, 1, trimmed, 0, trimmed.Length);
                    String fileText= System.Text.Encoding.ASCII.GetString(trimmed);
                    if (fileText.EndsWith("EOT"))
                    {
                        _pendingFileTransferDB.MarkPendingAsComplete(fileID);
                    }
                }
            }
            else
            {
                try
                {
                    PendingFileQueue pfq = new PendingFileQueue(data, id, GetFileLoc(fileID, source));
                    pendingFileQueue.Add(fileID, pfq);
                }
                catch(Exception ex)
                { 

                    ((PendingFileQueue)pendingFileQueue[fileID]).AddPacket(data); 
                }
            }
        }

        private String GetFileLoc(String fileID,String source)
        {
            return _pendingFileTransferDB.GetPendingWithID(fileID).fileLocation;
        }
        
    }
}
