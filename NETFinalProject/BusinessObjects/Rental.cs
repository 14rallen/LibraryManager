using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class Rental
    {
        public int RentalID { get; set; }
        public int CustomerID { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime DateDue { get; set; }
        public decimal LateFee { get; set; }
        public bool Active { get; set; }

        public Rental()
        {

        }

        public Rental(int rentalID, int customerID, DateTime dateRented, DateTime dateDue, decimal lateFee, bool active)
        {
            RentalID = rentalID;
            CustomerID = customerID;
            DateRented = dateRented;
            LateFee = lateFee;
            Active = active;
        }
    }
}
