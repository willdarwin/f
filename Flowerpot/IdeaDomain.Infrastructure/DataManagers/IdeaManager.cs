using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class IdeaManager
    {
        private IDataAccess _dataAccess;

        public IdeaManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Get ideas with columns by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public List<IdeaDE> GetIdeasByUser(SqlCommand command, int userId)
        {
            var ideaDeList = new List<IdeaDE>();
            const string querysqlIdea = "SELECT * FROM [Idea] WHERE UserId = @userId and IsDeleted = 0";
            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@userId", userId, DbType.Int32);
            var ideasFromTable = _dataAccess.GetTable(command, querysqlIdea, paramListIdea);
            if (ideasFromTable.Rows.Count <= 0) return null;
            ideaDeList.AddRange(from DataRow row in ideasFromTable.Rows
                select new IdeaDE()
                {
                    IdeaId = Convert.ToInt32(row["IdeaId"]), IdeaName = Convert.ToString(row["IdeaName"]), IdeaDescription = Convert.ToString(row["IdeaDescription"]), CreateTime = Convert.ToDateTime(row["CreateTime"]), IsDeleted = Convert.ToBoolean(row["IsDeleted"]), UserId = Convert.ToInt32(row["UserId"])
                });
            return ideaDeList;
        }

        /// <summary>
        /// Get an IdeaDE entity by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public IdeaDE GetIdeaById(SqlCommand command, int ideaId)
        {
            const string querysqlIdea = "SELECT * FROM [Idea] WHERE IdeaId = @ideaId and IsDeleted = 0";
            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@ideaId", ideaId, DbType.Int32);
            var ideasFromTable = _dataAccess.GetTable(command, querysqlIdea, paramListIdea);
            if (ideasFromTable.Rows.Count <= 0) return null;
            var row = ideasFromTable.Rows[0];
            var ideaDe = new IdeaDE()
            {
                IdeaId = Convert.ToInt32(row["IdeaId"]),
                IdeaName = Convert.ToString(row["IdeaName"]),
                IdeaDescription = Convert.ToString(row["IdeaDescription"]),
                CreateTime = Convert.ToDateTime(row["CreateTime"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                UserId = Convert.ToInt32(row["UserId"]),
            };
            return ideaDe;
        }

        /// <summary>
        /// Get an IdeaDE entity by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public IdeaDE GetIdeaByName(SqlCommand command, string ideaName)
        {
            var querysqlIdea = "SELECT * FROM [Idea] WHERE IdeaName = @IdeaName and IsDeleted = 0";
            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@IdeaName", ideaName, DbType.String);
            var ideasFromTable = _dataAccess.GetTable(command, querysqlIdea, paramListIdea);
            if (ideasFromTable.Rows.Count <= 0) return null;
            var row = ideasFromTable.Rows[0];
            var ideaDe = new IdeaDE()
            {
                IdeaId = Convert.ToInt32(row["IdeaId"]),
                IdeaName = Convert.ToString(row["IdeaName"]),
                IdeaDescription = Convert.ToString(row["IdeaDescription"]),
                CreateTime = Convert.ToDateTime(row["CreateTime"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                UserId = Convert.ToInt32(row["UserId"]),
            };
            return ideaDe;
        }

        /// <summary>
        /// Create an IdeaDE entity.
        /// </summary>
        /// <param name="ideaDe"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public int CreateIdea(SqlCommand command, IdeaDE ideaDe)
        {
            const string insertsqlIdea = "INSERT INTO [Idea] (IdeaName,IdeaDescription,CreateTime,IsDeleted,UserId) VALUES (@ideaName, @ideaDescription, @createTime, @isDeleted, @uid);select scope_identity()";
            var paramListIdea = new QueryParameter[5];
            paramListIdea[0] = new QueryParameter("@ideaName", ideaDe.IdeaName, DbType.String);
            paramListIdea[1] = new QueryParameter("@ideaDescription", ideaDe.IdeaDescription, DbType.String);
            paramListIdea[2] = new QueryParameter("@createTime", ideaDe.CreateTime, DbType.DateTime);
            paramListIdea[3] = new QueryParameter("@isdeleted", ideaDe.IsDeleted, DbType.Boolean);
            paramListIdea[4] = new QueryParameter("@uid", ideaDe.UserId, DbType.Int32);
            return Convert.ToInt32(_dataAccess.ExecuteScalar(command, insertsqlIdea, paramListIdea));
        }

        /// <summary>
        /// Update an ideaDE entity.
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="command"></param>
        /// <param name="ideaDe"></param>
        /// <returns></returns>
        public bool UpdateIdea(SqlCommand command, IdeaDE ideaDe)
        {
            const string updatesqlIdea = "UPDATE [Idea] SET IdeaName = @ideaName, IdeaDescription = @ideaDescription WHERE IdeaId = @ideaId";

            var paramListIdea = new QueryParameter[3];
            paramListIdea[0] = new QueryParameter("@ideaName", ideaDe.IdeaName, DbType.String);
            paramListIdea[1] = new QueryParameter("@ideaDescription", ideaDe.IdeaDescription, DbType.String);
            paramListIdea[2] = new QueryParameter("@ideaId", ideaDe.IdeaId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, updatesqlIdea, paramListIdea));
        }

        /// <summary>
        /// Delete an idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public bool DeleteIdeaById(SqlCommand command, int ideaId)
        {
            const string deletesqlIdea = "UPDATE [Idea] SET IsDeleted = 1 WHERE IdeaId = @ideaId";
            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@ideaId", ideaId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, deletesqlIdea, paramListIdea));
        }
    }
}
