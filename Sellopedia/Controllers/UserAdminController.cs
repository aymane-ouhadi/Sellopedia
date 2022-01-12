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


        //public ActionResult ImageUpload(Upload prodImg)
        //{
        //    string currentUserId = User.Identity.GetUserId();
        //    user = db.Users.FirstOrDefault(x => x.Id == currentUserId);

        //    ViewBag.Categories = new SelectList(db.Categories, "Id", "Name");
        //    ViewBag.User = user;

        //    if(ModelState.IsValid)
        //    {
        //        ViewBag.msg = "Success";
        //    }
        //    else
        //    {
        //        ViewBag.msg = "Failure";
        //    }

        //    return View(new Upload { Product = new Product(), Images = new List<ProductImage>() });
        //}




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

        [Authorize]
        public ActionResult CreateProduct()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateProduct(ProductViewModel model)
        {

            ApplicationDbContext db = new ApplicationDbContext();
            Product product = null;

            if (ModelState.IsValid)
            {
                product = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    OriginalPrice = model.OriginalPrice,
                    DiscountPrice = model.DiscountPrice,
                    Quantity = model.Quantity,
                    UserId = User.Identity.GetUserId(),
                    CategoryId = model.CategoryId
                };

                db.Products.Add(product);
                db.SaveChanges();


                foreach (HttpPostedFileBase file in model.ProductImages)
                {
                    if (file != null)
                    {
                        string ImagePath = null;
                        string FileName = null;
                        if (file != null)
                        {
                            //Setting up the file name ([Date]_[filename].[extension])
                            FileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string FileExtension = Path.GetExtension(file.FileName);
                            FileName = DateTime.Now.ToString("yyyyMMdd") + "_" + FileName.Trim() + FileExtension;

                            //Defining the upload path, this will return : [(ProjectPath)/(ProfileImagesPath)]
                            string UploadPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ConfigurationManager.AppSettings["ProductImagesPath"]);
                            //ConfigurationManager.AppSettings["ProfileImagesPath"] : the path to the profile images (You'll find it in Web.config : <add key="ProfileImagesPath" value="Storage/ProfileImages"/>)
                            ImagePath = UploadPath + FileName;

                            file.SaveAs(ImagePath);

                            //Adding the product image to the database
                            ProductImage productImage = new ProductImage
                            {
                                ProductId = product.Id,
                                Image = ImagePath
                            };

                            db.ProductImages.Add(productImage);
                            db.SaveChanges();

                        }
                    }
                }
            }
            return RedirectToAction("UserOwnerProducts");
        }


        //-------------- Khalid 

        [Authorize]
        public ActionResult EditProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.FirstOrDefault(x => x.Id == currentUserId);
            return View(user);
        }
    }
}