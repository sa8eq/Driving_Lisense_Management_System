using BussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmDriverLicenseInfo : Form
    {
        private int _LocalAppID;
        clsLocalDrivingLicenseApplication _LocalApp;
        clsDriver _Driver;
        clsLicense _License;
        public frmDriverLicenseInfo(int LocalAppID)
        {
            InitializeComponent();
            _LocalAppID = LocalAppID;
        }
        private void _FillInfo()
        {
            lblClass.Text = _LocalApp.LicenseClassInfo.ClassName;
            lblName.Text = _LocalApp.ApplicantFullName;
            lblLicneseID.Text = _License.LicenseID.ToString();
            lblNationalnumber.Text = _Driver.Person.NationalNumber;
            if(_Driver.Person.Gender==0)
            {
                lblGender.Text = "Female";
                picGender.Image = DVLD.Properties.Resources.Woman_32;
                picPerson.Image = DVLD.Properties.Resources.Female_512;
            }
            else
            {
                lblGender.Text = "Male";
                picGender.Image = DVLD.Properties.Resources.Man_32;
                picPerson.Image = DVLD.Properties.Resources.Male_512;
            }
            lblIssueDate.Text = _License.IssueDate.ToShortDateString();
            lblIssueReason.Text = _License.IssueReason.ToString();
            lblNotes.Text = _License.Notes;
            if(_License.IsActive)
            {
                lblIsActive.Text = "Yes";
                lblIsDetained.Text = "No";
            }
            else
            {
                lblIsActive.Text = "No";
                lblIsDetained.Text = "Yes";
            }
            lblBirthDate.Text = _Driver.Person.BirthDate.ToShortDateString();
            lblDriverID.Text = _Driver.DriverID.ToString();
            lblEpirationDate.Text = _License.ExpirationDate.ToShortDateString();

            if(!string.IsNullOrEmpty(_Driver.Person.ImagePath))
            {
                picPerson.ImageLocation = _Driver.Person.ImagePath;
            }
           
            
        }
        private void frmDriverLicenseInfo_Load(object sender, EventArgs e)
        {
            _LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalAppID);
            _License = clsLicense.FindLicneseByApplicationID(_LocalApp._ApplicationID);
            _Driver = clsDriver.FindByDriverID(_License.DriverID);
            if (_LocalApp != null && _License != null && _Driver != null)
            {
                _FillInfo();
            }
            else
            {
                if(MessageBox.Show("There Is No Driver License Info","",MessageBoxButtons.OK)==DialogResult.OK)
                {
                    this.Close();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
