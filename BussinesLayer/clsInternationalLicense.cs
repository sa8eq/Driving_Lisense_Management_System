using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    
    public class clsInternationalLicense
    {
        public enum enMode { AddNew = 1, Update = 2}
        private enMode Mode = enMode.AddNew;

        public int _InternationalLicenseID { set; get; }
        public int _ApplicationID { set; get; }
        public int _DriverID { set; get; }
        public int _IssuedUsingLocalLicenseID { set; get; }
        public DateTime _IssueDate { set; get; }
        public DateTime _ExpirationDate { set; get; }
        public bool _IsActive { set; get; }
        public int _CreatedByUserID { set; get; }
        public clsApplication _Application;
        public clsDriver _Driver;
        public clsLicense _LocalLicense;
        public clsUser _CreatedByUser;

        public clsInternationalLicense()
        {
            this._InternationalLicenseID = -1;
            this._ApplicationID = -1;
            this._DriverID = -1;
            this._IssuedUsingLocalLicenseID = -1;
            this._IssueDate = DateTime.Now;
            this._ExpirationDate = DateTime.Now;
            this._IsActive = true;
            this._CreatedByUserID = -1;
            this.Mode = enMode.AddNew;
        }
        public clsInternationalLicense(int InternationalLicenseID, int ApplicationID, int DriverID, 
            int IssuedUsingLocalLicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByUserID)
        {
            this._InternationalLicenseID = InternationalLicenseID;
            this._ApplicationID = ApplicationID;
            this._DriverID = DriverID;
            this._IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this._IssueDate = IssueDate;
            this._ExpirationDate = ExpirationDate;
            this._IsActive = IsActive;
            this._CreatedByUserID = CreatedByUserID;
            this.Mode = enMode.Update;
            _Application = clsApplication.FindBaseApplication(ApplicationID);
            _Driver = clsDriver.FindByDriverID(DriverID);
            _LocalLicense = clsLicense.Find(IssuedUsingLocalLicenseID);
            _CreatedByUser = clsUser.FindByUserID(CreatedByUserID);
        }

        private bool _AddNewInternationalLicense()
        {
            clsApplication App = new clsApplication();
             ;
            App._ApplicantPersonID = clsLicense.Find(this._IssuedUsingLocalLicenseID).DriverInfo.PersonID;
            App._ApplicationDate = this._IssueDate;
            App._ApplicationTypeID = (int)clsApplication.enApplicationType.NewInternationalLicense;
            App._Status = clsApplication.enStatus.Completed;
            App._LastStatusDate = this._IssueDate;
            App._PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense)._Fees;
            App._UserID = this._CreatedByUserID;
            if (App.Save())
            {
                this._InternationalLicenseID = clsInternationalLicensesData.AddNewInternationalLicense(App._ApplicationID,
                    this._DriverID, this._IssuedUsingLocalLicenseID, this._IssueDate, this._ExpirationDate, this._IsActive, this._CreatedByUserID);
            }

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
                                                                           this._CreatedByUserID);
    
        }
        public bool Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateInternationalLicense();
                default:
                    return false;
            }
        }
        public static DataTable GetAllInterNationalLicenses()
        {
            return clsInternationalLicensesData.GetAllInternationalLicenses();
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1, DriverID = -1, IssuedUsingLocalLicenseID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if (clsInternationalLicensesData.GetInternationalLicenseInfoByID(InternationalLicenseID,
                ref ApplicationID, ref DriverID, ref IssuedUsingLocalLicenseID,
                ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID,
                    IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            }
            else
                return null;
        }

        public static clsInternationalLicense FindByLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            int InternationalLicenseID = -1, ApplicationID = -1, DriverID = -1, CreatedByUserID = -1;
            DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
            bool IsActive = false;

            if (clsInternationalLicensesData.GetInternationalLicenseInfoByLocalLicenseID(IssuedUsingLocalLicenseID,
                ref InternationalLicenseID, ref ApplicationID, ref DriverID,
                ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByUserID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID,
                    IssuedUsingLocalLicenseID, IssueDate, ExpirationDate, IsActive, CreatedByUserID);
            }
            else
                return null;
        }

        public static bool IsDriverHasActiveInternationalLicense(int DriverID)
        {
          
            return (clsInternationalLicensesData.FindActiveInternationalLicenseIDByDriverID(DriverID) != -1);
        }

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicensesData.GetDriverInternationalLicenses(DriverID);
        }
    }
}
