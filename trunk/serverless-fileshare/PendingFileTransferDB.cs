using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace serverless_fileshare
{
    [Serializable]
    public class PendingFileTransferDB 
    {
        Hashtable _pendingFiles;
        ArrayList _completedTransfers;
        private static string _fileLocation="PendingTransfer.dat";
        public PendingFileTransferDB()
        {
            _pendingFiles = new Hashtable();
            _completedTransfers = new ArrayList();

            if (File.Exists(_fileLocation))
            {
                FileStream reader = new FileStream(_fileLocation,FileMode.OpenOrCreate, FileAccess.Read);
                BinaryFormatter format = new BinaryFormatter();
                try
                {
                    object returned = format.Deserialize(reader);
                    this._pendingFiles = ((PendingFileTransferDB)returned)._pendingFiles;
                    this._completedTransfers = ((PendingFileTransferDB)returned)._completedTransfers;
                    reader.Close();
                }
                catch (Exception ex) { }
               
            }
        }

        public void Save()
        {
            FileStream fs = new FileStream(_fileLocation, FileMode.Create, FileAccess.Write);
            BinaryFormatter format = new BinaryFormatter();
            format.Serialize(fs, this);
            fs.Close();
        }

        public PendingFileTransferDB(SerializationInfo info, StreamingContext ctxt)
        {
            _pendingFiles = (Hashtable)info.GetValue("pendingFiles", typeof(Hashtable));
            _completedTransfers = (ArrayList)info.GetValue("completedTransfer", typeof(ArrayList));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("pendingFiles", _pendingFiles);
            info.AddValue("completedTransfer", _completedTransfers);
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
