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
        private clsLocalDrivingLicenseApplication _LocalDrivingLicneseApplication;

        public frmIssueDriverLicenseForTheFirstTime(int LocalDrivingLicneseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicneseApplicationID = LocalDrivingLicneseApplicationID;
        }

        private void btmClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmIssueDriverLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {
            textBox1.Focus();
            _LocalDrivingLicneseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicneseApplicationID);

            if (_LocalDrivingLicneseApplication == null)
            {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicneseApplication.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }


            if (!_LocalDrivingLicneseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicneseApplication.GetActiveLicenseID();
            if (LicenseID != -1)
            {

                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }


            ctrlApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicneseApplicationID);
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = _LocalDrivingLicneseApplication.IssueLicenseForTheFirtTime(textBox1.Text.Trim(), clsGlobal.CurrentUser.UserID);

            if (LicenseID != -1)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            else
            {
                MessageBox.Show("License Was not Issued ! ",
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



        }
    }
}
