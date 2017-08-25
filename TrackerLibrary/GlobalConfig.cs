using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; }

        public static void InitializeConnections(bool useSQL, bool useTextFile)
        {
            if (useSQL)
            {
                //TODO - Create DB connection
            }
            if (useTextFile)
            {
                //TODO - Create TExt Connection
            }
        }
    }
}
