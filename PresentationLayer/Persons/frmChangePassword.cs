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
        private int _UserID = -1;
        clsUser _User;
        public frmChangePassword()
        {
            InitializeComponent();
        }

        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _User = clsUser.FindUser(_UserID);
        }
        private void LoadData()
        {
            if (_UserID != -1)
            {
                ctrlPersonDetails1.LoadPersonInfo(_User.PersonID);
                lblUserID.Text = _User.UserID.ToString();
                lblUserName.Text = _User.Username;
                if (_User.IsActive == true)
                {
                    lblIsActive.Text = "Yes";
                }
                else
                {
                    lblIsActive.Text = "No";
                }

            }
            else
            {
                return;
            }
        }
        private void ctrlPersonDetails1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtCurrentPassword.Text!=_User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Enter The Correct Current Password");
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            }
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtNewPassword.Text == _User.Password)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "You Cant Use The Same Password Already In Use, Try New One");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            }
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text!=txtNewPassword.Text)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation Doesnt Match");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _User.Password = txtConfirmPassword.Text;
            if (_User.Save())
            {
                MessageBox.Show("Data Updated Successfully");
            }
            else
            {
                MessageBox.Show("Failed Updating data");
            }
        }
    }
}
