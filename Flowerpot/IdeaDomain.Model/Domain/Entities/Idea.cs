using System;
using System.Collections.Generic;

namespace IdeaDomain.DomainLayer.Entities
{
    public class Idea
    {
        public Idea()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
            Columns = new List<ColumnInIdea>();
        }

        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<ColumnInIdea> Columns { get; set; }
    }
}
