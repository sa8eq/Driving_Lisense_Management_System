using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        
        static public DataTable GetAllPersons()
        {
            return clsPersonsDataAccessLayer.GetAllPersons();
        }
        static public DataTable GetAllPersonsWithFilter(string Where, string EqualTo)
        {
            return clsPersonsDataAccessLayer.GetAllPersonsWithFilter(Where, EqualTo);
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
        static public bool DeletePerson(int ID)
        {
            return clsPersonsDataAccessLayer.DeletePerson(ID);
        }
        public int Save()
        {
            switch(Mode)
            {
                case enMode.AddNew:
                    return AddNewPerson();
                    break;
                case enMode.Update:
                    return -1;
                    break;
                default:
                    return -1;
            }
        }
    }
}
