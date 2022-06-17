namespace Stream_Info_Handler
{
    partial class frm_bracket_assistant
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Name",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Twitter",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Pronouns",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sponsor",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Character",
            "",
            "",
            "",
            ""}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_bracket_assistant));
            this.cbx_round = new System.Windows.Forms.ComboBox();
            this.cbx_player1 = new System.Windows.Forms.ComboBox();
            this.cbx_player2 = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbx_player4 = new System.Windows.Forms.ComboBox();
            this.cbx_player3 = new System.Windows.Forms.ComboBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.lbl_player2 = new System.Windows.Forms.Label();
            this.lbl_player1 = new System.Windows.Forms.Label();
            this.lbl_bracket = new System.Windows.Forms.Label();
            this.btn_reset = new System.Windows.Forms.Button();
            this.btn_edit = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_push = new System.Windows.Forms.Button();
            this.btn_moveup = new System.Windows.Forms.Button();
            this.btn_movedown = new System.Windows.Forms.Button();
            this.lvw_matches = new System.Windows.Forms.ListView();
            this.clm_match = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_round = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tab_streamqueue = new System.Windows.Forms.TabPage();
            this.btn_player4 = new System.Windows.Forms.Button();
            this.btn_player3 = new System.Windows.Forms.Button();
            this.btn_player2 = new System.Windows.Forms.Button();
            this.btn_player1 = new System.Windows.Forms.Button();
            this.lvw_playerinfo = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tab_stationqueue = new System.Windows.Forms.TabPage();
            this.tab_autoseed = new System.Windows.Forms.TabPage();
            this.txt_players = new System.Windows.Forms.TextBox();
            this.lvw_seeding = new System.Windows.Forms.ListView();
            this.clm_seed = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_elo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_seedtag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_lock = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btn_link_event = new System.Windows.Forms.Button();
            this.cbx_host = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_tournament_url = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tab_streamqueue.SuspendLayout();
            this.tab_autoseed.SuspendLayout();
            this.SuspendLayout();
            // 
            // cbx_round
            // 
            this.cbx_round.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_round.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_round.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_round.FormattingEnabled = true;
            this.cbx_round.Items.AddRange(new object[] {
            "Grand Finals",
            "Winners Finals",
            "Winners Semifinals",
            "Winners Quarters",
            "Losers Finals",
            "Losers Semifinals",
            "Losers Quarters",
            "Losers Top 8",
            "Winners Round 1",
            "Winners Round 2",
            "Winners Round 3",
            "Winners Round 4",
            "Losers Round 1",
            "Losers Round 2",
            "Losers Round 3",
            "Losers Round 4",
            "Winners Top 16",
            "Winners Top 24",
            "Winners Top 32",
            "Winners Top 48",
            "Losers Top 16",
            "Losers Top 24",
            "Losers Top 32",
            "Losers Top 48"});
            this.cbx_round.Location = new System.Drawing.Point(9, 32);
            this.cbx_round.Name = "cbx_round";
            this.cbx_round.Size = new System.Drawing.Size(194, 28);
            this.cbx_round.TabIndex = 0;
            // 
            // cbx_player1
            // 
            this.cbx_player1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player1.FormattingEnabled = true;
            this.cbx_player1.Location = new System.Drawing.Point(227, 32);
            this.cbx_player1.Name = "cbx_player1";
            this.cbx_player1.Size = new System.Drawing.Size(201, 28);
            this.cbx_player1.TabIndex = 1;
            this.cbx_player1.TextChanged += new System.EventHandler(this.update_new_match);
            this.cbx_player1.Enter += new System.EventHandler(this.update_new_match);
            // 
            // cbx_player2
            // 
            this.cbx_player2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player2.FormattingEnabled = true;
            this.cbx_player2.Location = new System.Drawing.Point(462, 32);
            this.cbx_player2.Name = "cbx_player2";
            this.cbx_player2.Size = new System.Drawing.Size(204, 28);
            this.cbx_player2.TabIndex = 3;
            this.cbx_player2.TextChanged += new System.EventHandler(this.update_new_match);
            this.cbx_player2.Enter += new System.EventHandler(this.update_new_match);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbx_player4);
            this.groupBox1.Controls.Add(this.cbx_player3);
            this.groupBox1.Controls.Add(this.btn_add);
            this.groupBox1.Controls.Add(this.lbl_player2);
            this.groupBox1.Controls.Add(this.lbl_player1);
            this.groupBox1.Controls.Add(this.lbl_bracket);
            this.groupBox1.Controls.Add(this.cbx_player2);
            this.groupBox1.Controls.Add(this.cbx_round);
            this.groupBox1.Controls.Add(this.cbx_player1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(767, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a Match to Stream Queue";
            // 
            // cbx_player4
            // 
            this.cbx_player4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player4.FormattingEnabled = true;
            this.cbx_player4.Location = new System.Drawing.Point(462, 72);
            this.cbx_player4.Name = "cbx_player4";
            this.cbx_player4.Size = new System.Drawing.Size(204, 28);
            this.cbx_player4.TabIndex = 4;
            this.cbx_player4.TextChanged += new System.EventHandler(this.update_new_match);
            this.cbx_player4.Enter += new System.EventHandler(this.update_new_match);
            // 
            // cbx_player3
            // 
            this.cbx_player3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player3.FormattingEnabled = true;
            this.cbx_player3.Location = new System.Drawing.Point(227, 72);
            this.cbx_player3.Name = "cbx_player3";
            this.cbx_player3.Size = new System.Drawing.Size(201, 28);
            this.cbx_player3.TabIndex = 2;
            this.cbx_player3.TextChanged += new System.EventHandler(this.update_new_match);
            this.cbx_player3.Enter += new System.EventHandler(this.update_new_match);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(684, 32);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(77, 28);
            this.btn_add.TabIndex = 5;
            this.btn_add.Text = "Add Match";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // lbl_player2
            // 
            this.lbl_player2.AutoSize = true;
            this.lbl_player2.Location = new System.Drawing.Point(459, 16);
            this.lbl_player2.Name = "lbl_player2";
            this.lbl_player2.Size = new System.Drawing.Size(45, 13);
            this.lbl_player2.TabIndex = 7;
            this.lbl_player2.Text = "Player 2";
            // 
            // lbl_player1
            // 
            this.lbl_player1.AutoSize = true;
            this.lbl_player1.Location = new System.Drawing.Point(224, 16);
            this.lbl_player1.Name = "lbl_player1";
            this.lbl_player1.Size = new System.Drawing.Size(45, 13);
            this.lbl_player1.TabIndex = 6;
            this.lbl_player1.Text = "Player 1";
            // 
            // lbl_bracket
            // 
            this.lbl_bracket.AutoSize = true;
            this.lbl_bracket.Location = new System.Drawing.Point(6, 16);
            this.lbl_bracket.Name = "lbl_bracket";
            this.lbl_bracket.Size = new System.Drawing.Size(90, 13);
            this.lbl_bracket.TabIndex = 5;
            this.lbl_bracket.Text = "Round in Bracket";
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(6, 416);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(96, 37);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "Reset Queue";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Enabled = false;
            this.btn_edit.Location = new System.Drawing.Point(381, 416);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(173, 37);
            this.btn_edit.TabIndex = 5;
            this.btn_edit.Text = "Edit Selected Match";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.Enabled = false;
            this.btn_remove.Location = new System.Drawing.Point(202, 416);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(173, 37);
            this.btn_remove.TabIndex = 6;
            this.btn_remove.Text = "Remove Selected Match";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // btn_push
            // 
            this.btn_push.Enabled = false;
            this.btn_push.Location = new System.Drawing.Point(560, 416);
            this.btn_push.Name = "btn_push";
            this.btn_push.Size = new System.Drawing.Size(173, 37);
            this.btn_push.TabIndex = 4;
            this.btn_push.Text = "Push Selected Match to Stream";
            this.btn_push.UseVisualStyleBackColor = true;
            this.btn_push.Click += new System.EventHandler(this.btn_push_Click);
            // 
            // btn_moveup
            // 
            this.btn_moveup.Enabled = false;
            this.btn_moveup.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_moveup.Location = new System.Drawing.Point(739, 118);
            this.btn_moveup.Name = "btn_moveup";
            this.btn_moveup.Size = new System.Drawing.Size(34, 134);
            this.btn_moveup.TabIndex = 2;
            this.btn_moveup.Text = "↑";
            this.btn_moveup.UseVisualStyleBackColor = true;
            this.btn_moveup.Click += new System.EventHandler(this.btn_moveup_Click);
            // 
            // btn_movedown
            // 
            this.btn_movedown.Enabled = false;
            this.btn_movedown.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_movedown.Location = new System.Drawing.Point(739, 276);
            this.btn_movedown.Name = "btn_movedown";
            this.btn_movedown.Size = new System.Drawing.Size(34, 134);
            this.btn_movedown.TabIndex = 3;
            this.btn_movedown.Text = "↓";
            this.btn_movedown.UseVisualStyleBackColor = true;
            this.btn_movedown.Click += new System.EventHandler(this.btn_movedown_Click);
            // 
            // lvw_matches
            // 
            this.lvw_matches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clm_match,
            this.clm_round,
            this.clm_player1,
            this.clm_player2,
            this.clm_player3,
            this.clm_player4});
            this.lvw_matches.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvw_matches.FullRowSelect = true;
            this.lvw_matches.GridLines = true;
            this.lvw_matches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvw_matches.HideSelection = false;
            this.lvw_matches.Location = new System.Drawing.Point(6, 118);
            this.lvw_matches.Name = "lvw_matches";
            this.lvw_matches.Size = new System.Drawing.Size(727, 292);
            this.lvw_matches.TabIndex = 1;
            this.lvw_matches.UseCompatibleStateImageBehavior = false;
            this.lvw_matches.View = System.Windows.Forms.View.Details;
            this.lvw_matches.SelectedIndexChanged += new System.EventHandler(this.lvw_matches_SelectedIndexChanged);
            // 
            // clm_match
            // 
            this.clm_match.Text = "Match #";
            this.clm_match.Width = 73;
            // 
            // clm_round
            // 
            this.clm_round.Text = "Round in Bracket";
            this.clm_round.Width = 211;
            // 
            // clm_player1
            // 
            this.clm_player1.Text = "Player 1";
            this.clm_player1.Width = 200;
            // 
            // clm_player2
            // 
            this.clm_player2.Text = "Player 2";
            this.clm_player2.Width = 191;
            // 
            // clm_player3
            // 
            this.clm_player3.Text = "Player 3";
            this.clm_player3.Width = 191;
            // 
            // clm_player4
            // 
            this.clm_player4.Text = "Player 4";
            this.clm_player4.Width = 191;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tab_streamqueue);
            this.tabControl1.Controls.Add(this.tab_stationqueue);
            this.tabControl1.Controls.Add(this.tab_autoseed);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(787, 674);
            this.tabControl1.TabIndex = 8;
            // 
            // tab_streamqueue
            // 
            this.tab_streamqueue.Controls.Add(this.btn_player4);
            this.tab_streamqueue.Controls.Add(this.btn_player3);
            this.tab_streamqueue.Controls.Add(this.btn_player2);
            this.tab_streamqueue.Controls.Add(this.btn_player1);
            this.tab_streamqueue.Controls.Add(this.lvw_playerinfo);
            this.tab_streamqueue.Controls.Add(this.groupBox1);
            this.tab_streamqueue.Controls.Add(this.lvw_matches);
            this.tab_streamqueue.Controls.Add(this.btn_reset);
            this.tab_streamqueue.Controls.Add(this.btn_movedown);
            this.tab_streamqueue.Controls.Add(this.btn_edit);
            this.tab_streamqueue.Controls.Add(this.btn_moveup);
            this.tab_streamqueue.Controls.Add(this.btn_remove);
            this.tab_streamqueue.Controls.Add(this.btn_push);
            this.tab_streamqueue.Location = new System.Drawing.Point(4, 22);
            this.tab_streamqueue.Name = "tab_streamqueue";
            this.tab_streamqueue.Padding = new System.Windows.Forms.Padding(3);
            this.tab_streamqueue.Size = new System.Drawing.Size(779, 648);
            this.tab_streamqueue.TabIndex = 0;
            this.tab_streamqueue.Text = "Stream Queue";
            this.tab_streamqueue.UseVisualStyleBackColor = true;
            // 
            // btn_player4
            // 
            this.btn_player4.Location = new System.Drawing.Point(611, 616);
            this.btn_player4.Name = "btn_player4";
            this.btn_player4.Size = new System.Drawing.Size(152, 29);
            this.btn_player4.TabIndex = 14;
            this.btn_player4.Text = "Edit Player 4 Information";
            this.btn_player4.UseVisualStyleBackColor = true;
            this.btn_player4.Click += new System.EventHandler(this.btn_player4_Click);
            // 
            // btn_player3
            // 
            this.btn_player3.Location = new System.Drawing.Point(438, 616);
            this.btn_player3.Name = "btn_player3";
            this.btn_player3.Size = new System.Drawing.Size(152, 29);
            this.btn_player3.TabIndex = 13;
            this.btn_player3.Text = "Edit Player 3 Information";
            this.btn_player3.UseVisualStyleBackColor = true;
            this.btn_player3.Click += new System.EventHandler(this.btn_player3_Click);
            // 
            // btn_player2
            // 
            this.btn_player2.Location = new System.Drawing.Point(266, 616);
            this.btn_player2.Name = "btn_player2";
            this.btn_player2.Size = new System.Drawing.Size(152, 29);
            this.btn_player2.TabIndex = 12;
            this.btn_player2.Text = "Edit Player 2 Information";
            this.btn_player2.UseVisualStyleBackColor = true;
            this.btn_player2.Click += new System.EventHandler(this.btn_player2_Click);
            // 
            // btn_player1
            // 
            this.btn_player1.Location = new System.Drawing.Point(92, 616);
            this.btn_player1.Name = "btn_player1";
            this.btn_player1.Size = new System.Drawing.Size(152, 29);
            this.btn_player1.TabIndex = 11;
            this.btn_player1.Text = "Edit Player 1 Information";
            this.btn_player1.UseVisualStyleBackColor = true;
            this.btn_player1.Click += new System.EventHandler(this.btn_player1_Click);
            // 
            // lvw_playerinfo
            // 
            this.lvw_playerinfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader5});
            this.lvw_playerinfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvw_playerinfo.FullRowSelect = true;
            this.lvw_playerinfo.GridLines = true;
            this.lvw_playerinfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvw_playerinfo.HideSelection = false;
            this.lvw_playerinfo.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lvw_playerinfo.Location = new System.Drawing.Point(6, 459);
            this.lvw_playerinfo.Name = "lvw_playerinfo";
            this.lvw_playerinfo.Size = new System.Drawing.Size(767, 151);
            this.lvw_playerinfo.TabIndex = 10;
            this.lvw_playerinfo.UseCompatibleStateImageBehavior = false;
            this.lvw_playerinfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 80;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Player 1";
            this.columnHeader3.Width = 170;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Player 2";
            this.columnHeader4.Width = 170;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Player 3";
            this.columnHeader1.Width = 170;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Player 4";
            this.columnHeader5.Width = 170;
            // 
            // tab_stationqueue
            // 
            this.tab_stationqueue.Location = new System.Drawing.Point(4, 22);
            this.tab_stationqueue.Name = "tab_stationqueue";
            this.tab_stationqueue.Padding = new System.Windows.Forms.Padding(3);
            this.tab_stationqueue.Size = new System.Drawing.Size(779, 648);
            this.tab_stationqueue.TabIndex = 1;
            this.tab_stationqueue.Text = "Station Queue";
            this.tab_stationqueue.UseVisualStyleBackColor = true;
            // 
            // tab_autoseed
            // 
            this.tab_autoseed.Controls.Add(this.txt_players);
            this.tab_autoseed.Controls.Add(this.lvw_seeding);
            this.tab_autoseed.Controls.Add(this.button4);
            this.tab_autoseed.Controls.Add(this.button3);
            this.tab_autoseed.Controls.Add(this.button2);
            this.tab_autoseed.Controls.Add(this.btn_link_event);
            this.tab_autoseed.Controls.Add(this.cbx_host);
            this.tab_autoseed.Controls.Add(this.label1);
            this.tab_autoseed.Controls.Add(this.txt_tournament_url);
            this.tab_autoseed.Location = new System.Drawing.Point(4, 22);
            this.tab_autoseed.Name = "tab_autoseed";
            this.tab_autoseed.Size = new System.Drawing.Size(779, 648);
            this.tab_autoseed.TabIndex = 2;
            this.tab_autoseed.Text = "AutoSeed";
            this.tab_autoseed.UseVisualStyleBackColor = true;
            // 
            // txt_players
            // 
            this.txt_players.Location = new System.Drawing.Point(636, 16);
            this.txt_players.Margin = new System.Windows.Forms.Padding(2);
            this.txt_players.Multiline = true;
            this.txt_players.Name = "txt_players";
            this.txt_players.Size = new System.Drawing.Size(52, 18);
            this.txt_players.TabIndex = 9;
            // 
            // lvw_seeding
            // 
            this.lvw_seeding.AllowDrop = true;
            this.lvw_seeding.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clm_seed,
            this.clm_elo,
            this.clm_seedtag,
            this.clm_lock});
            this.lvw_seeding.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvw_seeding.FullRowSelect = true;
            this.lvw_seeding.GridLines = true;
            this.lvw_seeding.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvw_seeding.HideSelection = false;
            this.lvw_seeding.Location = new System.Drawing.Point(10, 45);
            this.lvw_seeding.Name = "lvw_seeding";
            this.lvw_seeding.Size = new System.Drawing.Size(765, 452);
            this.lvw_seeding.TabIndex = 8;
            this.lvw_seeding.UseCompatibleStateImageBehavior = false;
            this.lvw_seeding.View = System.Windows.Forms.View.Details;
            this.lvw_seeding.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvw_seeding_ItemDrag);
            this.lvw_seeding.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvw_seeding_DragDrop);
            this.lvw_seeding.DragEnter += new System.Windows.Forms.DragEventHandler(this.lvw_seeding_DragEnter);
            // 
            // clm_seed
            // 
            this.clm_seed.Text = "Seed";
            this.clm_seed.Width = 79;
            // 
            // clm_elo
            // 
            this.clm_elo.Text = "ELO";
            this.clm_elo.Width = 85;
            // 
            // clm_seedtag
            // 
            this.clm_seedtag.Text = "Tag";
            this.clm_seedtag.Width = 359;
            // 
            // clm_lock
            // 
            this.clm_lock.Text = "Seed Lock";
            this.clm_lock.Width = 109;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(527, 503);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(121, 39);
            this.button4.TabIndex = 7;
            this.button4.Text = "Update Seeding";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(654, 503);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(121, 39);
            this.button3.TabIndex = 6;
            this.button3.Text = "Export Seeding";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 503);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 39);
            this.button2.TabIndex = 5;
            this.button2.Text = "Lock Seed";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btn_link_event
            // 
            this.btn_link_event.Location = new System.Drawing.Point(473, 16);
            this.btn_link_event.Name = "btn_link_event";
            this.btn_link_event.Size = new System.Drawing.Size(95, 23);
            this.btn_link_event.TabIndex = 3;
            this.btn_link_event.Text = "Link Event";
            this.btn_link_event.UseVisualStyleBackColor = true;
            this.btn_link_event.Click += new System.EventHandler(this.btn_link_event_Click);
            // 
            // cbx_host
            // 
            this.cbx_host.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_host.Enabled = false;
            this.cbx_host.FormattingEnabled = true;
            this.cbx_host.Items.AddRange(new object[] {
            "Select a Tournament Host",
            "Challonge",
            "Smashgg"});
            this.cbx_host.Location = new System.Drawing.Point(322, 16);
            this.cbx_host.Name = "cbx_host";
            this.cbx_host.Size = new System.Drawing.Size(145, 21);
            this.cbx_host.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tounrament URL";
            // 
            // txt_tournament_url
            // 
            this.txt_tournament_url.Location = new System.Drawing.Point(10, 16);
            this.txt_tournament_url.Name = "txt_tournament_url";
            this.txt_tournament_url.Size = new System.Drawing.Size(306, 20);
            this.txt_tournament_url.TabIndex = 0;
            // 
            // frm_bracket_assistant
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(790, 676);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_bracket_assistant";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Master Orders Bracket Assistant";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_streamqueue_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tab_streamqueue.ResumeLayout(false);
            this.tab_autoseed.ResumeLayout(false);
            this.tab_autoseed.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ComboBox cbx_round;
        private System.Windows.Forms.ComboBox cbx_player1;
        private System.Windows.Forms.ComboBox cbx_player2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label lbl_player2;
        private System.Windows.Forms.Label lbl_player1;
        private System.Windows.Forms.Label lbl_bracket;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_push;
        private System.Windows.Forms.Button btn_moveup;
        private System.Windows.Forms.Button btn_movedown;
        private System.Windows.Forms.ListView lvw_matches;
        private System.Windows.Forms.ColumnHeader clm_match;
        private System.Windows.Forms.ColumnHeader clm_round;
        private System.Windows.Forms.ColumnHeader clm_player1;
        private System.Windows.Forms.ColumnHeader clm_player2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tab_streamqueue;
        private System.Windows.Forms.TabPage tab_stationqueue;
        private System.Windows.Forms.TabPage tab_autoseed;
        private System.Windows.Forms.ListView lvw_playerinfo;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Button btn_player1;
        private System.Windows.Forms.Button btn_player2;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ComboBox cbx_player4;
        private System.Windows.Forms.ComboBox cbx_player3;
        private System.Windows.Forms.ColumnHeader clm_player3;
        private System.Windows.Forms.ColumnHeader clm_player4;
        private System.Windows.Forms.Button btn_player4;
        private System.Windows.Forms.Button btn_player3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button btn_link_event;
        private System.Windows.Forms.ComboBox cbx_host;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_tournament_url;
        private System.Windows.Forms.ListView lvw_seeding;
        private System.Windows.Forms.ColumnHeader clm_seed;
        private System.Windows.Forms.ColumnHeader clm_elo;
        private System.Windows.Forms.ColumnHeader clm_seedtag;
        private System.Windows.Forms.ColumnHeader clm_lock;
        private System.Windows.Forms.TextBox txt_players;
    }
}