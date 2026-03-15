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
using System.IO;
using DVLD.Properties;

namespace DVLD.Licenses.International_Licenses.Control
{
    public partial class ctrlInternationLicenseInfo : UserControl
    {
        private int _InternationalLicenseID;
        private clsInternationalLicense _InternationalLicense;
        public int InternationalID 
        { 
            set 
            {
                _InternationalLicenseID = value;
            }
            get 
            { 
                return _InternationalLicenseID;
            } 
        }
        public clsInternationalLicense InternationalLicense 
        { 
            get 
            { 
                return _InternationalLicense; 
            } 
        }
        public ctrlInternationLicenseInfo()
        {
            InitializeComponent();
        }
        private void _HandleImage()
        {
            string ImagePath = _InternationalLicense._Driver.Person.ImagePath;
            if (ImagePath != "")
            {
                if (File.Exists(ImagePath))
                {
                    PicPerson.Load(ImagePath);
                }
            }
        }
        private void _FillInfo()
        {
            lblName.Text = _InternationalLicense._Driver.Person.FullName;
            lblIntLicenseID.Text = _InternationalLicense._InternationalLicenseID.ToString();
            lblLocalLicenseID.Text = _InternationalLicense._IssuedUsingLocalLicenseID.ToString();
            lblNationalNumber.Text = _InternationalLicense._Driver.Person.NationalNumber;
            if(_InternationalLicense._Driver.Person.Gender == 0)
            {
                lblGender.Text = "Female";
                PicPerson.Image = Resources.Female_512;
                picGender.Image = Resources.Woman_32;
            }
            else
            {
                lblGender.Text = "Man";
                PicPerson.Image = Resources.Male_512;
                picGender.Image = Resources.Man_32;
            }
            lblIssueDate.Text = _InternationalLicense._IssueDate.ToShortDateString();
            lblApplicationID.Text = _InternationalLicense._ApplicationID.ToString();
            lblIsActive.Text = (_InternationalLicense._IsActive ? "Yes" : "No");
            lblDateOfBirth.Text = _InternationalLicense._Driver.Person.BirthDate.ToShortDateString();
            lblDriverID.Text = _InternationalLicense._DriverID.ToString();
            lblExpirationDate.Text = _InternationalLicense._ExpirationDate.ToShortDateString();
            _HandleImage();
        }
        public void Load(int InternationalID)
        {
            _InternationalLicenseID = InternationalID;
            _InternationalLicense = clsInternationalLicense.Find(_InternationalLicenseID);
            _FillInfo();
        }

        
    }
}
