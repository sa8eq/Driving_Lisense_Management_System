using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinesLayer;
using DVLD.Classess;
namespace DVLD
{
    public partial class frmLogIn : Form
    {
        public frmLogIn()
        {
            InitializeComponent();
        }
        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {
            if (txtUsername.Text == "" || string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUsername, "This Field Cannot Be Empty!");          
            }
            else
            {
                errorProvider1.SetError(txtUsername,null);
            }
        }
        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtPassword.Text == "" || string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "This Field Cannot Be Empty!");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.FindUserByUserNameAndPassword(txtUsername.Text, txtPassword.Text);

            if (User != null)
            {
                if(cbxRemmemberMe.Checked)
                {
                    clsGlobal.RememberUserNameAndPassword(txtUsername.Text.Trim(),txtPassword.Text.Trim());
                }
                else
                {
                    clsGlobal.RememberUserNameAndPassword("", "");
                }
             
                if (!User.IsActive)
                {
                    MessageBox.Show("Your Account Is DeActivated. Please Contact Your Admin");
                    return;
                }

                clsGlobal.CurrentUser = User;
                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();

            }
            else
            {
                MessageBox.Show("Invalid Username/Password");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLogIn_Load(object sender, EventArgs e)
        {
            string Username = "", Password = "";
            if(clsGlobal.GetStoredCredentials(ref Username, ref Password))
            {
                txtUsername.Text = Username;
                txtPassword.Text = Password;
                cbxRemmemberMe.Checked = true;
            }
            else
            {
                cbxRemmemberMe.Checked = false;
            }
        }
    }
}
