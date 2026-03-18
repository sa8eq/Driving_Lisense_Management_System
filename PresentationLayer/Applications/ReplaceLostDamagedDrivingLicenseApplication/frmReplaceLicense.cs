using BussinesLayer;
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
using static clsLicense;

namespace DVLD.Applications.ReplaceLostDamagedDrivingLicenseApplication
{
    public partial class frmReplaceLicense : Form
    {
        private int _NewLicense = -1;
        public frmReplaceLicense()
        {
            InitializeComponent();
        }

        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int _SelectedLicense = obj;
            linkShowLicensesHistory.Enabled = (_SelectedLicense != -1);
            lblOldLicenseID.Text = _SelectedLicense.ToString();

            if (_SelectedLicense==-1)
            {
                return;
            }
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;

            if (!ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("This License Is Inactive, You Can Issue a Replacement For It","Inactive License",MessageBoxButtons.OK);
                return;
            }
            if(ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsDetained)
            {
                MessageBox.Show("You Cant Issue A Replacement For A Detained License", "Detained License", MessageBoxButtons.OK);
                return;
            }
            if (ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.IsLicenseExpired())
            {
                MessageBox.Show("You Cant Issue A Replacement For An Expired License", "Expired License", MessageBoxButtons.OK);
                return;
            }
            btnIssueReplacement.Enabled = true;

        }

        private void frmReplaceLicense_Load(object sender, EventArgs e)
        {
            lblApplicationDate.Text = DateTime.Now.ToShortDateString();
            lblCreatedBy.Text = clsGlobal.CurrentUser.Username;
            rbDamaged.Checked = true;

        }

        private int _GetApplicationTypeID()
        {
            if(rbDamaged.Checked)
            {
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            }
            else
            {
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

            }
        }

        private void rbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            label1.Text = "Replacement For Damaged License";
            this.Text = label1.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID())._Fees.ToString();

            
        }

        private void rbLost_CheckedChanged(object sender, EventArgs e)
        {


            label1.Text = "Replacement For Lost License";
            this.Text = label1.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID())._Fees.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(_NewLicense);
            frm.ShowDialog();
        }

        private void linkShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();
        }

        private enIssueReason _GetIssueReason()
        {
            //this will decide which reason to issue a replacement for

            if (rbDamaged.Checked)

                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are You Sure You Want To Replace This License", "Confirm", MessageBoxButtons.YesNo,MessageBoxIcon.Question)!=DialogResult.Yes)
            {
                return;
            }


            clsLicense _ReplacmentLicense = ctrlDrivingLicenseInfoWithFilter1.SelectedLicenseInfo.Replace(_GetIssueReason(), clsGlobal.CurrentUser.UserID);
            if(_ReplacmentLicense==null)
            {
                MessageBox.Show("Issuing License Replacement Failed","Failed",MessageBoxButtons.OK);
                return;
            }
            lblReplacedLicenseID.Text = _ReplacmentLicense.LicenseID.ToString();
            lblLRApplicationID.Text = _ReplacmentLicense.ApplicationID.ToString();
            btnIssueReplacement.Enabled = false;
            linkShowLicenseInfo.Enabled = true;
            groupBox2.Enabled = false;
            ctrlDrivingLicenseInfoWithFilter1.FilterEnabled = false;
            MessageBox.Show("Issuing License Replacement Has Been Successfull", "Success", MessageBoxButtons.OK);
            
        }
    }
}
