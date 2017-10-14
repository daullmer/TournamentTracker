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
    public partial class TournamentDashbordForm : Form
    {

        List<Tournament> tournaments = GlobalConfig.Connection.GetTournament_All();
        
        public TournamentDashbordForm()
        {
            InitializeComponent();

            WireUpList();
        }

        private void WireUpList()
        {
            loadTournamentDropdown.DataSource = tournaments;
            loadTournamentDropdown.DisplayMember = nameof(Tournament.TournamentName);
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm frm = new CreateTournamentForm();
            frm.Show();
        }

        private void loadTournamentButton_Click(object sender, EventArgs e)
        {
            Tournament tm = loadTournamentDropdown.SelectedItem as Tournament;
            TournamentViewerForm frm = new TournamentViewerForm(tm);
            frm.Show();
        }
    }
}
