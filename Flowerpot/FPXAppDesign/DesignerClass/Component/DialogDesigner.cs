using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class DialogDesigner
    {
        [XmlAttribute]
        public string Title { get; set; }

        [XmlAttribute]
        public bool IsModal { get; set; }

        [XmlAttribute] 
        public DialogButtonType DialogType;

        [XmlElement]
        public string Content { get; set; }
    }

    public enum DialogButtonType
    { 
        OKCancel = 0,
        YesNo = 1
    }
}