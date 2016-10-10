using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace BusinessObjects
{
    public class Book
    {
        [StringLength(20)]
        [Required]
        public string ISBN {get; set;}
        [Required]
        public string Publisher { get; set; }
        [Display(Name = "Author")]
        [Required]
        public int AuthorID { get; set; }
        [StringLength(50)]
        [Required]
        public string Title { get; set; }
        public int Pages { get; set; }
        [Display(Name = "In Stock")]
        [Required]
        public int Copies { get; set; }

        public bool Active { get; set; }

        public HttpPostedFileBase ImageFile { get; set; }

        public Book()
        {

        }

        public Book(string isbn, string publisher, int authorID, string title, int pages, int copies)
        {
            ISBN = isbn;
            Publisher = publisher;
            AuthorID = authorID;
            Title = title;
            Pages = pages;
            Copies = copies;
        }

        public override string ToString()
        {
            return Title;
        }
    }
}
