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
    public partial class TournamentViewerForm : Form
    {
        private Tournament Tournament;
        BindingList<int> rounds = new BindingList<int>();
        BindingList<Matchup> selectedMatchups = new BindingList<Matchup>();

        public TournamentViewerForm(Tournament tournament)
        {
            InitializeComponent();

            Tournament = tournament;
            
            WireUpLists();

            LoadFormData();

            LoadRounds();
        }

        private void LoadFormData()
        {
            TournamentName.Text = Tournament.TournamentName;
        }

        private void LoadRounds()
        {
            rounds.Clear();

            rounds.Add(1);
            int currRound = 1;

            foreach (List<Matchup> matchups in Tournament.Rounds)
            {
                if (matchups.First().MatchupRound > currRound)
                {
                    currRound = matchups.First().MatchupRound;
                    rounds.Add(currRound);
                }
            }

            LoadMatchups(1);
        }

        private void WireUpLists()
        {
            RoundDropdown.DataSource = rounds;
            matchupListBox.DataSource = selectedMatchups;
            matchupListBox.DisplayMember = nameof(Matchup.DisplayName);

        }

        private void RoundDropdown_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)RoundDropdown.SelectedItem);
        }

        private void LoadMatchups(int round)
        {

            foreach (List<Matchup> matchups in Tournament.Rounds)
            {
                if (matchups.First().MatchupRound == round)
                {
                    selectedMatchups.Clear();
                    foreach (Matchup m in matchups)
                    {
                        if (m.Winner == null || !UnplayedOnlyCheckbox.Checked)
                        {
                            selectedMatchups.Add(m);
                        }
                    }
                }
            }

            if (selectedMatchups.Count > 0)
            {
                LoadMatchup(selectedMatchups.First()); 
            }

            DisplayMatchupInfo();

        }

        private void DisplayMatchupInfo()
        {
            bool isVisible = (selectedMatchups.Count > 0);

            teamOneNameLable.Visible = isVisible;
            teamOneScoreLable.Visible = isVisible;
            teamOneScoreValue.Visible = isVisible;

            teamTwoNameLable.Visible = isVisible;
            teamTwoScoreLable.Visible = isVisible;
            teamTwoScoreValue.Visible = isVisible;

            vsLable.Visible = isVisible;
            scoreButton.Visible = isVisible;
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (matchupListBox.SelectedItem != null)
            {
                LoadMatchup(matchupListBox.SelectedItem as Matchup);
            }
        }

        private void LoadMatchup(Matchup m)
        {

            for (int i = 0; i < m.Entrys.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entrys[0].TeamCompeting != null)
                    {
                        teamOneNameLable.Text = m.Entrys[0].TeamCompeting.TeamName;
                        teamOneScoreValue.Text = m.Entrys[0].Score.ToString();

                        teamTwoNameLable.Text = "<bye>";
                        teamTwoScoreValue.Text = "0";
                    }
                    else
                    {
                        teamOneNameLable.Text = "Not yet set";
                        teamOneScoreValue.Text = string.Empty;
                    }
                }

                if (i == 1)
                {
                    if (m.Entrys[1].TeamCompeting != null)
                    {
                        teamTwoNameLable.Text = m.Entrys[1].TeamCompeting.TeamName;
                        teamTwoScoreValue.Text = m.Entrys[1].Score.ToString();
                    }
                    else
                    {
                        teamTwoNameLable.Text = "Not yet set";
                        teamTwoScoreValue.Text = string.Empty;
                    }
                }
            }
        }

        private void UnplayedOnlyCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            LoadMatchups((int)RoundDropdown.SelectedItem);
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            Matchup m = matchupListBox.SelectedItem as Matchup;
            double teamOneScore = 0;
            double teamTwoScore = 0;


            for (int i = 0; i < m.Entrys.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entrys[0].TeamCompeting != null)
                    {
                        bool scoreValid = double.TryParse(teamOneScoreValue.Text, out teamOneScore);
                        if (scoreValid)
                        {
                            m.Entrys[0].Score = teamOneScore;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team one!");
                            return;
                        }
                    }
                }

                if (i == 1)
                {
                    if (m.Entrys[1].TeamCompeting != null)
                    {
                        bool scoreValid = double.TryParse(teamTwoScoreValue.Text, out teamTwoScore);
                        if (scoreValid)
                        {
                            m.Entrys[1].Score = teamTwoScore;
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid score for team two!");
                            return;
                        }
                    }
                }
            }

            TournamentLogic.UpdateTournamentResults(Tournament);

            LoadMatchups((int)RoundDropdown.SelectedItem);
        }
    }
}
