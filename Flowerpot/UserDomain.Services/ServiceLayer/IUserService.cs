using UserDomain.DomainLayer.Entities;
using UserDomain.DomainLayer.Entities;
using System.Collections.Generic;


namespace UserDomain.ServiceLayer
{
    /// <summary>
    /// Service interface for UserDomain
    /// </summary>
    public interface IUserService
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
        /// Changes the password.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="oldPassword">The old password.</param>
        /// <param name="newPassword">The new password.</param>
        /// <returns></returns>
        bool ChangePassword(string userName, string oldPassword, string newPassword);

        /// <summary>
        /// Invitations the code generate.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns></returns>
        bool InvitationCodeGenerate(int number);

        bool CheckInvitationCode(string code);

        List<InvitationCode> GetAllInvitationCode();

        User GetUserByName(string name);

        User GetUserById(int id);

        Role GetRoleByUserId(int id);
    }
}
