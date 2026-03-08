using BussinesLayer;
using DVLD.Tests.Schedual_Test;
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

namespace DVLD
{
    public partial class frmTestAppointments : Form
    {
        DataTable dt;
        private int _LocalDrivingLicenseApplicationID = -1;
        clsTestTypes _TestTypeInfo;
        clsTestTypes.enTestType _TestType;
        public frmTestAppointments(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestTypeInfo = clsTestTypes.GetTestByID(TestType);
        }
        private void CheckTestType()
        {
            switch (_TestTypeInfo._ID)
            {
                case clsTestTypes.enTestType.VisionTest:
                    {
                        label1.Text = "Vision " + label1.Text;
                        pictureBox1.Image = DVLD.Properties.Resources.Vision_512;
                        _TestTypeInfo = clsTestTypes.GetTestByID(clsTestTypes.enTestType.VisionTest);
                    }
                    break;
                case clsTestTypes.enTestType.WrittenTest:
                    {
                        label1.Text = "Written " + label1.Text;
                        pictureBox1.Image = DVLD.Properties.Resources.Written_Test_512;
                        _TestTypeInfo = clsTestTypes.GetTestByID(clsTestTypes.enTestType.WrittenTest);
                    }
                    break;
                case clsTestTypes.enTestType.StreetTest:
                    {
                        label1.Text = "Street " + label1.Text;
                        pictureBox1.Image = DVLD.Properties.Resources.driving_test_512;
                        _TestTypeInfo = clsTestTypes.GetTestByID(clsTestTypes.enTestType.StreetTest);
                    }
                    break;
            }
        }

        private void _RefreshAppointmentsForm()
        {
            CheckTestType();

            dataGridView1.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dataGridView1.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);


            ctrlApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            dt = clsTestAppointment.GetAllTestAppointments();

            dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", "LocalDrivingLicenseApplicationID", _LocalDrivingLicenseApplicationID.ToString());


            dataGridView1.DataSource = dt;
            if (dataGridView1.Rows.Count>0)
            {
                dataGridView1.Columns[0].HeaderText = "Test Appointment ID";
                dataGridView1.Columns[0].Width = 200;

                dataGridView1.Columns[1].HeaderText = "Appointment Date";
                dataGridView1.Columns[1].Width = 150;
               // dataGridView1.Columns[1].DefaultCellStyle.Format = "dd/mm/yyyy";


                dataGridView1.Columns[2].HeaderText = "Paid Fees";
                dataGridView1.Columns[2].Width = 125;

                dataGridView1.Columns[3].HeaderText = "Is Locked";
                dataGridView1.Columns[3].Width = 125;

                dataGridView1.Columns[4].Visible = false;
            }

            lblRecords.Text = dt.Rows.Count.ToString();

            

        }

        private void frmTestAppointments_Load(object sender, EventArgs e)
        {
            _RefreshAppointmentsForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {
            if(clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, (int)_TestTypeInfo._ID))
            {
                MessageBox.Show("There is An Active Scheduled Appointment, You Can't Book Another One", "Previous Appointment", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmSchedualeTest frm = new frmSchedualeTest(_LocalDrivingLicenseApplicationID, _TestTypeInfo);
            frm.ShowDialog();
            _RefreshAppointmentsForm();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmSchedualeTest frm = new frmSchedualeTest(TestAppointmentID);
            frm.ShowDialog();
            _RefreshAppointmentsForm();
        }
    }
}
