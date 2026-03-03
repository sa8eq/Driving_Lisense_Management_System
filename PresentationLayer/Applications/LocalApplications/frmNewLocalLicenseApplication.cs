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

namespace DVLD.Applications.Application
{
    public partial class frmNewLocalLicenseApplication : Form
    {

        public frmNewLocalLicenseApplication()
        {
            InitializeComponent();
            ctrlPersonDetailsWithFilter1.OnPersonSelected += MyCustomAction_OnPersonSelected;
            btnNext.Enabled = false;

        }
        public frmNewLocalLicenseApplication(int selectedNationalnumber)
        {
            InitializeComponent();
            ctrlPersonDetailsWithFilter1.LoadPersonInfo(selectedNationalnumber);
            ctrlPersonDetailsWithFilter1.OnPersonSelected += MyCustomAction_OnPersonSelected;
        }
        private int _PersonID = -1;
        private void MyCustomAction_OnPersonSelected(int PersonID)
        {
            _PersonID = PersonID;
            btnNext.Enabled = (_PersonID != -1);
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tc.SelectedIndex = 1;
            lblApplicationID.Text = "[???]";
            lblApplicationFees.Text = "15";
            lblApplicationDate.Text = DateTime.Now.ToString("dd/mm/yyyy");
            lblUsername.Text = clsGlobal.CurrentUser.Username;
        }

        private void frmNewLocalLicenseApplication_Load(object sender, EventArgs e)
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();
            cbLicensesClasses.DataSource = dt;
            cbLicensesClasses.DisplayMember = "ClassName";

            cbLicensesClasses.ValueMember = "LicenseClassID";

        }



        private void btnSave_Click(object sender, EventArgs e)
        {
            int SelectedClassID = Convert.ToInt32(cbLicensesClasses.SelectedValue);
            int AppTypeID = 1;

            if (clsLocalDrivingLicenseApplication.IsThereAnActiveApplication(_PersonID, AppTypeID, SelectedClassID))
            {
                MessageBox.Show("There is a previous Active Application", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; 
            }

            clsLocalDrivingLicenseApplication LocalApp = new clsLocalDrivingLicenseApplication();
            LocalApp._ApplicantPersonID = _PersonID;
            LocalApp.LicenseClassID = SelectedClassID;
            LocalApp._ApplicationDate = DateTime.Now;
            LocalApp._ApplicationTypeID = AppTypeID;
            LocalApp._Status = clsApplication.enStatus.New;
            LocalApp._LastStatusDate = DateTime.Now;
            LocalApp._PaidFees = 15;
            LocalApp._UserID = clsGlobal.CurrentUser.UserID;

            if (LocalApp.Save()) 
            {
                MessageBox.Show("Saved Successfully");
                btnSave.Enabled = false;
            }
            lblApplicationID.Text = LocalApp._ApplicationID.ToString();
        }


    }

}
