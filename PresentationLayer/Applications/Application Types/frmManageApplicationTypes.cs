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

namespace DVLD.Applications
{
    public partial class frmManageApplicationTypes : Form
    {
        private DataTable dt;
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            dt = clsApplicationType.GetAllApplicationType();
            dataGridView1.DataSource = dt;
            lblRecord.Text = dataGridView1.RowCount.ToString();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "ID";
                dataGridView1.Columns[0].Width = 100;

                dataGridView1.Columns[1].HeaderText = "Title";
                dataGridView1.Columns[1].Width = 470;

                dataGridView1.Columns[2].HeaderText = "Fees";
                dataGridView1.Columns[2].Width = 100;
            }
        }
        private void editApplicationTypeToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmEditApplicationType frm = new frmEditApplicationType(SelectedID);
            frm.ShowDialog();
            frmManageApplicationTypes_Load(null, null);
        }
    }
}
