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
using DVLD.Persons;
namespace DVLD
{
    public partial class frmManagePeople : Form
    {
        private static DataTable _dtAllPersons = clsPerson.GetAllPersons();

        private DataTable _dtPersons = _dtAllPersons.DefaultView.ToTable(false, "PersonID", "NationalNumber", "FirstName", "SecondName", "ThirdName", "LastName",
                                                                                 "GenderCaption", "BirthDate", "CountryName", "Phone", "Email", "Address");
        public frmManagePeople()
        {
            InitializeComponent();
        }
        private void _RefreshPersonsList()
        {
            _dtAllPersons = clsPerson.GetAllPersons();
            _dtPersons = _dtAllPersons.DefaultView.ToTable(false, "PersonID", "NationalNumber", "FirstName",
                                                                   "SecondName", "ThirdName", "LastName", "GenderCaption",
                                                                   "BirthDate", "CountryName", "Phone", "Email", "Address");

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _dtPersons;
            dataGridView1.AllowUserToAddRows = false;
            lblRecords.Text = _dtPersons.Rows.Count.ToString();
            cmbFilter.SelectedIndex = 0;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "Person ID";
                dataGridView1.Columns[0].Width = 100;

                dataGridView1.Columns[1].HeaderText = "National Number";
                dataGridView1.Columns[1].Width = 140;

                dataGridView1.Columns[2].HeaderText = "First Name";
                dataGridView1.Columns[2].Width = 100;

                dataGridView1.Columns[3].HeaderText = "Second Name";
                dataGridView1.Columns[3].Width = 100;

                dataGridView1.Columns[4].HeaderText = "Third Name";
                dataGridView1.Columns[4].Width = 100;

                dataGridView1.Columns[5].HeaderText = "Last Name";
                dataGridView1.Columns[5].Width = 100;

                dataGridView1.Columns[6].HeaderText = "Gender";
                dataGridView1.Columns[6].Width = 70;

                dataGridView1.Columns[7].HeaderText = "Date Of Birth";
                dataGridView1.Columns[7].Width = 100;

                dataGridView1.Columns[8].HeaderText = "Nationality";
                dataGridView1.Columns[8].Width = 100;

                dataGridView1.Columns[9].HeaderText = "Phone";
                dataGridView1.Columns[9].Width = 110;

                dataGridView1.Columns[10].HeaderText = "Email";
                dataGridView1.Columns[10].Width = 136;

                dataGridView1.Columns[11].HeaderText = "Address";
                dataGridView1.Columns[11].Width = 130;
            }

            
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Visible = (cmbFilter.Text != "None");
 
            if (txtFilter.Visible)
            {
                txtFilter.Text = "";
                txtFilter.Focus();
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.DataSource = _dtPersons;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPersonsList();
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm = new frmAddEditPerson();
            frm.ShowDialog();
            _RefreshPersonsList();
        }
        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch(cmbFilter.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "National Number":
                    FilterColumn = "NationalNumber";
                    break;
                case "First Name":
                    FilterColumn = "FirstName";
                    break;
                case "Second Name":
                    FilterColumn = "SecondName";
                    break;
                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;
                case "Last Name":
                    FilterColumn = "LastName";
                    break;
                case "Nationality":
                    FilterColumn = "CountryName";
                    break;
                case "Gender":
                    FilterColumn = "GenderCaption";
                    break;
                case "Phone":
                    FilterColumn = "Phone";
                    break;
                case "Email":
                    FilterColumn = "Email";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }
            if(txtFilter.Text.Trim()== ""|| txtFilter.Text.Trim() == "None")
            {
                _dtPersons.DefaultView.RowFilter = "";
                lblRecords.Text = dataGridView1.Rows.Count.ToString();
                return;
            }
            if(FilterColumn == "PersonID")
            {
                _dtPersons.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
            }
            else
            {
                _dtPersons.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }
            lblRecords.Text = dataGridView1.Rows.Count.ToString();

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
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int PersonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);
                if (MessageBox.Show("Are You Sure You Want To Delete This Person?", "Delete Person", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (clsPerson.DeletePerson(PersonID))
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
            _RefreshPersonsList();
        }
        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(SelectedID);
            
            frm.ShowDialog();

        }
        private void edToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["PersonID"].Value;
            frmAddEditPerson frm = new frmAddEditPerson(SelectedID);
            frm.ShowDialog();
           
            _RefreshPersonsList();
        }
    }
}
