using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FPXProcessorUI.Models;
using IdeaDomain.ServiceLayer;
using IdeaDomain.DomainLayer.Entities;

namespace FPXProcessorUI.Services
{
    public class ChartService
    {

        private IIdeaService _ideaService;

        public ChartModel CreateChart(Analyzer analyzer, string chartTitle, string chartType)
        {
            ChartModel resultModel = new ChartModel();

            //Add some test data
            List<DataSource> dataSource = new List<DataSource>();
            dataSource.Add(new DataSource("R", "27"));
            dataSource.Add(new DataSource("S", "13"));
            dataSource.Add(new DataSource("C", "26"));
            dataSource.Add(new DataSource("W", "7"));
            dataSource.Add(new DataSource("SL", "15"));
            dataSource.Add(new DataSource("P", "12"));
            //dataSource[0].label = "R";
            //dataSource[1].label = "S";
            //dataSource[2].label = "C";
            //dataSource[3].label = "W";
            //dataSource[4].label = "SL";
            //dataSource[5].label = "P";

            //dataSource[0].value = "27";
            //dataSource[1].value = "13";
            //dataSource[2].value = "26";
            //dataSource[3].value = "7";
            //dataSource[4].value = "15";
            //dataSource[5].value = "12";

            //List<DataSource> dataSource = _ideaService.GetIdeasForChart(analyzer);
            resultModel.chartType = chartType;
            resultModel.chartTitle = chartTitle;
            resultModel.valueCount = dataSource.Count;

            for (int i = 0; i < dataSource.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataSource[i].label))
                {
                    resultModel.Labels.Add(dataSource[i].label);
                }
                else
                {
                    resultModel.Labels.Add("Null");
                }
                if (!string.IsNullOrEmpty(dataSource[i].value))
                {
                    resultModel.Values.Add(dataSource[i].value);
                }
                else
                {
                    resultModel.Values.Add("0");
                }
            }

            return resultModel;
        }

        public ChartModel CreateChart(Analyzer analyzer, string chartTitle, string chartType, List<DataSource> dataSources)
        {
            ChartModel resultModel = new ChartModel();

            //Add some test data
            //dataSources.Add(new DataSource("R", "27"));
            //dataSources.Add(new DataSource("S", "13"));
            //dataSources.Add(new DataSource("C", "26"));
            //dataSources.Add(new DataSource("W", "7"));
            //dataSources.Add(new DataSource("SL", "15"));
            //dataSources.Add(new DataSource("P", "12"));
            //dataSource[0].label = "R";
            //dataSource[1].label = "S";
            //dataSource[2].label = "C";
            //dataSource[3].label = "W";
            //dataSource[4].label = "SL";
            //dataSource[5].label = "P";

            //dataSource[0].value = "27";
            //dataSource[1].value = "13";
            //dataSource[2].value = "26";
            //dataSource[3].value = "7";
            //dataSource[4].value = "15";
            //dataSource[5].value = "12";

            //List<DataSource> dataSource = _ideaService.GetIdeasForChart(analyzer);
            resultModel.chartType = chartType;
            resultModel.chartTitle = chartTitle;
            resultModel.valueCount = dataSources.Count;

            for (int i = 0; i < dataSources.Count; i++)
            {
                if (!string.IsNullOrEmpty(dataSources[i].label))
                {
                    resultModel.Labels.Add(dataSources[i].label);
                }
                else
                {
                    resultModel.Labels.Add("Null");
                }
                if (!string.IsNullOrEmpty(dataSources[i].value))
                {
                    resultModel.Values.Add(dataSources[i].value);
                }
                else
                {
                    resultModel.Values.Add("0");
                }
            }

            return resultModel;
        }
    }
}