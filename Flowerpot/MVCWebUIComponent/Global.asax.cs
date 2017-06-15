using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MVCWebUIComponent
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            //bool result = System.Web.HttpContext.Current.User.Identity.IsAuthenticated;
            //if (result)
            //{
            //    string userId = System.Web.HttpContext.Current.User.Identity.Name;
            //    Session.Add("UserId", userId);//一般是把User对象放入到Session中，方便以后随时用随时拿。在本项目中，因为只用到了Id,所以，放的只是Id
            //}
            //else
            //{
            //    Response.Redirect("/Account/Logon?returnUrl=" + Request.RawUrl);
            //    //FormsAuthentication.RedirectToLoginPage(Request.RawUrl);
            //    //Response.End();
            //    //Response.Redirect();
            //}
        }
    }
}