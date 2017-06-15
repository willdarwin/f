using System;
using System.Data;
using System.Data.SqlClient;
using UserDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;

namespace UserDomain.InfrastructureLayer.DataManagers
{
    public class UserManager
    {
        private IDataAccess _dataAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        public UserManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Creates the user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userDE">The user DE.</param>
        /// <returns></returns>
        public int CreateUser(SqlCommand command, UserDE userDE)
        {
            var sqlInsertUser = "INSERT INTO [User] ([UserName],[Password],[Email],[IsDeleted]) VALUES (@username,@userpassword,@email,@isDeleted);select scope_identity()";
            var paramList_User = new QueryParameter[4];
            paramList_User[0] = new QueryParameter("@username", userDE.UserName, DbType.String);
            paramList_User[1] = new QueryParameter("@userpassword", userDE.Password, DbType.String);
            paramList_User[2] = new QueryParameter("@email", userDE.Email, DbType.String);
            paramList_User[3] = new QueryParameter("@isDeleted", userDE.IsDeleted, DbType.String);
            return Convert.ToInt32(_dataAccess.ExecuteScalar(command, sqlInsertUser, paramList_User));
        }

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userDE">The user DE.</param>
        /// <returns></returns>
        public UserDE GetUserByEmail(SqlCommand command, UserDE userDE)
        {
            var sqlGettUser = "select * from [User] u Where u.Email =@email and u.IsDeleted = 0";
            var param_Email = new QueryParameter[1];
            param_Email[0] = new QueryParameter("@email", userDE.Email, DbType.String);
            var userTable = _dataAccess.GetTable(command, sqlGettUser, param_Email);
            if (userTable.Rows.Count > 0)
            {
                var row = userTable.Rows[0];
                userDE.UserId = Convert.ToInt32(row["UserId"]);
                userDE.UserName = Convert.ToString(row["UserName"]);
                userDE.Email = Convert.ToString(row["Email"]);
                userDE.Password = Convert.ToString(row["Password"]);
                userDE.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            }
            return userDE;
        }

        /// <summary>
        /// Gets the name of the user by user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userDE">The user DE.</param>
        /// <returns></returns>
        public UserDE GetUserByUserName(SqlCommand command, UserDE userDE)
        {
            var sqlGettUser = "select * from [User] u Where u.UserName =@userName and u.IsDeleted = 0";
            var param_Email = new QueryParameter[1];
            param_Email[0] = new QueryParameter("@userName", userDE.UserName, DbType.String);
            var userTable = _dataAccess.GetTable(command, sqlGettUser, param_Email);
            if (userTable.Rows.Count > 0)
            {
                var row = userTable.Rows[0];
                userDE.UserId = Convert.ToInt32(row["UserId"]);
                userDE.UserName = Convert.ToString(row["UserName"]);
                userDE.Email = Convert.ToString(row["Email"]);
                userDE.Password = Convert.ToString(row["Password"]);
                userDE.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            }
            return userDE;
        }

        /// <summary>
        /// Gets the user by user id.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userDE">The user DE.</param>
        /// <returns></returns>
        public UserDE GetUserByUserId(SqlCommand command, UserDE userDE)
        {
            var sqlGetUser = "select * from [User] u Where u.UserId =@userId and u.IsDeleted = 0";
            var param_UserId = new QueryParameter[1];
            param_UserId[0] = new QueryParameter("@userId", userDE.UserId, DbType.String);
            var userTable = _dataAccess.GetTable(command, sqlGetUser, param_UserId);
            if (userTable.Rows.Count > 0)
            {
                var row = userTable.Rows[0];
                userDE.UserId = Convert.ToInt32(row["UserId"]);
                userDE.UserName = Convert.ToString(row["UserName"]);
                userDE.Email = Convert.ToString(row["Email"]);
                userDE.Password = Convert.ToString(row["Password"]);
                userDE.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            }
            return userDE;
        }

        /// <summary>
        /// Creates the user tables.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userid">The userid.</param>
        /// <returns></returns>
        public int CreateUserTables(SqlCommand command, int userid)
        {
            var createUserTables =
                "CREATE TABLE [U" + userid + "_Row]([RowId] [int] IDENTITY(1,1) PRIMARY KEY,[IdeaId] [int] FOREIGN KEY references Idea(IdeaId) NOT NULL,[Version] [int] NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_Money]([MoneyId] [int] IDENTITY(1,1) PRIMARY KEY,[Value] [decimal](18,2) ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_Number]([NumberId] [int] IDENTITY(1,1) PRIMARY KEY,[Value] [decimal](18,2)  ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_Datetime]([DatetimeId] [int] IDENTITY(1,1)  PRIMARY KEY,[Value] [datetime] ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_ShortText]([ShortTextId] [int] IDENTITY(1,1) PRIMARY KEY,[Value] [nvarchar](500) ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_LongText]([LongTextId] [int] IDENTITY(1,1) PRIMARY KEY,[Value] [nvarchar](MAX) ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);" +
            "CREATE TABLE [U" + userid + "_ComplexType]([ComplexTypeId] [int] IDENTITY(1,1) PRIMARY KEY,[RefRowId] [int] ,[ColumnId] [int] FOREIGN KEY references ColumnInIdea(ColumnId) NOT NULL,[RowId] [int] FOREIGN KEY references [U" + userid + "_Row](RowId) NOT NULL,[IsDeleted] [bit] NOT NULL);";

            return _dataAccess.ExecuteNonQuery(command, createUserTables, null);

        }

        /// <summary>
        /// Validates the user.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userDE">The user DE.</param>
        /// <returns></returns>
        public UserDE ValidateUser(SqlCommand command, UserDE userDE)
        {
            var sqlValidateUser = "select * from [User] u Where u.UserName = @userName and u.Password = @userPassword and IsDeleted=0";
            var list = new QueryParameter[2];
            list[0] = new QueryParameter("@userName", userDE.UserName, DbType.String);
            list[1] = new QueryParameter("@userPassword", userDE.Password, DbType.String);
            var userTable = _dataAccess.GetTable(command, sqlValidateUser, list);
            if (userTable.Rows.Count > 0)
            {
                var row = userTable.Rows[0];
                userDE.UserId = Convert.ToInt32(row["UserId"]);
                userDE.UserName = Convert.ToString(row["UserName"]);
                userDE.Email = Convert.ToString(row["Email"]);
                userDE.Password = Convert.ToString(row["Password"]);
                userDE.IsDeleted = Convert.ToBoolean(row["IsDeleted"]);
            }
            return userDE;
        }
    }
}
