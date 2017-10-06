using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary.Models
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; }
        public List<Person> TeamMembers { get; set; } = new List<Person>();

        public override string ToString() => TeamName;
    }
}
