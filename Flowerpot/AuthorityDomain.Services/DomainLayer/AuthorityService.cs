using AuthorityDomain.ServiceLayer;
using AuthorityDomain.DomainLayer.Entities;
using System;
using AuthorityDomain.DomainLayer.RepositoryInterfaces;
using GeneralUtilities.Utilities.Unity;

namespace AuthorityDomain.DomainLayer
{
    public class AuthorityService : IAuthorityService
    {
        public IAuthorityRepository MyAuthorityRepository { get; set; }

        public AuthorityService()
        {
            MyAuthorityRepository = Container.Resolve<IAuthorityRepository>();
        }

        public AuthorityService(IAuthorityRepository authorityRepository)
        {
            MyAuthorityRepository = authorityRepository;
        }

        public Authority FindController(string controllerName)
        {
            return MyAuthorityRepository.FindController(controllerName);
        }

        public Authority FindAction(string controllerName, string actionName)
        {
            return MyAuthorityRepository.FindAction(controllerName, actionName);
        }

        public bool IsAccessible(int roleId, int authorityId)
        {
            return MyAuthorityRepository.IsAccssible(roleId, authorityId);
        }
    }
}
