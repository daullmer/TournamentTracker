using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class Matchup
    {
        public int Id { get; set; }
        public List<MatchupEntry> Entrys { get; set; } = new List<MatchupEntry>();
        public Team Winner { get; set; }
        public int WinnerId { get; set; }
        public int MatchupRound { get; set; }
        public string DisplayName {
            get
            {
                string output = "";
                foreach (MatchupEntry me in Entrys)
                {
                    if (me.TeamCompeting != null)
                    {
                        if (output.Length == 0)
                        {
                            output = me.TeamCompeting.TeamName;
                        }
                        else
                        {
                            output += $" vs. {me.TeamCompeting.TeamName}";
                        } 
                    }
                    else
                    {
                        output = "Matchup not yet determined";
                        break;
                    }
                }
                return output;
            }

        }

        public override string ToString()
        {
            return $"Runde: {MatchupRound}";
        }
    }
}
