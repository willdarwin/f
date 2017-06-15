using System;
using System.Configuration;

namespace UtilityComponent.DataFactory
{
    public class DataAccessFactory
    {
        /// <summary>
        /// Creates the data access.
        /// </summary>
        /// <returns></returns>
        public static IDataAccess CreateDataAccess()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FlowerPotConnectionString"].ToString();
            var databaseType = ConfigurationManager.ConnectionStrings["FlowerPotConnectionString"].ProviderName;
            switch (databaseType)
            {
                case "System.Data.SqlClient":
                    return new SqlDataAccess(connectionString);
                default: throw new Exception("The type of database isn't supported!");
            }
        }
    }
}
