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

namespace DVLD
{
    public partial class ctrlPersonDetailsWithFilter : UserControl
    {
        public event Action<int> OnPersonSelected;
        protected virtual void PersonSelected(int PersonID)
        {
            Action<int> handler = OnPersonSelected;
            if(handler!=null)
            {
                handler(PersonID);
            }
        }
        public ctrlPersonDetailsWithFilter()
        {
            InitializeComponent();
        }
        private bool _ShowAddPerson = true;
        public bool ShowAddPerson
        {
            get
            {
                return _ShowAddPerson;
            }
            set
            {
                _ShowAddPerson = value;
                btnAddPerson.Visible = true;
            }
        }
        private bool _FilterEnabled = true;
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilter.Enabled = _FilterEnabled;
            }
        }
        private int _PersonID = -1;
        public int PersonID
        {
            get { return ctrlPersonDetails1.PersonID; }
        }
        public clsPerson SelectedPersonInfo
        {
            get { return ctrlPersonDetails1.SelectedPersonInfo; }
        }
        public void LoadPersonInfo(int PersonID)
        {
            cmbFilter.SelectedIndex = 1;
            txtFilter.Text = PersonID.ToString();
            FindNow();
        }
        private void FindNow()
        {
            switch (cmbFilter.Text)
            {
                case "PersonID":
                    ctrlPersonDetails1.LoadPersonInfo(int.Parse(txtFilter.Text));
                    break;
                case "NationalNumber":
                    ctrlPersonDetails1.LoadPersonInfo(txtFilter.Text);
                    break;
                default:
                    break;

            }
            if (OnPersonSelected!=null && FilterEnabled)
            {
                OnPersonSelected(ctrlPersonDetails1.PersonID);
            }
        }
        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilter.Text = "";
            txtFilter.Focus();
        }
        private void btnFind_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("Some Fields Are Not Valid");
                return;
            }
            FindNow();
        }
        private void ctrlPersonDetailsWithFilter_Load(object sender, EventArgs e)
        {
            cmbFilter.SelectedIndex = 0;
            txtFilter.Focus();
        }
        private void txtFilter_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilter.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilter,"This Field Is Required");
            }
            else
            {
                errorProvider1.SetError(txtFilter, null);
            }
        }
        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPerson frm1 = new frmAddEditPerson();
            frm1.DataBack += DataBackEvent;
            frm1.ShowDialog();
        }
        private void DataBackEvent(object sender, int PersonID)
        {
            cmbFilter.SelectedIndex = 1;
            txtFilter.Text = PersonID.ToString();
            ctrlPersonDetails1.LoadPersonInfo(PersonID);
        }
        public void FilterFocus()
        {
            txtFilter.Focus();
        }
        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
            if(cmbFilter.Text == "PersonID")
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }
    }
}
