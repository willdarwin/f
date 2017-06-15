using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class TextDesigner
    {
        [XmlAttribute]
        public int Column { get; set; }

        [XmlElement]
        public string Text { get; set; }
    }
}