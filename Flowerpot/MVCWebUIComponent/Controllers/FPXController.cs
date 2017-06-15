using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FPXAppDesign;
using FPXAppDesign.DesignerClass;
using FPXAppDesign.DesignerClass.Component;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.ServiceLayer;
using MVCWebUIComponent.Models;
using MVCWebUIComponent.Services;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using UtilityComponent.AutoMapper;
using AccordionGroup = MVCWebUIComponent.Models.AccordionGroup;
using DialogButtonType = MVCWebUIComponent.Models.DialogButtonType;
using GridFunction = MVCWebUIComponent.Models.GridFunction;
using GridSortOrder = MVCWebUIComponent.Models.GridSortOrder;
using GridSourceType = MVCWebUIComponent.Models.GridSourceType;
using JsonReader = MVCWebUIComponent.Models.JsonReader;
using TabsGroup = MVCWebUIComponent.Models.TabsGroup;

//Task 0: Sequence
//Task 1: Assembly partial views
//Task 2: Generate URL
//Task 3: Reflect references(service provider)
//Task 4: Configure about two sides mapping
//Task 5: ScriptManager
//Task 6: When submit how to generate URL
//1. new member card & new customer 2. Member enter in  3. Statistics
//GetUserAppList()

namespace MVCWebUIComponent.Controllers
{
    public class FPXController : Controller
    {
        private GridModel InitializeGridModel()
        {
            var model = new GridModel();

            model.TableId = 1;

            var ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var columns = ideaService.GetColumnsByIdea(model.TableId);

            model.MyJsonReader.Root = "Rows";
            model.MyJsonReader.Page = "Page";
            model.MyJsonReader.Total = "TotalPages";
            model.MyJsonReader.Id = "Id";
            model.MyJsonReader.Cell = "Cell";
            model.MyJsonReader.Records = "TotalRecords";

            for (var i = 0; i < columns.Count; i++)
            {
                var column = columns[i];
                model.ColNames.Add(column.ColumnName);

                var colModel = new ColModel();
                colModel.Add("name", column.ColumnName);
                colModel.Add("index", column.ColumnName);
                colModel.Add("search", true);
                //colModel1.Add("formatter", "date");
                colModel.Add("formatoptions", "{srcformat: 'ISO8601Long',  newformat: 'm/d/Y', defaultValue:null}");
                colModel.Add("editable", true);
                colModel.Add("edittype", "text");
                if (column.DataTypeId == 2)
                {
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12,  dataInit: function (element) {  $(element).datepicker({ dateFormat: 'mm/dd/yy' } ) } }");
                }
                else
                {
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12}");
                }
                model.ColModels.Add(colModel);
            }

            model.ViewRecords = true;

            model.SourceType = GridSourceType.json;
            model.Url = "/Test/GridOperation";
            model.RowNum = 5;

            model.RowList.Add(5);
            model.RowList.Add(10);
            model.RowList.Add(20);

            model.Caption = "Grid List Test";
            model.SortName = "Date";
            model.SortOrder = GridSortOrder.desc;

            model.EditUrl = "/Test/GridOperation";

            //model.Theme = "blitzer";
            //model.Theme = "cupertino";
            //model.Theme = "dark-hive";
            //model.Theme = "le-frog";
            model.Theme = "start";
            model.Theme = "swanky-purse";

