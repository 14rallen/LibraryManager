using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessObjects;
using BusinessLogic;

namespace FinalProject.Controllers
{
    public class RentalController : Controller
    {
        BookManager bookManager = new BookManager();
        RentalManager rentalManager = new RentalManager();
        // GET: Rental
        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "")]
        public ActionResult Checkout(Book book)
        {
            string submit = Session["submit"].ToString();

            if(submit == "Checkout")
            {
                if(rentalManager.AddNewRental(book, ((AccessToken)Session["AccessToken"]).User))
                {
                    return RedirectToAction("Details", "Book", book);
                }
                else
                {
                    ModelState.AddModelError("", "Could not checkout book");
                    return View(book);
                }
            }
            else if(submit == "Return Book")
            {
                if(rentalManager.DeleteRental(book, ((AccessToken)Session["AccessToken"]).User))
                {
                    return RedirectToAction("Details", "Book", book);
                }
                else
                {
                    ModelState.AddModelError("", "Could not return book");
                    return View(book);
                }
            }
            else
            {
                return View(book);
            }
        }

        [AuthorizeUser(Roles = "")]
        public ActionResult MyRentals()
        {
            List<Rental> rentals = new List<Rental>();
            Customer user = ((AccessToken)Session["AccessToken"]).User;

            try
            {
                rentals = rentalManager.GetRentalListByCustomerID(user.CustomerID);
            }
            catch (Exception)
            {
                rentals = new List<Rental>();
                ModelState.AddModelError("", "You do not have any books checked out");
            }
            List<Book> books = new List<Book>();

            foreach (Rental rental in rentals)
            {
                books.Add(bookManager.GetRentedBookByRentalID(rental.RentalID));
            }

            Session["RentedBooks"] = books;
            Session["Rentals"] = rentals;

            return View();
        }
    }
}