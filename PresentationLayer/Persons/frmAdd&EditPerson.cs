using BussinesLayer;
using DVLD.Classess;
using PersonsBussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace DVLD
{
    public partial class frmAddEditPerson : Form
    {
        public delegate void DataBackEvenHandler(object sender, int PersonID);
        public event DataBackEvenHandler DataBack;
        enum enMode { AddNew = 0, Update = 1 }
        enum enGender { Female = 0, Male = 1 };
        clsPerson _Person;

        private enMode _Mode;
        private enGender _Gender;
        private int _PersonID = -1;
        public frmAddEditPerson()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }
        public frmAddEditPerson(int PersonID)
        {
            InitializeComponent();
            _Mode = enMode.Update;
            _PersonID = PersonID;
        }

        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetCountries();
            foreach (DataRow row in dtCountries.Rows)
            {
                cmbCountries.Items.Add(row["CountryName"]);
            }
        }

        private void _ResetToDefaultValues()
        {
            _FillCountriesInComboBox();
            if (_Mode == enMode.AddNew)
            {
                label1.Text = "Add New Person";
                _Person = new clsPerson();
            }
            else
            {
                label1.Text = "Edit Person Info";
            }
            if (rdbMale.Checked)
            {
                PersonPic.Image = Properties.Resources.Male_512;
            }
            else
            {
                PersonPic.Image = Properties.Resources.Female_512;
            }
            lblRemoveLink.Visible = (PersonPic.ImageLocation != null);
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = dateTimePicker1.MaxDate;

            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);

            cmbCountries.SelectedIndex = cmbCountries.FindString("Saudi Arabia");

            txtFirstName.Text = "";
            txtSecondName.Text = "";
            txtThirdName.Text = "";
            txtLastName.Text = "";
            txtNationalNo.Text = "";
            rdbMale.Checked = true;
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
            
        }

        private void _LoadData()
        {
            _Person = clsPerson.Find(_PersonID);
            if (_Person == null)
            {
                MessageBox.Show("No Person With This ID: " + _PersonID, "Person Not Found", MessageBoxButtons.OK);
                this.Close();
                return;
            }
            lblPersonID.Text = _Person.PersonID.ToString();
            txtFirstName.Text = _Person.FirstName;
            txtSecondName.Text = _Person.SecondName;
            txtThirdName.Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;
            txtNationalNo.Text = _Person.NationalNumber;
            txtAddress.Text = _Person.Address;
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            dateTimePicker1.Value = _Person.BirthDate;
            cmbCountries.SelectedIndex = cmbCountries.FindString(_Person.CountryInfo.CountryName);
            if (_Person.Gender == 0)
            {
                rdbFemale.Checked = true;
            }
            else
            {
                rdbMale.Checked = true;
            }
            if (!string.IsNullOrWhiteSpace(_Person.ImagePath))
            {
                PersonPic.ImageLocation = _Person.ImagePath;
            }
            lblRemoveLink.Visible = (!string.IsNullOrWhiteSpace(_Person.ImagePath));
            lblRemoveLink.Enabled = (!string.IsNullOrWhiteSpace(_Person.ImagePath));

            lblSetImageLink.Enabled = (string.IsNullOrWhiteSpace(_Person.ImagePath));
            lblSetImageLink.Visible = (string.IsNullOrWhiteSpace(_Person.ImagePath));
        }

        private void frmAddNewPerson_Load(object sender, EventArgs e)
        {
            _ResetToDefaultValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private bool _HandlePersonImage()
        {
            if(_Person.ImagePath!=PersonPic.ImageLocation)
            {
                if(_Person.ImagePath!="")
                {
                    try
                    {
                        PersonPic.Image.Dispose();
                        File.Delete(_Person.ImagePath);                        
                    }
                    catch(IOException ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                }
                if(PersonPic.ImageLocation!=null)
                {
                    string SourceImageFile = PersonPic.ImageLocation.ToString();
                    if(clsUtil.CopyImageToProjectImagesFloder(ref SourceImageFile))
                    {
                        PersonPic.ImageLocation = SourceImageFile;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Error Copying The Picture");
                        return false;
                    }
                }
            }
            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                // إذا رجعت false، نعرض رسالة للمستخدم ونوقف الحفظ
                MessageBox.Show("Some fields are not valid!, please put the mouse over the red icon(s) to see the error",
                                "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if(!_HandlePersonImage())
            {
                return;
            }

            int NationalityCountryID = clsCountry.Find(cmbCountries.Text).CountryID;
            _Person.FirstName = txtFirstName.Text.Trim();
            _Person.SecondName = txtSecondName.Text.Trim();
            _Person.ThirdName = txtThirdName.Text.Trim();
            _Person.LastName = txtLastName.Text.Trim();
            _Person.NationalNumber = txtNationalNo.Text.Trim();
            _Person.Email = txtEmail.Text.Trim();
            _Person.Address = txtAddress.Text.Trim();
            _Person.Phone = txtPhone.Text.Trim();
            _Person.CountryID = NationalityCountryID;
            _Person.BirthDate = dateTimePicker1.Value;

            if (rdbMale.Checked)
            {
                _Person.Gender = (short)enGender.Male;
            }
            else
            {
                _Person.Gender = (short)enGender.Female;
            }
            if (!string.IsNullOrWhiteSpace(PersonPic.ImageLocation))
            {
                _Person.ImagePath = PersonPic.ImageLocation;
            }
            else
            {
                _Person.ImagePath = "";
            }
            if (_Person.Save())
            {
                lblPersonID.Text = _Person.PersonID.ToString();
                _Mode = enMode.Update;
                label1.Text = "Edit Person Info";
                MessageBox.Show("Data Saved Successfully", "Saved", MessageBoxButtons.OK);

                DataBack?.Invoke(this, _Person.PersonID);
            }
            else
            {
                MessageBox.Show("Data Saving Failed", "Error", MessageBoxButtons.OK);
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                return;
            }
            if (!clsValidation.ValidateEmail(txtEmail.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtEmail, "Invalid Email Format");
            }
            else
            {
                errorProvider1.SetError(txtEmail, null);
            }

        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This Field Is Required");
                return;
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            if (clsPerson.IsPersonExist(txtNationalNo.Text.Trim()) && txtNationalNo.Text.Trim() != _Person.NationalNumber)
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNationalNo, "This National Number Is Already In Use By Another Person");
            }
            else
            {
                errorProvider1.SetError(txtNationalNo, null);
            }

            

        }

        private void rdbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (PersonPic.ImageLocation == null)
            {
                PersonPic.Image = Properties.Resources.Male_512;
            }
        }

        private void rdbFemale_CheckedChanged(object sender, EventArgs e)
        {
            if (PersonPic.ImageLocation == null)
            {
                PersonPic.Image = Properties.Resources.Female_512;
            }
        }

        private void lblSetImageLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                PersonPic.ImageLocation = selectedFilePath;
                lblRemoveLink.Visible = true;
                lblRemoveLink.Enabled = true;

                lblSetImageLink.Visible = false;
                lblSetImageLink.Enabled = false;

                // ...
            }
        }

        private void lblRemoveLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            PersonPic.ImageLocation = null;
            if(rdbMale.Checked)
            {
                PersonPic.Image = Properties.Resources.Male_512;
            }
            else
            {
                PersonPic.Image = Properties.Resources.Female_512;
            }
            lblRemoveLink.Visible = false;
            lblRemoveLink.Enabled = false;
            lblSetImageLink.Enabled = true;
            lblSetImageLink.Visible = true;
        }

        private void ValidateEmptyTextBox(object sender, CancelEventArgs e)
        {
            TextBox Temp = (TextBox)sender;

            if (string.IsNullOrWhiteSpace(Temp.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(Temp, "This field is mandatory!");
            }
            else
            {
                errorProvider1.SetError(Temp, null);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

