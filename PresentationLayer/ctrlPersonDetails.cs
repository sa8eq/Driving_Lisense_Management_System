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

namespace PresentationLayer
{
    public partial class ctrlPersonDetails : UserControl
    {
        public ctrlPersonDetails()
        {
            InitializeComponent();
            
        }
        private clsPersonBussinesLayer _Person = new clsPersonBussinesLayer();
        private void _DisplayPersonInfo(clsPersonBussinesLayer Person)
        {
            lblPersonID.Text = Person._ID.ToString();
            lblName.Text = Person._FirstName + " " + Person._SecondName + " " + Person._ThirdName + " " + Person._LastName;
            lblNAtionalNo.Text = Person._NationalNumber;
            lblGender.Text = (Person._Gender ==  1) ? "Male" : "Female";
            lblEmail.Text = Person._Email;
            lblAddress.Text = Person._Address;
            lblDateOfBirth.Text = Person._BirthDate.ToString("dd/MM/yyyy");
            lblPhone.Text = Person._Phone;
            lblCountry.Text = clsCountriesBussinesLayer.GetCountryName(Person._CountryID).ToString();


            if (!string.IsNullOrWhiteSpace(Person._ImagePath) && System.IO.File.Exists(Person._ImagePath))
            {
                pictureBox1.ImageLocation = Person._ImagePath;
            }
            else
            {
                pictureBox1.Image = (Person._Gender == 1) ? Properties.Resources.Male_512 : Properties.Resources.Female_512;
            }
        }
        private void _BringPersonInfo(int PersonID)
        {
            _Person = clsPersonBussinesLayer.GetPersonInfoByID(PersonID);
            if (_Person != null)
            {
                _DisplayPersonInfo(_Person);
            }
            else
            {
                return;
            }
        }
        public void LoadPersonInforamtion(int PersonID)
        {
            _BringPersonInfo(PersonID);
            
        }

        private void linklblEditPerson_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            frmUpdatePerson frm = new frmUpdatePerson(_Person);
            frm.ShowDialog();
            _BringPersonInfo(_Person._ID);
        }

      
    }
}
