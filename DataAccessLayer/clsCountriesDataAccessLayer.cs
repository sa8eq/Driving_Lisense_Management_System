using System;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using DataAccessSettings;

namespace DataAccessLayer
{
    public class clsCountriesDataAccessLayer
    {
        static public DataTable GetAllCountries()
        {
            DataTable CountriesDTB = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "SELECT CountryID, CountryName FROM Countries";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    CountriesDTB.Load(Reader);   
                }
                Reader.Close();
                Connection.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

            }
            return CountriesDTB;
        }
        static public object GetCountryName(int CountryID)
        {
            object ItemToReturn = null;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select CountryName from Countries where CountryID = @CountryID";
            SqlCommand Command = new SqlCommand(Query,Connection);
            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if(result!=null)
                {
                    ItemToReturn = result;
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return ItemToReturn;
        }
    }
}
