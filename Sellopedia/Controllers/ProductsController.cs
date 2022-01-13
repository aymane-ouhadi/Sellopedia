using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Sellopedia.Models;

namespace Sellopedia.Controllers
{
    public class ProductsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Products
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.User);
            return View(products.ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel model)
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


                foreach (HttpPostedFileBase file in model.ProductImages)
                {
                    if(file != null)
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
                            //ConfigurationManager.AppSettings["ProfileImagesPath"] : the path to the profile images (You'll find it in Web.config : <add key="ProfileImagesPath" value="Storage/ProfileImages"/>)
                            ImagePath = ConfigurationManager.AppSettings["ProductImagesPath"] + FileName;

                            file.SaveAs(ImagePath);

                            //Adding the product image to the database
                            ProductImage productImage = new ProductImage
                            {
                                ProductId = product.Id,
                                Image = ImagePath
                            };

                            db.ProductImages.Add(productImage);

                        }
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", product.UserId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,OriginalPrice,DiscountPrice,Quantity,UserId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.Users, "Id", "FirstName", product.UserId);
            return View(product);
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
