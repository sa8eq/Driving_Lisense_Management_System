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
    public class clsLicensesData
    {
        public static DataTable GetAllLicenses()
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Licenses";

            SqlCommand Command = new SqlCommand(query, Connection);

            try
            {
                Connection.Open();

                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }

                Reader.Close();
            }
            catch (Exception ex)
            {
                

            }
            finally
            {
                Connection.Close();
            }

            return dt;
        }

        public static int AddNewLicense(int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                                     string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int InsertedID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO Licenses 
                     (ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID)
                     VALUES 
                     (@ApplicationID, @DriverID, @LicenseClass, @IssueDate, @ExpirationDate, @Notes, @PaidFees, @IsActive, @IssueReason, @CreatedByUserID);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (string.IsNullOrEmpty(Notes))
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@Notes", Notes);

            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int id))
                {
                    InsertedID = id;
                }
            }
            catch (Exception ex)
            {
                InsertedID = -1;
            }
            finally
            {
                Connection.Close();
            }

            return InsertedID;

        }

        public static bool UpdateLicense(int LicenseID, int ApplicationID, int DriverID, int LicenseClass, DateTime IssueDate, DateTime ExpirationDate,
                                 string Notes, float PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE Licenses 
                     SET ApplicationID = @ApplicationID,
                         DriverID = @DriverID,
                         LicenseClass = @LicenseClass,
                         IssueDate = @IssueDate,
                         ExpirationDate = @ExpirationDate,
                         Notes = @Notes,
                         PaidFees = @PaidFees,
                         IsActive = @IsActive,
                         IssueReason = @IssueReason,
                         CreatedByUserID = @CreatedByUserID
                     WHERE LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

            if (string.IsNullOrEmpty(Notes))
                Command.Parameters.AddWithValue("@Notes", DBNull.Value);
            else
                Command.Parameters.AddWithValue("@Notes", Notes);

            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool DeleteLicense(int LicenseID)
        {
            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "DELETE FROM Licenses WHERE LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);
        }

        public static bool GetLicenseInfoByID(int LicenseID, ref int ApplicationID, ref int DriverID, ref int LicenseClass,
    ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes, ref float PaidFees,
    ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "SELECT * FROM Licenses WHERE LicenseID = @LicenseID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.Read())
                {
                    isFound = true;

                    ApplicationID = (int)Reader["ApplicationID"];
                    DriverID = (int)Reader["DriverID"];
                    LicenseClass = (int)Reader["LicenseClass"];
                    IssueDate = (DateTime)Reader["IssueDate"];
                    ExpirationDate = (DateTime)Reader["ExpirationDate"];

                    if (Reader["Notes"] != DBNull.Value)
                        Notes = (string)Reader["Notes"];
                    else
                        Notes = "";

                    PaidFees = Convert.ToSingle(Reader["PaidFees"]);
                    IsActive = (bool)Reader["IsActive"];
                    IssueReason = (byte)Reader["IssueReason"];
                    CreatedByUserID = (int)Reader["CreatedByUserID"];
                }
                Reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return isFound;
        }


        public static bool IsLicenseExist(int LicenseID)
        {
            bool isFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = "SELECT Found=1 FROM Licenses WHERE LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                isFound = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return isFound;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            bool isFound = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT Found=1 
                     FROM Licenses INNER JOIN
                          Drivers ON Licenses.DriverID = Drivers.DriverID
                     WHERE Drivers.PersonID = @PersonID 
                       AND Licenses.LicenseClass = @LicenseClassID 
                       AND Licenses.IsActive = 1"; 

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                isFound = Reader.HasRows;

                Reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                Connection.Close();
            }

            return isFound;
        }

        public static bool GetLicenseInfoByApplicationID(int ApplicationID,
    ref int LicenseID, ref int DriverID, ref int LicenseClass,
    ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
    ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByUserID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Licenses WHERE ApplicationID = @ApplicationID";

            SqlCommand command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    isFound = true;

                    LicenseID = (int)reader["LicenseID"];
                    DriverID = (int)reader["DriverID"];
                    LicenseClass = (int)reader["LicenseClass"];
                    IssueDate = (DateTime)reader["IssueDate"];
                    ExpirationDate = (DateTime)reader["ExpirationDate"];

                    if (reader["Notes"] != DBNull.Value)
                        Notes = (string)reader["Notes"];
                    else
                        Notes = "";

                    PaidFees = Convert.ToSingle(reader["PaidFees"]);
                    IsActive = (bool)reader["IsActive"];
                    IssueReason = (byte)reader["IssueReason"];
                    CreatedByUserID = (int)reader["CreatedByUserID"];
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }
    }
}
