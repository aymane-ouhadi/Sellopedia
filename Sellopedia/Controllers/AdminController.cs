using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static Sellopedia.Models.EnumerationsClass;

namespace Sellopedia.Controllers
{
    //--------------------------------------------- CRUD Categories
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        public ActionResult AdminLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AdminLogin(LoginViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return Json(new {x = "not working", y = ModelState.Values.ElementAt(0).Errors.ElementAt(0).ErrorMessage }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { x = "working", y = ModelState.Values.ElementAt(0).Value.RawValue, c = ModelState.Values.ElementAt(1).Value.RawValue }, JsonRequestBehavior.AllowGet);
            }
            return View();
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Stats()
        {
            // -- all important variables from the database 
            var users = db.Users.ToList();
            var products = db.Products.ToList();
            var orders = db.Orders.ToList();
            var carts = db.Carts.ToList();
            var categories = db.Categories.ToList();

            //-- Users
            var users_count = users.Count();
            var users_particular = users.Where(u => u.AccountType == AccountType.Particular).Count();
            var users_organisation = users.Where(u => u.AccountType == AccountType.Organisation).Count();

            // products
            var products_count = products.Count();
            var products_on_sale = products.Where(p => p.DiscountPrice != null).Count();
            var products_ordered = products.Where(p => p.Orders.Count() > 0).Count();

            // category / product
            var categories_name = categories.Select(c => c.Name).ToArray();
            var products_category = db.Categories.Select(c => new { c.Name, c.Products.Count }).ToList();

            // -- result json
            var result = new {
                users_count,
                users_particular,
                users_organisation,
                products_count,
                products_on_sale,
                products_ordered,
                products_category,
            };



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Categories()
        {
            return View(db.Categories.ToList());
        }

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


        public ActionResult EditCategory(int? id, string categoryName, string iconName)
        {
            Category model = db.Categories.Find(id);

            if (id == null || model == null)
            {
                TempData["edit_error"] = "The category you are trying to edit does not exist";
                return RedirectToAction("Categories");
            }

            model.Name = categoryName;
            model.Icon = iconName;

            db.Categories.AddOrUpdate(model);
            db.SaveChanges();

            TempData["edit_success"] = $"Category has been edited successfully.";
            return RedirectToAction("Categories");
        }


        public ActionResult DeleteCategory(int? id)
        {
            Category model = db.Categories.Find(id);

            if(id == null || model == null)
            {
                TempData["delete_error"] = "Unable to delet category. Selected item does not exist.";
                return RedirectToAction("Categories");
            }

            db.Categories.Remove(model);
            db.SaveChanges();

            TempData["delete_success"] = $"Deleted category <strong>\"{model.Name}\"</strong> successfully.";
            return RedirectToAction("Categories");
        }

        //--------------------------------------------- CRUD users
        public ActionResult Users()
        {
            return View(db.Users.Where(m => m.IsValid == true).ToList());
        }

        public JsonResult SearchUser(string search_text)
        {
            // firstname
            var result = db.Users
                .Where(u => u.FirstName.Contains(search_text))
                .Select(u => u.FirstName)
                .ToList();
            // lastname
            var result2 = db.Users
                .Where(u => u.LastName.Contains(search_text))
                .Select(u => u.LastName)
                .ToList();
            // username
            var result3 = db.Users
                .Where(u => u.UserName.Contains(search_text) && u.Email != u.UserName)
                .Select(u => u.UserName)
                .ToList();

            // append results together and send them as json
            result.AddRange(result2);
            result.AddRange(result3);

            return Json(result, JsonRequestBehavior.AllowGet);
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