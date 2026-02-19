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
    public partial class frmUserInfo : Form
    {
        public frmUserInfo()
        {
            InitializeComponent();
        }
        private int _UserID;
        clsUser _User;
        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            _User = clsUser.FindUser(_UserID);
            
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonDetails1_Load(object sender, EventArgs e)
        {
            ctrlPersonDetails1.LoadPersonInfo(_User.PersonID);
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            if(_User.IsActive)
            {
                lblIsActive.Text = "Yes";
            }
            else
            {
                lblIsActive.Text = "No";
            }
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.Username.ToString();
        }
    }
}
