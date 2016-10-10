using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObjects;
using FinalProject.Models;

namespace FinalProject.Controllers
{
    public class BookController : Controller
    {
        BookManager bookManager = new BookManager();
        PageDetails page = new PageDetails();
        AuthorManager authorManager = new AuthorManager();
        PublisherManager publisherManager = new PublisherManager();

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

        public ActionResult AutocompleteAuthor(string term)
        {
            var authors = authorManager.GetAuthorList(Active.active);
            var authorModels = authors.ConvertAll(obj => new AutoCompleteAuthorModel(obj));

            var filteredItems = authorModels.Where(author => author.label.ToLower().Contains(term.ToLower())).ToList();
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AutocompletePublisher(string term)
        {
            var publishers = publisherManager.GetPublisherList(Active.active);

            var filteredItems = publishers.Where(publisher => publisher.PublisherID.ToLower().Contains(term.ToLower())).ToList().ConvertAll(obj => obj.ToString());
            return Json(filteredItems, JsonRequestBehavior.AllowGet);
        }

        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Books(int? currentPage)
        {
            Session["Pages"] = new int[] { 5, 10, 20, 50, 100 };
            Session["PerPage"] = 5;

            List<Book> books;
            
            try
            {
                if(Session["Books"] != null)
                {
                    books = (List<Book>)Session["Books"];
                }
                else
                {
                    books = bookManager.GetBookList(Active.active);
                }
            }
            catch (Exception)
            {
                books = new List<Book>();
            }

            page.Count = books.Count;
            page.PerPage = Session["PerPage"] == null ? 5 : (int)Session["PerPage"];

            if (currentPage == null)
            {
                page.CurrentPage = 1;
            }
            else
            {
                if (currentPage <= 1)
                {
                    page.CurrentPage = 1;
                }
                else if (currentPage >= page.MaxPages)
                {
                    page.CurrentPage = page.MaxPages;
                }
                else
                {
                    page.CurrentPage = (int)currentPage;
                }
            }

            page.PerPage = 5;
            Session["PageDetails"] = page;

            return View(books);
        }

        [HttpPost]
        public ActionResult Books(int? currentPage, string searchType)
        {
            List<Book> books;
            string title = Request.Form["searchTitle"];
            string isbn = Request.Form["searchISBN"];
            string authorIDText = Request.Form["searchAuthor-id"];
            int authorID = 0;
            searchType = Request.Form["submit"];

            try
            {
                if (Session["PerPage"].ToString() == Request.Form["PerPage"].ToString())
                {
                    if (searchType == "Search Title")
                    {
                        try
                        {
                            books = bookManager.GetBookListByTitle(title);
                        }
                        catch (Exception)
                        {
                            books = new List<Book>();
                            ModelState.AddModelError("", "There are no books with this title");
                        }
                    }
                    else if (searchType == "Search ISBN")
                    {
                        try
                        {
                            books = bookManager.GetBookListByISBN(isbn);
                        }
                        catch(Exception)
                        {
                            books = new List<Book>();
                            ModelState.AddModelError("", "There are no books with this ISBN");
                        }
                    }
                    else if (searchType == "Search Author")
                    {
                        if (int.TryParse(authorIDText, out authorID))
                        {
                            books = bookManager.GetBookListByAuthorID(authorID);
                        }
                        else
                        {
                            books = new List<Book>();
                            ModelState.AddModelError("", "There are no books by this author");
                        }
                    }
                    else
                    {
                        books = bookManager.GetBookList(Active.active);
                    }
                }
                else
                {
                    books = bookManager.GetBookList(Active.active);
                }
            }
            catch (Exception)
            {
                books = new List<Book>();
            }

            Session["Books"] = books;

            page.Count = books.Count;
            string perPageText = Request.Form["PerPage"];
            int perPage = 5;
            int.TryParse(perPageText, out perPage);
            page.PerPage = perPage;

            Session["PerPage"] = perPage;

            if (currentPage == null)
            {
                page.CurrentPage = 1;
            }
            else
            {
                if (currentPage <= 1)
                {
                    page.CurrentPage = 1;
                }
                else if (currentPage >= page.MaxPages)
                {
                    page.CurrentPage = page.MaxPages;
                }
                else
                {
                    page.CurrentPage = (int)currentPage;
                }
            }

            page.PerPage = 5;
            Session["PageDetails"] = page;
            Session["BookTitle"] = title;
            Session["ISBN"] = isbn;
            Session["Author"] = authorID;
            Session["SearchType"] = searchType;

            return PartialView("BooksList", books);
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        public ActionResult Details(Book book)
        {
            if (book.ImageFile != null)
            {
                book.ImageFile.SaveAs(Server.MapPath(@"~\Content\Images\" + book.ISBN + ".png"));
                return View(book);
            }

            return View(book);
        }

        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddBook()
        {
            ViewBag.Authors = authorManager.GetAuthorList(Active.active);
            ViewBag.Publishers = publisherManager.GetPublisherList(Active.active);
            return View();
        }

        [HttpPost]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddBook(Book book)
        {
            Author author = null;
            author = authorManager.GetAuthorByID(book.AuthorID);
            Publisher publisher = null;
            publisher = publisherManager.GetPublisherByID(book.Publisher);

            string authorIDText = Request.Form["AuthorID"];
            int authorID = 0;
            int.TryParse(authorIDText, out authorID);
            book.AuthorID = authorID;
            try
            {
                UpdateModel<Book>(book);
            }
            catch (Exception) { /* book model cannot be updated */ }

            if (ModelState.IsValid)
            {
                if (bookManager.CheckISBN(book.ISBN))
                {
                    ModelState.AddModelError("", "ISBN already exists");
                    return View();
                }
                else if(author == null)
                {
                    ModelState.AddModelError("", "Author does not exist");
                    return View();
                }
                else if (publisher == null)
                {
                    ModelState.AddModelError("", "Publisher does not exist");
                    return View();
                }
                else
                {
                    bookManager.AddNewBook(book);
                    return RedirectToAction("Details", "Book", book);
                }
            }
            else
            {
                return View();
            }

        }


        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult EditBook(Book book)
        {
            ViewBag.Authors = authorManager.GetAuthorList(Active.active);
            ViewBag.Publishers = publisherManager.GetPublisherList(Active.active);

            Author author = null;
            author = authorManager.GetAuthorByID(book.AuthorID);
            Publisher publisher = null;
            publisher = publisherManager.GetPublisherByID(book.Publisher);

            string authorIDText = Request.Form["AuthorID"];
            int authorID = 0;
            int.TryParse(authorIDText, out authorID);
            book.AuthorID = authorID;
            try
            {
                UpdateModel<Book>(book);
            }
            catch (Exception) { /* book model cannot be updated */ }

            if (ModelState.IsValid)
            {
                string submit = Request.Form["submit"];

                if (submit == "Edit")
                {
                    if (author == null)
                    {
                        ModelState.AddModelError("", "Author does not exist");
                        return View(book);
                    }
                    else if (publisher == null)
                    {
                        ModelState.AddModelError("", "Publisher does not exist");
                        return View(book);
                    }
                    else
                    {
                        bookManager.EditBook(book);
                        return RedirectToAction("Books");
                    }
                }
                else
                {
                    return View(book);
                }
            }
            else
            {
                return View(book);
            }

        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult DeleteBook(Book book)
        {
            if (bookManager.DeleteBook(book, false))
            {
                return RedirectToAction("Books");
            }
            else
            {
                ModelState.AddModelError("", "Book could not be deleted");
                return RedirectToAction("Books");
            }

        }
    }
}