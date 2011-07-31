using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Collections;
using System.IO;
using System.Threading;

namespace serverless_fileshare
{
    public partial class FileSearchForm : Form
    {
        MyNeighbors myNeighbors;
        OutboundManager outbound;
        ArrayList fullResults;
        PendingFileTransferDB fileTransferDB;
        public FileSearchForm(MyNeighbors mn,MovingTCPScheduler scheduler)
        {
            InitializeComponent();
            myNeighbors=mn;
            outbound = scheduler.outboundManager;
            scheduler.fileSearchForm = this;
            fullResults = new ArrayList();
            fileTransferDB = scheduler.fileTransferDB;
            System.Threading.ThreadPool.SetMaxThreads(5, 5);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            tvResults.Nodes.Clear();
            System.Threading.ThreadStart ts = new System.Threading.ThreadStart(Search);
            System.Threading.Thread thread = new System.Threading.Thread(ts);
            thread.Start();
        }

        private void Search()
        {

            foreach (Neighbor nb in myNeighbors.GetListOfNeighbors())
            {
                outbound.SendSearchRequest(tbQuery.Text, IPAddress.Parse(nb.IPAddress));
            }
        }

        public void AddResults(ArrayList results,IPAddress neighbor)
        {
            fullResults.Clear();
            TreeNode tnNeighbor = new TreeNode(neighbor.ToString());
            TreeNode tnParent;
            tnParent = tnNeighbor;
            foreach (FileHash fileHash in results)
            {
                foreach (MyFile file in fileHash.FileList)
                {
                    file.ip = neighbor;
                    string[] folders = file.FileLoc.Split('\\');
                    for (int i = 0; i < folders.Count(); i++)
                    {
                        bool finished = false;
                        tnParent = tnNeighbor;
                        for (int j = 0; j < i && !finished; j++)
                        {
                            bool matchFound = false;
                            foreach (TreeNode tnChild in tnParent.Nodes)
                            {
                                if(tnChild.Text == folders[j])
                                {
                                    tnParent = tnChild;
                                    matchFound = true;   
                                }
                            }
                            if (!matchFound)
                            {
                                finished = true;
                            }
                        }
                        TreeNode directory = new TreeNode(folders[i]);
                        directory.Tag = file;
                        bool alreadyAdded = false;
                        foreach (TreeNode tn in tnParent.Nodes)
                        {
                            if (tn.Text == directory.Text)
                            {
                                alreadyAdded = true;
                                break;
                            }
                        }
                        if (!alreadyAdded)
                        {
                            tnParent.Nodes.Add(directory);
                        }
                    }

                }
            }
            try
            {
                //Threadsafe... Almost
                this.Invoke(new MethodInvoker(
                    delegate()
                    {
                        tvResults.Nodes.Add(tnNeighbor);
                        TreeNode node = tnNeighbor;
                        while (node.Nodes.Count == 1)
                        {
                            node.Expand();
                            if (node.NextNode != null)
                                node = node.NextNode;
                            else
                                node = node.Nodes[0];
                        }

                        foreach (TreeNode child in tvResults.Nodes)
                        {
                            if (child.Nodes.Count == 0)
                                child.Remove();
                        }
                    }));
            }
            catch { }

            
            
        }

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            
            System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(StartDownload), tvResults.SelectedNode);
            //StartDownload(tvResults.SelectedNode);
        }

        private void StartDownload(object parameter)
        {
            TreeNode tnDownload = (TreeNode)parameter;
            String folder="";
            DownloadFiles(tnDownload,folder);
        }

        /// <summary>
        /// Downloads the given tree node (and everything in it)
        /// </summary>
        /// <param name="tnDownload">Treenode to parse and download</param>
        /// <param name="builtDirectory">Built path </param>
        private void DownloadFiles(TreeNode tnDownload,String builtDirectory)
        {
            if (tnDownload.Nodes.Count==0)
            {
                MyFile file = (MyFile)tnDownload.Tag;
                String directory = Properties.Settings.Default.DownloadDirectory;
                String savingDir = directory + builtDirectory;
                if (!Directory.Exists(savingDir))
                    Directory.CreateDirectory(savingDir);
                fileTransferDB.AddPendingFile(new PendingFile(file.FileNumber,savingDir+  file.FileName, file.ip.ToString()));
                outbound.SendFileDownloadRequest(file.FileNumber, file.ip);
            }
            else
            {
                foreach (TreeNode tn in tnDownload.Nodes)
                {
                    
                    DownloadFiles(tn,builtDirectory +tnDownload.Text+"\\");
                }
            }
        }

        private void tvResults_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                tvResults.SelectedNode = e.Node;
                cmsTableRightclick.Show(System.Windows.Forms.Cursor.Position);
            }
        }

    }
}
