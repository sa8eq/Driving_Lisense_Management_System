using BussinesLayer;
using DVLD.Applications;
using DVLD.Classess;
using DVLD.Persons;
using DVLD.Tests.TestTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmMain : Form
    {
        frmLogIn _frmLogin;
        public frmMain(frmLogIn frm)
        {
            InitializeComponent();
            _frmLogin = frm;
            lblCurrentUserID.Text = clsGlobal.CurrentUser.UserID.ToString();
        }
        
        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm1 = Application.OpenForms["frmManagePeople"];
            if (frm1 != null)
            {
                frm1.Focus();
            }
            else
            {
                pictureBox1.SendToBack();
                frmManagePeople frm = new frmManagePeople();
                frm.MdiParent = this;
                frm.Show();
                frm.BringToFront();
            }
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pictureBox1.SendToBack();
            frmManageUsers frm = new frmManageUsers();
            frm.MdiParent = this;
            frm.Show();
            frm.BringToFront();

        }

        private void signOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _frmLogin.Show();
            this.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(clsGlobal.CurrentUser.UserID);
            frm.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(clsGlobal.CurrentUser.UserID);
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frm = new frmManageTestTypes();
            frm.ShowDialog();
        }
    }
}
