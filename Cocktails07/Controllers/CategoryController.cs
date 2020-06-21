using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cocktails07.Models;
using System.Drawing;
using System.Drawing.Imaging;

namespace Cocktails07.Controllers
{
    public class CategoryController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        private IngredientSumary IngSum = new IngredientSumary();
        //
        // GET: /Category/
        [Authorize(Roles = "Administrator")]
        public ViewResult Index()
        {
            return View(db.Categories.ToList());
        }
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Categories.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "Category";
            ViewBag.myControl = "Category";
            if (User.IsInRole("Payed"))
                return View(myComponents);
            else
                return View("IndexComponentTextOnly", myComponents);
        }

        //
        // GET: /Category/Details/5

        public ViewResult Details(long id)
        {
            Category Category = db.Categories.Find(id);
            IngSum.Id = (int)id;
            IngSum.Name = Category.Name;
            IngSum.Cocktails = from g in Category.CockTails select new SelectListItem { Value = g.CocktailId.ToString(), Text = g.Name };
            return View(IngSum);
        }

        //
        // GET: /Category/Create

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Category/Create

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Create(Category Category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(Category);
                db.SaveChanges();
                ImageTrans(Category.Name);
                return RedirectToAction("Index");
            }

            return View(Category);
        }

        //
        // GET: /Category/Edit/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(long id)
        {
            Category Category = db.Categories.Find(id);
            return View(Category);
        }

        //
        // POST: /Category/Edit/5

        [Authorize(Roles = "Administrator")]
        [HttpPost]
        public ActionResult Edit(Category Category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(Category).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(Category.Name);
                return RedirectToAction("Index");
            }
            return View(Category);
        }

        //
        // GET: /Category/Delete/5

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(long id)
        {
            Category Category = db.Categories.Find(id);
            return View(Category);
        }

        //
        // POST: /Category/Delete/5

        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Category Category = db.Categories.Find(id);
            db.Categories.Remove(Category);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        private void ImageTrans(String Name)
        {
            if (Request.Files.Count == 1 && Request.Files[0].ContentLength < 262164)
            {
                var biggerpath = Server.MapPath(Url.MyPictureContent(Name, "bigger"));
                Image.FromStream(Request.Files[0].InputStream).ResizeTo(73, 73).Save(biggerpath, ImageFormat.Png);
            }
        }

    }
}
