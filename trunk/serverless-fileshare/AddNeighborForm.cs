using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace serverless_fileshare
{
    public partial class AddNeighborForm : Form
    {
        MyNeighbors myNeighbors;
        public AddNeighborForm(MyNeighbors mnb)
        {
            InitializeComponent();
            this.myNeighbors = mnb;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                myNeighbors.AddNeighbor(tbIP.Text);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invalid IP");
            }
        }
    }
}
