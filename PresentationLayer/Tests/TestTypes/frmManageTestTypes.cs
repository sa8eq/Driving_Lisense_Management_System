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

namespace DVLD.Tests.TestTypes
{
    public partial class frmManageTestTypes : Form
    {
        private DataTable dt;
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            dt = clsTestTypes.GetAllTestTypes();
            if(dt.Rows.Count>0)
            {
                dataGridView1.DataSource = dt;

                lblRecords.Text = dataGridView1.Rows.Count.ToString();


                dataGridView1.Columns[0].HeaderText = "Test ID";
                dataGridView1.Columns[0].Width = 90;

                dataGridView1.Columns[1].HeaderText = "Title";
                dataGridView1.Columns[1].Width = 90;

                dataGridView1.Columns[2].HeaderText = "Discription";
                dataGridView1.Columns[2].Width = 345;

                dataGridView1.Columns[3].HeaderText = "Fees";
                dataGridView1.Columns[3].Width = 90;
            }
        }

        private void editTestTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmEditTestType frm = new frmEditTestType((clsTestTypes.enTestType)SelectedID);
            frm.ShowDialog();
            frmManageTestTypes_Load(null,null);
        }
    }
}
