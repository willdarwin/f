using System.Data;
using System.Data.SqlClient;
using IdeaDomain.InfrastructureLayer.DataEntities;
using UtilityComponent.DataFactory;
using System;

namespace IdeaDomain.InfrastructureLayer.DataManagers
{
    public class DataManager
    {
        private IDataAccess _dataAccess;

        public DataManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Adds the data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dataDE">The data DE.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="dataType">Type of the data.</param>
        public void AddData(SqlCommand command, DataDE dataDE, int userID, int dataType)
        {
            var dataTypeStr = "";
            var dbType = DbType.String;
            switch (dataType)
            {
                case 0:
                    dataTypeStr = "Money";
                    dbType = DbType.Decimal;
                    break;
                case 1:
                    dataTypeStr = "Number";
                    dbType = DbType.Decimal;
                    break;
                case 2:
                    dataTypeStr = "Datetime";
                    dbType = DbType.DateTime;
                    break;
                case 3:
                    dataTypeStr = "LongText";
                    dbType = DbType.String;
                    break;
                case 4:
                    dataTypeStr = "ShortText";
                    dbType = DbType.String;
                    break;
                case 5:
                    dataTypeStr = "ComplexType";
                    dbType = DbType.Int32;
                    break;
                case 6:
                    dataTypeStr = "Number";
                    dbType = DbType.Int32;
                    break;
                default: break;
            }
            var sql = "insert into U" + userID + "_" + dataTypeStr + " values(@value, @columnId, @rowId, @isDeleted)";
            var paramList = new QueryParameter[4];
            paramList[0] = new QueryParameter("@value", dataDE.Value, dbType);
            paramList[1] = new QueryParameter("@columnId", dataDE.ColumnId, DbType.Int32);
            paramList[2] = new QueryParameter("@rowId", dataDE.RowId, DbType.Int32);
            paramList[3] = new QueryParameter("@isDeleted", dataDE.IsDeleted, DbType.Boolean);
            _dataAccess.ExecuteScalar(command, sql, paramList);
        }

        /// <summary>
        /// Updates the data.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="dataDE">The data DE.</param>
        /// <param name="userID">The user ID.</param>
        /// <param name="dataType">Type of the data.</param>
        /// <returns></returns>
        public bool UpdateData(SqlCommand command, DataDE dataDE, int userID, int dataType)
        {
            var dataTypeStr = "";
            var valueName = "Value";
            var dbType = DbType.String;
            switch (dataType)
            {
                case 0:
                    dataTypeStr = "Money";
                    dbType = DbType.Decimal;
                    break;
                case 1:
                    dataTypeStr = "Number";
                    dbType = DbType.Decimal;
                    break;
                case 2:
                    dataTypeStr = "Datetime";
                    dbType = DbType.DateTime;
                    break;
                case 3:
                    dataTypeStr = "LongText";
                    dbType = DbType.String;
                    break;
                case 4:
                    dataTypeStr = "ShortText";
                    dbType = DbType.String;
                    break;
                case 5:
                    dataTypeStr = "ComplexType";
                    dbType = DbType.Int32;
                    valueName = "RefRowId";
                    break;
                case 6:
                    dataTypeStr = "Number";
                    dbType = DbType.Int32;
                    break;
                default: break;
            }
            var sql = "update  U" + userID + "_" + dataTypeStr + " set " + valueName + " = @value where columnId = @columnId and RowId = @rowId and IsDeleted = @isDeleted";

            var paramList = new QueryParameter[4];
            paramList[0] = new QueryParameter("@value", dataDE.Value, dbType);
            paramList[1] = new QueryParameter("@columnId", dataDE.ColumnId, DbType.Int32);
            paramList[2] = new QueryParameter("@rowId", dataDE.RowId, DbType.Int32);
            paramList[3] = new QueryParameter("@isDeleted", dataDE.IsDeleted, DbType.Boolean);
            return Convert.ToBoolean(_dataAccess.ExecuteNonQuery(command, sql, paramList));
        }
    }
}
