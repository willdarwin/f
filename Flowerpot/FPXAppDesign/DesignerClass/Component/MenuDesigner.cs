using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace FPXAppDesign.DesignerClass.Component
{
    public class MenuDesigner
    {
        [XmlArray]
        [XmlArrayItem("MenuItem")]
        public List<MenuItemDesigner> MenuItemList { get; set; }

        public static List<MenuItemDesigner> Sort(List<MenuItemDesigner> menuItems)
        {
            var menuItemList = new List<MenuItemDesigner>();
            var maxNodeLevel = 0;

            foreach (var mi in menuItems)
            {
                if (mi.NodeLevel > maxNodeLevel) maxNodeLevel = mi.NodeLevel;
            }

            for (var i = 0; i <= maxNodeLevel; i++)
            {
                foreach (var menuItem in menuItems)
                {

                }
            }

            return menuItemList;
        }
    }

    public class MenuItemDesigner
    {
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public bool State { get; set; }

        [XmlAttribute]
        public int NodeLevel { get; set; }

        [XmlAttribute]
        public string ParentNode { get; set; }

        [XmlElement]
        public string Item { get; set; }

        [XmlElement]
        public string Link { get; set; }
    }

    public class LeftMenuItemDesigner
    { 
        [XmlAttribute]
        public string Id { get; set; }

        [XmlAttribute]
        public string Value { get; set; }

        [XmlAttribute]
        public string Url { get; set; }

        [XmlAttribute]
        public string FileName { get; set; }
    }
}