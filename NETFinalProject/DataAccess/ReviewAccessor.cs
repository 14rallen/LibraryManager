using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class ReviewAccessor
    {
        public static List<Review> FetchReviewList(Active group = Active.active)
        {
            List<Review> reviews = new List<Review>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT BookID, RentalID, Content, Active " + 
                           @"FROM Review ";

            if(group == Active.active)
            {
                query += @"Where Active = 1 ";
            }
            else if(group == Active.inactive)
            {
                query += @"Where Active = 0 ";
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
                        Review currentReview = new Review()
                        {
                            ISBN = reader.GetString(0),
                            RentalID = reader.GetInt32(1),
                            Content = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };

                        reviews.Add(currentReview);
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

            return reviews;
        }

        public static bool InsertReview(Review review)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_review";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rentalID", review.RentalID);
            cmd.Parameters.AddWithValue("@bookID", review.ISBN);
            cmd.Parameters.AddWithValue("@content", review.Content);

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

        public static bool UpdateReview(Review review)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_review";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rentalID", review.RentalID);
            cmd.Parameters.AddWithValue("@bookID", review.ISBN);
            cmd.Parameters.AddWithValue("@content", review.Content);

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

        public static bool InactivateReview(Review review, bool toRestore)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_delete_review";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rentalID", review.RentalID);
            cmd.Parameters.AddWithValue("@bookID", review.ISBN);
            cmd.Parameters.AddWithValue("@active", toRestore);

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

        public static int CheckReview(int customerID, string isbn)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_check_review";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@bookID", isbn);

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
    }
}
