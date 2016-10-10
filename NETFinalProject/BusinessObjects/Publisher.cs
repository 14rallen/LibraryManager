using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Publisher
    {
        [StringLength(50)]
        [Display(Name = "Publisher")]
        [Required]
        public string PublisherID { get; set; }
        [Phone]
        [StringLength(10)]
        [Required]
        public string Phone { get; set; }
        public bool Active { get; set; }

        public Publisher()
        {

        }

        public Publisher(string publisher, string phone)
        {
            PublisherID = publisher;
            Phone = phone;
        }

        public override string ToString()
        {
            return PublisherID;
        }
    }
}
