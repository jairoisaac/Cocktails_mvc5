using Cocktails07.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cocktails07.Controllers
{
    public class HomeController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        public ActionResult Index()
        {
            ViewBag.Message = "Learn or Remember how to make basic Cocktails";
            //ViewBag.Message0 ="May your tip jar stay full";
            List<component> myCocktails = new List<component>();
            //var mCocktails = db.CockTails.ToList();
            var mCocktails = from cocks in db.CockTails select new { cocks.CocktailId, cocks.Name };
            mCocktails = mCocktails.OrderBy(c => c.Name);

             foreach (var c in mCocktails)
            {
                component myComponent = new component();
                myComponent.Id = c.CocktailId;
                myComponent.Name = c.Name;
                myCocktails.Add(myComponent);
                myComponent = null;
            }
            ViewBag.Title = "All Cocktails";
            ViewBag.myControl = "Cocktail";
            return View(myCocktails);
        }

        public ActionResult About()
        {
            //ViewBag.Message = "Your app description page.";

            //return View("Index");
            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
