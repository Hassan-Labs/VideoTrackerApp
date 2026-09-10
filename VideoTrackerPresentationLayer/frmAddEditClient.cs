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
    public partial class frmAddEditClient : Form
    {

        enum enMode { AddNew = 0 , Update = 1};
        enMode _Mode = enMode.AddNew;
        int _ClientID;
        clsClient _Client;

        public frmAddEditClient(int ClientID)
        {
            _ClientID = ClientID;
            if (_ClientID == -1)
            {
                _Mode = enMode.AddNew;
            }
            else
            {
                _Mode = enMode.Update;
            }

                InitializeComponent();
        }


        private void LoadData()
        {
            if(_Mode == enMode.AddNew)
            {
                lblClientMode.Text = "Add New Client";
                _Client = new clsClient();
                return;
            }
            _Client = clsClient.Find(_ClientID);
            if(_Client == null)
            {
                MessageBox.Show("The Client is not Exist , this screen will be close!", "Note", MessageBoxButtons.OK, MessageBoxIcon.Warning);                
                this.Close();
                return;

            }

            lblClientMode.Text = "Edit Mode " + _ClientID;
            lblClientID.Text = _Client.ClientId.ToString();
            txtClientName.Text = _Client.ClientName;
            txtPlatform.Text = _Client.Platform;
            txtContactInfo.Text = _Client.ContactInfo;
            
        }

        private void frmAddEditClient_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtClientName.Text.Trim()))
            {
                MessageBox.Show("Please enter a client name.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtClientName.Focus();
                return;
            }

           
            if (string.IsNullOrWhiteSpace(txtPlatform.Text.Trim()))
            {
                MessageBox.Show("Please enter a platform (e.g., YouTube, Upwork).", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPlatform.Focus();
                return;
            }
            _Client.ClientName = txtClientName.Text;
            _Client.Platform = txtPlatform.Text;
            _Client.ContactInfo = txtContactInfo.Text;
            
            if(_Client.Save())
            {
                MessageBox.Show("Client Saved Successfuly");
                _Mode = enMode.Update;
                lblClientMode.Text = "Edit" + _Client.ClientId.ToString();
                lblClientID.Text = _Client.ClientId.ToString();
            }
            else
                MessageBox.Show("Client Not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
    }

