using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.WebPages;

namespace Cocktails07
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            //WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
            //The next method is used to inicialize the recognition of iphone for this Application.
            InitializeDisplayModeProviders();
        }
        //The next method is used to inicialize the recognition of iphone for this Application.
         protected void InitializeDisplayModeProviders()
        {
            var phone = new DefaultDisplayMode("Phone")
            {
                //ContextCondition = ctx => ctx.GetOverriddenUserAgent() != null && 
                //    (ctx.GetOverriddenUserAgent().Contains("iPhone") || 
                //    ctx.GetOverriddenUserAgent().Contains("GT-I"))
                    //user agens for iphone and samsung galaxies
                ContextCondition = ctx => ctx.Request.Browser.IsMobileDevice && 
                    !(ctx.GetOverriddenUserAgent().Contains("iPad"))

            };
            DisplayModeProvider.Instance.Modes.Insert(0, phone);
        }
    }
}