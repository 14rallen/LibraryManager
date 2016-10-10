using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class Customer
    {
        public int CustomerID { get; set; }
        [Display(Name = "First Name")]
        [StringLength(50)]
        [Required]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        [StringLength(50)]
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [StringLength(50)]
        public string Email { get; set; }
        [Phone]
        [StringLength(10)]
        public string Phone { get; set; }
        [Display(Name = "Username")]
        [Required]
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Active { get; set; }

        [Compare("Password")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public Customer()
        {

        }

        public Customer(int customerID, string firstName, string lastName, string email,
            string phone, string userName, bool active)
        {
            CustomerID = customerID;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            UserName = userName;
            Active = active;
        }
    }
}
