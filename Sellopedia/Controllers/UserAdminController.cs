using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
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
            ViewBag.User = user;
            ViewBag.Categories = db.Categories.ToList();
            ViewBag.productCount = user.Products.Count;
            return View(user.Products.ToList());
        }

        [Authorize]
        public ActionResult UserCreateProduct(Product model/*, HttpPostedFileBase images*/)
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.User = user;
            //ViewBag.Images = model.ProductImages;

            //string ImagePath = null;
            //string FileName = null;
            //if (images != null)
            //{
            //    //Setting up the file name ([Date]_[filename].[extension])
            //    FileName = Path.GetFileNameWithoutExtension(images.FileName);
            //    string FileExtension = Path.GetExtension(images.FileName);
            //    FileName = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName.Trim() + FileExtension;
            //    string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ProfileImagesPath"]);

            //    ImagePath = UploadPath + FileName;

            //    images.SaveAs(ImagePath);

            //    Session["images"] = images.FileName;
            //    return View("Index", "Home");
            //    return RedirectToAction("Index", "Home");
            //}
            //else
            //{
            //    Session["images"] = "Failed :\"(";
            //    return View();
            //}

            if (ModelState.IsValid)
            {
                Product product = model;
                product.UserId = user.Id;
                db.Products.Add(product);
                db.SaveChanges();
                return View();
            }

            return View(model);
        }


        public ActionResult ImageUpload(Upload prodImg)
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

            ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
            ViewBag.User = user;

            if(ModelState.IsValid)
            {
                ViewBag.msg = "Success";
            }
            else
            {
                ViewBag.msg = "Failure";
            }

            return View(new Upload { Product = new Product(), Images = new List<ProductImage>() });
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