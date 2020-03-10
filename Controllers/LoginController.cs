using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wembsite.Models;
using System.Data.Entity;
using System.Net;


namespace Wembsite.Controllers
{
    public class LoginController : Controller
    {
        private UserDBContext db = new UserDBContext();
        // GET: Login
        public ActionResult Index()
        {
            // ViewBag.Message = "";
            return View();
        }

        public ActionResult VerifyCredentials(string un, string upwd)
        {
            User usr = db.userList.Find(un);
            if(usr==null || usr.password!=upwd)
            {
                
                ViewData["Message"] = "Invalid username or password!";
                return View("Index");
            }
           
            return RedirectToAction("../UserHome/Profile", usr);
        }

        public ActionResult addNewUser(string fname, string lname, string uname, string email, string upwd, string confirmpassword)
        {
            User usr = new User(uname, fname, lname, email, upwd);
            db.userList.Add(usr);
            db.SaveChanges();
            return View("Index");
        }

        public ActionResult SignUp()
        {
            return View("SignUp");
        }
    }
}