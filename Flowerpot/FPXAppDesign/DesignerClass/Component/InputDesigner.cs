using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    [Serializable]
    public class InputDesigner
    {
        [XmlAttribute]
        public int Row { get; set; }

        [XmlAttribute]
        public int Column { get; set; }

        [XmlElement]
        public LabelDesigner  Label { get; set; }

        [XmlElement]
        public TextDesigner Text { get; set; }

        [XmlElement]
        public ButtonDesigner Button { get; set; }
    }
}