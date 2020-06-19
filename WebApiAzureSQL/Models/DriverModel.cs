using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAzureSQL.Models
{
    public class DriverModel
    {
        string ConnectionString = "Server=tcp:azuresqldbklopezserver.database.windows.net,1433;Initial Catalog=AzureSqlDatabase;Persist Security Info=False;User ID=klopez;Password={password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        public int IDDriver { get; set; }
        public string Name {get;set;}
        public string Picture { get; set; }
        public string Status { get; set; }
        public PositionModel ActualPosition { get; set; }
        public List<DriverModel> GetAll()
        {
            List<DriverModel> list = new List<DriverModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Driver INNER JOIN Position ON Driver.IDActualPosition = Position.IDPosition";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new DriverModel
                                {
                                    IDDriver = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    ActualPosition = new PositionModel
                                    {
                                        Latitude = reader["Latitude"].ToString(),
                                        Longitude = reader["Longitude"].ToString()
                                    }
                                });
                            }
                        }
                    }
                }
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DriverModel Get(int id)
        {
            DriverModel driver = new DriverModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Driver INNER JOIN Position ON Driver.IDActualPosition = Position.IDPosition WHERE IDDriver = @IDDriver";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.Parameters.AddWithValue("@IDDriver", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                driver = new DriverModel
                                {
                                    IDDriver = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    ActualPosition = new PositionModel
                                    {
                                        Latitude = reader["Latitude"].ToString(),
                                        Longitude = reader["Longitude"].ToString()
                                    }
                                };
                            }
                        }
                    }
                }
                return driver;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApiResponse Insert()
        {
            object id;
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("createDriver", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@Picture", Picture);
                        cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                        id = cmd.ExecuteNonQuery();
                    }
                }
                if (id != null && id.ToString().Length > 0)
                {
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Result = int.Parse(id.ToString()),
                        Message = "EXCELSIOR, Conductor creado"
                    };
                    //return int.Parse(id.ToString());
                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Result = 0,
                        Message = "ERROR"
                    };
                    //return 0; 
                }
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
                throw;
            }
        }

        public ApiResponse Update(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand("updateDriver", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@Picture", Picture);
                        cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                        id = cmd.ExecuteNonQuery();
                    }
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = IDDriver,
                    Message = "EXCELSIOR, Conductor actualizado"
                };
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
                throw;
            }
        }
        //string ConnectionString, 
        public ApiResponse Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "deleteDriver";

                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = id,
                    Message = "EXCELSIOR, Conductor eliminado"
                };
            }
            catch (Exception exc)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = exc.Message
                };
                throw;
            }
        }
    }
}
