using BusinessLayer;
using BussinesLayer;
using DVLD.Classes;
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

namespace DVLD.Applications.Application
{
    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {
        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;


        }

        private void _LoadData()
        {
            ctrlPersonDetailsWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalDrivingLicenseApplicationID);
            if (_LocalDrivingLicenseApplication==null)
            {
                MessageBox.Show("No Application With ID = " + _LocalDrivingLicenseApplicationID.ToString());
                this.Close();
                return;
            }


            ctrlPersonDetailsWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication._ApplicantPersonID);
            lblApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication._ApplicationDate);
            cbLicensesClasses.SelectedIndex = cbLicensesClasses.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblApplicationFees.Text = _LocalDrivingLicenseApplication._PaidFees.ToString();
            lblUsername.Text = clsUser.FindByUserID(_LocalDrivingLicenseApplication._UserID).Username.ToString();

        }
        private void _FillLicensesClassesInComboBox()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();

            foreach(DataRow row in dt.Rows)
            {
                cbLicensesClasses.Items.Add(row["ClassName"]);
            }
        }
        private void _ResetDefaultValues()
        {
            _FillLicensesClassesInComboBox();
            if(_Mode==enMode.AddNew)
            {
                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
                ctrlPersonDetailsWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = false;

                cbLicensesClasses.SelectedIndex = 2;
                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense)._Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblUsername.Text = clsGlobal.CurrentUser.Username;
            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;
            }
            
        }
        private void frmNewLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            cbLicensesClasses.ValueMember = "LicenseClassID";

            _ResetDefaultValues();
            if(_Mode==enMode.Update)
            {
                _LoadData();
            }
        }
        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonDetailsWithFilter1.FilterFocus();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int LicenseClassID = clsLicenseClass.Find(cbLicensesClasses.Text).LicenseClassID;
            int ActiveApplicationID = clsApplication.GetActiveApplicationIDForLicenseClass(_SelectedPersonID, clsApplication.enApplicationType.NewDrivingLicense, LicenseClassID);

            if (ActiveApplicationID != -1)
            {
                MessageBox.Show("Choose another License Class, the selected Person Already have an active application for the selected class with id=" + ActiveApplicationID, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbLicensesClasses.Focus();
                return;
            }
            if (clsLicense.IsLicenseExistByPersonID(ctrlPersonDetailsWithFilter1.PersonID, LicenseClassID))
            {

                MessageBox.Show("Person already have a license with the same applied driving class, Choose diffrent driving class", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _LocalDrivingLicenseApplication._ApplicantPersonID = ctrlPersonDetailsWithFilter1.PersonID; ;
            _LocalDrivingLicenseApplication._ApplicationDate = DateTime.Now;
            _LocalDrivingLicenseApplication._ApplicationTypeID = 1;
            _LocalDrivingLicenseApplication._Status = clsApplication.enStatus.New;
            _LocalDrivingLicenseApplication._LastStatusDate = DateTime.Now;
            _LocalDrivingLicenseApplication._PaidFees = Convert.ToSingle(lblApplicationFees.Text);
            _LocalDrivingLicenseApplication._UserID = clsGlobal.CurrentUser.UserID;
            _LocalDrivingLicenseApplication.LicenseClassID = LicenseClassID;

            if (_LocalDrivingLicenseApplication.Save())
            {
                lblApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
                _Mode = enMode.Update;
                lblTitle.Text = "Update Local Driving License Application";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            if (ctrlPersonDetailsWithFilter1.PersonID != -1)
            {

                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];

            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
