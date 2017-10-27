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
        public void CreatePrize(Prize model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePercentage);
                p.Add("@id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@id");
            }
        }

        public void CreatePerson(Person model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAdress", model.EmailAdress);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPeople_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
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

        public void CreateTeam(Team model)
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
                TournamentLogic.UpdateTournamentResults(model);
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

        public List<Tournament> GetTournament_All()
        {
            List<Tournament> output;
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                output = connection.Query<Tournament>("dbo.spTournaments_GetAll").ToList();
                DynamicParameters p = new DynamicParameters();

                foreach (Tournament t in output)
                {
                    // populate prizes
                    p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);

                    t.Prizes = connection.Query<Prize>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    // populate teams
                    t.EnteredTeams = connection.Query<Team>("dbo.spTeam_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();
                    foreach (Team team in t.EnteredTeams)
                    {
                        p = new DynamicParameters();
                        p.Add("@TeamId", team.Id);
                        team.TeamMembers = connection.Query<Person>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).ToList();
                    }

                    // populate rounds
                    p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);
                    List<Matchup> matchups = connection.Query<Matchup>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).ToList();

                    foreach (Matchup matchup in matchups)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", matchup.Id);
                        matchup.Entrys = connection.Query<MatchupEntry>("dbo.spMatchupEntries_GetByMatchup", p, commandType: CommandType.StoredProcedure).ToList();
                        

                        List<Team> allTeams = GetTeam_All();

                        // populate each matchup (1 model)
                        if (matchup.WinnerId > 0)
                        {
                            matchup.Winner = allTeams.Where(x => x.Id == matchup.WinnerId).First();
                        }

                        // populate each entry (2 models)
                        foreach (MatchupEntry me in matchup.Entrys)
                        {
                            if (me.TeamCompetingId > 0)
                            {
                                me.TeamCompeting = allTeams.Where(x => x.Id == me.TeamCompetingId).First();
                            }

                            if (me.ParentMatchupId > 0)
                            {
                                me.ParentMatchup = matchups.Where(x => x.Id == me.ParentMatchupId).First();
                            }
                        }
                        
                    }

                    // List<List<Matchup>>
                    List<Matchup> currentRow = new List<Matchup>();
                    int currentRound = 1;
                    foreach (Matchup matchup in matchups)
                    {
                        if (matchup.MatchupRound > currentRound)
                        {
                            t.Rounds.Add(currentRow);
                            currentRow = new List<Matchup>();
                            currentRound++;
                        }

                        currentRow.Add(matchup);
                    }
                    t.Rounds.Add(currentRow);
                }


            }

            return output;
        }

        public void UpdateMatchup(Matchup model)
        {
            // spMatchups_Update @Id @WinnerId
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                DynamicParameters param = new DynamicParameters();
                if (model.Winner != null)
                {
                    param.Add("@Id", model.Id);
                    param.Add("@WinnerId", model.Winner.Id);

                    connection.Execute("dbo.spMatchups_Update", param, commandType: CommandType.StoredProcedure); 
                }


                // spMatchupsEntries_Update

                foreach (MatchupEntry me in model.Entrys)
                {
                    if (me.TeamCompeting != null)
                    {
                        param = new DynamicParameters();
                        param.Add("@Id", me.Id);
                        param.Add("@TeamCompetingId", me.TeamCompeting.Id);
                        param.Add("@Score", me.Score);

                        connection.Execute("dbo.spMatchupEntries_Update", param, commandType: CommandType.StoredProcedure); 
                    }
                }
            }
        }

        public void CompleteTournament(Tournament model)
        {
            //spTournament_Update
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(db)))
            {
                var p = new DynamicParameters();
                p.Add("@Id", model.Id);

                connection.Execute("dbo.spTournament_Update", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}