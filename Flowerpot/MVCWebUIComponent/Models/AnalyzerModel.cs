using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Trirand.Web.Mvc;
using System.Web.UI.WebControls;

namespace MVCWebUIComponent.Models
{
    public class AnalyzerModel
    {
        public AnalyzerModel()
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

        public string[] SelectList { get; set; }

        public string[] JoinList { get; set; }

        public string[] WhereList { get; set; }

        public bool IsDeleted { get; set; }

    }
    public class AnalyzerDetailModel
    {
        public AnalyzerDetailModel()
        {
            Rows = new List<RowModel>();
            Columns = new List<ColumnInIdeaModel>();
            CreateTime = DateTime.Now;
            IsDeleted = false;
        }
        public int AnalyzerId { get; set; }

        public string AnalyzerName { get; set; }

        public string IdeaDescription { get; set; }

        public DateTime CreateTime { get; set; }

        public bool IsDeleted { get; set; }

        public int UserId { get; set; }

        public IList<RowModel> Rows { get; set; }

        public IList<ColumnInIdeaModel> Columns { get; set; }
    }

    public class CreateAnalyzerModel
    {
        public CreateAnalyzerModel()
        {
            IdeaList = new List<IdeaModel>();
            Analyzer = new AnalyzerModel();
        }
        public IList<IdeaModel> IdeaList { get; set; }

        public AnalyzerModel Analyzer { get; set; }
    }

}
