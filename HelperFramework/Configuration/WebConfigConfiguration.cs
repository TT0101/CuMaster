using System;
using System.Collections.Generic;
using System.Configuration;

namespace HelperFramework.Configuration
{

    public static class WebConfigConfiguration
    {
        public static String GetWebConfigAppSetting(String settingName)
        {
            try
            {
                return System.Configuration.ConfigurationManager.AppSettings[settingName];

            }
            catch (System.Exception ex)
            {
                throw new KeyNotFoundException("Cound not read configuration setting " + settingName, ex);
            }
        }

        public static ConnectionStringSettings GetConnectionSetting(String connectionName)
        {
            try
            {
                return System.Configuration.ConfigurationManager.ConnectionStrings[connectionName];
            }
            catch (System.Exception ex)
            {
                throw new KeyNotFoundException("Cound not read connection setting " + connectionName, ex);
            }
        }

        
    }

}
