using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebUIComponent.Models
{
    public class MenuModel
    {
        public List<MenuItem> MenuItemList { get; set; }

        public static List<MenuItem> Sort(List<MenuItem> menuItems)
        {
            var menuItemList = new List<MenuItem>();
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

    public class MenuItem
    {
        public string Id { get; set; }
        public string Item { get; set; }
        public string Link { get; set; }
        public bool State { get; set; }
        public int NodeLevel { get; set; }
        public string ParentNode { get; set; }
    }

    public class LeftMenuItem
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public string Url { get; set; }

        public string FileName { get; set; }
    }
}