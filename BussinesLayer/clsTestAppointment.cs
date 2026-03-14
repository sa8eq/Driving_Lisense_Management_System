using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class clsTestAppointment
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int _TestAppointmentID { set; get; }

        public clsTestTypes.enTestType _TestTypeID { get; set; }

        public int _LocalDrivingLicneseApplicationID { set; get; }

        public DateTime _AppointmentDate { set; get; }

        public float _PaidFees { set; get; }

        public int _CreatedByUserID { set; get; }

        public bool _IsLocked { set; get; }

        public int _RetakeTestApplicationID { set; get; }

        public clsApplication _RetakeApplication;

        public int TestID
        {
            get { return _GetTestID(); }

        }

        public clsTestAppointment()
        {
            this._TestAppointmentID = -1;
            this._TestTypeID = clsTestTypes.enTestType.VisionTest;
            this._AppointmentDate = DateTime.Now;
            this._PaidFees = 0;
            this._CreatedByUserID = -1;
            this._RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;
        }

        public clsTestAppointment(int TestAppointmentID, clsTestTypes.enTestType TestTypeID,
           int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees,
           int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)

        {
            this._TestAppointmentID = TestAppointmentID;
            this._TestTypeID = TestTypeID;
            this._LocalDrivingLicneseApplicationID = LocalDrivingLicenseApplicationID;
            this._AppointmentDate = AppointmentDate;
            this._PaidFees = PaidFees;
            this._CreatedByUserID = CreatedByUserID;
            this._IsLocked = IsLocked;
            this._RetakeTestApplicationID = RetakeTestApplicationID;
            this._RetakeApplication = clsApplication.FindBaseApplication(RetakeTestApplicationID);
            Mode = enMode.Update;
        }


        private bool _AddNewTestAppointment()
        {
            this._TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment((int)this._TestTypeID, this._LocalDrivingLicneseApplicationID, this._AppointmentDate,
                this._PaidFees, this._CreatedByUserID, this._IsLocked, this._RetakeTestApplicationID);

            return (this._TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData.UpdateTestAppointment(this._TestAppointmentID,(int) this._TestTypeID, this._LocalDrivingLicneseApplicationID, this._AppointmentDate,
                this._PaidFees, this._CreatedByUserID, this._IsLocked, this._RetakeTestApplicationID);
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = 1; int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsTestAppointmentsData.GetTestAppointmentInfoByID(TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
            ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, (clsTestTypes.enTestType)TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID)
        {
            int TestAppointmentID = -1;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int CreatedByUserID = -1; bool IsLocked = false; int RetakeTestApplicationID = -1;

            if (clsTestAppointmentsData.GetLastTestAppointment(LocalDrivingLicenseApplicationID, (int)TestTypeID,
                ref TestAppointmentID, ref AppointmentDate, ref PaidFees, ref CreatedByUserID, ref IsLocked, ref RetakeTestApplicationID))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID);
            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();
        }

        public DataTable GetApplicationTestAppointmentsPerTestType(clsTestTypes.enTestType TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestAppointmentsPerTestType(this._LocalDrivingLicneseApplicationID, (int)TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID, clsTestTypes.enTestType TestTypeID)
        {
            return clsTestAppointmentsData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTestAppointment();
            }
            return false;
        }

        private int _GetTestID()
        {
            return clsTestAppointmentsData.GetTestID(_TestAppointmentID);
        }
    }
}
