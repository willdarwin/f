using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Collections.Specialized;
using UtilityComponent.Filters;

namespace MVCWebUIComponent.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Show()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show2()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show3()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show4()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show5()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show6()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ActionResult Show7()
        {
            Session["UserId"] = 1;
            return View();
        }

        public ContentResult GridOperation(string oper)
        {
            string result = "";
            oper = oper.Substring(0, 1).ToUpper() + oper.Substring(1, oper.Length - 1);
            Type type = Type.GetType("MVCWebUIComponent.Services.GridService");
            object service = Activator.CreateInstance(type);
            MethodInfo methodOper = type.GetMethod(oper);
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            NameValueCollection collection = new NameValueCollection();
            collection.Add(Request.Params);
            collection.Add("userId", Session["UserId"].ToString());
            result = methodOper.Invoke(service, flags, Type.DefaultBinder, new object[] { collection }, null) as string;
            return Content(result);
        }

        public ActionResult Template1()
        {
            Session["UserId"] = 1;
            return View();
        }
    }
}
