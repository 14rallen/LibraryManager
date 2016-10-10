using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObjects;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class OrderController : Controller
    {
        OrderManager orderManager = new OrderManager();
        BookManager bookManager = new BookManager();
        PageDetails pageDetails = new PageDetails();

        public ActionResult AutocompleteTitle(string term)
        {
            var books = bookManager.GetBookListByTitle(term);

            var filteredItems = books.Where(book => book.Title.ToLower().Contains(term.ToLower())).ToList().ConvertAll(obj => obj.ToString());
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompleteISBN(string term)
        {
            var books = bookManager.GetBookListByISBN(term);
            var bookModels = books.ConvertAll(obj => new AutoCompleteISBNModel(obj));

            var filteredItems = bookModels.Where(book => book.label.ToLower().Contains(term.ToLower())).ToList();
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        // GET: Order
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddOrder()
        {
            ViewBag.Books = bookManager.GetBookList(Active.active);

            List<BookModel> models = new List<BookModel>();

            foreach (Book book in ViewBag.Books)
            {
                models.Add(new BookModel(book));
            }

            ViewBag.Books = models;

            return View();

        }

        [HttpPost]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddOrder(List<Order> orders)
        {
            ViewBag.Books = bookManager.GetBookList(Active.active);

            List<BookModel> models = new List<BookModel>();

            foreach (Book book in ViewBag.Books)
            {
                models.Add(new BookModel(book));
            }

            ViewBag.Books = models;

            Customer user = ((AccessToken)Session["AccessToken"]).User;

            string quantity = Request.Form["OrderQuantity"];
            string isbn = Request.Form["searchISBN"];
            int qty = 0;

            if (!int.TryParse(quantity, out qty))
            {
                ModelState.AddModelError("", "Quantity must be a number");
                return View(orders);
            }

            if (ModelState.IsValid)
            {
                if (orderManager.AddOrders(user.CustomerID, isbn, qty))
                {
                    return RedirectToAction("Orders");
                }
                else
                {
                    ModelState.AddModelError("", "Order could not be completed");
                    return View(orders);
                }
            }
            else
            {
                return View(orders);
            }

        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult Orders(int? page)
        {
            List<OrderDesc> orders;
            try
            {
                orders = orderManager.GetOrderListDesc();
            }
            catch (Exception)
            {
                orders = new List<OrderDesc>();
            }

            pageDetails.Count = orders.Count;
            pageDetails.PerPage = 5;

            if (page == null)
            {
                pageDetails.CurrentPage = 1;
            }
            else
            {
                if (page <= 1)
                {
                    pageDetails.CurrentPage = 1;
                }
                else if (page >= pageDetails.MaxPages)
                {
                    pageDetails.CurrentPage = pageDetails.MaxPages;
                }
                else
                {
                    pageDetails.CurrentPage = (int)page;
                }
            }

            Session["PageDetails"] = pageDetails;
            return View(orders);
        }
    }
}