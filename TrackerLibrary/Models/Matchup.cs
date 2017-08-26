﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class Matchup
    {
        public List<MatchupEntry> Entrys { get; set; } = new List<MatchupEntry>();
        public Team Winner { get; set; }
        public int MatchupRound { get; set; }
    }
}