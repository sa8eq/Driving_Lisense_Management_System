using BussinesLayer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DVLD.Classess
{
    public static class clsGlobal
    {
        public static clsUser CurrentUser;

        // تحديد المسار الجديد
        private static string _FolderName = @"C:\DVLD_Data";
        private static string _FileName = "Credentials.txt";
        private static string _FilePath = Path.Combine(_FolderName, _FileName);

        public static bool RememberUserNameAndPassword(string Username, string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string regUsername = "Username";
            string regPassword = "Password";
            try
            {

                Registry.SetValue(keyPath, regUsername,Username,RegistryValueKind.String);
                Registry.SetValue(keyPath, regPassword, Password, RegistryValueKind.String);
                

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool GetStoredCredentials(ref string Username, ref string Password)
        {
            string keyPath = @"HKEY_CURRENT_USER\SOFTWARE\DVLD";
            string regUsername = "Username";
            string regPassword = "Password";
            try
            {
                Username = Registry.GetValue(keyPath, regUsername, null) as string;
                Password = Registry.GetValue(keyPath, regPassword, null) as string;
                
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public static string HashPassword(string Password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] HashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return BitConverter.ToString(HashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}