using BussinesLayer;
using DVLD.Classess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmEditApplicationType : Form
    {

        clsApplicationType App;
        public frmEditApplicationType(int SelectedID)
        {
            InitializeComponent();
            App = clsApplicationType.Find(SelectedID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmEditApplication_Load(object sender, EventArgs e)
        {
            if (App == null)
            {
                MessageBox.Show("Error: Could not find application type details.");
                this.Close();
                return;
            }
            lblID.Text = App._ID.ToString();
            txtTitle.Text = App._Title;
            txtFees.Text = App._Fees.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren()) 
            {
                MessageBox.Show("Please fix the errors before saving.");
                return;
            }
            App._Title = txtTitle.Text;
            App._Fees = Convert.ToSingle(txtFees.Text);
            if (App.Save())
            {
                MessageBox.Show("Application Type Has Been Updated Successfully");
                this.Close();
            }
            else
            {
                MessageBox.Show("Failed Updating Application Type");
            }
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "This Field Cannot Be Empty");
            }
            else if (!clsValidation.IsNumber(txtFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Invalid format. Please enter numbers only.");
            }
            else
            {
                errorProvider1.SetError(txtFees, null);
            }
        }

        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtTitle, "This Field Cannot Be Empty");
            }
            else
            {
                errorProvider1.SetError(txtTitle, null);
            }
        }
    }
}
