using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessObjects;
using BusinessLogic;

namespace FinalProject.Models
{
    public class AutoCompleteISBNModel
    {
        public string label { get; set; }
        public string value { get; set; }

        public AutoCompleteISBNModel(Book book)
        {
            label = book.Title + " (" + book.ISBN + ")";
            value = book.ISBN;
        }
    }
}