using Microsoft.AspNet.Identity;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        public ActionResult EditProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.Find(currentUserId);

            // chekc for user if particular or organisation
            //ViewBag.userIsParticular = (user.UserName == "" || user.UserName == user.Email) ? true : false;

            return View(user);
        }

        [Authorize]
        [HttpPost]
        public ActionResult EditProfile(ApplicationUser model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            // update user in db
            ApplicationUser user = model;
            db.Users.AddOrUpdate(user);
            db.SaveChanges();

            return RedirectToAction("EditProfile");
        }
    }
}