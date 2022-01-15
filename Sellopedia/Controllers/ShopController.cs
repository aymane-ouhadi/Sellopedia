using Microsoft.AspNet.Identity;
using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    public class ShopController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        public ActionResult Feed()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        public ActionResult Product(int? id)
        {
            if (id == null || db.Products.Find(id) == null)
            {
                return RedirectToAction("Feed");
            }

            Product product = db.Products.Find(id);

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.User = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ProductImage = db.ProductImages.ToList();

            // get the mean score of all the reviews of this product
            int reviewsCount = product.Reviews.ToList().Count,
                reviewsScoreTotal = product.Reviews.Select(r => r.Score).Sum();

            ViewBag.ProductRating = (float)reviewsScoreTotal / reviewsCount;

            return View(product);
        }

        //[HttpGet]
        //public ActionResult CreateReview(int id)
        //{
        //    Review review = new Review();
        //    review.UserId = User.Identity.GetUserId();
        //    review.ProductId = id;
        //    return View(review);
        //}

        [Authorize]
        [HttpPost]
        public ActionResult CreateReview(Review model)
        {
            // check if user already posted a review about his product
            int reviewCount = db.Reviews.Where(r => r.UserId == model.UserId && r.ProductId == model.ProductId).ToList().Count;


            if(!ModelState.IsValid || reviewCount > 0)
            {
                if(reviewCount > 0)
                {
                    ViewBag.reviewError = "You already have a review on this product. Only one allowed !";
                }
                return RedirectToAction("Product", new { id = model.ProductId });
            }

            db.Reviews.Add(model);
            db.SaveChanges();

            return Json(new { userId = model.UserId, productId = model.ProductId, score = model.Score, message = model.Message }, JsonRequestBehavior.AllowGet);
        }
    }
}