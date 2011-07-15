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

        //Classes
        MyFilesDB myFiles;
        MyNeighbors myNeighbors;
        MovingTCPScheduler scheduler;
        public Form1()
        {
            InitializeComponent();

            myFiles = new MyFilesDB();
            myNeighbors = new MyNeighbors();
            scheduler = new MovingTCPScheduler(myFiles);
            scheduler.Start();

            fileSearchForm = new FileSearchForm(myNeighbors, scheduler);


            myFiles.AddFile(@"C:\Tucker.jpg");
            myFiles.AddFile(@"C:\Party.jpg");
            myFiles.AddFile(@"C:\Test\from\300.avi");
            myNeighbors.AddNeighbor("172.16.1.100");
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fileSearchForm.ShowDialog();
        }

    }
}
