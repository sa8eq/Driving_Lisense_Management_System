using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    

    public class clsTestTypes
    {
        public enum enMode { AddNew = 0, Update = 1}
        enMode _Mode = enMode.AddNew;

        public enum enTestType { VisionTest = 1, WrittenTest = 2, StreetTest = 3}

        public clsTestTypes.enTestType _ID { set; get; }
        public string _Title { set; get; }
        public string _Discreption { set; get; }
        public float _Fees { set; get; }

        public clsTestTypes()
        {
            this._ID = clsTestTypes.enTestType.VisionTest;
            this._Title = "";
            this._Discreption = "";
            this._Fees = 0;
            _Mode = enMode.AddNew;
        }

        public clsTestTypes(int TestID, string Title, string Discription, float Fees)
        {
            this._ID = (clsTestTypes.enTestType)TestID;
            this._Title = Title;
            this._Discreption = Discription;
            this._Fees = Fees;
            _Mode = enMode.Update;
        }

        private bool _AddNewTestType()
        {
            this._ID = (clsTestTypes.enTestType)clsTestTypesData.AddNewTestType(this._Title, this._Discreption, this._Fees);

            return (this._Title != "");
        }

        private bool _UpdateTestType()
        {
            return clsTestTypesData.UpdateTestType((int)this._ID, this._Title, this._Discreption, this._Fees);
        }


        public static DataTable GetAllTestTypes()
        {
            return clsTestTypesData.GetAllTestTypes();
        }

        public static clsTestTypes GetTestByID(clsTestTypes.enTestType TestID)
        {
            string Title = "", Discription = "";
            float Fees = 0;

            if(clsTestTypesData.GetTestByID((int)TestID, ref Title, ref Discription, ref Fees))
            {
                return new clsTestTypes((int)TestID, Title, Discription, Fees);
            }
            return null;
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.AddNew:
                    if(_AddNewTestType())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    return _UpdateTestType();
            }
            return false;
        }

    }
}
