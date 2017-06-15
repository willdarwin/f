using UserDomain.DomainLayer.Entities;
using UserDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.AutoMapper;
using UserDomain.Infrastructure.DataEntities;

namespace UserDomain.InfrastructureLayer.Translator
{
    public class UserDomainModelDataEntities : AutoMapperWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserDomainModelDataEntities"/> class.
        /// </summary>
        public UserDomainModelDataEntities()
        {
            Initialize(cfg =>
            {
                cfg.CreateMap<UserDE, User>();
                cfg.CreateMap<RoleDE, Role>();
                cfg.CreateMap<InvitationCodeDE, InvitationCode>();

                cfg.CreateMap<User, UserDE>();
                cfg.CreateMap<Role, RoleDE>();
                cfg.CreateMap<InvitationCode, InvitationCodeDE>();
            });
        }
    }
}