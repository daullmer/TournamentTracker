namespace TrackerUI
{
    partial class TournamentDashbordForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.createTournament = new System.Windows.Forms.Label();
            this.loadTournamentDropdown = new System.Windows.Forms.ComboBox();
            this.selectTeamLable = new System.Windows.Forms.Label();
            this.loadTournamentButton = new System.Windows.Forms.Button();
            this.createTournamentButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // createTournament
            // 
            this.createTournament.AutoSize = true;
            this.createTournament.Font = new System.Drawing.Font("Segoe UI Light", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createTournament.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.createTournament.Location = new System.Drawing.Point(65, 9);
            this.createTournament.Name = "createTournament";
            this.createTournament.Size = new System.Drawing.Size(367, 50);
            this.createTournament.TabIndex = 11;
            this.createTournament.Text = "Tournament Dashbord";
            // 
            // loadTournamentDropdown
            // 
            this.loadTournamentDropdown.FormattingEnabled = true;
            this.loadTournamentDropdown.Location = new System.Drawing.Point(111, 111);
            this.loadTournamentDropdown.Name = "loadTournamentDropdown";
            this.loadTournamentDropdown.Size = new System.Drawing.Size(283, 38);
            this.loadTournamentDropdown.TabIndex = 18;
            // 
            // selectTeamLable
            // 
            this.selectTeamLable.AutoSize = true;
            this.selectTeamLable.BackColor = System.Drawing.Color.White;
            this.selectTeamLable.Font = new System.Drawing.Font("Segoe UI", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.selectTeamLable.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.selectTeamLable.Location = new System.Drawing.Point(131, 68);
            this.selectTeamLable.Name = "selectTeamLable";
            this.selectTeamLable.Size = new System.Drawing.Size(240, 40);
            this.selectTeamLable.TabIndex = 17;
            this.selectTeamLable.Text = "Load Tournament";
            // 
            // loadTournamentButton
            // 
            this.loadTournamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.loadTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.loadTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.loadTournamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.loadTournamentButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadTournamentButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.loadTournamentButton.Location = new System.Drawing.Point(148, 155);
            this.loadTournamentButton.Name = "loadTournamentButton";
            this.loadTournamentButton.Size = new System.Drawing.Size(209, 41);
            this.loadTournamentButton.TabIndex = 26;
            this.loadTournamentButton.Text = "Load Tournament";
            this.loadTournamentButton.UseVisualStyleBackColor = true;
            this.loadTournamentButton.Click += new System.EventHandler(this.loadTournamentButton_Click);
            // 
            // createTournamentButton
            // 
            this.createTournamentButton.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.createTournamentButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(102)))), ((int)(((byte)(102)))), ((int)(((byte)(102)))));
            this.createTournamentButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.createTournamentButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.createTournamentButton.Font = new System.Drawing.Font("Segoe UI Semibold", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.createTournamentButton.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(51)))), ((int)(((byte)(153)))), ((int)(((byte)(255)))));
            this.createTournamentButton.Location = new System.Drawing.Point(127, 261);
            this.createTournamentButton.Name = "createTournamentButton";
            this.createTournamentButton.Size = new System.Drawing.Size(250, 71);
            this.createTournamentButton.TabIndex = 27;
            this.createTournamentButton.Text = "Create Tournament";
            this.createTournamentButton.UseVisualStyleBackColor = true;
            this.createTournamentButton.Click += new System.EventHandler(this.createTournamentButton_Click);
            // 
            // TournamentDashbordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(512, 347);
            this.Controls.Add(this.createTournamentButton);
            this.Controls.Add(this.loadTournamentButton);
            this.Controls.Add(this.loadTournamentDropdown);
            this.Controls.Add(this.selectTeamLable);
            this.Controls.Add(this.createTournament);
            this.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.Name = "TournamentDashbordForm";
            this.Text = "Tournament Dashbord";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label createTournament;
        private System.Windows.Forms.ComboBox loadTournamentDropdown;
        private System.Windows.Forms.Label selectTeamLable;
        private System.Windows.Forms.Button loadTournamentButton;
        private System.Windows.Forms.Button createTournamentButton;
    }
}