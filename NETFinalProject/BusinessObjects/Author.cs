using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Author
    {
        public int AuthorID { get; set; }
        [StringLength(50)]
        [Display(Name = "First Name")]
        [Required]
        public string FirstName { get; set; }
        [StringLength(50)]
        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }
        public bool Active { get; set; }

        public Author()
        {
            
        }

        public Author(int authorID, string firstName, string lastName)
        {
            AuthorID = authorID;
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString() 
        {
            return FirstName + " " + LastName;
        }
    }
}
