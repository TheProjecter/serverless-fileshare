using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace serverless_fileshare
{
    public class MyNeighbors
    {
        ArrayList _listOfNeighbors;
        private const String _fileLoc = "MyNeighbors.dat";
        System.Windows.Forms.Timer _checkNeighbors = new System.Windows.Forms.Timer();

        public MyNeighbors()
        {

            Load();
            _checkNeighbors.Interval = Properties.Settings.Default.PortChangeInterval;
            _checkNeighbors.Tick += new EventHandler(_checkNeighbors_Tick);
            
        }

        void _checkNeighbors_Tick(object sender, EventArgs e)
        {
            foreach (Neighbor nb in _listOfNeighbors)
            {
                //TODO: telnet on the current port?
            }
        }

        public void AddNeighbor(String IPAddress)
        {
            Neighbor nb = new Neighbor();
            nb.IPAddress = IPAddress;
            if (!_listOfNeighbors.Contains(nb))
            {
                _listOfNeighbors.Add(nb);
                Save();
            }
        }

        public ArrayList GetListOfNeighbors()
        {
            return _listOfNeighbors;
        }
        
        private void Load()
        {

            if (!File.Exists(_fileLoc))
            {
                _listOfNeighbors = new ArrayList();
                Save();
            }

            FileStream fs = new FileStream(_fileLoc,
                        FileMode.OpenOrCreate, FileAccess.Read);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                _listOfNeighbors = (ArrayList)bf.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }
        }

        public void Save()
        {
            FileStream fs = new FileStream(_fileLoc,
                        FileMode.OpenOrCreate, FileAccess.Write);

            try
            {

                BinaryFormatter bf = new BinaryFormatter();
                bf.Serialize(fs, _listOfNeighbors);
            }
            finally
            {
                fs.Close();
            }
        }
    }

    [Serializable]
    struct Neighbor
    {
        public String IPAddress;

        public override Boolean Equals(object obj)
        {
            Neighbor nb = (Neighbor)obj;
            return IPAddress == nb.IPAddress;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
