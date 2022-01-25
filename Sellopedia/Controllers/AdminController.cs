using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    //--------------------------------------------- CRUD Categories
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Categories()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateCategory(Category model)
        {
            return View();
        }

        //--------------------------------------------- CRUD users
        public ActionResult Users()
        {
            return View(db.Users.Where(m => m.IsValid == true).ToList());
        }

        public ActionResult BannedUsers()
        {
            return View(db.Users.Where(m => m.IsValid == false).ToList());
        }

        public ActionResult EditValidity(string id)
        {
            var user = db.Users.Find(id);
            user.IsValid = !user.IsValid;
            user.IsWhiteListed = false;
            db.SaveChanges();

            if (user.IsValid) {
                return RedirectToAction("BannedUsers");
            }
            return RedirectToAction("Users");
        }

        public ActionResult EditWhiteListing(string id)
        {
            var user = db.Users.Find(id);
            user.IsWhiteListed = !user.IsWhiteListed;
            db.SaveChanges();

            return RedirectToAction("Users");
        }
    }
}