using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCWebUIComponent.Models
{
    public class ProgressbarModel
    {
        // unit millisecond
        public int LoadingTimeInterval { get; set; }
        public int IncrementTimeInterval { get; set; }
    }
}