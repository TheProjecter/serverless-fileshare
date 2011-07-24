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
                    //file.ip = neighbor;
                    //fullResults.Add(file);
                    //DataGridViewRow row = new DataGridViewRow();

                    //DataGridViewCell namecell = new DataGridViewTextBoxCell();
                    //namecell.Value = file.FileLoc;
                    //row.Cells.Add(namecell);

                    //DataGridViewCell nebcell = new DataGridViewTextBoxCell();
                    //nebcell.Value = neighbor.ToString();
                    //row.Cells.Add(nebcell);

                    ////Threadsafe... Almost
                    //this.Invoke(new MethodInvoker(
                    //    delegate()
                    //        {
                    //            gvQueryResults.Rows.Add(row);
                    //        }));
        
                    
                }
            }
            //Threadsafe... Almost
            this.Invoke(new MethodInvoker(
                delegate()
                {
                    tvResults.Nodes.Add(tnNeighbor);
                }));
            
        }

        //private void gvQueryResults_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        //{
        //    //If it is a right click
        //    if (e.Button == MouseButtons.Right)
        //    {
        //        cmsTableRightclick.Show(System.Windows.Forms.Cursor.Position);
        //    }
        //}

        private void downloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //foreach (DataGridViewRow row in gvQueryResults.Rows)
            //{
            //    if (row.Selected)
            //    {
            //        MyFile file=(MyFile)fullResults[row.Index];
            //        String directory=Properties.Settings.Default.DownloadDirectory;
            //        fileTransferDB.AddPendingFile(new PendingFile(file.FileNumber, directory + file.FileName,file.ip.ToString()));
            //        outbound.SendFileDownloadRequest(file.FileNumber, file.ip);
            //    }
            //}
            
            System.Threading.ThreadStart ts = new System.Threading.ThreadStart(StartDownload);
            System.Threading.Thread thread = new System.Threading.Thread(ts);
            thread.Start();
            
        }

        private void StartDownload()
        {
            TreeNode tnDownload = tvResults.SelectedNode;
            DownloadFiles(tnDownload);
        }

        private void DownloadFiles(TreeNode tnDownload)
        {
            if (tnDownload.Nodes == null)
            {
                MyFile file = (MyFile)tnDownload.Tag;
                String directory = Properties.Settings.Default.DownloadDirectory;
                fileTransferDB.AddPendingFile(new PendingFile(file.FileNumber, directory + file.FileName, file.ip.ToString()));
                outbound.SendFileDownloadRequest(file.FileNumber, file.ip);
            }
            else
            {
                foreach (TreeNode tn in tnDownload.Nodes)
                {
                    DownloadFiles(tn);
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
