using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FPXProcessorUI.Models
{
    public class ChartModel
    {
        public ChartModel()
        {
            Labels = new List<string>();
            Values = new List<string>();
        }
        public List<string> Labels;
        public List<string> Values;
        public string chartTitle;
        public string chartType;
        public int valueCount;
    }
}