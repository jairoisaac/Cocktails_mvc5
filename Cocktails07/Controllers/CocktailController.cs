using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Cocktails07.Models;

namespace Cocktails07.Controllers
{ 
    public class CocktailController : Controller
    {
        private CockTailsIngredientsEntities db = new CockTailsIngredientsEntities();
        private Models.CocktailSumary MyCocktailSumary = new Models.CocktailSumary();
        private Models.CocktaiIngredients MyCoctailIngredient = new Models.CocktaiIngredients();
        //
        // GET: /Cocktail/

        //public ViewResult Index()
        //{
        //    List<component> myComponents = new List<component>();
        //    component mComponent = new component();
        //    component mComponent0 = new component();
        //    mComponent.Id = 1;
        //    mComponent.Name = "Find";
        //    myComponents.Add(mComponent);

        //    mComponent0.Id = 2;
        //    mComponent0.Name = "List";
        //    myComponents.Add(mComponent0);
        //    mComponent = null;

        //    ViewBag.Title = "Options to get the cocktail";
        //    ViewBag.myControl = "Cocktail";
        //    return View("IndexSearches", myComponents);

        //}


        public ViewResult Index()
        {
            try
            {
                //var MyL = from c in db.CockTails select new SelectListItem { Value = c.CocktailId.ToString(), Text = c.Name };
                var MyL = from c in db.CockTails.ToList() select new SelectListItem { Value = c.CocktailId.ToString(), Text = c.Name };
                MyL = MyL.OrderBy(b => b.Text);
                //MyCocktailSumary.Cocktail = MyL.First().Text;
                //MyCocktailSumary.CocktailId = Int32.Parse(MyL.First().Value);
                //MyCocktailSumary.Cocktails = MyL.OrderBy(o => o.Text);
                MyCocktailSumary.Cocktails = MyL;

            }
            catch
            {
                var MyL = from c in db.CockTails select new SelectListItem { Value = "0", Text = "Not a Cocktail yet" };
                MyCocktailSumary.Cocktails = MyL;
            }
            //return View(MyCocktailSumary);
            //return View(IndexView, MyCocktailSumary);
            if (User.IsInRole("Administrator"))
                return View(MyCocktailSumary);
            else
                return View("IndexView", MyCocktailSumary);
                //return View("Find", MyCocktailSumary);
        }
        public ViewResult List()
        {
            try
            {
                //var MyL = from c in db.CockTails select new SelectListItem { Value = c.CocktailId.ToString(), Text = c.Name };
                var MyL = from c in db.CockTails.ToList() select new SelectListItem { Value = c.CocktailId.ToString(), Text = c.Name };
                //MyCocktailSumary.Cocktail = MyL.First().Text;
                //MyCocktailSumary.CocktailId = Int32.Parse(MyL.First().Value);
                //MyCocktailSumary.Cocktails = MyL.OrderBy(o => o.Text);
                MyCocktailSumary.Cocktails = MyL;

            }
            catch
            {
                var MyL = from c in db.CockTails select new SelectListItem { Value = "0", Text = "Not a Cocktail yet" };
                MyCocktailSumary.Cocktails = MyL;
            }
            //return View(MyCocktailSumary);
            //return View(IndexView, MyCocktailSumary);
            if (User.IsInRole("Administrator"))
                return View(MyCocktailSumary);
            else
                return View("IndexView", MyCocktailSumary);
        }

        //public ViewResult Find()
        //{
        //    var MyL = from c in db.CockTails.ToList() select new SelectListItem { Value = c.CocktailId.ToString(), Text = c.Name };
        //    MyCocktailSumary.Cocktails = MyL;
        //    return View(MyCocktailSumary);
        //}


        //
        // GET: /Cocktail/Details/5

        public ViewResult Details(long id)
        {
            CockTail cocktail = db.CockTails.Find(id);
            return View(cocktail);
        }

        //
        // GET: /Cocktail/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            GetAllIngredients();
            return View(MyCoctailIngredient);
            //return View();
        } 

        //
        // POST: /Cocktail/Create

        //[HttpPost]
        //public ActionResult Create(CockTail cocktail)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.CockTails.Add(cocktail);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");  
        //    }

        //    return View(cocktail);
        //}
        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                //var c = db.CockTails.Last().CocktailId;
                //var cId = c.CocktailId;
                CockTail myCok = new CockTail { Name = collection["Name"]};
                db.CockTails.Add(myCok);
                db.SaveChanges();
                myCok = this.InsertAllIngredients(collection["Liquors"], collection["NoAlcohols"], collection["Garnishes"], collection["Cordials"], collection["Glass"], collection["Process"],collection["Categories"], myCok);
                db.CockTails.Find(myCok.CocktailId);
                db.Entry(myCok).State = EntityState.Modified;
                db.SaveChanges();
                //db.CockTails.Add(myCok);
                //db.SaveChanges();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }
        
