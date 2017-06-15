using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdeaDomain.DomainLayer.Entities
{
    public class DataSource
    {
        public DataSource()
        {
            this.label = string.Empty;
            this.value = string.Empty;
        }

        public DataSource(string label, string value)
        {
            this.label = label;
            this.value = value;
        }

        public string label;
        public string value;
    }
}
