namespace Stream_Info_Handler.SavePlayer
{
    partial class SavePlayerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SavePlayerForm));
            this.txt_tag = new System.Windows.Forms.TextBox();
            this.txt_twitter = new System.Windows.Forms.TextBox();
            this.txt_sponsor = new System.Windows.Forms.TextBox();
            this.lbl_tag = new System.Windows.Forms.Label();
            this.lbl_twitter = new System.Windows.Forms.Label();
            this.lbl_region = new System.Windows.Forms.Label();
            this.lbl_sponsorpre = new System.Windows.Forms.Label();
            this.lbl_main = new System.Windows.Forms.Label();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.cbx_region = new System.Windows.Forms.ComboBox();
            this.lbl_fullname = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.lbl_sponsor = new System.Windows.Forms.Label();
            this.lbl_misc = new System.Windows.Forms.Label();
            this.txt_misc = new System.Windows.Forms.TextBox();
            this.txt_elo = new System.Windows.Forms.TextBox();
            this.lbl_elo = new System.Windows.Forms.Label();
            this.lbl_ownerid = new System.Windows.Forms.Label();
            this.lbl_playerid = new System.Windows.Forms.Label();
            this.btn_create = new System.Windows.Forms.Button();
            this.ckb_wireless = new System.Windows.Forms.CheckBox();
            this.cbx_fullsponsor = new System.Windows.Forms.ComboBox();
            this.btn_character = new System.Windows.Forms.Button();
            this.ckb_character = new System.Windows.Forms.CheckBox();
            this.cbx_pronouns = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_tag
            // 
            this.txt_tag.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_tag.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_tag.Location = new System.Drawing.Point(12, 52);
            this.txt_tag.Name = "txt_tag";
            this.txt_tag.Size = new System.Drawing.Size(155, 20);
            this.txt_tag.TabIndex = 0;
            this.txt_tag.TextChanged += new System.EventHandler(this.txt_tag_TextChanged);
            // 
            // txt_twitter
            // 
            this.txt_twitter.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_twitter.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_twitter.Location = new System.Drawing.Point(174, 52);
            this.txt_twitter.Name = "txt_twitter";
            this.txt_twitter.Size = new System.Drawing.Size(155, 20);
            this.txt_twitter.TabIndex = 1;
            // 
            // txt_sponsor
            // 
            this.txt_sponsor.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_sponsor.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_sponsor.Location = new System.Drawing.Point(470, 51);
            this.txt_sponsor.Name = "txt_sponsor";
            this.txt_sponsor.Size = new System.Drawing.Size(90, 20);
            this.txt_sponsor.TabIndex = 3;
            // 
            // lbl_tag
            // 
            this.lbl_tag.AutoSize = true;
            this.lbl_tag.Location = new System.Drawing.Point(10, 36);
            this.lbl_tag.Name = "lbl_tag";
            this.lbl_tag.Size = new System.Drawing.Size(26, 13);
            this.lbl_tag.TabIndex = 4;
            this.lbl_tag.Text = "Tag";
            // 
            // lbl_twitter
            // 
            this.lbl_twitter.AutoSize = true;
            this.lbl_twitter.Location = new System.Drawing.Point(170, 36);
            this.lbl_twitter.Name = "lbl_twitter";
            this.lbl_twitter.Size = new System.Drawing.Size(39, 13);
            this.lbl_twitter.TabIndex = 5;
            this.lbl_twitter.Text = "Twitter";
            // 
            // lbl_region
            // 
            this.lbl_region.AutoSize = true;
            this.lbl_region.Location = new System.Drawing.Point(332, 36);
            this.lbl_region.Name = "lbl_region";
            this.lbl_region.Size = new System.Drawing.Size(41, 13);
            this.lbl_region.TabIndex = 6;
            this.lbl_region.Text = "Region";
            // 
            // lbl_sponsorpre
            // 
            this.lbl_sponsorpre.AutoSize = true;
            this.lbl_sponsorpre.Location = new System.Drawing.Point(468, 35);
            this.lbl_sponsorpre.Name = "lbl_sponsorpre";
            this.lbl_sponsorpre.Size = new System.Drawing.Size(75, 13);
            this.lbl_sponsorpre.TabIndex = 7;
            this.lbl_sponsorpre.Text = "Sponsor Prefix";
            // 
            // lbl_main
            // 
            this.lbl_main.AutoSize = true;
            this.lbl_main.Location = new System.Drawing.Point(595, 20);
            this.lbl_main.Name = "lbl_main";
            this.lbl_main.Size = new System.Drawing.Size(53, 13);
            this.lbl_main.TabIndex = 18;
            this.lbl_main.Text = "Character";
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(250, 151);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(105, 29);
            this.btn_save.TabIndex = 9;
            this.btn_save.Text = "Update Information";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(360, 151);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(105, 29);
            this.btn_cancel.TabIndex = 10;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // cbx_region
            // 
            this.cbx_region.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_region.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
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
            this.cbx_region.Location = new System.Drawing.Point(334, 51);
            this.cbx_region.Name = "cbx_region";
            this.cbx_region.Size = new System.Drawing.Size(130, 21);
            this.cbx_region.TabIndex = 2;
            // 
            // lbl_fullname
            // 
            this.lbl_fullname.AutoSize = true;
            this.lbl_fullname.Location = new System.Drawing.Point(10, 81);
            this.lbl_fullname.Name = "lbl_fullname";
            this.lbl_fullname.Size = new System.Drawing.Size(54, 13);
            this.lbl_fullname.TabIndex = 19;
            this.lbl_fullname.Text = "Full Name";
            // 
            // txt_name
            // 
            this.txt_name.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_name.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_name.Location = new System.Drawing.Point(70, 78);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(193, 20);
            this.txt_name.TabIndex = 4;
            // 
            // lbl_sponsor
            // 
            this.lbl_sponsor.AutoSize = true;
            this.lbl_sponsor.Location = new System.Drawing.Point(268, 80);
            this.lbl_sponsor.Name = "lbl_sponsor";
            this.lbl_sponsor.Size = new System.Drawing.Size(96, 13);
            this.lbl_sponsor.TabIndex = 22;
            this.lbl_sponsor.Text = "Full Sponsor Name";
            // 
            // lbl_misc
            // 
            this.lbl_misc.AutoSize = true;
            this.lbl_misc.Location = new System.Drawing.Point(296, 106);
            this.lbl_misc.Name = "lbl_misc";
            this.lbl_misc.Size = new System.Drawing.Size(32, 13);
            this.lbl_misc.TabIndex = 26;
            this.lbl_misc.Text = "Misc.";
            // 
            // txt_misc
            // 
            this.txt_misc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_misc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_misc.Location = new System.Drawing.Point(334, 104);
            this.txt_misc.Name = "txt_misc";
            this.txt_misc.Size = new System.Drawing.Size(226, 20);
            this.txt_misc.TabIndex = 24;
            // 
            // txt_elo
            // 
            this.txt_elo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txt_elo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txt_elo.Location = new System.Drawing.Point(231, 104);
            this.txt_elo.Name = "txt_elo";
            this.txt_elo.Size = new System.Drawing.Size(59, 20);
            this.txt_elo.TabIndex = 23;
            // 
            // lbl_elo
            // 
            this.lbl_elo.AutoSize = true;
            this.lbl_elo.Location = new System.Drawing.Point(197, 107);
            this.lbl_elo.Name = "lbl_elo";
            this.lbl_elo.Size = new System.Drawing.Size(28, 13);
            this.lbl_elo.TabIndex = 25;
            this.lbl_elo.Text = "ELO";
            // 
            // lbl_ownerid
            // 
            this.lbl_ownerid.AutoSize = true;
            this.lbl_ownerid.Location = new System.Drawing.Point(170, 5);
            this.lbl_ownerid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_ownerid.Name = "lbl_ownerid";
            this.lbl_ownerid.Size = new System.Drawing.Size(127, 13);
            this.lbl_ownerid.TabIndex = 27;
            this.lbl_ownerid.Text = "Owned by: UGSGAMING";
            // 
            // lbl_playerid
            // 
            this.lbl_playerid.AutoSize = true;
            this.lbl_playerid.Location = new System.Drawing.Point(10, 5);
            this.lbl_playerid.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbl_playerid.Name = "lbl_playerid";
            this.lbl_playerid.Size = new System.Drawing.Size(98, 13);
            this.lbl_playerid.TabIndex = 28;
            this.lbl_playerid.Text = "Player ID: 1000001";
            // 
            // btn_create
            // 
            this.btn_create.Enabled = false;
            this.btn_create.Location = new System.Drawing.Point(139, 151);
            this.btn_create.Name = "btn_create";
            this.btn_create.Size = new System.Drawing.Size(105, 29);
            this.btn_create.TabIndex = 29;
            this.btn_create.Text = "Create New Player";
            this.btn_create.UseVisualStyleBackColor = true;
            this.btn_create.Visible = false;
            this.btn_create.Click += new System.EventHandler(this.btn_create_Click);
            // 
            // ckb_wireless
            // 
            this.ckb_wireless.AutoSize = true;
            this.ckb_wireless.Location = new System.Drawing.Point(13, 131);
            this.ckb_wireless.Name = "ckb_wireless";
            this.ckb_wireless.Size = new System.Drawing.Size(138, 17);
            this.ckb_wireless.TabIndex = 30;
            this.ckb_wireless.Text = "Wireless Controller User";
            this.ckb_wireless.UseVisualStyleBackColor = true;
            // 
            // cbx_fullsponsor
            // 
            this.cbx_fullsponsor.FormattingEnabled = true;
            this.cbx_fullsponsor.Location = new System.Drawing.Point(370, 78);
            this.cbx_fullsponsor.Margin = new System.Windows.Forms.Padding(2);
            this.cbx_fullsponsor.Name = "cbx_fullsponsor";
            this.cbx_fullsponsor.Size = new System.Drawing.Size(190, 21);
            this.cbx_fullsponsor.TabIndex = 31;
            this.cbx_fullsponsor.SelectedIndexChanged += new System.EventHandler(this.Cbx_fullsponsor_SelectedIndexChanged);
            // 
            // btn_character
            // 
            this.btn_character.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_character.Location = new System.Drawing.Point(566, 36);
            this.btn_character.Name = "btn_character";
            this.btn_character.Size = new System.Drawing.Size(112, 112);
            this.btn_character.TabIndex = 32;
            this.btn_character.UseVisualStyleBackColor = true;
            this.btn_character.Click += new System.EventHandler(this.btn_character_Click);
            // 
            // ckb_character
            // 
            this.ckb_character.AutoSize = true;
            this.ckb_character.Checked = true;
            this.ckb_character.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ckb_character.Enabled = false;
            this.ckb_character.Location = new System.Drawing.Point(301, 130);
            this.ckb_character.Name = "ckb_character";
            this.ckb_character.Size = new System.Drawing.Size(110, 17);
            this.ckb_character.TabIndex = 33;
            this.ckb_character.Text = "Update Character";
            this.ckb_character.UseVisualStyleBackColor = true;
            // 
            // cbx_pronouns
            // 
            this.cbx_pronouns.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_pronouns.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_pronouns.FormattingEnabled = true;
            this.cbx_pronouns.Items.AddRange(new object[] {
            "He/Him",
            "She/Her",
            "They/Them",
            "He/They",
            "She/They"});
            this.cbx_pronouns.Location = new System.Drawing.Point(70, 103);
            this.cbx_pronouns.Name = "cbx_pronouns";
            this.cbx_pronouns.Size = new System.Drawing.Size(121, 21);
            this.cbx_pronouns.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "Pronouns";
            // 
            // SavePlayerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 191);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbx_pronouns);
            this.Controls.Add(this.ckb_character);
            this.Controls.Add(this.btn_character);
            this.Controls.Add(this.cbx_fullsponsor);
            this.Controls.Add(this.ckb_wireless);
            this.Controls.Add(this.btn_create);
            this.Controls.Add(this.lbl_playerid);
            this.Controls.Add(this.lbl_ownerid);
            this.Controls.Add(this.lbl_misc);
            this.Controls.Add(this.txt_misc);
            this.Controls.Add(this.txt_elo);
            this.Controls.Add(this.lbl_elo);
            this.Controls.Add(this.lbl_sponsor);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.lbl_fullname);
            this.Controls.Add(this.cbx_region);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.lbl_main);
            this.Controls.Add(this.lbl_sponsorpre);
            this.Controls.Add(this.lbl_region);
            this.Controls.Add(this.lbl_twitter);
            this.Controls.Add(this.lbl_tag);
            this.Controls.Add(this.txt_sponsor);
            this.Controls.Add(this.txt_twitter);
            this.Controls.Add(this.txt_tag);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SavePlayerForm";
            this.Text = "Save Player Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_tag;
        private System.Windows.Forms.TextBox txt_twitter;
        private System.Windows.Forms.Label lbl_tag;
        private System.Windows.Forms.Label lbl_twitter;
        private System.Windows.Forms.Label lbl_region;
        private System.Windows.Forms.Label lbl_sponsorpre;
        private System.Windows.Forms.Label lbl_main;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.TextBox txt_sponsor;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.ComboBox cbx_region;
        private System.Windows.Forms.Label lbl_fullname;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label lbl_sponsor;
        private System.Windows.Forms.Label lbl_misc;
        private System.Windows.Forms.TextBox txt_misc;
        private System.Windows.Forms.TextBox txt_elo;
        private System.Windows.Forms.Label lbl_elo;
        private System.Windows.Forms.Label lbl_ownerid;
        private System.Windows.Forms.Label lbl_playerid;
        private System.Windows.Forms.Button btn_create;
        private System.Windows.Forms.CheckBox ckb_wireless;
        private System.Windows.Forms.ComboBox cbx_fullsponsor;
        private System.Windows.Forms.Button btn_character;
        private System.Windows.Forms.CheckBox ckb_character;
        private System.Windows.Forms.ComboBox cbx_pronouns;
        private System.Windows.Forms.Label label1;
    }
}