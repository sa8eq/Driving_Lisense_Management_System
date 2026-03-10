using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class clsDriver
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int DriverID { get; set; }
        public int PersonID { get; set; }
        public clsPerson Person; 
        public int CreatedByUserID { get; set; }
        public clsUser CreatedByUser;
        public DateTime CreatedDate { get; set; }

        public clsDriver()
        {
            this.DriverID = -1;
            this.PersonID = -1;
            this.CreatedByUserID = -1;
            this.CreatedDate = DateTime.Now;

            Mode = enMode.AddNew;
        }

        private clsDriver(int DriverID, int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            Person = clsPerson.Find(PersonID);
            this.CreatedByUserID = CreatedByUserID;
            CreatedByUser = clsUser.FindByUserID(CreatedByUserID);
            this.CreatedDate = CreatedDate;

            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            this.DriverID = clsDriverData.AddNewDriver(this.PersonID, this.CreatedByUserID, this.CreatedDate);
            return (this.DriverID != -1);
        }

        
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            int PersonID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static clsDriver FindByPersonID(int PersonID)
        {
            int DriverID = -1;
            int CreatedByUserID = -1;
            DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID(PersonID, ref DriverID, ref CreatedByUserID, ref CreatedDate))
            {
                return new clsDriver(DriverID, PersonID, CreatedByUserID, CreatedDate);
            }
            else
            {
                return null;
            }
        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();
        }

        public static bool DeleteDriver(int DriverID)
        {
            return clsDriverData.DeleteDriver(DriverID);
        }

        public static bool IsDriverExistByPersonID(int PersonID)
        {
            return clsDriverData.IsDriverExistByPersonID(PersonID);
        }
    }
}
