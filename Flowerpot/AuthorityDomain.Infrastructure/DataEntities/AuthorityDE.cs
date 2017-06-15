using System;
using System.Collections.Generic;
using System.Text;

namespace AuthorityDomain.Infrastructure.DataEntities
{
    public class AuthorityDE
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsController { get; set; }

        public bool IsAllowedNoneRoles { get; set; }

        public string ControllerName { get; set; }

        public bool IsAllowedAllRoles { get; set; }
    }
}
