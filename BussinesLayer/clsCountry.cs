using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace PersonsBussinesLayer
{
    public class clsCountry
    {
        public int CountryID { set; get; }
        public string CountryName { set; get; }
        public clsCountry()
        {
            CountryID = -1;
            CountryName = "";
        }
        public clsCountry(int CountryID, string CountryName)
        {
            this.CountryID = CountryID;
            this.CountryName = CountryName;
        }
        public static clsCountry Find(int CountryID)
        {
            string CountryName = "";
            if(clsCountriesData.GetCountryInfoByID(CountryID, ref CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        public static clsCountry Find(string CountryName)
        {
            int CountryID = -1;
            if(clsCountriesData.GetCountryInfoByName(ref CountryID, CountryName))
            {
                return new clsCountry(CountryID, CountryName);
            }
            else
            {
                return null;
            }
        }
        static public DataTable GetCountries()
        {
            return clsCountriesData.GetAllCountries();
        }
        
    }
}
