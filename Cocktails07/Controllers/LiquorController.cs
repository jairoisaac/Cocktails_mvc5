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
    public class LiquorController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();
        //
        // GET: /Liquor/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.Liquors.ToList());
        }
        
        [Route("Liquors")]
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Liquors.ToList();
            foreach (var c in mComponent)
            {
                component myComponent = new component();
                myComponent.Id = c.Id;
                myComponent.Name = c.Name;
                myComponents.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "Liquor";
            ViewBag.myControl = "Liquor";
            return View(myComponents);
        }

        //
        // GET: /Liquor/Details/5
        public ViewResult Details(long id)
        {
            
            Liquor liquor = db.Liquors.Find(id);
            IngItsCock.Id = (int)id;
            //IngSum.Id = (int)id;
            //IngSum.Name = liquor.Name;
            ViewBag.Details = "Cocktails with " + liquor.Name.Replace("_", " ");
            IngItsCock.Name = liquor.Name;
            //IngSum.Cocktails = from a in liquor.CocktailLiquors select new SelectListItem {Value = a.CocktailId.ToString(),Text = a.CockTail.Name };
            IngItsCock.ItsCocktails = from b in liquor.CocktailLiquors select new component { Id = b.CocktailId, Name = b.CockTail.Name };
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            //IngSum.Cocktails = (from a in liquor.CocktailLiquors select new SelectListItem { Value = a.CocktailId.ToString(), Text = a.CockTail.Name }).OrderBy(o => o.Text);
            return View(IngItsCock);
        }

        //
        // GET: /Liquor/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Liquor/Create
        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(Liquor liquor)
        {
            if (ModelState.IsValid)
            {
                db.Liquors.Add(liquor);
                db.SaveChanges();
                ImageTrans(liquor.Name);      
                return RedirectToAction("Index");  
            }

            return View(liquor);
        }
        
        //
        // GET: /Liquor/Edit/5
        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            Liquor liquor = db.Liquors.Find(id);
            return View(liquor);
        }

        //
        // POST: /Liquor/Edit/5
        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(Liquor liquor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(liquor).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(liquor.Name);
                return RedirectToAction("Index");
            }
            return View(liquor);
        }

        //
        // GET: /Liquor/Delete/5
        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            Liquor liquor = db.Liquors.Find(id);
            return View(liquor);
        }

        //
        // POST: /Liquor/Delete/5
        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Liquor liquor = db.Liquors.Find(id);
            db.Liquors.Remove(liquor);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public void test(long id)
        {
             Liquor liquor = db.Liquors.Find(id);
             //var Mgarnishes = (from d in db.Garnishes from e in d.CockTails where e.CocktailId == id select new { d.Name, d.Id, id }).ToList();
             var myCoc = from a in liquor.CocktailLiquors select new { a.Amount, a.CockTail.Name };

             //var myCoc = liquor.CocktailLiquors.Select(l => l.CockTail.Name).ToList();
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