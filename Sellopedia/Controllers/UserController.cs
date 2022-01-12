using Microsoft.AspNet.Identity;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
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
    }
}