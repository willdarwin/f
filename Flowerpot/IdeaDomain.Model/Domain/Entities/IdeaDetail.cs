using System;
using System.Collections.Generic;

namespace IdeaDomain.DomainLayer.Entities
{
    public class IdeaDetail
    {
        public IdeaDetail()
        {
            Columns = new List<ColumnInIdea>();
            Rows = new List<Row>();
            IsDeleted = false;
        }

        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<ColumnInIdea> Columns { get; set; }

        public IList<Row> Rows { get; set; }
    }
}
