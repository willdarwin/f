using System.Collections.Generic;
using UserDomain.DomainLayer.Entities;

namespace UserDomain.DomainLayer.RepositoryInterfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        User RegisterUser(User user);

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        User ValidateUser(User user);

        /// <summary>
        /// Generates the invitation code.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        bool GenerateInvitationCode(int count);

        bool CheckInvitationCode(string code);

        List<InvitationCode> GetAllInvitationCode();
    }
}
