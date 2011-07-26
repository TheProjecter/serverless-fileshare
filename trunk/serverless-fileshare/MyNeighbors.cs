using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
namespace serverless_fileshare
{
    public class MyNeighbors
    {
        ArrayList _listOfNeighbors;
        private const String _fileLoc = "MyNeighbors.dat";
        System.Windows.Forms.Timer _checkNeighbors = new System.Windows.Forms.Timer();
        OutboundManager _outbound;
        public MyNeighbors(MovingTCPScheduler scheduler)
        {

            Load();
            _outbound = scheduler.outboundManager;
            _checkNeighbors.Interval = Properties.Settings.Default.PortChangeInterval*60*1000;
            _checkNeighbors.Tick += new EventHandler(_checkNeighbors_Tick);
            _checkNeighbors.Start();
            scheduler.myNeighbors = this;
        }

        /// <summary>
        /// When the timer clicks this method gets a random start location in the
        /// list of neighbors and sends a "download request" to the next 10
        ///  people in the net for a list of their neighbors.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _checkNeighbors_Tick(object sender, EventArgs e)
        {
            
            Random rand=new Random();
            int maxValue = _listOfNeighbors.Count-1;
            int maxCount = _listOfNeighbors.Count - 1;
            if (_listOfNeighbors.Count > 10)
            {
                maxValue = _listOfNeighbors.Count - 11;
                maxCount = 10;
            }
            int startLoc=rand.Next(maxValue);
            if(startLoc+maxCount>_listOfNeighbors.Count)
                maxCount=(_listOfNeighbors.Count-startLoc);
            foreach (Neighbor nb in _listOfNeighbors.GetRange(startLoc,maxCount))
            {
                _outbound.SendNeighborDownloadRequest(IPAddress.Parse(nb.IPAddress));
            }
        }

        public void AddNeighbor(String IPAddressStr)
        {
            IPAddress ip = IPAddress.Parse(IPAddressStr);
            Neighbor nb = new Neighbor();
            nb.IPAddress = IPAddressStr;
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
