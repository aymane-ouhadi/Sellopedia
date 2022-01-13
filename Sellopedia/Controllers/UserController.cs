﻿using Microsoft.AspNet.Identity;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        //--------- User Profile -------------//
        // GET: User/EditProfile
        public ActionResult EditProfile()
        {
            string currentUserId = User.Identity.GetUserId();
            user = db.Users.Find(currentUserId);

            // chekc for user if particular or organisation
            //ViewBag.userIsParticular = (user.UserName == "" || user.UserName == user.Email) ? true : false;

            return View(user);
        }

        // POST: User/EditProfile
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



        //--------- User Products -------------//
        // GET: User/MyProducts
        public ActionResult MyProducts()
        {
            string userId = User.Identity.GetUserId();
            Product[] products = db.Products.Where(p => p.UserId == userId).OrderByDescending(p => p.Id).ToArray();

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.User = db.Users.Find(userId);
            ViewBag.ProductImage = db.ProductImages.ToList();

            return View(products);
        }

        // GET: User/EditProduct/id
        public ActionResult EditProduct(int? id)
        {
            if(id == null || db.Products.Find(id) == null)
            {
                return RedirectToAction("MyProducts");
            }

            Product product = db.Products.Find(id);

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.User = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ProductImage = db.ProductImages.ToList();

            return View(product);
        }

        // Post: User/EditProduct/id
        [HttpPost]
        public ActionResult EditProduct(Product model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            // product images shouldnt be update/edited
            Product product = model;
            db.Products.AddOrUpdate(product);
            db.SaveChanges();

            return RedirectToAction("MyProducts");
        }

        // GET: User/ProductDetails/id
        public ActionResult ProductDetails(int? id)
        {
            if (id == null || db.Products.Find(id) == null)
            {
                return RedirectToAction("MyProducts");
            }

            Product product = db.Products.Find(id);

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.User = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ProductImage = db.ProductImages.ToList();

            return View(product);
        }

        // GET: User/CreateProduct
        public ActionResult CreateProduct()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                                Image = Path.Combine("/", ConfigurationManager.AppSettings["ProductImagesPath"], FileName)
                            };

                            db.ProductImages.Add(productImage);
                            db.SaveChanges();

                        }
                    }
                }
            }
            return RedirectToAction("MyProducts");
        }


        //--------- Products -------------//
        // GET: Products/Details/5
        //public ActionResult ProductDetails(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}
    }
}