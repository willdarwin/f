using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class LongTextDE
    {
        public LongTextDE()
        {
            IsDeleted = false;
        }
        public int LongTextId { get; set; }
        public string Value { get; set; }
        public int ColumnId { get; set; }
        public int RowId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
