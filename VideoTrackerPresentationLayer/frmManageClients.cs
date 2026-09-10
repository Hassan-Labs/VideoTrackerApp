using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoTrackerBusinessLayer;

namespace VideoTrackerPresentationLayer
{
    public partial class frmManageClients : Form
    {
        public frmManageClients()
        {
            InitializeComponent();
        }

       private void RefershClientsList()
        {
            dgvAllClients.DataSource = clsClient.GetAllClients();
        }

        private void frmManageClients_Load(object sender, EventArgs e)
        {
            RefershClientsList();
        }

        private void btnAddClient_Click(object sender, EventArgs e)
        {
            frmAddEditClient frm = new frmAddEditClient(-1);
            frm.ShowDialog();
            RefershClientsList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditClient frm = new frmAddEditClient((int)dgvAllClients.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            RefershClientsList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Client ? " + (int)dgvAllClients.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsClient.DeleteClient((int)dgvAllClients.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Client Has Been Deleted!");
                    RefershClientsList();
                }
                else
                    MessageBox.Show("Client Has NOT Been Deleted!");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
