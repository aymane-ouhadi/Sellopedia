using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    public class UserAdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        //public UserAdminController()
        //{
        //        string currentUserId = User.Identity.GetUserId();
        //        user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            
        //}

        [Authorize(Roles = "User")]
        public ActionResult UserPage()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(user);
        }
        

        [Authorize(Roles = "Admin")]
        public ActionResult AdminPage()
        {
            //UserManager<ApplicationUser> UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //var user = UserManager.FindById(User.Identity.GetUserId());

            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            ViewBag.product = user.Products.ToList();
            return View(user);
        }

        [Authorize]
        public ActionResult Order()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View();
        }

        [Authorize]
        public ActionResult CreateOrder(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            Order order = new Order()
            {
                UserId = user.Id,
                ProductId = id,
                Quantity = 1,
                OrderPrice = db.Products.Where(p => p.Id == id).FirstOrDefault().OriginalPrice * 1
            };
            db.Orders.Add(order);
            db.SaveChanges();
            return View();
        }
    }
}