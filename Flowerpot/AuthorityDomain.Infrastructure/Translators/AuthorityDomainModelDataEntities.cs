using System;
using System.Collections.Generic;
using System.Text;
using UtilityComponent.AutoMapper;
using AuthorityDomain.Infrastructure.DataEntities;
using AuthorityDomain.DomainLayer.Entities;

namespace AuthorityDomain.Infrastructure.Translators
{
    public class AuthorityDomainModelDataEntities : AutoMapperWrapper
    {
        public AuthorityDomainModelDataEntities()
        {
            Initialize(cfg =>
            {
                cfg.CreateMap<AuthorityDE, Authority>();
                cfg.CreateMap<Authority, AuthorityDE>();
            });
        }
    }
}
