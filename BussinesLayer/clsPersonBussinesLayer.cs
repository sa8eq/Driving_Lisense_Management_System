using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BussinesLayer
{
    public class clsPersonBussinesLayer
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;
        public int _ID { get; set; }
        public string _NationalNumber { get; set; }
        public string _FirstName { get; set; }
        public string _SecondName { get; set; }
        public string _ThirdName { get; set; }
        public string _LastName { get; set; }
        public byte _Gender { get; set; }
        public DateTime _BirthDate { get; set; }
        public string _Address { get; set; }
        public string _Phone { get; set; }
        public string _Email { get; set; }
        public int _CountryID { get; set; }
        public string _ImagePath { get; set; }

        public clsPersonBussinesLayer(string NationalNumber, string FirstName, string SecondName,
            string ThirdName, string LastName, byte Gender, DateTime BirthDate,
            string Address, string Phone, string Email, int CountryID, string ImagePath)
        {
            _NationalNumber = NationalNumber;
            _FirstName = FirstName;
            _SecondName = SecondName;
            _ThirdName = ThirdName;
            _LastName = LastName;
            _Gender = Gender;
            _BirthDate = BirthDate;
            _Address = Address;
            _Phone = Phone;
            _Email = Email;
            _CountryID = CountryID;
            _ImagePath = ImagePath;

        }
        public clsPersonBussinesLayer()
        {
            this._ID = -1;
            this._NationalNumber = "";
            this._FirstName = "";
            this._SecondName = "";
            this._ThirdName = "";
            this._LastName = "";
            this._Gender = 0;
            this._BirthDate = DateTime.Now;
            this._Address = "";
            this._Phone = "";
            this._Email = "";
            this._CountryID = 0;
            this._ImagePath = "";
            this.Mode = enMode.AddNew;
        }
        static public DataTable GetAllPersons()
        {
            return clsPersonsDataAccessLayer.GetAllPersons();
        }
        static public DataTable GetAllPersonsWithFilter(string Where, string EqualTo)
        {
            return clsPersonsDataAccessLayer.GetAllPersonsWithFilter(Where, EqualTo);
        }
        static public clsPersonBussinesLayer Find(string NationalNumber)
        {
            // استخدم دالتك الحالية لجلب البيانات
            DataTable dt = GetAllPersonsWithFilter("NationalNumber", NationalNumber);

            if (dt.Rows.Count > 0)
            {
                // استخرج الـ ID من الجدول الذي عاد
                int personID = (int)dt.Rows[0]["PersonID"];

                // استخدم الدالة الموجودة أصلاً عندك لجلب الكائن كاملاً
                return GetPersonInfoByID(personID);
            }

            return null;
        }
        static public bool IsExist(string Where, string EqualTo)
        {
            return (GetAllPersonsWithFilter(Where, EqualTo).Rows.Count!=0);
        }
        static public int GetRowsCount()
        {
            return clsPersonsDataAccessLayer.GetRowsCount();
        }
        private int AddNewPerson()
        {
            this._ID = clsPersonsDataAccessLayer.AddNewPerson(_NationalNumber, _FirstName,
                _SecondName, _ThirdName, _LastName, _Gender, _BirthDate,
                _Address, _Phone, _Email, _CountryID, _ImagePath);

            if(this._ID>0)
            {
                this.Mode = enMode.Update;
            }
            return this._ID;
        }
        private bool UpdatePerson()
        {
            return clsPersonsDataAccessLayer.UpdatePerson(this._ID , _NationalNumber, _FirstName,
                _SecondName, _ThirdName, _LastName, _Gender, _BirthDate,
                _Address, _Phone, _Email, _CountryID, _ImagePath);
        }
        static public bool DeletePerson(int ID)
        {
            return clsPersonsDataAccessLayer.DeletePerson(ID);
        }
        static public clsPersonBussinesLayer GetPersonInfoByID(int PersonID)
        {
            DataTable dt = clsPersonsDataAccessLayer.GetPersonInfoByID(PersonID);
            clsPersonBussinesLayer Person = new clsPersonBussinesLayer();
            
            if(dt.Rows.Count>0)
            {
                DataRow row = dt.Rows[0];
                Person._ID = PersonID;
                Person._FirstName = row["FirstName"].ToString();
                Person._SecondName = row["SecondName"].ToString();
                Person._ThirdName = row["ThirdName"].ToString();
                Person._LastName = row["LastName"].ToString();
                Person._NationalNumber = row["NationalNumber"].ToString();
                Person._Email = row["Email"].ToString();
                Person._Address = row["Address"].ToString();
                Person._Phone = row["Phone"].ToString();
                Person._ImagePath = (row["ImagePath"] != DBNull.Value ? row["ImagePath"].ToString():"");
                Person._BirthDate = (DateTime)row["BirthDate"];
                Person._CountryID = (int)row["NationalityID"];
                Person._Gender = Convert.ToByte(row["Gender"]);
                Person.Mode = enMode.Update;
                return Person;
            }

            return null;
        }
        public bool Save(ref string Message)
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    if(AddNewPerson()!=-1)
                    {
                        Message = $"Person Has Been Added successfully With ID {this._ID}";
                        this.Mode = enMode.Update;
                        return true;
                    }
                    break;
                case enMode.Update:
                    if (UpdatePerson())
                    {
                        Message = $"The Person With ID {this._ID} + Has Been Updated Successfully";
                        return true;
                    }
                    break;
                default:
                    return false;
            }
            return false;
        }
    }
}
