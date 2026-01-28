using DataAccessSettings;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;
using System.Runtime.Remoting.Messaging;
namespace DataAccessLayer
{
    public class clsPersonsDataAccessLayer
    {
        static public DataTable GetAllPersons()
        {
            DataTable PersonsDataTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT Persons.PersonID, Persons.NationalNumber, 
                 Persons.FirstName, Persons.SecondName, Persons.ThirdName, Persons.LastName,
                 CASE 
                    WHEN Persons.Gender = 0 THEN 'Female' 
                    ELSE 'Male' 
                 END as Gender, 
                 Persons.BirthDate, 
                 Countries.CountryName as Nationality, 
                 Persons.Phone, Persons.Email 
                 FROM Persons 
                 INNER JOIN Countries ON Persons.NationalityID = Countries.CountryID";
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
                Console.WriteLine("Error: " + ex.Message.ToString());
            }
            finally
            {
                Connection.Close();
            }

            return PersonsDataTable;
        }
        static public DataTable GetAllPersonsWithFilter(string Where, string EqualTo)
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = $"select * from Persons where {Where} = @EqualTo";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@EqualTo", EqualTo);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if(Reader.HasRows)
                {
                    dt.Load(Reader);
                   
                }
                Reader.Close();
                Connection.Close();
            }
            catch(Exception ex)
            {
                throw new Exception("Error: " + ex.Message.ToString());
            }
            finally
            {
                Connection.Close();
            }
            return dt;
        }
        static public int GetRowsCount()
        {
            object count;
            DataTable PersonsDataTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "select count(*) from Persons";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                count = Command.ExecuteScalar();
                Connection.Close();
                return (int)count;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message.ToString());
            }
            finally
            {
                Connection.Close();
            }
            return 0;
        }
        static public int AddNewPerson( string NationalNumber, string FirstName,string SecondName,
        string ThirdName, string LastName, byte gender, DateTime BirthDate, string Address,
        string Phone, string Email, int CountryID, string ImagePath)
        {
            int resultint = -1;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"insert into Persons (NationalNumber, FirstName, SecondName, ThirdName, LastName, Gender, BirthDate, Address, Phone, Email, NationalityID, ImagePath)
                            values(@NationalNumber, @FirstName, @SecondName, @ThirdName, @LastName, @Gender, @BirthDate, @Address, @Phone, @Email, @NationalityID, @ImagePath);
                            select SCOPE_IDENTITY();";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName",SecondName);
            Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@Gender", gender);
            Command.Parameters.AddWithValue("@BirthDate", BirthDate);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Email", Email);
            Command.Parameters.AddWithValue("@NationalityID", CountryID );
            Command.Parameters.AddWithValue("@ImagePath", ImagePath);
            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if(result!=null&&int.TryParse(result.ToString(), out int PersonID))
                {
                    return PersonID;
                }
                Connection.Close();
            }
            catch(Exception ex)
            {
                throw new Exception("SQL Database Error: " + ex.Message);
               
            }
            finally
            {
                Connection.Close();
            }
            return resultint;
        }
        static public bool UpdatePerson(int PersonID,string NationalNumber, string FirstName, string SecondName,
        string ThirdName, string LastName, byte gender, DateTime BirthDate, string Address,
        string Phone, string Email, int CountryID, string ImagePath)
        {
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"UPDATE Persons SET NationalNumber = @NationalNumber ,FirstName = @FirstName, SecondName = @SecondName, 
                             ThirdName = @ThirdName, LastName = @LastName, Gender = @Gender, BirthDate = @BirthDate, Address = @Address,
                             Phone = @Phone, Email = @Email, NationalityID = @NAtionalityID, ImagePath = @ImagePath
                             WHERE PersonID = @PersonID;";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNumber", NationalNumber);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@ThirdName", ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@Gender", gender);
            Command.Parameters.AddWithValue("@BirthDate", BirthDate);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Email", Email);
            Command.Parameters.AddWithValue("@NationalityID", CountryID);
            if (string.IsNullOrEmpty(ImagePath))
                Command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);
            else
                Command.Parameters.AddWithValue("@ImagePath", ImagePath);

            try
            {
                Connection.Open();
                int result = Command.ExecuteNonQuery();
                if(result!=0)
                {
                    Connection.Close();
                    return true;
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
            return false;
        }
        static public bool DeletePerson(int ID)
        {
            int rowEffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"delete from Persons where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", ID);

            try
            {
                Connection.Open();
                rowEffected = Command.ExecuteNonQuery();
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
            return (rowEffected > 0);
        }
        static public object GetPersonItem(int PersonID, string Item)
        {
            object ItemToReturn = null;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = $"select {Item} from Persons where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID",PersonID);
            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();
                if(result!=null)
                {
                    ItemToReturn = result;
                }
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
            return ItemToReturn;
        }
        static public DataTable GetPersonInfoByID(int PersonID)
        {
            DataTable dt = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select * from Persons where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);
            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();
                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }
                Reader.Close();
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
            return dt;
        }

    }
}
