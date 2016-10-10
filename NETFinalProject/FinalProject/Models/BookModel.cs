using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObjects;
using BusinessLogic;

namespace FinalProject.Models
{
    public class BookModel: Book
    {
        public string AuthorName { get; set; }
        public BookModel(Book book)
        {
            AuthorManager authorManager = new AuthorManager();
            Author author = authorManager.GetAuthorByID(book.AuthorID);
            AuthorName = author.LastName + ", " + author.FirstName;
            this.Title = book.Title;
            this.ISBN = book.ISBN;
        }

        public override string ToString()
        {
            return Title + " by " + AuthorName;
        }
    }
}