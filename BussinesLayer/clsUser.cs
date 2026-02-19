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
        public enMode Mode;
        public int UserID { set; get; }
        public int PersonID { set; get; }
        public string Username { set; get; } 
        public string Password { set; get; }
        public bool IsActive { set; get; }
        public clsPerson PersonInfo;
        public clsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.Username = "";
            this.Password = "";
            this.IsActive = false;
            Mode = enMode.AddNewUser;
        }
        public clsUser(int UserID, int ParsonID, string Username, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.Username = Username;
            this.Password = Password;
            this.IsActive = IsActive;
            this.PersonInfo = clsPerson.Find(this.PersonID);
            Mode = enMode.UpdateUser;
        }
        static public clsUser FindUser(string Username, string Password)
        {
            int _UserID = -1;
            int _PersonID = -1;
            bool _IsActive = false;

            bool IsFound = clsUserData.FindUserByUserName(Username, Password ,ref _UserID, ref _PersonID, ref _IsActive);
            if(IsFound)
            {
                return new clsUser(_UserID, _PersonID, Username, Password, _IsActive);
            }
            else
            {
                return null;
            }
        }
        static public DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        private bool _AddNewUser()
        {
            this.UserID = clsUserData.AddNewUser(this.PersonID, this.Username, this.Password, this.IsActive);
            return (this.UserID) != -1;
           
        }
        //private bool _UpdateUser()
        //{

        //}
        public bool Save()
        {
            switch(Mode)
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
                //case enMode.UpdateUser:
                //    if(_UpdateUser())
                //    {
                //        return true;
                //    }
                //    else
                //    {
                //        return false;
                //    }
                default:
                    return false;
            }
        }
        public static bool IsUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUSerExistForPersonID(PersonID);
        }
        public static bool IsUserExist(string Username)
        {
            return clsUserData.IsUserExist(Username);
        }
    }
}
