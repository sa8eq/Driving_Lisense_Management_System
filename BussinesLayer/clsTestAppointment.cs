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
        public int _TestTypeID { set; get; }
        public clsTestTypes _TestTypeInfo;
        public int _LocalDrivingLicneseApplicationID { set; get; }
        public clsLocalDrivingLicenseApplication _LocalDrivingLicneseApplicationInfo;
        public DateTime _AppointmentDate { set; get; }
        public float _PaidFees { set; get; }
        public int _CreatedByUserID { set; get; }
        public clsUser _CreatedByUserInfo;
        public bool _IsLocked { set; get; }
        public int _RetakeTestApplicationID { set; get; }
        public clsApplication _RetakeApplication;
        public clsTestAppointment()
        {
            _TestAppointmentID = -1;
            _TestTypeID = -1;
            _LocalDrivingLicneseApplicationID = -1;
            _AppointmentDate = DateTime.Now;
            _PaidFees = -1;
            _CreatedByUserID = -1;
            _IsLocked = false;
            _RetakeTestApplicationID = -1;
            Mode = enMode.AddNew;
        }
        public clsTestAppointment(int TestAppointmentID, int TestTypeID, int LocalDrivingLicenseApplicationID,
                                    DateTime AppointmentDate, float PaidFees, int CreatedByUserID, bool IsLocked, int RetakeTestApplicationID)
        {
            this._TestAppointmentID = TestAppointmentID;
            this._TestTypeID = TestTypeID;
            this._LocalDrivingLicneseApplicationID = LocalDrivingLicenseApplicationID;
            this._AppointmentDate = AppointmentDate;
            this._PaidFees = PaidFees;
            this._CreatedByUserID = CreatedByUserID;
            this._IsLocked = IsLocked;
            this._RetakeTestApplicationID = RetakeTestApplicationID;
            this._TestTypeInfo = clsTestTypes.GetTestByID((clsTestTypes.enTestType)TestTypeID);
            this._LocalDrivingLicneseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingApplicationID(LocalDrivingLicenseApplicationID);
            this._CreatedByUserInfo = clsUser.FindByUserID(CreatedByUserID);
            this._RetakeApplication = clsApplication.FindBaseApplication(_RetakeTestApplicationID);


            Mode = enMode.Update;
        }
        private bool _AddNewTestAppointment()
        {
            this._TestAppointmentID = clsTestAppointmentsData.AddNewTestAppointment(this._TestTypeID, this._LocalDrivingLicneseApplicationID, this._AppointmentDate,
                this._PaidFees, this._CreatedByUserID, this._IsLocked, this._RetakeTestApplicationID);

            return (this._TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            return clsTestAppointmentsData.UpdateTestAppointment(this._TestAppointmentID, this._TestTypeID, this._LocalDrivingLicneseApplicationID, this._AppointmentDate,
                this._PaidFees, this._CreatedByUserID, this._IsLocked, this._RetakeTestApplicationID);
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

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentsData.GetAllTestAppointments();
        }

        public bool Delete(int TestAppointmentID)
        {
            return clsTestAppointmentsData.DeleteTestAppointment(TestAppointmentID);
        }

        public static clsTestAppointment FindByTestAppointmentID(int TestAppointmentID)
        {
            // تعريف المتغيرات لاستقبال البيانات من طبقة البيانات
            int TestTypeID = -1;
            int LocalDrivingLicenseApplicationID = -1;
            DateTime AppointmentDate = DateTime.Now;
            float PaidFees = 0;
            int CreatedByUserID = -1;
            bool IsLocked = false;
            int RetakeTestApplicationID = -1;

            // استدعاء دالة طبقة البيانات (يجب أن تكون قد أنشأت هذه الدالة في clsTestAppointmentData)
            bool IsFound = clsTestAppointmentsData.GetTestAppointmentInfoByID
                (
                    TestAppointmentID, ref TestTypeID, ref LocalDrivingLicenseApplicationID,
                    ref AppointmentDate, ref PaidFees, ref CreatedByUserID,
                    ref IsLocked, ref RetakeTestApplicationID
                );

            if (IsFound)
            {
                // إرجاع كائن جديد من الكلاس محملاً بالبيانات
                return new clsTestAppointment(
                    TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
                    AppointmentDate, PaidFees, CreatedByUserID, IsLocked, RetakeTestApplicationID
                );
            }
            else
            {
                return null;
            }
        }

        public static bool IsTestAppointmentExist(int TestAppointmentID)
        {
            return clsTestAppointmentsData.IsTestAppointmentExist(TestAppointmentID);
        }
    }
}
