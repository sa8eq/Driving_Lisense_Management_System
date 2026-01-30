using DataAccessSettings;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.Remoting.Messaging;
namespace DataAccessLayer
{
    public class clsPersonData
    {
        public static bool GetPersonInfoByID(int PersonID, ref string NationalNumber, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName,  ref DateTime BirthDate, ref short Gender, ref string Address,
            ref string Phone, ref string Email,ref int NationalityID ,ref string ImagePath)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select * from Persons where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID); 
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.Read())
                {
                    FirstName = (string)Reader["FirstName"];
                    if (Reader["SecondName"]!=DBNull.Value)
                    {
                        SecondName = (string)Reader["SecondName"];
                    }
                    else
                    {
                        SecondName = "";
                    }
                    if (Reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)Reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }
                    LastName = (string)Reader["LastName"];
                    NationalNumber = (string)Reader["NationalNumber"];
                    BirthDate = (DateTime)Reader["BirthDate"];
                    Gender = Convert.ToInt16(Reader["Gender"]);
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    if (Reader["Email"]!=DBNull.Value)
                    {
                        Email = (string)Reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }
                    NationalityID = (int)Reader["NationalityID"];
                    if (Reader["ImagePath"]!=DBNull.Value)
                    {
                        ImagePath = (string)Reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

                    Reader.Close();
                    Connection.Close();
                    return IsFound;
                }
                else
                {
                    Reader.Close();
                    Connection.Close();
                    IsFound = false;
                    return IsFound;
                }         
            }
            catch(Exception ex)
            {
                IsFound = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        public static bool GetPersonInfoByNationalNumber(string NationalNumber,  ref int PersonID, ref string FirstName, ref string SecondName, ref string ThirdName,
            ref string LastName, ref DateTime BirthDate, ref short Gender, ref string Address,
            ref string Phone, ref string Email, ref int NationalityID, ref string ImagePath)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select * from Persons where NationalNumber = @NationalNumber";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.Read())
                {
                    PersonID = (int)Reader["PersonID"];
                    FirstName = (string)Reader["FirstName"];
                    if (Reader["SecondName"] != DBNull.Value)
                    {
                        SecondName = (string)Reader["SecondName"];
                    }
                    else
                    {
                        SecondName = "";
                    }
                    if (Reader["ThirdName"] != DBNull.Value)
                    {
                        ThirdName = (string)Reader["ThirdName"];
                    }
                    else
                    {
                        ThirdName = "";
                    }
                    LastName = (string)Reader["LastName"];
                    BirthDate = (DateTime)Reader["BirthDate"];
                    Gender = Convert.ToInt16(Reader["Gender"]);
                    Address = (string)Reader["Address"];
                    Phone = (string)Reader["Phone"];
                    if (Reader["Email"] != DBNull.Value)
                    {
                        Email = (string)Reader["Email"];
                    }
                    else
                    {
                        Email = "";
                    }
                    NationalityID = (int)Reader["NationalityID"];
                    if (Reader["ImagePath"] != DBNull.Value)
                    {
                        ImagePath = (string)Reader["ImagePath"];
                    }
                    else
                    {
                        ImagePath = "";
                    }

                    Reader.Close();
                    Connection.Close();
                    return IsFound;
                }
                else
                {
                    Reader.Close();
                    Connection.Close();
                    IsFound = false;
                    return IsFound;
                }
            }
            catch (Exception ex)
            {
                IsFound = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsFound;
        }

        static public int AddNewPerson(string NationalNumber, string FirstName, string SecondName,
            string ThirdName, string LastName, short gender, DateTime BirthDate, string Address,
            string Phone, string Email, int CountryID, string ImagePath)
        {
            int InsertedID = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"insert into Persons (NationalNumber, FirstName, SecondName, ThirdName, LastName, Gender, BirthDate, Address, Phone, Email, NationalityID, ImagePath)
                            values(@NationalNumber, @FirstName, @SecondName, @ThirdName, @LastName, @Gender, @BirthDate, @Address, @Phone, @Email, @NationalityID, @ImagePath);
                            select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            if (SecondName != "" && SecondName != null)
            {
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            }
            else
            {
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);
            }
            if (ThirdName != "" && ThirdName != null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            }
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@Gender", gender);
            Command.Parameters.AddWithValue("@BirthDate", BirthDate);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            if (Email != "" && Email != null)
            {
                Command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            }
            Command.Parameters.AddWithValue("@NationalityID", CountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }
            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null && int.TryParse(result.ToString(), out InsertedID))
                {
                    return InsertedID;
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("SQL Database Error: " + ex.Message);

            }
            finally
            {
                Connection.Close();
            }
            return InsertedID;
        }

        static public bool UpdatePerson(int PersonID, string NationalNumber, string FirstName, string SecondName,
        string ThirdName, string LastName, short gender, DateTime BirthDate, string Address,
        string Phone, string Email, int CountryID, string ImagePath)
        {
            int rowAffected = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Persons SET NationalNumber = @NationalNumber ,FirstName = @FirstName, SecondName = @SecondName, 
                             ThirdName = @ThirdName, LastName = @LastName, Gender = @Gender, BirthDate = @BirthDate, Address = @Address,
                             Phone = @Phone, Email = @Email, NationalityID = @NationalityID, ImagePath = @ImagePath
                             WHERE PersonID = @PersonID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            if (SecondName != "" && SecondName != null)
            {
                Command.Parameters.AddWithValue("@SecondName", SecondName);
            }
            else
            {
                Command.Parameters.AddWithValue("@SecondName", System.DBNull.Value);
            }
            if (ThirdName != "" && ThirdName != null)
            {
                Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            }
            else
            {
                Command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);
            }
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@Gender", gender);
            Command.Parameters.AddWithValue("@BirthDate", BirthDate);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            if (Email != "" && Email != null)
            {
                Command.Parameters.AddWithValue("@Email", Email);
            }
            else
            {
                Command.Parameters.AddWithValue("@Email", System.DBNull.Value);
            }
            Command.Parameters.AddWithValue("@NationalityID", CountryID);
            if (ImagePath != "" && ImagePath != null)
            {
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            }
            else
            {
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            }
            

            try
            {
                Connection.Open();
                rowAffected = Command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    Connection.Close();
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

        static public DataTable GetAllPersons()
        {
            DataTable PersonsDataTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT Persons.PersonID, Persons.NationalNumber, 
                 Persons.FirstName, Persons.BirthDate, Persons.SecondName, Persons.ThirdName, Persons.LastName,
                 Persons.Gender, 
                 CASE 
                    WHEN Persons.Gender = 0 THEN 'Female' 
                    ELSE 'Male' 
                 END as GenderCaption, 
                 Persons.Address, Persons.Phone, Persons.Email, Countries.CountryName, Persons.NationalityID, Persons.ImagePath
                 FROM Persons INNER JOIN Countries ON Persons.NationalityID = Countries.CountryID";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    PersonsDataTable.Load(Reader);
                }
                Reader.Close();
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

            return PersonsDataTable;
        }

        static public bool DeletePerson(int ID)
        {
            int rowEffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"DELETE FROM Persons WHERE PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                Connection.Open();
                rowEffected = Command.ExecuteNonQuery();
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
            return (rowEffected > 0);
        }

        static public bool IsPersonExist(int PersonID)
        {
            bool IsFound = true;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"Select FOUND=1 from Persons where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID",PersonID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if(result !=null)
                {
                    return IsFound;
                }
                Connection.Close();
            }
            catch(Exception ex)
            {
                IsFound = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }

        static public bool IsPersonExist(string NationalNumber)
        {
            bool IsFound = false;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"Select FOUND=1 from Persons where NationalNumber = @NationalNumber";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if (result != null)
                {
                    IsFound = true;
                    return IsFound;
                }
                Connection.Close();
            }
            catch (Exception ex)
            {
                IsFound = false;
                throw new Exception(ex.Message);
            }
            finally
            {
                Connection.Close();
            }
            return IsFound;
        }
    }
}
