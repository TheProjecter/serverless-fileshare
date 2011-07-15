using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace serverless_fileshare
{
    public class PendingFileTransferDB
    {
        Hashtable _pendingFiles;
        public PendingFileTransferDB()
        {
            _pendingFiles = new Hashtable();
        }

        public void AddPendingFile(PendingFile pFile)
        {
            _pendingFiles.Add(pFile.id, pFile);
        }

        /// <summary>
        /// Returns the PendingFile object with the give ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PendingFile GetPendingWithID(int id)
        {
            object found= _pendingFiles[id];
            if(found==null)
                return null;
            return (PendingFile)found;
        }
    }
}
