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


            if (myNeighbors.GetListOfNeighbors().Count == 0)
            {
                MessageBox.Show("Hello, you must be new here...\n\nYou must enter the IP address of someone already in the neighborhood to join.\n\nOnce you are connected go to File>Hash Files to begin sharing and search for filesfrom the main screen.");
                AddNeighborForm addnb = new AddNeighborForm(myNeighbors);
                addnb.ShowDialog();
            }
        }

        void refreshDataTimer_Tick(object sender, EventArgs e)
        {
            lblMyIP.Text = GetLocalIP();
            lblNumNeighbors.Text = myNeighbors.GetListOfNeighbors().Count.ToString();
            lblNumShared.Text = myFiles.GetNumberOfFiles().ToString();

            if (scheduler.fileTransferDB.GetPendingFileCount() != gvCurrentDownloads.Rows.Count)
            {
                gvCurrentDownloads.Rows.Clear();
                foreach (PendingFile pf in scheduler.fileTransferDB.GetPendingFileList())
                {
                    if (pf.lastPacketReceived.AddMinutes(15) < DateTime.Now)
                        scheduler.outboundManager.SendFileDownloadRequest(pf.id,IPAddress.Parse(pf.Source));
                    DataGridViewRow row = new DataGridViewRow();
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = pf.fileLocation;
                    row.Cells.Add(cell);

                    cell = new DataGridViewTextBoxCell();
                    cell.Value = pf.Source;
                    row.Cells.Add(cell);

                    gvCurrentDownloads.Rows.Add(row);

                }
            }
        }

        private String GetLocalIP()
        {
            IPHostEntry host;
            string localIP = "?";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily.ToString() == "InterNetwork")
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }

        private void StopEverything()
        {
            myFiles.Save();
            scheduler.Stop();
        }

        #region Event Handlers

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fileSearchForm.Show();
        }

        private void shareFIlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            filesToShareForm.Show();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StopEverything();
            Application.Exit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopEverything();
            Application.Exit();
        }

        

        private void addNeighborToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNeighborForm addnb = new AddNeighborForm(myNeighbors);
            addnb.ShowDialog();
        }

        private void btnViewCompleted_Click(object sender, EventArgs e)
        {

        }


        #endregion

        
    }
}
