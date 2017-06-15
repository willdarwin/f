using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebUIComponent.Models
{
    public class AccordionModel
    {
        public List<AccordionGroup> AccordionGroups { get; set; }
    }

    public class AccordionGroup
    {
        public string Header { get; set; }
        public string Body { get; set; }        
    }
}