using BussinesLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmManageDrivers : Form
    {
        DataTable dt;
        public frmManageDrivers()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageDrivers_Load(object sender, EventArgs e)
        {
            dt = clsDriver.GetAllDrivers();
            if(dt.Rows.Count>0)
            {
                dataGridView1.DataSource = dt;

                dataGridView1.Columns[0].HeaderText = "Driver ID";
                dataGridView1.Columns[0].Width = 80;

                dataGridView1.Columns[1].HeaderText = "Person ID";
                dataGridView1.Columns[1].Width = 80;

                dataGridView1.Columns[2].HeaderText = "National Number";
                dataGridView1.Columns[2].Width = 150;

                dataGridView1.Columns[3].HeaderText = "Full Name";
                dataGridView1.Columns[3].Width = 200;

                dataGridView1.Columns[4].HeaderText = "Date";
                dataGridView1.Columns[4].Width = 120;

                dataGridView1.Columns[5].HeaderText = "Active Licneses";
                dataGridView1.Columns[5].Width = 100;
            }
            cmbFilter.SelectedIndex = 0;
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbFilter.SelectedIndex == 0)
            {
                txtFilter.Enabled = false;
            }
            else
            {
                txtFilter.Enabled = true;
            }
            
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "", FilterValue = "";

            switch (cmbFilter.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National Number":
                    FilterColumn = "NationalNumber";
                    break;
                case "Full Name":
                    FilterColumn = "FullName";
                    break;
            }
            FilterValue = txtFilter.Text;
            if (txtFilter.Text.Trim() == "" || txtFilter.Text.Trim() == "None")
            {
                dt.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();
                return;
            }
            if (FilterColumn == "DriverID" || FilterColumn == "PersonID")
            {
                if(int.TryParse(FilterValue, out int value ))
                {
                    dt.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, value);
                }
                else
                {
                    dt.DefaultView.RowFilter = "";
                    lblRecords.Text = dataGridView1.Rows.Count.ToString();
                    return;
                }
                
            }
            else
            {
                dt.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, FilterValue);
            }
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
    }
}
