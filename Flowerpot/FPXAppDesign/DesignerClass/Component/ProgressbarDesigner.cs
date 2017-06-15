using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class ProgressbarDesigner
    {
        [XmlElement]
        // unit millisecond
        public int LoadingTimeInterval { get; set; }

        [XmlElement]
        public int IncrementTimeInterval { get; set; }
    }
}