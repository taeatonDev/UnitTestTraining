using System;
using System.Configuration;
using SMG.Core.Data.Database;

namespace UnitTestWorkshop.Data.Databases
{
    /// <summary>
    /// Business Requirements:
    /// 1. Retrieves the Connection String from the App/Web Config of the executing application.
    /// 2. If the connection string is missing a value, throw exception
    /// 3. If connection string is missing, throw exception
    /// </summary>
    public class UnitTestDatabase : BaseDatabase
    {
        public override string GetConnectionString()
        {
            string connectionString;

            if (ConfigurationManager.ConnectionStrings["UnitTestDatabase"] != null)
            {
                connectionString = ConfigurationManager.ConnectionStrings["UnitTestDatabase"].ConnectionString;

                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    throw new Exception(string.Format("ConnectionStrings - UnitTestDatabase - Must be provided."));
                }
            }
            else
            {
                throw new Exception(string.Format("ConnectionStrings - UnitTestDatabase - Must be provided."));
            }

            return connectionString;
        }
    }
}