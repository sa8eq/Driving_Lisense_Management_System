using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace BussinesLayer
{
    public class clsUser
    {
        public enum enMode { AddNewUser =0, UpdateUser = 1}
        public enMode Mode = enMode.AddNewUser;

        public int UserID { set; get; }

        public int PersonID { set; get; }
        public clsPerson PersonInfo;

        public string Username { set; get; } 

        public string Password { set; get; }

        public bool IsActive { set; get; }

        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.Username = "";
            this.Password = "";
            this.IsActive = false;
            Mode = enMode.AddNewUser;
        }
        private clsUser(int UserID, int PersonID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsPerson.Find(this.PersonID);
            Mode = enMode.UpdateUser;
        }

        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.Username, this.Password, this.IsActive);
            return (this.UserID) != -1;

        }
        private bool _UpdateUser()
        {
            return clsUserData.UpdateUser(this.UserID, this.PersonID, this.Username, this.Password, this.IsActive);
        }
        static public clsUser FindByUserID(int UserID)
        {
            int PersonID = 0;
            string Username = "";
            string Password = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByUserID(UserID, ref PersonID, ref Username, ref Password, ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, Username, Password, IsActive);

            }
            else
            {
                return null;
            }
        }
        static public clsUser FindByPErsonID(int PersonID)
        {
            int UserID = 0;
            string Username = "";
            string Password = "";
            bool IsActive = false;
            bool IsFound = clsUserData.GetUserInfoByPersonID(PersonID, ref UserID, ref Username, ref Password, ref IsActive);
            if (IsFound)
            {
                return new clsUser(UserID, PersonID, Username, Password, IsActive);

            }
            else
            {
                return null;
            }
        }
        static public clsUser FindUserByUserNameAndPassword(string Username, string Password)
        {
            int _UserID = -1;
            int _PersonID = -1;
            bool _IsActive = false;

            bool IsFound = clsUserData.GetUserInfoByUsernameAndPassword(Username, Password ,ref _UserID, ref _PersonID, ref _IsActive);
            if(IsFound)
            {
                return new clsUser(_UserID, _PersonID, Username, Password, _IsActive);
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
                case enMode.AddNewUser:
                    if (_AddNewUser())
                    {
                        this.Mode = enMode.UpdateUser;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.UpdateUser:
                    return _UpdateUser();

                default:
                    return false;
            }
        }
        static public DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID);
        }
        public static bool IsUserExist(string Username)
        {
            return clsUserData.IsUserExist(Username);
        }
        public static bool IsUserExist(int UserID)
        {
            return clsUserData.IsUserExist(UserID);
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistForPersonID(PersonID);
        }
        
        
    }
}
