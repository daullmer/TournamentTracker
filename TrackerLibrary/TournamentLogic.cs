using System;
using System.Collections.Generic;
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
    }
}
