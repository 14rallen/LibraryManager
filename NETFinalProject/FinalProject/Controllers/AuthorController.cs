using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObjects;

namespace FinalProject.Controllers
{
    public class AuthorController : Controller
    {
        AuthorManager authorManager = new AuthorManager();
        // GET: Author
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddAuthor(string from)
        {
            Session["From"] = from;
            return View();
        }

        [HttpPost]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                authorManager.AddNewAuthor(author);
                if (Session["From"].ToString() == "add")
                {
                    return RedirectToAction("AddBook", "Book", null);
                }
                else if (Session["From"].ToString() == "edit")
                {
                    return RedirectToAction("EditBook", "Book", null);
                }
                else
                {
                    return RedirectToAction("Index", "Home", null);
                }
            }
            else
            {
                return View();
            }

        }


        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult EditAuthor(Author author)
        {
            if (ModelState.IsValid)
            {
                string submit = Request.Form["submit"];

                if (submit == "Edit")
                {
                    authorManager.EditAuthor(author);
                    return RedirectToAction("AddBook", "Book", null);
                }
                else
                {
                    return View(author);
                }
            }
            else
            {
                return View(author);
            }

        }
    }
}