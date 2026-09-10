using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VideoTrackerDataAccessLayer
{
    public class clsVideoProjectData
    {

        public static bool GetProjectInfoByID(int ProjectID, ref string Title,
         ref int DurationInSeconds, ref string Status, ref decimal Price, ref DateTime DeliveryDate, ref int ClientID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT * FROM VideoProjects WHERE ProjectID = @ProjectID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProjectID", ProjectID);

            try
            {
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    // The record was found
                    isFound = true;
                    
                    ClientID = (int)reader["ClientID"];

                    // Title
                    if (reader["Title"] != DBNull.Value)
                    {
                        Title = (string)reader["Title"];
                    }
                    else
                    {
                        Title = "";
                    }

                    // DurationInSeconds
                    if (reader["DurationInSeconds"] != DBNull.Value)
                    {
                        DurationInSeconds = (int)reader["DurationInSeconds"];
                    }
                    else
                    {
                        DurationInSeconds = 0;
                    }

                    // Status
                    if (reader["Status"] != DBNull.Value)
                    {
                        Status = (string)reader["Status"];
                    }
                    else
                    {
                        Status = "";
                    }

                    // Price
                    if (reader["Price"] != DBNull.Value)
                    {
                        Price = (decimal)reader["Price"];
                    }
                    else
                    {
                        Price = 0.0m;
                    }

                    // DeliveryDate
                    if (reader["DeliveryDate"] != DBNull.Value)
                    {
                        DeliveryDate = (DateTime)reader["DeliveryDate"];
                    }
                    else
                    {
                        DeliveryDate = DateTime.MinValue;
                    }
                

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


        public static int AddNewProject( string Title,
          int DurationInSeconds, string Status, decimal Price, DateTime DeliveryDate, int ClientID)
        {
            //this function will return the new contact id if succeeded and -1 if not.
            int ProjectID = -1;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"INSERT INTO [dbo].[VideoProjects]
           ([Title]
           ,[DurationInSeconds]
           ,[Status]
           ,[Price]
           ,[DeliveryDate]
           ,[ClientID])
     VALUES
           (@Title
           ,@DurationInSeconds
           ,@Status
           ,@Price
           ,@DeliveryDate
           ,@ClientID);
                         SELECT SCOPE_IDENTITY();";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ClientID", ClientID);

            if (Title != "")
            {
                command.Parameters.AddWithValue("@Title", Title);

            }
            else
                command.Parameters.AddWithValue("@Title", System.DBNull.Value);


            if (DurationInSeconds != 0)
            {
                command.Parameters.AddWithValue("@DurationInSeconds", DurationInSeconds);

            }
            else
                command.Parameters.AddWithValue("@DurationInSeconds", System.DBNull.Value);


            if (Status != "")
            {
                command.Parameters.AddWithValue("@Status", Status);

            }
            else
                command.Parameters.AddWithValue("@Status", System.DBNull.Value);


            if (Price != 0)
            {
                command.Parameters.AddWithValue("@Price", Price);

            }
            else
                command.Parameters.AddWithValue("@Price", System.DBNull.Value);



            if (DeliveryDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
            }
            else
            {
                command.Parameters.AddWithValue("@DeliveryDate", System.DBNull.Value);
            }


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();


                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    ProjectID = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);

            }

            finally
            {
                connection.Close();
            }


            return ProjectID;
        }


        public static bool UpdateProject(int ProjectID,  string Title,
          int DurationInSeconds,  string Status,  decimal Price,  DateTime DeliveryDate,  int ClientID)
        {

            int rowsAffected = 0;
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"UPDATE [dbo].[VideoProjects]
   SET [Title] = @Title
      ,[DurationInSeconds] =@DurationInSeconds
      ,[Status] = @Status
      ,[Price] = @Price
      ,[DeliveryDate] = @DeliveryDate
      ,[ClientID] = @ClientID
 WHERE ProjectID = @ProjectID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProjectID", ProjectID);
            command.Parameters.AddWithValue("@ClientID", ClientID);



            if (Title != "")
            {
                command.Parameters.AddWithValue("@Title", Title);

            }
            else
                command.Parameters.AddWithValue("@Title", System.DBNull.Value);


            if (DurationInSeconds != 0)
            {
                command.Parameters.AddWithValue("@DurationInSeconds", DurationInSeconds);

            }
            else
                command.Parameters.AddWithValue("@DurationInSeconds", System.DBNull.Value);


            if (Status != "")
            {
                command.Parameters.AddWithValue("@Status", Status);

            }
            else
                command.Parameters.AddWithValue("@Status", System.DBNull.Value);


            if (Price != 0)
            {
                command.Parameters.AddWithValue("@Price", Price);

            }
            else
                command.Parameters.AddWithValue("@Price", System.DBNull.Value);



            if (DeliveryDate != DateTime.MinValue)
            {
                command.Parameters.AddWithValue("@DeliveryDate", DeliveryDate);
            }
            else
            {
                command.Parameters.AddWithValue("@DeliveryDate", System.DBNull.Value);
            }




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


        public static DataTable GetAllProjects()
        {

            DataTable dt = new DataTable();
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT        VideoProjects.ProjectID, VideoProjects.Title, VideoProjects.DurationInSeconds, VideoProjects.Status, VideoProjects.Price, VideoProjects.DeliveryDate, Clients.ClientName\r\nFROM            Clients INNER JOIN\r\n                         VideoProjects ON Clients.ClientID = VideoProjects.ClientID";

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

        public static bool DeleteProject(int ProjectID)
        {

            int rowsAffected = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"Delete from VideoProjects
                                where ProjectID = @ProjectID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProjectID", ProjectID);

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

        public static bool IsProjectExist(int ProjectID)
        {
            bool isFound = false;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "SELECT Found=1 FROM VideoProjects WHERE ProjectID = @ProjectID";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@ProjectID", ProjectID);

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
