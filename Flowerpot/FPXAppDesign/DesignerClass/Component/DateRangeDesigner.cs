using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class DateRangeDesigner
    {
        [XmlElement]
        public DatePickerDesigner From { get; set; }

        [XmlElement]
        public DatePickerDesigner To { get; set; }
    }
}