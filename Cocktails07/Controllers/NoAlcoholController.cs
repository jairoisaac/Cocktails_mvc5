using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Drawing;
using System.Drawing.Imaging;
using Cocktails07.Models;

namespace Cocktails07.Controllers
{ 
    public class NoAlcoholController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();
        //
        // GET: /NoAlcohol/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.NoAlcohols.ToList());
        }
        [Route("Complements")]
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.NoAlcohols.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "No Alcohol";
            ViewBag.myControl = "NoAlcohol";
                return View(myComponents);
        }

        //
        // GET: /NoAlcohol/Details/5

        public ViewResult Details(long id)
        {
            NoAlcohol NoAlcohol = db.NoAlcohols.Find(id);
            IngItsCock.Id = (int)id;
            //IngSum.Id = (int)id;
            //IngSum.Name = NoAlcohol.Name;
            ViewBag.Details = "Cocktails with " + NoAlcohol.Name.Replace("_", " ");
            IngItsCock.Name = NoAlcohol.Name;
            //IngSum.Cocktails = from a in NoAlcohol.CocktailNoAlcohols select new SelectListItem {Value = a.CocktailId.ToString(),Text = a.CockTail.Name };
            IngItsCock.ItsCocktails = from b in NoAlcohol.CocktailNoAlcohols select new component { Id = b.CocktailId, Name = b.CockTail.Name };
            //IngSum.Cocktails = (from a in NoAlcohol.CocktailNoAlcohols select new SelectListItem { Value = a.CocktailId.ToString(), Text = a.CockTail.Name }).OrderBy(o => o.Text);
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            return View(IngItsCock);
        }

        //
        // GET: /NoAlcohol/Create

        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /NoAlcohol/Create

        [HttpPost]
        [Authorize(Roles="Administrator")]
        public ActionResult Create(NoAlcohol noalcohol)
        {
            if (ModelState.IsValid)
            {
                db.NoAlcohols.Add(noalcohol);
                db.SaveChanges();
                ImageTrans(noalcohol.Name);
                return RedirectToAction("Index");  
            }

            return View(noalcohol);
        }
        
        //
        // GET: /NoAlcohol/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            NoAlcohol noalcohol = db.NoAlcohols.Find(id);
            return View(noalcohol);
        }

        //
        // POST: /NoAlcohol/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(NoAlcohol noalcohol)
        {
            if (ModelState.IsValid)
            {
                db.Entry(noalcohol).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(noalcohol.Name);
                return RedirectToAction("Index");
            }
            return View(noalcohol);
        }

        //
        // GET: /NoAlcohol/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            NoAlcohol noalcohol = db.NoAlcohols.Find(id);
            return View(noalcohol);
        }

        //
        // POST: /NoAlcohol/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            NoAlcohol noalcohol = db.NoAlcohols.Find(id);
            db.NoAlcohols.Remove(noalcohol);
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