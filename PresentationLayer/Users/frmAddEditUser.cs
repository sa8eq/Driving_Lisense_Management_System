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

namespace DVLD.Persons
{
    public partial class frmAddEditUser : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        private enMode Mode;
        private int _UserID = -1;
        clsUser _User;

        public frmAddEditUser()
        {
            InitializeComponent();
        }
        public frmAddEditUser(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            Mode = (_UserID == -1) ? enMode.AddNew : enMode.Update;
        }
        
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnNext_Click(object sender, EventArgs e)
        {
            if(Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tabPage2.Enabled = true;
                tabControl1.SelectedTab = tabControl1.TabPages["tabPage2"];
            }
            if (ctrlPersonDetailsWithFilter1.PersonID != -1)
            {
                if (clsUser.IsUserExistForPersonID(ctrlPersonDetailsWithFilter1.PersonID))
                {
                    MessageBox.Show("Selected Person already has a User Account");
                    ctrlPersonDetailsWithFilter1.FilterFocus();
                }
                else
                {
                    btnSave.Enabled = true;
                    tabPage2.Enabled = true;
                    tabControl1.SelectedTab = tabControl1.TabPages["tabPage2"];
                }
            }
            else
            {
                MessageBox.Show("Please select a Person", "Select a Person");
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some fields are missing or invalid!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _User.Username = txtUsername.Text.Trim();
            _User.Password = txtPassword.Text.Trim();
            _User.PersonID = ctrlPersonDetailsWithFilter1.PersonID;
            _User.IsActive = cbxIsActive.Checked;

            if (_User.Save())
            {
                lblUserID.Text = _User.UserID.ToString();

                Mode = enMode.Update;
                label1.Text = "Update User";
                this.Text = "Update User";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void _ResetDefaultValues()
        {
            if (Mode == enMode.AddNew)
            {
                label1.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
                tabPage2.Enabled = false;
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
            else
            {
                label1.Text = "Edit User";
                this.Text = "Edit User";
                tabPage2.Enabled = true;
                btnSave.Enabled = true;
            }
            txtUsername.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            cbxIsActive.Checked = true;
        }
        private void _LoadData()
        {
            _User = clsUser.FindByUserID(_UserID);
            ctrlPersonDetailsWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("Error: User Not Found");
                this.Close();
                return;
            }

            lblUserID.Text = _User.UserID.ToString();
            txtUsername.Text = _User.Username;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            cbxIsActive.Checked = _User.IsActive;
            ctrlPersonDetailsWithFilter1.LoadPersonInfo(_User.PersonID);

        }
        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int PersonID)
        {
            _User.PersonID = PersonID;
            if (PersonID != -1)
            {
                lblUserID.Text = "[???]";
                btnNext.Enabled = true;
            }
            else
            {
                btnNext.Enabled = false;
            }
        }
        private void frmAddUser_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();
            if (Mode == enMode.Update)
            {
                _LoadData();
            }
            ctrlPersonDetailsWithFilter1.OnPersonSelected += ctrlPersonDetailsWithFilter1_OnPersonSelected;
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmPassword.Text != txtPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "This Password Does Not Match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }
        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "This field is mandatory");
                return;
            }

            if (Mode == enMode.AddNew || (Mode == enMode.Update && txtUsername.Text != _User.Username))
            {
                if (clsUser.IsUserExist(txtUsername.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUsername, "Username is already used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUsername, null);
                }
            }
            else
            {
                errorProvider1.SetError(txtUsername, null);
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This Field Is Mandatory");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

       
    }
}
