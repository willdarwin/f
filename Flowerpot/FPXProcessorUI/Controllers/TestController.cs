using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Reflection;
using System.Collections.Specialized;
using FPXAppDesign;
using FPXAppDesign.DesignerClass;
using FPXProcessorUI.Models;
using FPXProcessorUI.Services;
using IdeaDomain.DomainLayer.Entities;

namespace FPXProcessorUI.Controllers
{
    public class TestController : Controller
    {
        //public TranslatorBetweenDesignerAndModel Translator = new TranslatorBetweenDesignerAndModel();
        //
        // GET: /Test/

        public ActionResult Index()
        {
            //const string path = @"C:\Wen's Assignments\Flowerpot\Flowerpot\FPXProcessorUI\Xml\UserApp.xml";
            //FPXAppManager appManager = new FPXAppManager();
            //UserApp userApp = appManager.DeserializeUserApp(path);
            //FPXAppDesign.DesignerClass.Component.GridModel grid = userApp.Pages[1].GridList[1];
            //GridModel gridModel = Translator.Map<GridModel>(grid);

            return View("Show");
        }

        public ActionResult Show()
        {
            ChartService chartService = new ChartService();
            Analyzer analyzer = new Analyzer();
            string chartTitle = @"Pie";
            string chartType = @"pie";
            ChartModel chartModel = chartService.CreateChart(analyzer, chartTitle, chartType);

            return View("Chart", chartModel);
        }

        public ContentResult GridOperation(string oper)
        {
            GridService gridService = new GridService();
            string result = "";
            oper = oper.Substring(0, 1).ToUpper() + oper.Substring(1, oper.Length - 1);
            
            NameValueCollection collection = new NameValueCollection();
            collection.Add(Request.Params);
            //collection.Add("userId", Session["UserId"].ToString());
            collection.Add("userId", "1");

            gridService.Show(collection);
            return Content(result);
        }
    }
}
