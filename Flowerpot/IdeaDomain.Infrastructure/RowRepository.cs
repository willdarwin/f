using System;
using System.Data.SqlClient;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.InfrastructureLayer.DataEntities;
using IdeaDomain.InfrastructureLayer.DataManagers;
using IdeaDomain.InfrastructureLayer.Translator;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.Repositories
{
    public class RowRepository : IRowRepository
    {
        private IDataAccess _dataAccess;
        private DataManager _dataManger;
        private RowManager _rowManager;
        private readonly IdeaDomainModelDataEntities _mapper = new IdeaDomainModelDataEntities();

        public RowRepository()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
            _dataManger = new DataManager();
            _rowManager = new RowManager();
        }

        /// <summary>
        /// Gets the row by id.
        /// </summary>
        /// <param name="rowId">The row id.</param>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public Row GetRowById(int rowId, int userId)
        {
            var row = new Row();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand {Connection = connection};
                row = _mapper.Map<Row>(_rowManager.GetRowById(cmd, rowId, userId));
                row.UserId = userId;
                connection.Close();
            }
            return row;
        }

        /// <summary>
        /// Adds the row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public bool AddRow(Row row)
        {
            var result = false;
            var connection = _dataAccess.CreateConnection();
            connection.Open();
            var sqltrans = connection.BeginTransaction();
            var cmd = new SqlCommand
            {
                Connection = connection,
                Transaction = sqltrans
            };
            try
            {
                var rowNumber = _rowManager.AddRow(cmd, _mapper.Map<RowDE>(row), row.UserId);
                for (var i = 0; i < row.Columns.Count; i++)
                {
                    var column = row.Columns[i];
                    var dataDe = new DataDE
                    {
                        RowId = rowNumber,
                        ColumnId = column.ColumnId,
                        IsDeleted = false,
                        Value = row.Values[i]
                    };
                    _dataManger.AddData(cmd, dataDe, row.UserId, column.DataTypeId);
                }
                sqltrans.Commit();
                result = true;
            }
            catch
            {
                sqltrans.Rollback();
                result = false;
            }
            finally
            {
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Updates the row.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <returns></returns>
        public bool UpdateRow(Row row)
        {
            var result = false;
            if (row == null || (row.Version != GetRowById(row.RowId, row.UserId).Version)) return false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand
                {
                    Connection = connection,
                    Transaction = sqltrans
                };
                try
                {
                    row.Version += 1;
                    _rowManager.UpdateRow(cmd, _mapper.Map<RowDE>(row), row.UserId);
                    for (var i = 0; i < row.Columns.Count; i++)
                    {
                        var column = row.Columns[i];
                        var dataDe = new DataDE
                        {
                            RowId = row.RowId,
                            ColumnId = column.ColumnId,
                            IsDeleted = false,
                            Value = row.Values[i]
                        };
                        if (!_dataManger.UpdateData(cmd, dataDe, row.UserId, column.DataTypeId))
                        {
                            _dataManger.AddData(cmd, dataDe, row.UserId, column.DataTypeId);
                        }

                    }
                    sqltrans.Commit();
                    result = true;
                }
                catch
                {
                    sqltrans.Rollback();
                    result = false;
                }
                finally
                {
                    connection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes the row.
        /// </summary>
        /// <param name="rowID">The row ID.</param>
        /// <param name="userID">The user ID.</param>
        /// <returns></returns>
        public bool DeleteRow(int rowID, int userID)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand {Connection = connection};
                result = _rowManager.DeleteRow(cmd, rowID, userID);
                connection.Close();
            }
            return result;
        }

    }
}
