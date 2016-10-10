using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class OrderAccessor
    {
        public static List<Order> FetchOrderList()
        {
            List<Order> orders = new List<Order>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT OrderID, CustomerID, DateOrdered " +
                           @"FROM [Order] ";

            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Order currentOrder = new Order()
                        {
                            OrderID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DateOrdered = reader.GetDateTime(2)
                        };

                        orders.Add(currentOrder);
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
            return orders;
        }

        public static OrderDesc FetchOrderListDesc(int orderID)
        {
            OrderDesc order = new OrderDesc();

            SqlConnection conn = DBConnection.GetDBConnection();
            var query = @"sp_select_orderdesc_by_order";
            SqlCommand cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@orderID", orderID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                
                while (reader.Read())
                {
                    order = new OrderDesc()
                    {
                        OrderID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        DateOrdered = reader.GetDateTime(2),
                        Quantity = reader.GetInt32(3),
                        Title = reader.GetString(4)
                    };
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
            return order;
        }

        public static bool InsertOrder(int customerID, string isbn, int quantity)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_order";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@bookID", isbn);
            cmd.Parameters.AddWithValue("@quantity", quantity);

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
    }
}
