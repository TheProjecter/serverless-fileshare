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
        public Form1()
        {
            InitializeComponent();
            MyFilesDB myFiles = new MyFilesDB();
            myFiles.AddFile(@"E:\Cales stuff\pics\me\jamaica.jpg");
            scheduler = new MovingTCPScheduler(myFiles);
            //outbound = new OutboundManager(scheduler);
            MyNeighbors myNeighbors = new MyNeighbors(scheduler);
            scheduler.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
               // outbound.SendFile(0, @"C:\Test\holiday.mp3", IPAddress.Parse("172.16.1.100"));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
