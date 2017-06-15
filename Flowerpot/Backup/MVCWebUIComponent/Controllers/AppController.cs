using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using UtilityComponent.AutoMapper;
using UtilityComponent.Filters;
using MVCWebUIComponent.Models;
using System.IO;
using IdeaDomain.ServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer;
using IdeaDomain.DomainLayer.Entities;
using System.Text;
using FPXAppDesign;
using FPXAppDesign.DesignerClass;
using FPXAppDesign.DesignerClass.Component;
using System.Collections.Specialized;

namespace MVCWebUIComponent.Controllers
{
    //[CheckinLogin]
    public class AppController : Controller
    {
        #region ----Xml Upload----

        [HttpPost]
        public JsonResult UploadXml(HttpPostedFileBase fileData)
        {
            try
            {
                //string hostURL = "http://" + Request.Url.Host + ":" + Request.Url.Port + "/UploadPicture/";
                //string hostURL = System.Configuration.ConfigurationManager.AppSettings["HostUrl"] + "UploadPicture/";
                //string newURL = hostURL + fileData.FileName;

                if (Request.RequestContext.HttpContext.Session == null) return null;

                int userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);
                Dictionary<string, string> userAppList = GetUserAppList(userId);

                #region Need to process the same UserApp name

                if (userAppList.Any(userApp => userApp.Key == fileData.FileName.Split('.')[0]))
                {
                    return Json(new
                        {
                            HasError = "1",
                            ErrMsg = " Upload failed. Because there is already a file with the same name in this location.",
                        });
                }

                #endregion

                string storeFilePath = Server.MapPath("~/Xml/" + SetFileName(fileData.FileName, userId));
                fileData.SaveAs(storeFilePath);

                return Json(new
                {
                    HasError = "0",
                    IsSuccess = "1",
                    Count = userAppList.Count()
                });
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    HasError = "1",
                    ErrMsg = "Upload failed. " + ex.Message,
                });
            }
        }

        private string SetFileName(string fileName, int id)
        {
            string tmp = fileName.Remove(fileName.Length - 4);
            string time = DateTime.Now.ToShortDateString().Replace('/', '-');
            string newName = String.Format("{0}_{1}_{2}.xml", id.ToString(), tmp, time);
            return newName;
        }

        public Dictionary<string, string> GetUserAppList(int userId = 1, bool isFileNameWithExtension = false,
                                           string virtualPath = @"~/Xml", string fileName = @"*",
                                           string fileExtension = @".Xml", char separator = '_')
        {
            string physicalPath = Server.MapPath(virtualPath);
            if (!Directory.Exists(physicalPath)) return null;

            List<string> fileNamesIncludingPath = Directory.GetFiles(physicalPath, fileName + fileExtension, SearchOption.AllDirectories).ToList();
            var userAppList = new Dictionary<string, string>();

            foreach (string name in fileNamesIncludingPath)
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
                string fileNameWithExtension = Path.GetFileName(name);
                if (fileNameWithoutExtension == null) continue;

                if (isFileNameWithExtension)
                    userAppList.Add(fileNameWithoutExtension, name);
                else
                {
                    string[] tempArray = fileNameWithoutExtension.Split(separator);
                    if (tempArray[0].Equals(userId.ToString())) userAppList.Add(tempArray[1], fileNameWithExtension);
                }
            }
            return userAppList;
        }
        #endregion

        #region ----Component Initialization----

        private GridModel InitializeGridModel(GridDesigner gridDesigner)
        {
            GridModel model = new GridModel();

            IIdeaService ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            Idea idea = ideaService.GetIdeaByName(gridDesigner.IdeaName);
            List<ColumnInIdea> columns = ideaService.GetColumnsByIdea(idea.IdeaId);

            model.TableId = idea.IdeaId;
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
                colModel.Add("resizable", true);
                colModel.Add("sortable", true);
                //colModel1.Add("formatter", "date");
                colModel.Add("formatoptions", "{srcformat: 'ISO8601Long',  newformat: 'm/d/Y', defaultValue:null}");
                colModel.Add("editable", true);
                colModel.Add("edittype", "text");
                if (column.DataTypeId == Convert.ToInt32(Models.DataTypeId.Datetime))
                {
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12,  dataInit: function (element) {  $(element).datepicker({ dateFormat: 'mm/dd/yy' } ) } }");
                }
                else if (column.DataTypeId == Convert.ToInt32(Models.DataTypeId.Entity))
                {
                    string cellvalue = "";
                    Idea refIdea = ideaService.GetIdeaById(column.ReferedIdeaId);
                    refIdea.Columns = ideaService.GetRefColumnsByColumnId(column.ColumnId);
                    IdeaDetail rowDetail = ideaService.GetIdeaData(refIdea, "", "", 0, 1000, null);
                    if (rowDetail.Rows.Count > 0)
                    {
                        for (int j = 0; j < rowDetail.Rows.Count; j++)
                        {
                            string rowValue = "";
                            foreach (var value in rowDetail.Rows[j].Values)
                            {
                                rowValue += value + ",";
                            }
                            if (rowValue.Contains(','))
                            {
                                rowValue = rowValue.Remove(rowValue.LastIndexOf(","), 1);
                            }
                            rowValue = rowDetail.Rows[j].RowId + ":" + rowValue;
                            cellvalue += rowValue + ";";
                        }
                        cellvalue = cellvalue.Remove(cellvalue.LastIndexOf(";"), 1);
                    }
                    colModel["edittype"] = "select";
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12, value : '" + cellvalue + "' }");
                }
                else
                {
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12}");
                }
                model.ColModels.Add(colModel);
            }

            model.ViewRecords = true;

            model.SourceType = Models.GridSourceType.json;
            model.Url = "/App/GridOperation";
            model.RowNum = 20;

            model.RowList.Add(5);
            model.RowList.Add(10);
            model.RowList.Add(20);

            model.Caption = "Grid List";
            model.SortName = "Date";
            model.SortOrder = Models.GridSortOrder.desc;

            model.EditUrl = "/App/GridOperation";

            //model.Theme = "trontastic";
            model.Theme = gridDesigner.Theme;

            return model;
        }

        private ChartModel InitializeChartModel(ChartDesigner chartDesigner)
        {
            if (!string.IsNullOrEmpty(chartDesigner.IdeaName) && chartDesigner.IdeaId > 0)
            {
                IIdeaService ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
                Idea idea = ideaService.GetIdeaByName(chartDesigner.IdeaName);
                List<ColumnInIdea> columns = ideaService.GetColumnsByIdea(idea.IdeaId);
            }
            if (chartDesigner.AnalyzerId <= 0) return null;
            ChartModel chartModel = ChartModel.GetChartSourceByAnalyzerId(chartDesigner.AnalyzerId);
            chartModel.Id = chartDesigner.Id;
            chartModel.ChartTitle = chartDesigner.ChartTitle;
            chartModel.ChartType = chartDesigner.ChartType;

            return chartModel;
        }

        private LeftMenuItem[] InitializeLeftMenuModel(LeftMenuItemDesigner[] leftMenuItemDesigners)
        {
            if (Request.RequestContext.HttpContext.Session == null) return null;

            int userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);
            Dictionary<string, string> userAppList = GetUserAppList(userId);

            foreach (var userApp in userAppList)
            {
                foreach (var leftMenuItemDesigner in leftMenuItemDesigners)
                {
                    if (userApp.Key == leftMenuItemDesigner.FileName) leftMenuItemDesigner.FileName = userApp.Value;
                }
            }

            var translator = new TranslatorBetweenDesignerAndModel();
            var leftMenu = translator.Map<LeftMenuItem[]>(leftMenuItemDesigners);

            return leftMenu;
        }

        private TabsModel InitializeTabsModel(TabsDesigner tabsDesigner)
        {
            var translator = new TranslatorBetweenDesignerAndModel();
            var tabsModel = translator.Map<TabsModel>(tabsDesigner);
            return tabsModel;
        }

        #endregion

        #region ----Component Render----

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

        public string GetComponentsFromPageNodeInXml(Page page)
        {
            if (page == null) return null;

            StringBuilder mainBody = new StringBuilder();

            //Dictionary<string, object> dictionary = page.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(page, null));

            if (page.TabsDesigner != null)
            {
                TabsModel tabs = InitializeTabsModel(page.TabsDesigner);
                string tabsString = RenderRazorViewToString("Punch-Card Machine", tabs);
                mainBody.Append("<br/>");
                mainBody.Append(tabsString);
            }

            if (page.GridList != null && page.GridList.Count != 0)
            {
                foreach (var grid in page.GridList)
                {
                    GridModel gridModel = InitializeGridModel(grid);
                    string gridString = RenderRazorViewToString("Grid", gridModel);
                    mainBody.Append("<br/>");
                    mainBody.Append(gridString);
                }
            }

            if (page.ChartList != null && page.ChartList.Count != 0)
            {
                foreach (var chart in page.ChartList)
                {
                    ChartModel chartModel = InitializeChartModel(chart);
                    string chartString = RenderRazorViewToString("Chart", chartModel);
                    mainBody.Append("<br/>");
                    mainBody.Append(chartString);
                }
            }

            return mainBody.ToString();
        }

        public string RenderAllComponentsIntoTemplate(string templateName = @"Template1.cshtml",
                                                      string path = @"/Views/App/", params string[] componentString)
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
            //int lastIndex = result.LastIndexOf("$",StringComparison.Ordinal);
            //result = result.Remove(startIndex, lastIndex - startIndex);

            return result;
        }

        #endregion

        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            Dictionary<string, string> userAppList = GetUserAppList(userId);

            List<AppModel> modelList = userAppList.Select(userApp => new AppModel() { AppName = userApp.Key, AppFileNameWithExtension = userApp.Value }).ToList();

            return View(modelList);
        }

        public ActionResult Details(int id)
        {
            //AppModel app = null;
            ////if (id == 3)
            ////{
            //    app = new AppModel()
            //    {
            //        Id = 1,
            //        AppName = "Lean Gymnasium Club Management System",
            //        IdeaList = new List<IdeaModel>
            //        {
            //            new IdeaModel{IdeaId = 1,IdeaName = "Customer"},
            //            new IdeaModel{IdeaId = 2,IdeaName = "GymCard"},
            //            new IdeaModel{IdeaId = 3,IdeaName = "ConsumptionRecord"}
            //        },
            //        UserList = new List<AppUserModel>
            //        {
            //            new AppUserModel{UserName = "admin",Password = "000000"}  
            //        }
            //    };
            ////}
            //return View(app);

            int userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            string userAppName = string.Empty;
            //List<string> userAppList = GetUserAppList(userId, true);
            List<string> userAppList = new List<string>();
            for (int i = 1; i <= userAppList.Count(); i++)
            {
                if (id == i)
                {
                    userAppName = userAppList[i - 1] + ".xml";
                    break;
                }
            }

            //userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            string name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            TranslatorBetweenDesignerAndModel translator = new TranslatorBetweenDesignerAndModel();

            LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
            LeftMenuItem[] leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            string leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            StringBuilder leftMenuBuilder = new StringBuilder(leftMenuString);

            string csboardString = string.Empty;
            csboardString = RenderRazorViewToString("ClassScheduleBoard", null);

            StringBuilder mainBody = new StringBuilder();
            mainBody.Append("<br/>");
            //mainBody.Append(csboardString);
            //mainBody.Append("<br/>");   

            MvcHtmlString mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            string template = "/Views/App/Template1.cshtml";
            string path = "/Views/App/";
            string htmlName = "Input.html";
            Dictionary<string, string> dic = new Dictionary<string, string>();
            string message = string.Empty;

            dic.Add("title", "FPX Gym App");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");
            dic.Add("Schedule", csboardString);

            string result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            HtmlString htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        public ActionResult Gym1(string ideaName)
        {
            string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);
            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);
            Page page = userApp.Pages[0];

            TranslatorBetweenDesignerAndModel translator = new TranslatorBetweenDesignerAndModel();

            LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
            LeftMenuItem selectedMenuItem = new LeftMenuItem();
            LeftMenuItem[] leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            string leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            StringBuilder leftMenuBuilder = new StringBuilder(leftMenuString);

            string gridString = string.Empty;
            string tabsString = string.Empty;
            string csboardString = string.Empty;
            csboardString = RenderRazorViewToString("ClassScheduleBoard", null);

            foreach (LeftMenuItem leftMenuItem in leftMenuModel)
            {
                if (leftMenuItem.Value == ideaName)
                {
                    selectedMenuItem = leftMenuItem;
                    break;
                }
            }
            if (selectedMenuItem.Id != -1)
            {
                //GridModel grid = InitializeGridModel(ideaName);
                GridModel grid = InitializeGridModel(page.GridList[0]);
                gridString = RenderRazorViewToString("Grid", grid);
                if (selectedMenuItem.Id == 3)
                {
                    TabsModel tabs = new TabsModel()
                    {
                        TabsGroups = new List<Models.TabsGroup>()
                        {
                            new Models.TabsGroup(){ Header = "Start Tab"},
                            new Models.TabsGroup(){ Header = "End Tab" }
                        }
                    };
                    tabsString = RenderRazorViewToString("Punch-Card Machine", tabs);
                }
            }
            else
            { }

            StringBuilder mainBody = new StringBuilder();

            mainBody.Append(tabsString);
            mainBody.Append("<br/>");
            mainBody.Append(gridString);
            mainBody.Append("<br/>");

            string result = RenderAllComponentsIntoTemplate(@"Template1.cshtml", @"/Views/App/", "FPX Gym Page", leftMenuBuilder.ToString(), mainBody.ToString());
            ViewBag.Result = result;
            return View("PageT");
        }

        public ActionResult Nova(int pageIndex, string userAppName)
        {
            //const int pageIndex = 3;
            //const string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            if (string.IsNullOrEmpty(userAppName) || pageIndex < 1) return null;

            string userAppFullPath = Server.MapPath("~/Xml/" + userAppName);
            UserApp userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            string pageTitle = userApp.Description;

            string templateName = @"Template1.cshtml";
            if (!string.IsNullOrEmpty(userApp.Template))
            {
                templateName = userApp.Template + @".cshtml";
            }

            const string path = @"/Views/App/";

            string leftMenuString = string.Empty;
            if (userApp.LeftMenu != null)
            {
                LeftMenuItemDesigner[] leftMenu = userApp.LeftMenu;
                var leftMenuModel = InitializeLeftMenuModel(leftMenu);
                leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            }

            string mainBody = GetComponentsFromPageNodeInXml(userApp.Pages[pageIndex - 1]);

            StringBuilder partialViewNoModel = new StringBuilder();
            //Need to process some partial views which didn't need models
            if (userApp.PartialViews != null && userApp.Pages.Count() != 0)
            {
                foreach (var partialView in userApp.PartialViews)
                {
                    partialViewNoModel.Append(RenderRazorViewToString(partialView, null));
                }
            }

            //if (userApp.Pages != null && userApp.Pages.Count() != 0 && userApp.Pages[pageIndex].Bookings != null && userApp.Pages[pageIndex].Bookings.Count != 0)
            if(userApp.Pages[pageIndex-1].Bookings.Count !=0)
            {
                foreach (var booking in userApp.Pages[pageIndex-1].Bookings)
                {
                    partialViewNoModel.Append(RenderRazorViewToString("SeatBookingMap", null));
                }
            }

            string result = RenderAllComponentsIntoTemplate(templateName, path, pageTitle, leftMenuString, mainBody, partialViewNoModel.ToString());

            ViewBag.Result = result;
            return View("PageT");
        }

        [HttpPost]
        public ActionResult Details(AppModel app)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<AppUserModel> userList = new List<AppUserModel>
                    {
                        new AppUserModel{UserName = "admin",Password = "000000"}  
                    };
                    foreach (AppUserModel user in userList)
                    {
                        if (user.UserName == app.User && user.Password == app.Psd)
                        {
                            Session["AppUser"] = app.User;
                            ViewBag.AppUser = app.User;
                        }
                    }
                }

                if (string.IsNullOrEmpty(ViewBag.AppUser))
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");

                return RedirectToAction("Details");
            }
            catch
            {
                return View();
            }
        }

        /// <summary>
        /// Gets the idea grid columns.
        /// </summary>
        /// <param name="cardId">The card id.</param>
        /// <returns></returns>
        public string InsertConsumptionRecord(string cardId)
        {
            IIdeaService _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            IRowService _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            int rowIndex = 0;
            string cardRefId = string.Empty;
            IdeaDetail ideaDetail = _ideaService.GetIdeaData(2, "", "", 0, 1000, null);
            for (int i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (int i = 0; i < ideaDetail.Rows.Count; i++)
            {
                if (ideaDetail.Rows[i].Values[rowIndex].ToString() == cardId)
                {
                    cardRefId = ideaDetail.Rows[i].RowId.ToString();
                }
            }
            NameValueCollection collection = new NameValueCollection();
            collection.Add("StartTime", DateTime.Now.ToString());
            collection.Add("EndTime", DateTime.Now.ToString());
            collection.Add("CardId", cardRefId);
            collection.Add("ConsumptionAmount", "20");

            int ideaId = 3;
            int userId = 0;

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
                return string.Empty;
            else
                userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            Idea idea = _ideaService.GetIdeaById(ideaId);
            Row row = new Row()
            {
                IdeaId = ideaId,
                UserId = userId,
                Columns = idea.Columns
            };
            foreach (var column in row.Columns)
            {
                row.Values.Add(collection[column.ColumnName]);
            }
            string result = _rowService.AddRow(row).ToString();
            return result;
        }

        public string UpdateConsumptionRecord(string cardId)
        {
            IIdeaService _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            IRowService _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());

            string result = "";
            int ideaId = 3;
            int userId = 0;
            int rowIndex = 0;
            string cardRefId = string.Empty;

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
                return string.Empty;
            else
                userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            Idea idea = _ideaService.GetIdeaById(ideaId);
            IdeaDetail ideaDetail = _ideaService.GetIdeaData(2, "", "", 0, 1000, null);
            for (int i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (int i = 0; i < ideaDetail.Rows.Count; i++)
            {
                if (ideaDetail.Rows[i].Values[rowIndex].ToString() == cardId)
                {
                    cardRefId = ideaDetail.Rows[i].RowId.ToString();
                }
            }

            ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", 0, 1000, null);
            for (int i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            List<Row> row = ideaDetail.Rows.OrderByDescending(m => m.RowId).Where(m => m.Values[rowIndex].ToString() == cardRefId).Take(1).ToList();
            row[0].Columns = idea.Columns;
            row[0].Values[1] = DateTime.Now.ToString();
            row[0].UserId = userId;

            result = _rowService.UpdateRow(row[0]).ToString();

            return result;
        }

        public JsonResult GetAllCardId()
        {
            IIdeaService _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            IRowService _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            int ideaId = 2;
            int rowIndex = 0;
            string[] cardIdJsonData;

            IdeaDetail ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", 0, 1000, null);

            cardIdJsonData = new string[ideaDetail.Rows.Count];

            for (int i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (int i = 0; i < ideaDetail.Rows.Count; i++)
            {
                cardIdJsonData[i] = ideaDetail.Rows[i].Values[rowIndex].ToString();
            }
            return Json(cardIdJsonData, JsonRequestBehavior.AllowGet);

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
            if (Session["UserId"] == null) Session["UserId"] = 1;
            collection.Add("userId", Session["UserId"].ToString());
            result = methodOper.Invoke(service, flags, Type.DefaultBinder, new object[] { collection }, null) as string;
            return Content(result);
        }
    }

    public class TranslatorBetweenDesignerAndModel : AutoMapperWrapper
    {
        public TranslatorBetweenDesignerAndModel()
        {
            Initialize(cfg =>
            {
                cfg.CreateMap<AccordionDesigner, AccordionModel>();
                cfg.CreateMap<ButtonDesigner, ButtonModel>();
                cfg.CreateMap<ChartDesigner, ChartModel>();
                cfg.CreateMap<DatePickerDesigner, DatePickerModel>();
                cfg.CreateMap<DateRangeDesigner, DateRangeModel>();
                cfg.CreateMap<DialogDesigner, DialogModel>();

                cfg.CreateMap<GridDesigner, GridModel>();
                cfg.CreateMap<FPXAppDesign.DesignerClass.Component.JsonReader, Models.JsonReader>();

                cfg.CreateMap<InputDesigner, InputModel>();
                cfg.CreateMap<LabelDesigner, LabelModel>();
                cfg.CreateMap<LocalizedDisplayNameDesigner, LocalizedDisplayName>();
                cfg.CreateMap<MenuDesigner, MenuModel>();
                cfg.CreateMap<ProgressbarDesigner, ProgressbarModel>();
                cfg.CreateMap<RowDesigner, RowModel>();
                cfg.CreateMap<TabsDesigner, TabsModel>();
                cfg.CreateMap<FPXAppDesign.DesignerClass.Component.TabsGroup, Models.TabsGroup>();
                cfg.CreateMap<TextDesigner, TextModel>();
                cfg.CreateMap<LeftMenuItemDesigner, LeftMenuItem>();
                cfg.CreateMap<BookingDesigner, Booking>();
            });
            var mapper = Configuration.CreateMap<ColumnModelDesigner, ColModel>();
            //mapper.ForMember(d => d.ColumnModels, opt => opt.MapFrom(s => s.ColumnModels));
            mapper.ConstructUsing(s => DictionaryToListKeyValuePair(s));
        }

        public static ColModel DictionaryToListKeyValuePair(ColumnModelDesigner columnModel)
        {
            if (columnModel == null) return null;
            ColModel colModel = new ColModel();

            foreach (var keyValuePair in columnModel)
            {
                if (keyValuePair != null)
                {
                    colModel.Add(keyValuePair.Key, keyValuePair.Value);
                }
            }
            return colModel;
        }

        public static List<ColModel> ListDictionaryToListListKeyValuePair(List<ColumnModelDesigner> keyValuePairListList)
        {
            if (keyValuePairListList == null) return null;
            List<ColModel> dictionaryList = new List<ColModel>();
            foreach (var keyValuePairList in keyValuePairListList)
            {
                if (keyValuePairList != null)
                {
                    foreach (var keyValuePair in keyValuePairList)
                    {
                        if (keyValuePair != null)
                        {
                            var dic = new ColModel() { { keyValuePair.Key, keyValuePair.Value } };
                            dictionaryList.Add(dic);
                        }
                    }
                }
            }
            return dictionaryList;
        }
    }

    public class HtmlGenerator
    {
        public static string Create(string template, string path, string htmlName, Dictionary<string, string> dic, ref string message)
        {
            string templatePath = System.Web.HttpContext.Current.Server.MapPath(template);
            string htmlPath = System.Web.HttpContext.Current.Server.MapPath(path);
            string htmlNamePath = Path.Combine(htmlPath, htmlName);
            Encoding encode = Encoding.UTF8;
            StringBuilder html = new StringBuilder();

            html.Append(File.ReadAllText(templatePath, encode));

            foreach (System.Collections.Generic.KeyValuePair<string, string> d in dic)
            {
                html.Replace(string.Format("${0}$", d.Key), d.Value);
            }

            if (!Directory.Exists(htmlPath)) Directory.CreateDirectory(htmlPath);

            File.WriteAllText(htmlNamePath, html.ToString(), encode);

            return html.ToString();
        }
    }
}
