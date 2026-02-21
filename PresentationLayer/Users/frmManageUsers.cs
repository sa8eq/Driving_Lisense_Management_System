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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Persons
{
    public partial class frmManageUsers : Form
    {
        private DataTable _dtAllUsers;
        public frmManageUsers()
        {
            InitializeComponent();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _RefrshForm()
        {
            cmbIsActive.Enabled = false;
            cmbIsActive.Visible = false;
            txtFilter.Enabled = false;
            txtFilter.Visible = false;

            _dtAllUsers = clsUser.GetAllUsers();
            dataGridView1.DataSource = _dtAllUsers;
            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns[0].HeaderText = "User ID";
                dataGridView1.Columns[1].HeaderText = "Person ID";
                dataGridView1.Columns[2].HeaderText = "Full Name";
                dataGridView1.Columns[3].HeaderText = "Username";
                dataGridView1.Columns[4].HeaderText = "Password";
                dataGridView1.Columns[5].HeaderText = "Is Active";
                dataGridView1.Columns[2].Width = 250;
            }
            cmbFilter.SelectedIndex = 0;
            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            _RefrshForm();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";

            switch (cmbFilter.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;
                case "Username":
                    FilterColumn = "Username";
                    break;
                case "Password":
                    FilterColumn = "Password";
                    break;
                default:
                    FilterColumn = "None";
                    break;
            }

            if (string.IsNullOrWhiteSpace(txtFilter.Text) || FilterColumn == "None")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dataGridView1.Rows.Count.ToString();
                return;
            }

            string filterExpression = "";

            if (FilterColumn == "UserID" || FilterColumn == "PersonID")
            {
                if (int.TryParse(txtFilter.Text.Trim(), out int temp))
                {
                    filterExpression = string.Format("[{0}] = {1}", FilterColumn, txtFilter.Text.Trim());
                }
                else
                {
                    _dtAllUsers.DefaultView.RowFilter = "";
                    return;
                }
            }
            else
            {
                filterExpression = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilter.Text.Trim());
            }

            _dtAllUsers.DefaultView.RowFilter = filterExpression;
            lblRecordsCount.Text = dataGridView1.Rows.Count.ToString();
        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbFilter.SelectedIndex == 0)
            {
                cmbIsActive.Enabled = false;
                cmbIsActive.Visible = false;
                txtFilter.Enabled = false;
                txtFilter.Visible = false;

            }
            else if (cmbFilter.SelectedIndex == 5)
            {
                cmbIsActive.Enabled = true;
                cmbIsActive.Visible = true;
                txtFilter.Enabled = false;
                txtFilter.Visible = false;
                cmbIsActive.SelectedIndex = 0;
            }
            else
            {
                cmbIsActive.Enabled = false;
                cmbIsActive.Visible = false;
                txtFilter.Enabled = true;
                txtFilter.Visible = true;
            }


        }

        private void cmbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsActive";
            switch (cmbIsActive.Text)
            {
                case "Yes":
                    _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = 1", FilterColumn);
                    break;
                case "No":
                    _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = 0", FilterColumn);
                    break;
                case "All":
                    _dtAllUsers.DefaultView.RowFilter = "";
                    break;
                default:
                    _dtAllUsers.DefaultView.RowFilter = "";
                    break;
            }
            lblRecordsCount.Text = dataGridView1.Rows.Count.ToString();
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            frmAddEditUser frm = new frmAddEditUser();
            frm.ShowDialog();

            _RefrshForm();
        }

        private void sendSMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Yet To Be Implemented");
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This Feature Is Yet To Be Implemented");
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnAddUser_Click(sender, e);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frm = new frmUserInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["UserID"].Value));
            frm.Show();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                int UserID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["UserID"].Value);
                if (MessageBox.Show("Are You Sure You Want To Delete This User?", "Delete User", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (clsUser.DeleteUser(UserID))
                    {
                        MessageBox.Show("User Deleted Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Failed Deleting User");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please Choose A User To Delete");
            }
            _RefrshForm();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["UserID"].Value;
            frmAddEditUser frm = new frmAddEditUser(SelectedID);
            frm.ShowDialog();

            _RefrshForm();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedID = (int)dataGridView1.CurrentRow.Cells["UserID"].Value;
            frmChangePassword frm = new frmChangePassword(SelectedID);
            frm.ShowDialog();

            _RefrshForm();
        }
    }
}
