using System;
using System.Collections.Generic;

namespace IdeaDomain.DomainLayer.Entities
{
    public class ColumnInIdea
    {
        public ColumnInIdea()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        public int ColumnId { get; set; }

        public string ColumnName { get; set; }

        public int DataTypeId { get; set; }

        public int ReferedIdeaId { get; set; }

        public string ReferedIdeaIdName { get; set; }

        public List<int> ReferedColumnIds { get; set; }

        public string ReferedColumnNames { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int IdeaId { get; set; }
    }
}
