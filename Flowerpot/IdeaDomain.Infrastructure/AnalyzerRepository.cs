using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using IdeaDomain.DomainLayer.Entities;
using IdeaDomain.DomainLayer.RepositoryInterfaces;
using IdeaDomain.InfrastructureLayer.DataEntities;
using IdeaDomain.InfrastructureLayer.DataManagers;
using IdeaDomain.InfrastructureLayer.Translator;
using Newtonsoft.Json.Linq;
using UtilityComponent.DataFactory;

namespace IdeaDomain.InfrastructureLayer.Repositories
{
    public class AnalyzerRepository : IAnalyzerRepository
    {

        private IDataAccess _dataAccess = DataAccessFactory.CreateDataAccess();
        AnalyzerManager _analyzerManager;
        ColumnManager _columnManager;
        IdeaManager _ideaManager;
        IdeaUnionManager _ideaUnionManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnalyzerRepository"/> class.
        /// </summary>
        public AnalyzerRepository()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
            _analyzerManager = new AnalyzerManager();
            _columnManager = new ColumnManager();
            _ideaManager = new IdeaManager();
            _ideaUnionManager = new IdeaUnionManager();
        }

        private readonly IdeaDomainModelDataEntities _mapper = new IdeaDomainModelDataEntities();

        #region Public Functions

        #region Operation Functions

        /// <summary>
        /// Creates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        public Analyzer CreateAnalyzer(Analyzer analyzer)
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();

                var cmd = new SqlCommand();
                var sqltrans = connection.BeginTransaction();

                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                if (string.IsNullOrEmpty(analyzer.WhereQuery))
                {
                    analyzer.WhereQuery += " 1=1 ";
                }
                try
                {
                    analyzer.AnalyzerId = _analyzerManager.CreateAnalyzer(cmd, _mapper.Map<AnalyzerDE>(analyzer));
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            return analyzer;
        }

        /// <summary>
        /// Updates the analyzer.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        public Analyzer UpdateAnalyzer(Analyzer analyzer)
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();

                var cmd = new SqlCommand();
                var sqltrans = connection.BeginTransaction();

                cmd.Connection = connection;
                cmd.Transaction = sqltrans;

