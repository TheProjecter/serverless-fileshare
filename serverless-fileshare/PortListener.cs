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
        private string _localIp;
        private int _port;
        private Boolean _keepListening;
        /// <summary>
        /// Create the port listener object
        /// </summary>
        /// <param name="ip">Local IP address</param>
        /// <param name="port">Port number to listen on</param>
        public PortListener(string ip, Int32 port)
        {
            int maxConnections=Properties.Settings.Default.MaxIncomingConnections;
            ThreadPool.SetMaxThreads(maxConnections, 
                maxConnections*2);
            ThreadPool.SetMinThreads(maxConnections, maxConnections);


            // Set the TcpListener IP & Port.
            IPAddress localAddr = IPAddress.Parse(ip);
            _listener = new TcpListener(localAddr, port);
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
                try
                {

                    if (!_listener.Pending())
                    {
                        Thread.Sleep(500); // choose a number (in milliseconds) that makes sense
                        continue; // skip to next iteration of loop
                    }

                    // Get client's request and process it for web request.
                    ProcessRequest();

                }
                catch (SocketException e)
                {
                    // Listener Error.

                }

                catch (InvalidOperationException er)
                {


                }
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
        public void ProcessRequest()
        {
            throw new Exception("PortListener.ProcessRequst() not implemented");
        }
    }

}
