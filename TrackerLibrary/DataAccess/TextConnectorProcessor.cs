using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess.TextHelpers
{
    public static class TextConnectorProcessor
    {
        public static string FullFilePath(this string fileName)
        {
            return $"{ ConfigurationManager.AppSettings["filePath"] }\\{ fileName }";
        }

        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        public static List<Prize> ConvertToPrizes(this List<string> lines)
        {
            List<Prize> output = new List<Prize>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Prize p = new Prize
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeAmount = decimal.Parse(cols[3]),
                    PrizePercentage = double.Parse(cols[4])
                };
                output.Add(p);
            }
            return output;
        }

        public static void SaveToPrizeFile(this List<Prize> models)
        {
            List<string> lines = new List<string>();

            foreach (Prize p in models)
            {
                lines.Add($"{ p.Id },{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(GlobalConfig.PrizesFile.FullFilePath(), lines);
        }

        public static List<Person> ConvertToPerson (this List<string> lines)
        {
            List<Person> output = new List<Person>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                Person p = new Person
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAdress = cols[3]
                };
                output.Add(p);
            }
            return output;
        }

        public static void SaveToPeopleFile(this List<Person> models)
        {
            List<string> lines = new List<string>();

            foreach (Person p in models)
            {
                lines.Add($"{ p.Id },{p.FirstName},{p.LastName},{p.EmailAdress}");
            }

            File.WriteAllLines(GlobalConfig.PeopleFile.FullFilePath(), lines);
        }

        public static List<Team> ConvertToTeams(this List<string> lines)
        {
            List<Team> output = new List<Team>();
            List<Person> people = GlobalConfig.PeopleFile.FullFilePath().LoadFile().ConvertToPerson();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Team t = new Team();
                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');
                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(people
                        .Where(x => x.Id == int.Parse(id))
                        .First());
                }
                output.Add(t);
            }

            return output;
        }

        public static void SaveToTeamFile(this List<Team> models)
        {
            List<string> lines = new List<string>();

            foreach (Team t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToSting(t.TeamMembers)}");
            }

            File.WriteAllLines(GlobalConfig.TeamFile.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<Tournament> model)
        {
            List<string> lines = new List<string>();
            foreach (Tournament t in model)
            {
                lines.Add($"{ t.Id },{ t.TournamentName },{ t.EntryFee },{ ConvertTeamListToString(t.EnteredTeams) },{ ConvertPrizeListToString(t.Prizes) },{ ConvertRoundListToString(t.Rounds) }");
            }

            File.WriteAllLines(GlobalConfig.TournamentFile.FullFilePath(), lines);
        }

        public static void SaveRoundsToFile(this Tournament model)
        {
            //Loop through each round
            //Loop through each matchup
            //get the id for the new matchup
            //save the matchup
            //Loop through each entry, get the id and save it

            foreach (List<Matchup> round in model.Rounds)
            {
                foreach (Matchup matchup in round)
                {
                    //Load all of the matchups from file
                    //get the id and add 1
                    //store the id
                    //save the matchup record
                    matchup.SaveMatchupToFile();
                }
            }

        }

        public static List<MatchupEntry> ConvertToMatchupEntrys(this List<string> lines)
        {
            List<MatchupEntry> output = new List<MatchupEntry>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                MatchupEntry me = new MatchupEntry();
                me.Id = int.Parse(cols[0]);

                if (cols[1].Length == 0)
                    me.TeamCompeting = null;
                else
                    me.TeamCompeting = LookupTeamById(int.Parse(cols[1]));
                
                me.Score = double.Parse(cols[2]);

                if (int.TryParse(cols[3], out int parentId))
                    me.ParentMatchup = LookupMatchupById(parentId);
                else
                    me.ParentMatchup = null;

                output.Add(me);
            }

            return output;
        }
        
        public static List<MatchupEntry> ConvertStringToMatchupEntryModels(string lines)
        {
            string[] ids = lines.Split('|');
            List<MatchupEntry> output = new List<MatchupEntry>();
            List<string> entrys = GlobalConfig.MatchupEntryFile.FullFilePath().LoadFile();
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach (string entry in entrys)
                {
                    string[] cols = entry.Split(',');
                    if (cols[0] == id)
                    {
                        matchingEntries.Add(entry);
                    }
                }
            }

            output = matchingEntries.ConvertToMatchupEntrys();

            return output;
        }

        //todo
        private static Matchup LookupMatchupById(int id)
        {
            List<string> matchups = GlobalConfig
                .MatchupFile
                .FullFilePath()
                .LoadFile();

            foreach (string matchup in matchups)
            {
                string[] cols = matchup.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingMatchups = new List<string>();
                    matchingMatchups.Add(matchup);
                    return matchingMatchups.ConvertToMatchups().First();
                }
            }

            return null;
        }

        private static Team LookupTeamById(int id)
        {
            List<string> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile();


            foreach (string team in teams)
            {
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();
                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeams().First();
                }
            }

            return null;
        }

        public static List<Matchup> ConvertToMatchups(this List<string> lines)
        {
            List<Matchup> output = new List<Matchup>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                Matchup m = new Matchup();
                m.Id = int.Parse(cols[0]);
                m.Entrys = ConvertStringToMatchupEntryModels(cols[1]);

                if (cols[2].Length == 0)
                    m.Winner = null;
                else
                    m.Winner = LookupTeamById(int.Parse(cols[2]));

                m.MatchupRound = int.Parse(cols[3]);
                output.Add(m);
            }
            return output;
        }

        public static void SaveMatchupToFile(this Matchup matchup)
        {
            List<Matchup> matchups = GlobalConfig.MatchupFile
                .FullFilePath()
                .LoadFile()
                .ConvertToMatchups();

            int currentId = 1;
            if (matchups.Count > 0)
            {
                currentId = matchups.OrderByDescending(x => x.Id).First().Id + 1;
            }
            matchup.Id = currentId;

            matchups.Add(matchup);
        
            foreach (MatchupEntry entry in matchup.Entrys)
            {
                entry.SaveEntryToFile();
            }

            // save to matchup file
            List<string> lines = new List<string>();
            foreach (Matchup m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{ m.Id },{ ConvertMatchupEntryListToString(m.Entrys) },{ winner },{ m.MatchupRound} ");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void UpdateMatchupToFile(this Matchup matchup)
        {
            List<Matchup> matchups = GlobalConfig.MatchupFile
               .FullFilePath()
               .LoadFile()
               .ConvertToMatchups();


            Matchup oldMatchup = new Matchup();
            foreach (Matchup m in matchups)
            {
                if (m.Id == matchup.Id)
                {
                    oldMatchup = m;
                }
            }
            matchups.Remove(oldMatchup);


            matchups.Add(matchup);
            foreach (MatchupEntry entry in matchup.Entrys)
            {
                entry.UpdateEntryToFile();
            }

            // save to matchup file
            List<string> lines = new List<string>();
            foreach (Matchup m in matchups)
            {
                string winner = "";
                if (m.Winner != null)
                {
                    winner = m.Winner.Id.ToString();
                }
                lines.Add($"{ m.Id },{ ConvertMatchupEntryListToString(m.Entrys) },{ winner },{ m.MatchupRound} ");
            }
            File.WriteAllLines(GlobalConfig.MatchupFile.FullFilePath(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntry entry)
        {
            List<MatchupEntry> entries = GlobalConfig.MatchupEntryFile
                .FullFilePath()
                .LoadFile()
                .ConvertToMatchupEntrys();

            int currentId = 1;
            if (entries.Count > 0)
            {
                currentId = entries.OrderByDescending(x => x.Id).First().Id + 1;
            }
            entry.Id = currentId;
            entries.Add(entry);

            // save to entries file
            List<string> lines = new List<string>();
            foreach (MatchupEntry e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{e.Id},{teamCompeting},{e.Score},{parent}");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        public static void UpdateEntryToFile(this MatchupEntry entry)
        {
            List<MatchupEntry> entries = GlobalConfig.MatchupEntryFile
                .FullFilePath()
                .LoadFile()
                .ConvertToMatchupEntrys();

            MatchupEntry oldEntry = new MatchupEntry();

            foreach (MatchupEntry e in entries)
            {
                if (e.Id == entry.Id)
                {
                    oldEntry = e;
                }
            }

            entries.Remove(oldEntry);

            entries.Add(entry);

            // save to entries file
            List<string> lines = new List<string>();
            foreach (MatchupEntry e in entries)
            {
                string parent = "";
                if (e.ParentMatchup != null)
                {
                    parent = e.ParentMatchup.Id.ToString();
                }
                string teamCompeting = "";
                if (e.TeamCompeting != null)
                {
                    teamCompeting = e.TeamCompeting.Id.ToString();
                }
                lines.Add($"{e.Id},{teamCompeting},{e.Score},{parent}");
            }

            File.WriteAllLines(GlobalConfig.MatchupEntryFile.FullFilePath(), lines);
        }

        public static List<Tournament> ConvertToTournaments(
            this List<string> lines)
        {
            //id,Name,Fee,(id|id|id - EnteredTeams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)

            List<Tournament> output = new List<Tournament>();
            List<Team> teams = GlobalConfig.TeamFile.FullFilePath().LoadFile().ConvertToTeams();
            List<Prize> prizes = GlobalConfig.PrizesFile.FullFilePath().LoadFile().ConvertToPrizes();
            List<Matchup> matchups = GlobalConfig.MatchupFile.FullFilePath().LoadFile().ConvertToMatchups();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                Tournament t = new Tournament();
                t.Id = int.Parse(cols[0]);
                t.TournamentName = cols[1];
                t.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split('|');
                foreach (string id in teamIds)
                {
                    t.EnteredTeams.Add(teams
                        .Where(x => x.Id == int.Parse(id))
                        .First());
                }

                if (cols[4].Length > 0)
                {
                    string[] prizeIds = cols[4].Split('|');
                    foreach (string id in prizeIds)
                    {
                        t.Prizes.Add(prizes
                            .Where(x => x.Id == int.Parse(id))
                            .First());
                    } 
                }

                // capture rounds information
                string[] rounds = cols[5].Split('|');
                foreach (string round in rounds)
                {
                    string[] msText = round.Split('^');
                    List<Matchup> ms = new List<Matchup>();
                    foreach (string matchupModelTextId in msText)
                    {
                        ms.Add(matchups
                            .Where(x => x.Id == int.Parse(matchupModelTextId))
                            .First());
                    }
                    t.Rounds.Add(ms);
                }
                output.Add(t);
            }

            return output;
        }

        private static string ConvertPeopleListToSting(List<Person> people)
        {
            string output = "";

            if (people.Count == 0) return "";

            foreach (Person p in people)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertTeamListToString(List<Team> teams)
        {
            string output = "";

            if (teams.Count == 0) return "";

            foreach (Team p in teams)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupEntryListToString(List<MatchupEntry> entries)
        {
            string output = "";

            if (entries.Count == 0) return "";

            foreach (MatchupEntry e in entries)
            {
                output += $"{e.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertPrizeListToString(List<Prize> prizes)
        {
            string output = "";

            if (prizes.Count == 0) return "";

            foreach (Prize p in prizes)
            {
                output += $"{p.Id}|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertRoundListToString(List<List<Matchup>> rounds)
        {
            string output = "";

            if (rounds.Count == 0) return "";

            foreach (List<Matchup> p in rounds)
            {
                output += $"{ ConvertMatchupListToString(p) }|";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }

        private static string ConvertMatchupListToString(List<Matchup> matchups)
        {
            string output = "";

            if (matchups.Count == 0) return "";

            foreach (Matchup m in matchups)
            {
                output += $"{m.Id}^";
            }
            output = output.Substring(0, output.Length - 1);

            return output;
        }
    }
}