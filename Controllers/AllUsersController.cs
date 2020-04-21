using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Wembsite.Models;

namespace Wembsite.Controllers
{
    public class AllUsersController : Controller
    {
        // GET: AllUsers
        public ActionResult Index()
        {
            List<User> users = CRUD.AllUsers();
            return View(users);
        }

        public ActionResult Edit(string username)
        {
            User user = CRUD.getUser(username);
            return View(user);
        }

        public ActionResult Delete(string username)
        {
            CRUD.deleteUser(username);
            List<User> users = CRUD.AllUsers();
            return View("Index", users);
        }
    }
}