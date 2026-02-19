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
    public partial class frmAddUser : Form
    {
        enum enMode { AddNew = 0, Update = 1 }
        enMode Mode;
        int _PersonID;
        int _UserID = -1;
        clsUser _User;
        public frmAddUser()
        {
            InitializeComponent();
        }
        public frmAddUser(int UserID)
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
            tabControl1.SelectedIndex = 1;
            btnSave.Enabled = true;
            
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
        
        private void frmAddUser_Load(object sender, EventArgs e)
        {
            ctrlPersonDetailsWithFilter1.OnPersonSelected += ctrlPersonDetailsWithFilter1_OnPersonSelected;

            if (Mode == enMode.AddNew)
            {
                this.Text = "Add New User";
                _User = new clsUser();
                btnNext.Enabled = false;
                return;
            }

            this.Text = "Update User";
            _User = clsUser.FindUser(_UserID);

            if (_User == null)
            {
                MessageBox.Show("Error: User Not Found");
                this.Close();
                return;
            }

            ctrlPersonDetailsWithFilter1.LoadPersonInfo(_User.PersonID);
            ctrlPersonDetailsWithFilter1.FilterEnabled = false;

            _PersonID = _User.PersonID;
            txtUsername.Text = _User.Username;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            cbxIsActive.Checked = _User.IsActive;
            lblUserID.Text = _User.UserID.ToString();

            btnNext.Enabled = true;


        }
        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int PersonID)
        {
            _PersonID = PersonID;
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
        private bool _IsValidUser()
        {
            clsPerson Person = new clsPerson();
            Person = clsPerson.Find(_User.PersonID);
            if (Person == null)
            {
                
                return false;
            }
            else
            {
                return true;
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
            _User.PersonID = _PersonID;
            _User.IsActive = cbxIsActive.Checked;

            if (Mode == enMode.AddNew && clsUser.IsUserExistForPersonID(_User.PersonID))
            {
                MessageBox.Show("This person already has a user account!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (_User.Save())
            {
                Mode = enMode.Update;
                this.Text = "Update User";
                lblUserID.Text = _User.UserID.ToString();
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Data Saving Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
