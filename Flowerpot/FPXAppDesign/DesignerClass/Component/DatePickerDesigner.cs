using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class DatePickerDesigner
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Value { get; set; }
    }
}