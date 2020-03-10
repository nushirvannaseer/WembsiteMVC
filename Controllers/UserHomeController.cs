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
        public  ActionResult Profile(User usr)
        {
            return View(usr);
        }
    }
}
