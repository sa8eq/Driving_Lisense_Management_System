using BussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace DVLD.Applications.Application
{
    public partial class frmManageLocalDrivingLicensesApplications : Form
    {
        DataTable dt;
        public frmManageLocalDrivingLicensesApplications()
        {
            InitializeComponent();
        }

        private void _RefreshPersonsList()
        {
            txtFilter.Visible = false;
            txtFilter.Enabled = false;
            cmFilter.SelectedText = "None";
            dt = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicensesApplications();
            dataGridView1.DataSource = dt;
            lblRecordsCounts.Text = dt.Rows.Count.ToString();

            if(dt.Rows.Count>0)
            {
                dataGridView1.Columns[0].HeaderText = "L.D.L.AppID";
                dataGridView1.Columns[0].Width = 80;

                dataGridView1.Columns[1].HeaderText = "Driving Class";
                dataGridView1.Columns[1].Width = 190;

                dataGridView1.Columns[2].HeaderText = "National Number";
                dataGridView1.Columns[2].Width = 120;

                dataGridView1.Columns[3].HeaderText = "Full Name";
                dataGridView1.Columns[3].Width = 160;

                dataGridView1.Columns[4].HeaderText = "Application Date";
                dataGridView1.Columns[4].Width = 170;
                //dataGridView1.Columns[4].DefaultCellStyle.Format = "dd/mm/yyyy";

                dataGridView1.Columns[5].HeaderText = "Passed Tests";
                dataGridView1.Columns[5].Width = 100;

                dataGridView1.Columns[6].HeaderText = "Status";
                dataGridView1.Columns[6].Width = 60;
            }
        }
        private void frmManageLocalDrivingLicensesApplications_Load(object sender, EventArgs e)
        {
            _RefreshPersonsList();
        }

        private void cmFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(cmFilter.Text)
            {
                case "None":
                    txtFilter.Visible = false;
                    txtFilter.Enabled = false;
                    break;
                default:
                    txtFilter.Visible = true;
                    txtFilter.Enabled = true;
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cmFilter.Text)
            {
                case "L.D.L.AppID":
                    FilterColumn = "L.D.L.AppID";
                    break;
                case "National Number":
                    FilterColumn = "NationalNumber";
                    break;
                case "Full Name":
                    FilterColumn = "Full Name";
                    break;
                case "Status":
                    FilterColumn = "Status";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (txtFilter.Text.Trim() == "" || FilterColumn == "None")
            {
                dt.DefaultView.RowFilter = "";
                lblRecordsCounts.Text = dataGridView1.Rows.Count.ToString();
                return;
            }

            string FilterValue = txtFilter.Text.Trim();
            if (FilterColumn == "Status")
            {
                // إذا كتب المستخدم رقم، نحوله داخلياً للكلمة المقابلة له
                if (FilterValue == "1") FilterValue = "New";
                else if (FilterValue == "2") FilterValue = "Cancelled";
                else if (FilterValue == "3") FilterValue = "Completed";
            }

            dt.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", FilterColumn, FilterValue);

            lblRecordsCounts.Text = dataGridView1.Rows.Count.ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {

            frmNewLocalLicenseApplication frm = new frmNewLocalLicenseApplication();

            frm.ShowDialog();
            _RefreshPersonsList();
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmTestAppointments frm = new frmTestAppointments();

            frm.ShowDialog();
            _RefreshPersonsList();
        }
    }
}
