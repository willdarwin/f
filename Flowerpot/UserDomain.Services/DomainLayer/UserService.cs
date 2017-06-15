using System.Collections.Generic;
using GeneralUtilities.Utilities.Unity;
using UserDomain.DomainLayer.Entities;
using UserDomain.DomainLayer.RepositoryInterfaces;
using UserDomain.ServiceLayer;


namespace UserDomain.DomainLayer
{
    public class UserService : IUserService
    {
        /// <summary>
        /// Gets or sets the user repository.
        /// </summary>
        /// <value>
        /// The user repository.
        /// </value>
        public IUserRepository UserRepository { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        public UserService()
        {
            UserRepository = Container.Resolve<IUserRepository>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="userRepository">The user repository.</param>
        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public User RegisterUser(User user)
        {
            return UserRepository.RegisterUser(user);
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public User ValidateUser(User user)
        {
            return UserRepository.ValidateUser(user);
        }

        /// <summary>
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        public bool ChangePassword(string userName, string oldPassword, string newPassword)
        {
            return true;
        }

        /// <summary>
        /// Invitations the code generate.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        public bool InvitationCodeGenerate(int number)
        {
            var result = false;
            result = UserRepository.GenerateInvitationCode(number);
            return result;
        }

        public bool CheckInvitationCode(string code)
        {
            var result = false;
            result = UserRepository.CheckInvitationCode(code);
            return result;
        }

        public List<InvitationCode> GetAllInvitationCode()
        {
            var list = UserRepository.GetAllInvitationCode();
            return list;
        }


        public User GetUserByName(string name)
        {
            throw new System.NotImplementedException();
        }

        public User GetUserById(int id)
        {
            throw new System.NotImplementedException();
        }


        public Role GetRoleByUserId(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