                try
                {
                    _analyzerManager.UpdateAnalyzer(cmd, _mapper.Map<AnalyzerDE>(analyzer));
                    sqltrans.Commit();
                }
                catch
                {
                    sqltrans.Rollback();
                }
                finally
                {
                    connection.Close();
                }
            }
            return analyzer;
        }

        /// <summary>
        /// Deletes the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public bool DeleteAnalyzerById(int analyzerId)
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();

                var cmd = new SqlCommand();
                var sqltrans = connection.BeginTransaction();

                cmd.Connection = connection;
                cmd.Transaction = sqltrans;
                try
                {
                    _analyzerManager.DeleteAnalyzerById(cmd, analyzerId);
                    sqltrans.Commit();
                    return true;
                }
                catch
                {
                    sqltrans.Rollback();
                    return false;
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        #endregion

        #region Search Functions

        /// <summary>
        /// Gets the analyzers by user id.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <returns></returns>
        public List<Analyzer> GetAnalyzersByUserId(int userId)
        {
            var analyzerList = new List<Analyzer>();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                analyzerList = _mapper.Map<List<Analyzer>>(_analyzerManager.GetAnalyzersByUserId(cmd, userId));
                connection.Close();
            }
            return analyzerList;
        }

        /// <summary>
        /// Gets the analyzer by id.
        /// </summary>
        /// <param name="analyzerId">The analyzer id.</param>
        /// <returns></returns>
        public Analyzer GetAnalyzerById(int analyzerId)
        {
            var analyzer = new Analyzer();
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                analyzer = _mapper.Map<Analyzer>(_analyzerManager.GetAnalyzerById(cmd, analyzerId));
                connection.Close();
            }
            return analyzer;
        }

        /// <summary>
        /// Gets the analyzer data.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <returns></returns>
        public AnalyzerDetail GetAnalyzerData(Analyzer analyzer, string sidx = "", string sord = "asc", string filters = "")
        {
            using (var connection = _dataAccess.CreateConnection())
            {
                connection.Open();
                var cmd = new SqlCommand();
                cmd.Connection = connection;
                var fromList = ChangeQueryToList(analyzer.JoinQuery);
                var selectQuery = "Select " + analyzer.SelectQuery + " From ";
                for (var i = 0; i < fromList.Count(); i++)
                {
                    if (i == 0 || i % 3 == 2)//get Form Query
                    {
                        selectQuery += GetIdeaString(cmd, fromList[i].ToString());
                    }
                    else
                    {
                        selectQuery += (string)fromList[i];
                    }
                }
                selectQuery += " Where " + analyzer.WhereQuery;
                var result = _analyzerManager.GetAnalyzerData(cmd, selectQuery);
                if (!string.IsNullOrEmpty(sidx))
                {
                    var rows = result.Select(GetFilterQuery(filters), sidx + " " + sord);//search in datatable
                    result = rows.Any() ? rows.CopyToDataTable() : result.Clone();
                }
                var analyzerValue = _mapper.Map<AnalyzerDetail>(analyzer);
                foreach (var analyzerColumn in from DataColumn column in result.Columns select new ColumnInIdea() { ColumnName = column.ColumnName })
                {
                    analyzerValue.Columns.Add(analyzerColumn);
                }
                if (result.Rows.Count <= 0) return analyzerValue;
                for (var i = 0; i < result.Rows.Count; i++)
                {
                    var row = new Row();
                    for (var j = 0; j < result.Columns.Count; j++)
                    {
                        row.Values.Add(result.Rows[i][j]);
                    }
                    analyzerValue.Rows.Add(row);
                }
                return analyzerValue;
            }
        }

        #endregion

        #endregion

        #region Private Functions

        /// <summary>
        /// Gets the filter query.
        /// </summary>
        /// <param name="filters">The filters.</param>
        /// <returns></returns>
        private string GetFilterQuery(string filters)
        {
            if (!string.IsNullOrEmpty(filters))
            {
                var filtersObject = JObject.Parse(filters);
                var group = (string)filtersObject["groupOp"];
                var rules = (JArray)filtersObject["rules"];
                var whereQuery = " 1=1 ";
                foreach (var rule in rules)
                {
                    var field = (string)rule["field"];
                    var op = (string)rule["op"];
                    var data = (string)rule["data"];
                    whereQuery += " and " + field + " = '" + data + "'";
                }
                return whereQuery;
            }
            return null;
        }

        /// <summary>
        /// Changes the query to list.
        /// </summary>
        /// <param name="joinQuery">From query.</param>
        /// <returns></returns>
        private List<string> ChangeQueryToList(string joinQuery)
        {
            return (from Match m in Regex.Matches(joinQuery, @"\[(.*?)\]") select m.Groups[1].Value).ToList();
        }

        /// <summary>
        /// Gets the idea string.
        /// </summary>
        /// <param name="cmd">The CMD.</param>
        /// <param name="ideaId">The idea id.</param>
        /// <returns></returns>
        private string GetIdeaString(SqlCommand cmd, string ideaName)
        {
            var idea = _ideaManager.GetIdeaByName(cmd, ideaName);
            var columns = _columnManager.GetColumnnsByIdea(cmd, idea.IdeaId);

            var selectQuery = " (select Top 100000 R.RowId,R.IdeaId,R.Version,R.IsDeleted";
            var joinQuery = " from U" + idea.UserId + "_Row R";
            var count = 0;
            foreach (var column in columns)
            {
                count++;
                switch (column.DataTypeId)
                {
                    case 0:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Money] M_" + count + " on M_" + count + ".RowId=R.RowId and M_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",M_" + count + ".Value '" + column.ColumnName + "'";
                        break;
                    case 1:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",N_" + count + ".Value '" + column.ColumnName + "'";
                        break;
                    case 2:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Datetime] D_" + count + " on D_" + count + ".RowId=R.RowId and D_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",D_" + count + ".Value '" + column.ColumnName + "'";
                        break;
                    case 3:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_LongText] L_" + count + " on L_" + count + ".RowId=R.RowId and L_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",L_" + count + ".Value '" + column.ColumnName + "'";
                        break;
                    case 4:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ShortText] S_" + count + " on S_" + count + ".RowId=R.RowId and S_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",S_" + count + ".Value '" + column.ColumnName+"'";
                        break;
                    case 5:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_ComplexType] C_" + count + " on C_" + count + ".RowId=R.RowId and C_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",C_" + count + ".RefRowId '" + column.ColumnName + "'";
                        break;
                    case 6:
                        joinQuery = joinQuery + " left join [U" + idea.UserId + "_Number] N_" + count + " on N_" + count + ".RowId=R.RowId and N_" + count + ".ColumnId = " + column.ColumnId + "";
                        selectQuery = selectQuery + ",N_" + count + ".Value '" + column.ColumnName + "'";
                        break;
                    default:
                        break;
                }
            }
            var sqlSearchQuery = selectQuery + joinQuery + " Where R.IdeaId = " + idea.IdeaId + " and R.IsDeleted = 0 Order by R.RowId)  AS " + idea.IdeaName;
            return sqlSearchQuery;
        }

        #endregion

    }
}
