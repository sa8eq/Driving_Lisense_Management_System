using BussinesLayer;
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
    public partial class frmReleaseDetainedLicense : Form
    {
        private int _SelectedLicenseID = -1;
        public frmReleaseDetainedLicense()
        {
            InitializeComponent();
        }
        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();
            _SelectedLicenseID = LicenseID;

            ctrlDrivingLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;
            lblLicenseID.Text = _SelectedLicenseID.ToString();
            linklblShowLicensesHistory.Enabled = (_SelectedLicenseID!=-1);
            linklblShowLicneseInfo.Enabled = (_SelectedLicenseID!=-1);
            if(_SelectedLicenseID==-1)
            {
                return;
            }


            if (!ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("This License Is Not Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense)._Fees.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.Username;
            lblDetainID.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainID.ToString();
            lblLicenseID.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID.ToString();
            

            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            lblDetainDate.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.DetainDate.ToShortDateString();
            lblFineFees.Text = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DetainedInfo.FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle(lblApplicationFees.Text)+Convert.ToSingle(lblFineFees.Text)).ToString();

            btnRelease.Enabled = true;
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want to Release This License?","Confirm",MessageBoxButtons.YesNo, MessageBoxIcon.Question)==DialogResult.No)
            {
                return;
            }
            int ApplicationID = -1;
            bool IsReleased = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID);

            lblApplicationID.Text = ApplicationID.ToString();

            if (!IsReleased)
            {
                MessageBox.Show("Failed Releasing This License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = true;
                return;
                
            }

            MessageBox.Show("Congratulation, License Has Been Released", "Released", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            btnRelease.Enabled = false;
        }

        private void linklblShowLicneseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void linklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(clsLicense.Find(_SelectedLicenseID).DriverInfo.PersonID);
            frm.ShowDialog();
        }
    }
}
