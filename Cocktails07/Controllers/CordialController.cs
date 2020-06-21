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
    public class CordialController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();

        //
        // GET: /Cordial/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.Cordials.ToList());
        }
     
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Cordials.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "Cordial";
            ViewBag.myControl = "Cordial";
            return View(myComponents);
        }

        //
        // GET: /Cordial/Details/5

        public ViewResult Details(long id)
        {
            Cordial Cordial = db.Cordials.Find(id);
            IngItsCock.Id = (int)id;
            //IngSum.Id = (int)id;
            //IngSum.Name = Cordial.Name;
            IngItsCock.Name = Cordial.Name;
            ViewBag.Details = "Cocktails with " + Cordial.Name.Replace("_", " ");
            //IngSum.Cocktails = from a in Cordial.CocktailCordials select new SelectListItem {Value = a.CocktailId.ToString(),Text = a.CockTail.Name };
            IngItsCock.ItsCocktails = from b in Cordial.CocktailCordials select new component { Id = b.CocktailId, Name = b.CockTail.Name };
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            //IngSum.Cocktails = (from a in Cordial.CocktailCordials select new SelectListItem { Value = a.CocktailId.ToString(), Text = a.CockTail.Name }).OrderBy(o => o.Text);
            return View(IngItsCock);
        }

        //
        // GET: /Cordial/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Cordial/Create
        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(Cordial cordial)
        {
            if (ModelState.IsValid)
            {
                db.Cordials.Add(cordial);
                db.SaveChanges();
                ImageTrans(cordial.Name);
                return RedirectToAction("Index");  
            }

            return View(cordial);
        }
        
        //
        // GET: /Cordial/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            Cordial cordial = db.Cordials.Find(id);
            return View(cordial);
        }

        //
        // POST: /Cordial/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(Cordial cordial)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cordial).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(cordial.Name);
                return RedirectToAction("Index");
            }
            return View(cordial);
        }

        //
        // GET: /Cordial/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            Cordial cordial = db.Cordials.Find(id);
            return View(cordial);
        }

        //
        // POST: /Cordial/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Cordial cordial = db.Cordials.Find(id);
            db.Cordials.Remove(cordial);
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
                Image.FromStream(Request.Files[0].InputStream).ResizeTo(109, 109).Save(biggerpath, ImageFormat.Png);
            }
        }
    }
}