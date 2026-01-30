using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Classess
{
    internal class clsUtil
    {
        public static string GenerateGUID()
        {
            return Guid.NewGuid().ToString();
        }
        public static bool CreateFolderIfNotExist(string DestinationFolder)
        {

            try
            {
                if (!Directory.Exists(DestinationFolder))
                {
                    Directory.CreateDirectory(DestinationFolder);
                    return true; 
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("حدث خطأ أثناء إنشاء المجلد: " + ex.Message);
                return false;
            }
            return true; ;
        }
        public static string ReplaceFileNameWithGUID(string sourceFile)
        {
            string filename = sourceFile;
            FileInfo fi = new FileInfo(filename);
            string ext = fi.Extension;
            return GenerateGUID() + ext;
        }
        public static bool CopyImageToProjectImagesFloder(ref string sourceFile)
        {
            string DestinationFolder = @"C:\\DVLD-Persons-Images\";
            if(!CreateFolderIfNotExist(DestinationFolder))
            {
                return false;
            }
            string destinationFile = DestinationFolder + ReplaceFileNameWithGUID(sourceFile);
            try
            {
                File.Copy(sourceFile, destinationFile, true);
            }
            catch(IOException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK,MessageBoxIcon.Error);
                return false;
            }
            sourceFile = destinationFile;
            return true;
        }
    }
}
