using BussinesLayer;
using DVLD.Applications.LocalApplications;
using DVLD.Licenses;
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

            if (dt.Rows.Count > 0)
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
                dataGridView1.Columns[4].DefaultCellStyle.Format = "dd/mm/yyyy";

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
            switch (cmFilter.Text)
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
                if (FilterValue == "1") FilterValue = "New";
                else if (FilterValue == "2") FilterValue = "Cancelled";
                else if (FilterValue == "3") FilterValue = "Completed";
            }

            dt.DefaultView.RowFilter = string.Format("Convert([{0}], 'System.String') LIKE '{1}%'", FilterColumn, FilterValue);

            lblRecordsCounts.Text = dataGridView1.Rows.Count.ToString();
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {


            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication();

            frm.ShowDialog();
            _RefreshPersonsList();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmLocalDrivingLicenseApplication frm = new frmLocalDrivingLicenseApplication(ID);
            frm.ShowDialog();
            _RefreshPersonsList();
        }
        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want to delete this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            clsLocalDrivingLicenseApplication ApplicationToDelete = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(ID);
            if (ApplicationToDelete != null)
            {
                if (ApplicationToDelete.Delete())
                {
                    MessageBox.Show("Application Deleted Successfully");
                }
                else
                {
                    MessageBox.Show("Deleting Application Failed");
                }
            }
            _RefreshPersonsList();
        }
        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmAddUpdateLocalDrivingLicesnseApplication frm = new frmAddUpdateLocalDrivingLicesnseApplication(ID);
            frm.ShowDialog();
            _RefreshPersonsList();
        }
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this application?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(LocalDrivingLicenseApplicationID);

            if (LocalDrivingLicenseApplication != null)
            {
                if (LocalDrivingLicenseApplication.Cancel())
                {
                    MessageBox.Show("Application Has Been canceled Successfully");
                }
                else
                {
                    MessageBox.Show("Application Cancelling Failed");
                }
            }
            else
            {
                MessageBox.Show("The Application is null");
            }
            _RefreshPersonsList();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
        
            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication =
                clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID
                    (LocalDrivingLicenseApplicationID);

            int TotalPassedTests = (int)dataGridView1.CurrentRow.Cells[5].Value;
            bool LicenseExist = localDrivingLicenseApplication.IsLicenseExist();

            issueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = (TotalPassedTests == 3) && !LicenseExist;
            showLicenseToolStripMenuItem.Enabled = LicenseExist;
            editApplicationToolStripMenuItem.Enabled = !LicenseExist && (localDrivingLicenseApplication._Status == clsLocalDrivingLicenseApplication.enStatus.New);
            schadualeTestToolStripMenuItem.Enabled = !LicenseExist;
            cancelApplicationToolStripMenuItem.Enabled = (localDrivingLicenseApplication._Status == clsLocalDrivingLicenseApplication.enStatus.New);
            deleteApplicationToolStripMenuItem.Enabled = (localDrivingLicenseApplication._Status == clsLocalDrivingLicenseApplication.enStatus.New) || (localDrivingLicenseApplication._Status == clsLocalDrivingLicenseApplication.enStatus.Cancelled);

            bool PassedVisionTest = clsLocalDrivingLicenseApplication.DoesPassTestType(localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, (int)clsTestTypes.enTestType.VisionTest);
            bool PassedWrittenTest = clsLocalDrivingLicenseApplication.DoesPassTestType(localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, (int)clsTestTypes.enTestType.WrittenTest);
            bool PassedStreetTest = clsLocalDrivingLicenseApplication.DoesPassTestType(localDrivingLicenseApplication.LocalDrivingLicenseApplicationID, (int)clsTestTypes.enTestType.StreetTest);



            schadualeTestToolStripMenuItem.Enabled = (!PassedVisionTest || !PassedWrittenTest || !PassedStreetTest) && (localDrivingLicenseApplication._Status == clsApplication.enStatus.New);

            if (schadualeTestToolStripMenuItem.Enabled)
            {
                visionTestToolStripMenuItem.Enabled = !PassedVisionTest;
                schedualeWrittenTestToolStripMenuItem.Enabled = PassedVisionTest && !PassedWrittenTest;
                schedualeStreetTestToolStripMenuItem.Enabled = PassedVisionTest && PassedWrittenTest && !PassedStreetTest;
            }
        }

        private void visionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmTestAppointments frm = new frmTestAppointments(ID, clsTestTypes.enTestType.VisionTest);

            frm.ShowDialog();
            _RefreshPersonsList();
        }

        private void schedualeWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            frmTestAppointments frm = new frmTestAppointments(ID, clsTestTypes.enTestType.WrittenTest);

            frm.ShowDialog();
            _RefreshPersonsList();
        }

        private void schedualeStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            frmTestAppointments frm = new frmTestAppointments(ID, clsTestTypes.enTestType.StreetTest);

            frm.ShowDialog();
            _RefreshPersonsList();
        }

        private void issueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmIssueDriverLicenseForTheFirstTime frm = new frmIssueDriverLicenseForTheFirstTime(ID);
            frm.ShowDialog();
            _RefreshPersonsList();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmDriverLicenseInfo frm = new frmDriverLicenseInfo(ID);
            frm.ShowDialog();
        }

        private void showPersonsLicenseHostoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
            frmPersonLicneseHistory frm = new frmPersonLicneseHistory(ID);
            frm.ShowDialog();
        }
    }
}
