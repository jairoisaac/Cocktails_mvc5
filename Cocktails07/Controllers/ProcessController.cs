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
    public class ProcessController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        //private IngredientSumary IngSum = new IngredientSumary();
        private IngredientItsCocktails IngItsCock = new IngredientItsCocktails();

        //
        // GET: /Process/
        [Authorize(Roles="Administrator")]
        public ViewResult Index()
        {
            return View(db.Processes.ToList());
        }
        [Route("Rituals")]
        public ViewResult IndexComponent()
        {
            List<component> myComponents = new List<component>();
            var mComponent = db.Processes.ToList();
            foreach (var c in mComponent)
            {
                if (!(c.Name.Contains("Pour") || c.Name.Contains("Stir")))
                {
                    //I need to exclude this process pour and stir.
                    component myComponent = new component();
                    myComponent.Id = c.Id;
                    myComponent.Name = c.Name;
                    myComponents.Add(myComponent);
                    myComponent = null;
                }
            }
            ViewBag.Title = "Ritual";
            ViewBag.myControl = "Process";
            return View(myComponents);
        }


        //
        // GET: /Process/Details/5

        public ViewResult Details(long id)
        {
            Process process = db.Processes.Find(id);
            ViewBag.Details = "Cocktails " + process.Name.Replace("_", " ");
            //IngSum.Id = (int)id;
            IngItsCock.Id = (int)id;
            IngItsCock.Name = process.Name;
            //IngSum.Name = process.Name;
            //IngSum.Cocktails = from p in process.CockTails select new SelectListItem {Value=p.CocktailId.ToString(), Text=p.Name };

            IngItsCock.ItsCocktails = from b in process.CockTails select new component { Id = b.CocktailId, Name = b.Name };
            IngItsCock.ItsCocktails = IngItsCock.ItsCocktails.OrderBy(b => b.Name);
            //return View(IngSum);
            return View(IngItsCock);
        }

        //
        // GET: /Process/Create

        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Process/Create

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(Process process)
        {
            if (ModelState.IsValid)
            {
                db.Processes.Add(process);
                db.SaveChanges();
                ImageTrans(process.Name);
                return RedirectToAction("Index");  
            }

            return View(process);
        }
        
        //
        // GET: /Process/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            Process process = db.Processes.Find(id);
            return View(process);
        }

        //
        // POST: /Process/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(Process process)
        {
            if (ModelState.IsValid)
            {
                db.Entry(process).State = EntityState.Modified;
                db.SaveChanges();
                ImageTrans(process.Name);
                return RedirectToAction("Index");
            }
            return View(process);
        }

        //
        // GET: /Process/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            Process process = db.Processes.Find(id);
            return View(process);
        }

        //
        // POST: /Process/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            Process process = db.Processes.Find(id);
            db.Processes.Remove(process);
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