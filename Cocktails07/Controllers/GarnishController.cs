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
    public class GarnishController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();

        //
        // GET: /Garnish/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.Garnishes.ToList());
        }
        [Route("Garnishes")]
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Garnishes.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "Garnish";
            ViewBag.myControl = "Garnish";
            return View(myComponents);
        }

        //
        // GET: /Garnish/Details/5

        public ViewResult Details(long id)
        {
            Garnish Garnish = db.Garnishes.Find(id);
            IngItsCock.Id = (int)id;
            //IngSum.Id = (int)id;
            //IngSum.Name = Garnish.Name;
            IngItsCock.Name = Garnish.Name;
            ViewBag.Details = "Cocktails with " + Garnish.Name.Replace("_", " ");
            //IngSum.Cocktails = from a in Garnish.CocktailGarnishs select new SelectListItem {Value = a.CocktailId.ToString(),Text = a.CockTail.Name };
            IngItsCock.ItsCocktails = from b in Garnish.CockTails select new component { Id = b.CocktailId, Name = b.Name };
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            //IngSum.Cocktails = (from a in Garnish.CocktailGarnishs select new SelectListItem { Value = a.CocktailId.ToString(), Text = a.CockTail.Name }).OrderBy(o => o.Text);
            return View(IngItsCock);
        }

        //
        // GET: /Garnish/Create

        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Garnish/Create

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(Garnish garnish)
        {
            if (ModelState.IsValid)
            {
                db.Garnishes.Add(garnish);
                db.SaveChanges();
                ImageTrans(garnish.Name);
                return RedirectToAction("Index");  
            }

            return View(garnish);
        }
        
        //
        // GET: /Garnish/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            Garnish garnish = db.Garnishes.Find(id);
            return View(garnish);
        }

        //
        // POST: /Garnish/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(Garnish garnish)
        {
            if (ModelState.IsValid)
            {
                db.Entry(garnish).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(garnish.Name);
                return RedirectToAction("Index");
            }
            return View(garnish);
        }

        //
        // GET: /Garnish/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            Garnish garnish = db.Garnishes.Find(id);
            return View(garnish);
        }

        //
        // POST: /Garnish/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Garnish garnish = db.Garnishes.Find(id);
            db.Garnishes.Remove(garnish);
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