using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class RentalManager
    {
        private decimal dailyLateFee = 0.25M;

        public List<Rental> GetRentalList(Active group = Active.active)
        {
            try
            {
                var rentalList = RentalAccessor.FetchRentalList(group);

                if (rentalList.Count > 0)
                {
                    return rentalList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Rental> GetRentalListByCustomerID(int customerID)
        {
            try
            {
                var rentalList = RentalAccessor.FetchRentalListByCustomerID(customerID);

                if (rentalList.Count > 0)
                {
                    return rentalList;
                }
                else
                {
                    throw new ApplicationException("There were no records found.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Rental GetRentalByCustomerID(int customerID, string isbn)
        {
            try
            {
                var rental = RentalAccessor.FetchRentalByCustomerID(customerID, isbn);

                return rental;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool AddNewRental(Book book, Customer user)
        {
            Rental rental;

            DateTime dateRented = DateTime.Now;
            DateTime dateDue = dateRented.AddDays(14);

            rental = new Rental()
            {
                CustomerID = user.CustomerID,
                DateRented = dateRented,
                DateDue = dateDue,
                LateFee = dailyLateFee
            };

            try
            {
                if (RentalAccessor.InsertRental(rental, book.ISBN))
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public bool DeleteRental(Book book, Customer user)
        {
            try
            {
                if (RentalAccessor.InactivateRental(book, user))
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }

            return false;
        }

        public Rental GetRentalForRentalWindow()
        {
            Rental rental;

            DateTime dateRented = DateTime.Now;
            DateTime dateDue = dateRented.AddDays(14);

            rental = new Rental()
            {
                DateRented = dateRented,
                DateDue = dateDue,
                LateFee = dailyLateFee
            };

            return rental;
        }


    }
}
