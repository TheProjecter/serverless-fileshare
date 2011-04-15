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
        public static TcpListener _listener;
        private static TcpClient _client;
        private static NetworkStream _clientStream;
        private IPAddress _localIp;
        private int _port;
        private Boolean _keepListening;
        /// <summary>
        /// Create the port listener object
        /// </summary>
        /// <param name="ip">Local IP address</param>
        /// <param name="port">Port number to listen on</param>
        public PortListener(IPAddress ip, Int32 port)
        {
            int maxConnections=Properties.Settings.Default.MaxIncomingConnections;
            ThreadPool.SetMaxThreads(maxConnections, 
                maxConnections*2);
            ThreadPool.SetMinThreads(maxConnections, maxConnections);


            // Set the TcpListener IP & Port.
            _listener = new TcpListener(ip, port);
            _localIp = ip;
            _port = port;
        }

        /// <summary>
        /// Starts the listener on a new thread
        /// </summary>
        public void Start()          // Run this on a separate thread
        {                            
            _listener.Start();

            Console.WriteLine("Starting server...\n");
            Console.WriteLine("Listening on {0}:{1}...", _localIp, _port);

            while (_keepListening)
            {

                //blocks until a client has connected to the server
                TcpClient client = _listener.AcceptTcpClient();

                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(ProcessRequest));
                clientThread.Start(client);
            }

            _listener.Stop();

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
            TcpClient tcpClient = (TcpClient)client;
            NetworkStream clientStream = tcpClient.GetStream();

            //TODO: Define a chunk size... 4Kb packets isn't much...
            byte[] message = new byte[4096];
            int bytesRead;

            while (true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = clientStream.Read(message, 0, 4096);
                }
                catch
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received
                //TODO: Here we need to parse the filetransactionID from the message and then retrieve
                //      the rest of the data and write it out to the defined file.
                throw new Exception("Message Received: But parse code is incomplete");

                //ASCIIEncoding encoder = new ASCIIEncoding();
                //System.Diagnostics.Debug.WriteLine(encoder.GetString(message, 0, bytesRead));
            }

            tcpClient.Close();
        }
    }

}
