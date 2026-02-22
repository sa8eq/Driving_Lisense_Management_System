using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class clsApplicationType
    {
        public enum enMode { Addnew = 0, Update =1}
        public enMode Mode = enMode.Addnew;
        public int _ID { set; get; }
        public string _Title { set; get; }
        public float _Fees { set; get; }
        public clsApplicationType()
        {
            _ID = 0;
            _Title = "";
            _Fees = 0;
            Mode = enMode.Addnew;
        }
        public clsApplicationType(int ID, string Title, float Fees)
        {
            _ID = ID;
            _Title = Title;
            _Fees = Fees;
            Mode = enMode.Update;
        }
        private bool _AddNewApplicationType()
        {
            this._ID = clsApplicationTypesData.AddNewApplicationType(this._Title, this._Fees);

            return (this._ID != -1);
        }
        private bool _UpdateApplicationType()
        {
            return clsApplicationTypesData.UpdateApplicationType(this._ID, this._Title, this._Fees);
        }

        static public DataTable GetAllApplicationType()
        {
            return clsApplicationTypesData.GetAllApplicationTypes();
        }
        static public clsApplicationType Find(int ID)
        {
            string Title = "";
            float Fees = 0;

            if(clsApplicationTypesData.GetApplicationByID(ID, ref Title, ref Fees))
            {
                return new clsApplicationType(ID, Title, Fees);
            }
            return null;
        }

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Addnew:
                    if(_AddNewApplicationType())
                    {
                        Mode = enMode.Update;
                        return true;
                    };
                    break;
                case enMode.Update:
                    return _UpdateApplicationType();
                    
            }
            return false;
        }
    }
}
