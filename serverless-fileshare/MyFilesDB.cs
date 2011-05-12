using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace serverless_fileshare
{
    class MyFilesDB
    {

        private const String _fileLoc = "FileHash.dat";

        private Hashtable _fileHashes;

        public MyFilesDB()
        {
            Load();
        }

        /// <summary>
        /// Adds the file at the given location and splits it up until multiple entries in the hash db
        /// </summary>
        /// <param name="fileLoc"></param>
        public void AddFile(String fileLoc)
        {
            //Remove the last slash in case it is a folder
            if (fileLoc.EndsWith("\\"))
                fileLoc = fileLoc.Substring(0, fileLoc.Length - 1);

            int lastSlash=fileLoc.LastIndexOf(@"\");
            String filename = fileLoc.Substring(lastSlash, fileLoc.Length - lastSlash);
            String[] toSplit = { " ", "-","\\" };
            foreach (String hashText in fileLoc.ToUpper().Split(toSplit, StringSplitOptions.None))
            {
                FileHash fHash = new FileHash();
                fHash.FileLoc = fileLoc;
                fHash.FileName = filename;
                fHash.Hash = hashText;
                _fileHashes.Add(hashText, fHash);
            }
        }

        /// <summary>
        /// Loads the hash table from disk
        /// </summary>
        private void Load()
        {

            if (!File.Exists(_fileLoc))
            {
                _fileHashes = new Hashtable();
                Save();
            }

            FileStream fs = new FileStream(_fileLoc,
                        FileMode.OpenOrCreate, FileAccess.Write);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                _fileHashes = (Hashtable)bf.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
        }

        public ArrayList SearchFor(String query)
        {
            ArrayList itemsFound = new ArrayList();
            String[] toSplit = { " ", "-","\\" };
            foreach (String hashSplit in query.ToUpper().Split(toSplit,StringSplitOptions.None))
            {
                if (_fileHashes.ContainsKey(hashSplit.GetHashCode()))
                {
                    itemsFound.Add(_fileHashes[hashSplit.GetHashCode()]);
                }
            }

            return itemsFound;
        }

        /// <summary>
        /// Saves the current hashtable to disk
        /// </summary>
        public void Save()
        {
            FileStream fs = new FileStream(_fileLoc,
                        FileMode.OpenOrCreate, FileAccess.Write);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, _fileHashes);
            }
            finally
            {
                fs.Close();
            }
        }
    }


    [Serializable]
    struct FileHash
    {
        public String Hash;
        public String FileName;
        public String FileLoc;
    }
}
