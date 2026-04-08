using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration;

namespace HealthClaimsProcessor.Core.DataAccess
{
    public class DatabaseFactory
    {
        private static Database _database;

        public static Database GetDatabase()
        {
            if (_database == null)
            {
                _database = new DatabaseProviderFactory().Create("HealthClaimsDB");
            }
            return _database;
        }

        public static Database GetDatabase(string connectionName)
        {
            return new DatabaseProviderFactory().Create(connectionName);
        }
    }
}
