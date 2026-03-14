using BussinesLayer;
using DVLD.Classess;
using DVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static BussinesLayer.clsTestTypes;

namespace DVLD.Tests.Schedual_Test.Control
{
    public partial class ctrlScheduleTest : UserControl
    {
        public ctrlScheduleTest()
        {
            InitializeComponent();
        }
        
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode = enMode.AddNew;

        public enum enCreationMode { FirstTimeSchedule = 0, RetakeTestSchedule = 1 };
        private enCreationMode _CreationMode = enCreationMode.FirstTimeSchedule;

        private clsTestTypes.enTestType _TestTypeID = clsTestTypes.enTestType.VisionTest;
        
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        
        private int _LocalDrivingLicenseApplicationID = -1;
        
        private clsTestAppointment _TestAppointment;
        
        private int _TestAppointmentID = -1;

        public clsTestTypes.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value;

                switch (_TestTypeID)
                {

                    case clsTestTypes.enTestType.VisionTest:
                        {
                            groupBox2.Text = "Vision Test";
                            pictureBox1.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestTypes.enTestType.WrittenTest:
                        {
                            groupBox2.Text = "Written Test";
                            pictureBox1.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestTypes.enTestType.StreetTest:
                        {
                            groupBox2.Text = "Street Test";
                            pictureBox1.Image = Resources.driving_test_512;
                            break;


                        }
                }
            }
        }

        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if(_TestAppointment==null)
            {
                MessageBox.Show("Error: No Appointment With This ID Of " + _TestAppointmentID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

            lblFees.Text = _TestAppointment._PaidFees.ToString();
            if(DateTime.Compare(DateTime.Now, _TestAppointment._AppointmentDate)<0)
            {
                dateTimePicker1.MinDate = DateTime.Now;
            }
            else
            {
                dateTimePicker1.MinDate = _TestAppointment._AppointmentDate;
            }

            dateTimePicker1.Value = _TestAppointment._AppointmentDate;

            if(_TestAppointment._RetakeTestApplicationID==-1)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblRetakeAppFees.Text = _TestAppointment._RetakeApplication._PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = _TestAppointment._RetakeTestApplicationID.ToString();
            }
            return true;
        }

        private bool _HandleActiveTestAppointmentConstraint()
        {
            if(_Mode == enMode.AddNew && _LocalDrivingLicenseApplication.IsThereAnActiveScheduledTest((int)_TestTypeID))
            {
                lblUserMessage.Text = "Person Already Has An Active Appointment For This Test Type";
                btnSave.Enabled = false;
                dateTimePicker1.Enabled = false;
                return false;
            }
            return true;
        }

        private bool _HandleAppointmentLockedConstraint()
        {
            if(_TestAppointment._IsLocked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person Already Sat for the test, Appointment Locked";
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                return false;
            }
            else
            {
                lblUserMessage.Visible = false;
            }
            return true;
        }

        private bool _HandlePreviousTestConstraint()
        {
            switch(TestTypeID)
            {
                case clsTestTypes.enTestType.VisionTest:
                    lblUserMessage.Visible = false;
                    return true;
                case clsTestTypes.enTestType.WrittenTest:
                    if(!_LocalDrivingLicenseApplication.DoesPassTestType((int)clsTestTypes.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Schedule, Vision Test Must Be Passed First";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;
                    }
                    return true;
                case clsTestTypes.enTestType.StreetTest:
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType((int)clsTestTypes.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Schedule, Written Test Must Be Passed First";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dateTimePicker1.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dateTimePicker1.Enabled = true;
                    }
                    return true;
            }
            return false;
        }

        public void LoadInfo(int LocalDrivingLicneseApplicationID, int AppointmentID=-1)
        {
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicneseApplicationID;
            _TestAppointmentID = AppointmentID;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.
                               FindByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);

            if(_LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("Error: No Local Driving Licnese Application With ID Of " + _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

            if(_LocalDrivingLicenseApplication.DoesAttendTestType((int)_TestTypeID))
            {
                _CreationMode = enCreationMode.RetakeTestSchedule;
            }
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;

            if(_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest)._Fees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = "0";
            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblName.Text = _LocalDrivingLicenseApplication.ApplicantFullName;

            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest((int)_TestTypeID).ToString();

            if(_Mode==enMode.AddNew)
            {
                lblFees.Text = clsTestTypes.GetTestByID(_TestTypeID)._Fees.ToString();
                dateTimePicker1.MinDate = DateTime.Now;
                lblRetakeTestAppID.Text = "N/A";

                _TestAppointment = new clsTestAppointment();
            }
            else
            {
                if(!_LoadTestAppointmentData())
                {
                    return;
                }
            }

            lblTotalFees.Text = (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();

            if(!_HandleActiveTestAppointmentConstraint())
            {
                return;
            }
            if(!_HandleAppointmentLockedConstraint())
            {
                return;
            }
            if(!_HandlePreviousTestConstraint())
            {
                return;
            }
        }
        
        private bool HandleRetakeApplication()
        {
            if(_Mode == enMode.AddNew && _CreationMode == enCreationMode.RetakeTestSchedule)
            {
                clsApplication App = new clsApplication();
                App._ApplicantPersonID = _LocalDrivingLicenseApplication._ApplicantPersonID;
                App._ApplicationDate = DateTime.Now;
                App._ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                App._Status = clsApplication.enStatus.Completed;
                App._LastStatusDate = DateTime.Now;
                App._PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest)._Fees;
                App._UserID = clsGlobal.CurrentUser.UserID;

                if(!App.Save())
                {
                    _TestAppointment._RetakeTestApplicationID = -1;
                    MessageBox.Show("Failed To Create Application", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                _TestAppointment._RetakeTestApplicationID = App._ApplicationID;
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!HandleRetakeApplication())
                return;

            _TestAppointment._TestTypeID = _TestTypeID;
            _TestAppointment._LocalDrivingLicneseApplicationID = _LocalDrivingLicenseApplicationID;
            _TestAppointment._AppointmentDate = dateTimePicker1.Value;
            _TestAppointment._PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment._CreatedByUserID = clsGlobal.CurrentUser.UserID;

            if (_TestAppointment.Save())
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully","Saved",MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Error: Data Is not Saved Successfully", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            btnSave.Enabled = false;
        }
    }
}
