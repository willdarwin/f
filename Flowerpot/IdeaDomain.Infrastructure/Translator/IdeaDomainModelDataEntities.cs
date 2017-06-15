using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.AutoMapper;

namespace IdeaDomain.InfrastructureLayer.Translator
{
    public class IdeaDomainModelDataEntities : AutoMapperWrapper
    {
        /// <summary>
        /// Configure all the mappings that need to be done using AutoMapper
        /// </summary>
        public IdeaDomainModelDataEntities()
        {
            Initialize(cfg =>
            {
                cfg.CreateMap<IdeaDE, Idea>();
                cfg.CreateMap<ColumnDE, ColumnInIdea>();
                cfg.CreateMap<RowDE, Row>();
                cfg.CreateMap<IdeaDetail, Idea>();
                cfg.CreateMap<AnalyzerDE, Analyzer>();
                cfg.CreateMap<AnalyzerDetail, Analyzer>();


                cfg.CreateMap<Idea, IdeaDE>();
                cfg.CreateMap<ColumnInIdea, ColumnDE>();
                cfg.CreateMap<Row, RowDE>();
                cfg.CreateMap<Idea, IdeaDetail>();
                cfg.CreateMap<Analyzer, AnalyzerDE>();
                cfg.CreateMap<Analyzer, AnalyzerDetail>();

            });

        }
    }
}