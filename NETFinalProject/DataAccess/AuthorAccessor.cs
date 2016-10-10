using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class AuthorAccessor
    {
        public static List<Author> FetchAuthorList(Active group = Active.active)
        {
            List<Author> authors = new List<Author>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT AuthorID, FirstName, LastName, Active " +
                           @"FROM Author ";

            if(group == Active.active)
            {
                query += @"WHERE Active = 1 ";
            }
            else if(group == Active.inactive)
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
                        Author currentAuthor = new Author()
                        {
                            AuthorID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Active = reader.GetBoolean(3)
                        };

                        authors.Add(currentAuthor);
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
            return authors;
        }

        public static Author FetchAuthorByID(int authorID)
        {
            Author author = null;

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_author_by_authorid";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@authorID", authorID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        author = new Author()
                        {
                            AuthorID = reader.GetInt32(0),
                            FirstName = reader.GetString(1),
                            LastName = reader.GetString(2),
                            Active = reader.GetBoolean(3)
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
            return author;
        }

        public static bool InsertAuthor(Author author)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_author";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@firstName", author.FirstName);
            cmd.Parameters.AddWithValue("@lastName", author.LastName);

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

        public static bool UpdateAuthor(Author author)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_author";
            var cmd = new SqlCommand(query, conn);

            Console.WriteLine(author.FirstName + author.LastName + author.AuthorID);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@firstName", author.FirstName);
            cmd.Parameters.AddWithValue("@lastName", author.LastName);
            cmd.Parameters.AddWithValue("@authorID", author.AuthorID);

            try
            {
                conn.Open();

                Console.WriteLine("it is " + cmd.ExecuteNonQuery());

                inserted = true;
                
            }
            catch (Exception)
            {
                throw;
            }

            //return count;
            return inserted;
        }

        public static bool InactivateAuthor(Author author, bool toRestore)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_delete_author";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@authorID", author.AuthorID);
            cmd.Parameters.AddWithValue("@active", toRestore);

            try
            {
                conn.Open();

                if(cmd.ExecuteNonQuery() == 1)
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
