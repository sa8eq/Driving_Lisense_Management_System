using BussinesLayer;
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
        private clsApplication _Application;
        private clsLicense _License;
        private int _LicenseID;
        private int _ILicenseID = -1;
        public frmNewInternationalApplication()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool _ValidateConstraints()
        { 
            if(!_License.IsActive)
            {
                MessageBox.Show("To Issue New International Driving License Local License Should Active", "Inactive Local License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //if(_License.IsDetained)
            //{
            //    MessageBox.Show("Local Driver License Is Detained, Release it to Issue International License", "Detained Local License", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    return false;
            //}
            if (_License.ExpirationDate < DateTime.Now)
            {
                MessageBox.Show("Expired Local License, Renew It First To Issue International License", "Expired Local License", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
                return false;
            }
            if(_License.LicenseClassIfo.LicenseClassID != 3)
            {
                MessageBox.Show("To Issue International License Driver Should Have A Type 3 Local License", "Not Type 3 Local License", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            if(clsInternationalLicense.IsDriverHasActiveInternationalLicense(_License.DriverID))
            {
                _ILicenseID = clsInternationalLicense.FindByLocalLicenseID(_LicenseID)._InternationalLicenseID;
                MessageBox.Show("The Driver Already Has An International Driving License", "International License Exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                linklblShowLicensesHistory.Enabled = true;
                linklblShowLicenseInfo.Enabled = true;
                return false;
            }
            linklblShowLicensesHistory.Enabled = true;
            btnIssue.Enabled = true;
            return true;
        }

        private void _FillInternationalLicenseApplicationInfo()
        {
            if (_ValidateConstraints())
            {
                

                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblIssueDate.Text = DateTime.Now.ToShortDateString();
                clsApplicationType apptype = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense);
                lblFees.Text = apptype._Fees.ToString();
                lblLocalLicneseID.Text = ctrlDrivingLicenseInfoWithFilter1.LicenseID.ToString();
                lblExpirationDate.Text = DateTime.Now.AddYears(1).ToShortDateString();
                lblCreatedByUserName.Text = clsGlobal.CurrentUser.Username;
            }
        }
        private void ctrlDrivingLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _LicenseID = obj;
            _License = clsLicense.Find(_LicenseID);
            if(_License==null)
            {
               return;
            }
            _FillInternationalLicenseApplicationInfo();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsInternationalLicense ILicense = new clsInternationalLicense();

            ILicense._DriverID = _License.DriverID;
            ILicense._IssuedUsingLocalLicenseID = _LicenseID;
            ILicense._IssueDate = Convert.ToDateTime(lblIssueDate.Text);
            ILicense._ExpirationDate = Convert.ToDateTime(lblIssueDate.Text).AddYears(1);
            ILicense._IsActive = true;
            ILicense._CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if(ILicense.Save())
            {
                MessageBox.Show("Congratulation, International Driving License Has Been Issued Successfully","Saved Successfully",MessageBoxButtons.OK,MessageBoxIcon.Information);
                _ILicenseID = ILicense._InternationalLicenseID;
                linklblShowLicenseInfo.Enabled = true;
                lblInternationApplicationID.Text = ILicense._ApplicationID.ToString();
                lblInternationalLicenseID.Text = _ILicenseID.ToString();
            }
            else
            {
                MessageBox.Show("Error: Issuing International Driving License Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            
        }

        private void linklblShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(_License.DriverInfo.Person.PersonID);
            frm.ShowDialog();
        }

        private void linklblShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_ILicenseID);
            frm.ShowDialog();
        }
    }
}
