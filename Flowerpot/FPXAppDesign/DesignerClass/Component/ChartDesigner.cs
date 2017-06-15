using System.Collections.Generic;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class ChartDesigner
    {
        public ChartDesigner()
        {
            Labels = new List<string>();
            Values = new List<string>();
        }

        [XmlAttribute]
        public int Id { get; set; }

        [XmlAttribute]
        public string ChartTitle { get; set; }

        [XmlAttribute]
        public string ChartType { get; set; }

        //[XmlAttribute]
        //public int ValueCount { get; set; }

        [XmlAttribute]
        public int IdeaId { get; set; }

        [XmlAttribute]
        public string IdeaName { get; set; }

        [XmlAttribute]
        public int AnalyzerId { get; set; }

        [XmlAttribute]
        public string AnalyzerName { get; set; }

        [XmlArray]
        [XmlArrayItem("Label")]
        public List<string> Labels { get; set; }

        [XmlArray]
        [XmlArrayItem("Value")]
        public List<string> Values { get; set; }

    }
}