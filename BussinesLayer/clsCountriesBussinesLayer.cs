using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
namespace PersonsBussinesLayer
{
    public class clsCountriesBussinesLayer
    {
       static public DataTable GetCountries()
       {
            return clsCountriesDataAccessLayer.GetAllCountries();
       }
       static public object GetCountryName(int CountryID)
       {
            return clsCountriesDataAccessLayer.GetCountryName(CountryID);
       }
    }
}
