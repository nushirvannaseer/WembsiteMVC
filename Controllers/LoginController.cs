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

        //if logged in goto homepage
        public ActionResult Index()
        {
            if (Session["username"] != null)
                return RedirectToAction("../UserHome/Profile");
            // ViewBag.Message = "";
            return View();
        }

        //This is for the login page
        public ActionResult VerifyCredentials(string un, string upwd)
        {           
            //if logged in
            if(Session["username"]!=null)
                return RedirectToAction("../UserHome/Profile");

            User usr = CRUD.getUser(un);
            if (usr==null || usr.password!=upwd)
            {
               //if invalid login goto homepage 
                ViewData["Message"] = "Invalid username or password!";
                return View("Index");
            }
            
            Session["username"] = un;           
            return RedirectToAction("../UserHome/Profile");
        }

        //This is for the sign up page
        public ActionResult addNewUser(string fname, string lname, string uname, string email, string upwd, string confirmpassword)
        {
            if(confirmpassword!=upwd)
            {
                string model = "Passwords do not match!";
                return View("SignUp", (object)model);
            }

            int result = CRUD.signUp(uname, fname, lname, email, upwd);
            if (result == 1)
            {
                return RedirectToAction("../UserHome/Profile");
            }

            else 
            {
                string model = "UserID not unique";
                return View("Index", (object)model);
            }
            
        }

        //Simply redirects user to signup page
        public ActionResult SignUp()
        {
            return View("SignUp");
        }

        //Redirects to ForgotPassword Page
        public ActionResult ForgotPassword()
        {
            return View();
        }

        //sends password recovery code to user
        public ActionResult SendCodeToUser(string email)
        {
            Random random = new Random();
            random.Next(100000, 999999);
            return View("Index");
        }
    }
}