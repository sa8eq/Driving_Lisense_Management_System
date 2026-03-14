using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLicenseClass
    {
        public enum enMode { AddNew =0, Update = 1}
        public enMode Mode = enMode.AddNew;


        public int LicenseClassID { get; set; }
        public string ClassName { get; set; }
        public string ClassDescription { get; set; }
        public byte MinimumAllowedAge { get; set; }
        public byte DefaultValidityLength { get; set; }
        public float ClassFees { get; set; }

        public clsLicenseClass()
        {
            this.LicenseClassID = -1;
            this.ClassName = "";
            this.ClassDescription = "";
            this.MinimumAllowedAge = 18;
            this.DefaultValidityLength = 10;
            this.ClassFees = 0;
            Mode = enMode.AddNew;
        }
        private clsLicenseClass(int ID, string Name, string Description, byte MinAge, byte Validity, float Fees)
        {
            this.LicenseClassID = ID;
            this.ClassName = Name;
            this.ClassDescription = Description;
            this.MinimumAllowedAge = MinAge;
            this.DefaultValidityLength = Validity;
            this.ClassFees = Fees;
            Mode = enMode.Update;
        }

        private bool _AddNewLicenseClass()
        {
            this.LicenseClassID = clsLicenseClassesData.AddNewLicenseClass(this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
            return (this.LicenseClassID != -1);
        }
        private bool _UpdateLicenseClass()
        {

            return clsLicenseClassesData.UpdateLicenseClass(this.LicenseClassID, this.ClassName, this.ClassDescription,
                this.MinimumAllowedAge, this.DefaultValidityLength, this.ClassFees);
        }

        public static clsLicenseClass Find(int LicenseClassID)
        {
            string Name = ""; string Description = "";
            byte MinAge = 0; byte Validity = 0; float Fees = 0;

            if (clsLicenseClassesData.GetLicenseClassInfoByID(LicenseClassID, ref Name, ref Description, ref MinAge, ref Validity, ref Fees))
                return new clsLicenseClass(LicenseClassID, Name, Description, MinAge, Validity, Fees);
            else
                return null;
        }
        public static clsLicenseClass Find(string ClassName)
        {
            int LicenseClassID = -1; string ClassDescription = "";
            byte MinimumAllowedAge = 18; byte DefaultValidityLength = 10; float ClassFees = 0;

            if (clsLicenseClassesData.GetLicenseClassInfoByClassName(ClassName, ref LicenseClassID, ref ClassDescription,
                    ref MinimumAllowedAge, ref DefaultValidityLength, ref ClassFees))

                return new clsLicenseClass(LicenseClassID, ClassName, ClassDescription,
                    MinimumAllowedAge, DefaultValidityLength, ClassFees);
            else
                return null;

        }

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassesData.GetAllLicensesClasses();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicenseClass())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicenseClass();

            }

            return false;
        }
    }
}