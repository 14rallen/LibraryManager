using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class OverdueBook
    {
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string ISBN { get; set; }
        public string UserName { get; set; }
        public DateTime DateRented { get; set; }
        public DateTime DateDue { get; set; }
        public int DaysOverdue { get; set; }
        public decimal TotalFee { get; set; }
    }
}
