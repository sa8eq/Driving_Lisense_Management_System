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
        public frmAddUser()
        {
            InitializeComponent();
        }
        enum enMode { AddNew = 0, Update = 1 }
        enMode Mode;
        int _PersonID;
        clsUser _User;
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
            _User = new clsUser();
            _User.Username = txtUsername.Text;
            _User.Password = txtPassword.Text.ToString();
            _User.PersonID = _PersonID;
            _User.IsActive = cbxIsActive.Checked;
            if(!_IsValidUser())
            {
                MessageBox.Show("New User Should Have A Record In Persons Data Base");
                return;
            }
            if(Mode == enMode.AddNew && clsUser.IsUserExistForPersonID(_User.PersonID))
            {
                MessageBox.Show("You Cant Have More Than 1 User Profile For Each Person");
                return;
            }
            
            if (!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Missings");
                return;
            }

            if(_User.Save())
            {
                Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK);
                lblUserID.Text = _User.UserID.ToString();
            }
            else
            {
                MessageBox.Show("Data Saving Failed", "Error", MessageBoxButtons.OK);
            }
        }
        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (clsUser.IsUserExist(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "This Username is in use");
            }
            else if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "This Field Is Mandatory");
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
