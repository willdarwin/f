using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UtilityComponent.DataFactory;
using System.Data.SqlClient;
using IdeaDomain.InfrastructureLayer.DataEntities;
using System.Data;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class ShortTextManager
    {

        private IDataAccess _dataAccess;

        public ShortTextManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        public int AddShortText(SqlConnection connection, SqlCommand command, ShortTextDE shortTextDE, int userID)
        {
            int result = 0;
            string sql = "insert into ShortText_" + userID + " values(@Value, @ColumnId, @RowId, @IsDeleted)";
            QueryParameter[] paramList = new QueryParameter[4];
            paramList[0] = new QueryParameter("@Value", shortTextDE.Value, DbType.String);
            paramList[1] = new QueryParameter("@ColumnId", shortTextDE.ColumnId, DbType.Int32);
            paramList[2] = new QueryParameter("@RowId", shortTextDE.RowId, DbType.Int32);
            paramList[3] = new QueryParameter("@IsDeleted", shortTextDE.IsDeleted, DbType.Boolean);
            result = Convert.ToInt32(_dataAccess.ExecuteScalar(connection, command, sql, paramList));
            return result;
        }

    }
}
