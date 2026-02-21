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
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }
        private void _ResetDefaultValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus();
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            _ResetDefaultValues();

            _User = clsUser.FindByUserID(_UserID);
            if(_User==null)
            {
                MessageBox.Show("User Cannot Be Found");
                this.Close();

                return;
            }
            ctrlUserCard1.LoadUserInfo(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password Cannot be Empty");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
            if(txtCurrentPassword.Text!=_User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current Password Doesnt Match");
                return;           
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }
        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "Current Password Cannot be Empty");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
            if(txtNewPassword.Text==_User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New Password Matches The Current Password");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
        }
        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtConfirmPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirmation Password Cannot be Empty");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
            if(txtConfirmPassword.Text != txtNewPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Confirmation Password Does Not Match The NEw Password");
                return;
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Mandatory");
                return;
            }
            _User.Password = txtNewPassword.Text;
            if(_User.Save())
            {
                MessageBox.Show("Password Changed Successfully");
                _ResetDefaultValues();
            }
            else
            {
                MessageBox.Show("Password Change Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
