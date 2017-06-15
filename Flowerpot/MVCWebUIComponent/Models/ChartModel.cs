using System;
using System.Collections.Generic;
using IdeaDomain.DomainLayer;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.ServiceLayer;

namespace MVCWebUIComponent.Models
{
    public class ChartModel
    {
        public ChartModel()
        {
            Labels = new List<string>();
            Values = new List<string>();
        }

        public int Id { get; set; }

        public string ChartTitle { get; set; }

        public string ChartType { get; set; }

        public int ValueCount { get; set; }

        public int IdeaId { get; set; }

        public string IdeaName { get; set; }

        public int AnalyzerId { get; set; }

        public string AnalyzerName { get; set; }

        public List<string> Labels { get; set; }

        public List<string> Values { get; set; }

        public static ChartModel GetChartSourceByAnalyzerId(int analyzerId, string chartType = "pie")
        {
            var service = new AnalyzerService();
            var detail = service.GetAnalyzerDataById(analyzerId);
            var result = ConvertToChartModel(detail);
            result.ChartTitle = string.Empty;
            result.ChartType = chartType;
            result.ValueCount = result.Labels.Count;
            return result;
        }

        public static ChartModel ConvertToChartModel(AnalyzerDetail detail)
        {
            if (detail.Columns.Count != 2) return null;

            var source = new ChartModel();
            foreach (var r in detail.Rows)
            {
                source.Labels.Add(r.Values[0].ToString());
                source.Values.Add(Convert.ToInt32(r.Values[1] == DBNull.Value ? 0 : r.Values[1]).ToString());
            }
            return source;
        }

        public static ChartModel InitializeChartModel()
        {
            IAnalyzerService target = new AnalyzerService();

            var analyzer = new Analyzer
                {
                    AnalyzerName = "Times customer go to the Gym this month",
                    SelectQuery =
                        "Select Customer.CustomerId, sum(ConsumptionRecord.ConsumptionAmount) as Turnover",
                    JoinQuery = " [3][ right join ][2][ on ConsumptionRecord.CardId = GymCard.RowId ][ right join ][1] [ on GymCard.CustomerId = Customer.RowId ]",
                    WhereQuery = "Where 1=1 Group by Customer.CustomerId"
                };
            //
            //Customer.CustomerId,sum(ConsumptionRecord.ConsumptionAmount) as turnover
            //
            //Select  month(ConsumptionRecord.StartTime) as month,sum(ConsumptionRecord.ConsumptionAmount) as turnover 
            //Where ConsumptionRecord.StartTime >2012  Group by Customer.CustomerId,ConsumptionRecord.StartTime
            //order by month
            //
            //analyzer = new Analyzer
            //{
            //    AnalyzerName = "Times customer go to the Gym this year",
            //    SelectQuery =
            //        "Select Customer.CustomerId, " +
            //        "sum(case when (year(ConsumptionRecord.StartTime)=year(GETDATE())) then 1 else 0 end) as times ",
            //    JoinQuery = " [3][ right join ][2][ on ConsumptionRecord.CardId = GymCard.RowId ][ right join ][1] [ on GymCard.CustomerId = Customer.RowId ]",
            //    WhereQuery = "Where 1=1 Group by Customer.CustomerId"
            //}; 

            var data = target.GetAnalyzerData(analyzer);
            var result = ConvertToChartModel(data);
            result.ChartTitle = "pie";
            result.ChartType = "column";
            result.ValueCount = result.Labels.Count;
            return result;
        }
    }
}