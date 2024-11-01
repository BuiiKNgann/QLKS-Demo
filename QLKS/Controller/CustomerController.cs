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
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel; Integrated Security=True; TrustServerCertificate=True;";
        List<IModel> customer = new List<IModel>();

        public List<IModel> Items => customer;

        public bool Create(IModel model)
        {
            if (model is CustomerModel customer)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO customer (cid, cname, mobile, nationality, gender, dob, idproof, address, checkin, checkout, checkoutStatus, roomid) VALUES (@cid, @cname, @mobile, @nationality, @gender, @dob, @idproof, @address, @checkin, @checkout, @checkoutStatus, @roomid)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@cid", customer.cid);
                        command.Parameters.AddWithValue("@cname", customer.cname);
                        command.Parameters.AddWithValue("@mobile", customer.mobile);
                        command.Parameters.AddWithValue("@nationality", customer.nationality);
                        command.Parameters.AddWithValue("@gender", customer.gender);
                        command.Parameters.AddWithValue("@dob", customer.dob);
                        command.Parameters.AddWithValue("@idproof", customer.idproof);
                        command.Parameters.AddWithValue("@address", customer.address);
                        command.Parameters.AddWithValue("@checkin", customer.checkin);
                        command.Parameters.AddWithValue("@checkout", customer.checkout); 
                        command.Parameters.AddWithValue("@checkoutStatus", customer.checkoutStatus); 
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
            return true;
        }

        public bool Load()
        {
            return true;
        }

        public bool Load(object id)
        {
            return true;
        }

        public IModel Read(IModel id)
        {
            return null;
        }

        public bool Update(IModel model)
        {
            return true;
        }
        //public List<int> GetRoomIds()
        //{
        //    List<int> roomIds = new List<int>();
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        connection.Open();
        //        string query = "SELECT roomid FROM rooms";
        //        using (SqlCommand command = new SqlCommand(query, connection))
        //        {
        //            using (SqlDataReader reader = command.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    roomIds.Add(reader.GetInt32(0));
        //                }
        //            }
        //        }
        //    }
        //    return roomIds;
        //}
    }
}
