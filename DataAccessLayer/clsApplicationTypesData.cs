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
                clsLogging.ErrorLogExceptions(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return null;
        }
        public static int AddNewApplicationType(string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO ApplicationTypes (ApplicationTypeTitle, ApplicationTypeFees)
                     VALUES (@ApplicationTypeTitle, @ApplicationTypeFees);
                     SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationTypeFees", ApplicationTypeFees);

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
                clsLogging.ErrorLogExceptions(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return ApplicationID;
        }
        public static bool UpdateApplicationType(int ApplicationTypeID, string ApplicationTypeTitle, float ApplicationTypeFees)
        {
            int affectedrows = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE ApplicationTypes 
                             SET ApplicationTypeTitle = @ApplicationTypeTitle, 
                                 ApplicationTypeFees = @ApplicationTypeFees 
                             WHERE ApplicationTypeID = @ApplicationTypeID";
            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);
            Command.Parameters.AddWithValue("@ApplicationTypeFees", ApplicationTypeFees);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                Connection.Open();
                affectedrows = Command.ExecuteNonQuery();
                Connection.Close();
            }
            catch(Exception ex)
            {
                clsLogging.ErrorLogExceptions(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return (affectedrows > 0);
        }
        public static bool GetApplicationByID(int ApplicationTypeID, ref string ApplicationTypeTitle, ref float ApplicationTypeFees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from ApplicationTypes where ApplicationTypeID = @ApplicationTypeID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    ApplicationTypeTitle = (string)Reader["ApplicationTypeTitle"];
                    ApplicationTypeFees = Convert.ToSingle(Reader["ApplicationTypeFees"]);
                    Reader.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                clsLogging.ErrorLogExceptions(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return false;
        }
    }
}
