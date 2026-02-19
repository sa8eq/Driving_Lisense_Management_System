using BussinesLayer;
using DVLD.Persons;
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
        public frmMain()
        {
            InitializeComponent();
        }
        private clsUser _User;
        public frmMain(clsUser CurrentUser)
        {
            InitializeComponent();
            _User = CurrentUser;
            txtCurrentUserID.Text = _User.UserID.ToString();
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
            _User = null;
            this.Close();
        }

        private void currentUserInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(_User.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmChangePassword frm = new frmChangePassword(_User.UserID);
            frm.ShowDialog();
        }
    }
}
