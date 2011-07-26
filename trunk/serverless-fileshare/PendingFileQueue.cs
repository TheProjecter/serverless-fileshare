using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.Threading;
using System.IO;
namespace serverless_fileshare
{
    /// <summary>
    /// Queues the packets when there is an error in saving the file
    /// </summary>
    public class PendingFileQueue
    {
        Queue _queue;
        Thread _td;
        int _fileID;
        String _fileLoc;
        /// <summary>
        /// Creates the pending file and enqueues the first object
        /// </summary>
        /// <param name="firstPacket">first errored packet</param>
        /// <param name="filesaver">the filesaver object used to save files</param>
        public PendingFileQueue(byte[] firstPacket,int fileID,String fileLoc)
        {
            _fileID = fileID;
            _fileLoc = fileLoc;
            _queue = new Queue();
            AddPacket(firstPacket);
            ThreadStart ts = new ThreadStart(CycleThrough);
            _td = new Thread(ts);
            _td.Name = "FileID:"+fileID;
            _td.Start();
        }

        public void AddPacket(byte[] packet)
        {
            _queue.Enqueue(packet);
        }

        private void CycleThrough()
        {
            while (true)
            {
                if (_queue.Count > 0)
                {
                    byte[] top = (byte[])_queue.Peek();
                    if (SavePacket(top))
                    {
                        _queue.Dequeue();
                    }
                    else
                    {
                        //wait 10ms before trying to save the next packet
                        Thread.Sleep(10);
                    }
                }
                else
                {
                    Thread.Sleep(5);
                }
            }
        }

        private Boolean SavePacket(byte[] data)
        {
            //try
            //{
                int fileId = BitConverter.ToInt32(data,0);
                int toSkip = BitConverter.GetBytes(fileId).Length;

                FileStream fs = new FileStream(_fileLoc, FileMode.Append);

                fs.Write(data, toSkip, data.Length-toSkip);
                fs.Close();
                return true;
            /*}
            catch (IOException ex)
            {
                throw ex;
                return false;
                
            }
             */
        }
    }
}
