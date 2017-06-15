using System;

namespace IdeaDomain.InfrastructureLayer.DataEntities
{
    public class ColumnDE
    {
        public ColumnDE()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }

        public int ColumnId { get; set; }

        public string ColumnName { get; set; }

        public int DataTypeId { get; set; }

        public int ReferedIdeaId { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int IdeaId { get; set; }

    }
}
