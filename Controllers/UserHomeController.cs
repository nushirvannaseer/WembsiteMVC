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
using System.IO;
using System.Windows.Forms;

namespace Wembsite.Controllers
{
    public class UserHomeController : Controller
    {
        private UserDBContext db = new UserDBContext();
        public ActionResult Profile()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            User user = CRUD.getUser(Session["username"].ToString());
            return View(user);
        }

        public ActionResult Followers()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<User> users = CRUD.DisplayFollowersOfAUser(Session["username"].ToString());
            return View("Followers", users);
        }

        public ActionResult Following()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<User> users = CRUD.DisplayFollowingsOfAUser(Session["username"].ToString());
            return View("Following", users);
        }

        public ActionResult FollowRequests()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<User> users = CRUD.DisplayFollowRequestsOfAUser(Session["username"].ToString());
            return View("FollowRequests", users);
        }

        public ActionResult SendFollowRequest(string usernameB, string source)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            CRUD.SendFollowRequest(Session["username"].ToString(), usernameB);
            if (source == "../AllUsers/NonSessionUserProfile")
                return View(source, CRUD.getUser(usernameB));
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult CancelFollowRequest(string usernameB, string source)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            CRUD.CancelFollowRequest(Session["username"].ToString(), usernameB);
            if (source == "../AllUsers/NonSessionUserProfile")
                return View(source, CRUD.getUser(usernameB));
            List<User> users = CRUD.AllUsers();
            return View(source, users);
        }

        public ActionResult AcceptFollowRequest(string sender, string source)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
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
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
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
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
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

        public ActionResult PublishPost(string postContent, string privacy, HttpPostedFileBase file)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            if (file != null && file.ContentLength > 0)
            {
                string fileName = Session["username"].ToString() + "__" + file.FileName;
                string path = Path.Combine(Server.MapPath("~/UserUploads"),
                                           Path.GetFileName(fileName));
                file.SaveAs(path);
                path = "../UserUploads/" + fileName;
                path=path.Replace('\\', '/');
                //path=path.Prepend("")
                string[] extension = file.FileName.Split('.');
                int last = extension.Count();
                //ViewBag.Message = "File uploaded successfully";
                CRUD.NewPost(Session["username"].ToString(), postContent, privacy, path, extension[last - 1]);
                return RedirectToAction("Profile");
            }
            if(postContent=="")
            {
                ViewData["Message"] = "Only Text post cannot be empty";
                return View("CreateNewPost");
            }

            CRUD.NewPost(Session["username"].ToString(), postContent, privacy);
            return RedirectToAction("Profile");
        }

        public ActionResult AllPosts()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<UserContent> postList = CRUD.AllPostsOfAUser(Session["username"].ToString());
            return View(postList);
        }

        public ActionResult ViewPost(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            UserContent post = CRUD.GetUserPost(id);
            return View(post);
        }

        public ActionResult EditPost(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            UserContent post = CRUD.GetUserPost(id);
            return View(post);
        }

        public ActionResult SaveEditChanges(int id, string privacy, string RawData)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            CRUD.EditPost(id, privacy, RawData);
            return RedirectToAction("AllPosts");
        }

        public ActionResult DeletePost(int id)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            CRUD.DeletePost(id);
            return RedirectToAction("AllPosts");
        }

        public ActionResult HomePage()
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<UserContent> postList = CRUD.HomepagePost(Session["username"].ToString());

            return View(postList);
        }

        public ActionResult LikePost(int contentID, string likedBy)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            int originalLikes = Convert.ToInt32(CRUD.GetUserPost(contentID).likes);
            CRUD.LikePost(contentID, likedBy);
            ViewData["Message"] = "Unlike";
            //if like was already present
            if (originalLikes == Convert.ToInt32(CRUD.GetUserPost(contentID).likes))
            {
                CRUD.UnLikePost(contentID, likedBy);
                ViewData["Message"] = "Like";
            }
            UserContent post = CRUD.GetUserPost(contentID);
            return PartialView("_LikesPartialView", post);
        }

        public ActionResult GetLikeList(int contentID)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<User> likers = CRUD.GetLikeList(contentID);
            return PartialView("_LikeListPartialView", likers);
        }

        public ActionResult _NonSessionUserPosts(string username)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            List<UserContent> posts = CRUD.getNonSessionUserPosts(username, Session["username"].ToString());
            return View(posts);
        }

        public ActionResult Search(string searchText)
        {
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
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
            if (Session["username"] == null)
                return RedirectToAction("SessionExpired");
            
            var cID = ""; int i = 0;
            while (contentID[i] >= 48 && contentID[i] <= 57)
            {
                cID += contentID[i];
                i++;
            }
            ViewData["ContentID"] = cID;
            if (commentText == "" || commentText==null)
                return PartialView("_Comment", CRUD.GetCommentsOfAPost(Convert.ToInt32(cID)));

            CRUD.AddComment(Convert.ToInt32(cID), Session["username"].ToString(), commentText);
            return PartialView("_Comment", CRUD.GetCommentsOfAPost(Convert.ToInt32(cID)));
        }

        public ActionResult SessionExpired()
        {
            return View();
        }
    }
}
