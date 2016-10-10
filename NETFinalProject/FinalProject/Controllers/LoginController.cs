using BusinessLogic;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FinalProject.Controllers
{
    public class LoginController : Controller
    {
        CustomerManager customerManager = new CustomerManager();
        
        public ActionResult Index()
        {
            return View();
        }


        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Customer model, string returnUrl)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(model);
            //}

            bool verified = false;

            try
            {
                verified = SecurityManager.ValidateExistingUser(model.UserName, model.Password);

                if (verified)
                {
                    Customer customer = customerManager.GetCustomerByUserName(model.UserName);
                    this.Session["AccessToken"] = SecurityManager.LogInUser(customer);

                    return View("~/Views/Home/Index.cshtml");
                }
                else
                {
                    ModelState.AddModelError("", "Username or password is incorrect");
                    return View();
                }
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
        }

        public ActionResult Logoff()
        {
            this.Session.Abandon();
            Session.Clear();
            return View("../Login/Login");
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Customer customer, string confirmPassword)
        {
            if (customerManager.CheckUserName(customer.UserName))
            {
                ModelState.AddModelError("", "Username already exists");
                return View();
            }
            else if (customer.Password != customer.ConfirmPassword)
            {
                ModelState.AddModelError("", "Passwords do not match");
                return View();
            }
            else
            {
                try
                {
                    customerManager.AddCustomer(customer);
                    Customer user = customerManager.GetCustomerByUserName(customer.UserName);
                    Session["AccessToken"] = SecurityManager.LogInUser(user);
                    return View("../Home/Index");
                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View();
                }
            }

            
        }

        [AuthorizeUser(Roles = "")]
        public ActionResult ViewProfile(Customer customer)
        {
            return View(customer);
        }

        [AuthorizeUser(Roles = "")]
        public ActionResult EditProfile()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [AuthorizeUser(Roles = "")]
        public ActionResult EditProfile(Customer customer)
        {
            try
            {
                if(customerManager.EditCustomer(customer))
                {
                    AccessToken accessToken = (AccessToken)Session["AccessToken"];
                    accessToken.User = customerManager.GetCustomerByID(accessToken.User.CustomerID);
                    Session["AccessToken"] = accessToken;
                    return View("../Home/Index");
                }
                else
                {
                    ModelState.AddModelError("", "Profile could not be edited");
                    return View();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}