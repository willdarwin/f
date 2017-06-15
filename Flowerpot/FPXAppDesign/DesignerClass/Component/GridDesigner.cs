using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXApplicationInterface.Interface;

namespace FPXAppDesign.DesignerClass.Component
{
    //[XmlRoot]
    //public class Grid : IGrid
    //{
    //    #region attribute
    //    [XmlAttribute]
    //    public string Name { get; set; }

    //    [XmlAttribute]
    //    public long Id { get; set; }

    //    [XmlAttribute]
    //    public string Text { get; set; }

    //    [XmlAttribute]
    //    public int XAxis { get; set; }

    //    [XmlAttribute]
    //    public int YAxis { get; set; }

    //    [XmlAttribute]
    //    public string Theme { get; set; }
    //    #endregion

    //    #region nodes

    //    [XmlElement]
    //    public DataTable Table { get; set; }

    //    [XmlElement]
    //    public Analyser Analyser { get; set; }

    //    #endregion
    //}

    [XmlRoot]
    public class GridDesigner
    {
        [XmlAttribute]
        public GridSourceType SourceType { get; set; }

        [XmlAttribute]
        public string Theme { get; set; }

        [XmlAttribute]
        public string Url { get; set; }

        [XmlAttribute]
        public int TableId { get; set; }

        [XmlAttribute]
        public string Pager { get; set; }

        [XmlAttribute]
        public string Caption { get; set; }

        [XmlAttribute]
        public string SortName { get; set; }

        [XmlAttribute]
        public GridSortOrder SortOrder { get; set; }

        [XmlAttribute]
        public string EditUrl { get; set; }

        [XmlAttribute]
        public string AddUrl { get; set; }

        [XmlAttribute]
        public string DeleteUrl { get; set; }

        [XmlAttribute]
        public bool MultiSelect { get; set; }

        [XmlAttribute]
        public bool ViewRecords { get; set; }

        [XmlAttribute]
        public int Width { get; set; }

        [XmlAttribute]
        public int RowNum { get; set; }

        [XmlAttribute("EntityId")]
        public int IdeaId { get; set; }

        [XmlAttribute("EntityName")]
        public string IdeaName { get; set; }

        [XmlAttribute]
        public int AnalyzerId { get; set; }

        [XmlAttribute]
        public string AnalyzerName { get; set; }

        #region nodes

        [XmlArray]
        [XmlArrayItem("ColumnName")]
        public List<string> ColumnNames { get; set; }

        [XmlArray]
        [XmlArrayItem("ColumnModel")]
        //public List<List<KeyValuePair<string, object>>> ColumnModels { get; set; }
        public List<ColumnModelDesigner> ColumnModels { get; set; }

        [XmlArray]
        [XmlArrayItem("Row")] 
        public List<int> RowList { get; set; }

        [XmlElement]
        public JsonReader MyJsonReader { get; set; }

        [XmlElement]
        public ChartDataSource Table { get; set; }

        #endregion

        public GridDesigner()
        {
            MyJsonReader = new JsonReader();
            ColumnNames = new List<string>();
            ColumnModels = new List<ColumnModelDesigner>();
            RowList = new List<int>();

            SortOrder = GridSortOrder.asc;
            EditUrl = "";
            AddUrl = "";
            DeleteUrl = "";
        }
    }

    public class ColumnModelDesigner : List<KeyValuePair<string, object>>
    { 
    }

    public class KeyValuePair<TK, TV>
    {
        public TK Key { get; set; }
        public TV Value { get; set; }
        public KeyValuePair() { }
        public KeyValuePair(TK key, TV value)
        {
            this.Key = key;
            this.Value = value;
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

    //public class ColModel : Dictionary<string, object>
    //{}

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
