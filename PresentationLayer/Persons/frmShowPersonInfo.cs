using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Persons
{
    public partial class frmShowPersonInfo : Form
    {
        public frmShowPersonInfo(int PersonID)
        {
            InitializeComponent();
            ctrlPersonDetails1.LoadPersonInfo(PersonID);
        }
        public frmShowPersonInfo(string NationalNumber)
        {
            InitializeComponent();
            ctrlPersonDetails1.LoadPersonInfo(NationalNumber);
        }
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
