using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BussinesLayer
{
    public class clsHashing
    {
        public static string ComputeHash(string Password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] HashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(Password));
                return BitConverter.ToString(HashedBytes).Replace("-", "").ToLower();
            }
        }
        public static bool CompareHash(string OriginalData, string HashedData)
        {
            return (ComputeHash(OriginalData) == HashedData);
        }
    }
}
