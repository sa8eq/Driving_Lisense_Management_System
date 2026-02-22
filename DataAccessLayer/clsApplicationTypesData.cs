using DataAccessSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class clsApplicationTypesData
    {
        public static DataTable GetAllApplicationTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from ApplicationTypes";
            SqlCommand Command = new SqlCommand(query, Connection);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dt.Load(Reader);
                    Reader.Close();
                    Connection.Close();
                    return dt;
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

            return null;
        }
        public static int AddNewApplicationType(string Title, float Fees)
        {
            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO ApplicationTypes (Title, Fees)
                     VALUES (@Title, @Fees);
                     SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ApplicationID = insertedID;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
            finally
            {
                Connection.Close();
            }

            return ApplicationID;
        }
        public static bool UpdateApplicationType(int ApplicationID, string Title, float Fees)
        {
            int affectedrows = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE ApplicationTypes 
                             SET Title = @Title, 
                                 Fees = @Fees 
                             WHERE ApplicationID = @ApplicationID";
            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Fees", Fees);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            try
            {
                Connection.Open();
                affectedrows = Command.ExecuteNonQuery();
                Connection.Close();
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return (affectedrows > 0);
        }
        public static bool GetApplicationByID(int ApplicationID, ref string Title, ref float Fees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from ApplicationTypes where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID",  ApplicationID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    Title = (string)Reader["Title"];
                    Fees = Convert.ToSingle(Reader["Fees"]);
                    Reader.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return false;
        }
    }
}
