using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TriChem.Domain.Models;

namespace TriChem.AdminPanel.Controllers
{
    public class AuthenticationController : Controller
    {
        // GET: Authentication  
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            using (var _db = new TriChemEntities())
            {
                var user = _db.User.Where(u => u.UserName == username && u.Password == password).FirstOrDefault();
                if (user == null)
                {
                    ViewBag.Message = "inavlid username or password";
                    return View();

                }
                Session["username"] = user.UserName;
                Session.Timeout = 100000;
            }
            return RedirectToAction("Index", "Home");
        }
    }
}