using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Threading;
namespace serverless_fileshare
{
    public partial class AddFilesToShare : Form
    {

        MyFilesDB myFilesDb;
        String startPath;
        Boolean isShown=false;
        public AddFilesToShare(MyFilesDB db)
        {
            InitializeComponent();
            myFilesDb = db;
        }

        

        private void ThreadedHashDirectory()
        {
            addDirectory(startPath);
        }


        /// <summary>
        /// Recursively goes through all files in the directory 
        /// and adds them to the MyFilesDB
        /// </summary>
        /// <param name="directory">Current Directory To Search</param>
        private void addDirectory(String directory)
        {
            try
            {
                foreach (String file in Directory.GetFiles(directory))
                {
                    myFilesDb.AddFile(file);
                    DataGridViewRow row = new DataGridViewRow();
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = "Hashed: " + file;
                    row.Cells.Add(cell);
                    if (isShown && !gvLog.IsDisposed && !this.IsDisposed)
                    {
                        try
                        {
                            this.Invoke(new MethodInvoker(
                                    delegate()
                                    {
                                        gvLog.Rows.Insert(0, row);
                                    }));
                        }
                        catch { isShown = false; }
                    }

                }
            }
            catch { }

            foreach (String nextDir in Directory.GetDirectories(directory))
            {
                addDirectory(nextDir);
            }
        }



        #region EventHandlers

        private void btnAddFile_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                String path = folderBrowserDialog.SelectedPath;
                startPath = path;
                ThreadStart ts = new ThreadStart(ThreadedHashDirectory);
                Thread t = new Thread(ts);
                t.Start();
            }
        }


        private void AddFilesToShare_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            isShown = false;
            this.Hide();
        }

        private void AddFilesToShare_Shown(object sender, EventArgs e)
        {
            isShown = true;
        }
        #endregion

        
    }
}
