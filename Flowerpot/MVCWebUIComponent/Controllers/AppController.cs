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

                var userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);
                var userAppList = GetUserAppList(userId);

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

                var storeFilePath = Server.MapPath("~/Xml/" + SetFileName(fileData.FileName, userId));
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
            var tmp = fileName.Remove(fileName.Length - 4);
            var time = DateTime.Now.ToShortDateString().Replace('/', '-');
            var newName = String.Format("{0}_{1}_{2}.xml", id.ToString(), tmp, time);
            return newName;
        }

        public Dictionary<string, string> GetUserAppList(int userId = 1, bool isFileNameWithExtension = false,
                                           string virtualPath = @"~/Xml", string fileName = @"*",
                                           string fileExtension = @".Xml", char separator = '_')
        {
            var physicalPath = Server.MapPath(virtualPath);
            if (!Directory.Exists(physicalPath)) return null;

            var fileNamesIncludingPath = Directory.GetFiles(physicalPath, fileName + fileExtension, SearchOption.AllDirectories).ToList();
            var userAppList = new Dictionary<string, string>();

            foreach (var name in fileNamesIncludingPath)
            {
                var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(name);
                var fileNameWithExtension = Path.GetFileName(name);
                if (fileNameWithoutExtension == null) continue;

                if (isFileNameWithExtension)
                    userAppList.Add(fileNameWithoutExtension, name);
                else
                {
                    var tempArray = fileNameWithoutExtension.Split(separator);
                    if (tempArray[0].Equals(userId.ToString())) userAppList.Add(tempArray[1], fileNameWithExtension);
                }
            }
            return userAppList;
        }
        #endregion

        #region ----Component Initialization----

        private GridModel InitializeGridModel(GridDesigner gridDesigner)
        {
            var model = new GridModel();

            var ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var idea = ideaService.GetIdeaByName(gridDesigner.IdeaName);
            var columns = ideaService.GetColumnsByIdea(idea.IdeaId);

            model.TableId = idea.IdeaId;
            model.MyJsonReader.Root = "Rows";
            model.MyJsonReader.Page = "Page";
            model.MyJsonReader.Total = "TotalPages";
            model.MyJsonReader.Id = "Id";
            model.MyJsonReader.Cell = "Cell";
            model.MyJsonReader.Records = "TotalRecords";

            foreach (var column in columns)
            {
                model.ColNames.Add(column.ColumnName);

                var colModel = new ColModel
                {
                    {"name", column.ColumnName},
                    {"index", column.ColumnName},
                    {"search", true},
                    {"resizable", true},
                    {"sortable", true},
                    {"formatoptions", "{srcformat: 'ISO8601Long',  newformat: 'm/d/Y', defaultValue:null}"},
                    {"editable", true},
                    {"edittype", "text"}
                };
                //colModel1.Add("formatter", "date");
                if (column.DataTypeId == Convert.ToInt32(Models.DataTypeId.Datetime))
                {
                    colModel.Add("editoptions", "{ size: 12,  maxlengh: 12,  dataInit: function (element) {  $(element).datepicker({ dateFormat: 'mm/dd/yy' } ) } }");
                }
                else if (column.DataTypeId == Convert.ToInt32(Models.DataTypeId.Entity))
                {
                    var cellvalue = "";
                    var refIdea = ideaService.GetIdeaById(column.ReferedIdeaId);
                    refIdea.Columns = ideaService.GetRefColumnsByColumnId(column.ColumnId);
                    var rowDetail = ideaService.GetIdeaData(refIdea, "", "", 0, 1000, null);
                    if (rowDetail.Rows.Count > 0)
                    {
                        foreach (var t in rowDetail.Rows)
                        {
                            var rowValue = t.Values.Aggregate("", (current, value) => current + (value + ","));
                            if (rowValue.Contains(','))
                            {
                                rowValue = rowValue.Remove(rowValue.LastIndexOf(",", StringComparison.Ordinal), 1);
                            }
                            rowValue = t.RowId + ":" + rowValue;
                            cellvalue += rowValue + ";";
                        }
                        cellvalue = cellvalue.Remove(cellvalue.LastIndexOf(";", StringComparison.Ordinal), 1);
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
                var ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
                var idea = ideaService.GetIdeaByName(chartDesigner.IdeaName);
                var columns = ideaService.GetColumnsByIdea(idea.IdeaId);
            }
            if (chartDesigner.AnalyzerId <= 0) return null;
            var chartModel = ChartModel.GetChartSourceByAnalyzerId(chartDesigner.AnalyzerId);
            chartModel.Id = chartDesigner.Id;
            chartModel.ChartTitle = chartDesigner.ChartTitle;
            chartModel.ChartType = chartDesigner.ChartType;

            return chartModel;
        }

        private LeftMenuItem[] InitializeLeftMenuModel(LeftMenuItemDesigner[] leftMenuItemDesigners)
        {
            if (Request.RequestContext.HttpContext.Session == null) return null;

            var userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);
            var userAppList = GetUserAppList(userId);

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

            var mainBody = new StringBuilder();

            //Dictionary<string, object> dictionary = page.GetType().GetProperties().ToDictionary(p => p.Name, p => p.GetValue(page, null));

            if (page.TabsDesigner != null)
            {
                var tabs = InitializeTabsModel(page.TabsDesigner);
                var tabsString = RenderRazorViewToString("Punch-Card Machine", tabs);
                mainBody.Append("<br/>");
                mainBody.Append(tabsString);
            }

            if (page.GridList != null && page.GridList.Count != 0)
            {
                foreach (var grid in page.GridList)
                {
                    var gridModel = InitializeGridModel(grid);
                    var gridString = RenderRazorViewToString("Grid", gridModel);
                    mainBody.Append("<br/>");
                    mainBody.Append(gridString);
                }
            }

            if (page.ChartList != null && page.ChartList.Count != 0)
            {
                foreach (var chart in page.ChartList)
                {
                    var chartModel = InitializeChartModel(chart);
                    var chartString = RenderRazorViewToString("Chart", chartModel);
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
            var message = string.Empty;

            for (var i = 0; i < componentString.Length; i++)
            {
                dic.Add("Component" + i, componentString[i]);
            }

            var result = HtmlGenerator.Create(path + templateName, path, htmlPageName, dic, ref message);

            //Remove redundant $Component$ in template
            //int startIndex = result.IndexOf("$", StringComparison.Ordinal);
            //int lastIndex = result.LastIndexOf("$",StringComparison.Ordinal);
            //result = result.Remove(startIndex, lastIndex - startIndex);

            return result;
        }

        #endregion

        public ActionResult Index()
        {
            var userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var userAppList = GetUserAppList(userId);

            var modelList = userAppList.Select(userApp => new AppModel() { AppName = userApp.Key, AppFileNameWithExtension = userApp.Value }).ToList();

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

            var userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
            {
                return RedirectToAction("LogOn", "Account");
            }

            var userAppName = string.Empty;
            //List<string> userAppList = GetUserAppList(userId, true);
            var userAppList = new List<string>();
            for (var i = 1; i <= userAppList.Count(); i++)
            {
                if (id == i)
                {
                    userAppName = userAppList[i - 1] + ".xml";
                    break;
                }
            }

            //userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            var name = System.Configuration.ConfigurationManager.AppSettings["HostUrl"];

            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);

            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            var translator = new TranslatorBetweenDesignerAndModel();

            var leftMenu = userApp.LeftMenu;
            var leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            var leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            var leftMenuBuilder = new StringBuilder(leftMenuString);

            var csboardString = string.Empty;
            csboardString = RenderRazorViewToString("ClassScheduleBoard", null);

            var mainBody = new StringBuilder();
            mainBody.Append("<br/>");
            //mainBody.Append(csboardString);
            //mainBody.Append("<br/>");   

            var mvcHtmlString = new MvcHtmlString(mainBody.ToString());
            ViewBag.MvcHtmlString = mvcHtmlString;
            ViewBag.razorString = mainBody.ToString();

            var template = "/Views/App/Template1.cshtml";
            var path = "/Views/App/";
            var htmlName = "Input.html";
            var dic = new Dictionary<string, string>();
            var message = string.Empty;

            dic.Add("title", "FPX Gym App");
            dic.Add("Component_LeftMenu", leftMenuBuilder.ToString());
            dic.Add("Component1", mainBody.ToString());
            dic.Add("Component2", "");
            dic.Add("Schedule", csboardString);

            var result = HtmlGenerator.Create(template, path, htmlName, dic, ref message);
            ViewBag.Result = result;
            var htmlString = new HtmlString(result);
            ViewBag.HtmlSting = htmlString;
            return View("PageT");
        }

        public ActionResult Gym1(string ideaName)
        {
            var userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);
            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);
            var page = userApp.Pages[0];

            var translator = new TranslatorBetweenDesignerAndModel();

            var leftMenu = userApp.LeftMenu;
            var selectedMenuItem = new LeftMenuItem();
            var leftMenuModel = translator.Map<LeftMenuItem[]>(leftMenu);
            var leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            var leftMenuBuilder = new StringBuilder(leftMenuString);

            var gridString = string.Empty;
            var tabsString = string.Empty;
            var csboardString = string.Empty;
            csboardString = RenderRazorViewToString("ClassScheduleBoard", null);

            foreach (var leftMenuItem in leftMenuModel)
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
                var grid = InitializeGridModel(page.GridList[0]);
                gridString = RenderRazorViewToString("Grid", grid);
                if (selectedMenuItem.Id == 3)
                {
                    var tabs = new TabsModel()
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

            var mainBody = new StringBuilder();

            mainBody.Append(tabsString);
            mainBody.Append("<br/>");
            mainBody.Append(gridString);
            mainBody.Append("<br/>");

            var result = RenderAllComponentsIntoTemplate(@"Template1.cshtml", @"/Views/App/", "FPX Gym Page", leftMenuBuilder.ToString(), mainBody.ToString());
            ViewBag.Result = result;
            return View("PageT");
        }

        public ActionResult Nova(int pageIndex, string userAppName)
        {
            //const int pageIndex = 3;
            //const string userAppName = @"1_Lean Gymnasium Club Management System_20131022.xml";
            if (string.IsNullOrEmpty(userAppName) || pageIndex < 1) return null;

            var userAppFullPath = Server.MapPath("~/Xml/" + userAppName);
            var userApp = FPXAppManager.DeserializeUserApp(userAppFullPath);

            var pageTitle = userApp.Description;

            var templateName = @"Template1.cshtml";
            if (!string.IsNullOrEmpty(userApp.Template))
            {
                templateName = userApp.Template + @".cshtml";
            }

            const string path = @"/Views/App/";

            var leftMenuString = string.Empty;
            if (userApp.LeftMenu != null)
            {
                var leftMenu = userApp.LeftMenu;
                var leftMenuModel = InitializeLeftMenuModel(leftMenu);
                leftMenuString = RenderRazorViewToString("LeftMenu", leftMenuModel);
            }

            var mainBody = GetComponentsFromPageNodeInXml(userApp.Pages[pageIndex - 1]);

            var partialViewNoModel = new StringBuilder();
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

            var result = RenderAllComponentsIntoTemplate(templateName, path, pageTitle, leftMenuString, mainBody, partialViewNoModel.ToString());

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
                    var userList = new List<AppUserModel>
                    {
                        new AppUserModel{UserName = "admin",Password = "000000"}  
                    };
                    foreach (var user in userList)
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
            var _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            var rowIndex = 0;
            var cardRefId = string.Empty;
            var ideaDetail = _ideaService.GetIdeaData(2, "", "", 0, 1000, null);
            for (var i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (var i = 0; i < ideaDetail.Rows.Count; i++)
            {
                if (ideaDetail.Rows[i].Values[rowIndex].ToString() == cardId)
                {
                    cardRefId = ideaDetail.Rows[i].RowId.ToString();
                }
            }
            var collection = new NameValueCollection();
            collection.Add("StartTime", DateTime.Now.ToString());
            collection.Add("EndTime", DateTime.Now.ToString());
            collection.Add("CardId", cardRefId);
            collection.Add("ConsumptionAmount", "20");

            var ideaId = 3;
            var userId = 0;

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
                return string.Empty;
            else
                userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            var idea = _ideaService.GetIdeaById(ideaId);
            var row = new Row()
            {
                IdeaId = ideaId,
                UserId = userId,
                Columns = idea.Columns
            };
            foreach (var column in row.Columns)
            {
                row.Values.Add(collection[column.ColumnName]);
            }
            var result = _rowService.AddRow(row).ToString();
            return result;
        }

        public string UpdateConsumptionRecord(string cardId)
        {
            var _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());

            var result = "";
            var ideaId = 3;
            var userId = 0;
            var rowIndex = 0;
            var cardRefId = string.Empty;

            if (Request.RequestContext.HttpContext.Session["UserId"] == null)
                return string.Empty;
            else
                userId = Convert.ToInt32(Request.RequestContext.HttpContext.Session["UserId"]);

            var idea = _ideaService.GetIdeaById(ideaId);
            var ideaDetail = _ideaService.GetIdeaData(2, "", "", 0, 1000, null);
            for (var i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (var i = 0; i < ideaDetail.Rows.Count; i++)
            {
                if (ideaDetail.Rows[i].Values[rowIndex].ToString() == cardId)
                {
                    cardRefId = ideaDetail.Rows[i].RowId.ToString();
                }
            }

            ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", 0, 1000, null);
            for (var i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            var row = ideaDetail.Rows.OrderByDescending(m => m.RowId).Where(m => m.Values[rowIndex].ToString() == cardRefId).Take(1).ToList();
            row[0].Columns = idea.Columns;
            row[0].Values[1] = DateTime.Now.ToString();
            row[0].UserId = userId;

            result = _rowService.UpdateRow(row[0]).ToString();

            return result;
        }

        public JsonResult GetAllCardId()
        {
            var _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            var _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            var ideaId = 2;
            var rowIndex = 0;
            string[] cardIdJsonData;

            var ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", 0, 1000, null);

            cardIdJsonData = new string[ideaDetail.Rows.Count];

            for (var i = 0; i < ideaDetail.Columns.Count; i++)
            {
                if (ideaDetail.Columns[i].ColumnName == "CardId")
                {
                    rowIndex = i;
                    break;
                }
            }
            for (var i = 0; i < ideaDetail.Rows.Count; i++)
            {
                cardIdJsonData[i] = ideaDetail.Rows[i].Values[rowIndex].ToString();
            }
            return Json(cardIdJsonData, JsonRequestBehavior.AllowGet);

        }

        public ContentResult GridOperation(string oper)
        {
            var result = "";
            oper = oper.Substring(0, 1).ToUpper() + oper.Substring(1, oper.Length - 1);
            var type = Type.GetType("MVCWebUIComponent.Services.GridService");
            var service = Activator.CreateInstance(type);
            var methodOper = type.GetMethod(oper);
            var flags = BindingFlags.Public | BindingFlags.Instance;
            var collection = new NameValueCollection();
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
            var colModel = new ColModel();

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
            var dictionaryList = new List<ColModel>();
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
            var templatePath = System.Web.HttpContext.Current.Server.MapPath(template);
            var htmlPath = System.Web.HttpContext.Current.Server.MapPath(path);
            var htmlNamePath = Path.Combine(htmlPath, htmlName);
            var encode = Encoding.UTF8;
            var html = new StringBuilder();

            html.Append(File.ReadAllText(templatePath, encode));

            foreach (var d in dic)
            {
                html.Replace(string.Format("${0}$", d.Key), d.Value);
            }

            if (!Directory.Exists(htmlPath)) Directory.CreateDirectory(htmlPath);

            File.WriteAllText(htmlNamePath, html.ToString(), encode);

            return html.ToString();
        }
    }
}
