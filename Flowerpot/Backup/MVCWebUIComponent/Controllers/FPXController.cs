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
            GridModel model = new GridModel();

            model.TableId = 1;

            IIdeaService ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            List<ColumnInIdea> columns = ideaService.GetColumnsByIdea(model.TableId);

            model.MyJsonReader.Root = "Rows";
            model.MyJsonReader.Page = "Page";
            model.MyJsonReader.Total = "TotalPages";
            model.MyJsonReader.Id = "Id";
            model.MyJsonReader.Cell = "Cell";
            model.MyJsonReader.Records = "TotalRecords";

            for (int i = 0; i < columns.Count; i++)
            {
                ColumnInIdea column = columns[i];
                model.ColNames.Add(column.ColumnName);

                ColModel colModel = new ColModel();
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
            LabelModel labelModel = new LabelModel();
            labelModel.Text = @"My Bills: ";

            return labelModel;
        }

        private ChartModel InitializeChartModel(string title, string type, List<DataSource> dataSources)
        {
            ChartService chartService = new ChartService();
            Analyzer analyzer = new Analyzer();
            //string chartTitle = @"Pie";
            //string chartType = @"pie";

            string chartTitle = title;
            string chartType = type;
            ChartModel chartModel = chartService.CreateChart(analyzer, chartTitle, chartType, dataSources);
            return chartModel;
        }

        public string RenderHtml(Type type, string name, object value, IDictionary<string, object> attributes)
        {
            TagBuilder builder = new TagBuilder(type.Name.ToLower());

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
            string physicalPath = Server.MapPath(virtualPath);
            if (!Directory.Exists(physicalPath)) return null;

            List<string> fileNames = Directory.GetFiles(physicalPath, fileName + fileExtension, SearchOption.AllDirectories).ToList();
            var userAppList = new List<string>();
            foreach (string name in fileNames)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
                if (fileNameWithoutExtension == null) continue;

                string[] tempArray = fileNameWithoutExtension.Split(separator);
                if (tempArray[0].Equals(userId.ToString())) userAppList.Add(tempArray[1]);
            }
            return userAppList;
        }

        public ContentResult GridOperation(string oper)
        {
            GridService gridService = new GridService();
            string result = "";
            oper = oper.Substring(0, 1).ToUpper() + oper.Substring(1, oper.Length - 1);

            NameValueCollection collection = new NameValueCollection();
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
            string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            string name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            TranslatorBetweenDesignerAndModel translator = new TranslatorBetweenDesignerAndModel();


            #region Initialization

            InputModel input = new InputModel
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

            LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
            LeftMenuItem[] leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            string leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            StringBuilder leftMenuBuilder = new StringBuilder(leftMenuString);

            string inputString = RenderRazorViewToString("Input", input);


            StringBuilder mainBody = new StringBuilder();

            mainBody.Append("<br/>");
            mainBody.Append(inputString);

            MvcHtmlString mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            string template = "/Views/FPX/Template1.cshtml";
            string path = "/Views/FPX/";
            string htmlName = "Input.html";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string message = string.Empty;

            dic.Add("title", "FPX Input Page");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");

            string result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            HtmlString htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        public ActionResult Test()
        {
            List<string> nameList = new List<string> { "Volvo", "Mack", "Renault", "UD" };
            string razorString = RenderRazorViewToString("Message", nameList);
            //ViewBag.razorString = razorString;
            //razorString = "<form>Enter your password:<input type=text><input type=submit value=\"Log In\"/></form>";
            MvcHtmlString str = new MvcHtmlString(razorString);
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

            string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            string name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            //ChartDesigner chart = userApp.Pages[1].ChartList[0];

            TranslatorBetweenDesignerAndModel translator = new TranslatorBetweenDesignerAndModel();
            //ChartModel chartModel = translator.Map<ChartModel>(chart);
            List<DataSource> dataSources = new List<DataSource>();
            ChartModel chartModel = InitializeChartModel("ChartInHtml5", "pie", dataSources);

            #region Initialization

            InputModel input = new InputModel
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
            DatePickerModel datePicker = new DatePickerModel();
            DateRangeModel dateRange = new DateRangeModel();
            AccordionModel accordion = new AccordionModel()
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
            ProgressbarModel progressbar = new ProgressbarModel()
            {
                LoadingTimeInterval = 1000,
                IncrementTimeInterval = 50
            };
            TabsModel tabs = new TabsModel()
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
            MenuModel menu = new MenuModel()
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
            DialogModel dialog = new DialogModel()
            {
                Title = "Basic Dialog",
                Content = "This is an animated dialog which is useful for displaying information. The dialog window can be moved, resized and closed with the 'x' icon.",
                IsModal = true,
                DialogType = DialogButtonType.OKCancel
            };

            GridModel billList = InitializeGridModel();
            LabelModel title = InitializeLabelModel();

            #endregion

            LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
            LeftMenuItem[] leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            string leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            StringBuilder leftMenuBuilder = new StringBuilder(leftMenuString);

            string inputString = RenderRazorViewToString("Input", input);
            string datePickerString = RenderRazorViewToString("DatePicker", datePicker);
            string dateRangeString = RenderRazorViewToString("DateRange", dateRange);
            string accordionString = RenderRazorViewToString("Accordion", accordion);
            string progressbarString = RenderRazorViewToString("Progressbar", progressbar);
            string tabsString = RenderRazorViewToString("Tabs", tabs);
            //string menuString = RenderRazorViewToString("Menu", menu);
            string dialogString = RenderRazorViewToString("Dialog", dialog);
            string gridString = RenderRazorViewToString("Grid", billList);
            string labelString = RenderRazorViewToString("Label", title);

            string chartString = RenderRazorViewToString("Chart", chartModel);

            StringBuilder mainBody = new StringBuilder();

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

            MvcHtmlString mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            string template = "/Views/FPX/Template1.cshtml";
            string path = "/Views/FPX/";
            string htmlName = "Input.html";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string message = string.Empty;

            dic.Add("title", "FPX Input Page");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");

            string result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            HtmlString htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        [HttpPost]
        public ActionResult Input(string test)
        {
            InputModel inputModel = new InputModel();
            TempData["inputModel"] = inputModel;
            return View("Index");
        }

        public string RenderAllComponentsIntoTemplate(string templateName = @"Template1.cshtml", string path = @"/Views/App/", params string[] componentString)
        {
            const string htmlPageName = "Input.html";
            var dic = new Dictionary<string, string>();
            string message = string.Empty;

            for (int i = 0; i < componentString.Length; i++)
            {
                dic.Add("Component" + i, componentString[i]);
            }

            string result = HtmlGenerator.Create(path + templateName, path, htmlPageName, dic, ref message);

            //Remove redundant $Component$ in template
            //int startIndex = result.IndexOf("$", StringComparison.Ordinal);
            //int lastIndex = result.LastIndexOf("$", StringComparison.Ordinal);
            //result = result.Remove(startIndex, lastIndex - startIndex);

            return result;
        }

        public ActionResult Statistics()
        {
            string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            ChartDesigner chart = userApp.Pages[1].ChartList[0];

            TranslatorBetweenDesignerAndModel translator = new TranslatorBetweenDesignerAndModel();
            //ChartModel chartModel = translator.Map<ChartModel>(chart);
            ChartModel chartModel = ChartModel.GetChartSourceByAnalyzerId(1);
            //ChartModel chartModel = ChartModel.InitializeChartModel();
            LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
            LeftMenuItem[] leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            string leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            StringBuilder leftMenuBuilder = new StringBuilder(leftMenuString);

            string chartString = RenderRazorViewToString("Chart", chartModel);

            StringBuilder mainBody = new StringBuilder();
            mainBody.Append(chartString);


            MvcHtmlString mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            //ViewBag.MvcHtmlString = mvcHtmlString;
            //ViewBag.razorString = mainBody.ToString();

            string result = RenderAllComponentsIntoTemplate(@"Template1.cshtml", @"/Views/App/", "FPX Gym Page", leftMenuBuilder.ToString(), mainBody.ToString());


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
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
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
            foreach (string sKey in viewContext.RouteData.Values.Keys)
            {
                Write(textWriter, "<br/><b>Key</b>: {0}, <b>Value</b>: {1}", sKey, viewContext.RouteData.Values[sKey]);
            }

            Write(textWriter, "<br/><u>View Information</u>");
            foreach (string sKey in viewContext.ViewData.Keys)
            {
                Write(textWriter, "</br><b>Key</b>: {0}, <b>Value</b>: {1}", sKey, viewContext.ViewData[sKey]);
            }

            Write(textWriter, "<br/><u>Model Information</u></br>");
            var userAppList = viewContext.ViewData.Model as List<UserApp>;
            Write(textWriter, "<table width='200px'><tr><th align='left'>Id</th><th align='left'>Name</th></tr>");

            foreach (UserApp p in userAppList)
            {
                Write(textWriter, "<tr><td>{0}</td><td>{1}</td></tr>", p.User, p.Pages);
            }

            Write(textWriter, "</table>");
        }
    }
}
