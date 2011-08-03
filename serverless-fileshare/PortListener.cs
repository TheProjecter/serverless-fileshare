using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace serverless_fileshare
{
    class PortListener
    {
        private IPAddress _localIp;
        private int _port;
        private Boolean _keepListening;
        private PacketSorter _sorter;

        int packetsReceived = 0;
        /// <summary>
        /// Create the port listener object
        /// </summary>
        /// <param name="ip">Local IP address</param>
        /// <param name="port">Port number to listen on</param>
        public PortListener(IPAddress ip, Int32 port,PacketSorter sorter)
        {
            
            int maxConnections=Properties.Settings.Default.MaxIncomingConnections;
            ThreadPool.SetMaxThreads(maxConnections, 
                maxConnections*2);
            ThreadPool.SetMinThreads(maxConnections, maxConnections);
            _keepListening = false;
            // Set the TcpListener IP & Port.
            _localIp = ip;
            _port = port;
            _sorter = sorter;
        }

        public int GetPortNumber() { return _port; }

        public Boolean isStarted() { return _keepListening; }

        /// <summary>
        /// Starts the listener on a new thread
        /// </summary>
        public void Start()          // Run this on a separate thread
        {
            ThreadStart ts = new ThreadStart(threadedStart);
            Thread td = new Thread(ts);
            td.Start();

        }
        private void threadedStart()
        {
            try
            {
                TcpListener _listener = new TcpListener(_localIp, _port);
                _keepListening = true;
                _listener.Start(100000);

                Console.WriteLine("Starting server...\n");
                Console.WriteLine("Listening on {0}:{1}...", _localIp, _port);

                while (_keepListening)
                {

                    //blocks until a client has connected to the server
                    TcpClient client = _listener.AcceptTcpClient();

                    //create a thread to handle communication 
                    //with connected client
                    ProcessRequest(client);
                    //ThreadPool.QueueUserWorkItem(new WaitCallback(ProcessRequest), client);
                    //Thread clientThread = new Thread(new ParameterizedThreadStart(ProcessRequest));
                    //clientThread.Start(client);
                }

                _listener.Stop();
            }
            catch { }
        }
        public void Stop()
        {
            _keepListening = false;

        }


        /// <summary>
        /// Go though the listener and decied what the request is and begin/continue transfer if needed
        /// </summary>
        public void ProcessRequest(object client)
        {
            int msgSize = 1 + Properties.Settings.Default.PacketDataSize;
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();


            byte[] message = new byte[msgSize];
            int bytesRead=0;

            while (bytesRead<Properties.Settings.Default.PacketDataSize)
            {
                int bytesReadThisTime = 0;
                try
                {
                    //blocks until a client sends a message
                    bytesReadThisTime= clientStream.Read(message,bytesRead, msgSize-(bytesRead));
                    bytesRead += bytesReadThisTime;
                }
                catch(Exception ex)
                {
                    throw ex;
                    //a socket error has occured
                    break;
                }

                if (bytesReadThisTime == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

            }
            
            SFPacket packet = new SFPacket(message, bytesRead);
            String ip = tcpClient.Client.RemoteEndPoint.ToString();
            packet._sourceIP =IPAddress.Parse(ip.Substring(0,ip.IndexOf(':')));
            packetsReceived++;
            clientStream.Close();
            tcpClient.Close();

            _sorter.SortPacket(packet);

            Console.WriteLine("Received: " + bytesRead);
          
        }
    }

}
