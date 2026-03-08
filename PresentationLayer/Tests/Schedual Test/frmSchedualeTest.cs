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
using static BussinesLayer.clsApplication;
using static BussinesLayer.clsTestTypes;

namespace DVLD.Tests.Schedual_Test
{
    public partial class frmSchedualeTest : Form
    {
        private int _LocalDrivingLicneseApplicationID = -1;
        clsTestAppointment _TestAppointment;
        clsTestTypes _TestTypeInfo;
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication;
        clsApplication RetakeApplication;
        public frmSchedualeTest(int LocalDrivingLicenseApplicationID, clsTestTypes TestTypeInfo)
        {
            InitializeComponent();
            _LocalDrivingLicneseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeInfo = TestTypeInfo;
        }
        public frmSchedualeTest(int TestAppointmentID)
        {

            InitializeComponent();
            _TestAppointment = clsTestAppointment.FindByTestAppointmentID(TestAppointmentID);
            _LocalDrivingLicneseApplicationID = _TestAppointment._LocalDrivingLicneseApplicationID;

            LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicneseApplicationID);
            _TestTypeInfo = clsTestTypes.GetTestByID((clsTestTypes.enTestType)_TestAppointment._TestTypeID);

        }
        private void FillInfo()
        {
            lblDLAppID.Text = LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = LocalDrivingLicenseApplication.ApplicantFullName;
            int TestTrials = clsLocalDrivingLicenseApplication.TotalTrialsPerTest(_LocalDrivingLicneseApplicationID, (int)_TestTypeInfo._ID);
            lblTrial.Text = TestTrials.ToString();
            dateTimePicker1.MinDate = DateTime.Now;
            lblFees.Text = _TestTypeInfo._Fees.ToString();

            switch (_TestTypeInfo._ID)
            {
                case clsTestTypes.enTestType.VisionTest:
                    {
                        pictureBox1.Image = DVLD.Properties.Resources.Vision_512;
                    }
                    break;
                case clsTestTypes.enTestType.WrittenTest:
                    {
                        pictureBox1.Image = DVLD.Properties.Resources.Written_Test_512;
                    }
                    break;
                case clsTestTypes.enTestType.StreetTest:
                    {
                        pictureBox1.Image = DVLD.Properties.Resources.driving_test_512;
                    }
                    break;
            }

            groupBox1.Enabled = (TestTrials > 0);

            if(groupBox1.Enabled)
            {
                
                RetakeApplication = clsApplication.FindBaseApplication(LocalDrivingLicenseApplication._ApplicationID);
                lblRTestFees.Text = RetakeApplication._ApplicationTypeInfo._Fees.ToString();
                lblTotalFees.Text = (RetakeApplication._ApplicationTypeInfo._Fees + _TestTypeInfo._Fees).ToString();
            }
        }
        private void frmSchedualeTest_Load(object sender, EventArgs e)
        {
            LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicneseApplicationID);
            FillInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int RetakeAppID = -1;

            if (groupBox1.Enabled)
            {
                clsApplication RetakeApp = new clsApplication();
                RetakeApp._ApplicantPersonID = LocalDrivingLicenseApplication._ApplicantPersonID;
                RetakeApp._ApplicationDate = DateTime.Now;
                RetakeApp._ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                RetakeApp._Status = clsApplication.enStatus.New;
                RetakeApp._LastStatusDate = DateTime.Now;
                RetakeApp._Mode = clsApplication.enMode.AddNew;
                RetakeApp._PaidFees = (float)clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest)._Fees;
                RetakeApp._UserID = clsGlobal.CurrentUser.UserID;

                if (!RetakeApp.Save())
                {
                    MessageBox.Show("Error: Couldnt Save The Retake Application");
                    return;
                }
                RetakeAppID = RetakeApp._ApplicationID;
            }

            clsTestAppointment TestAppointment;

            if (_TestAppointment != null)
            {
                TestAppointment = _TestAppointment;
            }
            else
            {
                TestAppointment = new clsTestAppointment();
                TestAppointment._TestTypeID = (int)_TestTypeInfo._ID;
                TestAppointment._LocalDrivingLicneseApplicationID = LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
                TestAppointment._CreatedByUserID = clsGlobal.CurrentUser.UserID;
                TestAppointment._IsLocked = false;
                TestAppointment._RetakeTestApplicationID = RetakeAppID;
            }

            TestAppointment._AppointmentDate = dateTimePicker1.Value;
            TestAppointment._PaidFees = (RetakeAppID != -1)
                ? _TestTypeInfo._Fees + clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest)._Fees
                : _TestTypeInfo._Fees;

            if (TestAppointment.Save())
            {
                MessageBox.Show("Test Appointment Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnSave.Enabled = false;
            }
            else
            {
                MessageBox.Show("Failed: Saving Test Appointment", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
