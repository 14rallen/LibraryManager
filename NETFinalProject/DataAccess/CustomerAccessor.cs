using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class CustomerAccessor
    {
        public static Customer FetchCustomerByUserName(string username)
        {
            Customer customer = new Customer();

            SqlConnection conn = DBConnection.GetDBConnection();
            var query = @"sp_select_customer_by_username";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer = new Customer()
                        {
                            CustomerID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            UserName = reader.GetString(5),
                            Password = reader.GetString(6),
                            Active = reader.GetBoolean(7)
                        };

                        if(reader.IsDBNull(3))
                        {
                            customer.Email = null;
                        }
                        else
                        {
                            customer.Email = reader.GetString(3);
                        }

                        if(reader.IsDBNull(4))
                        {
                            customer.Phone = null;
                        }
                        else
                        {
                            customer.Phone = reader.GetString(4);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return customer;
        }

        public static Customer FetchCustomerByID(int customerID)
        {
            Customer customer = new Customer();

            SqlConnection conn = DBConnection.GetDBConnection();
            var query = @"sp_select_customer_by_id";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@customerID", customerID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        customer = new Customer()
                        {
                            CustomerID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            UserName = reader.GetString(5),
                            Password = reader.GetString(6),
                            Active = reader.GetBoolean(7)
                        };

                        if (reader.IsDBNull(3))
                        {
                            customer.Email = null;
                        }
                        else
                        {
                            customer.Email = reader.GetString(3);
                        }

                        if (reader.IsDBNull(4))
                        {
                            customer.Phone = null;
                        }
                        else
                        {
                            customer.Phone = reader.GetString(4);
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return customer;
        }

        public static bool InsertCustomer(Customer customer)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_customer";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@lastName", customer.LastName);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@phone", customer.Phone);
            cmd.Parameters.AddWithValue("@username", customer.UserName);
            cmd.Parameters.AddWithValue("@password", customer.Password);

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                inserted = true;
            }
            catch (Exception)
            {
                throw;
            }

            //return count;
            return inserted;
        }

        public static bool IsCustomerStaff(string username)
        {
            bool isStaff = false;

            SqlConnection conn = DBConnection.GetDBConnection();
            var query = @"sp_validate_customer_is_staff";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();

                if((int)cmd.ExecuteScalar() == 1)
                {
                    isStaff = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }
            return isStaff;
        }

        public static int CheckUserName(string username)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_check_username";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);

            try
            {
                conn.Open();

                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }

            return count;
        }

        public static int FindUserByUsernameAndPassword(string username, string password)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_validate_customer_username_and_password";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@password", password);

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteScalar();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static int SetPasswordForUsername(string username, string oldPassword, string newPassword)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_customer_password_for_username";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@username", username);
            cmd.Parameters.AddWithValue("@oldPassword", oldPassword);
            cmd.Parameters.AddWithValue("@newPassword", newPassword);

            try
            {
                conn.Open();
                count = (int)cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return count;
        }

        public static bool UpdateCustomer(Customer customer)
        {
            bool updated = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_customer";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@firstName", customer.FirstName);
            cmd.Parameters.AddWithValue("@lastName", customer.LastName);
            cmd.Parameters.AddWithValue("@email", customer.Email);
            cmd.Parameters.AddWithValue("@phone", customer.Phone);
            cmd.Parameters.AddWithValue("@username", customer.UserName);
            cmd.Parameters.AddWithValue("@active", customer.Active);

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                updated = true;
            }
            catch (Exception)
            {
                throw;
            }

            return updated;
        }

        public static List<Role> RetrieveRolesByUserID(int userID)
        {
            var roles = new List<Role>();
            var conn = DBConnection.GetDBConnection();

            var query = @"sp_select_roles";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userID", userID);

            try
            {
                conn.Open();
                var reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(new Role()
                        {
                            RoleID = reader.GetString(0),
                            Description = reader.GetString(1)
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                conn.Close();
            }

            return roles;
        }
    }
}
