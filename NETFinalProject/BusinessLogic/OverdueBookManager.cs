using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace BusinessLogic
{
    public class OverdueBookManager
    {
        private RentalManager rentalManager = new RentalManager();
        private BookManager bookManager = new BookManager();
        private CustomerManager customerManager = new CustomerManager();

        public List<OverdueBook> GetOverdueBooks()
        {
            List<OverdueBook> overdueBooks = new List<OverdueBook>();

            List<Rental> rentals = rentalManager.GetRentalList();

            for (int i = 0; i < rentals.Count; i++ )
            {
                OverdueBook overdueBook = new OverdueBook();
                overdueBook.DateRented = rentals[i].DateRented;
                overdueBook.DateDue = rentals[i].DateDue;

                int daysOverdue = (int)(DateTime.Now - overdueBook.DateDue).TotalDays;

                Console.WriteLine(daysOverdue);

                if(daysOverdue > 0)
                {
                    overdueBook.DaysOverdue = daysOverdue;
                    overdueBook.TotalFee = rentals[0].LateFee * overdueBook.DaysOverdue;

                    Customer customer = customerManager.GetCustomerByID(rentals[i].CustomerID);
                    overdueBook.UserName = customer.UserName;

                    BookDesc book = bookManager.GetRentedBookByRentalIDDesc(rentals[i].RentalID);
                    overdueBook.Title = book.Title;
                    overdueBook.ISBN = book.ISBN;
                    overdueBook.AuthorName = book.AuthorName;

                    overdueBooks.Add(overdueBook);
                }
            }

            return overdueBooks;
        }
    }
}
