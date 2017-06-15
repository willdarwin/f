using System.Collections.Generic;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class AccordionDesigner
    {
        [XmlArray]
        [XmlArrayItem("Accordion")]
        public List<AccordionGroup> AccordionGroups { get; set; }
    }

    public class AccordionGroup
    {
        public string Header { get; set; }
        public string Body { get; set; }        
    }
}