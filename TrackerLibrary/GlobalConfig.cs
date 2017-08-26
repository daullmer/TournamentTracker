using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.DataAccess;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitializeConnections(bool useSQL, bool useTextFile)
        {
            if (useSQL)
            {
                //TODO - Setup SQLConnector properly
                SQLConnector sql = new SQLConnector();
                Connections.Add(sql);
            }
            if (useTextFile)
            {
                //TODO - Create TExt Connection
                TextConnector text = new TextConnector();
                Connections.Add(text);
            }
        }

        public static string CnnString(string conName)
        {
            return ConfigurationManager.ConnectionStrings[conName].ConnectionString;
        }
    }
}
