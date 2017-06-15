using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.InfrastructureLayer.DataEntities;
using IdeaDomain.InfrastructureLayer.DataManagers;
using IdeaDomain.InfrastructureLayer.Translator;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.Repositories
{
    public class IdeaRepository : IIdeaRepository
    {
        private readonly IDataAccess _dataAccess = DataAccessFactory.CreateDataAccess();
        RowManager _rowManager;
        readonly ColumnManager _columnManager;
        readonly IdeaUnionManager _ideaUnionManager;
        readonly IdeaManager _ideaManager;

        /// <summary>
        /// Initialize a data access interface
        /// </summary>
        public IdeaRepository()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
            _rowManager = new RowManager();
            _columnManager = new ColumnManager();
            _ideaManager = new IdeaManager();
            _ideaUnionManager = new IdeaUnionManager();
        }
        private readonly IdeaDomainModelDataEntities _mapper = new IdeaDomainModelDataEntities();

        /// <summary>
        /// Get all ideas by userid.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>Return idea list</returns>
        public List<Idea> GetIdeasByUser(int userId)
        {
            var ideaListResult = new List<Idea>();
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
                    var ideaList = _mapper.Map<List<Idea>>(_ideaManager.GetIdeasByUser(cmd, userId));
                    foreach (var idea in ideaList)
                    {
                        idea.Columns = _mapper.Map<List<ColumnInIdea>>(_columnManager.GetColumnnsByIdea(cmd, idea.IdeaId));
                        ideaListResult.Add(idea);
                    }
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                connection.Close();
            }
            return ideaListResult;
        }

        /// <summary>
        /// Get idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns>
        /// Return an idea object
        /// </returns>
        public Idea GetIdeaById(int ideaId)
        {
            var idea = new Idea();
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
                    idea = _mapper.Map<Idea>(_ideaManager.GetIdeaById(cmd, ideaId));
                    idea.Columns = _mapper.Map<List<ColumnInIdea>>(_columnManager.GetColumnnsByIdea(cmd, ideaId));
                    var refColumnIds = new List<int>();
                    foreach (var column in idea.Columns)
                    {
                        var refColumnName = "";
                        if (column.DataTypeId == 5 && column.ReferedIdeaId > 0)
                        {
                            var refIdea = _mapper.Map<Idea>(_ideaManager.GetIdeaById(cmd, column.ReferedIdeaId));
                            column.ReferedIdeaIdName = refIdea.IdeaName;
                            var refColumns = (this.GetRefColumnsByColumnId(column.ColumnId));
                            if (refColumns != null && refColumns.Count > 0)
                            {
                                foreach (var refColumn in refColumns)
                                {
                                    refColumnIds.Add(refColumn.ColumnId);
                                    refColumnName += refColumn.ColumnName + ",";
                                }
                                column.ReferedColumnIds = refColumnIds;
                                column.ReferedColumnNames = refColumnName.Remove(refColumnName.LastIndexOf(",", StringComparison.Ordinal), 1);
                            }
                            else
                            {
                                column.ReferedIdeaIdName = "";
                                column.ReferedColumnNames = "";
                            }
                        }
                        else
                        {
                            column.ReferedIdeaIdName = "";
                            column.ReferedColumnNames = "";
                        }
                    }
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                connection.Close();
            }
            return idea;
        }

        /// <summary>
        /// Get idea by ideaName.
        /// </summary>
        /// <param name="ideaName"></param>
        /// <returns>
        /// Return an idea object
        /// </returns>
        public Idea GetIdeaByName(string ideaName)
        {
            var idea = new Idea();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                try
                {
                    idea = _mapper.Map<Idea>(_ideaManager.GetIdeaByName(cmd, ideaName));
                    idea.Columns = _mapper.Map<List<ColumnInIdea>>(_columnManager.GetColumnnsByIdea(cmd, idea.IdeaId));
                    var refColumnIds = new List<int>();
                    foreach (var column in idea.Columns)
                    {
                        var refColumnName = "";
                        if (column.DataTypeId == 5 && column.ReferedIdeaId > 0)
                        {
                            var refIdea = _mapper.Map<Idea>(_ideaManager.GetIdeaById(cmd, column.ReferedIdeaId));
                            column.ReferedIdeaIdName = refIdea.IdeaName;
                            var refColumns = (this.GetRefColumnsByColumnId(column.ColumnId));
                            if (refColumns != null && refColumns.Count > 0)
                            {
                                foreach (var refColumn in refColumns)
                                {
                                    refColumnIds.Add(refColumn.ColumnId);
                                    refColumnName += refColumn.ColumnName + ",";
                                }
                                column.ReferedColumnIds = refColumnIds;
                                column.ReferedColumnNames = refColumnName.Remove(refColumnName.LastIndexOf(",", StringComparison.Ordinal), 1);
                            }
                            else
                            {
                                column.ReferedIdeaIdName = "";
                                column.ReferedColumnNames = "";
                            }
                        }
                        else
                        {
                            column.ReferedIdeaIdName = "";
                            column.ReferedColumnNames = "";
                        }
                    }
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                connection.Close();
            }
            return idea;
        }

        /// <summary>
        /// Create an idea with its columns under a transaction.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        public Idea CreateIdea(Idea idea)
        {
            if (idea == null) return null;
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
                    idea.IdeaId = _ideaManager.CreateIdea(cmd, _mapper.Map<IdeaDE>(idea));
                    if (idea.Columns != null)
                    {
                        var columnListNew = new List<ColumnInIdea>();
                        foreach (var column in idea.Columns)
                        {
                            column.IdeaId = idea.IdeaId;
                            column.ColumnId = _columnManager.CreateColumn(cmd, _mapper.Map<ColumnDE>(column));
                            if (column.DataTypeId == 5)
                            {
                                foreach (var t in column.ReferedColumnIds)
                                    _columnManager.InsertRefColumn(cmd, column.ColumnId, t);
                            }
                            columnListNew.Add(column);
                        }
                        idea.Columns = columnListNew;
                    }
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                connection.Close();
            }
            return idea;
        }

        /// <summary>
        /// Update an idea.
        /// </summary>
        /// <param name="idea"></param>
        /// <returns></returns>
        public bool UpdateIdea(Idea idea)
        {
            var result = false;
            if (idea != null)
            {
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
                        result = _ideaManager.UpdateIdea(cmd, _mapper.Map<IdeaDE>(idea));
                        if (idea.Columns != null && result)
                        {
                            var columnsList = new List<ColumnInIdea>();
                            foreach (var column in idea.Columns)
                            {
                                column.IdeaId = idea.IdeaId;
                                column.ColumnId = _columnManager.CreateColumn(cmd, _mapper.Map<ColumnDE>(column));
                                if (column.DataTypeId != 5) continue;
                                foreach (var t in column.ReferedColumnIds)
                                {
                                    _columnManager.InsertRefColumn(cmd, column.ColumnId, t);
                                }
                            }
                        }
                        sqltrans.Commit();
                    }
                    catch
                    {
                        sqltrans.Rollback();
                    }
                    connection.Close();
                }
            }
            return result;
        }

        /// <summary>
        /// Delete an idea by ideaid.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public bool DeleteIdeaById(int ideaId)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                var referenceResult = _columnManager.CheckReference(cmd, ideaId);
                if (!referenceResult)
                {
                    result = _ideaManager.DeleteIdeaById(cmd, ideaId);
                }
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Validate Idea Name Duplication.
        /// </summary>
        /// <param name="ideaName"></param>
        /// <param name="userId"></param>
        /// <returns></returns> 
        public bool ValidateIdeaNameDuplication(string ideaName, int userId)
        {
            var result = false;
            var ideaNames = new List<string>();
            var ideaList = new List<Idea>();

            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                ideaList = _mapper.Map<List<Idea>>(_ideaManager.GetIdeasByUser(cmd, userId));
                if (ideaList == null) return false;
                ideaNames.AddRange(ideaList.Select(idea => idea.IdeaName));
                result = ideaNames.Contains(ideaName);
            }
            return result;
        }

        /// <summary>
        /// Get columns by idea.
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        public List<ColumnInIdea> GetColumnsByIdea(int ideaId)
        {
            List<ColumnInIdea> columnList;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                columnList = _mapper.Map<List<ColumnInIdea>>(_columnManager.GetColumnnsByIdea(cmd, ideaId));
                connection.Close();
            }
            return columnList;
        }

        /// <summary>
        /// Get a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public ColumnInIdea GetColumnById(int columnId)
        {
            ColumnInIdea column;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                column = _mapper.Map<ColumnInIdea>(_columnManager.GetColumnById(cmd, columnId));
                connection.Close();
            }
            return column;
        }

        /// <summary>
        /// Update a column by column entity.
        /// </summary>
        /// <param name="column"></param>
        /// <returns></returns>
        /// ML
        public bool UpdateColumn(ColumnInIdea column)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var sqltrans = connection.BeginTransaction();
                var cmd = new SqlCommand
                {
                    Connection = connection,
                    Transaction = sqltrans
                };
                result = _columnManager.DeleteColumnById(cmd, column.ColumnId);
                if (result)
                {
                    column.ColumnId = _columnManager.CreateColumn(cmd, _mapper.Map<ColumnDE>(column));
                    if (column.DataTypeId == 5)
                    {
                        foreach (var t in column.ReferedColumnIds)
                            _columnManager.InsertRefColumn(cmd, column.ColumnId, t);
                    }
                }
                sqltrans.Commit();
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Delete a column by columnid.
        /// </summary>
        /// <param name="columnId"></param>
        /// <returns></returns>
        public bool DeleteColumnById(int columnId)
        {
            var result = false;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                result = _columnManager.DeleteColumnById(cmd, columnId);
                connection.Close();
            }
            return result;
        }

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public IdeaDetail GetIdeaData(Idea idea, string sidx, string sord, int page, int rows, string filters)
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };

                var ideaValue = _mapper.Map<IdeaDetail>(idea);
                var rowTable = _ideaUnionManager.GetIdeaData(cmd, _mapper.Map<List<ColumnDE>>(ideaValue.Columns), _mapper.Map<IdeaDE>(idea), sidx, sord, page, rows, filters);
                if (rowTable.Rows.Count > 0)
                {
                    for (var i = 0; i < rowTable.Rows.Count; i++)
                    {
                        var row = new Row
                        {
                            RowId = Convert.ToInt32(rowTable.Rows[i][0]),
                            IdeaId = Convert.ToInt32(rowTable.Rows[i][1]),
                            Version = Convert.ToInt32(rowTable.Rows[i][2]),
                            IsDeleted = Convert.ToBoolean(rowTable.Rows[i][3])
                        };
                        for (var j = 0; j < ideaValue.Columns.Count; j++)
                        {
                            row.Values.Add(rowTable.Rows[i][j + 4]);
                        }
                        ideaValue.Rows.Add(row);
                    }
                }
                connection.Close();
                return ideaValue;
            }

        }

        /// <summary>
        /// Counts the idea rows.
        /// </summary>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public int CountIdeaRows(Idea idea, string filters)
        {
            var rowsCount = 0;
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                rowsCount = _ideaUnionManager.CountIdeaRows(cmd, _mapper.Map<List<ColumnDE>>(idea.Columns), _mapper.Map<IdeaDE>(idea), filters);
                connection.Close();
            }
            return rowsCount;
        }

        /// <summary>
        /// Gets the ref columns by column id.
        /// </summary>
        /// <param name="columnId">The column id.</param>
        /// <returns></returns>
        public List<ColumnInIdea> GetRefColumnsByColumnId(int columnId)
        {
            var columns = new List<ColumnInIdea>();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand { Connection = connection };
                var dt = _columnManager.GetRefColumnsByColumnId(cmd, columnId);
                columns.AddRange(from DataRow row in dt.Rows select _mapper.Map<ColumnInIdea>(_columnManager.GetColumnById(cmd, Convert.ToInt32(row["ReferedColumnId"]))));
            }
            return columns;
        }

    }
}
