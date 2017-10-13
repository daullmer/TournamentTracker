using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.DataAccess
{
    public interface IDataConnection
    {
        Prize CreatePrize(Prize model);
        Person CreatePerson(Person model);
        Team CreateTeam(Team model);
        void CreateTournament(Tournament model);
        List<Person> GetPerson_All();
        List<Team> GetTeam_All();
        List<Tournament> GetTournament_All();
    }
}
