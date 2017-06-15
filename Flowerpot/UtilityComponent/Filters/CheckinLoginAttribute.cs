using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web.Routing;

namespace UtilityComponent.Filters
{

    public class CheckinLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                var routes = new RouteValueDictionary(new
                {
                    controller = "Account",
                    action = "Logon",
                    referURL = filterContext.HttpContext.Request.RawUrl
                });
                filterContext.Result = new RedirectToRouteResult(routes);
                return;
            }
        }
    }
}
