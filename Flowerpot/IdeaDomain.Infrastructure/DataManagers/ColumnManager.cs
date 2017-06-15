using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class ColumnManager
    {
        private IDataAccess _dataAccess;

        public ColumnManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Gets the columnns by idea.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        public List<ColumnDE> GetColumnnsByIdea(SqlCommand command, int ideaId)
        {
            var columnList = new List<ColumnDE>();
            const string querysqlColumn = "SELECT * FROM [ColumnInIdea] WHERE IdeaId = @ideaId and IsDeleted = 0";
            var paramListColumn = new QueryParameter[1];
            paramListColumn[0] = new QueryParameter("@ideaId", ideaId, DbType.Int32);
            var columnTable = _dataAccess.GetTable(command, querysqlColumn, paramListColumn);
            if (columnTable.Rows.Count <= 0) return columnList;
            columnList.AddRange(from DataRow row in columnTable.Rows
                                select new ColumnDE()
                                {
                                    ColumnId = Convert.ToInt32(row["ColumnId"]),
                                    ColumnName = Convert.ToString(row["ColumnName"]),
                                    DataTypeId = Convert.ToInt32(row["DataTypeId"]),
                                    ReferedIdeaId = Convert.ToInt32(row["ReferedIdeaId"]),
                                    CreateTime = Convert.ToDateTime(row["CreateTime"]),
                                    IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                                    IdeaId = Convert.ToInt32(row["IdeaId"])
                                });
            return columnList;
        }

        /// <summary>
        /// Gets the column by id
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        public ColumnDE GetColumnById(SqlCommand command, int columnId)
        {
            const string querysqlColumn = "SELECT * FROM [ColumnInIdea] WHERE ColumnId = @columnId and IsDeleted = 0";
            var paramListColumn = new QueryParameter[1];
            paramListColumn[0] = new QueryParameter("@columnId", columnId, DbType.Int32);
            var columnTable = _dataAccess.GetTable(command, querysqlColumn, paramListColumn);
            if (columnTable.Rows.Count <= 0) return null;
            var row = columnTable.Rows[0];
            var columnDe = new ColumnDE()
            {
                ColumnId = Convert.ToInt32(row["ColumnId"]),
                ColumnName = Convert.ToString(row["ColumnName"]),
                DataTypeId = Convert.ToInt32(row["DataTypeId"]),
                ReferedIdeaId = Convert.ToInt32(row["ReferedIdeaId"]),
                CreateTime = Convert.ToDateTime(row["CreateTime"]),
                IsDeleted = Convert.ToBoolean(row["IsDeleted"]),
                IdeaId = Convert.ToInt32(row["IdeaId"])
            };
            return columnDe;
        }

        /// <summary>
        /// Create an ColumnDE entity.
        /// </summary>
        /// <param name="column"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        public int CreateColumn(SqlCommand command, ColumnDE columnDe)
        {
            const string insertsqlColumn = "INSERT INTO [ColumnInIdea] (ColumnName,DataTypeId,ReferedIdeaId,CreateTime,IsDeleted,IdeaId) VALUES (@columnName, @typeId, @referedIdeaId, @createTime, @isDeleted, @ideaId);select scope_identity()";
            var paramListColumn = new QueryParameter[6];
            paramListColumn[0] = new QueryParameter("@columnName", columnDe.ColumnName, DbType.String);
            paramListColumn[1] = new QueryParameter("@typeId", columnDe.DataTypeId, DbType.Int32);
            if (columnDe.DataTypeId == 5)
            {
                paramListColumn[2] = new QueryParameter("@referedIdeaId", columnDe.ReferedIdeaId, DbType.Int32);
            }
            else
                paramListColumn[2] = new QueryParameter("@referedIdeaId", 0, DbType.Int32);
            paramListColumn[3] = new QueryParameter("@createTime", columnDe.CreateTime, DbType.DateTime);
            paramListColumn[4] = new QueryParameter("@isdeleted", columnDe.IsDeleted, DbType.Boolean);
            paramListColumn[5] = new QueryParameter("@ideaId", columnDe.IdeaId, DbType.Int32);
            return Convert.ToInt32(_dataAccess.ExecuteScalar(command, insertsqlColumn, paramListColumn));
        }

        /// <summary>
        /// Updates the column_ new.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="columnDe">The column DE.</param>
        /// <returns></returns>
        public bool UpdateColumn(SqlCommand command, ColumnDE columnDe)
        {
            var updatesqlColumn = "UPDATE [ColumnInIdea] SET ColumnName = @columnName, DataTypeId = @dataTypeId, ReferedIdeaId = @referedIdeaId, CreateTime = CONVERT(varchar(100), GETDATE(), 20) WHERE ColumnId = @columnId";
            var paramListColumn = new QueryParameter[4];
            paramListColumn[0] = new QueryParameter("@columnName", columnDe.ColumnName, DbType.String);
            paramListColumn[1] = new QueryParameter("@dataTypeId", columnDe.DataTypeId, DbType.Int32);
            if (columnDe.DataTypeId == 5)
                paramListColumn[2] = new QueryParameter("@referedIdeaId", columnDe.ReferedIdeaId, DbType.Int32);
            else
                paramListColumn[2] = new QueryParameter("@referedIdeaId", 0, DbType.Int32);
            paramListColumn[3] = new QueryParameter("@columnId", columnDe.ColumnId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, updatesqlColumn, paramListColumn));
        }

        /// <summary>
        /// Delete a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public bool DeleteColumnById(SqlCommand command, int columnId)
        {
            const string deletesqlColumn = "UPDATE [ColumnInIdea] SET IsDeleted = 1 WHERE ColumnId = @columnId";
            var paramListColumn = new QueryParameter[1];
            paramListColumn[0] = new QueryParameter("@columnId", columnId, DbType.Int32);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, deletesqlColumn, paramListColumn));
        }

        /// <summary>
        /// Checks the reference.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="referedIdeaIdId">The r idea type id.</param>
        /// <returns></returns>
        public bool CheckReference(SqlCommand command, int referedIdeaIdId)
        {
            const string checksqlColumn = "select * from ColumnInIdea C join Idea I on I.IsDeleted=0 and I.IdeaId = C.IdeaId where C.ReferedIdeaId = @referedIdeaIdId";
            var paramListColumn = new QueryParameter[1];
            paramListColumn[0] = new QueryParameter("@referedIdeaIdId", referedIdeaIdId, DbType.Int32);
            var resultTable = _dataAccess.GetTable(command, checksqlColumn, paramListColumn);
            return resultTable.Rows.Count > 0;
        }

        /// <summary>
        /// Gets the ref columns by column id.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        public DataTable GetRefColumnsByColumnId(SqlCommand command, int columnId)
        {
            const string sql = "select ReferedColumnId from ColumnInReference where ColumnId = @columnId";
            var paramListColumn = new QueryParameter[1];
            paramListColumn[0] = new QueryParameter("@columnId", columnId, DbType.Int32);
            var resultTable = _dataAccess.GetTable(command, sql, paramListColumn);
            return resultTable;

        }

        /// <summary>
        /// Insert referenced columns' id to be column's displayed tag.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="columnId">The column id.</param>
        /// <param name="refColumnIds">The reference columns' id.</param>
        public void InsertRefColumn(SqlCommand command, int columnId, int refColumnId)
        {
            const string insertsqlRefcolumn = "insert into ColumnInReference values(@ColumnId,@ReferedColumnId)";
            var paramListRefcolumn = new QueryParameter[2];
            paramListRefcolumn[0] = new QueryParameter("@ColumnId", columnId, DbType.Int32);
            paramListRefcolumn[1] = new QueryParameter("@ReferedColumnId", refColumnId, DbType.Int32);
            _dataAccess.ExecuteScalar(command, insertsqlRefcolumn, paramListRefcolumn);
        }

    }
}
