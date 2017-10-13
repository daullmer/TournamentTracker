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

        public override string ToString()
        {
            return $"Runde: {MatchupRound}";
        }
    }
}
