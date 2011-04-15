using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;

namespace serverless_fileshare
{
    class MovingTCPScheduler
    {
        PortFinder _portFinder = new PortFinder();
        PortListener[] _portListeners;
        System.Windows.Forms.Timer _portChangeClock = new System.Windows.Forms.Timer();

        public MovingTCPScheduler()
        {
            _portListeners = new PortListener[3];
            _portChangeClock.Interval = Properties.Settings.Default.PortChangeInterval;
            _portChangeClock.Tick += new EventHandler(timer_Tick);
            
        }

        public void Start()
        {
            _portChangeClock.Start();
            UpdateListeners();
        }

        public void Stop()
        {
            _portChangeClock.Stop();
            StopListeners();
        }


        private void timer_Tick(object sender, EventArgs e)
        {
            UpdateListeners();
        }
        

        private void UpdateListeners()
        {
            StopListeners();

            _portListeners[0] = new PortListener(IPAddress.Any, _portFinder.GetPreviousPort());
            _portListeners[1] = new PortListener(IPAddress.Any, _portFinder.GetCurrentPort());
            _portListeners[2] = new PortListener(IPAddress.Any, _portFinder.GetNextPort());

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
                listener.Stop();
            }
        }
    }
}
