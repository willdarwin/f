using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPXProcessorUI.Models
{
    public class TabsModel
    {
        public List<TabsGroup> TabsGroups { get; set; }
    }

    public class TabsGroup
    {
        public string Header { get; set; }
        public string Body { get; set; }
    }
}