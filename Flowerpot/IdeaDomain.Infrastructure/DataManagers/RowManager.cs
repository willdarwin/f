using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class RowManager
    {
        private IDataAccess _dataAccess;

        public RowManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Gets the rows by idea.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The command.</param>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public List<RowDE> GetRowsByIdea(SqlCommand command, IdeaDE idea)
        {
            var rowList = new List<RowDE>();
            var querysqlRow = "SELECT * FROM [U" + idea.UserId + "_Row] WHERE IdeaId = @ideaId and IsDeleted = 0";
            var paramListRow = new QueryParameter[1];
            paramListRow[0] = new QueryParameter("@ideaId", idea.IdeaId, DbType.Int32);
            var rowTable = _dataAccess.GetTable(command, querysqlRow, paramListRow);
            if (rowTable.Rows.Count > 0)
            {
                rowList.AddRange(from DataRow row in rowTable.Rows
                    select new RowDE()
                    {
                        RowId = Convert.ToInt32(row["RowId"]), IdeaId = Convert.ToInt32(row["IdeaId"]), Version = Convert.ToInt32(row["Version"]), IsDeleted = Convert.ToBoolean(row["IsDeleted"])
                    });
            }
            return rowList;
        }

        /// <summary>
        /// Get a RowDE entity by rowid and userid.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="rowId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RowDE GetRowById(SqlCommand command, int rowId, int userId)
        {
            var querysqlRow = "SELECT * FROM [U" + userId + "_Row] WHERE RowId = @rowId and IsDeleted = 0";
            var paramListRow = new QueryParameter[1];
            paramListRow[0] = new QueryParameter("@RowId", rowId, DbType.Int32);
            var rowTable = _dataAccess.GetTable(command, querysqlRow, paramListRow);
            if (rowTable.Rows.Count <= 0) return null;
            var row = rowTable.Rows[0];
            var rowDe = new RowDE()
            {
                RowId = Convert.ToInt32(row["RowId"]),
                IdeaId = Convert.ToInt32(row["IdeaId"]),
                Version = Convert.ToInt32(row["Version"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"])
            };
            return rowDe;
        }

        /// <summary>
        /// Create an RowDE entity.
        /// </summary>
        /// <param name="connection">The connection.</param>
        /// <param name="command">The command.</param>
        /// <param name="rowDE">The row DE.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public int AddRow(SqlCommand command, RowDE rowDE, int userId)
        {
            var insertsqlRow = "INSERT INTO [U" + userId + "_Row] (IdeaId,[Version],IsDeleted) VALUES ( @ideaId,@version,@isDeleted);select scope_identity()";
            var paramListRow = new QueryParameter[3];
            paramListRow[0] = new QueryParameter("@ideaId", rowDE.IdeaId, DbType.Int32);
            paramListRow[1] = new QueryParameter("@version", rowDE.Version, DbType.Int32);
            paramListRow[2] = new QueryParameter("@isdeleted", rowDE.IsDeleted, DbType.Boolean);
            return Convert.ToInt32(_dataAccess.ExecuteScalar(command, insertsqlRow, paramListRow));
        }

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="rowDE">The row DE.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public int UpdateRow(SqlCommand command, RowDE rowDE, int userID)
        {
            var result = 1;
            var sql = "UPDATE [U" + userID + "_Row] SET [Version] = @Version where RowId = @RowId";
            var paramList = new QueryParameter[2];
            paramList[0] = new QueryParameter("@Version", rowDE.Version, DbType.Int32);
            paramList[1] = new QueryParameter("@RowId", rowDE.RowId, DbType.Int32);
            result = Convert.ToInt32(_dataAccess.ExecuteScalar(command, sql, paramList));
            return result;
        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="rowDE">The row DE.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public bool DeleteRow(SqlCommand command, RowDE rowDE, int userID)
        {
            var sql = "UPDATE [U" + userID + "_Row] SET IsDeleted = 1 WHERE RowId = @rowId";
            var paramListRow = new QueryParameter[1];
            paramListRow[0] = new QueryParameter("@rowId", rowDE.RowId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, sql, paramListRow));
        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="rowID">The row ID.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public bool DeleteRow(SqlCommand command, int rowID, int userID)
        {
            var sql = "UPDATE [U" + userID + "_Row] SET IsDeleted = 1 WHERE RowId = @rowId";
            var paramListRow = new QueryParameter[1];
            paramListRow[0] = new QueryParameter("@RowId", rowID, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, sql, paramListRow));
        }

    }
}
