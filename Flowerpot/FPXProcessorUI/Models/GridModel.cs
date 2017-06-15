using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FPXApplicationInterface.Interface;

namespace FPXProcessorUI.Models
{
    public class GridModel : IGrid
    {
        public GridSourceType SourceType { get; set; }

        public string Theme { get; set; }

        public string Url { get; set; }

        public int TableId { get; set; }

        public string Pager { get; set; }

        public string Caption { get; set; }

        public List<string> ColumnNames { get; set; }

        public List<ColModel> ColumnModels { get; set; }

        //public List<Dictionary<string, object>> ColumnModels { get; set; }

        public int RowNum { get; set; }

        public List<int> RowList { get; set; }

        public string SortName { get; set; }

        public GridSortOrder SortOrder { get; set; }

        public string EditUrl { get; set; }

        public string AddUrl { get; set; }

        public string DeleteUrl { get; set; }

        public bool MultiSelect { get; set; }

        public bool ViewRecords { get; set; }

        public int? Width { get; set; }

        public JsonReader MyJsonReader { get; set; }

        public GridModel()
        {
            MyJsonReader = new JsonReader();
            ColumnNames = new List<string>();
            ColumnModels = new List<ColModel>();
            //ColumnModels = new List<Dictionary<string, object>>();
            RowList = new List<int>();

            SortOrder = GridSortOrder.asc;
            EditUrl = "";
            AddUrl = "";
            DeleteUrl = "";
        }
    }

    public class JsonReader
    {
        public string Root { get; set; }

        public string Page { get; set; }

        public string Total { get; set; }

        public string Id { get; set; }

        public string Cell { get; set; }

        public string Records { get; set; }
    }

    public class ColModel : Dictionary<string, object>
    {
    }

    public enum GridSourceType
    {
        xml,
        json,
        array
    }

    public enum GridSortOrder
    {
        asc,
        desc
    }

    public class MyJsonResult
    {
        public string TotalPages { get; set; }

        public string Page { get; set; }

        public string TotalRecords { get; set; }

        public List<GridRow> Rows { get; set; }

        public MyJsonResult()
        {
            Rows = new List<GridRow>();
        }

    }

    public class GridRow
    {
        public int Id { get; set; }

        public List<string> Cell { get; set; }

        public GridRow()
        {
            Cell = new List<string>();
        }
    }

    public class GridFunction
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}