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
        private int _SelectedLicenseID;
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

        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            if (SelectedLicenseID == -1)
                return;
            clsDetainedLicense dl = clsDetainedLicense.FindByLicenseID(SelectedLicenseID);

            if (!clsDetainedLicense.IsLicenseDetained(SelectedLicenseID))
            {
                MessageBox.Show("This License Is Not Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(dl==null)
            {
                MessageBox.Show("DL Is Null", "NULL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _SelectedLicenseID = SelectedLicenseID;

            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            lblDetainID.Text = dl.DetainID.ToString();
            lblDetainDate.Text = dl.DetainDate.ToShortDateString();
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicense)._Fees.ToString();
            lblLicenseID.Text = dl.LicenseID.ToString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.Username;
            lblFineFees.Text = dl.FineFees.ToString();
            lblTotalFees.Text = (dl.FineFees + int.Parse(lblApplicationFees.Text)).ToString();

            linklblShowLicensesHistory.Enabled = true;
            linklblShowLicneseInfo.Enabled = true;
            btnRelease.Enabled = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            clsLicense License = clsLicense.Find(_SelectedLicenseID);

            int ApplicationID = -1;

            if (License.ReleaseDetainedLicense(clsGlobal.CurrentUser.UserID, ref ApplicationID))
            {
                MessageBox.Show("Congratulation, License Has Been Released","Released",MessageBoxButtons.OK,MessageBoxIcon.Information);
                btnRelease.Enabled = false;
            }
            else
            { 
                MessageBox.Show("Failed Releasing This License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRelease.Enabled = true;
            }

            lblApplicationID.Text = ApplicationID.ToString();
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
