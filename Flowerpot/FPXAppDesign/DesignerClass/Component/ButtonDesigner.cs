using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class ButtonDesigner
    {
        [XmlAttribute]
        public string Text { get; set; }
    }    
}