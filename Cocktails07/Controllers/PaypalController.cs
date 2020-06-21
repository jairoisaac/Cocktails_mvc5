using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Configuration;
using Cocktails07.Models;

namespace Cocktails07.Controllers
{
    public class PaypalController : Controller
    {
        //
        // GET: /Paypal/
        public ActionResult Index()
        {
            if (User.IsInRole("NoPayed") || !User.Identity.IsAuthenticated)
            {
                ViewBag.Message = "Buy the PROFESSIONAL edition of Basic Cocktails";
                //ViewBag.Message0 = "May your tip jar stay full";
                return View();
            }
            else { return RedirectToAction("Index", "Home"); }
        }

        public ActionResult RedirectFromPaypal()
        {
            Roles.RemoveUserFromRole(User.Identity.Name, "NoPayed");
            Roles.AddUserToRole(User.Identity.Name, "Payed");
            return View();
        }
        public ActionResult CancelFromPaypal()
        {
            return View();
        }
        public ActionResult NotifyFromPaypal()
        {
            return View();
        }
        [Authorize]
        public ActionResult ValidateCommand(string product, string totalPrice)
        {
            bool useSandbox = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSandbox"]);
            var paypal = new PaypalModel(useSandbox);

            paypal.item_name = product;
            paypal.amount = totalPrice;
            return View(paypal);
        }
    }
}
