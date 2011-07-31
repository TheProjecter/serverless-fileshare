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
        ArrayList _completedTransfers;
        public PendingFileTransferDB()
        {
            _pendingFiles = new Hashtable();
        }

        public void AddPendingFile(PendingFile pFile)
        {
            String key=pFile.id+pFile.Source;
            if(!_pendingFiles.Contains(key))
            _pendingFiles.Add(key, pFile);
        }

        public void MarkPendingAsComplete(String id)
        {
            PendingFile pf = GetPendingWithID(id);
            if (pf != null)
            {
                _completedTransfers.Add(pf);
                _pendingFiles.Remove(id);
            }
        }

        public ArrayList GetPendingFileList()
        {
            ArrayList toReturn = new ArrayList();
            foreach (String key in _pendingFiles.Keys)
            {
                toReturn.Add(_pendingFiles[key]);
            }
            return toReturn;
        }

        public int GetPendingFileCount()
        {
            return _pendingFiles.Count;
        }

        /// <summary>
        /// Returns the PendingFile object with the give ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PendingFile GetPendingWithID(String id)
        {
            object found= _pendingFiles[id];
            if(found==null)
                return null;
            return (PendingFile)found;
        }
    }
}
