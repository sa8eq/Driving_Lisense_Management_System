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

namespace DVLD.Licenses
{
    public partial class frmIssueDriverLicenseForTheFirstTime : Form
    {
        private int _LocalDrivingLicneseApplicationID;
        public frmIssueDriverLicenseForTheFirstTime(int LocalDrivingLicneseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicneseApplicationID = LocalDrivingLicneseApplicationID;
        }

        private void frmIssueDriverLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            ctrlApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicneseApplicationID);
        }

        private void btmClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicneseApplicationID);
            clsDriver Driver = new clsDriver();

            Driver.PersonID = LocalApp._ApplicantPersonID;
            Driver.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            Driver.CreatedDate = DateTime.Now;

            if(Driver.Save())
            {
                clsLicense Licnese = new clsLicense();

                Licnese.ApplicationID = LocalApp._ApplicationID;
                Licnese.DriverID = Driver.DriverID;
                Licnese.LicenseClass = LocalApp.LicenseClassID;
                Licnese.IssueDate = DateTime.Now;
                Licnese.ExpirationDate = DateTime.Now.AddYears(LocalApp.LicenseClassInfo.DefaultValidityLength);
                Licnese.Notes = textBox1.Text;
                Licnese.PaidFees = LocalApp.LicenseClassInfo.ClassFees;
                Licnese.IsActive = true;
                Licnese.IssueReason = clsLicense.enIssueReason.FirstTime;
                Licnese.CreatedByUserID = clsGlobal.CurrentUser.UserID;
                if(Licnese.Save())
                {
                    LocalApp._Status = clsApplication.enStatus.Completed;
                    LocalApp.Save();
                    MessageBox.Show("Congratulation, a New Driving License Has Been Issued, You Are Now A Legal Driver","Congratulation");
                    btnIssue.Enabled = false;
                    textBox1.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Sorry, Issuing New Driving Licnese Has Failed");
                }

            }
            else
            {
                MessageBox.Show("Couldn't Generete a Driver row in database");
            }

        }
    }
}
