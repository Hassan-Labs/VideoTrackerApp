using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoTrackerBusinessLayer;

namespace VideoTrackerPresentationLayer
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void _RefershMainList()
        {
            dgvMain.DataSource = clsVideoProject.GetAllProjects();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmManageClients frm = new frmManageClients();
            frm.ShowDialog();
            _RefershMainList();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            _RefershMainList();
        }

        private void btnAddProject_Click(object sender, EventArgs e)
        {
            frmAddEditProject frm = new frmAddEditProject(-1);
            frm.ShowDialog();
            _RefershMainList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddEditProject frm = new frmAddEditProject((int)dgvMain.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefershMainList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this Project ? " + (int)dgvMain.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (clsVideoProject.DeleteProject((int)dgvMain.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Project Has Been Deleted!");
                    _RefershMainList();
                }
                else
                    MessageBox.Show("Project Has NOT Been Deleted!");
            }
          
        }
    }
}
