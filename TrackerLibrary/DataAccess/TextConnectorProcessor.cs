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

        public static void SaveToPrizeFile(this List<Prize> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Prize p in models)
            {
                lines.Add($"{ p.Id },{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePercentage}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
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

        public static void SaveToPeopleFile(this List<Person> models, string fileName)
        {
            List<string> lines = new List<string>();

            foreach (Person p in models)
            {
                lines.Add($"{ p.Id },{p.FirstName},{p.LastName},{p.EmailAdress}");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<Team> ConvertToTeams(this List<string> lines, string peopleFileName)
        {
            List<Team> output = new List<Team>();
            List<Person> people = peopleFileName.FullFilePath().LoadFile().ConvertToPerson();

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

        public static void SaveToTeamFile(this List<Team> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (Team t in models)
            {
                lines.Add($"{t.Id},{t.TeamName},{ConvertPeopleListToSting(t.TeamMembers)}");
            }

            File.WriteAllLines(filename.FullFilePath(), lines);
        }

        public static void SaveToTournamentFile(this List<Tournament> model, string fileName)
        {
            List<string> lines = new List<string>();
            foreach (Tournament t in model)
            {
                lines.Add($@"{ t.Id },
                    { t.TournamentName },
                    { t.EntryFee },
                    { ConvertTeamListToString(t.EnteredTeams) },
                    { ConvertPrizeListToString(t.Prizes) },
                    { ConvertRoundListToString(t.Rounds) }");
            }

            File.WriteAllLines(fileName.FullFilePath(), lines);
        }

        public static List<Tournament> ConvertToTournaments(
            this List<string> lines, string teamFile, string peopleFile, string prizesFile)
        {
            //id,Name,Fee,(id|id|id - EnteredTeams),(id|id|id - Prizes),(Rounds - id^id^id|id^id^id|id^id^id)

            List<Tournament> output = new List<Tournament>();
            List<Team> teams = teamFile.FullFilePath().LoadFile().ConvertToTeams(peopleFile);
            List<Prize> prizes = prizesFile.FullFilePath().LoadFile().ConvertToPrizes();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                Tournament t = new Tournament();
                t.Id = int.Parse(cols[0]);
                t.TournamentName = cols[1];
                t.EntryFee = decimal.Parse(cols[2]);

                string[] teamIds = cols[3].Split(',');
                foreach (string id in teamIds)
                {
                    t.EnteredTeams.Add(teams
                        .Where(x => x.Id == int.Parse(id))
                        .First());
                }

                string[] prizeIds = cols[4].Split('|');
                foreach (string id in prizeIds)
                {
                    t.Prizes.Add(prizes
                        .Where(x => x.Id == int.Parse(id))
                        .First());
                }

                // todo capture rounds information
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
