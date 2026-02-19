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
            if (!this.ValidateChildren())
            {
                return;
            }
            clsUser CurrentUser = clsUser.FindUser(txtUsername.Text, txtPassword.Text);
            if (CurrentUser == null)
            {
                MessageBox.Show("Invalid Username/Password");
                return;
            }
            if(CurrentUser.IsActive = false)
            {
                MessageBox.Show("Your Account Is DeActivated. Please Contact Your Admin");
                return;
            }
            if (cbxRemmemberMe.Checked)
            {
                txtUsername.Text = CurrentUser.Username.ToString();
                txtPassword.Text = CurrentUser.Password.ToString();
            }
            else
            {
                txtUsername.Text = "";
                txtPassword.Text = "";
            }
            
            frmMain frm = new frmMain(CurrentUser);
            frm.ShowDialog();
            

        }
    }
}
