
namespace UserDomain.DomainLayer.Entities
{
    public class User
    {
        public User()
        {
            IsDeleted = false;
        }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public bool IsDeleted { get; set; }

        public UserStatus Status { get; set; }
    }

    public enum UserStatus
    {
        Success = 1,
        EmailRegistered = 2,
        UserNameRegistered = 3,
        Failied = 4
    }
}
