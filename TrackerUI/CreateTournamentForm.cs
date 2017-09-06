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
    public partial class CreateTournamentForm : Form
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
    }
}
