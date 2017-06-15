using System.Threading;
using System.Web.Mvc;
using MVCWebUIComponent.Filter;

namespace MVCWebUIComponent.Controllers
{
    public class BaseController : Controller
    {
        protected override void ExecuteCore()
        {
            string cultureName = null;
            if (Request.QueryString["localize"] != null && Request.QueryString["localize"] != string.Empty)
            {
                cultureName = Request.QueryString["localize"];
                Session["cultureName"] = cultureName;
            }
            else
            {
                if (Session["cultureName"] == null)
                {
                    cultureName = Request.UserLanguages[0];
                    Session["cultureName"] = cultureName;
                }
                else
                {
                    cultureName = Session["cultureName"] as string;
                }
            }
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(cultureName);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;

            base.ExecuteCore();
        }

    }
}
