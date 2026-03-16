using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    
    public class clsInternationalLicense: clsApplication
    {
        public enum enMode { AddNew = 1, Update = 2}
        private enMode Mode = enMode.AddNew;
        public clsDriver _Driver;
        public int _InternationalLicenseID { set; get; }
        public int _DriverID { set; get; }
        public int _IssuedUsingLocalLicenseID { set; get; }
        public DateTime _IssueDate { set; get; }
        public DateTime _ExpirationDate { set; get; }
        public bool _IsActive { set; get; }
        public clsInternationalLicense()

        {
            this._ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;

            this._InternationalLicenseID = -1;
            this._DriverID = -1;
            this._IssuedUsingLocalLicenseID = -1;
            this._IssueDate = DateTime.Now;
            this._ExpirationDate = DateTime.Now;

            this._IsActive = true;


            Mode = enMode.AddNew;

        }
        public clsInternationalLicense(int ApplicationID, int ApplicantPersonID,
            DateTime ApplicationDate,
             enStatus ApplicationStatus, DateTime LastStatusDate,
             float PaidFees, int CreatedByUserID,
             int InternationalLicenseID, int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate, bool IsActive)

        {
            base._ApplicationID = ApplicationID;
            base._ApplicantPersonID = ApplicantPersonID;
            base._ApplicationDate = ApplicationDate;
            base._ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            base._Status = ApplicationStatus;
            base._LastStatusDate = LastStatusDate;
            base._PaidFees = PaidFees;
            base._UserID = CreatedByUserID;

            this._InternationalLicenseID = InternationalLicenseID;
            this._ApplicationID = ApplicationID;
            this._DriverID = DriverID;
            this._IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this._IssueDate = IssueDate;
            this._ExpirationDate = ExpirationDate;
            this._IsActive = IsActive;
            this._UserID = CreatedByUserID;

            this._Driver = clsDriver.FindByDriverID(this._DriverID);

            Mode = enMode.Update;
        }
        private bool _AddNewInternationalLicense()
        {
            this._InternationalLicenseID =
               clsInternationalLicensesData.AddNewInternationalLicense(this._ApplicationID, this._DriverID, this._IssuedUsingLocalLicenseID,
              this._IssueDate, this._ExpirationDate,
              this._IsActive, this._UserID);


            return (this._InternationalLicenseID != -1);
        }
        private bool _UpdateInternationalLicense()
        {
            return clsInternationalLicensesData.UpdateInternationalLicense(
                                                                           this._InternationalLicenseID,
                                                                           this._ApplicationID,
                                                                           this._DriverID,
                                                                           this._IssuedUsingLocalLicenseID,
                                                                           this._IssueDate,
                                                                           this._ExpirationDate,
                                                                           this._IsActive,
                                                                           this._UserID);
    
        }
        public static DataTable GetAllInterNationalLicenses()
        {
            return clsInternationalLicensesData.GetAllInternationalLicenses();
        }
        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true; int CreatedByUserID = 1;

            if (clsInternationalLicensesData.GetInternationalLicenseInfoByID(InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssuedUsingLocalLicenseID,
            ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                clsApplication Application = clsApplication.FindBaseApplication(ApplicationID);


                return new clsInternationalLicense(Application._ApplicationID,
                    Application._ApplicantPersonID,
                                     Application._ApplicationDate,
                                    (enStatus)Application._Status, Application._LastStatusDate,
                                     Application._PaidFees, Application._UserID,
                                     InternationalLicenseID, DriverID, IssuedUsingLocalLicenseID,
                                         IssueDate, ExpirationDate, IsActive);

            }

            else
                return null;
        }
        public bool Save()
        {
            if (!base.Save())
                return false;

            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }
        public static int GetDriverActiveInternationalLicense(int DriverID)
        {

            return clsInternationalLicensesData.FindActiveInternationalLicenseIDByDriverID(DriverID);
        }
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicensesData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
