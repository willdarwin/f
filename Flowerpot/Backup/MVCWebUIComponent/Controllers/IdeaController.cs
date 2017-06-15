using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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


namespace MVCWebUIComponent.Controllers
{
    [CheckinLogin]
    public class IdeaController : BaseController
    {

        private IRowService RowService { get; set; }
        private IIdeaService IdeaService { get; set; }

        public IdeaController(IIdeaService ideaService, IRowService rowService)
        {
            IdeaService = ideaService;
            RowService = rowService;
        }

        public IdeaController()
        {
            IdeaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            RowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
        }


        /// <summary>
        /// Indexes this instance.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Index()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            List<Idea> ideaList = IdeaService.GetIdeasByUser(userId);
            List<IdeaModel> modelList = Mapper.Map<List<IdeaModel>>(ideaList);
            return View(modelList);
        }

        /// <summary>
        /// Detailses the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Details(int id)
        {
            Idea idea = IdeaService.GetIdeaById(id);
            IdeaModel model = Mapper.Map<IdeaModel>(idea);
            return View(model);
        }

        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Create()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            IEnumerable<IdeaModel> ideaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
            SelectList slist = new SelectList(ideaList, "IdeaId", "IdeaName");
            ViewData["ideaList"] = slist;

            return View();
        }

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Create(IdeaModel model)
        {
            if (ModelState.IsValid)
            {
                for (int i = 0; i < model.Columns.Count; i++)
                {
                    string refColumnIdString = Request["Columns[" + i + "].ReferedColumnIds"];
                    if (!string.IsNullOrEmpty(refColumnIdString))
                    {
                        string[] refColumnId = refColumnIdString.Split(new Char[] { ',' });
                        for (int j = 0; j < refColumnId.Length; j++)
                        {
                            model.Columns[i].ReferedColumnIds[j] = refColumnId[j];
                        }
                    }
                }
                int userId = Convert.ToInt32(Session["UserId"]);
                model.UserId = userId;
                Idea idea = Mapper.Map<Idea>(model);
                bool validationResult = IdeaService.ValidateIdeaNameDuplication(idea.IdeaName, userId);

                if (!validationResult)
                {
                    IdeaService.CreateIdea(idea);
                }
                else
                {
                    ModelState.AddModelError("", "The Idea Name has not to be Duplicated.");
                }
            }
            else
            {
                ModelState.AddModelError("", "The Idea Name or Column Name provided is incorrect.");
            }

            return RedirectToAction("Index", "Idea");
        }

        /// <summary>
        /// Edits the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Edit(int id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            Idea idea = IdeaService.GetIdeaById(id);
            IdeaModel model = Mapper.Map<IdeaModel>(idea);

            IEnumerable<IdeaModel> ideaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
            SelectList slist = new SelectList(ideaList, "IdeaId", "IdeaName");
            ViewData["ideaList"] = slist;

            return View(model);
        }

        /// <summary>
        /// Edits the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Edit(int id, IdeaModel model)
        {
            if (ModelState.IsValid)
            {
                int userId = Convert.ToInt32(Session["UserId"]);
                model.UserId = userId;
                Idea idea = Mapper.Map<Idea>(model);
                bool result = IdeaService.UpdateIdea(idea);
                if (!result)
                {
                    ModelState.AddModelError("", "Can Not Update");
                }
            }
            else
            {
                ModelState.AddModelError("", "The Idea Name or Column Name provided is incorrect.");
            }

            return RedirectToAction("Edit", new { Id = id });
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            bool result = IdeaService.DeleteIdeaById(id);
            if (!result)
            {
                ModelState.AddModelError("", "Can Not Delete");
            }
            return RedirectToAction("Index", "Idea");
        }

        /// <summary>
        /// Edits the column.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult EditColumn(int id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            ColumnInIdea column = IdeaService.GetColumnById(id);
            ColumnInIdeaModel model = Mapper.Map<ColumnInIdeaModel>(column);

            IEnumerable<IdeaModel> ideaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
            SelectList slist = new SelectList(ideaList, "IdeaId", "IdeaName");
            ViewData["ideaList"] = slist;

            return View(model);
        }

        /// <summary>
        /// Edits the column.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult EditColumn(ColumnInIdeaModel model)
        {
            if (ModelState.IsValid)
            {
                ColumnInIdea column = Mapper.Map<ColumnInIdea>(model);
                IdeaService.UpdateColumn(column);
                string refColumnIdString = Request["ReferedColumnId"];
                if (model.MyDataTypeId.Equals("Entity"))
                {
                    if (!string.IsNullOrEmpty(refColumnIdString))
                    {
                        string[] refColumnId = refColumnIdString.Split(new Char[] { ',' });
                        for (int j = 0; j < refColumnId.Length; j++)
                        {
                            model.ReferedColumnIds[j] = refColumnId[j];
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("EditColumn", "The Idea Name or Column Name provided is incorrect.");
            }

            return RedirectToAction("Edit", new { Id = model.IdeaId });
        }

        //
        // GET: /Column/Delete/5
        public ActionResult DeleteColumn(int id)
        {
            ColumnInIdea column = IdeaService.GetColumnById(id);
            int ideaId = column.IdeaId;

            bool result = IdeaService.DeleteColumnById(id);
            if (!result)
            {
                ModelState.AddModelError("", "Can Not Delete");
            }
            return RedirectToAction("Edit", new { Id = ideaId });
        }

        /// <summary>
        /// Get all ideas of current user for idea reference.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GetIdeaList()
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            IEnumerable<IdeaModel> ideaList = Mapper.Map<List<IdeaModel>>(IdeaService.GetIdeasByUser(userId));
            return Json(ideaList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Get all columns of referenced idea.
        /// </summary>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GetColumnsList(int ideaId)
        {
            IEnumerable<ColumnInIdeaModel> columnList = Mapper.Map<List<ColumnInIdeaModel>>(IdeaService.GetColumnsByIdea(ideaId));
            return Json(columnList, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Informations
        /// </summary>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Informations(int ideaId)
        {
            ViewBag.ideaId = ideaId;
            return View();
        }

        /// <summary>
        /// Grids the data.
        /// </summary>
        /// <param name="sidx">The sidx.</param>
        /// <param name="sord">The sord.</param>
        /// <param name="ideaId">The idea id.</param>
        /// <param name="page">The page.</param>
        /// <param name="rows">The rows.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GridData(string sidx, string sord, int ideaId = 1, int page = 1, int rows = 10)
        {
            page = Convert.ToInt32(Request["page"]);
            rows = Convert.ToInt32(Request["rows"]);
            sidx = Request["sidx"];
            sord = Request["sord"];
            int pageIndex = page - 1;
            int pageSize = rows;
            string filters = Request["filters"];

            IdeaDetailModel model = new IdeaDetailModel();
            model = Mapper.Map<IdeaDetailModel>(IdeaService.GetIdeaData(ideaId, "", "", pageIndex, pageSize, filters));

            Idea idea = IdeaService.GetIdeaById(ideaId);
            int totalRecords = IdeaService.CountIdeaRows(idea, filters);
            int totalPages = (int)Math.Ceiling((float)totalRecords / (float)pageSize);

            ArrayList dataRows = new ArrayList();

            foreach (var item in model.Rows)
            {
                List<object> cellvalue = new List<object>();
                for (int j = 0; j < model.Columns.Count; j++)
                {
                    if (model.Columns[j].ReferedIdeaId != 0)
                    {
                        string filter = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"R.RowId\",\"op\":\"bw\",\"data\":\"" + item.Values[j] + "\"}]}";
                        string rowValue = "";
                        Idea refIdea = IdeaService.GetIdeaById(model.Columns[j].ReferedIdeaId);
                        refIdea.Columns = IdeaService.GetRefColumnsByColumnId(model.Columns[j].ColumnId);
                        IdeaDetail rowDetail = IdeaService.GetIdeaData(refIdea, "", "", 0, 1, filter);
                        if (rowDetail.Rows.Count > 0)
                        {
                            foreach (var value in rowDetail.Rows[0].Values)
                            {
                                rowValue += value;
                            }
                            rowValue = "<a href=\"javascript:void(0)\" onclick=\"OpenSubGrid(" + model.Columns[j].ReferedIdeaId + "," + item.Values[j] + ");\">" + rowValue + "</a>";
                            cellvalue.Add(rowValue);
                        }
                    }
                    else
                    {
                        cellvalue.Add(item.Values[j]);
                    }
                }
                cellvalue.Insert(0, item.Version);
                object row = new
                {
                    id = item.RowId,
                    cell = cellvalue
                };
                dataRows.Add(row);
            }

            var jsonData = new
            {
                total = totalPages,
                page = page,
                records = totalRecords,
                rows = dataRows
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Grids the data by row id.
        /// </summary>
        /// <param name="rowId">The row id.</param>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GridDataByRowId(int rowId, int ideaId = 21)
        {
            IdeaDetailModel model = new IdeaDetailModel();
            string filters = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"R.RowId\",\"op\":\"bw\",\"data\":\"" + rowId + "\"}]}";
            IdeaDetail ideadetail = IdeaService.GetIdeaData(ideaId, "", "", 0, 1, filters);
            model = Mapper.Map<IdeaDetailModel>(ideadetail);

            Idea idea = IdeaService.GetIdeaById(ideaId);

            ArrayList dataRows = new ArrayList();

            foreach (var item in model.Rows)
            {
                List<object> cellvalue = item.Values.ToList();
                cellvalue.Insert(0, item.Version);
                object row = new
                {
                    id = item.RowId,
                    cell = cellvalue
                };
                dataRows.Add(row);
            }

            var jsonData = new
            {
                total = 1,
                page = 1,
                records = 1,
                rows = dataRows
            };

            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Edits the data.
        /// </summary>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public string EditData(int ideaId)
        {
            ideaId = Convert.ToInt32(Request["ideaId"]);
            int userId = Convert.ToInt32(Session["UserId"]);
            string operation = Request["oper"];

            if (operation == "del")
            {
                Array idArray = Request["id"].Split(',');
                foreach (var id in idArray)
                {
                    DeleteRow(Convert.ToInt32(id));
                }
            }

            if (operation == "add")
            {
                IdeaModel idea = Mapper.Map<IdeaModel>(IdeaService.GetIdeaById(ideaId));
                RowModel row = new RowModel()
                {
                    IdeaId = ideaId,
                    UserId = userId,
                    Columns = idea.Columns
                };
                foreach (var column in row.Columns)
                {
                    row.Values.Add(Request[column.ColumnName]);
                }
                if (!AddRow(row))
                {
                    return "failed!";
                }
            }

            if (operation == "edit")
            {
                int id = Convert.ToInt32(Request["id"]);
                RowModel row = Mapper.Map<RowModel>(RowService.GetRowById(id, userId));
                if (row.Version == Convert.ToInt32(Request["Version"]))
                {
                    IdeaModel idea = Mapper.Map<IdeaModel>(IdeaService.GetIdeaById(ideaId));
                    row.Columns = idea.Columns;
                    foreach (var column in row.Columns)
                    {
                        row.Values.Add(Request[column.ColumnName]);
                    }
                    if (!UpdateRow(row))
                    {
                        return "failed!";
                    }
                }
            }
            return "success!";
        }

        /// <summary>
        /// Add a row with data.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public bool AddRow(RowModel row)
        {
            return RowService.AddRow(Mapper.Map<Row>(row));
        }

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="row">The row.</param>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public bool UpdateRow(RowModel row)
        {
            return RowService.UpdateRow(Mapper.Map<Row>(row));
        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="id">The id.</param>
        public bool DeleteRow(int id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            return RowService.DeleteRow(id, userId);
        }

        /// <summary>
        /// Gets the idea grid columns.
        /// </summary>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult GetIdeaGridColumns(int ideaId)
        {
            IdeaModel idea = new IdeaModel
            {
                IdeaId = ideaId,
                UserId = Convert.ToInt32(Session["UserId"])
            };
            idea = Mapper.Map<IdeaModel>(IdeaService.GetIdeaById(ideaId));
            ArrayList colNames = new ArrayList();
            IList<object> colModels = new List<object>();
            object rowVersion = new
            {
                name = "Version",
                index = "R.Version",
                editable = true,
                hidden = true
            };
            colNames.Add("Version");
            colModels.Add(rowVersion);
            int count = 0;
            string widthPersent = 100 / idea.Columns.Count + "%";
            foreach (var item in idea.Columns)
            {
                object colModel;
                count++;
                switch (item.MyDataTypeId)
                {
                    case DataTypeId.Money:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "M_" + count + ".Value",
                            editable = true,
                            editrules = new
                            {
                                number = true
                            },
                            align = "center",
                            formatter = "currency",
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.Number:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "N_" + count + ".Value",
                            editable = true,
                            editrules = new
                            {
                                number = true
                            },
                            formatter = "number",
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.Datetime:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "D_" + count + ".Value",
                            editable = true,
                            editoptions = new
                            {
                                dataInit = "js:function(elm){setTimeout(function(){jQuery(elm).datepicker({}});});}"
                            },
                            formatter = "date",
                            formatoptions = new
                            {
                                srcformat = "ISO8601Long",
                                newformat = "Y-m-d H:i:s"
                            },
                            searchoptions = new
                            {
                                dataInit = "function(element) { $(element).datepicker({dateFormat:  'yyyy-mm-dd'}) }$"
                            },
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.LongText:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "L_" + count + ".Value",
                            editable = true,
                            edittype = "textarea",
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.ShortText:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "S_" + count + ".Value",
                            editable = true,
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.Entity:
                        string cellvalue = "";
                        Idea refIdea = IdeaService.GetIdeaById(item.ReferedIdeaId);
                        refIdea.Columns = IdeaService.GetRefColumnsByColumnId(item.ColumnId);
                        IdeaDetail rowDetail = IdeaService.GetIdeaData(refIdea, "", "", 0, 500, null);//Modified by Wen Junjian on 2013-10-21
                        if (rowDetail.Rows.Count > 0)
                        {
                            for (int i = 0; i < rowDetail.Rows.Count; i++)
                            {
                                string rowValue = "";
                                foreach (var value in rowDetail.Rows[i].Values)
                                {
                                    rowValue += value + ",";
                                }
                                if (rowValue.Contains(','))
                                {
                                    rowValue = rowValue.Remove(rowValue.LastIndexOf(","), 1);
                                }
                                rowValue = rowDetail.Rows[i].RowId + ":" + rowValue;
                                cellvalue += rowValue + ";";
                            }
                            cellvalue = cellvalue.Remove(cellvalue.LastIndexOf(";"), 1);
                        }
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "C_" + count + ".RefRowId",
                            editable = true,
                            edittype = "select",
                            editoptions = new
                            {
                                value = cellvalue
                            },
                            width = widthPersent
                        };
                        break;
                    case DataTypeId.Status:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = "N_" + count + ".Value",
                            editable = true,
                            edittype = "select",
                            editoptions = new
                            {
                                value = "1:Success!;0:Failed"
                            },
                            formatter = "select",
                            width = widthPersent
                        };
                        break;
                    default:
                        colModel = new
                        {
                            columnId = item.ColumnId,
                            name = item.ColumnName,
                            index = item.ColumnName,
                            editable = true,
                            search = false,
                            width = widthPersent
                        };
                        break;
                }

                colModels.Add(colModel);
                colNames.Add(item.ColumnName);
            }
            var jsonData = new
            {
                IdeaName = idea.IdeaName,
                colNames = colNames,
                colModels = colModels
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);

        }

    }
}
