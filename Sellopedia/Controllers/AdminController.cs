using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sellopedia.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUser user = null;

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Categories()
        {
            return View(db.Categories.ToList());
        }

        //[HttpGet]
        //public ActionResult CreateCategory()
        //{
        //    return View();
        //}

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
    }
}