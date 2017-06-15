using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class AnalyzerManager
    {
        private IDataAccess _dataAccess;

        public AnalyzerManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Gets the analyzers by userId.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<AnalyzerDE> GetAnalyzersByUserId(SqlCommand command, int userId)
        {
            const string querysqlAnalyzer = "SELECT * FROM [Analyzer] WHERE UserId = @userId and IsDeleted = 0";
            var paramListAnalyzer = new QueryParameter[1];
            paramListAnalyzer[0] = new QueryParameter("@userId", userId, DbType.Int32);
            var analyzersFromTable = _dataAccess.GetTable(command, querysqlAnalyzer, paramListAnalyzer);
            if (analyzersFromTable.Rows.Count <= 0) return null;
            return (from DataRow row in analyzersFromTable.Rows
                    select new AnalyzerDE()
                    {
                        AnalyzerId = Convert.ToInt32(row["AnalyzerId"]),
                        AnalyzerName = Convert.ToString(row["AnalyzerName"]),
                        CreateTime = Convert.ToDateTime(row["CreateTime"]),
                        IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                        UserId = Convert.ToInt32(row["UserId"]),
                        SelectQuery = Convert.ToString(row["SelectQuery"]),
                        JoinQuery = Convert.ToString(row["JoinQuery"]),
                        WhereQuery = Convert.ToString(row["WhereQuery"])
                    }).ToList();
        }

        /// <summary>
        /// Gets the analyzer by id.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public AnalyzerDE GetAnalyzerById(SqlCommand command, int analyzerId)
        {
            const string querysqlAnalyzer = "SELECT * FROM [Analyzer] WHERE AnalyzerId = @analyzerId and IsDeleted = 0";
            var paramListAnalyzer = new QueryParameter[1];
            paramListAnalyzer[0] = new QueryParameter("@analyzerId", analyzerId, DbType.Int32);
            var analyzersFromTable = _dataAccess.GetTable(command, querysqlAnalyzer, paramListAnalyzer);
            if (analyzersFromTable.Rows.Count <= 0) return null;
            var row = analyzersFromTable.Rows[0];
            var analyzerDe = new AnalyzerDE()
            {
                AnalyzerId = Convert.ToInt32(row["AnalyzerId"]),
                AnalyzerName = Convert.ToString(row["AnalyzerName"]),
                CreateTime = Convert.ToDateTime(row["CreateTime"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                UserId = Convert.ToInt32(row["UserId"]),
                SelectQuery = Convert.ToString(row["SelectQuery"]),
                JoinQuery = Convert.ToString(row["JoinQuery"]),
                WhereQuery = Convert.ToString(row["WhereQuery"])
            };
            return analyzerDe;
        }

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="analyzerDe">The analyzer DE.</param>
        /// <returns></returns>
        public int CreateAnalyzer(SqlCommand command, AnalyzerDE analyzerDe)
        {
            const string insertsqlAnalyzer = "INSERT INTO [Analyzer] (AnalyzerName,CreateTime,UserId,SelectQuery,JoinQuery,WhereQuery,IsDeleted) VALUES (@analyzerName, @createTime, @userId,@selectQuery,@joinQuery,@whereQuery, @isDeleted);select scope_identity()";
            var paramListAnalyzer = new QueryParameter[7];
            paramListAnalyzer[0] = new QueryParameter("@analyzerName", analyzerDe.AnalyzerName, DbType.String);
            paramListAnalyzer[1] = new QueryParameter("@createTime", analyzerDe.CreateTime, DbType.DateTime);
            paramListAnalyzer[2] = new QueryParameter("@userId", analyzerDe.UserId, DbType.Int32);
            paramListAnalyzer[3] = new QueryParameter("@selectQuery", analyzerDe.SelectQuery, DbType.String);
            paramListAnalyzer[4] = new QueryParameter("@joinQuery", analyzerDe.JoinQuery, DbType.String);
            paramListAnalyzer[5] = new QueryParameter("@whereQuery", analyzerDe.WhereQuery, DbType.String);
            paramListAnalyzer[6] = new QueryParameter("@isDeleted", analyzerDe.IsDeleted, DbType.Boolean);
            return Convert.ToInt32(_dataAccess.ExecuteScalar(command, insertsqlAnalyzer, paramListAnalyzer));
        }

        /// <summary>
        /// Updates the analyzer.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="analyzerDe">The analyzer DE.</param>
        /// <returns></returns>
        public bool UpdateAnalyzer(SqlCommand command, AnalyzerDE analyzerDe)
        {
            const string updatesqlAnalyzer = "UPDATE [Analyzer] SET AnalyzerName=@analyzerName, SelectQuery=@selectQuery, JoinQuery=@joinQuery, WhereQuery=@whereQuery WHERE AnalyzerId = @analyzerId";
            var paramListAnalyzer = new QueryParameter[5];
            paramListAnalyzer[0] = new QueryParameter("@analyzerName", analyzerDe.AnalyzerName, DbType.String);
            paramListAnalyzer[1] = new QueryParameter("@selectQuery", analyzerDe.SelectQuery, DbType.String);
            paramListAnalyzer[2] = new QueryParameter("@joinQuery", analyzerDe.JoinQuery, DbType.String);
            paramListAnalyzer[3] = new QueryParameter("@whereQuery", analyzerDe.WhereQuery, DbType.String);
            paramListAnalyzer[4] = new QueryParameter("@analyzerId", analyzerDe.AnalyzerId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, updatesqlAnalyzer, paramListAnalyzer));
        }

        /// <summary>
        /// Deletes the analyzer by id.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public bool DeleteAnalyzerById(SqlCommand command, int analyzerId)
        {
            const string deletesqlAnalyzer = "UPDATE [Analyzer] SET IsDeleted = 1 WHERE AnalyzerId = @analyzerId";
            var paramListAnalyzer = new QueryParameter[1];
            paramListAnalyzer[0] = new QueryParameter("@analyzerId", analyzerId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, deletesqlAnalyzer, paramListAnalyzer));
        }

        /// <summary>
        /// Gets the analyzer data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="analyzerDE">The analyzer DE.</param>
        /// <returns></returns>
        public DataTable GetAnalyzerData(SqlCommand command, string selectQuery)
        {
            var rowTable = _dataAccess.GetTable(command, selectQuery, null);
            return rowTable;
        }

    }
}
