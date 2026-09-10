using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTrackerDataAccessLayer
{
    public class clsClientData
    {
        public static bool GetClientInfoByID(int ClientID, ref string ClientName,
           ref string PlatForm, ref string ContactInfo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Clients WHERE ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ClientName = (string)reader["ClientName"];

                    if (reader["Platform"] != DBNull.Value)
                    {
                        PlatForm = (string)reader["PlatForm"];
                    }
                    else
                        PlatForm = "";


                    if (reader["ContactInfo"] != DBNull.Value)
                    {
                        ContactInfo = (string)reader["ContactInfo"];
                    }
                    else
                        ContactInfo = "";




                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }


        public static bool GetClientInfoByName(string ClientName, ref int ClientID,
          ref string PlatForm, ref string ContactInfo)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Clients WHERE ClientName = @ClientName";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientName", ClientName);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;

                    ClientID = (int)reader["ClientID"];

                    if (reader["Platform"] != DBNull.Value)
                    {
                        PlatForm = (string)reader["PlatForm"];
                    }
                    else
                        PlatForm = "";


                    if (reader["ContactInfo"] != DBNull.Value)
                    {
                        ContactInfo = (string)reader["ContactInfo"];
                    }
                    else
                        ContactInfo = "";




                }
                else
                {
                    // The record was not found
                    isFound = false;
                }

                reader.Close();


            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }
            finally
            {
                connection.Close();
            }

            return isFound;
        }

        public static int AddNewClient(string ClientName,
               string Platform, string ContactInfo)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int ClientID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[Clients]
               ([ClientName]
               ,[Platform]
               ,[ContactInfo])
                  VALUES
               (@ClientName
               ,@Platform
               ,@ContactInfo);
               SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientName", ClientName);

            if(Platform != "")
            {
                command.Parameters.AddWithValue("@Platform", Platform);
               
            }
            else
                command.Parameters.AddWithValue("@Platform", System.DBNull.Value);


            if (ContactInfo != "")
            {
                command.Parameters.AddWithValue("@ContactInfo", ContactInfo);

            }
            else
                command.Parameters.AddWithValue("@ContactInfo", System.DBNull.Value);


           


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ClientID = insertedID;
                }
            }

            catch (Exception ex)
            {
               
                // Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return ClientID;
        }


        public static bool UpdateClient (int ClientID, string ClientName,
               string Platform, string ContactInfo)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[Clients]
                         SET [ClientName] = @ClientName
                         ,[Platform] =@Platform
                        ,[ContactInfo] =@ContactInfo
                        WHERE ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);
            command.Parameters.AddWithValue("@ClientName", ClientName);
          


            if (Platform != "")
            {
                command.Parameters.AddWithValue("@Platform", Platform);

            }
            else
                command.Parameters.AddWithValue("@Platform", System.DBNull.Value);


            if (ContactInfo != "")
            {
                command.Parameters.AddWithValue("@ContactInfo", ContactInfo);

            }
            else
                command.Parameters.AddWithValue("@ContactInfo", System.DBNull.Value);




            try
            {
                connection.Open();
                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                return false;
            }

            finally
            {
                connection.Close();
            }

            return (rowsAffected > 0);
        }


        public static DataTable GetAllClients()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM Clients";

            SqlCommand command = new SqlCommand(query, connection);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                if (reader.HasRows)

                {
                    dt.Load(reader);
                }

                reader.Close();


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }

            return dt;

        }


        public static bool DeleteClient(int ClientID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete FROM Clients
                                where ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

            try
            {
                connection.Open();

                rowsAffected = command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
            }
            finally
            {

                connection.Close();

            }

            return (rowsAffected > 0);

        }


        public static bool IsClientstExist(int ClientID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM Clients WHERE ClientID = @ClientID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                isFound = reader.HasRows;

                reader.Close();
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
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
