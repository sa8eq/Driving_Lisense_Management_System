using BussinesLayer;
using System;
using System.IO;

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
            try
            {
                if (!Directory.Exists(_FolderName))
                {
                    Directory.CreateDirectory(_FolderName);
                }

                if (string.IsNullOrEmpty(Username))
                {
                    if (File.Exists(_FilePath))
                        File.Delete(_FilePath);
                    return true;
                }

                string dataToSave = Username + "#//#" + Password;
                File.WriteAllText(_FilePath, dataToSave);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool GetStoredCredentials(ref string Username, ref string Password)
        {
            try
            {
                if (File.Exists(_FilePath))
                {
                    string fileContent = File.ReadAllText(_FilePath);
                    string[] result = fileContent.Split(new string[] { "#//#" }, StringSplitOptions.None);

                    if (result.Length == 2)
                    {
                        Username = result[0];
                        Password = result[1];
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}