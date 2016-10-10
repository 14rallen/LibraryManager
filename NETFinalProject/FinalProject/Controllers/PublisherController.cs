using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BusinessLogic;
using BusinessObjects;

namespace FinalProject.Controllers
{
    public class PublisherController : Controller
    {
        PublisherManager publisherManager = new PublisherManager();
        // GET: Author
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddPublisher()
        {
            return View();
        }

        [HttpPost]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult AddPublisher(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    publisherManager.AddNewPublisher(publisher);
                }
                catch(Exception)
                {
                    ModelState.AddModelError("", "Publisher could not be added. The publisher might already exist.");
                    return View();
                }
                return RedirectToAction("AddBook", "Book", null);
            }
            else
            {
                return View();
            }

        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Get)]
        [AuthorizeUser(Roles = "Admin,Employee")]
        public ActionResult EditPublisher(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                string submit = Request.Form["submit"];

                if (submit == "Edit")
                {
                    publisherManager.EditPublisher(publisher);
                    return RedirectToAction("AddBook", "Book", null);
                }
                else
                {
                    return View(publisher);
                }
            }
            else
            {
                return View(publisher);
            }

        }
    }
}