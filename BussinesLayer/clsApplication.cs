using DataAccessLayer;
using DataAccessSettings;
using System;
using System.Data;
using System.Data.SqlClient;

namespace BussinesLayer
{
    public class clsApplication
    {
        public enum enMode { AddNew = 0, Update = 1 }
        public enum enStatus { New = 1, Cancelled = 2, Completed = 3 }
        public enum enApplicationType { NewDrivingLicense = 1, RenewDrivingLicense = 2, ReplaceLostDrivingLicense = 3,  ReplaceDamagedDrivingLicense = 4, 
        ReleaseDetainedDrivingLicense = 5, NewInternationalLicense = 6, RetakeTest = 7}

        public enMode _Mode = enMode.AddNew;

        public int _ApplicationID { set; get; }

        public int _ApplicantPersonID { set; get; }
        public clsPerson _PersonInfo { get; set; }

        public string ApplicantFullName 
        {
            get
            {
                return clsPerson.Find(_ApplicantPersonID).FullName;
            }
        }

        public DateTime _ApplicationDate { set; get; }

        public int _ApplicationTypeID { set; get; }
        public clsApplicationType _ApplicationTypeInfo;

        public enStatus _Status { set; get; }

        public string StatusText 
        { 
            get
            {
                switch(_Status)
                {
                    case enStatus.New:
                        return "New";
                    case enStatus.Cancelled:
                        return "Cancelled";
                    case enStatus.Completed:
                        return "Completed";
                    default:
                        return "Unknown";
                }
            }
        }

        public DateTime _LastStatusDate { set; get; }

        public float _PaidFees { set; get; }

        public int _UserID { set; get; }

        public clsUser _CreatedByUserInfo;



        public clsApplication()
        {
            this._ApplicationID = -1;
            this._ApplicantPersonID = -1;
            this._ApplicationDate = DateTime.Now;
            this._ApplicationTypeID = -1;
            this._Status = enStatus.New;
            this._LastStatusDate = DateTime.Now;
            this._PaidFees = 0;
            this._UserID = -1;
            this._Mode = enMode.AddNew;
        }

        public clsApplication(int ApplicationID, int PersonID, DateTime ApplicationDate, int ApplicationTypeID,
            enStatus Status, DateTime LastStatusDate, float PaidFees, int UserID)
        {
            this._ApplicationID = ApplicationID;
            this._ApplicantPersonID = PersonID;
            this._PersonInfo = clsPerson.Find(PersonID);
            this._ApplicationDate = ApplicationDate;
            this._ApplicationTypeID = ApplicationTypeID;
            this._ApplicationTypeInfo = clsApplicationType.Find(ApplicationTypeID);
            this._Status = Status;
            this._LastStatusDate = LastStatusDate;
            this._PaidFees = PaidFees;
            this._UserID = UserID;
            this._CreatedByUserInfo = clsUser.FindByUserID(UserID);
            this._Mode = enMode.Update;
        }
        
        public bool _AddNewApplication()
        {
            this._ApplicationID = clsApplicationData.AddNewApplication(
                this._ApplicantPersonID, this._ApplicationDate, this._ApplicationTypeID,
                (byte)this._Status, this._LastStatusDate, this._PaidFees, this._UserID);

            return (this._ApplicationID != -1);
        }

        public bool _UpdateApplication()
        {
            return clsApplicationData.UpdateApplication(
                this._ApplicationID, (byte)this._Status, this._LastStatusDate);
        }

        public static clsApplication FindBaseApplication(int ApplicationID)
        {
            int ApplicantPersonID = -1, CreatedByUserID = -1, ApplicationTypeID = -1;
            float PaidFees = -1; byte ApplicationStatus = 1;
            DateTime ApplicationDate = DateTime.Now, LastStatusDate = DateTime.Now ;


            if(clsApplicationData.GetApplicationInfoByID(ApplicationID,ref ApplicantPersonID,ref ApplicationDate,ref ApplicationTypeID,ref ApplicationStatus,ref LastStatusDate,ref PaidFees,ref CreatedByUserID))
            {
                return new clsApplication(ApplicationID, ApplicantPersonID, ApplicationDate, ApplicationTypeID, (enStatus)ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
            }
            else
            {
                return null;
            }
        }

        public bool Cancel()
        {
            return clsApplicationData.UpdateStatus(_ApplicationID, 2);
        }

        public bool SetComplete()
        {
            return clsApplicationData.UpdateStatus(_ApplicationID, 3);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewApplication())
                    {

                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateApplication();

            }

            return false;
        }

        public bool Delete()
        {
            return clsApplicationData.DeleteApplication(this._ApplicationID);
        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            return clsApplicationData.IsApplicationExist(ApplicationID);
        }

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
            return clsApplicationData.DoesPersonHaveActiveApplication(PersonID, ApplicationTypeID);
        }

        public bool DoesPersonHaveActiveApplication(int ApplicationTypeID)
        {
           return clsApplicationData.DoesPersonHaveActiveApplication(this._ApplicantPersonID,ApplicationTypeID);
        }

        public static int GetActiveApplicationID(int PersonID, clsApplication.enApplicationType ApplicationType)
        {
            return clsApplicationData.GetActiveApplicationID(PersonID,(int)ApplicationType );
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, clsApplication.enApplicationType ApplicationTypeID, int LicenseClassID)
        {
            return clsApplicationData.GetActiveApplicationIDForLicenseClass(PersonID, (int)ApplicationTypeID, LicenseClassID);
        }

        public int GetActiveApplicationID(clsApplication.enApplicationType ApplicationTypeID)
        {
            return GetActiveApplicationID(this._ApplicantPersonID, ApplicationTypeID);
        }
    }
}