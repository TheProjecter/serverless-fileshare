using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;

namespace serverless_fileshare
{
    public partial class Form1 : Form
    {
        MovingTCPScheduler scheduler;
        OutboundManager outbound;

        MyNeighbors myNeighbors;
        public Form1()
        {
            InitializeComponent();
            MyFilesDB myFiles = new MyFilesDB();
            myFiles.AddFile(@"C:\Tucker.jpg");
            myFiles.AddFile(@"C:\Party.jpg");
            scheduler = new MovingTCPScheduler(myFiles);
            scheduler.Start();
            outbound = new OutboundManager(scheduler);
            myNeighbors = new MyNeighbors(scheduler);
            myNeighbors.AddNeighbor("172.16.1.100");
        }

        private void button1_Click(object sender, EventArgs e)
        {
                foreach (Neighbor nb in myNeighbors.GetListOfNeighbors())
                {
                    outbound.SendSearchRequest(tbSearch.Text,IPAddress.Parse(nb.IPAddress));
                }
               // outbound.SendFile(0, @"C:\Test\holiday.mp3", IPAddress.Parse("172.16.1.100"));
                
            
        }
    }
}
