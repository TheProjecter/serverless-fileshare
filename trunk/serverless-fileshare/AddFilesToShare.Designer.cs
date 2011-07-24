namespace serverless_fileshare
{
    partial class AddFilesToShare
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAddFile = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.gvLog = new System.Windows.Forms.DataGridView();
            this.Log = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddFile
            // 
            this.btnAddFile.Location = new System.Drawing.Point(13, 13);
            this.btnAddFile.Name = "btnAddFile";
            this.btnAddFile.Size = new System.Drawing.Size(75, 23);
            this.btnAddFile.TabIndex = 0;
            this.btnAddFile.Text = "Add Files";
            this.btnAddFile.UseVisualStyleBackColor = true;
            this.btnAddFile.Click += new System.EventHandler(this.btnAddFile_Click);
            // 
            // gvLog
            // 
            this.gvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvLog.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Log});
            this.gvLog.Location = new System.Drawing.Point(12, 42);
            this.gvLog.Name = "gvLog";
            this.gvLog.RowHeadersVisible = false;
            this.gvLog.Size = new System.Drawing.Size(404, 275);
            this.gvLog.TabIndex = 1;
            // 
            // Log
            // 
            this.Log.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Log.HeaderText = "Log";
            this.Log.Name = "Log";
            // 
            // AddFilesToShare
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(428, 329);
            this.Controls.Add(this.gvLog);
            this.Controls.Add(this.btnAddFile);
            this.Name = "AddFilesToShare";
            this.Text = "AddFilesToShare";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddFilesToShare_FormClosing);
            this.Shown += new System.EventHandler(this.AddFilesToShare_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.gvLog)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddFile;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.DataGridView gvLog;
        private System.Windows.Forms.DataGridViewTextBoxColumn Log;
    }
}