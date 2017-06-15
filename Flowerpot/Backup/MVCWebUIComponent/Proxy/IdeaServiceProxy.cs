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
            int ideaId = Convert.ToInt32(collection["ideaId"]);
            string strPage = collection["page"];
            string strRows = collection["rows"];
            string sord = collection["sord"];
            string sidx = collection["sidx"];
            string filters = collection["filters"];
            string result = "";
            int page = 0;
            int rows = 0;
            if (strPage != null)
            {
                page = Convert.ToInt32(strPage);
            }
            if (strRows != null)
            {
                rows = Convert.ToInt32(strRows);
            }
            IdeaDetail ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", page - 1, rows, filters);
            Idea idea = _ideaService.GetIdeaById(ideaId);

            MyJsonResult jsonResult = new MyJsonResult();
            int total = _ideaService.CountIdeaRows(idea, filters);
            jsonResult.TotalRecords = total.ToString();
            jsonResult.Page = page.ToString();
            jsonResult.TotalPages = (total / rows + 1).ToString();

            for (int i = 0; i < ideaDetail.Rows.Count; i++)
            {
                Row row = ideaDetail.Rows[i];
                GridRow gridRow = new GridRow();
                gridRow.Id = row.RowId;
                for (int j = 0; j < ideaDetail.Columns.Count; j++)
                {
                    gridRow.Cell.Add(row.Values[j].ToString());
                }
                jsonResult.Rows.Add(gridRow);
            }

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(MyJsonResult));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, jsonResult);
            result = Encoding.UTF8.GetString(ms.ToArray());
            return result;
        }

        public string Add(NameValueCollection collection)
        {
            int ideaId = Convert.ToInt32(collection["ideaId"]);
            int userId = Convert.ToInt32(collection["userId"]);
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

        public string Edit(NameValueCollection collection)
        {
            string result = "";
            int ideaId = Convert.ToInt32(collection["ideaId"]);
            int userId = Convert.ToInt32(collection["userId"]);
            int id = Convert.ToInt32(collection["id"]);
            Row row = _rowService.GetRowById(id, userId);
            Idea idea = _ideaService.GetIdeaById(ideaId);
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
            string result = "";
            int userId = Convert.ToInt32(collection["userId"]);
            int rowId = Convert.ToInt32(collection["id"]);
            result = _rowService.DeleteRow(rowId, userId).ToString();
            return result;
        }
    }
}
