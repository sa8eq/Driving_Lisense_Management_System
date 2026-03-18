using BusinessLayer;
using BussinesLayer;
using DVLD.Classes;
using DVLD.Classess;
using DVLD.Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.RenewDrivingLicenseApplication
{
    public partial class frmRenewDrivingLicense : Form
    {
        public frmRenewDrivingLicense()
        {
            InitializeComponent();
        }
        private int _SelectedLicenseID = -1;
        private int _NewLicenseID = -1;
        private void frmRenewDrivingLicense_Load(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter1.txtLicenseIDFocus();
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblIssueDate.Text = DateTime.Now.ToShortDateString();
            lblExpirationDate.Text = "[???]";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense)._Fees.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.Username;
        }

        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;
            lblOldLicenseID.Text = _SelectedLicenseID.ToString();

            linkShowLicensesHistory.Enabled = (_SelectedLicenseID!=-1);
            if (_SelectedLicenseID == -1)
                return;

            int DefaultValidityLength = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassIfo.DefaultValidityLength;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength)).ToString();
            lblLicenseFees.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClassIfo.ClassFees.ToString();
            lblTotalFees.Text = (Convert.ToInt32(lblApplicationFees.Text) + Convert.ToInt32(lblLicenseFees.Text)).ToString();
            txtNotes.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Notes;

            if (!ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("This License Is Not Expired, You Can Not Renew In The Mean Time","Error",MessageBoxButtons.OKCancel,MessageBoxIcon.Stop);
                return;
            }
            if(ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("This License Is Detained. Release It Frist To Renew It", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            if (!ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("This License Is Inactive, You Can Not Renew It", "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Stop);
                return;
            }
            btnRenew.Enabled = true;
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_NewLicenseID);
            frm.ShowDialog();
        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(clsLicense.Find(_SelectedLicenseID).DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want To Renew This License?","Confirm",MessageBoxButtons.YesNo,MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }
            clsLicense _RenewedLicense = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.RenewLicense(txtNotes.Text, clsGlobal.CurrentUser.UserID);
            if(_RenewedLicense==null)
            {
                return;
            }
            btnRenew.Enabled = false;
            linkShowLicenseInfo.Enabled = true;
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            
            lblRLApplicationID.Text = _RenewedLicense.ApplicationID.ToString();
            _NewLicenseID = _RenewedLicense.LicenseID;
            lblRenewedLicenseID.Text = _RenewedLicense.LicenseID.ToString();

            
            MessageBox.Show("Congratulation, License Has Been Renewed With New ID Of : " +_RenewedLicense.LicenseID.ToString(), "Renewed successfully", MessageBoxButtons.OKCancel);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
