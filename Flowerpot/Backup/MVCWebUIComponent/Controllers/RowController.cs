using System;
using System.Web.Mvc;
using AutoMapper;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.ServiceLayer;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using MVCWebUIComponent.Models;
using MVCWebUIComponent.Models.Translates;
using UtilityComponent.Filters;

namespace MVCWebUIComponent.Controllers
{
    public class RowController : Controller
    {
        private IRowService RowService { get; set; }

        public RowController(IRowService rowService)
        {
            RowService = rowService;
        }

        public RowController()
        {
            RowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
        }

        /// <summary>
        /// Creates the specified row model.
        /// </summary>
        /// <param name="rowModel">The row model.</param>
        /// <returns></returns>
        [HttpPost]
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult Create(RowModel rowModel)
        {
            Row row = Mapper.Map<RowModel, Row>(rowModel);
            RowService.AddRow(row);
            return View();
        }

        /// <summary>
        /// Creates all.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [AutoMapperConfigurationActionFilter(typeof(IdeaDomainMVCProfile))]
        public ActionResult CreateAll(IdeaDetailModel model)
        {
            for (int i = 0; i < model.Rows.Count; i++)
            {
                RowModel rowModel = model.Rows[i];
                rowModel.Columns = model.Columns;
                rowModel.IdeaId = model.IdeaId;
                rowModel.UserId = model.UserId;
                Row row = Mapper.Map<RowModel, Row>(rowModel);
                RowService.AddRow(row);
            }
            return RedirectToAction("Informations", "Idea", new { ideaId = model.IdeaId });
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ActionResult Delete(int id)
        {
            int userId = Convert.ToInt32(Session["UserId"]);
            Row row = RowService.GetRowById(id, userId);
            RowService.DeleteRow(id, userId);
            return RedirectToAction("Informations", "Idea", new { ideaId = row.IdeaId });
        }

    }
}
