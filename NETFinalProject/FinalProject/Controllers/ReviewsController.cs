using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObjects;

namespace FinalProject.Controllers
{
    public class ReviewsController : Controller
    {
        BookManager bookManager = new BookManager();
        ReviewManager reviewManager = new ReviewManager();
        RentalManager rentalManager = new RentalManager();
        public ActionResult Reviews(string isbn)
        {
            AccessToken accessToken = ((AccessToken)Session["AccessToken"]);
            
            ViewBag.ISBN = isbn;
            List<ReviewDesc> reviews = reviewManager.GetReviewListByISBNDesc(isbn, Active.active);
            if(reviews.Count == 0)
            {
                ModelState.AddModelError("", "There are no reviews for this book");
            }
            return View(reviews);
        }

        [AuthorizeUser(Roles = "")]
        public ActionResult AddReview(string isbn)
        {
            AccessToken accessToken = ((AccessToken)Session["AccessToken"]);

            if(rentalManager.GetRentalByCustomerID(accessToken.User.CustomerID, isbn) == null)
            {
                Session["Error"] = "Please check out the book before leaving a review";
                return RedirectToAction("Reviews", "Reviews", new { isbn });
            }

            Session["Error"] = "";

            if (reviewManager.CheckReview(accessToken.User.CustomerID, isbn))
            {
                ViewBag.Content = reviewManager.GetReviewByCustomerBook(accessToken.User.UserName, isbn, Active.active).Content;
            }
            ViewBag.ISBN = isbn;
            return View();
        }

        [HttpPost]
        [AuthorizeUser(Roles = "")]
        public ActionResult AddReview(Review review)
        {
            AccessToken accessToken = ((AccessToken)Session["AccessToken"]);

            Rental rental = rentalManager.GetRentalByCustomerID(accessToken.User.CustomerID, review.ISBN);
            review.RentalID = rental.RentalID;

            if(ModelState.IsValid)
            {
                if(reviewManager.CheckReview(accessToken.User.CustomerID, review.ISBN))
                {
                    reviewManager.EditReview(review);
                }
                else
                {
                    reviewManager.AddReview(review);
                }

                return RedirectToAction("Reviews", "Reviews", new { review.ISBN });
            }

            return View();
        }
    }
}