using BussinesLayer;
using DVLD.Classes;
using DVLD.Classess;
using DVLD.Licenses;
using DVLD.Licenses.International_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.International_Applications
{
    public partial class frmNewInternationalApplication : Form
    {
        private int _InternationalLicenseID = -1;
        public frmNewInternationalApplication()
        {
            InitializeComponent();
        }
        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int SelectedLicenseID = obj;
            lblLocalLicneseID.Text = SelectedLicenseID.ToString();
            linklblShowLicensesHistory.Enabled = (SelectedLicenseID != -1);

            if (SelectedLicenseID == -1)
            {
                return;
            }
            if (ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("To Issue International License Driver Should Have A Type 3 Local License", "Not Type 3 Local License", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            int ActiveInternaionalLicenseID = clsInternationalLicense.GetDriverActiveInternationalLicense(ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID);
            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already have an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linklblShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternaionalLicenseID;
                btnIssue.Enabled = false;
                return;
            }

            btnIssue.Enabled = true;

        }
        private void frmNewInternationalApplication_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));//add one year.
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense)._Fees.ToString();
            lblCreatedByUserName.Text = clsGlobal.CurrentUser.Username;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnIssue_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            clsInternationalLicense InternationalLicense = new clsInternationalLicense();

            InternationalLicense._ApplicantPersonID = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID;
            InternationalLicense._ApplicationDate = DateTime.Now;
            InternationalLicense._Status = clsApplication.enStatus.Completed;
            InternationalLicense._LastStatusDate = DateTime.Now;
            InternationalLicense._PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense)._Fees;
            InternationalLicense._UserID = clsGlobal.CurrentUser.UserID;


            InternationalLicense._DriverID = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverID;
            InternationalLicense._IssuedUsingLocalLicenseID = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.LicenseID;
            InternationalLicense._IssueDate = DateTime.Now;
            InternationalLicense._ExpirationDate = DateTime.Now.AddYears(1);

            InternationalLicense._UserID = clsGlobal.CurrentUser.UserID;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblInternationApplicationID.Text = InternationalLicense._ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense._InternationalLicenseID;
            lblInternationalLicenseID.Text = InternationalLicense._InternationalLicenseID.ToString();
            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense._InternationalLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssue.Enabled = false;
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            linklblShowLicenseInfo.Enabled = true;



        }
        private void linklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }
        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }
        private void frmNewInternationalApplication_Activated(object sender, EventArgs e)
        {
            ctrlDrivingLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
    }
}
