using QLKS.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TreeView;

namespace QLKS.Controller
{
    internal class EmployeeController : IController
    {
        readonly string connectionString = "server=NGANBUI2003; Initial Catalog=Hotel; Integrated Security=True; TrustServerCertificate=True;";
        List<IModel> employees = new List<IModel>();

        public List<IModel> Items => employees;

        public bool Create(IModel model)
        {
            if (model is EmployeeModel employee)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO employee (ename, mobile,gender,emailid,role,pass ) VALUES ( @ename, @mobile, @gender,@emailid,@role,@pass)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                     //   command.Parameters.AddWithValue("@eid", employee.eid);
                        command.Parameters.AddWithValue("@ename", employee.ename);
                        command.Parameters.AddWithValue("@mobile", employee.mobile);
                        command.Parameters.AddWithValue("@gender", employee.gender);
                        command.Parameters.AddWithValue("@emailid", employee.emailid);
                        command.Parameters.AddWithValue("@role", employee.role);
                        command.Parameters.AddWithValue("@pass", employee.pass);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return true;
        }

        public bool Delete(IModel id)
        {
            if (id is EmployeeModel employee)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM employee WHERE eid = @eid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@eid", employee.eid);

                        int rowsAffected = command.ExecuteNonQuery();
                        return rowsAffected > 0;
                    }
                }
            }
            return true;
        }

        public bool IsExist(object model)
        {
            if (model is EmployeeModel employee)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM employee WHERE eid = @eid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@eid", employee.eid);
                        int count = (int)command.ExecuteScalar();
                        return count > 0;
                    }
                }
            }
            return true;
        }

        public bool Load()
        {
            employees.Clear();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM employee ORDER BY eid ASC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                           EmployeeModel employee = new EmployeeModel

                           {
                               eid = reader.GetInt32(0),         
                               ename = reader.GetString(1),       
                               mobile = reader.GetInt64(2),       
                               gender = reader.GetString(3),        
                               emailid = reader.GetString(4),      
                               role = reader.GetString(5),    
                               pass= reader.GetString(6)     
                           };
                            employees.Add(employee);
                        }
                    }
                }
            }
            return employees.Count > 0;
        }

        public bool Load(object id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM employee WHERE eid = @eid";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@eid", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            EmployeeModel employee = new EmployeeModel
                            {
                                eid = reader.GetInt32(0),           
                                ename = reader.GetString(1),        
                                mobile = reader.GetInt64(2),        
                                gender = reader.GetString(3),       
                                emailid = reader.GetString(4),     
                                role = reader.GetString(5),      
                                pass = reader.GetString(6)
                            };
                            employees.Clear();
                            employees.Add(employee);
                            return true;
                        }
                    }
                }
            }
            return true;
        }

        public IModel Read(IModel id)
        {
            if (id is EmployeeModel employee)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM employee WHERE eid = @eid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@eid", employee.eid);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new EmployeeModel
                                {
                                    eid = reader.GetInt32(0),           
                                    ename = reader.GetString(1),        
                                    mobile = reader.GetInt64(2),        
                                    gender = reader.GetString(3),        
                                    emailid = reader.GetString(4),       
                                    role = reader.GetString(5),    
                                    pass = reader.GetString(6)
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
            if (!(model is EmployeeModel employee))
            {
                Console.WriteLine("Model không hợp lệ.");
                return false;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE employee SET ename = @ename, mobile = @mobile, gender=@gender, emailid=@emailid, role=@role, pass=@pass  WHERE eid = @eid";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@eid", employee.eid);
                        command.Parameters.AddWithValue("@ename", employee.ename);
                        command.Parameters.AddWithValue("@mobile", employee.mobile);
                        command.Parameters.AddWithValue("@gender", employee.gender);
                        command.Parameters.AddWithValue("@emailid", employee.emailid);
                        command.Parameters.AddWithValue("@role", employee.role);
                        command.Parameters.AddWithValue("@pass", employee.pass);

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
