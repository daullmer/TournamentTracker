using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using TrackerLibrary.Models;
using Dapper;
using System.Linq;

namespace TrackerLibrary.DataAccess
{
    public class SQLConnector : IDataConnection
    {
        private const string db = "Tournaments";

        /// <summary>
        /// Saves a new prize to the database
        /// </summary>
        /// <param name="model">The prize information.</param>
        /// <returns>The prize information, including the Id</returns>
        public Prize CreatePrize(Prize model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
                return model;
            }
        }

        public Person CreatePerson(Person model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAdress", model.EmailAdress);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
                return model;
            }
        }

        public List<Person> GetPerson_All()
        {
            List<Person> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<Person>("dbo.spPeople_GetAll").ToList();
            }

            return output;
        }

        public Team CreateTeam(Team model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@TeamName", model.TeamName);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", param, commandType: CommandType.StoredProcedure);

                model.Id = param.Get<int>("@id");

                foreach (Person p in model.TeamMembers)
                {
                    param = new DynamicParameters();
                    param.Add("@TeamId", model.Id);
                    param.Add("@PersonId", p.Id);

                    connection.Execute("dbo.spTeamMember_Insert", param, commandType: CommandType.StoredProcedure);
                }

                return model;
            }
        }

        public List<Team> GetTeam_All()
        {
            List<Team> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<Team>("dbo.spTeams_GetAll").ToList();

                foreach (Team team in output)
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<Person>("dbo.spTeamMembers_GetByTeam", param, commandType: CommandType.StoredProcedure).ToList();
                }
            }

            return output;
        }

        public void CreateTournament(Tournament model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                SaveTournament(model, connection);
                SaveTournamentPrizes(model, connection);
                SaveTournamentEnterys(model, connection);
                SaveTournamentRounds(model, connection);
            }
        }

        private static void SaveTournament(Tournament model, IDbConnection connection)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@TournamentName", model.TournamentName);
            param.Add("@EntreryFee", model.EntryFee);
            param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournament_Insert", param, commandType: CommandType.StoredProcedure);

            model.Id = param.Get<int>("@id");
        }

        private static void SaveTournamentPrizes(Tournament model, IDbConnection connection)
        {
            foreach (Prize p in model.Prizes)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@TournamentId", model.Id);
                param.Add("@PrizeId", p.Id);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                connection.Execute("dbo.spTournamentPrizes_Insert", param, commandType: CommandType.StoredProcedure);
            }
        }

        private static void SaveTournamentEnterys(Tournament model, IDbConnection connection)
        {
            foreach (Team t in model.EnteredTeams)
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("@TournamentId", model.Id);
                param.Add("@TeamId", t.Id);
                param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                connection.Execute("dbo.spTournamentEnterys_Insert", param, commandType: CommandType.StoredProcedure);
            }
        }

        private static void SaveTournamentRounds(Tournament tournament, IDbConnection connection)
        {
            //List<List<Matchup>> Rounds
            //List<MatchupEntry> Enterys

            //Looping through the rounds
                //Loop through the Matchups
                //Save the matchup
                //Loop throught the Enterys

            foreach (List<Matchup> round in tournament.Rounds)
            {
                foreach (Matchup matchup in round)
                {
                    DynamicParameters param = new DynamicParameters();
                    param.Add("@TournamentId", tournament.Id);
                    param.Add("@MatchupRound", matchup.MatchupRound);
                    param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                    connection.Execute("dbo.spMatchups_Insert", param, commandType: CommandType.StoredProcedure);

                    matchup.Id = param.Get<int>("@id");

                    foreach (MatchupEntry entery in matchup.Entrys)
                    {
                        param = new DynamicParameters();
                        param.Add("@MatchupId", matchup.Id);


                        if (entery.ParentMatchup == null)
                            param.Add("@ParentMatchupId", null);
                        else
                            param.Add("@ParentMatchupId", entery.ParentMatchup.Id);


                        if (entery.TeamCompeting == null)
                            param.Add("@TeamCompetingId", null);
                        else
                            param.Add("@TeamCompetingId", entery.TeamCompeting.Id);


                        param.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);


                        connection.Execute("dbo.spMatchupEntry_Insert", param, commandType: CommandType.StoredProcedure);
                    }
                }
            }
        }
    }
}