using BussinesLayer;
using DVLD.Classes;
using DVLD.Classess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.DetainLicenses
{
    public partial class frmDetainLicense : Form
    {
        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;
        public frmDetainLicense()
        {
            InitializeComponent();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("You Have To Enter A Find Amout To Proceed To Detain This License", "Mandatory Field", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            }


            if (MessageBox.Show("Are you sure you want to Detain This license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            _DetainID = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Detain(Convert.ToSingle(txtFees.Text), clsGlobal.CurrentUser.UserID);

            if (_DetainID == -1)
            {
                MessageBox.Show("Couldn't Detain This License With ID: " + _SelectedLicenseID.ToString(), "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                return;
            }
          
            
            MessageBox.Show("License With ID: " + _SelectedLicenseID.ToString() + " Has Been Detained Successfully", "Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);

            
            lblDetainID.Text = _DetainID.ToString();
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            txtFees.Enabled = false;
            btnDetain.Enabled = false;
        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByUser.Text = clsGlobal.CurrentUser.Username;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
             _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();
            linklnlShowLicenseHistory.Enabled = (_SelectedLicenseID!=-1);
            linklblShowLicenseInfo.Enabled = (_SelectedLicenseID!=-1);
            if (_SelectedLicenseID == -1)
            {
                return;
            }
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;

         
            if (ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {

                MessageBox.Show("You Cant Detain A License That Is Already Detaind. Release It First", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
            if (!ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {

                MessageBox.Show("This License Is Inactive, You Can Not Detain It", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }
            if (ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ExpirationDate < DateTime.Now)
            {

                MessageBox.Show("This License Is Expired, You Can Not Detain It", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                return;
            }


            btnDetain.Enabled = true;
            txtFees.Enabled = true;
            txtFees.Focus();

        }


        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void linklnlShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(clsLicense.Find(_SelectedLicenseID).DriverInfo.PersonID);
            frm.ShowDialog();
        }


        private void txtFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "You Have Enter The Fine Amount");
                
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void frmDetainLicense_Activated(object sender, EventArgs e)
        {

        }
    }
}
