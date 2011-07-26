namespace serverless_fileshare
{
    partial class Form1
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
            this.btnSearch = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.shareFIlesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.label1 = new System.Windows.Forms.Label();
            this.lblMyIP = new System.Windows.Forms.Label();
            this.lblNumNeighbors = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblNumShared = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.addNeighborToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnSearch
            // 
            this.btnSearch.Location = new System.Drawing.Point(87, 37);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(118, 23);
            this.btnSearch.TabIndex = 0;
            this.btnSearch.Text = "Search Neighbors";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(292, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.shareFIlesToolStripMenuItem,
            this.addNeighborToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // shareFIlesToolStripMenuItem
            // 
            this.shareFIlesToolStripMenuItem.Name = "shareFIlesToolStripMenuItem";
            this.shareFIlesToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.shareFIlesToolStripMenuItem.Text = "Share Files";
            this.shareFIlesToolStripMenuItem.Click += new System.EventHandler(this.shareFIlesToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(126, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(84, 78);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "My IP:";
            // 
            // lblMyIP
            // 
            this.lblMyIP.AutoSize = true;
            this.lblMyIP.Location = new System.Drawing.Point(127, 78);
            this.lblMyIP.Name = "lblMyIP";
            this.lblMyIP.Size = new System.Drawing.Size(64, 13);
            this.lblMyIP.TabIndex = 3;
            this.lblMyIP.Text = "172.21.10.1";
            // 
            // lblNumNeighbors
            // 
            this.lblNumNeighbors.AutoSize = true;
            this.lblNumNeighbors.Location = new System.Drawing.Point(127, 100);
            this.lblNumNeighbors.Name = "lblNumNeighbors";
            this.lblNumNeighbors.Size = new System.Drawing.Size(13, 13);
            this.lblNumNeighbors.TabIndex = 5;
            this.lblNumNeighbors.Text = "5";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 100);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "# of Neighbors:";
            // 
            // lblNumShared
            // 
            this.lblNumShared.AutoSize = true;
            this.lblNumShared.Location = new System.Drawing.Point(127, 123);
            this.lblNumShared.Name = "lblNumShared";
            this.lblNumShared.Size = new System.Drawing.Size(13, 13);
            this.lblNumShared.TabIndex = 7;
            this.lblNumShared.Text = "5";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 123);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "# of Shared Files:";
            // 
            // addNeighborToolStripMenuItem
            // 
            this.addNeighborToolStripMenuItem.Name = "addNeighborToolStripMenuItem";
            this.addNeighborToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.addNeighborToolStripMenuItem.Text = "Add Neighbor";
            this.addNeighborToolStripMenuItem.Click += new System.EventHandler(this.addNeighborToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 214);
            this.Controls.Add(this.lblNumShared);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lblNumNeighbors);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMyIP);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSearch);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Serverless-Fileshare";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fIleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem shareFIlesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblMyIP;
        private System.Windows.Forms.Label lblNumNeighbors;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblNumShared;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem addNeighborToolStripMenuItem;

    }
}

