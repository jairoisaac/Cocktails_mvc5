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
    public class GlassController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();
        //
        // GET: /Glass/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.Glasses.ToList());
        }
        [Route("Glasses")]
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Glasses.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "Glass";
            ViewBag.myControl = "Glass";
            return View(myComponents);
        }
        //
        // GET: /Glass/Details/5

        public ViewResult Details(long id)
        {
            Glass glass = db.Glasses.Find(id);
            //IngSum.Id = (int)id;
            IngItsCock.Id = (int)id;
            //IngSum.Name = glass.Name;
            IngItsCock.Name = glass.Name;
            ViewBag.Details = "Cocktails in " + glass.Name.Replace("_", " ");
            //IngSum.Cocktails = from g in glass.CockTails select new SelectListItem { Value = g.CocktailId.ToString(), Text = g.Name };
            IngItsCock.ItsCocktails = from b in glass.CockTails select new component { Id = b.CocktailId, Name = b.Name };
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            return View(IngItsCock);
        }

        //
        // GET: /Glass/Create

        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Glass/Create

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(Glass glass)
        {
            if (ModelState.IsValid)
            {
                db.Glasses.Add(glass);
                db.SaveChanges();
                ImageTrans(glass.Name);
                return RedirectToAction("Index");  
            }

            return View(glass);
        }
        
        //
        // GET: /Glass/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            Glass glass = db.Glasses.Find(id);
            return View(glass);
        }

        //
        // POST: /Glass/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(Glass glass)
        {
            if (ModelState.IsValid)
            {
                db.Entry(glass).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(glass.Name);
                return RedirectToAction("Index");
            }
            return View(glass);
        }

        //
        // GET: /Glass/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            Glass glass = db.Glasses.Find(id);
            return View(glass);
        }

        //
        // POST: /Glass/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Glass glass = db.Glasses.Find(id);
            db.Glasses.Remove(glass);
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