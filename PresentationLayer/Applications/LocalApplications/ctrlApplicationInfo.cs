using BusinessLayer;
using BussinesLayer;
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

namespace DVLD.Test_Appointment.Controls
{
    public partial class ctrlApplicationInfo : UserControl
    {
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _LicenseID;
        public int LocalDrivingLicneseApplicationID
        {
            get { return _LocalDrivingLicenseApplicationID; }
        }
        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }
        private void _ResetLocalDrivingLicenseApplicationInfo()
        {
            _LocalDrivingLicenseApplicationID = -1;
            ctrlApplicationBasicInfo1.ResetApplicationInfo();
            lblLDLAppID.Text = "[????]";
            lblLicenseClass.Text = "[????]";
        }
        private void _FillLocalDrivingApplicationInfo()
        {
            //_LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();

            //incase there is license enable the show link.
            linkShowLicenseInfo.Enabled = (_LicenseID != -1);


            lblLDLAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblLicenseClass.Text = clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName;
            lblTestsPassed.Text = _LocalDrivingLicenseApplication.GetPassedTestsCount().ToString() + "/3";
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_LocalDrivingLicenseApplication._ApplicationID);
        }
        public void LoadApplicationInfoByLocalDrivingAppID(int LocalDrivingLicenseApplicationID)
        {
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(LocalDrivingLicenseApplicationID);
            
            if(_LocalDrivingLicenseApplication==null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();

                MessageBox.Show("No Application With This Application ID");
                return;
            }
            _FillLocalDrivingApplicationInfo();
        }

        public void LoadApplicationInfoByApplicationID(int ApplicationID)
        {
             _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByApplicationID(ApplicationID);
            if (_LocalDrivingLicenseApplication == null)
            {
                _ResetLocalDrivingLicenseApplicationInfo();


                MessageBox.Show("No Application with ApplicationID = " + _LocalDrivingLicenseApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _FillLocalDrivingApplicationInfo();
        }

        private void linkShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID);
            frm.ShowDialog();
        }
    }
}
