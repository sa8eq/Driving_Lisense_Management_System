using BussinesLayer;
using DVLD.Classess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.TestTypes
{
    public partial class frmEditTestType : Form
    {
        clsTestTypes TestType = new clsTestTypes();
        clsTestTypes.enTestType ID = 0;
        public frmEditTestType(clsTestTypes.enTestType TestID)
        {
            InitializeComponent();

            ID = TestID;

        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            TestType = clsTestTypes.GetTestByID(ID);
            if(TestType!=null)
            {
                txtTitle.Text = TestType._Title;
                txtFees.Text = TestType._Fees.ToString();
                txtDiscription.Text = TestType._Discreption;
                lblID.Text = ((int)ID).ToString();
            }
            else
            {
                MessageBox.Show("Cannot Find This Test Type With This ID");
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("All Fields Are Mandatory");
                return;
            }
            TestType._Title = txtTitle.Text;
            TestType._Discreption = txtDiscription.Text;
            TestType._Fees = Convert.ToSingle(txtFees.Text);
            if(TestType.Save())
            {
                MessageBox.Show("Data Has Been Saved Successfully");
            }
            else
            {
                MessageBox.Show("Saving Data Failed");
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "Thie Field Can Not Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }

        private void txtDiscription_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtDiscription.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtDiscription, "Thie Field Can Not Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtDiscription, null);
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Thie Field Can Not Be Empty");
            }
            else if(!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This Field Can Only Accept Numbers");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }
    }
}
