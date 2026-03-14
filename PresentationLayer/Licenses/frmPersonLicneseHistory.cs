using BussinesLayer;
using DVLD.Licenses.Control;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses
{
    public partial class frmPersonLicneseHistory : Form
    {
        int _PersonID;

        public frmPersonLicneseHistory()
        {
            InitializeComponent();
        }

        public frmPersonLicneseHistory(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmPersonLicneseHistory_Load(object sender, EventArgs e)
        {
            if (_PersonID != -1)
            {
                ctrlPersonDetailsWithFilter1.LoadPersonInfo(_PersonID);
                ctrlPersonDetailsWithFilter1.FilterEnabled = false;
                ctrlLicneseHistory1.LoadInfoByPersonID(_PersonID);
            }
            else
            {
                ctrlPersonDetailsWithFilter1.Enabled = true;
                ctrlPersonDetailsWithFilter1.FilterFocus();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPersonDetailsWithFilter1_OnPersonSelected(int obj)
        {
            _PersonID = obj;
            if(_PersonID==-1)
            {
                ctrlLicneseHistory1.Clear();
            }
            else
            {
                ctrlLicneseHistory1.LoadInfoByPersonID(_PersonID);
            }
        }
    }
}
