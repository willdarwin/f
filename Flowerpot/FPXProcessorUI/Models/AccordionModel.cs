using System.Collections.Generic;

namespace FPXProcessorUI.Models
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