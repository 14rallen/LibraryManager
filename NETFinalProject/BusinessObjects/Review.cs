using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Review
    {
        public int RentalID { get; set; }
        [StringLength(20)]
        [Required]
        public string ISBN { get; set; }
        [StringLength(2000)]
        [Required]
        public string Content { get; set; }
        public bool Active { get; set; }

        public Review()
        {

        }

        public Review(int rentalID, string isbn, string content, bool active)
        {
            RentalID = rentalID;
            ISBN = isbn;
            Content = content;
            Active = active;
        }
    }
}
