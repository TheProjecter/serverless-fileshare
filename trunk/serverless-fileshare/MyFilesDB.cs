using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
namespace serverless_fileshare
{
    public class MyFilesDB
    {

        private const String _fileLoc = "FileHash.dat";
        private const String _fileNameLoc = "FileNames.dat";

        private Hashtable _fileHashes;
        private ArrayList _fileNames;
        private String[] toSplit = { " ", "-", "\\","." };
        public MyFilesDB()
        {

            _fileHashes = new Hashtable();
            _fileNames = new ArrayList();
            Load();
        }

        /// <summary>
        /// Adds the file at the given location and splits it up into multiple entries in the hash db
        /// </summary>
        /// <param name="fileLoc"></param>
        public void AddFile(String fileLoc)
        {
            if (File.Exists(fileLoc))
            {
                if (!_fileNames.Contains(fileLoc))
                {
                    //Remove the last slash in case it is a folder
                    if (fileLoc.EndsWith("\\"))
                        fileLoc = fileLoc.Substring(0, fileLoc.Length - 1);

                    int lastSlash = fileLoc.LastIndexOf(@"\") + 1;
                    String filename = fileLoc.Substring(lastSlash, fileLoc.Length - lastSlash);
                    //split file location into spaces,dashes, and slashes
                    foreach (String hashText in fileLoc.ToUpper().Split(toSplit, StringSplitOptions.None))
                    {
                        FileHash fHash = new FileHash();
                        fHash.Hash = hashText.ToUpper().GetHashCode();
                        if (_fileHashes.ContainsKey(fHash.Hash))
                        {
                            fHash = (FileHash)_fileHashes[fHash.Hash];
                        }
                        else
                        {
                            fHash.FileList = new ArrayList();
                            _fileHashes.Add(fHash.Hash, fHash);
                        }
                        fHash.FileList.Add(new MyFile(filename, fileLoc,_fileNames.Count));
                       

                    }
                    _fileNames.Add(fileLoc);
                    Save();
                }
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
                _fileNames = new ArrayList();
                Save();
            }

            FileStream fs = new FileStream(_fileLoc,
                        FileMode.OpenOrCreate, FileAccess.Read);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                _fileHashes = (Hashtable)bf.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }

            fs = new FileStream(_fileNameLoc,
                        FileMode.OpenOrCreate, FileAccess.Read);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                _fileNames = (ArrayList)bf.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
        }

        public ArrayList SearchFor(String query)
        {
            ArrayList itemsFound = new ArrayList();
            

            foreach (String hashSplit in query.ToUpper().Split(toSplit,StringSplitOptions.None))
            {
                if (_fileHashes.ContainsKey(hashSplit.GetHashCode()))
                {
                    itemsFound.Add(_fileHashes[hashSplit.GetHashCode()]);
                }
            }
            //TODO: Order items found by the number of occurances of the given string
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

            fs = new FileStream(_fileNameLoc,
                        FileMode.OpenOrCreate, FileAccess.Write);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, _fileNames);
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
        public int Hash;
        public ArrayList FileList;
    }

    [Serializable]
    class MyFile
    {

        public String FileName;
        public String FileLoc;
        public int FileNumber;
        public MyFile(String filename, String fileloc,int fileNumber)
        {
            FileName = filename;
            FileLoc = fileloc;
            FileNumber = fileNumber;
        }

        public MyFile(SerializationInfo info, StreamingContext ctxt)
        {
            FileName = (String)info.GetValue("FileName", typeof(int));
            FileLoc = (String)info.GetValue("FileLoc", typeof(string));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {

            info.AddValue("FileName", FileName);
            info.AddValue("FileLoc", FileLoc);
        }
    }
}
