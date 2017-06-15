using AutoMapper;
using UserDomain.DomainLayer.Entities;

namespace MVCWebUIComponent.Models.Translates
{
    public class UserDomainMVCProfile : Profile
    {
        public override string ProfileName
        {
            get { return "UserDomainMVCProfile"; }
        }
        protected override void Configure()
        {
            Mapper.CreateMap<InvitationCodeModel, InvitationCode>();

            Mapper.CreateMap<InvitationCode, InvitationCodeModel>();
        }

    }
}