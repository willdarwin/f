using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using UserDomain.DomainLayer.Entities;
using UserDomain.DomainLayer.RepositoryInterfaces;
using UserDomain.Infrastructure.DataEntities;
using UserDomain.Infrastructure.DataManagers;
using UserDomain.InfrastructureLayer.DataEntities;
using UserDomain.InfrastructureLayer.DataManagers;
using UserDomain.InfrastructureLayer.Translator;
using UtilityComponent.DataFactory;

namespace UserDomain.InfrastructureLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        /// <summary>
        /// Initialize a data access interface
        /// </summary>
        private IDataAccess _dataAccess = DataAccessFactory.CreateDataAccess();
        private UserManager _userManager;
        private InvitationCodeManager _invitationCodeManager;
        private readonly UserDomainModelDataEntities _mapper = new UserDomainModelDataEntities();

        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        public UserRepository()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
            _userManager = new UserManager();
            _invitationCodeManager = new InvitationCodeManager();
        }

        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public User RegisterUser(User user)
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                try
                {
                    if (CheckEmail(user))
                    {
                        user.Status = UserStatus.EmailRegistered;
                        connection.Close();
                        return user;
                    }
                    else if (CheckName(user))
                    {
                        user.Status = UserStatus.UserNameRegistered;
                        connection.Close();
                        return user;
                    }
                    user.UserId = _userManager.CreateUser(cmd, _mapper.Map<UserDE>(user));
                    _userManager.CreateUserTables(cmd, user.UserId);
                    sqltrans.Commit();

                }
                catch (Exception ex)
                {
                    sqltrans.Rollback();
                    user.Status = UserStatus.Failied;
                    user.UserId = -1;
                }
                connection.Close();
            }
            user.Status = UserStatus.Success;
            return user;
        }

        /// <summary>
        /// Checks the name.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool CheckName(User user)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                result = _userManager.GetUserByUserName(cmd, _mapper.Map<UserDE>(user)).UserId > 0 ? true : false;
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Checks the email.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public bool CheckEmail(User user)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                result = _userManager.GetUserByEmail(cmd, _mapper.Map<UserDE>(user)).UserId > 0 ? true : false;
            }
            return result;
        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        public User ValidateUser(User user)
        {
            var userDE = new UserDE();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                try
                {
                    userDE = _userManager.ValidateUser(cmd, _mapper.Map<UserDE>(user));
                    user = _mapper.Map<User>(userDE);
                }
                catch (Exception ex)
                {
                }
                connection.Close();
            }
            return user;
        }

        /// <summary>
        /// Generates the invitation code.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public bool GenerateInvitationCode(int count)
        {
            var result = false;
            var r = _invitationCodeManager.Create(count);
            if (r)
            {
                result = true;
            }
            return result;
        }


        public bool CheckInvitationCode(string code)
        {
            var result = false;
            result = _invitationCodeManager.Check(code);
            return result;
        }

        public List<InvitationCode> GetAllInvitationCode()
        {
            var list = new List<InvitationCode>();
            var listDE = _invitationCodeManager.GetAllInvitationCode();
            for (var i = 0; i < listDE.Count; i++ )
            {
                var code = _mapper.Map<InvitationCodeDE, InvitationCode>(listDE[i]);
                list.Add(code);
            }
            return list;
        }
    }
}
