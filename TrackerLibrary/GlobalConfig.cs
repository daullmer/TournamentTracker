﻿using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.DataAccess;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                //TODO - Setup SQLConnector properly
                SQLConnector sql = new SQLConnector();
                Connection= sql;
            }
            else if (db == DatabaseType.TextFile)
            {
                //TODO - Create TExt Connection
                TextConnector text = new TextConnector();
                Connection= text;
            }
        }

        public static string CnnString(string conName)
        {
            return ConfigurationManager.ConnectionStrings[conName].ConnectionString;
        }
    }
}