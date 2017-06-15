using System;
using System.Collections.Generic;
using System.Text;
using UtilityComponent.DataFactory;
using System.Data;
using AuthorityDomain.Infrastructure.DataEntities;

namespace AuthorityDomain.Infrastructure.DataManagers
{
    public class AuthorityManager
    {
        private IDataAccess _dataAccess;

        public AuthorityManager()
        {
            _dataAccess = DataAccessFactory.CreateDataAccess();
        }

        public bool IsAllowed(int roleId, int authorityId)
        {
            var result = false;
            const string sql = "select * from AuthorityRole where RoleId=@RoleId and ControllerActionId=@ControllerActionId";
            var queryParamers = new QueryParameter[2];
            queryParamers[0] = new QueryParameter("RoleId", roleId, DbType.Int32);
            queryParamers[1] = new QueryParameter("ControllerActionId", authorityId, DbType.Int32);
            try
            {
                _dataAccess.Connection.Open();
                var table = _dataAccess.GetTable(sql, queryParamers);
                if (table != null && Convert.ToBoolean(table.Rows[0]["IsAllowed"]))
                {
                    result = true;
                }
            }
            catch (System.Exception ex)
            {
            	//To do: Log
            }
            finally
            {
                _dataAccess.Close();
            }
            return result;
        }

        public AuthorityDE FindController(string controllerName)
        {
            AuthorityDE authorityDe = null;
            var sql = "select * from Authoriy where IsController=@IsController and ControllerName=@ControllerName";
            var queryParameters = new QueryParameter[2];
            queryParameters[0] = new QueryParameter("IsController", true, DbType.Boolean);
            queryParameters[1] = new QueryParameter("ControllerName", controllerName, DbType.String);
            try
            {
                _dataAccess.Connection.Open();
                var table = _dataAccess.GetTable(sql, queryParameters);
                authorityDe = new AuthorityDE();
                authorityDe.Id = Convert.ToInt32(table.Rows[0]["Id"]);
                authorityDe.IsAllowedAllRoles = Convert.ToBoolean(table.Rows[0]["IsAllowedAllRoles"]);
                authorityDe.IsAllowedNoneRoles = Convert.ToBoolean(table.Rows[0]["IsAllowedNoneRoles"]);
                authorityDe.IsController = Convert.ToBoolean(table.Rows[0]["IsController"]);
                authorityDe.Name = Convert.ToString(table.Rows[0]["Name"]);
                authorityDe.ControllerName = Convert.ToString(table.Rows[0]["ControllerName"]);
            }
            catch (System.Exception ex)
            {
                //To do: Log
            }
            finally
            {
                _dataAccess.Close();
            }
            return authorityDe;
        }

        public AuthorityDE FindAction(string controllerName, string actionName)
        {
            AuthorityDE authorityDE = null;
            var sql = "select * from Authority where IsController=@IsController and ControllerName=@ControllerName and Name=@Name";
            var queryParameters = new QueryParameter[3];
            queryParameters[0] = new QueryParameter("IsController", false, DbType.Boolean);
            queryParameters[1] = new QueryParameter("ControllerName", controllerName, DbType.String);
            queryParameters[2] = new QueryParameter("Name", actionName, DbType.String);
            try
            {
                _dataAccess.Connection.Open();
                var table = _dataAccess.GetTable(sql, queryParameters);
                authorityDE = new AuthorityDE();
                authorityDE.Id = Convert.ToInt32(table.Rows[0]["Id"]);
                authorityDE.IsAllowedAllRoles = Convert.ToBoolean(table.Rows[0]["IsAllowedAllRoles"]);
                authorityDE.IsAllowedNoneRoles = Convert.ToBoolean(table.Rows[0]["IsAllowedNoneRoles"]);
                authorityDE.IsController = Convert.ToBoolean(table.Rows[0]["IsController"]);
                authorityDE.Name = Convert.ToString(table.Rows[0]["Name"]);
                authorityDE.ControllerName = Convert.ToString(table.Rows[0]["ControllerName"]);
            }
            catch (System.Exception ex)
            {
                //To do: Log
            }
            finally
            {
                _dataAccess.Close();
            }
            return authorityDE;
        }
    }
}
