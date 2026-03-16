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
    public class clsDetainedLicenseData
    {

        public static int AddNewDetainedLicense(int LicenseID, DateTime DetainDate,
                float FineFees, int CreatedByUserID)
        {
            int DetainID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO DetainedLicenses 
                            (LicenseID, DetainDate, FineFees, CreatedByUserID, IsReleased)
                            VALUES 
                            (@LicenseID, @DetainDate, @FineFees, @CreatedByUserID, 0);
                            SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && int.TryParse(Result.ToString(), out int insertedID))
                {
                    DetainID = insertedID;
                }
            }
            catch (Exception ex) { }
            finally { Connection.Close(); }

            return DetainID;
        }

        public static bool ReleaseDetainedLicense(int DetainID, int ReleasedByUserID, int ReleaseApplicationID)
        {
            int rowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE DetainedLicenses 
                            SET IsReleased = 1, 
                                ReleaseDate = @ReleaseDate, 
                                ReleasedByUserID = @ReleasedByUserID, 
                                ReleaseApplicationID = @ReleaseApplicationID
                            WHERE DetainID = @DetainID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@DetainID", DetainID);
            Command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

            try
            {
                Connection.Open();
                rowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex) { return false; }
            finally { Connection.Close(); }

            return (rowsAffected > 0);
        }

        public static DataTable GetAllDetainedLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM DetainedLicenses_View";

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows) dt.Load(Reader);
                Reader.Close();
            }
            catch (Exception ex) { }
            finally { Connection.Close(); }

            return dt;
        }

        public static bool IsLicenseDetained(int LicenseID)
        {
            bool IsDetained = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT IsDetained = 1 FROM DetainedLicenses WHERE LicenseID = @LicenseID AND IsReleased = 0";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null) IsDetained = true;
            }
            catch (Exception ex) { }
            finally { Connection.Close(); }

            return IsDetained;
        }

        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID,
        ref int DetainID, ref DateTime DetainDate, ref float FineFees,
    ref int CreatedByUserID, ref bool IsReleased, ref DateTime ReleaseDate,
    ref int ReleasedByUserID, ref int ReleaseApplicationID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT * FROM DetainedLicenses WHERE LicenseID = @LicenseID And IsReleased = 0";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;
                    DetainID = (int)Reader["DetainID"];
                    DetainDate = (DateTime)Reader["DetainDate"];
                    FineFees = Convert.ToSingle(Reader["FineFees"]);
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                    IsReleased = (bool)Reader["IsReleased"]; 

                    ReleaseDate = (Reader["ReleaseDate"] == DBNull.Value) ? DateTime.MinValue : (DateTime)Reader["ReleaseDate"];
                    ReleasedByUserID = (Reader["ReleasedByUserID"] == DBNull.Value) ? -1 : (int)Reader["ReleasedByUserID"];
                    ReleaseApplicationID = (Reader["ReleaseApplicationID"] == DBNull.Value) ? -1 : (int)Reader["ReleaseApplicationID"];
                }
                Reader.Close();
            }
            catch (Exception ex) { isFound = false; }
            finally { Connection.Close(); }

            return isFound;
        }

        
    }
}
