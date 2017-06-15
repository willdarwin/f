using System;
using System.Collections.Generic;

namespace IdeaDomain.DomainLayer.Entities
{
    public class AnalyzerDetail
    {
        public AnalyzerDetail()
        {
            Columns = new List<ColumnInIdea>();
            Rows = new List<Row>();
            IsDeleted = false;
        }

        public int AnalyzerId { get; set; }

        public string AnalyzerName { get; set; }

        public DateTime CreateTime { get; set; }

        public string SelectQuery { get; set; }

        public string JoinQuery { get; set; }

        public string WhereQuery { get; set; }

        public IList<ColumnInIdea> Columns { get; set; }

        public IList<Row> Rows { get; set; }

        public int UserId { get; set; }

        public bool IsDeleted { get; set; }

    }
}
