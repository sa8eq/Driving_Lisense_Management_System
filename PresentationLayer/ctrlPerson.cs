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
using System.Text.RegularExpressions;
using System.Drawing.Text;
namespace PresentationLayer
{
    public partial class ctrlPerson : UserControl
    {
        public delegate void DataHandle(object sender, int PersonID);
        public event DataHandle DataBack;
        private clsPersonBussinesLayer _Person = new clsPersonBussinesLayer();
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
            clsPersonBussinesLayer PersonWithNationalNo = clsPersonBussinesLayer.Find(txtNationalNo.Text);
            if (PersonWithNationalNo != null && PersonWithNationalNo._ID != _Person._ID)
            {
                errorProvider1.SetError(txtNationalNo, "National Number is already in use by another person!");
                IsNotDupplicated = false;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, "");
            }
            if (clsPersonBussinesLayer.IsExist("Phone", txtPhone.Text))
            {
                DataTable dt = clsPersonBussinesLayer.GetAllPersonsWithFilter("Phone", txtPhone.Text);
                if (dt.Rows.Count > 0 && (int)dt.Rows[0]["PersonID"] != _Person._ID)
                {
                    errorProvider1.SetError(txtPhone, "Phone Number is already in use!");
                    IsNotDupplicated = false;
                }
                else
                {
                    errorProvider1.SetError(txtPhone, "");
                }
            }
            return IsNotDupplicated;
        }
        private bool _IsChecked()
        {
            return (_IsFilled()&& _IsNotDuplicated());
        }
        private bool _SaveImageToDistination()
        {
            if (_Person._ImagePath == PersonPic.ImageLocation) {return true;}
            if (!string.IsNullOrEmpty(_Person._ImagePath) && File.Exists(_Person._ImagePath))
            {
                try
                {
                    File.Delete(_Person._ImagePath);
                }
                catch {  }
            }
            if (string.IsNullOrEmpty(PersonPic.ImageLocation))
            {
                _Person._ImagePath = "";
                return true;
            }
            try
            {
                string folderPath = @"C:\DVLD-Persons-Images";
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
                string extension = Path.GetExtension(PersonPic.ImageLocation);
                string newFileName = Guid.NewGuid().ToString() + extension;
                string destination = Path.Combine(folderPath, newFileName);
                if (File.Exists(PersonPic.ImageLocation))
                {
                    File.Copy(PersonPic.ImageLocation, destination, true);
                    _Person._ImagePath = destination;
                    PersonPic.ImageLocation = destination;
                    return true;
                }
                else
                {
                    MessageBox.Show("Source file not found: " + PersonPic.ImageLocation, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_IsChecked()) return;
            if (!_SaveImageToDistination()) return;
           
            _Person._NationalNumber = txtNationalNo.Text;
            _Person._FirstName = txtFirstName.Text;
            _Person._SecondName = txtSecondName.Text;
            _Person._ThirdName = txtThirdName.Text;
            _Person._LastName = txtLastName.Text;
            _Person._Gender = rdbFemale.Checked ? (byte)0 : (byte)1;
            _Person._BirthDate = dateTimePicker1.Value;
            _Person._Address = txtAddress.Text;
            _Person._Phone = txtPhone.Text;
            _Person._Email = txtEmail.Text;
            _Person._CountryID = (int)cmbCountries.SelectedValue;
            _Person._ImagePath = PersonPic.ImageLocation;

            string Message = "";

            
            if (_Person.Save(ref Message))
            {
                
                MessageBox.Show(Message, "Succeed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DataBack?.Invoke(this, _Person._ID);
            }
            else
            {
                MessageBox.Show("Failed To Save", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void SetImageLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        { 
            OpenFileDialog OpenFile = new OpenFileDialog();
            OpenFile.Filter = "jpg files (*.jpg)|*.jpg |png files (*.png)|*.png";

            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
               
                PersonPic.ImageLocation = OpenFile.FileName;

                lblRemoveLink.Visible = true;
                lblRemoveLink.Enabled = true;
            }
        }
        private void lblRemoveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            PersonPic.ImageLocation = null;
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
            if (string.IsNullOrWhiteSpace(txtNationalNo.Text))
            {
                errorProvider1.SetError(txtNationalNo, "National Number is required!");
                return;
            }

            clsPersonBussinesLayer PersonWithNationalNo = clsPersonBussinesLayer.Find(txtNationalNo.Text);

            if (PersonWithNationalNo != null && PersonWithNationalNo._ID != _Person._ID)
            {
                e.Cancel = true; 
                errorProvider1.SetError(txtNationalNo, "This National Number is already in use by another person.");
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
            if (string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                errorProvider1.SetError(txtPhone, "Phone Number is required");
                return;
            }

            DataTable dt = clsPersonBussinesLayer.GetAllPersonsWithFilter("Phone", txtPhone.Text);

            if (dt.Rows.Count > 0 && (int)dt.Rows[0]["PersonID"] != _Person._ID)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPhone, "Phone Number is already in use by another person.");
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
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }
        public void LoadPersonData(int PersonID)
        {
            _Person = clsPersonBussinesLayer.GetPersonInfoByID(PersonID);
            if(_Person!=null)
            {
                txtFirstName.Text = _Person._FirstName;
                txtSecondName.Text = _Person._SecondName;
                txtThirdName.Text = _Person._ThirdName;
                txtLastName.Text = _Person._LastName;
                txtNationalNo.Text = _Person._NationalNumber;
                dateTimePicker1.Value = _Person._BirthDate;
                txtAddress.Text = _Person._Address;
                txtEmail.Text = _Person._Email;
                txtPhone.Text = _Person._Phone;
                cmbCountries.SelectedValue= _Person._CountryID;
                cmbCountries.Enabled = false;
                if(!string.IsNullOrWhiteSpace(_Person._ImagePath))
                {
                    PersonPic.ImageLocation = _Person._ImagePath;
                }
                
                if(_Person._Gender==0)
                {
                    rdbFemale.Checked = true;
                }
                else
                {
                    rdbMale.Checked = true;
                }
                lblRemoveLink.Enabled = true;
                lblRemoveLink.Visible = true;
            }
        }
    }
}