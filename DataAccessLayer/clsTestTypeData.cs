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
    public class clsTestTypesData
    {
        public static DataTable GetAllTestTypes()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from TestTypes";
            SqlCommand Command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                    Reader.Close();
                    Connection.Close();
                    return dt;
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

            return null;
        }
        public static int AddNewTestType(string Title, string Discription, float Fees)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO TestTypes (Title, Discription, Fees)
                     VALUES (@Title, @Discription ,@Fees);
                     SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Discription", Discription);
            Command.Parameters.AddWithValue("@Fees", Fees);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    TestID = insertedID;
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

            return TestID;
        }
        public static bool UpdateTestType(int TestID, string Title, string Discription ,float Fees)
        {
            int affectedrows = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"UPDATE TestTypes 
                             SET Title = @Title, 
                                 Discription = @Discription,
                                 Fees = @Fees 
                             WHERE TestID = @TestID";
            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@Title", Title);
            Command.Parameters.AddWithValue("@Discription", Discription);
            Command.Parameters.AddWithValue("@Fees", Fees);
            Command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                Connection.Open();
                affectedrows = Command.ExecuteNonQuery();
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return (affectedrows > 0);
        }
        public static bool GetTestByID(int TestID, ref string Title, ref string Discription,ref float Fees)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string query = @"select * from TestTypes where TestID = @TestID";

            SqlCommand Command = new SqlCommand(query, Connection);
            Command.Parameters.AddWithValue("@TestID", TestID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    Title = (string)Reader["Title"];
                    Discription = (string)Reader["Discription"];
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
