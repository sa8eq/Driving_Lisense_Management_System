using BussinesLayer;
using PersonsBussinesLayer;
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
    public partial class ctrlPersonDetails : UserControl
    {
        public ctrlPersonDetails()
        {
            InitializeComponent();
            
        }
        private clsPerson _Person;
        private int _PersonID = -1;
        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }
        public int PersonID
        {
            get { return _PersonID; }
        }

        public void ResetPersonInfo()
        {
            lblPersonID.Text = "???";
            lblName.Text = "???";
            lblNAtionalNo.Text = "???";
            lblGender.Text = "???";
            lblEmail.Text = "???";
            lblAddress.Text = "???";
            lblDateOfBirth.Text = "???";
            lblPhone.Text = "???";
            lblCountry.Text = "???";
            pictureBox1.ImageLocation = "";
        }

        private void _LoadPersonImage()
        {
            if (!string.IsNullOrWhiteSpace(_Person.ImagePath) && System.IO.File.Exists(_Person.ImagePath))
            {
                pictureBox1.ImageLocation = _Person.ImagePath;
            }
            else
            {
                pictureBox1.Image = (_Person.Gender == 1) ? Properties.Resources.Male_512 : Properties.Resources.Female_512;
            }
        }

        private void _LoadPersonInfo()
        {
            _PersonID = _Person.PersonID;
            linklblEditPerson.Enabled = true;
            linklblEditPerson.Visible = true;

            lblPersonID.Text = _Person.PersonID.ToString();
            lblName.Text = _Person.FirstName + " " + _Person.SecondName + " " + _Person.ThirdName + " " + _Person.LastName;
            lblNAtionalNo.Text = _Person.NationalNumber;
            lblGender.Text = (_Person.Gender == 1) ? "Male" : "Female";
            lblEmail.Text = _Person.Email;
            lblAddress.Text = _Person.Address;
            lblDateOfBirth.Text = _Person.BirthDate.ToString("dd/MM/yyyy");
            lblPhone.Text = _Person.Phone;
            lblCountry.Text = clsCountry.Find(_Person.CountryID).CountryName;
            _LoadPersonImage();
        }  

        public void LoadPersonInfo(int PersonID)
        {
            _Person = clsPerson.Find(PersonID);
            if(_Person==null)
            {
                ResetPersonInfo();
                MessageBox.Show("Error: Person With This ID Was Not Found");
                return;
            }
            _LoadPersonInfo();
        }

        public void LoadPersonInfo(string NationalNumber)
        {
            _Person = clsPerson.Find(NationalNumber);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("Error: Person With This ID Was Not Found");
                return;
            }
            _LoadPersonInfo();
        }

        private void linklblEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmAddEditPerson frm = new frmAddEditPerson(_PersonID);
            frm.ShowDialog();

            LoadPersonInfo(_PersonID);


        }

        private void ctrlPersonDetails_Load(object sender, EventArgs e)
        {

        }
    }
}
