using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class MatchupEntry
    {
        public Team TeamCompeting { get; set; }
        public double Score { get; set; }
        public Matchup ParentMatchup { get; set; }
    }
}
