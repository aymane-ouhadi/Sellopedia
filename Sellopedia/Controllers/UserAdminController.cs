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

        // ------------ Mohamed
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
            return View(db.Orders.ToList());
        }

        //--------------------------------//
        [Authorize]
        public ActionResult UserOwnerProducts()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(db.Products.ToList());
        }

        [Authorize]
        public ActionResult UserCreateProduct(Product model)
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            Category c1 = db.Categories.ToArray()[0];
            
            if(ModelState.IsValid)
            {
                Product product = model;
                product.UserId = user.Id;
                product.Category = c1;
                db.Products.Add(product);
                db.SaveChanges();
            }

            return View();
        }







        // ------------ Aymane
        public ActionResult LeaveReview(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            Review review = new Review()
            {
                UserId = user.Id,
                ProductId = id,
                Score = 4,
                Message = "Great product dude"
            };

            db.Reviews.Add(review);
            db.SaveChanges();

            return View(db.Reviews.ToList());
        }
    }
}