using Sellopedia.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
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
    }
}