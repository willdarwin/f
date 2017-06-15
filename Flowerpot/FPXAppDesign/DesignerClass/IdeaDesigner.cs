using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass
{
    public class IdeaDesigner
    {
        [XmlAttribute("EntityId")]
        public int IdeaId { get; set; }

        [XmlAttribute("EntityName")]
        public string IdeaName { get; set; }
    }
}
