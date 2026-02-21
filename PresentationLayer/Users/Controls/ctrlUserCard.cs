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

namespace DVLD.Persons.Controls
{
    public partial class ctrlUserCard : UserControl
    {
        
        private clsUser _User;
        private int _UserID = -1;
        public int UserID
        {
            get { return _UserID; }
        }
        public ctrlUserCard()
        {
            InitializeComponent();
        }
        private void _ResetPersonInfo()
        {
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }
        private void _FillUserInfo()
        {
            ctrlPersonDetails1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.Username.ToString();
            if(_User.IsActive)
            {
                lblIsActive.Text = "Yes";
            }
            else
            {
                lblIsActive.Text = "No";
            }
        }
        public void LoadUserInfo(int UserID)
        {
            _UserID = UserID;
            _User = clsUser.FindByUserID(UserID);
            if(_User==null)
            {
                _ResetPersonInfo();
                MessageBox.Show("No User with this userid was found");
                return;
            }
            _FillUserInfo();
        }
    }
}
