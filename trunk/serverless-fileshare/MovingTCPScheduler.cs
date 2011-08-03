using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace serverless_fileshare
{
    public class MovingTCPScheduler
    {
        PortFinder _portFinder = new PortFinder();
        PortListener[] _portListeners;
        PacketSorter _sorter;
        private Dictionary<string, DateTime> _unreachableNeighbors;
        public OutboundManager outboundManager;
        public FileSearchForm fileSearchForm;
        public PendingFileTransferDB fileTransferDB;
        public MyNeighbors myNeighbors;

        System.Windows.Forms.Timer _portChangeClock = new System.Windows.Forms.Timer();

        public MovingTCPScheduler(MyFilesDB myFiles)
        {

            fileTransferDB = new PendingFileTransferDB();
            _portListeners = new PortListener[3];
            _portChangeClock.Interval = (Properties.Settings.Default.PortChangeInterval*50*1000)/2;
            _portChangeClock.Tick += new EventHandler(timer_Tick);
            outboundManager = new OutboundManager(this);
            _sorter = new PacketSorter(myFiles,this);
            _unreachableNeighbors = new Dictionary<string, DateTime>();
        }

        public void Start()
        {
            _portChangeClock.Start();
            _portListeners[0] = new PortListener(IPAddress.Any, _portFinder.GetPreviousPort(), _sorter);
            _portListeners[1] = new PortListener(IPAddress.Any, _portFinder.GetCurrentPort(), _sorter);
            _portListeners[2] = new PortListener(IPAddress.Any, _portFinder.GetNextPort(), _sorter);
            StartListeners();
        }

        public void Stop()
        {
            _portChangeClock.Stop();
            StopListeners();
            fileTransferDB.Save();
            myNeighbors.Save();
        }

        public void SendPacket(SFPacket packet, IPAddress destination)
        {
            SendPacket(packet, destination, 0);
        }

        public void SendPacket(SFPacket packet, IPAddress destination, int tries)
        {
            object[] obj = { packet, destination,tries };
            if (tries == 0)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadedPacketSend), obj);
                Thread.Sleep(10);
            }
            else
            {
                if (tries < 1000000) 
                    ThreadedPacketSend(obj);
            }
        }
        private void ThreadedPacketSend(object parameters)
        {
            

            object[] parms = (object[])parameters;
            SFPacket packet = (SFPacket)parms[0];
            IPAddress destination =(IPAddress) parms[1];
            int tries = (int)parms[2];
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPEndPoint destip = new IPEndPoint(destination, _portFinder.GetCurrentPort());
                socket.Connect(destip);
                socket.SendTimeout = 0;
                byte[] toSend = packet.GetRawPacket();
                Console.WriteLine("Sending: " + toSend.Length);
                int total = 0;
                int size = toSend.Length;
                int dataleft = size;
                int sent;
                while (total < size)
                {
                    sent = socket.Send(toSend, total, dataleft, SocketFlags.None);
                    total += sent;
                    dataleft -= sent;
                }
                socket.Close();
                if (_unreachableNeighbors.ContainsKey(destination.ToString()))
                {
                    _unreachableNeighbors.Remove(destination.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Thread.Sleep(50);
                SendPacket(packet, destination,tries);
             
            }
             
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateListeners();
        }
        

        private void UpdateListeners()
        {
            if (_portListeners[1].GetPortNumber() != _portFinder.GetCurrentPort())
            {
                ((PortListener)_portListeners[0]).Stop();
                _portListeners[0] = _portListeners[1];
                _portListeners[1] = _portListeners[2];
                _portListeners[2] = new PortListener(IPAddress.Any, _portFinder.GetNextPort(), _sorter);
                ((PortListener)_portListeners[2]).Start();
                StartListeners();
            }
        }

        private void StartListeners()
        {
            foreach (PortListener listener in _portListeners)
            {
                if(!listener.isStarted())
                    listener.Start();
            }
        }

        private void StopListeners()
        {
            foreach (PortListener listener in _portListeners)
            {
                if(listener!=null)
                    listener.Stop();
            }
        }
    }
}
