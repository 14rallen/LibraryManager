using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BusinessLogic;
using BusinessObjects;

namespace FinalProject.Models
{
    public class AutoCompleteAuthorModel : Author
    {
        public string label { get; set; }
        public string value { get; set; }

        public AutoCompleteAuthorModel(Author author)
        {
            label = author.ToString();
            value = author.AuthorID.ToString();
        }
    }
}