using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class OrderDesc : Order
    {
        public string Title { get; set; }
        public string StaffName { get; set; }
        public int Quantity { get; set; }
    }
}
