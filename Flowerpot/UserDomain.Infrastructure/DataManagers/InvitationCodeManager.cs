using System;
using System.Collections.Generic;
using System.Text;
using UtilityComponent.Unity;
using UtilityComponent.DataFactory;
using System.Data;
using System.Data.SqlClient;
using UserDomain.Infrastructure.DataEntities;

namespace UserDomain.Infrastructure.DataManagers
{
    public class InvitationCodeManager
    {

        private IDataAccess _dataAccess;

        public InvitationCodeManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        /// <summary>
        /// Creates the specified count.
        /// </summary>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        public bool Create(int count)
        {
            var result = false;
            var r = 0;
            var list = new InvitationCodeGenerator().GetInvitationCodeByCount(count);
            var sqlBuilder = new StringBuilder();
            for (var i = 0; i < list.Count; i++ )
            {
                sqlBuilder.Append("insert into InvitationCode values(@value" + i + ", 0, null);");
            }
            var queryParams = new QueryParameter[list.Count];
            for (var i = 0; i < list.Count; i++ )
            {
                queryParams[i] = new QueryParameter("@value" + i, list[i], DbType.Int32);
            }
            try
            {
                _dataAccess.Connection.Open();
                r = _dataAccess.ExecuteNonQuery(sqlBuilder.ToString(), queryParams);
            }
            catch (System.Exception ex)
            {
            	//Todo: LOg
            }
            finally
            {
                _dataAccess.Connection.Close();
            }
            if (r > 0)
            {
                result = true;
            }
            return result;
        }


        public bool Check(string code)
        {
            var result = false;
            int? count = null;
            var sqlBuilder = new StringBuilder();
            sqlBuilder.Append("select count(*) from InvitationCode where value=@value");
            var param = new QueryParameter("@value", code, DbType.String);
            try
            {
                _dataAccess.Connection.Open();
                count = _dataAccess.ExecuteScalar(sqlBuilder.ToString(), param) as int?;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.Connection.Close();
            }
            if (count != null && count > 0)
            {
                result = true;
            }
            return result;
        }

        public List<InvitationCodeDE> GetAllInvitationCode()
        {
            var list = new List<InvitationCodeDE>();
            var sql = "select * from InvitationCode";
            DataTable table = null;
            try
            {
                _dataAccess.Connection.Open();
                table = _dataAccess.GetTable(sql);
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
            finally
            {
                _dataAccess.Connection.Close();
            }
            if (table != null)
            {
                for (var i = 0; i < table.Rows.Count; i++ )
                {
                    var row = table.Rows[i];
                    var code = new InvitationCodeDE();
                    code.Id = Convert.ToInt32(row["Id"]);
                    code.Value = Convert.ToString(row["Value"]);
                    code.Obsolete = Convert.ToBoolean(row["Obsolete"]);
                    if (row["UserId"] != DBNull.Value)
                    {
                        code.UserId = Convert.ToInt32(row["UserId"]);
                    }
                    list.Add(code);
                }
            }
            return list;
        }
    }
}
