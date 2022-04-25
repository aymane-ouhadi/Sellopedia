using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
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

        public ActionResult Index(string search, int? categoryId, int? minPrice, int? maxPrice)
        {
            // ! should show a message of no result found in the view

            if(minPrice == null)
            {
                minPrice = 50;
            }
            if(maxPrice == null)
            {
                maxPrice = 5000;
            }
            ViewBag.minPrice = minPrice;
            ViewBag.maxPrice = maxPrice;

            var products = db.Products
                .Where(p => p.OriginalPrice >= minPrice && p.OriginalPrice <= maxPrice);

            if(categoryId != null)
            {
                var productsSearch = products.Where(p => p.CategoryId == categoryId).ToList();
                return View(productsSearch);
            }

            if (search != null)
            {
                var productsSearch = products.Where(p => p.Name.Contains(search)).ToList();
                return View(productsSearch);
            }

            //products = db.Products.ToList();
            //if (User.Identity.GetUserId() != null)
            //{
            //    return View(products.Where(p => p.UserId != User.Identity.GetUserId()).ToList());
            //}
            return View(products.ToList());
        }

        public ActionResult Product(int? id)
        {
            if (id == null || db.Products.Find(id) == null)
            {
                return RedirectToAction("Index");
            }

            Product product = db.Products.Find(id);

            ViewBag.Categories = db.Categories.ToList();
            ViewBag.User = db.Users.Find(User.Identity.GetUserId());
            ViewBag.ProductImage = db.ProductImages.ToList();

            // get the mean score of all the reviews of this product
            int reviewsCount = product.Reviews.ToList().Count,
                reviewsScoreTotal = product.Reviews.Select(r => r.Score).Sum();

            int ordersCount = db.Orders.Where(p => p.ProductId == id).Count();

            ViewBag.ProductRating = (float)reviewsScoreTotal / reviewsCount;
            ViewBag.ProductOrders = ordersCount;

            return View(product);
        }

        [Authorize]
        public ActionResult Cart()
        {
            return View();
        }

        [Authorize]
        public ActionResult Checkout()
        {
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());

            return View(user);
        }

        [HttpPost]
        public ActionResult ConfirmOrder(string Id, string Address, IList<Order> Orders, decimal TotalPrice)
        {
            //creating the cart and saving it in the database
            Cart cart = new Cart()
            {
                TotalPrice = TotalPrice,
                OrderDate = DateTime.Now,
                UserId = Id,
                Address = Address
            };
            db.Carts.Add(cart);
            db.SaveChanges();

            
            foreach(Order order in Orders)
            {
                //adding the foreign key to each order and saving it in the database
                order.CartId = cart.Id;
                db.Orders.Add(order);
                db.SaveChanges();

                //modifying the quantity of the products
                Product product = db.Products.Find(order.ProductId);
                product.Quantity -= order.Quantity;
                db.SaveChanges();
            }


            return Json(new { 
                result = "Redirect",
                url = Url.Action("Index", "Shop")
            });
        }

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

            return RedirectToAction("Product", new { id = model.ProductId });
        }





        //------------ Functional Methods ------------//
        public JsonResult AddToCart(int product_id)
        {
            db.Configuration.ProxyCreationEnabled = false;

            Product product = db.Products.Find(product_id);
            string product_main_image = db.ProductImages.Where(image => image.ProductId == product_id).FirstOrDefault().Image;

            //Order order = new Order()
            //{
            //    UserId = User.Identity.GetUserId(),
            //    ProductId = product.Id,
            //    OrderPrice = (decimal) (product.DiscountPrice != null ? product.DiscountPrice : product.OriginalPrice),
            //    Quantity = 1
            //};

            var order = new
            {
                UserId = User.Identity.GetUserId(),
                OrderPrice = (decimal)(product.DiscountPrice != null ? product.DiscountPrice : product.OriginalPrice),
                CurrentProduct = new
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    ProductImage = product_main_image,
                    ProductPrice = (decimal)(product.DiscountPrice != null ? product.DiscountPrice : product.OriginalPrice),
                },
                Quantity = 1
            };

            return Json(order, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SearchProduct(string search_text)
        {
            var result = db.Products.Where(p => p.Name.Contains(search_text))
                .Select(p => p.Name)
                .ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}