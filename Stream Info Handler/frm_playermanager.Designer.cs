namespace Stream_Info_Handler
{
    partial class frm_playermanager
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
            "1600",
            "Serris",
            "UGS",
            "@serrisfroon",
            "Dan S",
            "IL - Illinois",
            "Captain Falcon",
            "TO"}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_playermanager));
            this.label20 = new System.Windows.Forms.Label();
            this.cbx_character_roster = new System.Windows.Forms.ComboBox();
            this.btn_removeplayer = new System.Windows.Forms.Button();
            this.btn_editplayer = new System.Windows.Forms.Button();
            this.btn_addplayer = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.lvw_players = new System.Windows.Forms.ListView();
            this.clh_elo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_tag = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_sponsor = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_twitter = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_name = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_region = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_main = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clh_misc = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ckb_wireless = new System.Windows.Forms.CheckBox();
            this.lbl_playerid = new System.Windows.Forms.Label();
            this.lbl_ownerid = new System.Windows.Forms.Label();
            this.lbl_misc = new System.Windows.Forms.Label();
            this.txt_misc = new System.Windows.Forms.TextBox();
            this.txt_elo = new System.Windows.Forms.TextBox();
            this.lbl_elo = new System.Windows.Forms.Label();
            this.lbl_sponsor = new System.Windows.Forms.Label();
            this.txt_fullsponsor = new System.Windows.Forms.TextBox();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.lbl_fullname = new System.Windows.Forms.Label();
            this.cbx_region = new System.Windows.Forms.ComboBox();
            this.lbl_main = new System.Windows.Forms.Label();
            this.lbl_sponsorpre = new System.Windows.Forms.Label();
            this.lbl_region = new System.Windows.Forms.Label();
            this.lbl_twitter = new System.Windows.Forms.Label();
            this.lbl_tag = new System.Windows.Forms.Label();
            this.txt_sponsor = new System.Windows.Forms.TextBox();
            this.txt_twitter = new System.Windows.Forms.TextBox();
            this.txt_tag = new System.Windows.Forms.TextBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.btn_character = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(140, 328);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(87, 13);
            this.label20.TabIndex = 129;
            this.label20.Text = "Character Roster";
            // 
            // cbx_character_roster
            // 
            this.cbx_character_roster.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_character_roster.FormattingEnabled = true;
            this.cbx_character_roster.Location = new System.Drawing.Point(144, 344);
            this.cbx_character_roster.Name = "cbx_character_roster";
            this.cbx_character_roster.Size = new System.Drawing.Size(189, 21);
            this.cbx_character_roster.TabIndex = 128;
            this.cbx_character_roster.SelectedIndexChanged += new System.EventHandler(this.cbx_character_roster_SelectedIndexChanged);
            // 
            // btn_removeplayer
            // 
            this.btn_removeplayer.Enabled = false;
            this.btn_removeplayer.Location = new System.Drawing.Point(609, 329);
            this.btn_removeplayer.Name = "btn_removeplayer";
            this.btn_removeplayer.Size = new System.Drawing.Size(105, 37);
            this.btn_removeplayer.TabIndex = 127;
            this.btn_removeplayer.Text = "Remove Player";
            this.btn_removeplayer.UseVisualStyleBackColor = true;
            this.btn_removeplayer.Click += new System.EventHandler(this.Btn_removeplayer_Click);
            // 
            // btn_editplayer
            // 
            this.btn_editplayer.Enabled = false;
            this.btn_editplayer.Location = new System.Drawing.Point(499, 329);
            this.btn_editplayer.Name = "btn_editplayer";
            this.btn_editplayer.Size = new System.Drawing.Size(105, 37);
            this.btn_editplayer.TabIndex = 126;
            this.btn_editplayer.Text = "Edit Player";
            this.btn_editplayer.UseVisualStyleBackColor = true;
            this.btn_editplayer.Click += new System.EventHandler(this.Btn_editplayer_Click);
            // 
            // btn_addplayer
            // 
            this.btn_addplayer.Location = new System.Drawing.Point(389, 329);
            this.btn_addplayer.Name = "btn_addplayer";
            this.btn_addplayer.Size = new System.Drawing.Size(105, 37);
            this.btn_addplayer.TabIndex = 125;
            this.btn_addplayer.Text = "Add New Player";
            this.btn_addplayer.UseVisualStyleBackColor = true;
            this.btn_addplayer.Click += new System.EventHandler(this.btn_addplayer_Click);
            // 
            // btn_refresh
            // 
            this.btn_refresh.Location = new System.Drawing.Point(8, 328);
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.Size = new System.Drawing.Size(104, 37);
            this.btn_refresh.TabIndex = 124;
            this.btn_refresh.Text = "Refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // lvw_players
            // 
            this.lvw_players.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clh_elo,
            this.clh_tag,
            this.clh_sponsor,
            this.clh_twitter,
            this.clh_name,
            this.clh_region,
            this.clh_main,
            this.clh_misc});
            this.lvw_players.FullRowSelect = true;
            this.lvw_players.GridLines = true;
            this.lvw_players.HideSelection = false;
            this.lvw_players.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.lvw_players.Location = new System.Drawing.Point(8, 8);
            this.lvw_players.MultiSelect = false;
            this.lvw_players.Name = "lvw_players";
            this.lvw_players.Size = new System.Drawing.Size(706, 315);
            this.lvw_players.TabIndex = 123;
            this.lvw_players.UseCompatibleStateImageBehavior = false;
            this.lvw_players.View = System.Windows.Forms.View.Details;
            this.lvw_players.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lvw_players_ColumnClick);
            this.lvw_players.SelectedIndexChanged += new System.EventHandler(this.lvw_players_SelectedIndexChanged);
            // 
            // clh_elo
            // 
            this.clh_elo.Text = "ELO";
            this.clh_elo.Width = 40;
            // 
            // clh_tag
            // 
            this.clh_tag.Text = "Tag";
            this.clh_tag.Width = 125;
            // 
            // clh_sponsor
            // 
            this.clh_sponsor.Text = "Sponsor";
            this.clh_sponsor.Width = 52;
            // 
            // clh_twitter
            // 
            this.clh_twitter.Text = "Twitter";
            this.clh_twitter.Width = 105;
            // 
            // clh_name
            // 
            this.clh_name.Text = "Name";
            this.clh_name.Width = 75;
            // 
            // clh_region
            // 
            this.clh_region.Text = "Region";
            this.clh_region.Width = 85;
            // 
            // clh_main
            // 
            this.clh_main.Text = "Main";
            this.clh_main.Width = 90;
            // 
            // clh_misc
            // 
            this.clh_misc.Text = "Misc";
            this.clh_misc.Width = 100;
            // 
            // ckb_wireless
            // 
            this.ckb_wireless.AutoSize = true;
            this.ckb_wireless.Enabled = false;
            this.ckb_wireless.Location = new System.Drawing.Point(20, 503);
            this.ckb_wireless.Name = "ckb_wireless";
            this.ckb_wireless.Size = new System.Drawing.Size(138, 17);
            this.ckb_wireless.TabIndex = 152;
            this.ckb_wireless.Text = "Wireless Controller User";
            this.ckb_wireless.UseVisualStyleBackColor = true;
            // 
            // lbl_playerid
            // 
            this.lbl_playerid.AutoSize = true;
            this.lbl_playerid.Location = new System.Drawing.Point(16, 378);
            this.lbl_playerid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_playerid.Name = "lbl_playerid";
            this.lbl_playerid.Size = new System.Drawing.Size(98, 13);
            this.lbl_playerid.TabIndex = 151;
            this.lbl_playerid.Text = "Player ID: 1000001";
            // 
            // lbl_ownerid
            // 
            this.lbl_ownerid.AutoSize = true;
            this.lbl_ownerid.Location = new System.Drawing.Point(176, 378);
            this.lbl_ownerid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ownerid.Name = "lbl_ownerid";
            this.lbl_ownerid.Size = new System.Drawing.Size(127, 13);
            this.lbl_ownerid.TabIndex = 150;
            this.lbl_ownerid.Text = "Owned by: UGSGAMING";
            // 
            // lbl_misc
            // 
            this.lbl_misc.AutoSize = true;
            this.lbl_misc.Location = new System.Drawing.Point(114, 480);
            this.lbl_misc.Name = "lbl_misc";
            this.lbl_misc.Size = new System.Drawing.Size(32, 13);
            this.lbl_misc.TabIndex = 149;
            this.lbl_misc.Text = "Misc.";
            // 
            // txt_misc
            // 
            this.txt_misc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_misc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_misc.Enabled = false;
            this.txt_misc.Location = new System.Drawing.Point(152, 477);
            this.txt_misc.Name = "txt_misc";
            this.txt_misc.Size = new System.Drawing.Size(414, 20);
            this.txt_misc.TabIndex = 147;
            // 
            // txt_elo
            // 
            this.txt_elo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_elo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_elo.Enabled = false;
            this.txt_elo.Location = new System.Drawing.Point(50, 477);
            this.txt_elo.Name = "txt_elo";
            this.txt_elo.Size = new System.Drawing.Size(59, 20);
            this.txt_elo.TabIndex = 146;
            // 
            // lbl_elo
            // 
            this.lbl_elo.AutoSize = true;
            this.lbl_elo.Location = new System.Drawing.Point(16, 480);
            this.lbl_elo.Name = "lbl_elo";
            this.lbl_elo.Size = new System.Drawing.Size(28, 13);
            this.lbl_elo.TabIndex = 148;
            this.lbl_elo.Text = "ELO";
            // 
            // lbl_sponsor
            // 
            this.lbl_sponsor.AutoSize = true;
            this.lbl_sponsor.Location = new System.Drawing.Point(274, 453);
            this.lbl_sponsor.Name = "lbl_sponsor";
            this.lbl_sponsor.Size = new System.Drawing.Size(96, 13);
            this.lbl_sponsor.TabIndex = 145;
            this.lbl_sponsor.Text = "Full Sponsor Name";
            // 
            // txt_fullsponsor
            // 
            this.txt_fullsponsor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_fullsponsor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_fullsponsor.Enabled = false;
            this.txt_fullsponsor.Location = new System.Drawing.Point(376, 450);
            this.txt_fullsponsor.Name = "txt_fullsponsor";
            this.txt_fullsponsor.Size = new System.Drawing.Size(190, 20);
            this.txt_fullsponsor.TabIndex = 136;
            // 
            // txt_name
            // 
            this.txt_name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_name.Enabled = false;
            this.txt_name.Location = new System.Drawing.Point(76, 451);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(193, 20);
            this.txt_name.TabIndex = 134;
            // 
            // lbl_fullname
            // 
            this.lbl_fullname.AutoSize = true;
            this.lbl_fullname.Location = new System.Drawing.Point(16, 454);
            this.lbl_fullname.Name = "lbl_fullname";
            this.lbl_fullname.Size = new System.Drawing.Size(54, 13);
            this.lbl_fullname.TabIndex = 144;
            this.lbl_fullname.Text = "Full Name";
            // 
            // cbx_region
            // 
            this.cbx_region.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_region.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_region.Enabled = false;
            this.cbx_region.FormattingEnabled = true;
            this.cbx_region.Items.AddRange(new object[] {
            "AL - Alabama",
            "AK - Alaska",
            "AZ - Arizona",
            "AR -Arkansas",
            "Ca - California",
            "CO - Colorado",
            "CT - Connecticut",
            "DE - Deleware",
            "DC - Washington, DC",
            "FL - florida",
            "GA - Georgia",
            "HI - Hawaii",
            "ID - Idaho",
            "IL - Illinois",
            "IN - Indiana",
            "IA - Iowa",
            "KS - Kansas",
            "KY - Kentucky",
            "LA - Louisiana",
            "ME - Maine",
            "MD - Maryland",
            "MA - Massachusetts",
            "MI - Michigan",
            "MN - Minnesota",
            "MS - Mississippi",
            "MO - Missouri",
            "MT - Montana",
            "NE - Nebraska",
            "NV - Nevada",
            "NH - New Hampshire",
            "NJ - New Jersey",
            "NM - New Mexico",
            "NY - New York",
            "NC - North Carolina",
            "ND - North Dakota",
            "OH - Ohio",
            "OK - Oklahoma",
            "OR - Oregon",
            "PA - Pennsylvania",
            "RI - Rhode Island",
            "SC - South Carolina",
            "SD - South Dakota",
            "TN - Tennessee",
            "TX - Texas",
            "UT - Utah",
            "VT - Vermont",
            "VA - Virginia",
            "WA - Washington",
            "WV - West Virginia",
            "WI - Wisconsin",
            "WY - Wyoming",
            "MX - Mexico",
            "CAN - Canada"});
            this.cbx_region.Location = new System.Drawing.Point(340, 424);
            this.cbx_region.Name = "cbx_region";
            this.cbx_region.Size = new System.Drawing.Size(130, 21);
            this.cbx_region.TabIndex = 132;
            // 
            // lbl_main
            // 
            this.lbl_main.AutoSize = true;
            this.lbl_main.Location = new System.Drawing.Point(594, 408);
            this.lbl_main.Name = "lbl_main";
            this.lbl_main.Size = new System.Drawing.Size(53, 13);
            this.lbl_main.TabIndex = 143;
            this.lbl_main.Text = "Character";
            // 
            // lbl_sponsorpre
            // 
            this.lbl_sponsorpre.AutoSize = true;
            this.lbl_sponsorpre.Location = new System.Drawing.Point(474, 407);
            this.lbl_sponsorpre.Name = "lbl_sponsorpre";
            this.lbl_sponsorpre.Size = new System.Drawing.Size(75, 13);
            this.lbl_sponsorpre.TabIndex = 141;
            this.lbl_sponsorpre.Text = "Sponsor Prefix";
            // 
            // lbl_region
            // 
            this.lbl_region.AutoSize = true;
            this.lbl_region.Location = new System.Drawing.Point(338, 408);
            this.lbl_region.Name = "lbl_region";
            this.lbl_region.Size = new System.Drawing.Size(41, 13);
            this.lbl_region.TabIndex = 139;
            this.lbl_region.Text = "Region";
            // 
            // lbl_twitter
            // 
            this.lbl_twitter.AutoSize = true;
            this.lbl_twitter.Location = new System.Drawing.Point(176, 408);
            this.lbl_twitter.Name = "lbl_twitter";
            this.lbl_twitter.Size = new System.Drawing.Size(39, 13);
            this.lbl_twitter.TabIndex = 137;
            this.lbl_twitter.Text = "Twitter";
            // 
            // lbl_tag
            // 
            this.lbl_tag.AutoSize = true;
            this.lbl_tag.Location = new System.Drawing.Point(16, 408);
            this.lbl_tag.Name = "lbl_tag";
            this.lbl_tag.Size = new System.Drawing.Size(26, 13);
            this.lbl_tag.TabIndex = 135;
            this.lbl_tag.Text = "Tag";
            // 
            // txt_sponsor
            // 
            this.txt_sponsor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_sponsor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_sponsor.Enabled = false;
            this.txt_sponsor.Location = new System.Drawing.Point(476, 424);
            this.txt_sponsor.Name = "txt_sponsor";
            this.txt_sponsor.Size = new System.Drawing.Size(90, 20);
            this.txt_sponsor.TabIndex = 133;
            // 
            // txt_twitter
            // 
            this.txt_twitter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_twitter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_twitter.Enabled = false;
            this.txt_twitter.Location = new System.Drawing.Point(180, 425);
            this.txt_twitter.Name = "txt_twitter";
            this.txt_twitter.Size = new System.Drawing.Size(155, 20);
            this.txt_twitter.TabIndex = 131;
            // 
            // txt_tag
            // 
            this.txt_tag.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_tag.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_tag.Enabled = false;
            this.txt_tag.Location = new System.Drawing.Point(18, 425);
            this.txt_tag.Name = "txt_tag";
            this.txt_tag.Size = new System.Drawing.Size(155, 20);
            this.txt_tag.TabIndex = 130;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Enabled = false;
            this.btn_cancel.Location = new System.Drawing.Point(365, 526);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(105, 37);
            this.btn_cancel.TabIndex = 153;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.Btn_cancel_Click);
            // 
            // btn_update
            // 
            this.btn_update.Enabled = false;
            this.btn_update.Location = new System.Drawing.Point(254, 526);
            this.btn_update.Name = "btn_update";
            this.btn_update.Size = new System.Drawing.Size(105, 37);
            this.btn_update.TabIndex = 154;
            this.btn_update.Text = "Update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // btn_character
            // 
            this.btn_character.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_character.Enabled = false;
            this.btn_character.Location = new System.Drawing.Point(572, 424);
            this.btn_character.Name = "btn_character";
            this.btn_character.Size = new System.Drawing.Size(96, 96);
            this.btn_character.TabIndex = 155;
            this.btn_character.Text = "Click to Select a Character";
            this.btn_character.UseVisualStyleBackColor = true;
            this.btn_character.Click += new System.EventHandler(this.btn_character_Click);
            // 
            // frm_playermanager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(724, 568);
            this.Controls.Add(this.btn_character);
            this.Controls.Add(this.btn_update);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.ckb_wireless);
            this.Controls.Add(this.lbl_playerid);
            this.Controls.Add(this.lbl_ownerid);
            this.Controls.Add(this.lbl_misc);
            this.Controls.Add(this.txt_misc);
            this.Controls.Add(this.txt_elo);
            this.Controls.Add(this.lbl_elo);
            this.Controls.Add(this.lbl_sponsor);
            this.Controls.Add(this.txt_fullsponsor);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.lbl_fullname);
            this.Controls.Add(this.cbx_region);
            this.Controls.Add(this.lbl_main);
            this.Controls.Add(this.lbl_sponsorpre);
            this.Controls.Add(this.lbl_region);
            this.Controls.Add(this.lbl_twitter);
            this.Controls.Add(this.lbl_tag);
            this.Controls.Add(this.txt_sponsor);
            this.Controls.Add(this.txt_twitter);
            this.Controls.Add(this.txt_tag);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.cbx_character_roster);
            this.Controls.Add(this.btn_removeplayer);
            this.Controls.Add(this.btn_editplayer);
            this.Controls.Add(this.btn_addplayer);
            this.Controls.Add(this.btn_refresh);
            this.Controls.Add(this.lvw_players);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_playermanager";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Player Manager";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_playermanager_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.ComboBox cbx_character_roster;
        private System.Windows.Forms.Button btn_removeplayer;
        private System.Windows.Forms.Button btn_editplayer;
        private System.Windows.Forms.Button btn_addplayer;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.ListView lvw_players;
        private System.Windows.Forms.ColumnHeader clh_elo;
        private System.Windows.Forms.ColumnHeader clh_sponsor;
        private System.Windows.Forms.ColumnHeader clh_tag;
        private System.Windows.Forms.ColumnHeader clh_twitter;
        private System.Windows.Forms.ColumnHeader clh_main;
        private System.Windows.Forms.ColumnHeader clh_name;
        private System.Windows.Forms.ColumnHeader clh_region;
        private System.Windows.Forms.ColumnHeader clh_misc;
        private System.Windows.Forms.CheckBox ckb_wireless;
        private System.Windows.Forms.Label lbl_playerid;
        private System.Windows.Forms.Label lbl_ownerid;
        private System.Windows.Forms.Label lbl_misc;
        private System.Windows.Forms.TextBox txt_misc;
        private System.Windows.Forms.TextBox txt_elo;
        private System.Windows.Forms.Label lbl_elo;
        private System.Windows.Forms.Label lbl_sponsor;
        private System.Windows.Forms.TextBox txt_fullsponsor;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label lbl_fullname;
        private System.Windows.Forms.ComboBox cbx_region;
        private System.Windows.Forms.Label lbl_main;
        private System.Windows.Forms.Label lbl_sponsorpre;
        private System.Windows.Forms.Label lbl_region;
        private System.Windows.Forms.Label lbl_twitter;
        private System.Windows.Forms.Label lbl_tag;
        private System.Windows.Forms.TextBox txt_sponsor;
        private System.Windows.Forms.TextBox txt_twitter;
        private System.Windows.Forms.TextBox txt_tag;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.Button btn_character;
    }
}