using System;
using System.Data;
using System.Net;
using System.Data.SqlClient;
using DataAccessSettings;

namespace DataAccessLayer
{
    public class clsCountriesData
    {
        public static bool GetCountryInfoByID(int CountryID, ref string CountryName)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select CountryName from Countries where CountryID = @CountryID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null)
                {
                    CountryName = result.ToString();
                }
                else
                {
                    
                    IsFound = false;
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw;
                
            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }

        public static bool GetCountryInfoByName(ref int CountryID, string CountryName)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select CountryID from Countries where CountryName = @CountryName";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryName", CountryName);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null)
                {
                    int.TryParse(result.ToString(), out CountryID);
                }
                else
                {

                    IsFound = false;
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw;

            }
            finally
            {

                Connection.Close();
            }
            return IsFound;
        }

        static public DataTable GetAllCountries()
        {
            DataTable CountriesDTB = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "SELECT * FROM Countries order by CountryName";
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
                throw;
            }
            finally
            {
                Connection.Close();
            }
            return CountriesDTB;
        }   
    }
}
