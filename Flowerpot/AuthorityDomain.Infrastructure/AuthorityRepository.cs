using System;
using System.Collections.Generic;
using System.Text;
using AuthorityDomain.Infrastructure.DataManagers;
using AuthorityDomain.DomainLayer.RepositoryInterfaces;
using AuthorityDomain.DomainLayer.Entities;
using AuthorityDomain.Infrastructure.DataEntities;
using AuthorityDomain.Infrastructure.Translators;

namespace AuthorityDomain.InfrastructureLayer.Repositories
{
    public class AuthorityRepository : IAuthorityRepository
    {

        private readonly AuthorityManager _authorityManager;
        private readonly AuthorityDomainModelDataEntities _mapper = new AuthorityDomainModelDataEntities();

        public AuthorityRepository()
        {
            _authorityManager = new AuthorityManager();
        }

        public bool IsAccssible(int roleId, int authoriyId)
        {
            var result = false;
            result = _authorityManager.IsAllowed(roleId, authoriyId);
            return result;
        }

        public Authority FindController(string controllerName)
        {
            var authorityDE = _authorityManager.FindController(controllerName);
            return _mapper.Map<AuthorityDE, Authority>(authorityDE);
        }

        public Authority FindAction(string controllerName, string actionName)
        {
            var authorityDE = _authorityManager.FindAction(controllerName, actionName);
            return _mapper.Map<AuthorityDE, Authority>(authorityDE);
        }
    }
}
