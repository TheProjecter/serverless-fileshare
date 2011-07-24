namespace serverless_fileshare
{
    partial class FileSearchForm
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
            this.components = new System.ComponentModel.Container();
            this.tbQuery = new System.Windows.Forms.TextBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.gvQueryResults = new System.Windows.Forms.DataGridView();
            this.Filename = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Neighbor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsTableRightclick = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.gvQueryResults)).BeginInit();
            this.cmsTableRightclick.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbQuery
            // 
            this.tbQuery.Location = new System.Drawing.Point(12, 12);
            this.tbQuery.Name = "tbQuery";
            this.tbQuery.Size = new System.Drawing.Size(100, 20);
            this.tbQuery.TabIndex = 0;
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(129, 12);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 23);
            this.btnSearch.TabIndex = 1;
            this.btnSearch.Text = "Search";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // gvQueryResults
            // 
            this.gvQueryResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gvQueryResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gvQueryResults.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Filename,
            this.Neighbor});
            this.gvQueryResults.Location = new System.Drawing.Point(12, 41);
            this.gvQueryResults.Name = "gvQueryResults";
            this.gvQueryResults.ReadOnly = true;
            this.gvQueryResults.RowHeadersVisible = false;
            this.gvQueryResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gvQueryResults.Size = new System.Drawing.Size(500, 293);
            this.gvQueryResults.TabIndex = 2;
            this.gvQueryResults.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.gvQueryResults_CellMouseClick);
            // 
            // Filename
            // 
            this.Filename.HeaderText = "Filename";
            this.Filename.Name = "Filename";
            this.Filename.ReadOnly = true;
            // 
            // Neighbor
            // 
            this.Neighbor.HeaderText = "Neighbor";
            this.Neighbor.Name = "Neighbor";
            this.Neighbor.ReadOnly = true;
            // 
            // cmsTableRightclick
            // 
            this.cmsTableRightclick.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadToolStripMenuItem});
            this.cmsTableRightclick.Name = "cmsTableRightclick";
            this.cmsTableRightclick.ShowImageMargin = false;
            this.cmsTableRightclick.Size = new System.Drawing.Size(128, 48);
            // 
            // downloadToolStripMenuItem
            // 
            this.downloadToolStripMenuItem.Name = "downloadToolStripMenuItem";
            this.downloadToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
            this.downloadToolStripMenuItem.Text = "Download";
            this.downloadToolStripMenuItem.Click += new System.EventHandler(this.downloadToolStripMenuItem_Click);
            // 
            // FileSearchForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 346);
            this.Controls.Add(this.gvQueryResults);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.tbQuery);
            this.Name = "FileSearchForm";
            this.Text = "FileSearchForm";
            ((System.ComponentModel.ISupportInitialize)(this.gvQueryResults)).EndInit();
            this.cmsTableRightclick.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbQuery;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.DataGridView gvQueryResults;
        private System.Windows.Forms.DataGridViewTextBoxColumn Filename;
        private System.Windows.Forms.DataGridViewTextBoxColumn Neighbor;
        private System.Windows.Forms.ContextMenuStrip cmsTableRightclick;
        private System.Windows.Forms.ToolStripMenuItem downloadToolStripMenuItem;
    }
}