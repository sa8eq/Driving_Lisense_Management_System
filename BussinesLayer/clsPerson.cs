using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using PersonsBussinesLayer;
namespace BussinesLayer
{
    public class clsPerson
    {
        private enum enMode { AddNew = 0, Update = 1 };
        private enMode Mode = enMode.AddNew;

        public int PersonID { get; set; }
        
        public string NationalNumber { get; set; }
        
        public string FirstName { get; set; }
        
        public string SecondName { get; set; }
        
        public string ThirdName { get; set; }
        
        public string LastName { get; set; }
        
        public short Gender { get; set; }
        
        public DateTime BirthDate { get; set; }
        
        public string Address { get; set; }
        
        public string Phone { get; set; }
        
        public string Email { get; set; }
        
        public int CountryID { get; set; }
        
        public clsCountry CountryInfo;

        private string _ImagePath;

        public string ImagePath 
        {
            get {return _ImagePath; }
            set {_ImagePath = value; } 
        }

        public clsPerson()
        {
            this.PersonID = -1;
            this.NationalNumber = "";
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.Gender = 0;
            this.BirthDate = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.CountryID = -1;
            this.ImagePath = "";
            this.Mode = enMode.AddNew;
        }
        
        public clsPerson(int PersonID,string NationalNumber, string FirstName, string SecondName,
            string ThirdName, string LastName, short Gender, DateTime BirthDate, string Address,
            string Phone, string Email, int CountryID, string ImagePath)
        {
            this.PersonID = PersonID;
            this.NationalNumber = NationalNumber;
            this.FirstName = FirstName;
            this.SecondName = SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.Gender = Gender;
            this.BirthDate = BirthDate;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.CountryID = CountryID;
            this.ImagePath = ImagePath;
            this.CountryInfo = clsCountry.Find(CountryID);
            this.Mode = enMode.Update;

        }

        private bool _AddNewPerson()
        {
            this.PersonID = clsPersonData.AddNewPerson(this.NationalNumber, this.FirstName,
                this.SecondName, this.ThirdName, this.LastName, this.Gender, this.BirthDate,
                this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);

            return (this.PersonID)!=-1;
        }

        private bool _UpdatePerson()
        {
            return clsPersonData.UpdatePerson(this.PersonID, this.NationalNumber, this.FirstName,
                this.SecondName, this.ThirdName, this.LastName, this.Gender, this.BirthDate,
                this.Address, this.Phone, this.Email, this.CountryID, this.ImagePath);
        }

        public static clsPerson Find(int PersonID)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", NationalNumber = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime BirthDate = DateTime.Now;
            short Gender = -1;
            int CountryID = -1;

            bool IsFound = clsPersonData.GetPersonInfoByID(PersonID, ref NationalNumber, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                                                        ref BirthDate, ref Gender, ref Address, ref Phone,
                                                                       ref Email, ref CountryID, ref ImagePath);
            if(IsFound)
            {
                return new clsPerson(PersonID, NationalNumber, FirstName, 
                                     SecondName, ThirdName, LastName,
                                     Gender, BirthDate, Address, Phone,
                                     Email, CountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public static clsPerson Find(string NationalNumber)
        {
            int PersonID = -1;
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Address = "", Phone = "", Email = "", ImagePath = "";
            DateTime BirthDate = DateTime.Now;
            short Gender = -1;
            int CountryID = -1;

            bool IsFound = clsPersonData.GetPersonInfoByNationalNumber(NationalNumber, ref PersonID, ref FirstName, ref SecondName, ref ThirdName, ref LastName,
                                                                        ref BirthDate, ref Gender, ref Address, ref Phone,
                                                                       ref Email, ref CountryID, ref ImagePath);
            if (IsFound)
            {
                return new clsPerson(PersonID, NationalNumber, FirstName, 
                                     SecondName, ThirdName, LastName,
                                     Gender, BirthDate, Address, Phone,
                                     Email, CountryID, ImagePath);
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {
                        this.Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case enMode.Update:
                    if (_UpdatePerson())
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
            }
            return false;
        }

        static public DataTable GetAllPersons()
        {
            return clsPersonData.GetAllPersons();
        }

        static public bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID);
        }

        static public bool IsPersonExist(int ID)
        {
            return clsPersonData.IsPersonExist(ID);
        }

        static public bool IsPersonExist(string NationalNumber)
        {
            return clsPersonData.IsPersonExist(NationalNumber);
        }
    }
}
