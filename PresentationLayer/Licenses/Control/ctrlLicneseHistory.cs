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
        private DataTable dt;
        
        public ctrlLicneseHistory()
        {
            InitializeComponent();
            
        }

        public void LoadDataWithDriverID(int DriverID)
        {
            dt = clsLicense.GetAllLicenses();
            if (dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dt.DefaultView.RowFilter = string.Format("DriverID = {0}", DriverID);
                dataGridView1.Columns[2].Visible = false;
                dataGridView1.Columns[6].Visible = false;
                dataGridView1.Columns[7].Visible = false;
                dataGridView1.Columns[9].Visible = false;
                dataGridView1.Columns[10].Visible = false;

                dataGridView1.Columns[4].Width = 140;
                dataGridView1.Columns[5].Width = 140;

            }
            lblRecords.Text = dataGridView1.Rows.Count.ToString();
        }
    }
}
