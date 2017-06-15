using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Specialized;
using IdeaDomain.DomainLayer;
using MVCWebUIComponent.Models;
using System.Linq.Dynamic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Json;
using AutoMapper;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using GeneralUtilities.Utilities.Unity;
using IdeaDomain.ServiceLayer;
using IdeaDomain.DomainLayer.Entities;

namespace MVCWebUIComponent.Proxy
{
    public class ServiceProxy
    {
        private IIdeaService _ideaService;
        private IRowService _rowService;

        public ServiceProxy()
        {
            _ideaService = PolicyInjection.Wrap<IIdeaService>(Container.Resolve<IdeaService>());
            _rowService = PolicyInjection.Wrap<IRowService>(Container.Resolve<IRowService>());
        }

        public string Show(NameValueCollection collection)
        {
            var ideaId = Convert.ToInt32(collection["ideaId"]);
            var strPage = collection["page"];
            var strRows = collection["rows"];
            var sord = collection["sord"];
            var sidx = collection["sidx"];
            var filters = collection["filters"];
            var result = "";
            var page = 0;
            var rows = 0;
            if (strPage != null)
            {
                page = Convert.ToInt32(strPage);
            }
            if (strRows != null)
            {
                rows = Convert.ToInt32(strRows);
            }
            var ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", page - 1, rows, filters);
            var idea = _ideaService.GetIdeaById(ideaId);

            var jsonResult = new MyJsonResult();
            var total = _ideaService.CountIdeaRows(idea, filters);
            jsonResult.TotalRecords = total.ToString();
            jsonResult.Page = page.ToString();
            jsonResult.TotalPages = (total / rows + 1).ToString();

            for (var i = 0; i < ideaDetail.Rows.Count; i++)
            {
                var row = ideaDetail.Rows[i];
                var gridRow = new GridRow();
                gridRow.Id = row.RowId;
                for (var j = 0; j < ideaDetail.Columns.Count; j++)
                {
                    gridRow.Cell.Add(row.Values[j].ToString());
                }
                jsonResult.Rows.Add(gridRow);
            }

            var ser = new DataContractJsonSerializer(typeof(MyJsonResult));
            var ms = new MemoryStream();
            ser.WriteObject(ms, jsonResult);
            result = Encoding.UTF8.GetString(ms.ToArray());
            return result;
        }

        public string Add(NameValueCollection collection)
        {
            var ideaId = Convert.ToInt32(collection["ideaId"]);
            var userId = Convert.ToInt32(collection["userId"]);
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

        public string Edit(NameValueCollection collection)
        {
            var result = "";
            var ideaId = Convert.ToInt32(collection["ideaId"]);
            var userId = Convert.ToInt32(collection["userId"]);
            var id = Convert.ToInt32(collection["id"]);
            var row = _rowService.GetRowById(id, userId);
            var idea = _ideaService.GetIdeaById(ideaId);
            row.Columns = idea.Columns;
            foreach (var column in row.Columns)
            {
                row.Values.Add(collection[column.ColumnName]);
            }
            result = _rowService.UpdateRow(row).ToString();
            return result;
        }

        public string Del(NameValueCollection collection)
        {
            var result = "";
            var userId = Convert.ToInt32(collection["userId"]);
            var rowId = Convert.ToInt32(collection["id"]);
            result = _rowService.DeleteRow(rowId, userId).ToString();
            return result;
        }
    }
}
