using BussinesLayer;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class clsLicense
{
    public enum enMode { AddNew = 0, Update = 1 };
    public enMode Mode = enMode.AddNew;
    public enum enIssueReason { FirstTime = 1, Renew = 2, ReplacementForDamaged = 3, ReplacementForLost = 4}
    public enIssueReason IssueReason = enIssueReason.FirstTime;
    public int LicenseID { get; set; }
    public int ApplicationID { get; set; }
    clsApplication Application;
    public int DriverID { get; set; }
    clsDriver Driver;
    public int LicenseClass { get; set; }
    public DateTime IssueDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Notes { get; set; }
    public float PaidFees { get; set; }
    public bool IsActive { get; set; }
    public int CreatedByUserID { get; set; }
    clsUser CreatedByUser;

    public clsLicense()
    {
        this.LicenseID = -1;
        this.ApplicationID = -1;
        this.DriverID = -1;
        this.LicenseClass = -1;
        this.IssueDate = DateTime.Now;
        this.ExpirationDate = DateTime.Now;
        this.Notes = "";
        this.PaidFees = 0;
        this.IsActive = true;
        this.IssueReason = (enIssueReason)1;
        this.CreatedByUserID = -1;

        Mode = enMode.AddNew;
    }

    private clsLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass,
        DateTime IssueDate, DateTime ExpirationDate, string Notes, float PaidFees,
        bool IsActive, byte IssueReason, int CreatedByUserID)
    {
        this.LicenseID = LicenseID;
        this.ApplicationID = ApplicationID;
        Application = clsApplication.FindBaseApplication(ApplicationID);
        this.DriverID = DriverID;
        Driver = clsDriver.FindByDriverID(DriverID);
        this.LicenseClass = LicenseClass;
        this.IssueDate = IssueDate;
        this.ExpirationDate = ExpirationDate;
        this.Notes = Notes;
        this.PaidFees = PaidFees;
        this.IsActive = IsActive;
        this.IssueReason = (enIssueReason)IssueReason;
        this.CreatedByUserID = CreatedByUserID;
        CreatedByUser = clsUser.FindByUserID(CreatedByUserID);

        Mode = enMode.Update;
    }

    private bool _AddNewLicense()
    {
        
        this.LicenseID = clsLicensesData.AddNewLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,
            (byte)this.IssueReason, this.CreatedByUserID);

        return (this.LicenseID != -1);
    }

    private bool _UpdateLicense()
    {
        return clsLicensesData.UpdateLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass,
            this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees, this.IsActive,
            (byte)this.IssueReason, this.CreatedByUserID);
    }

    public static clsLicense Find(int LicenseID)
    {
        int ApplicationID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
        DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        string Notes = "";
        float PaidFees = 0;
        bool IsActive = true;
        byte IssueReason = 1;

        if (clsLicensesData.GetLicenseInfoByID(LicenseID, ref ApplicationID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes, ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
        {
            return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);
        }
        else
            return null;
    }

    public bool Save()
    {
        switch (Mode)
        {
            case enMode.AddNew:
                if (_AddNewLicense())
                {
                    Mode = enMode.Update;
                    return true;
                }
                else
                    return false;

            case enMode.Update:
                return _UpdateLicense();
        }
        return false;
    }

    public static bool DeleteLicense(int LicenseID)
    {
        return clsLicensesData.DeleteLicense(LicenseID);
    }

    public static bool IsLicenseExist(int LicenseID)
    {
        return clsLicensesData.IsLicenseExist(LicenseID);
    }

    public static DataTable GetAllLicenses()
    {
        return clsLicensesData.GetAllLicenses();
    }
    public static bool IsLicneseExistByPersonID(int PersonID, int LicenseClassID)
    {
        return clsLicensesData.IsLicenseExistByPersonID(PersonID, LicenseClassID);
    }

    public static clsLicense FindLicneseByApplicationID(int ApplicationID)
    {
        int LicenseID = -1, DriverID = -1, LicenseClass = -1, CreatedByUserID = -1;
        DateTime IssueDate = DateTime.Now, ExpirationDate = DateTime.Now;
        string Notes = "";
        float PaidFees = 0;
        bool IsActive = true;
        byte IssueReason = 1;

        if (clsLicensesData.GetLicenseInfoByApplicationID(ApplicationID,
            ref LicenseID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByUserID))
        {
            return new clsLicense(LicenseID, ApplicationID, DriverID, LicenseClass,
                                  IssueDate, ExpirationDate, Notes, PaidFees,
                                  IsActive, IssueReason, CreatedByUserID);
        }
        else
        {
            return null;
        }
    }
}