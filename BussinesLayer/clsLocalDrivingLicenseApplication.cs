using BusinessLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class clsLocalDrivingLicenseApplication : clsApplication
    {
        public int LocalDrivingLicenseApplicationID { get; set; }
        public int LicenseClassID { get; set; }

        public clsLicenseClass LicenseClassInfo;

        public clsLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = -1;
            this.LicenseClassID = -1;
            this._Mode = enMode.AddNew;
        }

        static public DataTable GetAllLocalDrivingLicensesApplications()
        {
            return clsLocalDrivingLicenseApplicationData.GetAllLocalDrivingLicensesApplications();
        }

        public new bool Save()
        {

            if (!base.Save()) return false;
            base._Mode = enMode.AddNew;
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLocalDrivingLicenseApplication())
                    {
                        this._Mode = enMode.Update; 
                        return true;
                    }
                    return false;

                case enMode.Update:
                    return true;
            }
            return false;
        }

        public static bool IsThereAnActiveApplication(int PersonID, int AppTypeID, int LicenseClassID)
        {
            return clsLocalDrivingLicenseApplicationData.IsThereAnActiveApplication(PersonID, AppTypeID, LicenseClassID);
        }

        private bool _AddNewLocalDrivingLicenseApplication()
        {
            this.LocalDrivingLicenseApplicationID = clsLocalDrivingLicenseApplicationData.AddNewLocalDrivingLicenseApplication(
            this._ApplicationID, this.LicenseClassID);

            return (this.LocalDrivingLicenseApplicationID != -1);
        }
    }
}
