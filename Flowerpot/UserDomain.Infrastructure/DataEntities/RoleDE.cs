
namespace UserDomain.InfrastructureLayer.DataEntities
{
    public class RoleDE
    {
        public RoleDE()
        {
            IsDeleted = false;
        }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool IsDeleted { get; set; }
    }
}

