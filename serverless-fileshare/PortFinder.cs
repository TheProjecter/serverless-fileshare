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
        int currentPort;
        public PortFinder()
        {
            currentPort = 90;
            //TODO: Implement algorithm
        }

        /// <summary>
        /// Finds the current port number though the algorithm and returns it
        /// </summary>
        /// <returns>Current port number</returns>
        public int GetCurrentPort()
        {
            return currentPort;
        }

        /// <summary>
        /// Finds the next port in the algorithm
        /// </summary>
        /// <returns>next port number to use</returns>
        public int GetNextPort()
        {
            return currentPort;
        }

        /// <summary>
        /// Finds the previous port in the algorithm
        /// </summary>
        /// <returns>previous port number</returns>
        public int GetPreviousPort()
        {
            return currentPort;
        }

    }
}
