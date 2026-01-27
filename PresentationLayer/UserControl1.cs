using BussinesLayer;
using PersonsBussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Text.RegularExpressions;
using System.Drawing.Text;
namespace Driving_Lisense_Managment_System
{
    public partial class ctrlPerson : UserControl
    {
        public delegate void DataHandle(object sender, int PersonID);
        public event DataHandle DataBack;
        public ctrlPerson()
        {
            InitializeComponent();
        }
        void MinimumAge()
        {
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
        }
        private void rdbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbFemale.Checked)
            {
                PersonPic.Image = Properties.Resources.Female_512;
            }
        }
        private void rdbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbMale.Checked)
            {
                PersonPic.Image = Properties.Resources.Male_512;
            }
        }
        void FillCountriesComboBox()
        {
           

            DataTable dtCountries = clsCountriesBussinesLayer.GetCountries();
           
            cmbCountries.DisplayMember = "CountryName";
            cmbCountries.ValueMember = "CountryID";
            cmbCountries.DataSource = dtCountries;
        }
        void RefrshForm()
        {
            MinimumAge();
            FillCountriesComboBox();
        }
        private void ctrlPerson_Load(object sender, EventArgs e)
        {
            RefrshForm();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.FindForm()?.Close();
        }
        private bool _IsFilled()
        {
            bool IsFilled = true;
            TextBox[] requiredFields = { txtNationalNo, txtFirstName, txtLastName, txtAddress, txtPhone };
            foreach(TextBox a in requiredFields)
            {
                if(string.IsNullOrWhiteSpace(a.Text))
                {
                    errorProvider1.SetError(a, "This Field Is Required ");
                    IsFilled = false;
                }
            }
            return IsFilled;
        }
        private bool _IsNotDuplicated()
        {
            bool IsNotDupplicated = true;
            TextBox[] requiredFields = { txtNationalNo, txtPhone, txtEmail };
            foreach(TextBox a in requiredFields)
            {
                if (a == txtNationalNo)
                {
                    if (clsPersonBussinesLayer.IsExist("NationalNumber", a.Text))
                    {
                        errorProvider1.SetError(a,"National Number Is Already In Use");
                        IsNotDupplicated = false;
                    }
                }
                if (a == txtPhone)
                {
                    if (clsPersonBussinesLayer.IsExist("Phone", a.Text))
                    {
                        errorProvider1.SetError(a, "Phone Is Already In Use");

                        IsNotDupplicated = false;
                    }
                }
                if (a == txtEmail)
                {
                    if (clsPersonBussinesLayer.IsExist("Email", a.Text))
                    {
                        errorProvider1.SetError(a,"The Email You Enter Is In Use, Enter Another Email");

                        IsNotDupplicated = false;
                    }
                }
            }
            return IsNotDupplicated;
        }
        private bool _IsChecked()
        {
            return (_IsFilled()&& _IsNotDuplicated());
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_IsChecked())
            {
                byte gen = rdbFemale.Checked ? (byte)0 : (byte)1;
                int CountryID = -1;
                if (cmbCountries.SelectedValue != null)
                {
                    int.TryParse(cmbCountries.SelectedValue.ToString(), out CountryID);
                }
                if (CountryID <= 0)
                {
                    MessageBox.Show("خطأ: يرجى التأكد من اختيار دولة صحيحة من القائمة.");
                    return;
                }

                string imagePath = string.IsNullOrEmpty(PersonPic.ImageLocation) ? "" : PersonPic.ImageLocation;

                clsPersonBussinesLayer Person = new clsPersonBussinesLayer(txtNationalNo.Text, txtFirstName.Text,
                txtSecondName.Text, txtThirdName.Text, txtLastName.Text, gen, dateTimePicker1.Value,
                txtAddress.Text, txtPhone.Text, txtEmail.Text, CountryID, imagePath);


                int PersonID = Person.Save();
                if (PersonID != -1)
                {
                    DataBack?.Invoke(this, PersonID);
                    MessageBox.Show("The Person Has Been Saved With ID: " + PersonID.ToString());
                }
                else
                {
                    MessageBox.Show("Error: Person could not be saved.");
                }
            }
            else
            {
                MessageBox.Show("Fill The Required Field");
            }
            
        }
        private void SetImageLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "jpg files (*.jpg)|*.jpg |png files (*.png)|*.png";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                Directory.CreateDirectory(@"C:\DVLD-Persons-Images");

                string Distination = Path.Combine(@"C:\DVLD-Persons-Images",
                    Guid.NewGuid().ToString() + Path.GetExtension(OpenFile.FileName));

                File.Copy(OpenFile.FileName, Distination);
                PersonPic.ImageLocation = Distination;

                lblRemoveLink.Visible = true;
                lblRemoveLink.Enabled = true;
            }
        }
        private void lblRemoveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            File.Delete(PersonPic.ImageLocation); ;
            lblRemoveLink.Visible = false;
            lblRemoveLink.Enabled = false;

            if (rdbMale.Checked)
            {
                PersonPic.Image = Properties.Resources.Male_512;
            }
            else
            {
                PersonPic.Image = Properties.Resources.Female_512;
            }
        }
        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "National Number is required!");
                return;
            }
            if(clsPersonBussinesLayer.IsExist("NationalNumber",txtNationalNo.Text))
            {
                e.Cancel = true;
                //txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "This National Number Is Used Before, You Can't Used Anymore");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }
        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtFirstName, "First Name is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, "");
            }
        }
        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtLastName, "Last Name is required!");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, "");
            }
        }
        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtPhone,"Phone Number Is Required");
            }
            else if(clsPersonBussinesLayer.IsExist("Phone", txtPhone.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone Number Already In Use, Try Diffrent Number");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }
        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                //e.Cancel = true;
                errorProvider1.SetError(txtAddress,"Adress Is Required");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAddress, "");
            }
        }
        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if(string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                e.Cancel = false;
            }
            else if (!Regex.IsMatch(txtEmail.Text, emailPattern))
            {
                e.Cancel = true; 
                errorProvider1.SetError(txtEmail, "Please enter a valid email address (e.g., user@example.com)");
            }
            else
            {
                // 4. إذا كان الإيميل صحيحاً، امسح الخطأ
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }
    }
}