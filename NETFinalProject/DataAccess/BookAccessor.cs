using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using System.Data.SqlClient;

namespace DataAccess
{
    public class BookAccessor
    {
        public static List<Book> FetchBookList(Active group = Active.active)
        {
            List<Book> books = new List<Book>();

            SqlConnection conn = DBConnection.GetDBConnection();

            string query = @"SELECT BookID, PublisherID, AuthorID, " +
                           @"Title, Pages, Copies, Active " +
                           @"FROM Book ";

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
                        Book currentBook = new Book()
                        {
                            ISBN = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            AuthorID = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Pages = reader.GetInt32(4),
                            Copies = reader.GetInt32(5),
                            Active = reader.GetBoolean(6)
                        };

                        books.Add(currentBook);
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
            return books;
        }

        public static List<Book> FetchBookListByAuthorID(int authorID)
        {
            List<Book> books = new List<Book>();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_book_by_author";
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
                        Book currentBook = new Book()
                        {
                            ISBN = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            AuthorID = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Pages = reader.GetInt32(4),
                            Copies = reader.GetInt32(5),
                            Active = reader.GetBoolean(6)
                        };

                        books.Add(currentBook);
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
            return books;
        }

        public static List<Book> FetchBookListByCustomerID(int customerID)
        {
            List<Book> books = new List<Book>();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_book_by_customer";
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
                        Book currentBook = new Book()
                        {
                            ISBN = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            AuthorID = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Pages = reader.GetInt32(4),
                            Copies = reader.GetInt32(5),
                            Active = reader.GetBoolean(6)
                        };

                        books.Add(currentBook);
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
            return books;
        }

        public static Book FetchBookByRentalID(int rentalID)
        {
            Book book = new Book();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_book_by_rental";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@rentalID", rentalID);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while(reader.Read())
                {
                    book = new Book()
                    {
                        ISBN = reader.GetString(0),
                        Publisher = reader.GetString(1),
                        AuthorID = reader.GetInt32(2),
                        Title = reader.GetString(3),
                        Pages = reader.GetInt32(4),
                        Copies = reader.GetInt32(5),
                        Active = reader.GetBoolean(6)
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
            return book;
        }

        public static List<Book> FetchBookListByISBN(string isbn)
        {
            List<Book> books = new List<Book>();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_book_by_isbn";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@isbn", isbn);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Book currentBook = new Book()
                        {
                            ISBN = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            AuthorID = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Pages = reader.GetInt32(4),
                            Copies = reader.GetInt32(5),
                            Active = reader.GetBoolean(6)
                        };

                        books.Add(currentBook);
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
            return books;
        }

        public static List<Book> FetchBookListByTitle(string title)
        {
            List<Book> books = new List<Book>();

            var conn = DBConnection.GetDBConnection();
            var query = @"sp_select_book_by_title";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@title", title);

            try
            {
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Book currentBook = new Book()
                        {
                            ISBN = reader.GetString(0),
                            Publisher = reader.GetString(1),
                            AuthorID = reader.GetInt32(2),
                            Title = reader.GetString(3),
                            Pages = reader.GetInt32(4),
                            Copies = reader.GetInt32(5),
                            Active = reader.GetBoolean(6)
                        };

                        books.Add(currentBook);
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
            return books;
        }

        public static bool InsertBook(Book book)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_insert_book";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@bookID", book.ISBN);
            cmd.Parameters.AddWithValue("@publisherID", book.Publisher);
            cmd.Parameters.AddWithValue("@authorID", book.AuthorID);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@pages", book.Pages);
            cmd.Parameters.AddWithValue("@copies", book.Copies);

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

        public static bool UpdateBook(Book book)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_update_book";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@bookID", book.ISBN);
            cmd.Parameters.AddWithValue("@publisherID", book.Publisher);
            cmd.Parameters.AddWithValue("@authorID", book.AuthorID);
            cmd.Parameters.AddWithValue("@title", book.Title);
            cmd.Parameters.AddWithValue("@pages", book.Pages);
            cmd.Parameters.AddWithValue("@copies", book.Copies);

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

        public static bool InactivateBook(Book book, bool toRestore)
        {
            //int count = 0;
            bool inserted = false;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_delete_book";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@bookID", book.ISBN);
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

        public static int CheckBookID(string bookID)
        {
            int count = 0;
            var conn = DBConnection.GetDBConnection();
            var query = @"sp_check_bookid";
            var cmd = new SqlCommand(query, conn);

            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@bookID", bookID);

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
