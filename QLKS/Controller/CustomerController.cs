using QLKS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKS.Controller
{
    internal class CustomerController : IController
    {
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel_Manager; Integrated Security=True; TrustServerCertificate=True;";
        List<IModel> customers = new List<IModel>();

        public List<IModel> Items => customers;

        public bool Create(IModel model)
        {
            if (model is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO customer ( cname, mobile, nationality, gender, dob, idproof, address, checkin, checkout, roomid) VALUES ( @cname, @mobile, @nationality, @gender, @dob, @idproof, @address, @checkin, @checkout, @roomid)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // command.Parameters.AddWithValue("@cid", customer.cid);
                        command.Parameters.AddWithValue("@cname", customer.cname);
                        command.Parameters.AddWithValue("@mobile", customer.mobile);
                        command.Parameters.AddWithValue("@nationality", customer.nationality);
                        command.Parameters.AddWithValue("@gender", customer.gender);
                        command.Parameters.AddWithValue("@dob", customer.dob);
                        command.Parameters.AddWithValue("@idproof", customer.idproof);
                        command.Parameters.AddWithValue("@address", customer.address);
                        command.Parameters.AddWithValue("@checkin", customer.checkin);
                        command.Parameters.AddWithValue("@checkout", customer.checkout);
                        //   command.Parameters.AddWithValue("@checkoutStatus", customer.checkoutStatus); 
                        command.Parameters.AddWithValue("@roomid", customer.roomid);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return false; // Trả về false nếu model không phải là CustomerModel
        }

        public bool Delete(IModel id)
        {
            return true;
        }

        public bool IsExist(object model)
        {
            if (model is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM customer WHERE cid = @cid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", customer.cid);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            return true;
        }

        public bool Load()
        {
            customers.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM customer ORDER BY cid ASC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CustomerModel customer = new CustomerModel
                            {
                                cid = reader.GetInt32(0),
                                cname = reader.GetString(1),
                                mobile = reader.GetInt64(2),
                                nationality = reader.GetString(3),
                                gender = reader.GetString(4),
                                dob = reader.GetDateTime(5),
                                idproof = reader.GetString(6),
                                address = reader.GetString(7),
                                checkin = reader.GetDateTime(8),
                                checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9), // Kiểm tra null cho checkout
                                roomid = reader.GetInt32(10)
                            };
                            customers.Add(customer);
                        }
                    }
                }
            }
            return customers.Count > 0; // Trả về true nếu có dữ liệu
        }
        public bool Load(object id)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customer WHERE cid = @cid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                CustomerModel customer = new CustomerModel
                                {
                                    cid = reader.GetInt32(0), // Không cần kiểm tra IsDBNull
                                    cname = reader.GetString(1), // Không cần kiểm tra IsDBNull
                                    mobile = reader.IsDBNull(2) ? 0 : reader.GetInt64(2), // Kiểm tra IsDBNull cho mobile
                                    nationality = reader.GetString(3), // Không cần kiểm tra IsDBNull
                                    gender = reader.GetString(4), // Không cần kiểm tra IsDBNull
                                    dob = reader.GetDateTime(5), // Không cần kiểm tra IsDBNull
                                    idproof = reader.GetString(6), // Không cần kiểm tra IsDBNull
                                    address = reader.GetString(7), // Không cần kiểm tra IsDBNull
                                    checkin = reader.GetDateTime(8), // Không cần kiểm tra IsDBNull
                                    checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9), // Kiểm tra IsDBNull cho checkout
                                    roomid = reader.GetInt32(10) // Không cần kiểm tra IsDBNull
                                };
                                customers.Clear();
                                customers.Add(customer);
                                return true;
                            }
                        }
                    }
                }
                return true;
            }

            public IModel Read(IModel id)
            {
            if (id is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM customer WHERE cid = @cid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", customer.cid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new CustomerModel
                                {
                                    cid = reader.GetInt32(0),
                                    cname = reader.GetString(1),
                                    mobile = reader.IsDBNull(2) ? 0 : reader.GetInt64(2),
                                    nationality = reader.GetString(3),
                                    gender = reader.GetString(4),
                                    dob = reader.GetDateTime(5),
                                    idproof = reader.GetString(6),
                                    address = reader.GetString(7),
                                    checkin = reader.GetDateTime(8),
                                    checkout = reader.IsDBNull(9) ? (DateTime?)null : reader.GetDateTime(9),
                                    roomid = reader.GetInt32(10)
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
                return true;
            }
        public List<dynamic> GetCustomerRoomDetails()
        {
            List<dynamic> customerRoomDetails = new List<dynamic>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = @"
            SELECT c.cid, c.cname, c.mobile, c.nationality, c.gender, c.dob, c.idproof, c.address, 
                   c.checkin, c.checkout, c.roomid, r.roomType, r.bed, r.price
            FROM customer AS c
            JOIN rooms AS r ON c.roomid = r.roomid
            ORDER BY c.cid ASC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var customerRoom = new
                            {
                                cid = reader.GetInt32(0),
                                cname = reader.GetString(1),
                                mobile = reader.GetInt64(2),
                                nationality = reader.GetString(3),
                                gender = reader.GetString(4),
                                dob = reader.GetDateTime(5),
                                idproof = reader.GetString(6),
                                address = reader.GetString(7),
                                checkin = reader.GetDateTime(8),
                                checkout = reader.GetDateTime(9),
                                roomid = reader.GetInt32(10),
                                roomType = reader.GetString(11),
                                bed = reader.GetString(12),
                                price = reader.GetInt64(13)
                            };
                            customerRoomDetails.Add(customerRoom);
                        }
                    }
                }
            }
            return customerRoomDetails;
        }

    }
    }
