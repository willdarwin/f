using System;
using System.Collections.Generic;
using System.Text;
using UtilityComponent.DataFactory;
using System.Data.SqlClient;
using System.Data;

namespace IdeaDomain.Infrastructure.DataEntities
{
    public class DataManager
    {
        private IDataAccess _dataAccess;

        public DataManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        public int AddData(SqlConnection connection, SqlCommand command, DataDE dataDE, int userID, int dataType)
        {
            int result = 0;
            string dataTypeStr = "";
            DbType dbType = DbType.String;
            switch (dataType)
            {
                case 0:
                    {
                        dataTypeStr = "Money"; dbType = DbType.Int32;
                    }
                    break;
                case 1:
                    {
                        dataTypeStr = "Number"; dbType = DbType.Int32;
                    }
                    break;
                case 2:
                    {
                        dataTypeStr = "Datetime"; dbType = DbType.DateTime;
                    }
                    break;
                case 3:
                    {
                        dataTypeStr = "LongText"; dbType = DbType.String;
                    }
                    break;
                case 4:
                    {
                        dataTypeStr = "ShortText"; dbType = DbType.String;
                    }
                    break;
                case 5:
                    {
                        dataTypeStr = "Status"; dbType = DbType.String;
                    }
                    break;
            }
            string sql = "insert into " + dataTypeStr + "_" + userID + " values(@Value, @ColumnId, @RowId, @IsDeleted)";
            QueryParameter[] paramList = new QueryParameter[4];
            paramList[0] = new QueryParameter("@Value", dataDE.Value, dbType);
            paramList[1] = new QueryParameter("@ColumnId", dataDE.ColumnId, DbType.Int32);
            paramList[2] = new QueryParameter("@RowId", dataDE.RowId, DbType.Int32);
            paramList[3] = new QueryParameter("@IsDeleted", dataDE.IsDeleted, DbType.Boolean);
            result = Convert.ToInt32(_dataAccess.ExecuteScalar(connection, command, sql, paramList));
            return result;
        }
    }
}
