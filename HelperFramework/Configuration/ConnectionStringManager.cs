using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperFramework.Configuration
{
    public enum DatabaseName
    {
        CuMaster
    }

    public static class ConnectionStringManager
    {
        private static string[] databaseConnStringKeys = new string[] { "CuMaster_DB_Connection" };
        private static string[] databaseConnStrings = new string[] { "" };

        public static string GetConnectionString(DatabaseName dbName)
        {
            int dbConnStrIndex = (int)dbName;

            if (dbConnStrIndex > databaseConnStringKeys.Length)
            {
                throw new IndexOutOfRangeException("Invalid parameter supplied for dbName.");
            }

            if (databaseConnStrings[dbConnStrIndex] == "")
            {
                databaseConnStrings[dbConnStrIndex] = WebConfigConfiguration.GetConnectionSetting(databaseConnStringKeys[dbConnStrIndex]).ConnectionString;

                //if it's still blank, means we didn't get it
                if (databaseConnStrings[dbConnStrIndex] == "")
                {
                    throw new KeyNotFoundException("Connection string for " + dbConnStrIndex.ToString() + " cannot be retrieved.");
                }
            }

            return databaseConnStrings[dbConnStrIndex];
        }
    }
}
