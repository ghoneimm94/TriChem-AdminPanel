using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TriChem.AdminPanel.Filters
{
    public class AuthorizeUserAttribute : AuthorizeAttribute
    {

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {

            filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary(new { controller = "Authentication", action = "Login" })
            );
        }

        //Core authentication, called before each action
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["username"] != null)
            {
                return true;
            }
            return false;
        }
    }

}
