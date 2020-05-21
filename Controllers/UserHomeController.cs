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

        public ActionResult CreateNewPost()
        {
            return View();
        }

        public ActionResult PublishPost(string postContent, string privacy)
        {
            CRUD.NewPost(Session["username"].ToString(), postContent, privacy);
            return RedirectToAction("Profile");
        }

        public ActionResult AllPosts()
        {
            List<UserContent> postList=CRUD.AllPostsOfAUser(Session["username"].ToString());
            return View(postList);
        }

        public ActionResult ViewPost(int id)
        {
            UserContent post = CRUD.GetUserPost(id);
            return View(post);
        }

        public ActionResult EditPost(int id)
        {
            UserContent post=CRUD.GetUserPost(id);
            return View(post);
        }

        public ActionResult SaveEditChanges(int id, string privacy, string RawData)
        {
            CRUD.EditPost(id, privacy, RawData);
            return RedirectToAction("AllPosts");
        }

        public ActionResult DeletePost(int id)
        {
            CRUD.DeletePost(id);
            return RedirectToAction("AllPosts");
        }

        public ActionResult HomePage()
        {
            List<UserContent> postList = CRUD.HomepagePost(Session["username"].ToString());
            
            return View(postList);
        }

        public ActionResult LikePost(int contentID, string likedBy)
        {
            int originalLikes = Convert.ToInt32(CRUD.GetUserPost(contentID).likes);
            CRUD.LikePost(contentID, likedBy);
            ViewData["Message"] = "Unlike";
            //if like was already present
            if (originalLikes== Convert.ToInt32(CRUD.GetUserPost(contentID).likes))
            {
                CRUD.UnLikePost(contentID, likedBy);
                ViewData["Message"] = "Like";
            }
            UserContent post = CRUD.GetUserPost(contentID);
            return PartialView("_LikesPartialView", post);
        }

        public ActionResult GetLikeList(int contentID)
        {
            List<User> likers = CRUD.GetLikeList(contentID);
            return PartialView("_LikeListPartialView", likers);
        }

        public ActionResult _NonSessionUserPosts(string username)
        {
            List<UserContent> posts = CRUD.getNonSessionUserPosts(username, Session["username"].ToString());
            return View(posts);
        }

        public ActionResult Search(string searchText)
        {
            if (searchText == "")
            {
                return RedirectToAction("Profile");
            }
            ViewData["searchText"] = searchText;
            List<User> searchResults = CRUD.Search(searchText);
            return View("SearchResults", searchResults);
        }
    }
}
