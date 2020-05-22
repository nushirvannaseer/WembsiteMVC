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

        public ActionResult SendFollowRequest(string usernameB, string source)
        {
            CRUD.SendFollowRequest( Session["username"].ToString(), usernameB);
            if (source == "../AllUsers/NonSessionUserProfile")
                return View(source, CRUD.getUser(usernameB));
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult CancelFollowRequest(string usernameB, string source)
        {
            CRUD.CancelFollowRequest(Session["username"].ToString(), usernameB);
            if (source == "../AllUsers/NonSessionUserProfile")
                return View(source, CRUD.getUser(usernameB));
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult AcceptFollowRequest(string sender, string source)
        {
            CRUD.AcceptFollowRequest(sender, Session["username"].ToString());
            if (source == "FollowRequests")
            {
                List<User> u = CRUD.DisplayFollowRequestsOfAUser(Session["username"].ToString());
                return View(source, u);
            }
               
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult DeleteFollowRequest(string sender, string source)
        {
            CRUD.CancelFollowRequest(sender, Session["username"].ToString());
            if (source == "FollowRequests")
            {
                List<User> u = CRUD.DisplayFollowRequestsOfAUser(Session["username"].ToString());
                return View(source, u);
            }
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult LogoutUser()
        {
            Session["username"] = null;
            return RedirectToAction("../Login/Index");
        }

        public ActionResult DeleteFollower(string followee, string source)
        {
            CRUD.DeleteFollower(followee, Session["username"].ToString());
            if (source == "../AllUsers/NonSessionUserProfile")
            {
                return View(source, CRUD.getUser(followee));
            }
            List<User> users = CRUD.AllUsers();
            return View(source, users);
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

        public ActionResult AddComment(string commentText, string contentID)
        {
            var cID = ""; int i = 0;
            while(contentID[i]>=48 && contentID[i]<=57)
            {
                cID += contentID[i];
                i++;
            }
            CRUD.AddComment(Convert.ToInt32(cID), Session["username"].ToString(), commentText);
            ViewData["ContentID"] = cID;
            return PartialView("_Comment", CRUD.GetCommentsOfAPost(Convert.ToInt32(cID)));
        }
    }
}
