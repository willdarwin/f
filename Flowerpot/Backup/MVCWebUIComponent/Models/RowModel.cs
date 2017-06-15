using System.Collections.Generic;

namespace MVCWebUIComponent.Models
{
    public class RowModel
    {
        public RowModel()
        {
            Columns = new List<ColumnInIdeaModel>();
            Values = new List<object>();
            IsDeleted = false;
        }

        public int RowId { get; set; }

        public int IdeaId { get; set; }

        public int Version { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<ColumnInIdeaModel> Columns { get; set; }

        public IList<object> Values { get; set; }
    }
}