            return model;
        }

        private LabelModel InitializeLabelModel()
        {
            var labelModel = new LabelModel();
            labelModel.Text = @"My Bills: ";

            return labelModel;
        }

        private ChartModel InitializeChartModel(string title, string type, List<DataSource> dataSources)
        {
            var chartService = new ChartService();
            var analyzer = new Analyzer();
            //string chartTitle = @"Pie";
            //string chartType = @"pie";

            var chartTitle = title;
            var chartType = type;
            var chartModel = chartService.CreateChart(analyzer, chartTitle, chartType, dataSources);
            return chartModel;
        }

        public string RenderHtml(Type type, string name, object value, IDictionary<string, object> attributes)
        {
            var builder = new TagBuilder(type.Name.ToLower());

            builder.GenerateId(name);
            builder.MergeAttributes(attributes, replaceExisting: true);
            builder.MergeAttribute("value", value.ToString(), replaceExisting: true);
            builder.MergeAttribute("name", name, replaceExisting: true);

            return builder.ToString(TagRenderMode.Normal);
        }

        public string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                //sw.Flush();
                return sw.GetStringBuilder().ToString();
            }
        }

        public List<string> GetUserAppList(int userId = 1, string virtualPath = @"~/Xml", string fileName = @"*", string fileExtension = @".Xml", char separator = '_')
        {
            var physicalPath = Server.MapPath(virtualPath);
            if (!Directory.Exists(physicalPath)) return null;

            var fileNames = Directory.GetFiles(physicalPath, fileName + fileExtension, SearchOption.AllDirectories).ToList();
            var userAppList = new List<string>();
            foreach (var name in fileNames)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
                if (fileNameWithoutExtension == null) continue;

                var tempArray = fileNameWithoutExtension.Split(separator);
                if (tempArray[0].Equals(userId.ToString())) userAppList.Add(tempArray[1]);
            }
            return userAppList;
        }

        public ContentResult GridOperation(string oper)
        {
            var gridService = new GridService();
            var result = "";
            oper = oper.Substring(0, 1).ToUpper() + oper.Substring(1, oper.Length - 1);

            var collection = new NameValueCollection();
            collection.Add(Request.Params);
            collection.Add("userId", "1");

            gridService.Show(collection);
            return Content(result);
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Input()
        {
            var userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            var name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            var translator = new TranslatorBetweenDesignerAndModel();


            #region Initialization

            var input = new InputModel
            {
                Label = new LabelModel
                {
                    Text = "Column : "
                },
                Text = new TextModel
                {
                    Text = "",
                    Column = 1
                },
                Button = new ButtonModel
                {
                    Text = "Submit"
                },
                Column = 1,
                Row = 2
            };

            #endregion

            var leftMenu = userApp.LeftMenu;
            var leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            var leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            var leftMenuBuilder = new StringBuilder(leftMenuString);

            var inputString = RenderRazorViewToString("Input", input);


            var mainBody = new StringBuilder();

            mainBody.Append("<br/>");
            mainBody.Append(inputString);

            var mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            var template = "/Views/FPX/Template1.cshtml";
            var path = "/Views/FPX/";
            var htmlName = "Input.html";
            var dic = new Dictionary<string, string>();
            var message = string.Empty;

            dic.Add("title", "FPX Input Page");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");

            var result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            var htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        public ActionResult Test()
        {
            var nameList = new List<string> { "Volvo", "Mack", "Renault", "UD" };
            var razorString = RenderRazorViewToString("Message", nameList);
            //ViewBag.razorString = razorString;
            //razorString = "<form>Enter your password:<input type=text><input type=submit value=\"Log In\"/></form>";
            var str = new MvcHtmlString(razorString);
            ViewBag.razorString = razorString;
            ViewBag.MvcHtmlString = str;
            TextWriter textWriter = new StringWriter();
            textWriter.Write(razorString);
            return View("Index");
            //return PartialView();
        }

        public ActionResult Show()
        {
            //const string userAppFullPath = @"C:\Wen's Assignments\Flowerpot\Flowerpot\FPXProcessorUI\Xml\UserApp.xml";

            var userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            var name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            //ChartDesigner chart = userApp.Pages[1].ChartList[0];

            var translator = new TranslatorBetweenDesignerAndModel();
            //ChartModel chartModel = translator.Map<ChartModel>(chart);
            var dataSources = new List<DataSource>();
            var chartModel = InitializeChartModel("ChartInHtml5", "pie", dataSources);

            #region Initialization

            var input = new InputModel
            {
                Label = new LabelModel
                {
                    Text = "Column : "
                },
                Text = new TextModel
                {
                    Text = "",
                    Column = 1
                },
                Button = new ButtonModel
                {
                    Text = "Submit"
                },
                Column = 1,
                Row = 2
            };
            var datePicker = new DatePickerModel();
            var dateRange = new DateRangeModel();
            var accordion = new AccordionModel()
            {
                AccordionGroups = new List<AccordionGroup>()
                {
                    new AccordionGroup()
                    {
                        Header = "Section 1",
                        Body = "11111"
                    },
                    new AccordionGroup()
                    {
                        Header = "Section 2",
                        Body = "22222"
                    },
                    new AccordionGroup()
                    {
                        Header = "Section 3",
                        Body = "33333"
                    }
                }
            };
            var progressbar = new ProgressbarModel()
            {
                LoadingTimeInterval = 1000,
                IncrementTimeInterval = 50
            };
            var tabs = new TabsModel()
            {
                TabsGroups = new List<TabsGroup>()
                {
                    new TabsGroup()
                    {
                        Header = "Section 1",
                        Body = "11111"
                    },
                    new TabsGroup()
                    {
                        Header = "Section 2",
                        Body = "22222"
                    },
                    new TabsGroup()
                    {
                        Header = "Section 3",
                        Body = "33333"
                    }
                }
            };
            var menu = new MenuModel()
            {
                MenuItemList = new List<MenuItem>()
                {
                    new MenuItem(){Id = "1",Item = "Section 1",
                    Link = "#",
                    State = true,
                    NodeLevel = 0,
                    ParentNode = null
                },
            new MenuItem(){
                Id = "2",               
                Item = "Section 2",
                Link = "#",
                State = true,
                NodeLevel = 0,
                ParentNode = null
            },
            new MenuItem()
            {
                Id = "3",                
                Item = "Section 3",
                Link = "#",
                State = true,
                NodeLevel = 1,
                ParentNode = "2"
            },
            new MenuItem()
            {
                Id = "4",               
                Item = "Section 4",
                Link = "#",
                State = true,
                NodeLevel = 1,
                ParentNode = "2"
            },
            new MenuItem()
            {
                Id = "5",                
                Item = "Section 5",
                Link = "#",
                State = true,
                NodeLevel = 2,
                ParentNode = "3"
            },
            new MenuItem()
            {
                Id = "6",               
                Item = "Section 6",
                Link = "#",
                State = true,
                NodeLevel = 0,
                ParentNode = null
            }
        }
            };
            var dialog = new DialogModel()
            {
                Title = "Basic Dialog",
                Content = "This is an animated dialog which is useful for displaying information. The dialog window can be moved, resized and closed with the 'x' icon.",
                IsModal = true,
                DialogType = DialogButtonType.OKCancel
            };

            var billList = InitializeGridModel();
            var title = InitializeLabelModel();

            #endregion

            var leftMenu = userApp.LeftMenu;
            var leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            var leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            var leftMenuBuilder = new StringBuilder(leftMenuString);

            var inputString = RenderRazorViewToString("Input", input);
            var datePickerString = RenderRazorViewToString("DatePicker", datePicker);
            var dateRangeString = RenderRazorViewToString("DateRange", dateRange);
            var accordionString = RenderRazorViewToString("Accordion", accordion);
            var progressbarString = RenderRazorViewToString("Progressbar", progressbar);
            var tabsString = RenderRazorViewToString("Tabs", tabs);
            //string menuString = RenderRazorViewToString("Menu", menu);
            var dialogString = RenderRazorViewToString("Dialog", dialog);
            var gridString = RenderRazorViewToString("Grid", billList);
            var labelString = RenderRazorViewToString("Label", title);

            var chartString = RenderRazorViewToString("Chart", chartModel);

            var mainBody = new StringBuilder();

            mainBody.Append(labelString);
            //mainBody.Append(gridString);
            mainBody.Append(chartString);
            mainBody.Append("<br/>");
            mainBody.Append(inputString);
            mainBody.Append("<br/>");
            mainBody.Append(datePickerString);
            mainBody.Append("<br/>");
            mainBody.Append(dateRangeString);
            mainBody.Append("<br/>");
            mainBody.Append(accordionString);
            mainBody.Append("<br/>");
            mainBody.Append(progressbarString);
            mainBody.Append("<br/>");
            mainBody.Append(tabsString);
            mainBody.Append("<br/>");
            //stringBuilder.Append(menuString);
            mainBody.Append("<br/>");
            mainBody.Append(dialogString);
            mainBody.Append("<br/>");

            var mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            var template = "/Views/FPX/Template1.cshtml";
            var path = "/Views/FPX/";
            var htmlName = "Input.html";
            var dic = new Dictionary<string, string>();
            var message = string.Empty;

            dic.Add("title", "FPX Input Page");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");

            var result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            var htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        [HttpPost]
        public ActionResult Input(string test)
        {
            var inputModel = new InputModel();
            TempData["inputModel"] = inputModel;
            return View("Index");
        }

        public string RenderAllComponentsIntoTemplate(string templateName = @"Template1.cshtml", string path = @"/Views/App/", params string[] componentString)
        {
            const string htmlPageName = "Input.html";
            var dic = new Dictionary<string, string>();
            var message = string.Empty;

            for (var i = 0; i < componentString.Length; i++)
            {
                dic.Add("Component" + i, componentString[i]);
            }

            var result = HtmlGenerator.Create(path + templateName, path, htmlPageName, dic, ref message);

            //Remove redundant $Component$ in template
            //int startIndex = result.IndexOf("$", StringComparison.Ordinal);
            //int lastIndex = result.LastIndexOf("$", StringComparison.Ordinal);
            //result = result.Remove(startIndex, lastIndex - startIndex);

            return result;
        }

        public ActionResult Statistics()
        {
            var userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            var chart = userApp.Pages[1].ChartList[0];

            var translator = new TranslatorBetweenDesignerAndModel();
            //ChartModel chartModel = translator.Map<ChartModel>(chart);
            var chartModel = ChartModel.GetChartSourceByAnalyzerId(1);
            //ChartModel chartModel = ChartModel.InitializeChartModel();
            var leftMenu = userApp.LeftMenu;
            var leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            var leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            var leftMenuBuilder = new StringBuilder(leftMenuString);

            var chartString = RenderRazorViewToString("Chart", chartModel);

            var mainBody = new StringBuilder();
            mainBody.Append(chartString);


            var mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            //ViewBag.MvcHtmlString = mvcHtmlString;
            //ViewBag.razorString = mainBody.ToString();

            var result = RenderAllComponentsIntoTemplate(@"Template1.cshtml", @"/Views/App/", "FPX Gym Page", leftMenuBuilder.ToString(), mainBody.ToString());


            ViewBag.Result = result;
            //HtmlString htmlString = new HtmlString(result);
            //ViewBag.HtmlSting = htmlString;
            return View("PageT");

            //MvcHtmlString mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            //ViewBag.MvcHtmlString = mvcHtmlString;
            //ViewBag.razorString = mainBody.ToString();

            //string template = "/Views/FPX/Template1.cshtml";
            //string path = "/Views/App/";
            //string htmlName = "Input.html";
            //Dictionary<string, string> dic = new Dictionary<string, string>();
            //string message = string.Empty;

            //dic.Add("title", "FPX Gym App");
            //dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            //dic.Add("Component1", mainBody.ToString());
            //dic.Add("Component2", "");

            //string result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            //ViewBag.Result = result;
            //HtmlString htmlString = new HtmlString(result);
            //ViewBag.HtmlSting = htmlString;
            //return View("PageT");
        }
    }

    //public class TranslatorBetweenDesignerAndModel : AutoMapperWrapper
    //{
    //    public TranslatorBetweenDesignerAndModel()
    //    {
    //        Initialize(cfg =>
    //        {
    //            cfg.CreateMap<AccordionDesigner, AccordionModel>();
    //            cfg.CreateMap<ButtonDesigner, ButtonModel>();
    //            cfg.CreateMap<ChartDesigner, ChartModel>();
    //            cfg.CreateMap<DatePickerDesigner, DatePickerModel>();
    //            cfg.CreateMap<DateRangeDesigner, DateRangeModel>();
    //            cfg.CreateMap<DialogDesigner, DialogModel>();

    //            cfg.CreateMap<GridDesigner, GridModel>();
    //            cfg.CreateMap<FPXAppDesign.DesignerClass.Component.JsonReader, JsonReader>();

    //            cfg.CreateMap<InputDesigner, InputModel>();
    //            cfg.CreateMap<LabelDesigner, LabelModel>();
    //            cfg.CreateMap<LocalizedDisplayNameDesigner, LocalizedDisplayName>();
    //            cfg.CreateMap<MenuDesigner, MenuModel>();
    //            cfg.CreateMap<ProgressbarDesigner, ProgressbarModel>();
    //            cfg.CreateMap<RowDesigner, RowModel>();
    //            cfg.CreateMap<TabsDesigner, TabsModel>();
    //            cfg.CreateMap<TextDesigner, TextModel>();
    //            cfg.CreateMap<LeftMenuItemDesigner, LeftMenuItem>();
    //        });
    //        var mapper = Configuration.CreateMap<ColumnModelDesigner, ColModel>();
    //        //mapper.ForMember(d => d.ColumnModels, opt => opt.MapFrom(s => s.ColumnModels));
    //        mapper.ConstructUsing(s => DictionaryToListKeyValuePair(s));
    //    }

    //    public static ColModel DictionaryToListKeyValuePair(ColumnModelDesigner columnModel)
    //    {
    //        if (columnModel == null) return null;
    //        ColModel colModel = new ColModel();

    //        foreach (var keyValuePair in columnModel)
    //        {
    //            if (keyValuePair != null)
    //            {
    //                colModel.Add(keyValuePair.Key, keyValuePair.Value);
    //            }
    //        }
    //        return colModel;
    //    }

    //    public static List<ColModel> ListDictionaryToListListKeyValuePair(List<ColumnModelDesigner> keyValuePairListList)
    //    {
    //        if (keyValuePairListList == null) return null;
    //        List<ColModel> dictionaryList = new List<ColModel>();
    //        foreach (var keyValuePairList in keyValuePairListList)
    //        {
    //            if (keyValuePairList != null)
    //            {
    //                foreach (var keyValuePair in keyValuePairList)
    //                {
    //                    if (keyValuePair != null)
    //                    {
    //                        var dic = new ColModel() { { keyValuePair.Key, keyValuePair.Value } };
    //                        dictionaryList.Add(dic);
    //                    }
    //                }
    //            }
    //        }
    //        return dictionaryList;
    //    }
    //}

    /// <summary>
    /// JSON Serialization and Deserialization Assistant Class
    /// </summary>
    public class JsonHelper
    {
        public static string JsonSerializer<T>(T t)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream();
            ser.WriteObject(ms, t);
            var jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            var ser = new DataContractJsonSerializer(typeof(T));
            var ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            var obj = (T)ser.ReadObject(ms);
            return obj;
        }
    }

    //public class HtmlGenerator
    //{
    //    public static string Create(string template, string path, string htmlName, Dictionary<string, string> dic, ref string message)
    //    {
    //        string templatePath = System.Web.HttpContext.Current.Server.MapPath(template);
    //        string htmlPath = System.Web.HttpContext.Current.Server.MapPath(path);
    //        string htmlNamePath = Path.Combine(htmlPath, htmlName);
    //        Encoding encode = Encoding.UTF8;
    //        StringBuilder html = new StringBuilder();

    //        html.Append(File.ReadAllText(templatePath, encode));

    //        foreach (System.Collections.Generic.KeyValuePair<string, string> d in dic)
    //        {
    //            html.Replace(string.Format("${0}$", d.Key), d.Value);
    //        }

    //        if (!Directory.Exists(htmlPath)) Directory.CreateDirectory(htmlPath);

    //        File.WriteAllText(htmlNamePath, html.ToString(), encode);

    //        return html.ToString();
    //    }
    //}

    public class UrlHelper
    {

    }

    public class FPXView : IView
    {
        private void Write(TextWriter textWriter, string sTemplate, params object[] oValues)
        {
            textWriter.Write(string.Format(sTemplate, oValues));
        }

        public void Render(ViewContext viewContext, TextWriter textWriter)
        {
            Write(textWriter, "<u>Route Information</u>");
            foreach (var sKey in viewContext.RouteData.Values.Keys)
            {
                Write(textWriter, "<br/><b>Key</b>: {0}, <b>Value</b>: {1}", sKey, viewContext.RouteData.Values[sKey]);
            }

            Write(textWriter, "<br/><u>View Information</u>");
            foreach (var sKey in viewContext.ViewData.Keys)
            {
                Write(textWriter, "</br><b>Key</b>: {0}, <b>Value</b>: {1}", sKey, viewContext.ViewData[sKey]);
            }

            Write(textWriter, "<br/><u>Model Information</u></br>");
            var userAppList = viewContext.ViewData.Model as List<UserApp>;
            Write(textWriter, "<table width='200px'><tr><th align='left'>Id</th><th align='left'>Name</th></tr>");

            foreach (var p in userAppList)
            {
                Write(textWriter, "<tr><td>{0}</td><td>{1}</td></tr>", p.User, p.Pages);
            }

            Write(textWriter, "</table>");
        }
    }
}