        //
        // GET: /Cocktail/Edit/5

        [Authorize(Roles="Administrator")]
        public ActionResult Edit(long id)
        {
            CockTail cocktail = db.CockTails.Find(id);
            GetAllIngredients();
            MyCoctailIngredient.CocktailId = (int)id;
            MyCoctailIngredient.Name = cocktail.Name;
            return PartialView("Edit", MyCoctailIngredient);
        }

        //
        // POST: /Cocktail/Edit/5

        [Authorize(Roles="Administrator")]
        [HttpPost]
        public ActionResult Edit(int Id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var MyCocktail = db.CockTails.Single(c => c.CocktailId == Id);
                MyCocktail.Name = collection["Name"];
                //To Do: Code to insert liquor
                //InsertAllIngredients(collection["Liquors"], collection["NoAlcohols"], collection["Garnishes"], collection["Cordials"],mCocktailId );
                MyCocktail = InsertAllIngredients(collection["Liquors"], collection["NoAlcohols"], collection["Garnishes"], collection["Cordials"],collection["Glass"],collection["Process"],collection["Categories"], MyCocktail);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        //
        // GET: /Cocktail/Delete/5

        [Authorize(Roles="Administrator")]
        public ActionResult Delete(long id)
        {
            CockTail cocktail = db.CockTails.Find(id);
            return View(cocktail);
        }

        //
        // POST: /Cocktail/Delete/5

        [Authorize(Roles="Administrator")]
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {            
            CockTail cocktail = db.CockTails.Find(id);
            db.CockTails.Remove(cocktail);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [Authorize(Roles="Administrator")]
        public ActionResult WatchIngredients(long id)
        {
            return PartialView("ViewCocktail", GetRecipe(id));
        }
        public ActionResult WatchCocktail(long id)
        {
            if (User.IsInRole("Payed"))
                return PartialView("ViewCockxIng", GetRecipe(id));
            else
                return PartialView("ViewCockxIngTextOnly", GetRecipe(id));
        }
        public ActionResult EditIngredientAmount(long ckId, string Tp, float Amt, long ingId)
        {
            // TODO: Add insert logic here
            switch (Tp)
            {
                case "L":
                    var MyLiq = (from l in db.CocktailLiquors where l.CocktailId == ckId & l.Id == ingId select l).Single();
                    MyLiq.Amount = Amt;
                    break;
                case "N":
                    var MyNoA = (from n in db.CocktailNoAlcohols where n.CocktailId == ckId & n.Id == ingId select n).Single();
                    MyNoA.Amount = Amt;
                    break;
                case "C":
                    var MyCor = (from c in db.CocktailCordials where c.CocktailId == ckId & c.Id == ingId select c).Single();
                    MyCor.Amount = Amt;
                    break;
            }
            db.SaveChanges();
            return null;
        }
        public ActionResult DeleteIngredient(long ckId, string Tp, long ingId)
        {
            // TODO: Add insert logic here
            switch (Tp)
            {
                case "L":
                    //var MyLiq = (from l in db.CocktailLiquors where l.CocktailId == ckId & l.Id == ingId select l).Single();
                    CocktailLiquor CokLiq = db.CocktailLiquors.Find(ckId, ingId);
                    db.CocktailLiquors.Remove(CokLiq);
                    break;
                case "N":
                    CocktailNoAlcohol CokNoa = db.CocktailNoAlcohols.Find(ckId, ingId);
                    db.CocktailNoAlcohols.Remove(CokNoa);
                    //var MyNoA = (from n in db.CocktailNoAlcohols where n.CocktailId == ckId & n.Id == ingId select n).Single();
                    //db.CocktailNoAlcohols.DeleteObject(MyNoA);
                    break;
                case "C":
                    //var MyCor = (from c in db.CocktailCordials where c.CocktailId == ckId & c.Id == ingId select c).Single();
                    //db.CocktailCordials.DeleteObject(MyCor);
                    CocktailCordial CokCor = db.CocktailCordials.Find(ckId, ingId);
                    db.CocktailCordials.Remove(CokCor);
                    break;
                case "G":
                    CockTail CokGar = db.CockTails.Find(ckId);
                    Garnish Gar = db.Garnishes.Find(ingId);
                    CokGar.Garnishes.Remove(Gar);
                    //var MyGar = (from c in db.CocktailGarnishes where c.CocktailId == ckId & c.Id == ingId select c).Single();
                    //db.CocktailGarnishes.DeleteObject(MyGar);
                    break;
                case "P":
                    CockTail CokPro = db.CockTails.Find(ckId);
                    Process Pro = db.Processes.Find(ingId);
                    CokPro.Processes.Remove(Pro);
                    //var MyGar = (from c in db.CocktailGarnishes where c.CocktailId == ckId & c.Id == ingId select c).Single();
                    //db.CocktailGarnishes.DeleteObject(MyGar);
                    break;
            }
            db.SaveChanges();
            return null;
        }
        public void GetAllIngredients()
        {
            var MyL = from c in db.Liquors.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyN = from c in db.NoAlcohols.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyC = from c in db.Cordials.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyG = from c in db.Garnishes.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyGl = from c in db.Glasses.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyP = from c in db.Processes.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };
            var MyR = from c in db.Categories.ToList() select new SelectListItem { Value = c.Id.ToString(), Text = c.Name };

            MyCoctailIngredient.Liquors = MyL;
            MyCoctailIngredient.NoAlcohols = MyN;
            MyCoctailIngredient.Cordials = MyC;
            MyCoctailIngredient.Garnishes = MyG;
            MyCoctailIngredient.Glass = MyGl;
            MyCoctailIngredient.Process = MyP;
            MyCoctailIngredient.Categories = MyR;
        }
        public CockTail InsertAllIngredients(string liq, string NoA, string gar, string cor, string gla, string pro, string cat, CockTail Coc)
        {
            
            // Liquor
            if (liq != null)
            {
                var Myliq = this.MyStringToInteger(liq);
                for (var i = 0; i < Myliq.Length; i++)
                {
                    Coc.CocktailLiquors.Add(new CocktailLiquor {CocktailId=Coc.CocktailId,Id=Myliq[i]});
                }
            }
            // No Alcohols
            if (NoA != null)
            {
                var MyNoA = this.MyStringToInteger(NoA);
                for (var i = 0; i < MyNoA.Length; i++)
                {
                    Coc.CocktailNoAlcohols.Add(new CocktailNoAlcohol {CocktailId=Coc.CocktailId,Id=MyNoA[i]});
                }
            }
            // Cordials
            if (cor != null)
            {
                var Mycor = this.MyStringToInteger(cor);
                for (var i = 0; i < Mycor.Length; i++)
                {
                    Coc.CocktailCordials.Add(new CocktailCordial { CocktailId = Coc.CocktailId,Id=Mycor[i]});
                }
            }
            // Garnishes
            if (gar != null)
            {
                var Mygar = this.MyStringToInteger(gar);
                for (var i = 0; i < Mygar.Length; i++)
                {
                    Coc.Garnishes.Add(db.Garnishes.Find(Mygar[i]));
                }
            }
            // Processes
            if (pro != null)
            {
                var Mypro = this.MyStringToInteger(pro);
                for (var i = 0; i < Mypro.Length; i++)
                {
                    Coc.Processes.Add(db.Processes.Find(Mypro[i]));
                }
            }

            // Glass
            Coc.GlassId = Convert.ToInt16(gla);
           // Categories
            Coc.CategoryId = Convert.ToInt16(cat);
           // Coc.ProcessId = Convert.ToInt16(pro);

            return Coc;
        }
        public int[] MyStringToInteger(string a)
        {
            //To convert a string like "1,5,14,2" into an array of integers like arr[0]=1,arr[1]=5 ...
            List<int> MyIds = new List<int>();
            while (a.IndexOf(",") != -1)
            {
                MyIds.Add(Convert.ToInt16(a.Substring(0, a.IndexOf(","))));
                a = a.Substring(a.IndexOf(",") + 1);
            }
            MyIds.Add(Convert.ToInt16(a));
            return MyIds.ToArray();
        }
        public JsonResult GetCocktails(string term)
        {
            var Cocktails = from c in db.CockTails
                            where c.Name.StartsWith(term)
                            select new
                            {
                                label = c.Name,
                                value = c.CocktailId
                            };
            var mylist = Cocktails.ToList();
            //Cocktails = db.CockTails.Where(x => x.Name.StartsWith(term)).Select(new
            //{
            //    label = x.Field<string>("Name"),
            //    value = x.Field<int>("CocktailId"),
            //}).ToList();

            //Cocktails = db.CockTails.Where(x => x.Name.StartsWith(term)).Select(x => x.Name).ToList();
            //Cocktails = db.CockTails.Where(x => x.Name.StartsWith(term)).ToList();
            return Json(mylist, JsonRequestBehavior.AllowGet);
        }

        public List<Ingredient> GetRecipe(long id) 
        {
            //HttpRequest currentRequest = HttpContext.Request.;
            //HttpRequest currentRequest = HttpContext.Current.Request;
            //String clientIP = currentRequest.ServerVariables["REMOTE_ADDR"];
            String ipAddress =   System.Web.HttpContext.Current.Request.UserHostAddress;
 
            UserUsage myUser = new UserUsage { dateTimeUpdate = DateTime.Now, CocktailId = id, Address = ipAddress };
            List<Ingredient> MyIngredients = new List<Ingredient>();
            var Mliquor = (from c in db.CocktailLiquors where c.CocktailId == id select new { c.Amount, c.Liquor.Name, c.Id, c.CocktailId }).ToList();
            var MnoAlcohols = (from b in db.CocktailNoAlcohols where b.CocktailId == id select new { b.Amount, b.NoAlcohol.Name, b.Id, b.CocktailId }).ToList();
            var Mcordials = (from a in db.CocktailCordials where a.CocktailId == id select new { a.Amount, a.Cordial.Name, a.Id, a.CocktailId }).ToList();
            //var Mgarnishes = db.Garnishes.Select(g => g.CockTails.Select(co => co.CocktailId == id)).ToList();
            //var Mgarnishes = (from d in db.Garnishes select d.CockTails.Select(s=>s.CocktailId==id)).ToList();
            //var Mgarnishes = (from d in db.Garnishes.ToList() let Amount =(float)1.0 where d.CockTails.Where == id select new {d.Name,d. }).ToList();
            var Mgarnishes = (from d in db.Garnishes from e in d.CockTails where e.CocktailId == id select new { d.Name, d.Id, id }).ToList();
            var Mprocesses = (from d in db.Processes from e in d.CockTails where e.CocktailId == id select new { d.Name, d.Id, id }).ToList();

            var Mglass = (from gl in db.CockTails where gl.CocktailId == id select gl.Glass).Single();
            var Mcategory = (from ct in db.CockTails where ct.CocktailId == id select ct.Category).Single();
            //var Mprocess = (from p in db.CockTails where p.CocktailId == id select p.Process).Single();

            foreach (var ing in Mliquor)
            {
                MyIngredients.Add(new Ingredient { CocktailId = (int)ing.CocktailId, Id = ing.Id, Type = "L", Amount = ing.Amount, Name = ing.Name });
            }
            foreach (var ing in MnoAlcohols)
            {
                MyIngredients.Add(new Ingredient { CocktailId = (int)ing.CocktailId, Id = ing.Id, Type = "N", Amount = ing.Amount, Name = ing.Name });
            }
            foreach (var ing in Mcordials)
            {
                MyIngredients.Add(new Ingredient { CocktailId = (int)ing.CocktailId, Id = ing.Id, Type = "C", Amount = ing.Amount, Name = ing.Name });

            }
            foreach (var ing in Mgarnishes)
            {
                //    MyIngredients.Add(new Ingredient { CocktailId = (int)ing.CocktailId, Id = ing.Id, Type = "G", Amount = 1, Name = ing.Name });
                MyIngredients.Add(new Ingredient { CocktailId = (int)id, Id = ing.Id, Type = "G", Amount = 1, Name = ing.Name });
            }
            foreach (var ing in Mprocesses)
            {
                //    MyIngredients.Add(new Ingredient { CocktailId = (int)ing.CocktailId, Id = ing.Id, Type = "G", Amount = 1, Name = ing.Name });
                MyIngredients.Add(new Ingredient { CocktailId = (int)id, Id = ing.Id, Type = "P", Amount = 1, Name = ing.Name });
            }

            MyIngredients.Add(new Ingredient { CocktailId = (int)id, Id = id, Type = "S", Amount = 1, Name = Mglass.Name }); // Here is adding a glass
            //MyIngredients.Add(new Ingredient { CocktailId = (int)id, Id = id, Type = "R", Amount = 1, Name = Mcategory.Name }); // Here is adding a the category
            //MyIngredients.Add(new Ingredient { CocktailId = (int)id, Id = id, Type = "P", Amount = 1, Name = Mprocess.Name });
            db.UserUsages.Add(myUser);
            db.SaveChanges();
            return MyIngredients;
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        public ActionResult SeeCocktailNoAjx(long id)
        {
            CockTail cocktail = db.CockTails.Find(id);
            ViewBag.Title = cocktail.Name;
            return View("SeeCocktailNoAjxPd",GetRecipe(id));
        }

    }
}