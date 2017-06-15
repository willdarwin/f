using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class MoneyDE
    {
        public MoneyDE()
        {
            IsDeleted = false;
        }
        public int MoneyId { get; set; }
        public decimal Value { get; set; }
        public int ColumnId { get; set; }
        public int RowId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
