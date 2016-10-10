using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccess;

namespace BusinessLogic
{
    public class ReviewManager
    {
        BookManager bookManager = new BookManager();
        RentalManager rentalManager = new RentalManager();
        CustomerManager customerManager = new CustomerManager();

        public List<Review> GetReviewList(Active group)
        {
            try
            {
                var reviewList = ReviewAccessor.FetchReviewList(group);

                if (reviewList.Count > 0)
                {
                    return reviewList;
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

        public List<Review> GetReviewListByISBN(string isbn, Active group)
        {
            List<Review> reviewsByISBN = new List<Review>();

            try
            {
                var reviewList = GetReviewList(group);

                foreach (Review review in reviewList)
                {
                    if (review.ISBN == isbn)
                    {
                        reviewsByISBN.Add(review);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return reviewsByISBN;
        }

        public List<ReviewDesc> GetReviewListByISBNDesc(string isbn, Active group)
        {
            List<Review> reviewsByISBN = new List<Review>();
            reviewsByISBN = GetReviewListByISBN(isbn, group);

            List<ReviewDesc> reviewDescs = new List<ReviewDesc>();

            foreach(Review review in reviewsByISBN)
            {
                ReviewDesc reviewDesc = new ReviewDesc();
                reviewDesc.Content = review.Content;
                reviewDesc.RentalID = review.RentalID;
                reviewDesc.ISBN = review.ISBN;
                reviewDesc.Active = review.Active;

                List<Rental> rentals = rentalManager.GetRentalList(Active.both);
                Customer user = new Customer();
                foreach(Rental rental in rentals)
                {
                    if(rental.RentalID == review.RentalID)
                    {
                        reviewDesc.UserName = customerManager.GetCustomerByID(rental.CustomerID).UserName;
                        break;
                    }
                }

                reviewDescs.Add(reviewDesc);

            }



            return reviewDescs;
        }

        /*
         * gets the one review a user has left on a specified book
         */ 
        public Review GetReviewByCustomerBook(string username, string isbn, Active group)
        {
            Review review = new Review();

            List<ReviewDesc> reviews = GetReviewListByISBNDesc(isbn, group);

            foreach(ReviewDesc reviewDesc in reviews)
            {
                if(reviewDesc.UserName == username)
                {
                    review.ISBN = reviewDesc.ISBN;
                    review.RentalID = reviewDesc.RentalID;
                    review.Content = reviewDesc.Content;
                    review.Active = reviewDesc.Active;
                    break;
                }
            }

            return review;
        }

        public bool AddReview(Review review)
        {
            try
            {
                if (ReviewAccessor.InsertReview(review))
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

        public bool EditReview(Review review)
        {
            try
            {
                if (ReviewAccessor.UpdateReview(review))
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

        public bool DeleteReview(Review review, bool toRestore)
        {
            try
            {
                if (ReviewAccessor.InactivateReview(review, toRestore))
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

        /*
         * checks if the customer has already left a review on a specified book
         */ 
        public bool CheckReview(int customerID, string isbn)
        {
            try
            {
                if (ReviewAccessor.CheckReview(customerID, isbn) >= 1)
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
    }
}
