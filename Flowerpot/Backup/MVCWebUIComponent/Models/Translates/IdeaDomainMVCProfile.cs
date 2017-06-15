using AutoMapper;
using IdeaDomain.DomainLayer.Entities;

namespace MVCWebUIComponent.Models.Translates
{
    public class IdeaDomainMVCProfile : Profile
    {
        public override string ProfileName
        {
            get { return "IdeaDomainMVCProfile"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<IdeaModel, Idea>();
            Mapper.CreateMap<RowModel, Row>();
            Mapper.CreateMap<IdeaDetailModel, IdeaDetail>();
            Mapper.CreateMap<ColumnInIdeaModel, ColumnInIdea>().ForMember(dest => dest.DataTypeId,
                               m => m.ResolveUsing(ResolveColumnInIdeaModel));
            Mapper.CreateMap<RowModel, Row>();
            Mapper.CreateMap<AnalyzerModel, Analyzer>();
            Mapper.CreateMap<AnalyzerDetailModel, AnalyzerDetail>();

            Mapper.CreateMap<Idea, IdeaModel>();
            Mapper.CreateMap<Row, RowModel>();
            Mapper.CreateMap<IdeaDetail, IdeaDetailModel>();
            Mapper.CreateMap<ColumnInIdea, ColumnInIdeaModel>().ForMember(dest => dest.MyDataTypeId,
                               m => m.ResolveUsing(ResolveColumnInIdea));
            Mapper.CreateMap<Row, RowModel>();
            Mapper.CreateMap<Analyzer, AnalyzerModel>();
            Mapper.CreateMap<AnalyzerDetail, AnalyzerDetailModel>();
        }

        private object ResolveColumnInIdea(ColumnInIdea c)
        {
            switch (c.DataTypeId)
            {
                case 0: return DataTypeId.Money;
                case 1: return DataTypeId.Number;
                case 2: return DataTypeId.Datetime;
                case 3: return DataTypeId.LongText;
                case 4: return DataTypeId.ShortText;
                case 5: return DataTypeId.Entity;
                case 6: return DataTypeId.Status;
            }
            return null;
        }

        private object ResolveColumnInIdeaModel(ColumnInIdeaModel c)
        {
            switch (c.MyDataTypeId)
            {
                case DataTypeId.Money: return 0;
                case DataTypeId.Number: return 1;
                case DataTypeId.Datetime: return 2;
                case DataTypeId.LongText: return 3;
                case DataTypeId.ShortText: return 4;
                case DataTypeId.Entity: return 5;
                case DataTypeId.Status: return 6;
            }
            return null;
        }

    }
}
