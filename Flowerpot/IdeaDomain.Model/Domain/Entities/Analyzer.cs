using System;

namespace IdeaDomain.DomainLayer.Entities
{
    public class Analyzer
    {
        public Analyzer()
        {
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int AnalyzerId { get; set; }

        public string AnalyzerName { get; set; }

        public DateTime CreateTime { get; set; }

        public int UserId { get; set; }

        public string SelectQuery { get; set; }

        public string JoinQuery { get; set; }

        public string WhereQuery { get; set; }

        public bool IsDeleted { get; set; }

    }
}
