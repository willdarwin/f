using System;
using System.Collections.Generic;
using System.Web.Mvc;
using AutoMapper;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.ServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using MVCWebUIComponent.Models;
using MVCWebUIComponent.Models.Translates;
using UtilityComponent.Filters;
using System.Text.RegularExpressions;
using System.Collections;
using System.Linq;

namespace MVCWebUIComponent.Controllers
{
    [CheckinLogin]
    public class AnalyzerController : BaseController
    {

        private IRowService RowService { get; set; }
        private IIdeaService IdeaService { get; set; }
        private IAnalyzerService AnalyzerService { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzerController"/> class.
        /// </summary>
        /// <param name="ideaService">The idea service.</param>
        /// <param name="rowService">The row service.</param>
        /// <param name="analyzerService">The analyzer service.</param>
        public AnalyzerController(IIdeaService ideaService, IRowService rowService, IAnalyzerService analyzerService)
        {
            IdeaService = ideaService;
            RowService = rowService;
            AnalyzerService = analyzerService;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzerController"/> class.
        /// </summary>
        public AnalyzerController()
        {
            IdeaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            RowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
            AnalyzerService = PolicyInjection.Wrap<IAnalyzerService>(Container.Resolve<IAnalyzerService>());
        }

        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Index()
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var analyzerList = AnalyzerService.GetAnalyzersByUserId(userId);
            var modelList = Mapper.Map<List<AnalyzerModel>>(analyzerList);
            return View(modelList);
        }

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult CreateAnalyzer(int id = 0)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            if (id > 0)
            {
                var createModel = new CreateAnalyzerModel();
                createModel.Analyzer = Mapper.Map<AnalyzerModel>(AnalyzerService.GetAnalyzerById(id));
                createModel.Analyzer.SelectList = Regex.Split(createModel.Analyzer.SelectQuery, @"\s+");
                createModel.Analyzer.JoinList = Regex.Split(createModel.Analyzer.JoinQuery, @"\s+");
                createModel.Analyzer.WhereList = Regex.Split(createModel.Analyzer.WhereQuery, @"\s+");
                createModel.IdeaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
                return View(createModel);
            }
            else
            {
                var createModel = new CreateAnalyzerModel();
                createModel.Analyzer.AnalyzerId = 0;
                createModel.Analyzer.AnalyzerName = "";
                createModel.IdeaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
                return View(createModel);
            }
        }

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <param name="createModel">The create model.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        [HttpPost]
        public ActionResult CreateAnalyzer(CreateAnalyzerModel createModel)
        {
            var userId = Convert.ToInt32(Session["UserId"]);
            var analyzerModel = new AnalyzerModel();
            analyzerModel = createModel.Analyzer;

            var analyzer = Mapper.Map<Analyzer>(analyzerModel);
            analyzer.UserId = userId;
            if (createModel.Analyzer.AnalyzerId != 0)
            {
                analyzer = AnalyzerService.UpdateAnalyzer(analyzer);
            }
            else
            {
                analyzer = AnalyzerService.CreateAnalyzer(analyzer);
            } return RedirectToAction("Index", "Analyzer");
        }

        /// <summary>
        /// Deletes the analyzer.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult DeleteAnalyzer(int id)
        {
            var result = AnalyzerService.DeleteAnalyzerById(id);
            if (!result)
            {
                ModelState.AddModelError("", "Can Not Delete");
            }
            return RedirectToAction("Index", "Analyzer");
        }

        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Informations(int dataid)
        {
            ViewBag.dataId = dataid;
            return View();
        }

        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GridData(string sidx, string sord, int dataid = 1, int page = 1, int rows = 10000)
        {
            page = Convert.ToInt32(Request["page"]);
            rows = Convert.ToInt32(Request["rows"]);
            sidx = Request["sidx"];
            sord = Request["sord"];
            var pageIndex = page - 1;
            var pageSize = rows;
            var filters = Request["filters"];

            var model = new AnalyzerDetailModel();
            model = Mapper.Map<AnalyzerDetailModel>(AnalyzerService.GetAnalyzerDataById(dataid,  sidx, sord, filters));

            var totalRecords = model.Rows.Count;
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            var dataRows = new ArrayList();
            model.Rows = model.Rows.AsEnumerable().Skip(pageIndex * pageSize).Take(pageSize).ToList();
            foreach (var item in model.Rows)
            {
                var cellvalue = new List<object>();
                for (var j = 0; j < model.Columns.Count; j++)
                {
                    cellvalue.Add(item.Values[j]);
                }
                object row = new
                {
                    id = "id",
                    cell = cellvalue
                };
                dataRows.Add(row);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = dataRows,
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GetAnalyzerGridColumns(int dataid)
        {
            var analyzer = new AnalyzerDetailModel
            {
                AnalyzerId = dataid,
                UserId = Convert.ToInt32(Session["UserId"])
            };
            analyzer = Mapper.Map<AnalyzerDetailModel>(AnalyzerService.GetAnalyzerDataById(dataid));
            var colNames = new ArrayList();
            IList<object> colModels = new List<object>();
            var widthPersent = 100 / analyzer.Columns.Count + "%";
            foreach (var item in analyzer.Columns)
            {
                object colModel;
                colModel = new
                {
                    columnId = item.ColumnId,
                    name = item.ColumnName,
                    index = item.ColumnName,
                    editable = true,
                    align = "center",
                    width = widthPersent
                };

                colModels.Add(colModel);
                colNames.Add(item.ColumnName);
            }
            var jsonData = new
            {
                AnalyzerName = analyzer.AnalyzerName,
                colNames = colNames,
                colModels = colModels,
                sidx = analyzer.Columns[0].ColumnName
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}
