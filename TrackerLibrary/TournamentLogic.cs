using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerLibrary
{
    public static class TournamentLogic
    {
        public static void CreateRounds(Tournament model)
        {
            // Order our List randomly of teams ;
            // check if is big enough - if not, add byes (2*2*2*2 - 2^n) ;
            // Create our first round of matchups ;
            // Create every Round after that - 8 Matchups - 4 Matchups - 2 Matchups - 1 Matchup ;
            List<Team> randomizedTeams = RandomizeTeamOrder(model.EnteredTeams);
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes(rounds, randomizedTeams.Count);

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
        }

        private static void CreateOtherRounds(Tournament model, int rounds)
        {
            int round = 2;
            List<Matchup> previousRound = model.Rounds[0];
            List<Matchup> currRound = new List<Matchup>();
            Matchup currMatchup = new Matchup();

            while (round <= rounds)
            {
                foreach (Matchup match in previousRound)
                {
                    currMatchup.Entrys.Add(new MatchupEntry { ParentMatchup = match });

                    if (currMatchup.Entrys.Count > 1)
                    {
                        currMatchup.MatchupRound = round;
                        currRound.Add(currMatchup);
                        currMatchup = new Matchup();
                    }
                }
                model.Rounds.Add(currRound);
                previousRound = currRound;

                currRound = new List<Matchup>();
                round += 1;
            }
        }

        private static List<Matchup> CreateFirstRound(int byes, List<Team> teams)
        {
            List<Matchup> output = new List<Matchup>();
            Matchup currentMatchup = new Matchup();

            foreach (Team team in teams)
            {
                currentMatchup.Entrys.Add(new MatchupEntry { TeamCompeting = team });

                if(byes > 0 || currentMatchup.Entrys.Count > 1)
                {
                    currentMatchup.MatchupRound = 1;
                    output.Add(currentMatchup);
                    currentMatchup = new Matchup();

                    if (byes >= 1)
                    {
                        byes -= 1;
                    }
                }
            }
            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;
            return output;
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }

            return output;
        }

        private static List<Team> RandomizeTeamOrder(List<Team> list)
        {
            return list.OrderBy(a => Guid.NewGuid()).ToList();
        }

        public static void UpdateTournamentResults(Tournament model)
        {
            int startingRound = model.CheckCurrentRound();
            List<Matchup> toScore = new List<Matchup>();
            foreach (List<Matchup> round in model.Rounds)
            {
                foreach (Matchup rm in round)
                {
                    if (rm.Winner == null && (rm.Entrys.Any(x => x.Score != 0) || rm.Entrys.Count == 1))
                    {
                        toScore.Add(rm);
                    }
                }
            }

            MarkWinnerInMatchup(toScore);
            AdvanceWinners(toScore, model);


            toScore.ForEach(x => GlobalConfig.Connection.UpdateMatchup(x));

            int endingRound = model.CheckCurrentRound();
            if (endingRound > startingRound)
            {
                model.AlertUsersToNewRound();
            }
        }

        public static void AlertUsersToNewRound(this Tournament model)
        {
            int currentRoundNum = model.CheckCurrentRound();
            List<Matchup> currentRound = model.Rounds.Where(x => x.First().MatchupRound == currentRoundNum).First();

            foreach (Matchup matchup in currentRound)
            {
                foreach (MatchupEntry me in matchup.Entrys)
                {
                    foreach (Person person in me.TeamCompeting.TeamMembers)
                    {
                        AlertPersonToNewRound(person, me.TeamCompeting.TeamName, matchup.Entrys.Where(y=>y.TeamCompeting != me.TeamCompeting).FirstOrDefault());
                    }
                }
            }
        }

        private static void AlertPersonToNewRound(Person person, string teamName, MatchupEntry competitor)
        {
            if (person.EmailAdress.Length == 0)
                return;
            
            string toAdress = "";
            string subject = "";
            StringBuilder body = new StringBuilder();

            if (competitor != null)
            {
                subject = $"You have a new matchup with {competitor.TeamCompeting.TeamName}"; body.AppendLine("<h1>You have a new matchup</h1>");
                body.Append("<strong>Competitor: </strong>");
                body.Append(competitor.TeamCompeting.TeamName);
                body.AppendLine();
                body.AppendLine("Have a great time!");
                body.AppendLine("~Tournament Tracker");
            }
            else
            {
                subject = "You have a bye week this round";
                body.AppendLine("Enjoy your week off!");
                body.AppendLine("~Tournament Tracker");
            }

            toAdress = person.EmailAdress;

            EmailLogic.SendEmail(toAdress, subject, body.ToString());
        }

        private static int CheckCurrentRound(this Tournament model)
        {
            int output = 1;
            foreach (List<Matchup> round in model.Rounds)
            {
                if (round.All(x => x.Winner != null))
                {
                    output += 1;
                }
            }

            return output;
        }

        private static void MarkWinnerInMatchup(List<Matchup> models)
        {
            // greater or lesser
            string greaterWins = ConfigurationManager.AppSettings["greaterWins"];
            
            foreach (Matchup m in models)
            {
                // Checks for bye week entry
                if (m.Entrys.Count == 1)
                {
                    m.Winner = m.Entrys[0].TeamCompeting;
                    // this loop is done
                    continue;
                }

                // 0 means false of lower wins
                if (greaterWins == "0")
                {
                    if (m.Entrys[0].Score < m.Entrys[1].Score)
                    {
                        m.Winner = m.Entrys[0].TeamCompeting;
                    }
                    else if (m.Entrys[1].Score < m.Entrys[0].Score)
                    {
                        m.Winner = m.Entrys[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allwo ties in this Application.");
                    }
                }
                // 1 means true, or greater wins
                else 
                {
                    if (m.Entrys[0].Score > m.Entrys[1].Score)
                    {
                        m.Winner = m.Entrys[0].TeamCompeting;
                    }
                    else if (m.Entrys[1].Score > m.Entrys[0].Score)
                    {
                        m.Winner = m.Entrys[1].TeamCompeting;
                    }
                    else
                    {
                        throw new Exception("We do not allwo ties in this Application.");
                    }
                } 
            }
        }

        private static void AdvanceWinners(List<Matchup> models, Tournament tournament)
        {
            foreach (Matchup m in models)
            {
                foreach (List<Matchup> round in tournament.Rounds)
                {
                    foreach (Matchup rm in round)
                    {
                        foreach (MatchupEntry me in rm.Entrys)
                        {
                            if (me.ParentMatchup != null)
                            {
                                if (me.ParentMatchup.Id == m.Id)
                                {
                                    me.TeamCompeting = m.Winner;
                                    GlobalConfig.Connection.UpdateMatchup(rm);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
