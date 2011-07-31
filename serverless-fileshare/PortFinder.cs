using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace serverless_fileshare
{
    /// <summary>
    /// Keeps track of the port changing algorithm. 
    /// </summary>
    class PortFinder
    {
        System.Windows.Forms.Timer timer;
        public PortFinder()
        {
            
        }

        private int GeneratePort(int day,int hour, int minute)
        {
            int minuteSection=minute / Properties.Settings.Default.PortChangeInterval+1;
            int port =Int32.Parse(day+""+hour+""+minuteSection);
            while(port.ToString().Length > 5)
                port =Int32.Parse(port.ToString().Substring(1, port.ToString().Length - 1));
            return port;
        }


        /// <summary>
        /// Finds the current port number though the algorithm and returns it
        /// </summary>
        /// <returns>Current port number</returns>
        public int GetCurrentPort()
        {
            return GeneratePort(DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute);
        }

        /// <summary>
        /// Finds the next port in the algorithm
        /// </summary>
        /// <returns>next port number to use</returns>
        public int GetNextPort()
        {
            DateTime nextPortDate = DateTime.Now.AddMinutes(Properties.Settings.Default.PortChangeInterval);
            return GeneratePort(nextPortDate.Day, nextPortDate.Hour, nextPortDate.Minute);
        }

        /// <summary>
        /// Finds the previous port in the algorithm
        /// </summary>
        /// <returns>previous port number</returns>
        public int GetPreviousPort()
        {
            DateTime nextPortDate = DateTime.Now.AddMinutes(-Properties.Settings.Default.PortChangeInterval);
            return GeneratePort(nextPortDate.Day, nextPortDate.Hour, nextPortDate.Minute);
        }

    }
}
