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
    }
}
