using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class PublisherAccessor
    {
        public static List<Publisher> FetchPublisherList(Active group = Active.active)
        {
            List<Publisher> publishers = new List<Publisher>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT PublisherID, Phone, Active " +
                           @"FROM Publisher ";

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
                        Publisher currentPublisher = new Publisher()
                        {
                            PublisherID = reader.GetString(0),
                            Phone = reader.GetString(1)
                        };

                        publishers.Add(currentPublisher);
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
            return publishers;
        }

        public static Publisher FetchPublisherByID(string publisherID)
        {
            Publisher publisher = null;

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_publisher_by_publisherid";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@publisherID", publisherID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        publisher = new Publisher()
                        {
                            PublisherID = reader.GetString(0),
                            Phone = reader.GetString(1),
                            Active = reader.GetBoolean(2)
                        };
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
            return publisher;
        }

        public static bool InsertPublisher(Publisher publisher)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_publisher";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@publisherID", publisher.PublisherID);
            cmd.Parameters.AddWithValue("@phone", publisher.Phone);

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

        public static bool UpdatePublisher(Publisher publisher)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_publisher";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@publisherID", publisher.PublisherID);
            cmd.Parameters.AddWithValue("@phone", publisher.Phone);

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

        public static bool InactivatePublisher(Publisher publisher, bool toRestore)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_delete_publisher";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@publisherID", publisher.PublisherID);
            cmd.Parameters.AddWithValue("@active", toRestore);

            try
            {
                conn.Open();

                if (cmd.ExecuteNonQuery() == 1)
                {
                    inserted = true;
                }
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
