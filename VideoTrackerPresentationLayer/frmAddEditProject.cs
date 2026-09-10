using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoTrackerBusinessLayer;

namespace VideoTrackerPresentationLayer
{
    public partial class frmAddEditProject : Form
    {

        enum enMode {AddNew = 0 , Update = 1};
        enMode _Mode = enMode.AddNew;
        int _ProjectID;
        clsVideoProject _clsProject;


        public frmAddEditProject(int ProjectID)
        {
            _ProjectID = ProjectID;
            if(_ProjectID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }
                InitializeComponent();
        }
        private void _FillStatusInComboBox()
        {
            cbStatus.Items.Clear();
            cbStatus.Items.Add("In Progress");
            cbStatus.Items.Add("Ready for Review");
            cbStatus.Items.Add("Delivered");
            cbStatus.Items.Add("Paid");
        }

        private void _FillClientsInComboBox()
        {
            DataTable dtClients = clsClient.GetAllClients();
            foreach(DataRow row in dtClients.Rows)
            {
                cbClient.Items.Add(row["ClientName"]);
            }
        }

        private void LoadData()
        {
            _FillStatusInComboBox();
            _FillClientsInComboBox();

            if (_Mode == enMode.AddNew)
            {
                lblProjectMode.Text = "Add New Project";
                _clsProject = new clsVideoProject();
                cbStatus.SelectedIndex = 0;
                cbClient.SelectedIndex = 0;
                return;
            }
            _clsProject = clsVideoProject.Find(_ProjectID);
            if(_clsProject == null)
            {
                MessageBox.Show("The Project is not Exist , this screen will be close!", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            lblProjectMode.Text = "Edit Project " + _ProjectID;
            lblProjectID.Text = _clsProject.ProjectID.ToString();
            txtTitle.Text = _clsProject.Title;
            txtDuration.Text = _clsProject.DurationInSecond.ToString();
            dtpDeliveryDate.Value = _clsProject.DeliveryDate;
            txtPrice.Text = _clsProject.Price.ToString();
            cbStatus.SelectedIndex = cbStatus.FindString(_clsProject.Status);


            clsClient Client = clsClient.Find(_clsProject.ClientID);
            if (Client != null)
            {
                cbClient.SelectedIndex = cbClient.FindString(Client.ClientName);
            }
                

        }

        private void cbClient_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmAddEditProject_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text.Trim()))
            {
                MessageBox.Show("Please enter a project title.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTitle.Focus();
                return;
            }

            if (!int.TryParse(txtDuration.Text.Trim(), out int duration))
            {
                MessageBox.Show("Duration must be a valid integer number (in seconds).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDuration.Focus();
                return;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), out decimal price))
            {
                MessageBox.Show("Price must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return;
            }

            clsClient Client1 = clsClient.Find(cbClient.Text);
            if(Client1 == null)
            {
                MessageBox.Show("Selected client does not exist in the database.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _clsProject.Title = txtTitle.Text;
            _clsProject.Price = price;
            _clsProject.DurationInSecond = duration;
            _clsProject.DeliveryDate = dtpDeliveryDate.Value;
            _clsProject.Status = cbStatus.Text;
            _clsProject.ClientID = Client1.ClientId;

            if (_clsProject.Save())
            {
                MessageBox.Show("Project Saved Successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lblProjectMode.Text = "Edit Project " + _clsProject.ProjectID;
                _Mode = enMode.Update;
                lblProjectID.Text = _clsProject.ProjectID.ToString();
            }
            else
            {
                MessageBox.Show("Failed to save the project.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

