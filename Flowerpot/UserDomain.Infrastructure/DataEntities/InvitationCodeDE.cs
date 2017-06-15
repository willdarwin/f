using System;
using System.Collections.Generic;
using System.Text;

namespace UserDomain.Infrastructure.DataEntities
{
    public class InvitationCodeDE
    {
        public int Id { get; set; }

        public string Value { get; set; }

        public bool Obsolete { get; set; }

        public int UserId { get; set; }
    }
}
