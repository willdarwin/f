using System.Collections.Generic;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class TabsDesigner
    {
        [XmlArray]
        [XmlArrayItem("Tab")]
        public List<TabsGroup> TabsGroups { get; set; }
    }

    public class TabsGroup
    {
        [XmlAttribute]
        public string Header { get; set; }

        [XmlAttribute]
        public string Body { get; set; }
    }
}