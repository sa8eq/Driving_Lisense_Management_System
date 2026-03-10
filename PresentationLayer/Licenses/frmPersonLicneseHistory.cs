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

namespace DVLD.Licenses
{
    public partial class frmPersonLicneseHistory : Form
    {
        int _LocalAppID;
        public frmPersonLicneseHistory(int LocalAppID)
        {
            InitializeComponent();
            _LocalAppID = LocalAppID;
        }

        private void frmPersonLicneseHistory_Load(object sender, EventArgs e)
        {
            clsLocalDrivingLicenseApplication LocalApp = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(_LocalAppID);
            clsDriver Driver = clsDriver.FindByPersonID(LocalApp._ApplicantPersonID);

            ctrlPersonDetailsWithFilter1.LoadPersonInfo(LocalApp._ApplicantPersonID);
            if (Driver != null)
            {
                ctrlLicneseHistory1.LoadDataWithDriverID(Driver.DriverID);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
