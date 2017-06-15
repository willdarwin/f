using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass
{
    public class AnalyzerDesigner
    {
        [XmlAttribute]
        public int AnalyzerId { get; set; }

        [XmlAttribute]
        public string AnalyzerName { get; set; }
    }
}
