using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BussinesLayer;
namespace PresentationLayer
{
    public partial class frmManagePeople : Form
    {
        
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Enabled = true;
            txtFilter.Visible = true;
            if (cmbFilter.Text.ToString()=="None")
            {
                txtFilter.Text = "";
                txtFilter.Enabled = false;
                txtFilter.Visible = false;
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.DataSource = clsPersonBussinesLayer.GetAllPersons();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void RefreshForm()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = clsPersonBussinesLayer.GetAllPersons();
            lblRecords.Text = clsPersonBussinesLayer.GetRowsCount().ToString();
            cmbFilter.SelectedIndex = 0;
        }


        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewPerson();
            frm.ShowDialog();
            this.RefreshForm();
        }
        private void _FilterView()
        {
            if (cmbFilter.Text != "None" && !string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                dataGridView1.DataSource = clsPersonBussinesLayer.GetAllPersonsWithFilter(cmbFilter.Text, txtFilter.Text);
            }
            else
            {
                dataGridView1.DataSource = clsPersonBussinesLayer.GetAllPersons();
            }
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _FilterView();
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Phone Call Feature Is Yet To Be Implemented");
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Sending Email Feature Is Yet To Be Implemented");
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddNew_Click(sender,e);
        }
        private void _DeletePerson()
        {
            if (dataGridView1.CurrentRow != null)
            {
                int PersonID = (int)dataGridView1.CurrentRow.Cells[0].Value;
                if (MessageBox.Show("Are You Sure You Want To Delete This Person?", "Delete Person", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (clsPersonBussinesLayer.DeletePerson(PersonID))
                    {
                        MessageBox.Show("Person With ID " + PersonID.ToString() + " Has Been Deleted Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed To Delete Person With ID " + PersonID.ToString());
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Choose A Person To Delete");
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _DeletePerson();
            RefreshForm();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;
            frmPersonDetails frm = new frmPersonDetails(SelectedID);
            
            frm.ShowDialog();

        }

        private void edToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;
            frmUpdatePerson frm = new frmUpdatePerson(SelectedID);
            frm.ShowDialog();
           
            RefreshForm();
        }
    }
}
