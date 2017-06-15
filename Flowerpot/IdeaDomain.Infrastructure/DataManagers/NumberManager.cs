using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityComponent.DataFactory;
using System.Data.SqlClient;
using System.Data;
using IdeaDomain.InfrastructureLayer.DataEntities;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class NumberManager
    {

        private IDataAccess _dataAccess;

        public NumberManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        public int AddNumber(SqlConnection connection, SqlCommand command, NumberDE numberDE, int userID)
        {
            int result = 0;
            string sql = "insert into Number_" + userID + " values(@Value, @ColumnId, @RowId, @IsDeleted)";
            QueryParameter[] paramList = new QueryParameter[4];
            paramList[0] = new QueryParameter("@Value", numberDE.Value, DbType.String);
            paramList[1] = new QueryParameter("@ColumnId", numberDE.ColumnId, DbType.Int32);
            paramList[2] = new QueryParameter("@RowId", numberDE.RowId, DbType.Int32);
            paramList[3] = new QueryParameter("@IsDeleted", numberDE.IsDeleted, DbType.Boolean);
            result = Convert.ToInt32(_dataAccess.ExecuteScalar(connection, command, sql, paramList));
            return result;
        }

    }
}
