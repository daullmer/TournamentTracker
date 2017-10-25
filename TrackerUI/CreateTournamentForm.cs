using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLibrary;
using TrackerLibrary.Models;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequestor, ITeamRequestor
    {
        List<Team> availableTeams = GlobalConfig.Connection.GetTeam_All();
        List<Team> selectedTeams = new List<Team>();
        List<Prize> selectedPrizes = new List<Prize>();

        public CreateTournamentForm()
        {
            InitializeComponent();
            WireUpList();
        }

        private void WireUpList()
        {
            selectTeamDropDown.DataSource = null;
            tournamentTeamsListBox.DataSource = null;
            prizesListBox.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = nameof(Team.TeamName);

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = nameof(Team.TeamName);

            prizesListBox.DataSource = selectedPrizes;
            prizesListBox.DisplayMember = nameof(Prize.PlaceName);
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            Team selected = selectTeamDropDown.SelectedItem as Team;

            if (selected != null)
            {
                availableTeams.Remove(selected);
                selectedTeams.Add(selected);
            }

            WireUpList();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // call the create prize form
            // Get back from the form a prize
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
            //Take the prize and put it in the List
        }

        public void PrizeComplete(Prize model)
        {
            selectedPrizes.Add(model);
            WireUpList();
        }

        public void TeamComplete(Team model)
        {
            selectedTeams.Add(model);
            WireUpList();
        }

        private void createTeamLable_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();

        }

        private void teamDelete_Click(object sender, EventArgs e)
        {
            Team selected = tournamentTeamsListBox.SelectedItem as Team;

            if (selected != null)
            {
                availableTeams.Add(selected);
                selectedTeams.Remove(selected);

                WireUpList();
            }
        }

        private void prizesDelete_Click(object sender, EventArgs e)
        {
            Prize selected = prizesListBox.SelectedItem as Prize;

            if (selected != null)
            {
                selectedPrizes.Remove(selected);

                WireUpList();
            }
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // Validate data
            bool feeAcceptable = decimal.TryParse(entryFeeValue.Text, out decimal fee);

            if (!feeAcceptable)
            {
                MessageBox.Show("You need to enter a valid fee",
                    "Invalid Fee",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                return;
            }

            // Create our Tounrnament mode
            Tournament tm = new Tournament
            {
                TournamentName = tournamentNameValue.Text,
                EntryFee = fee,
                Prizes = selectedPrizes,
                EnteredTeams = selectedTeams
            };


            // wire up Matchups
            TournamentLogic.CreateRounds(tm);

            // Create Tournament Entry
            // Create all of the PrizesEntrys
            // Create all of the TeamEntrys
            GlobalConfig.Connection.CreateTournament(tm);
            tm.AlertUsersToNewRound();

            TournamentViewerForm frm = new TournamentViewerForm(tm);
            frm.Show();
            this.Close();
        }
    }
}