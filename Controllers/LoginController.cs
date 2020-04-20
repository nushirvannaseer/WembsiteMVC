﻿using System;
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

        //This is for the login page
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
                //List<User> list = CRUD.getAllUsers();
                //return View("homePage", list);
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