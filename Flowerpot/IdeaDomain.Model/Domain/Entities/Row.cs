using System.Collections.Generic;

namespace IdeaDomain.DomainLayer.Entities
{
    public class Row
    {
        public Row()
        {
            Columns = new List<ColumnInIdea>();
            Values = new List<object>();
            IsDeleted = false;
        }

        public int RowId { get; set; }

        public int IdeaId { get; set; }

        public int Version { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<ColumnInIdea> Columns { get; set; }

        public IList<object> Values { get; set; }
    }
}
