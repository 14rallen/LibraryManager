using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BusinessLogic;
using BusinessObjects;

namespace FinalProject
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            HttpContext context = HttpContext.Current;
            AccessToken accessToken = (AccessToken)context.Session["AccessToken"];

            if (accessToken != null)
            {
                string[] roles;
                roles = Roles.Split(new char[]{','});

                if (!String.IsNullOrEmpty(Roles))
                {
                    if (!accessToken.IsUserInRoles(roles))
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Login", action = "Login" }));
                    }
                }
            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Login", action = "Login" }));
            }
        }
    }
}