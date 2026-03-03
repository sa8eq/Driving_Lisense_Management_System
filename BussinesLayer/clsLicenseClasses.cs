using System;
using System.Data;
using DataAccessLayer;

namespace BusinessLayer
{
    public class clsLicenseClass
    {
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
        }

        private clsLicenseClass(int ID, string Name, string Description, byte MinAge, byte Validity, float Fees)
        {
            this.LicenseClassID = ID;
            this.ClassName = Name;
            this.ClassDescription = Description;
            this.MinimumAllowedAge = MinAge;
            this.DefaultValidityLength = Validity;
            this.ClassFees = Fees;
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

        public static DataTable GetAllLicenseClasses()
        {
            return clsLicenseClassesData.GetAllLicensesClasses();
        }
    }
}