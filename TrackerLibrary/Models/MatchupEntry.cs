using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class MatchupEntry
    {
        public int Id { get; set; }
        public Team TeamCompeting { get; set; }
        public double Score { get; set; }
        public Matchup ParentMatchup { get; set; }

        public override string ToString()
        {
            if (TeamCompeting != null)
            {
                return "Team:" + TeamCompeting.TeamName;
            }
            else
            {
                return "Team steht noch nicht fest";
            }
        }

    }
}
