using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class RentalAccessor
    {
        public static List<Rental> FetchRentalList(Active group = Active.active)
        {
            List<Rental> rentals = new List<Rental>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT RentalID, CustomerID, DateRented, " +
                           @"DateDue, LateFee, Active " +
                           @"FROM Rental ";
              
            if(group == Active.active)
            {
                query += @"WHERE Active = 1 ";
            }
            else if (group == Active.inactive)
            {
                query += @"WHERE Active = 0 ";
            }
            
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Rental currentRental = new Rental()
                        {
                            RentalID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DateRented = reader.GetDateTime(2),
                            DateDue = reader.GetDateTime(3),
                            LateFee = reader.GetDecimal(4),
                            Active = reader.GetBoolean(5)
                        };

                        rentals.Add(currentRental);
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
            return rentals;
        }

        public static List<Rental> FetchRentalListByCustomerID(int customerID)
        {
            List<Rental> rentals = new List<Rental>();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_rental_by_customer";
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
                        Rental currentRental = new Rental()
                        {
                            RentalID = reader.GetInt32(0),
                            CustomerID = reader.GetInt32(1),
                            DateRented = reader.GetDateTime(2),
                            DateDue = reader.GetDateTime(3),
                            LateFee = reader.GetDecimal(4),
                            Active = reader.GetBoolean(5)
                        };

                        rentals.Add(currentRental);
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
            return rentals;
        }

        public static Rental FetchRentalByCustomerID(int customerID, string isbn)
        {
            Rental rental = null;

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_rental_by_customer_book";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@bookID", isbn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    rental = new Rental()
                    {
                        RentalID = reader.GetInt32(0),
                        CustomerID = reader.GetInt32(1),
                        DateRented = reader.GetDateTime(2),
                        DateDue = reader.GetDateTime(3),
                        LateFee = reader.GetDecimal(4),
                        Active = reader.GetBoolean(5)
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

            return rental;
        }

        public static bool InsertRental(Rental rental, string isbn)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_rental_with_book";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@customerID", rental.CustomerID);
            cmd.Parameters.AddWithValue("@dateRented", rental.DateRented);
            cmd.Parameters.AddWithValue("@dateDue", rental.DateDue);
            cmd.Parameters.AddWithValue("@lateFee", rental.LateFee);
            cmd.Parameters.AddWithValue("@bookID", isbn);

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

        public static bool InactivateRental(Book book, Customer user)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_delete_rental";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@bookID", book.ISBN);
            cmd.Parameters.AddWithValue("@customerID", user.CustomerID);

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
