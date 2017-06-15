
namespace UserDomain.DomainLayer.Entities
{
    public class Role
    {
        public Role()
        {
            IsDeleted = false;
        }

        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDescription { get; set; }

        public bool IsDeleted { get; set; }
    }
}
