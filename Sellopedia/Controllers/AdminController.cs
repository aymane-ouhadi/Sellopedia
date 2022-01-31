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

    // overwrite the default OnAuthorization 'loginPage' attr
    // to redirect to the admin login page instead of normal users login page
    public class CustomAuthorization : AuthorizeAttribute
    {
        public string LoginPage { get; set; } = "/Admin/AdminLogin";

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (!filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.HttpContext.Response.Redirect(LoginPage);
            }
            base.OnAuthorization(filterContext);
        }
    }


    
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        [AllowAnonymous]
        public ActionResult AdminLogin()
        {
            return View();
        }

        //[AllowAnonymous]
        //[HttpPost]
        //public ActionResult AdminLogin(LoginViewModel model)
        //{
        //    if(!ModelState.IsValid)
        //    {
        //        return Json(new {x = "not working", y = ModelState.Values.ElementAt(0).Errors.ElementAt(0).ErrorMessage }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        return Json(new { x = "working", y = ModelState.Values.ElementAt(0).Value.RawValue, c = ModelState.Values.ElementAt(1).Value.RawValue }, JsonRequestBehavior.AllowGet);
        //    }
        //    return View();
        //}

        // GET: Admin
        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult Index()
        {
            return View();
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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
            var admins = db.Roles.Where(r => r.Name == "Admin" && r.Users.Count > 0).Count();
            

            // products
            var products_count = products.Count();
            var products_on_sale = products.Where(p => p.DiscountPrice != null).Count();
            var products_ordered = db.Orders.Count();


            // category / product
            var categories_name = categories.Select(c => c.Name).ToArray();
            var products_category = db.Categories.Select(c => new { c.Name, c.Products.Count }).ToList();

            // -- result json
            var result = new {
                admins = admins,
                users = new {
                    //Total = users_count,
                    Particular = users_particular,
                    Organisation = users_organisation,
                },
                products = new {
                    Products = products_count,
                    OnSale = products_on_sale,
                    Orders = products_ordered,
                },
                products_category,
            };



            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        [HttpGet]
        public ActionResult Categories()
        {
            return View(db.Categories.ToList());
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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
        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult Users()
        {
            return View(db.Users.Where(m => m.IsValid == true).ToList());
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult BannedUsers()
        {
            return View(db.Users.Where(m => m.IsValid == false).ToList());
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
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

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult EditWhiteListing(string id)
        {
            var user = db.Users.Find(id);
            user.IsWhiteListed = !user.IsWhiteListed;
            db.SaveChanges();

            return RedirectToAction("Users");
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult TransactionsHistory()
        {
            List<Cart> carts = db.Carts.ToList();
            foreach (Cart cart in carts)
            {
                cart.Orders = db.Orders.Where(p => p.CartId == cart.Id).ToList();
            }
            return View(carts);
        }

        [CustomAuthorization(Roles = "Admin, SuperAdmin")]
        public ActionResult TransactionDetails(int Id)
        {
            List<Order> orders = db.Orders.Where(p => p.CartId == Id).ToList();
            return View(orders);
        }


        // Add new Admin Accounts --------------- //
        [CustomAuthorization(Roles = "Admin")]
        public ActionResult CreteAdmin()
        {
            return View();
        }

        [CustomAuthorization(Roles = "Admin")]
        [HttpPost]
        public ActionResult CreteAdmin(string model)
        {
            if (!User.IsInRole("SuperAdmin"))
            {
                TempData["privelege_error"] = "Only SuperAdmin can add a new Admin Account.";
                return View();
            }

            return View();
        }
    }
}