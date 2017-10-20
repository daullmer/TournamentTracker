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
    public partial class CreateTeamForm : Form
    {
        private List<Person> availableTeamMembers = GlobalConfig.Connection.GetPerson_All();
        private List<Person> selectedTeamMembers = new List<Person>();
        private ITeamRequestor callingForm;

        public CreateTeamForm(ITeamRequestor caller)
        {
            InitializeComponent();
            callingForm = caller;
            //createsampledata();
            WireUpLists();
        }

        private void CreateSampleData()
        {
            availableTeamMembers.Add(new Person { FirstName = "David", LastName = "Ullmer" });
            availableTeamMembers.Add(new Person { FirstName = "Sara", LastName = "Ullmer" });

            selectedTeamMembers.Add(new Person { FirstName = "Frank", LastName = "Ullmer" });
            selectedTeamMembers.Add(new Person { FirstName = "Gitta", LastName = "Ullmer" });
        }

        private void WireUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;
            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = nameof(Person.FullName);

            teamMembersListBox.DataSource = null;
            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = nameof(Person.FullName);
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                Person p = new Person();
                p.FirstName = firstNameValue.Text;
                p.LastName = lastNameValue.Text;
                p.EmailAdress = emailValue.Text;

                GlobalConfig.Connection.CreatePerson(p);
                selectedTeamMembers.Add(p);
                WireUpLists();

                firstNameValue.Text = "";
                lastNameValue.Text = "";
                emailValue.Text = "";
            }
            else
            {
                MessageBox.Show("You need to fill in all the fields.");
            }
        }

        private bool ValidateForm()
        {
            if (firstNameValue.Text.Length == 0) return false;
            if (lastNameValue.Text.Length == 0) return false;
            if (emailValue.Text.Length == 0) return false;
            return true;
        }

        private void addTeamMemberButton_Click(object sender, EventArgs e)
        {
            Person selected = selectTeamMemberDropDown.SelectedItem as Person;

            if (selected != null)
            {
                availableTeamMembers.Remove(selected);
                selectedTeamMembers.Add(selected);

                WireUpLists(); 
            }
        }

        private void memberRemove_Click(object sender, EventArgs e)
        {
            Person selected = teamMembersListBox.SelectedItem as Person;

            if (selected != null)
            {
                availableTeamMembers.Add(selected);
                selectedTeamMembers.Remove(selected);

                WireUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            Team t = new Team
            {
                TeamName = teamNameValue.Text,
                TeamMembers = selectedTeamMembers
            };

            GlobalConfig.Connection.CreateTeam(t);

            callingForm.TeamComplete(t);

            Close();
        }
    }
}
