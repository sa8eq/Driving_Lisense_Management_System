using BusinessLayer;
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

namespace DVLD.Tests.TakeTest
{
    public partial class frmTakeTest : Form
    {
        private int _TestAppointmentID;
        private int _LocalDrivingLicenseApplicationID;
        clsTestAppointment _TestAppointmentInfo;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplicationInfo;
        public frmTakeTest(int TestAppointmentID, int LocalDrivingLicenseApplicationID )
        {
            InitializeComponent();
            _TestAppointmentID = TestAppointmentID;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }
        private void _HandleTestType()
        {
            switch (_TestAppointmentInfo._TestTypeID)
            {
                case 1:
                    pictureBox1.Image = DVLD.Properties.Resources.Vision_512;
                    groupBox1.Text = "Vision Test";
                    break;
                case 2:
                    pictureBox1.Image = DVLD.Properties.Resources.Written_Test_512;
                    groupBox1.Text = "Written Test";
                    break;
                case 3:
                    pictureBox1.Image = DVLD.Properties.Resources.driving_test_512;
                    groupBox1.Text = "Street Test";
                    break;
            }
        }
        private void _FillInfo()
        {
            lblDLAppID.Text = _LocalDrivingLicenseApplicationInfo.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplicationInfo.ApplicantFullName;
            lblTrial.Text = clsLocalDrivingLicenseApplication.TotalTrialsPerTest(_LocalDrivingLicenseApplicationID, _TestAppointmentInfo._TestTypeID).ToString();
            lblDate.Text = _TestAppointmentInfo._AppointmentDate.ToString();
            lblFees.Text = _TestAppointmentInfo._PaidFees.ToString();
        
        }
        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            _TestAppointmentInfo = clsTestAppointment.FindByTestAppointmentID(_TestAppointmentID);
            if(_TestAppointmentInfo._IsLocked)
            {
                MessageBox.Show("This Appoitnemt Has Been Done Previuosly You Cant Take Aggain");
                this.Close();
            }
            _LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);
            _HandleTestType();
            _FillInfo();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to save. After saving you wount be able to change the Pass/Fail result of this test", "Confirm", MessageBoxButtons.YesNo)== DialogResult.No)
            {
                return;
            }
            clsTest Test = new clsTest();
            Test.TestAppointmentID = _TestAppointmentID;
            if(rbPass.Checked)
            {
                Test.TestResult = true;
            }
            else
            {
                Test.TestResult = false;
            }
            Test.Notes = txtNotes.Text;
            Test.CreatedByUserID = clsGlobal.CurrentUser.UserID;
            if (Test.Save())
            {
                _TestAppointmentInfo._IsLocked = true;
                _TestAppointmentInfo.Save();
                lblTestID.Text = Test.TestID.ToString();
                MessageBox.Show("Test Information Has Been Successfully Saved, And Test Appointment Is Locked");
                btnSave.Enabled = false;
                rbFail.Enabled = false;
                rbPass.Enabled = false;
                txtNotes.Enabled = false;
            }
            else
            {
                MessageBox.Show("Saving Test Information Failed");
            }
        }
    }
}
