using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXAppDesign.DesignerClass.Component;
using FPXApplicationInterface.Interface;


namespace FPXAppDesign.DesignerClass
{
    [XmlRoot]
    public class UserApp
    {
        #region attribute

        [XmlAttribute]
        public string Description { get; set; }

        [XmlAttribute]
        public string Status { get; set; }

        [XmlAttribute]
        public string DefaultPage { get; set; }

        [XmlAttribute]
        public string FileName { get; set; }

        [XmlAttribute]
        public string Template { get; set; }

        #endregion

        #region nodes

        [XmlElement]
        public UserInfo User { get; set; }

        [XmlArray("Entities")]
        [XmlArrayItem("Entity")]
        public List<IdeaDesigner> Ideas { get; set; }

        [XmlArray]
        [XmlArrayItem("Analyzer")]
        public List<AnalyzerDesigner> Analyzers { get; set; }

        [XmlArray]
        [XmlArrayItem("LeftMenuItem")]
        public LeftMenuItemDesigner[] LeftMenu { get; set; }

        [XmlArray]
        [XmlArrayItem("ChartDataSource")]
        public ChartDataSource[] ChartDataSources { get; set; }

        [XmlArray]
        [XmlArrayItem("PartialView")]
        public string[] PartialViews { get; set; }

        [XmlArray]
        [XmlArrayItem("Page")]
        public Page[] Pages { get; set; }

        #endregion
    }
}
