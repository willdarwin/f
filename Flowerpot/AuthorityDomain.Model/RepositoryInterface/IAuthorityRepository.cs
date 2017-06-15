using System;
using System.Collections.Generic;
using System.Text;
using AuthorityDomain.DomainLayer.Entities;

namespace AuthorityDomain.DomainLayer.RepositoryInterfaces
{
    public interface IAuthorityRepository
    {
        bool IsAccssible(int roleId, int authorityId);

        Authority FindController(string controllerName);

        Authority FindAction(string controllerName, string actionName);
    }
}
