using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BusinessObjects
{
    public class Order
    {
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public DateTime DateOrdered { get; set; }

        public Order()
        {

        }

        public Order(int orderID, int customerID, DateTime dateOrdered)
        {
            OrderID = orderID;
            CustomerID = customerID;
            DateOrdered = dateOrdered;
        }
    }
}
