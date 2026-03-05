using BussinesLayer;
using DVLD.Classes;
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

namespace DVLD.Applications.Controls
{
    public partial class ctrlApplicationBasicInfo : UserControl
    {
        private clsApplication _Application;

        private int _ApplicationID = -1;

        public int ApplicationID
        {
            get { return _ApplicationID; }
        }
        public ctrlApplicationBasicInfo()
        {
            InitializeComponent();
        }
        public void LoadApplicationInfo(int ApplicationID)
        {

            _Application = clsApplication.FindBaseApplication(ApplicationID);
            if (_Application == null)
            {

                ResetApplicationInfo();
                MessageBox.Show("No Application with ApplicationID = " + ApplicationID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
                _FillApplicationInfo();
        }

        private void _FillApplicationInfo()
        {
            _ApplicationID = _Application._ApplicationID; //
            lblApplicationID.Text = _Application._ApplicationID.ToString(); //
            lblStatus.Text = _Application.StatusText; //
            lblFees.Text = _Application._PaidFees.ToString(); //
            lblDate.Text = clsFormat.DateToShort(_Application._ApplicationDate); //
            lblLastStatusDate.Text = clsFormat.DateToShort(_Application._LastStatusDate); //

            // فحص الأجسام المرتبطة قبل الوصول لخصائصها لتجنب التوقف
            lblType.Text = (_Application._ApplicationTypeInfo != null) ? _Application._ApplicationTypeInfo._Title : "N/A"; //
            lblApplicant.Text = _Application.ApplicantFullName; //
            lblCreatedByUser.Text = (_Application._CreatedByUserInfo != null) ? _Application._CreatedByUserInfo.Username : "N/A"; //
            //_ApplicationID = _Application._ApplicationID;
            //lblApplicationID.Text = _Application._ApplicationID.ToString();
            //lblStatus.Text = _Application.StatusText;
            //lblType.Text = _Application._ApplicationTypeInfo._Title;
            //lblFees.Text = _Application._PaidFees.ToString();
            //lblApplicant.Text = _Application.ApplicantFullName;
            //lblDate.Text = clsFormat.DateToShort(_Application._ApplicationDate);
            //lblLastStatusDate.Text = clsFormat.DateToShort(_Application._LastStatusDate);
            //lblCreatedByUser.Text = _Application._CreatedByUserInfo.Username;
        }

        public void ResetApplicationInfo()
        {
            _ApplicationID = -1;

            lblApplicationID.Text = "[????]";
            lblStatus.Text = "[????]";
            lblType.Text = "[????]";
            lblFees.Text = "[????]";
            lblApplicant.Text = "[????]";
            lblDate.Text = "[????]";
            lblLastStatusDate.Text = "[????]";
            lblCreatedByUser.Text = "[????]";

        }

        private void llViewPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowPersonInfo frm = new frmShowPersonInfo(_Application._ApplicantPersonID);
            frm.ShowDialog();

            //Refresh
            LoadApplicationInfo(_ApplicationID);

        }
    }

}
