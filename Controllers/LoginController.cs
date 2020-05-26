using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wembsite.Models;
using System.Data.Entity;
using System.Net;
using System.Text.RegularExpressions;

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
            string model = "";
            if (confirmpassword!=upwd)
            {
                model = "Passwords do not match!";
                return View("SignUp", (object)model);
            }
            //At least 8 characters long 1 digits 1 Upper case 1 Lower case 1 Symbol
            var abc = "abcdefghijklmnopqrstuvwxyz";
            var ABC = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var symbol = "!@#$%^&*()";
            var digit = "1234567890";
            bool hasabc = false, hasABC = false, hasSymbol = false, hasDigit = false;
            for(int i=0; i < upwd.Length; ++i)
            {
                if (abc.Contains(upwd[i]))
                {
                    hasabc = true;
                }

                else if (ABC.Contains(upwd[i])){
                    hasABC = true;
                }

                else if (symbol.Contains(upwd[i]))
                {
                    hasSymbol = true;
                }

                else if (digit.Contains(upwd[i]))
                {
                    hasDigit = true;
                }
            }
            if (hasabc == false || hasABC == false || hasSymbol == false|| hasDigit == false)
            {
                model = "Password must have atleast 1 digit, 1 uppercase letter, 1 lowercase letter, and 1 symbol!";
                return View("SignUp", (object)model);
            }

            int result = CRUD.signUp(uname, fname, lname, email, upwd);
            if (result == 1)
            {
                Session["username"] = uname;
                return RedirectToAction("../UserHome/Profile");
            }

            else 
            {
                model = "UserID not unique";
                return View("SignUp", (object)model);
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