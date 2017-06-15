using UserDomain.DomainLayer.Entities;
using AuthorityDomain.DomainLayer.Entities;
using System.Collections.Generic;

namespace AuthorityDomain.ServiceLayer
{
    public interface IAuthorityService
    {
        Authority FindController(string controllerName);

        Authority FindAction(string controllerName, string actionName);

        bool IsAccessible(int roleId, int authorityId);
    }
}
