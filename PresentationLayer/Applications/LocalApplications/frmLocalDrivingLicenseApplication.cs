using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.LocalApplications
{
    public partial class frmLocalDrivingLicenseApplication : Form
    {
        private int ApplicationID = -1;
      
        public frmLocalDrivingLicenseApplication(int ID)
        {
            InitializeComponent();
            ApplicationID = ID;
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            ctrlApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(ApplicationID);
            

        }
    }
}
