using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        void CreatePrize(Prize model);
        void CreatePerson(Person model);
        void CreateTeam(Team model);
        void CreateTournament(Tournament model);
        void UpdateMatchup(Matchup model);
        void CompleteTournament(Tournament model);
        List<Person> GetPerson_All();
        List<Team> GetTeam_All();
        List<Tournament> GetTournament_All();
    }
}
