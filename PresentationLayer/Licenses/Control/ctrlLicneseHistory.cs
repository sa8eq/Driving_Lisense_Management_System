using BussinesLayer;
using DVLD.Licenses.International_Licenses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.Control
{
    public partial class ctrlLicneseHistory : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;

        public ctrlLicneseHistory()
        {
            InitializeComponent();

        }

        private void _LoadLocalLicenseInfo()
        {

            _dtDriverLocalLicensesHistory = clsDriver.GetLicenses(_DriverID);


            dataGridView1.DataSource = _dtDriverLocalLicensesHistory;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "Lic.ID";
                dataGridView1.Columns[0].Width = 110;

                dataGridView1.Columns[1].HeaderText = "App.ID";
                dataGridView1.Columns[1].Width = 110;

                dataGridView1.Columns[2].HeaderText = "Class Name";
                dataGridView1.Columns[2].Width = 270;

                dataGridView1.Columns[3].HeaderText = "Issue Date";
                dataGridView1.Columns[3].Width = 170;
                
                dataGridView1.Columns[4].HeaderText = "Expiration Date";
                dataGridView1.Columns[4].Width = 170;
                
                dataGridView1.Columns[5].HeaderText = "Is Active";
                dataGridView1.Columns[5].Width = 110;

            }
        }
        private void _LoadInternationalLicenseInfo()
        {
            _dtDriverInternationalLicensesHistory = clsDriver.GetInternationalLicenses(_DriverID);


            dataGridView2.DataSource = _dtDriverInternationalLicensesHistory;
            lblinterRecords.Text = dataGridView2.Rows.Count.ToString();

            if (dataGridView2.Rows.Count > 0)
            {
                dataGridView2.Columns[0].HeaderText = "Int.License ID";
                dataGridView2.Columns[0].Width = 160;

                dataGridView2.Columns[1].HeaderText = "Application ID";
                dataGridView2.Columns[1].Width = 130;

                dataGridView2.Columns[2].HeaderText = "L.License ID";
                dataGridView2.Columns[2].Width = 130;

                dataGridView2.Columns[3].HeaderText = "Issue Date";
                dataGridView2.Columns[3].Width = 180;

                dataGridView2.Columns[4].HeaderText = "Expiration Date";
                dataGridView2.Columns[4].Width = 180;

                dataGridView2.Columns[5].HeaderText = "Is Active";
                dataGridView2.Columns[5].Width = 120;
            }
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.FindByDriverID(_DriverID);

            if (_Driver == null)
            {
                MessageBox.Show("There Is Driver With This ID","No Driver");
                return;
            }

            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }

        public void LoadInfoByPersonID(int PersonID)
        {

            _Driver = clsDriver.FindByPersonID(PersonID);
            

            if (_Driver == null)
            {
                MessageBox.Show("There is no driver linked with this person ID", "No Driver");
                return;
            }
            _DriverID = _Driver.DriverID;
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }

        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtDriverInternationalLicensesHistory.Clear();

        }

        private void showInternationalLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int InternationalLicenseID = (int)dataGridView2.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showLicenseInfoToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int LicenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmShowDriverLicenseInfo frm = new frmShowDriverLicenseInfo(LicenseID);
            frm.ShowDialog();
        }
    }
}
