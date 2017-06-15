using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using IdeaDomain.InfrastructureLayer.DataEntities;
using Newtonsoft.Json.Linq;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class IdeaUnionManager
    {
        private IDataAccess _dataAccess;

        public IdeaUnionManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public DataTable GetIdeaData(SqlCommand command, List<ColumnDE> columns, IdeaDE idea, string sidx, string sord, int page, int rows, string filters)
        {
            var selectQuery = "select Top " + rows + " R.RowId,R.IdeaId,R.Version,R.IsDeleted";
            var joinQuery = " from U" + idea.UserId + "_Row R";
            var sortStatement = " Order by R.RowId";
            var count = 0;
            foreach (var column in columns)
            {
                count++;
                switch (column.DataTypeId)
                {
                    case 0:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Money] M_" + count + " on M_" + count + ".RowId=R.RowId and M_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",M_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by M_" + count + ".Value";
                            else
                                sortStatement = " Order by M_" + count + ".Value desc";
                        }
                        break;
                    case 1:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",N_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by N_" + count + ".Value";
                            else
                                sortStatement = " Order by N_" + count + ".Value desc";
                        }
                        break;
                    case 2:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Datetime] D_" + count + " on D_" + count + ".RowId=R.RowId and D_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",D_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by D_" + count + ".Value";
                            else
                                sortStatement = " Order by D_" + count + ".Value desc";
                        }
                        break;
                    case 3:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_LongText] L_" + count + " on L_" + count + ".RowId=R.RowId and L_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",L_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by L_" + count + ".Value";
                            else
                                sortStatement = " Order by L_" + count + ".Value desc";
                        }
                        break;
                    case 4:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ShortText] S_" + count + " on S_" + count + ".RowId=R.RowId and S_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",S_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by S_" + count + ".Value";
                            else
                                sortStatement = " Order by S_" + count + ".Value desc";
                        }
                        break;
                    case 5:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ComplexType] C_" + count + " on C_" + count + ".RowId=R.RowId and C_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",C_" + count + ".RefRowId '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by C_" + count + ".RefRowId";
                            else
                                sortStatement = " Order by C_" + count + ".RefRowId desc";
                        }
                        break;
                    case 6:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",N_" + count + ".Value '" + column.ColumnName+"'";
                        if (column.ColumnName == sidx)
                        {
                            if (sord == "asc")
                                sortStatement = " Order by N_" + count + ".Value";
                            else
                                sortStatement = " Order by N_" + count + ".Value desc";
                        }
                        break;
                    default:
                        break;
                }
            }
            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@ideaId", idea.IdeaId, DbType.Int32);
            var filterQuery = "";
            if (!string.IsNullOrEmpty(filters))
            {
                GetFilterQuery(filters, out filterQuery, out paramListIdea);
                paramListIdea[0] = new QueryParameter("@ideaId", idea.IdeaId, DbType.Int32);
            }

            var sqlSearchQuery = selectQuery + joinQuery + " Where R.IdeaId = @ideaId and R.RowId>(Select IsNull(Max(R.RowId),0) from (Select Top " + page * rows + " R.RowId " + joinQuery + " Where (R.IdeaId = " + idea.IdeaId + " and R.IsDeleted = 0" + filterQuery + ") Order by R.RowId ) AS R)  and R.IsDeleted = 0" + filterQuery + sortStatement;
            var rowTable = _dataAccess.GetTable(command, sqlSearchQuery, paramListIdea);
            return rowTable;
        }

        /// <summary>
        /// Gets the idea data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="idea">The idea.</param>
        /// <returns></returns>
        public int CountIdeaRows(SqlCommand command, List<ColumnDE> columns, IdeaDE idea, string filters)
        {
            var joinQuery = " from U" + idea.UserId + "_Row R";
            var count = 0;
            foreach (var column in columns)
            {
                count++;
                switch (column.DataTypeId)
                {
                    case 0:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Money] M_" + count + " on M_" + count + ".RowId=R.RowId and M_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 1:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 2:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Datetime] D_" + count + " on D_" + count + ".RowId=R.RowId and D_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 3:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_LongText] L_" + count + " on L_" + count + ".RowId=R.RowId and L_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 4:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ShortText] S_" + count + " on S_" + count + ".RowId=R.RowId and S_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 5:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ComplexType] C_" + count + " on C_" + count + ".RowId=R.RowId and C_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    case 6:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        break;
                    default:
                        break;
                }
            }

            var paramListIdea = new QueryParameter[1];
            paramListIdea[0] = new QueryParameter("@ideaId", idea.IdeaId, DbType.Int32);
            var filterQuery = "";
            if (!string.IsNullOrEmpty(filters))
            {
                GetFilterQuery(filters, out filterQuery, out paramListIdea);
                paramListIdea[0] = new QueryParameter("@ideaId", idea.IdeaId, DbType.Int32);
            }

            var selectQuery = "select count(*) " + joinQuery + " WHERE R.IdeaId = @ideaId and R.IsDeleted = 0" + filterQuery;

            var result = _dataAccess.GetTable(command, selectQuery, paramListIdea);
            var rowCount = Convert.ToInt32(result.Rows[0][0]);
            return rowCount;
        }

        /// <summary>
        /// Gets the filter params.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <param name="paramList_idea">The param list_idea.</param>
        /// <returns></returns>
        private void GetFilterQuery(string filters, out string filter_query, out QueryParameter[] paramList_idea)
        {
            var filtersObject = JObject.Parse(filters);
            var group = (string)filtersObject["groupOp"];
            var rules = (JArray)filtersObject["rules"];
            paramList_idea = new QueryParameter[rules.Count + 1];
            var ruleCount = 1;
            filter_query = "";
            foreach (var rule in rules)
            {
                var field = (string)rule["field"];
                var op = (string)rule["op"];
                var data = (string)rule["data"];
                if (field.Substring(0, 1) == "D")
                {
                    filter_query += " and " + field + "=@data_" + ruleCount;
                    paramList_idea[ruleCount] = new QueryParameter("@data_" + ruleCount, Convert.ToDateTime(data), DbType.Int32);
                }
                else
                {
                    filter_query += " and " + field + "=@data_" + ruleCount;
                    paramList_idea[ruleCount] = new QueryParameter("@data_" + ruleCount, data, DbType.Int32);
                }
                ruleCount++;
            }

        }
    }
}