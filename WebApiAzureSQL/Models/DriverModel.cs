using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiAzureSQL.Models
{
    public class DriverModel
    {
        string ConnectionString = "Server=tcp:azuresqlserver-delr.database.windows.net,1433;Initial Catalog=AzureSqlDatabase;Persist Security Info=False;User ID=azuresqluser;Password=Danielito#2031;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
        public int IDDriver { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }

        public string Status { get; set; }

        public PositionModel ActualPosition { get; set; }

        public ObservableCollection<DriverModel> GetAll()
        {

            ObservableCollection<DriverModel> list = new ObservableCollection<DriverModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "sp_getAllDrivers";// INNER JOIN Position";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new DriverModel
                                {
                                    IDDriver = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    Status = reader["Status"].ToString(),
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
            DriverModel obj = new DriverModel();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "sp_getDriver";

                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@IDDriver", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                obj = new DriverModel
                                {
                                    IDDriver = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    Status = reader["Status"].ToString(),
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
                return obj;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ApiResponse Insert()
        {
            try
            {
                object id;
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "sp_CreateDriver";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", Name);
                        cmd.Parameters.AddWithValue("@Picture", Picture);
                        cmd.Parameters.AddWithValue("@Status", Status);
                        cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                        cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                        id = cmd.ExecuteScalar();
                    }
                }
                if (id != null && id.ToString().Length > 0)
                {
                    //return int.Parse(id.ToString());
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Result = int.Parse(id.ToString()),
                        Message = "El conductor fue generado correctamente"
                    };

                }
                else
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Result = 0,
                        Message = "Error al generar el conductor"
                    };
                }
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = e.Message
                };
            }
        }

        public ApiResponse Update(int type)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    
                    string tsql = "";

                    if (type == 1) {
                        tsql = "sp_UpdateDriverOldPosition";
                        using (SqlCommand cmd = new SqlCommand(tsql, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IDDriver", IDDriver);
                            cmd.Parameters.AddWithValue("@Name", Name);
                            cmd.Parameters.AddWithValue("@Picture", Picture);
                            cmd.Parameters.AddWithValue("@Status", Status);
                            cmd.ExecuteScalar();
                        }

                    } 
                    else {
                        tsql = "sp_UpdateDriverNewPosition";
                        using (SqlCommand cmd = new SqlCommand(tsql, conn))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@IDDriver", IDDriver);
                            cmd.Parameters.AddWithValue("@Name", Name);
                            cmd.Parameters.AddWithValue("@Picture", Picture);
                            cmd.Parameters.AddWithValue("@Status", Status);
                            cmd.Parameters.AddWithValue("@Latitude", ActualPosition.Latitude);
                            cmd.Parameters.AddWithValue("@Longitude", ActualPosition.Longitude);
                            cmd.ExecuteScalar();
                        }

                    }


                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = IDDriver,
                    Message = "El viaje fue actualizado correctamente"
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = e.Message
                };
            }
        }
        
        public ApiResponse Delete(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "DELETE FROM Driver WHERE IDDriver = @IDDriver;";
                    using (SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        cmd.CommandType = System.Data.CommandType.Text;
                        cmd.Parameters.AddWithValue("@IDDriver", id);
                        cmd.ExecuteNonQuery();
                    }
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Result = id,
                    Message = "El viaje fue eliminado correctamente"
                };
            }
            catch (Exception e)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Result = null,
                    Message = e.Message
                };
            }
        }
    }
}
