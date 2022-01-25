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
            return View(db.Categories.ToList());
        }

        //[HttpGet]
        //public ActionResult CreateCategory()
        //{
        //    return View();
        //}

        [HttpPost]
        public ActionResult CreateCategory(string categoryName, string iconName)
        {
            Category model = new Category
            {
                Name = categoryName,
                Icon = iconName
            };

            db.Categories.Add(model);
            db.SaveChanges();

            //return Json(model, JsonRequestBehavior.AllowGet);
            return RedirectToAction("Categories");
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

        public ActionResult TransactionsHistory()
        {
            List<Cart> carts = db.Carts.ToList();
            foreach (Cart cart in carts)
            {
                cart.Orders = db.Orders.Where(p => p.CartId == cart.Id).ToList();
            }
            return View(carts);
        }

        public ActionResult TransactionDetails(int Id)
        {
            List<Order> orders = db.Orders.Where(p => p.CartId == Id).ToList();
            return View(orders);
        }
    }
}