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
        //Views
        FileSearchForm fileSearchForm;
        AddFilesToShare filesToShareForm;

        //Classes
        MyFilesDB myFiles;
        MyNeighbors myNeighbors;
        MovingTCPScheduler scheduler;
        System.Windows.Forms.Timer refreshDataTimer;
        public Form1()
        {
            InitializeComponent();

            myFiles = new MyFilesDB();
            scheduler = new MovingTCPScheduler(myFiles);
            myNeighbors = new MyNeighbors(scheduler);
            filesToShareForm = new AddFilesToShare(myFiles);
            scheduler.Start();

            fileSearchForm = new FileSearchForm(myNeighbors, scheduler);

            refreshDataTimer = new System.Windows.Forms.Timer();
            refreshDataTimer.Interval = 10 * 1000;
            refreshDataTimer.Tick += new EventHandler(refreshDataTimer_Tick);
            refreshDataTimer.Start();
            refreshDataTimer_Tick(null, null);


            myFiles.AddFile(@"C:\Tucker.jpg");
            myFiles.AddFile(@"C:\Party.jpg");
            myFiles.AddFile(@"C:\Test\from\300.avi");
            myNeighbors.AddNeighbor("172.16.1.100");
        }

        void refreshDataTimer_Tick(object sender, EventArgs e)
        {
            string ipList = "";
            foreach(IPAddress ip in Dns.GetHostAddresses(""))
            {
                if (!ip.IsIPv6LinkLocal && !ip.IsIPv6Teredo)
                {
                    ipList += ", " + ip.ToString();
                }
            }
            lblMyIP.Text = ipList.Substring(2);
            lblNumNeighbors.Text = myNeighbors.GetListOfNeighbors().Count.ToString();
            lblNumShared.Text = myFiles.GetNumberOfFiles().ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fileSearchForm.ShowDialog();
        }

        private void shareFIlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filesToShareForm.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            myFiles.Save();
        }

    }
}
