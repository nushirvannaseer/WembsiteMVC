using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Wembsite.Models;

namespace Wembsite.Controllers
{
    public class UserHomeController : Controller
    {
        private UserDBContext db = new UserDBContext();
        public  ActionResult Profile()
        {
            if (Session["username"] == null)
                return RedirectToAction("../Login/Index");
            User user = CRUD.getUser(Session["username"].ToString());
            return View(user);
        }

        public ActionResult Followers(string username)
        {
            List<User> users = CRUD.DisplayFollowersOfAUser(username);
            return View("Followers", users);
        }

        public ActionResult Following(string username)
        {
            List<User> users = CRUD.DisplayFollowingsOfAUser(username);
            return View("Following", users);
        }

        public ActionResult FollowRequests(string username)
        {
            List<User> users = CRUD.DisplayFollowRequestsOfAUser(username);
            return View("FollowRequests", users);
        }

        public ActionResult SendFollowRequest(string usernameB)
        {
            CRUD.SendFollowRequest( Session["username"].ToString(), usernameB);
            List<User> users = CRUD.AllUsers();
            return View("../AllUsers/Index", users);
        }

        public ActionResult CancelFollowRequest(string usernameB)
        {
            CRUD.CancelFollowRequest(Session["username"].ToString(), usernameB);
            List<User> users = CRUD.AllUsers();
            return View("../AllUsers/Index", users);
        }

        public ActionResult AcceptFollowRequest(string sender)
        {
            CRUD.AcceptFollowRequest(sender, Session["username"].ToString());
            List<User> users = CRUD.AllUsers();
            return View("../AllUsers/Index", users);
        }

        public ActionResult DeleteFollowRequest(string sender)
        {
            CRUD.CancelFollowRequest(sender, Session["username"].ToString());
            List<User> users = CRUD.AllUsers();
            return View("../AllUsers/Index", users);
        }

        public ActionResult LogoutUser()
        {
            Session["username"] = null;
            return RedirectToAction("../Login/Index");
        }

        public ActionResult DeleteFollower(string followee)
        {
            CRUD.DeleteFollower(followee, Session["username"].ToString());
            List<User> users = CRUD.AllUsers();
            return View("../AllUsers/Index", users);
        }
    }
}
