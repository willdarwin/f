using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXApplicationInterface.Interface;

namespace FPXAppDesign.DesignerClass
{
    //public class DataTable : IDataSource
    //{
    //    #region attribute
    //    [XmlAttribute]
    //    public int AppID { get; set; }

    //    [XmlAttribute]
    //    public int ID { get; set; }

    //    [XmlAttribute]
    //    public string Name { get; set; }
    //    #endregion

    //    #region nodes
    //    #endregion
    //}

    public class ChartDataSource
    {
        public ChartDataSource()
        {
            this.Label = string.Empty;
            this.Value = string.Empty;
        }

        public ChartDataSource(string label, string value)
        {
            this.Label = label;
            this.Value = value;
        }

        [XmlAttribute]
        public string Label { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}
