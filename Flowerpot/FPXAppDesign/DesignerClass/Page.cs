using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using FPXAppDesign.DesignerClass.Component;
using FPXApplicationInterface.Interface;

namespace FPXAppDesign.DesignerClass
{
    public class Page
    {
        #region attribute

        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public long Id { get; set; }

        [XmlAttribute]
        public TemplateType Template { get; set; }

        [XmlAttribute]
        public int Index { get; set; }

        [XmlAttribute]
        public bool IsDefault { get; set; }

        #endregion

        #region nodes

        [XmlElement]
        public Header Header { get; set; }

        [XmlArray]
        [XmlArrayItem("Menu")] 
        public List<MenuDesigner> MenuList { get; set; }

        [XmlArray]
        [XmlArrayItem("Grid")] 
        public List<GridDesigner> GridList { get; set; }

        [XmlArray]
        [XmlArrayItem("Input")] 
        public List<InputDesigner> InputList { get; set; }

        [XmlArray]
        [XmlArrayItem("Label")] 
        public List<LabelDesigner> LabelList { get; set; }

        [XmlArray]
        [XmlArrayItem("Text")] 
        public List<TextDesigner> TextModelList { get; set; }

        [XmlArray]
        [XmlArrayItem("Chart")] 
        public List<ChartDesigner> ChartList { get; set; }

        [XmlArray]
        [XmlArrayItem("Button")] 
        public List<ButtonDesigner> ButtonList { get; set; }

        [XmlArray]
        [XmlArrayItem("Booking")]
        public List<BookingDesigner> Bookings { get; set; }

        [XmlElement]
        public TabsDesigner TabsDesigner { get; set; }

        [XmlElement]
        public Footer Footer { get; set; }

        #endregion

        #region constructor

        public Page()
        {
            Template = TemplateType.DefaultT;
        }

        #endregion
    }
}
