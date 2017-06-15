using System.Collections.Generic;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class RowDesigner
    {
        public RowDesigner()
        {
            Columns = new List<ColumnInIdeaModel>();
            Values = new List<object>();
            IsDeleted = false;
        }

        [XmlAttribute]
        public int RowId { get; set; }

        [XmlAttribute]
        public int IdeaId { get; set; }

        [XmlAttribute]
        public int Version { get; set; }

        [XmlAttribute]
        public bool IsDeleted { get; set; }

        [XmlAttribute]
        public int UserId { get; set; }

        [XmlArray]
        [XmlArrayItem("Column")]
        public List<ColumnInIdeaModel> Columns { get; set; }

        [XmlArray]
        [XmlArrayItem("Value")]
        public List<object> Values { get; set; }
    }
}