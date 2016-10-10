using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class BookManager
    {
        /*
         * converts a list of books to a list of book descriptions
         */ 
        public List<BookDesc> ConvertBookListToDesc(List<Book> books)
        {
            AuthorManager authorManager = new AuthorManager();
            List<BookDesc> bookDescs = new List<BookDesc>();

            foreach (Book book in books)
            {
                Author author = authorManager.GetAuthorByID(book.AuthorID);

                BookDesc bookDesc = new BookDesc();

                bookDesc.Title = book.Title;
                bookDesc.ISBN = book.ISBN;
                bookDesc.AuthorName = author.LastName + ", " + author.FirstName;
                bookDesc.AuthorID = book.AuthorID;
                bookDesc.Publisher = book.Publisher;
                bookDesc.Pages = book.Pages;
                bookDesc.Copies = book.Copies;
                bookDesc.Active = book.Active;

                bookDescs.Add(bookDesc);
            }

            return bookDescs;
        }

        public BookDesc ConvertBookToDesc(Book book)
        {
            AuthorManager authorManager = new AuthorManager();
            Author author = authorManager.GetAuthorByID(book.AuthorID);
            BookDesc bookDesc = new BookDesc();

            bookDesc.Title = book.Title;
            bookDesc.ISBN = book.ISBN;
            bookDesc.AuthorName = author.LastName + ", " + author.FirstName;
            bookDesc.AuthorID = book.AuthorID;
            bookDesc.Publisher = book.Publisher;
            bookDesc.Pages = book.Pages;
            bookDesc.Copies = book.Copies;
            bookDesc.Active = book.Active;

            return bookDesc;
        }

        public List<Book> GetBookList(Active group)
        {
            try
            {
                var bookList = BookAccessor.FetchBookList(group);

                if (bookList.Count > 0)
                {
                    return bookList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (ApplicationException)
            {
                throw new ApplicationException("There were no records found.");
            }
        }

        public List<BookDesc> GetBookListDesc(Active group)
        {
            List<Book> books = GetBookList(group);
            List<BookDesc> bookDescs = ConvertBookListToDesc(books);
            return bookDescs;
        }

        public List<Book> GetBookListByAuthorID(int authorID)
        {
            try
            {
                var bookList = BookAccessor.FetchBookListByAuthorID(authorID);

                if (bookList.Count > 0)
                {
                    return bookList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BookDesc> GetBookListByAuthorIDDesc(int authorID)
        {
            List<Book> books = GetBookListByAuthorID(authorID);
            List<BookDesc> bookDescs = ConvertBookListToDesc(books);
            return bookDescs;
        }

        public List<Book> GetRentedBookListByCustomerID(int customerID)
        {
            try
            {
                var bookList = BookAccessor.FetchBookListByCustomerID(customerID);

                if (bookList.Count > 0)
                {
                    return bookList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BookDesc> GetRentedBookListByCustomerIDDesc(int customerID)
        {
            List<Book> books = GetRentedBookListByCustomerID(customerID);
            List<BookDesc> bookDescs = ConvertBookListToDesc(books);
            return bookDescs;
        }

        public Book GetRentedBookByRentalID(int rentalID)
        {
            try
            {
                var book = BookAccessor.FetchBookByRentalID(rentalID);

                if (book != null)
                {
                    return book;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BookDesc GetRentedBookByRentalIDDesc(int rentalID)
        {
            Book book = GetRentedBookByRentalID(rentalID);
            BookDesc bookDesc = ConvertBookToDesc(book);
            return bookDesc;
        }

        public List<Book> GetBookListByISBN(string isbn)
        {
            try
            {
                var bookList = BookAccessor.FetchBookListByISBN(isbn);

                if (bookList.Count > 0)
                {
                    return bookList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BookDesc> GetBookListByISBNDesc(string isbn)
        {
            List<Book> books = GetBookListByISBN(isbn);
            List<BookDesc> bookDescs = ConvertBookListToDesc(books);
            return bookDescs;
        }

        public List<Book> GetBookListByTitle(string title)
        {
            try
            {
                var bookList = BookAccessor.FetchBookListByTitle(title);

                if (bookList.Count > 0)
                {
                    return bookList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<BookDesc> GetBookListByTitleDesc(string title)
        {
            List<Book> books = GetBookListByTitle(title);
            List<BookDesc> bookDescs = ConvertBookListToDesc(books);
            return bookDescs;
        }

        public bool AddNewBook(Book book)
        {
            Console.WriteLine(book.Title);

            try
            {
                if (BookAccessor.InsertBook(book))
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool EditBook(Book book)
        {
            try
            {
                if (BookAccessor.UpdateBook(book))
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool DeleteBook(Book book, bool toRestore)
        {
            try
            {
                if (BookAccessor.InactivateBook(book, toRestore))
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        /**
         * checks if the isbn already exists
         */ 
        public bool CheckISBN(string isbn)
        {
            try
            {
                if (BookAccessor.CheckBookID(isbn) == 1)
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool IsBookRented(Book book, Customer customer)
        {
            bool isRented = false;

            try
            {
                List<Book> rentedBooks = GetRentedBookListByCustomerID(customer.CustomerID);

                foreach (Book rentedBook in rentedBooks)
                {
                    if (book.ISBN == rentedBook.ISBN)
                    {
                        isRented = true;
                        break;
                    }
                }
            }
            catch(Exception)
            {
                isRented = false;
            }

            return isRented;
        }
    }
}
