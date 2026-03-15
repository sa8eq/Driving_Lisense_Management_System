using BussinesLayer;
using DVLD.Licenses;
using DVLD.Licenses.International_Licenses;
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

namespace DVLD.Applications.International_Applications
{
    public partial class frmManageInternationalLicenseApplications : Form
    {
        DataTable dt;
        public frmManageInternationalLicenseApplications()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmNewInternationalApplication frm = new frmNewInternationalApplication();
            frm.ShowDialog();
            frmManageInternationalLicenseApplications_Load(null, null);
        }
        private void frmManageInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            dt = clsInternationalLicense.GetAllInterNationalLicenses();
            cbFilter.SelectedIndex = 0;
            

            dataGridView1.DataSource = dt;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "Int.License ID";
                dataGridView1.Columns[1].HeaderText = "Application ID";
                dataGridView1.Columns[2].HeaderText = "Driver ID";
                dataGridView1.Columns[3].HeaderText = "L.License ID";
                dataGridView1.Columns[4].HeaderText = "Issue Date";
                dataGridView1.Columns[5].HeaderText = "Expiration Date";
                dataGridView1.Columns[6].HeaderText = "Is Active";

                dataGridView1.Columns[0].Width = 100;
                dataGridView1.Columns[1].Width = 100;
                dataGridView1.Columns[2].Width = 100;
                dataGridView1.Columns[3].Width = 135;
                dataGridView1.Columns[4].Width = 135;
                dataGridView1.Columns[5].Width = 135;
                dataGridView1.Columns[6].Width = 100;

               dataGridView1.Columns[7].Visible = false;
            }
        }
        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.Text == "Is Active")
            {
                txtFilter.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }

            else

            {

                txtFilter.Visible = (cbFilter.Text != "None");
                cbIsReleased.Visible = false;

                if (cbFilter.Text == "None")
                {
                    txtFilter.Enabled = false;
                    if (dt == null) return;
                    dt.DefaultView.RowFilter = "";
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();

                }
                else
                    txtFilter.Enabled = true;

                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilter.Text)
            {
                case "International License ID":
                    FilterColumn = "InternationalLicenseID";
                    break;
                case "Application ID":
                    {
                        FilterColumn = "ApplicationID";
                        break;
                    }
                    ;

                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Local License ID":
                    FilterColumn = "IssuedUsingLocalLicenseID";
                    break;

                case "Is Active":
                    FilterColumn = "IsActive";
                    break;


                default:
                    FilterColumn = "None";
                    break;
            }


            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();

                return;
            }



            dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (dt == null) return;
            string FilterColumn = "IsActive";
            string FilterValue = cbIsReleased.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
                dt.DefaultView.RowFilter = "";
            else
                dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);

            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dataGridView1.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int DriverID = (int)dataGridView1.CurrentRow.Cells[2].Value;
            int PersonID = clsDriver.FindByDriverID(DriverID).PersonID;
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(PersonID);
            frm.ShowDialog();
        }
    }
}

