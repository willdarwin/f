﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdeaDomain.ServiceLayer;
using System.ComponentModel;
using System.Collections.Specialized;
using IdeaDomain.DomainLayer.Entities;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection;
using IdeaDomain.DomainLayer;
using Container = GeneralUtilities.Utilities.Unity.Container;
using MVCWebUIComponent.Models;

namespace MVCWebUIComponent.Services
{
    public class GridService
    {
        private IIdeaService _ideaService;
        private IRowService _rowService;

        public GridService()
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
            var ideaDetail = _ideaService.GetIdeaData(ideaId, sidx, sord, page - 1, rows, filters);
            var idea = _ideaService.GetIdeaById(ideaId);

            //int sortColumnId = 0; 
            //List<Row> rowList;
            //for (int i = 0; i < ideaDetail.Columns.Count; i++)
            //{
            //    if (ideaDetail.Columns[i].ColumnName == sidx && ideaDetail.Columns[i].DataTypeId == Convert.ToInt32(MVCWebUIComponent.Models.DataTypeId.IdeaType))
            //    {
            //        ideaDetail = _ideaService.GetIdeaData(ideaId, "", "", 0, 10000, filters);
            //        //sortColumnId = i;
            //        if (sord == "asc")
            //            rowList = ideaDetail.Rows.OrderBy(m => m.Values[i]).Take(rows).ToList();
            //        else
            //            rowList = ideaDetail.Rows.OrderByDescending(m => m.Values[i]).Take(rows).ToList();
            //        ideaDetail.Rows = rowList;
            //        break;
            //    }
            //}

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
                    if (ideaDetail.Columns[j].ReferedIdeaId != 0)
                    {
                        var rowValue = "";
                        var filter = "{\"groupOp\":\"AND\",\"rules\":[{\"field\":\"R.RowId\",\"op\":\"bw\",\"data\":\"" + row.Values[j] + "\"}]}";
                        var refIdea = _ideaService.GetIdeaById(ideaDetail.Columns[j].ReferedIdeaId);
                        refIdea.Columns = _ideaService.GetRefColumnsByColumnId(ideaDetail.Columns[j].ColumnId);
                        var rowDetail = _ideaService.GetIdeaData(refIdea, "", "", 0, 1, filter);
                        foreach (var value in rowDetail.Rows[0].Values)
                        {
                            rowValue += value;
                        }
                        rowValue = "<a href=\"javascript:void(0)\" onclick=\"OpenSubGrid(" + ideaDetail.Columns[j].ReferedIdeaId + "," + row.Values[j] + ");\">" + rowValue + "</a>";
                        gridRow.Cell.Add(rowValue);
                    }
                    else
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