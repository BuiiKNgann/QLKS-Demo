using QLKS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Controller
{
    internal class RoomController : IController
    {
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel; Integrated Security=True; TrustServerCertificate=True;";
        List<IModel> rooms = new List<IModel>();

        public List<IModel> Items => rooms;

        public bool Create(IModel model)
        {
            if (model is RoomModel room)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO rooms (roomNo, roomType,bed,price,booked ) VALUES ( @roomNo, @roomType, @bed,@price,@booked)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        //   command.Parameters.AddWithValue("@eid", employee.eid);
                        command.Parameters.AddWithValue("@roomNo", room.roomNo);
                        command.Parameters.AddWithValue("@roomType", room.roomType);
                        command.Parameters.AddWithValue("@bed", room.bed);
                        command.Parameters.AddWithValue("@price", room.price);
                        command.Parameters.AddWithValue("@booked", room.booked);


                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return true;
        }

        public bool Delete(IModel id)
        {
            if (id is RoomModel room)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM rooms WHERE roomid = @roomid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@roomid", room.roomid);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return true;
        }

        public bool IsExist(object model)
        {
            if (model is RoomModel room)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM rooms WHERE roomid = @roomid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@roomid", room.roomid);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            return true;
        }

        public bool Load()
        {
            rooms.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM rooms ORDER BY roomid ASC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            RoomModel room = new RoomModel

                            {

                                roomid = reader.GetInt32(0),
                                roomNo = reader.GetString(1),
                                roomType = reader.GetString(2),
                                bed = reader.GetString(3),
                                price = reader.GetInt64(4),
                                booked = reader.GetString(5)

                            };
                            rooms.Add(room);
                        }
                    }
                }
            }
            return rooms.Count > 0;
        }


        public bool Load(object id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM rooms WHERE roomid = @roomid";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            RoomModel room = new RoomModel
                            {
                                roomid = reader.GetInt32(0),
                                roomNo = reader.GetString(1),
                                roomType = reader.GetString(2),
                                bed = reader.GetString(3),
                                price = reader.GetInt64(4),
                                booked = reader.GetString(5)
                            };
                            rooms.Clear();
                            rooms.Add(room);
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        public IModel Read(IModel id)
        {

            if (id is RoomModel room)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM rooms WHERE roomid = @roomid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@roomid", room.roomid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new RoomModel
                                {
                                    roomid = reader.GetInt32(0),
                                    roomNo = reader.GetString(1),
                                    roomType = reader.GetString(2),
                                    bed = reader.GetString(3),
                                    price = reader.GetInt64(4),
                                    booked = reader.GetString(5)
                                };
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool Update(IModel model)
        {
            if (!(model is RoomModel room))
            {
                Console.WriteLine("Model không hợp lệ.");
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE rooms SET roomNo = @roomNo, roomType = @roomType, bed = @bed, price = @price, booked = @booked WHERE roomid = @roomid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@roomid", room.roomid); // Đúng là @roomid
                        command.Parameters.AddWithValue("@roomNo", room.roomNo);
                        command.Parameters.AddWithValue("@roomType", room.roomType);
                        command.Parameters.AddWithValue("@bed", room.bed);
                        command.Parameters.AddWithValue("@price", room.price);
                        command.Parameters.AddWithValue("@booked", room.booked);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                Console.WriteLine($"SQL Error: {sqlEx.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return false;
            }
        }
    }
}

