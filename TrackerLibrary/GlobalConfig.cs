using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.DataAccess;
using System.Configuration;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public const string PrizesFile = "Prizes.csv";
        public const string PeopleFile = "People.csv";
        public const string TeamFile = "Teams.csv";
        public const string TournamentFile = "Tournaments.csv";
        public const string MatchupFile = "Matchup.csv";
        public const string MatchupEntryFile = "MatchupEntry.csv";

        public static IDataConnection Connection { get; private set; }

        public static void InitializeConnections(DatabaseType db)
        {
            if (db == DatabaseType.Sql)
            {
                SQLConnector sql = new SQLConnector();
                Connection= sql;
            }
            else if (db == DatabaseType.TextFile)
            {
                TextConnector text = new TextConnector();
                Connection= text;
            }
        }

        public static string CnnString(string conName)
        {
            return ConfigurationManager.ConnectionStrings[conName].ConnectionString;
        }

        public static string AppKeyLookup(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
    }
}
