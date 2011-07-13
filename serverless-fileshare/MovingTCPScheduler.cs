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
        public OutboundManager outboundManager;
        public FileSearchForm fileSearchForm;

        System.Windows.Forms.Timer _portChangeClock = new System.Windows.Forms.Timer();

        public MovingTCPScheduler(MyFilesDB myFiles)
        {
            _portListeners = new PortListener[3];
            _portChangeClock.Interval = Properties.Settings.Default.PortChangeInterval;
            _portChangeClock.Tick += new EventHandler(timer_Tick);
            outboundManager = new OutboundManager(this);
            _sorter = new PacketSorter(myFiles,this);
        }

        public void Start()
        {
           // _portChangeClock.Start();
            UpdateListeners();
        }

        public void Stop()
        {
            _portChangeClock.Stop();
            StopListeners();
        }

        public void SendPacket(SFPacket packet, IPAddress destination)
        {
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
            }
            catch (Exception ex)
            {
                //TODO: Keep track of when to quit retrying send. Otherwise if  person goes offline this will run forever.
                Thread.Sleep(50);
                SendPacket(packet, destination);
            }
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateListeners();
        }
        

        private void UpdateListeners()
        {
            StopListeners();

            _portListeners[0] = new PortListener(IPAddress.Any, _portFinder.GetPreviousPort(),_sorter);
            _portListeners[1] = new PortListener(IPAddress.Any, _portFinder.GetCurrentPort(),_sorter);
            _portListeners[2] = new PortListener(IPAddress.Any, _portFinder.GetNextPort(),_sorter);

            StartListeners();
        }

        private void StartListeners()
        {
            foreach (PortListener listener in _portListeners)
            {
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
