using System;
using System.Collections.Generic;
using System.Text;

namespace UserDomain.DomainLayer.Entities
{
    public class InvitationCode
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool Obsolete { get; set; }

        public int UserId { get; set; }
    }
}
