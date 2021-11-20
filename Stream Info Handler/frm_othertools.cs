using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using IniParser;
using IniParser.Model;
using System.Drawing.Imaging;
using Stream_Info_Handler.CharacterSelect;
using CharacterLibrary;
using Stream_Info_Handler.AppSettings;
using SqlDatabaseLibrary.Models;

namespace Stream_Info_Handler
{
    public partial class frm_othertools : Form
    {
        public event closedform_event close_form;
        //Init settings file value
        string top8_settings;

        #region Top 8 Initializiation
        //Initialize Top 8 Variables
        bool separate_playerbases;
        string roster_dir;
        List<string> game_directories;
        string character_path = "";
        ComboBox.ObjectCollection character_names;
        ComboBox.ObjectCollection player_names;
        int enter_player = -1;
        int enter_character = -1;
        List<PlayerRecordModel> player_roster = new List<PlayerRecordModel>();
        List<string> player_id;
        int player_roster_size = 0;

        private static string _new_character_image;
        public static string new_character_image
        {
            get // this makes you to access value in form2
            {
                return _new_character_image;
            }
            set // this makes you to change value in form2
            {
                _new_character_image = value;
            }
        }

        public static int player_number = 8;
        public static top8_player[] player_boxes = new top8_player[player_number];
        public PlayerRecordModel[] player_data = new PlayerRecordModel[player_number];

        //Default Entries
        string DefaultCharacterRoster;
        string DefaultTournamentName;
        string DefaultTemplateImage;
        string DefaultBracketURL;
        string DefaultStreamURL;
        string DefaultVodsURL;

        string DefaultHorizontalCutoffPosition;
        string DefaultHorizontalCutoffDirection;
        string DefaultVerticalCutoffPosition;
        string DefaultVerticalCutoffDirection;
        string DefaultCharacterArtScaling;
        string DefaultCharacterArtX;
        string DefaultCharacterArtY;

        //Event Number
        bool IncludeEventNumber;
        int EventNumberX;
        int EventNumberY;
        bool IncludeEventNameBefore;
        string EventNumberFontName;
        string EventNumberJustification;
        int EventNumberTextSize;
        string EventNumberColor;

        //Entrants
        bool IncludeEntrants;
        int EntrantsX;
        int EntrantsY;
        bool IncludeEntrantsAfter;
        string EntrantsFontName;
        string EntrantsJustification;
        int EntrantsTextSize;
        string EntrantsColor;

        //Date
        bool IncludeDate;
        int DateX;
        int DateY;
        string DateFontName;
        string DateJustification;
        int DateTextSize;
        string DateColor;

        //Bracket URL
        bool IncludeBracket;
        int BracketX;
        int BracketY;
        string BracketFontName;
        string BracketJustification;
        int BracketTextSize;
        string BracketColor;

        //Stream URL
        bool IncludeStream;
        int StreamX;
        int StreamY;
        string StreamFontName;
        string StreamJustification;
        int StreamTextSize;
        string StreamColor;

        //VoDs URL
        bool IncludeVods;
        int VodsX;
        int VodsY;
        string VodsFontName;
        string VodsJustification;
        int VodsTextSize;
        string VodsColor;

        //Character Images
        string CharacterImageType;
        int CharacterImageWidth;
        int CharacterImageHeight;
        int CharacterXOffset;
        int CharacterYOffset;

        bool EnableTwitter;

        //First Place Player
        int FirstPlaceTagX;
        int FirstPlaceTagY;
        string FirstPlaceTagFontName;
        string FirstPlaceTagJustification;
        int FirstPlaceTagTextSize;
        string FirstPlaceTagColor;
        int FirstPlaceTagXOffset;
        int FirstPlaceTagYOffset;  
        int FirstPlaceTwitterX;
        int FirstPlaceTwitterY;
        string FirstPlaceTwitterFontName;
        string FirstPlaceTwitterJustification;
        int FirstPlaceTwitterTextSize;
        string FirstPlaceTwitterColor;
        bool FullArtMain;
        int FullArtPreviewX1;
        int FullArtPreviewY1;
        int FullArtPreviewX2;
        int FullArtPreviewY2;
        int FirstPlaceCharacterX;
        int FirstPlaceCharacterY;

        //Remaining Players
        int RemainingTagX;
        int RemainingTagY;
        string RemainingTagFontName;
        string RemainingTagJustification;
        int RemainingTagTextSize;
        string RemainingTagColor;
        int RemainingTagXOffset;
        int RemainingTagYOffset;
        int RemainingTwitterX;
        int RemainingTwitterY;
        string RemainingTwitterFontName;
        string RemainingTwitterJustification;
        int RemainingTwitterTextSize;
        string RemainingTwitterColor;
        int RemainingCharacterX;
        int RemainingCharacterY;
        int PlayerOffsetX;
        int PlayerOffsetY;
        bool XOffsetCharacters;

        #endregion Top 8 Initializiation

        public frm_othertools()
        {
            this.CenterToScreen();

            InitializeComponent();
        }

        private void frm_generator_Shown(object sender, EventArgs e)
        {

            #region Top8 Generator Startup
            //Associate boxes with players
            player_boxes[0] = new top8_player(cbx_tag1, txt_twitter1, btn_character1_1, btn_character1_2, btn_character1_3, btn_character1_4, btn_sponsor1, 0, cms_rightclick, btn_save_1);
            player_boxes[1] = new top8_player(cbx_tag2, txt_twitter2, btn_character2_1, btn_character2_2, btn_character2_3, btn_character2_4, btn_sponsor2, 1, cms_rightclick, btn_save_2);
            player_boxes[2] = new top8_player(cbx_tag3, txt_twitter3, btn_character3_1, btn_character3_2, btn_character3_3, btn_character3_4, btn_sponsor3, 2, cms_rightclick, btn_save_3);
            player_boxes[3] = new top8_player(cbx_tag4, txt_twitter4, btn_character4_1, btn_character4_2, btn_character4_3, btn_character4_4, btn_sponsor4, 3, cms_rightclick, btn_save_4);
            player_boxes[4] = new top8_player(cbx_tag5, txt_twitter5, btn_character5_1, btn_character5_2, btn_character5_3, btn_character5_4, btn_sponsor5, 4, cms_rightclick, btn_save_5);
            player_boxes[5] = new top8_player(cbx_tag6, txt_twitter6, btn_character6_1, btn_character6_2, btn_character6_3, btn_character6_4, btn_sponsor6, 5, cms_rightclick, btn_save_6);
            player_boxes[6] = new top8_player(cbx_tag7, txt_twitter7, btn_character7_1, btn_character7_2, btn_character7_3, btn_character7_4, btn_sponsor7, 6, cms_rightclick, btn_save_7);
            player_boxes[7] = new top8_player(cbx_tag8, txt_twitter8, btn_character8_1, btn_character8_2, btn_character8_3, btn_character8_4, btn_sponsor8, 7, cms_rightclick, btn_save_8);

            cbx_tag1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag2.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag3.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag4.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag5.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag6.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag7.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_tag8.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);


            //Set up the game directory box

            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            //Update the character roster box to contain correct info
            roster_dir = (string)xml.Root.Element("directories").Element("character-directory");
            if (Directory.Exists(roster_dir))
            {
                string[] folders;
                try
                {
                    folders = Directory.GetDirectories(roster_dir);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have access to the Character Databases Directory. Please update this in the settings and try again.");
                    this.Close();
                    return;
                }
                game_directories = new List<string>();
                cbx_character_roster.BeginUpdate();                                      //Begin
                cbx_character_roster.Items.Clear();                                      //Empty the item list

                foreach (string folder in folders)
                {
                    int dirlength = roster_dir.Length + 1;
                    string gamename = folder.Substring(dirlength, folder.Length - dirlength);
                    game_directories.Add(folder);
                    cbx_character_roster.Items.Add(gamename);
                }
                cbx_character_roster.EndUpdate();                                        //End
                cbx_character_roster.SelectedIndex = 0;                                  //Set the combobox index to 0
            }

            //Read the most recent top8 settings from the settings file

            top8_settings = (string)xml.Root.Element("general").Element("top8-settings");
            //Make sure the file exists
            if (!File.Exists(top8_settings))
            {
                if (MessageBox.Show("No Top 8 Settings Detected! Would you like to load a settings file?", "No Settings File Detected", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    DialogResult open_ini = ofd_ini.ShowDialog();
                    if (open_ini == DialogResult.OK)
                    {
                        top8_settings = ofd_ini.FileName;
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    if (MessageBox.Show("Would you like to create a new settings file? Selecting No will close the generator window.", "No Settings File Detected", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        DialogResult open_ini = sfd_ini.ShowDialog();
                        if (open_ini == DialogResult.OK)
                        {
                            top8_settings = sfd_ini.FileName;
                            if(File.Exists(top8_settings))
                            {
                                File.Delete(top8_settings);
                            }
                            File.Copy(Directory.GetCurrentDirectory() + @"\Resources\top8.ini", top8_settings);
                        }
                        else
                        {
                            this.Close();
                        }
                    }
                    else
                    {
                        this.Close();
                    }
                }

                //Update the settings with the location of the new file
                xml.Root.Element("general").Element("top8-settings").ReplaceWith(new XElement("top8-settings", top8_settings));
                xml.Save(SettingsFile.settingsFile);

            }
            read_ini(top8_settings);

            txt_horizontal_cutoff.Text = DefaultHorizontalCutoffPosition;
            txt_vertical_cutoff.Text = DefaultVerticalCutoffPosition;
            txt_fullart_scale.Text = DefaultCharacterArtScaling;
            txt_fullart_x.Text = DefaultCharacterArtX;
            txt_fullart_y.Text = DefaultCharacterArtY;
            txt_date.Text = DateTime.Now.ToString("M/dd/yy");
            #endregion Top8 Generator Startup
        }

        #region Top 8 Generator
        private void read_ini(string top8_ini)
        {
            var parser = new FileIniDataParser();
            parser.Parser.Configuration.CommentString = "#";
            IniData data = parser.ReadFile(top8_ini);
            string holdString;

            //Default Entries
            DefaultCharacterRoster = data["Tournament"]["DefaultCharacterRoster"];
            if (DefaultCharacterRoster == "null") DefaultCharacterRoster = "";
            DefaultTournamentName = data["Tournament"]["DefaultTournamentName"];
            if (DefaultTournamentName == "null") DefaultTournamentName = "";          
            DefaultTemplateImage = data["Tournament"]["DefaultTemplateImage"];
            if (DefaultTemplateImage == "null") DefaultTemplateImage = "";
            DefaultBracketURL = data["Tournament"]["DefaultBracketURL"];
            if (DefaultBracketURL == "null") DefaultBracketURL = "";
            DefaultStreamURL = data["Tournament"]["DefaultStreamURL"];
            if (DefaultStreamURL == "null") DefaultStreamURL = "";
            DefaultVodsURL = data["Tournament"]["DefaultVodsURL"];
            if (DefaultVodsURL == "null") DefaultVodsURL = "";

            cbx_character_roster.SelectedIndex = cbx_character_roster.Items.IndexOf(DefaultCharacterRoster);
            txt_tournament_name.Text = DefaultTournamentName;
            txt_template.Text = DefaultTemplateImage;
            txt_bracket_url.Text = DefaultBracketURL;
            txt_stream_url.Text = DefaultStreamURL;
            txt_vods_url.Text = DefaultVodsURL;

            DefaultHorizontalCutoffPosition = data["Tournament"]["DefaultHorizontalCutoffPosition"];
            DefaultHorizontalCutoffDirection = data["Tournament"]["DefaultHorizontalCutoffDirection"];
            DefaultVerticalCutoffPosition = data["Tournament"]["DefaultVerticalCutoffPosition"];
            DefaultVerticalCutoffDirection = data["Tournament"]["DefaultVerticalCutoffDirection"];
            DefaultCharacterArtScaling = data["Tournament"]["DefaultCharacterArtScaling"];

            if (DefaultHorizontalCutoffDirection == "left")
                cbx_horizontal_cutoff.SelectedIndex = 0;
            else
                cbx_horizontal_cutoff.SelectedIndex = 1;
            if (DefaultVerticalCutoffDirection == "below")
                cbx_vertical_cutoff.SelectedIndex = 0;
            else
                cbx_vertical_cutoff.SelectedIndex = 1;

            DefaultCharacterArtX = data["Tournament"]["DefaultCharacterArtX"];
            DefaultCharacterArtY = data["Tournament"]["DefaultCharacterArtY"];


            txt_horizontal_cutoff.Text = DefaultHorizontalCutoffPosition;
            txt_vertical_cutoff.Text = DefaultVerticalCutoffPosition;
            txt_fullart_scale.Text = DefaultCharacterArtScaling;
            txt_fullart_x.Text = DefaultCharacterArtX;
            txt_fullart_y.Text = DefaultCharacterArtY;

            //Event Number
            holdString = data["Tournament"]["IncludeEventNumber"];
            IncludeEventNumber = bool.Parse(holdString);
            holdString = data["Tournament"]["EventNumberX"];
            EventNumberX = Int32.Parse(holdString);
            holdString = data["Tournament"]["EventNumberY"];
            EventNumberY = Int32.Parse(holdString);
            holdString = data["Tournament"]["IncludeEventNameBefore"];
            IncludeEventNameBefore = bool.Parse(holdString);
            EventNumberFontName = data["Tournament"]["EventNumberFontName"];
            EventNumberJustification = data["Tournament"]["EventNumberJustification"];
            holdString = data["Tournament"]["EventNumberTextSize"];
            EventNumberTextSize = Int32.Parse(holdString);
            EventNumberColor = data["Tournament"]["EventNumberColor"];

            txt_event_number.Enabled = IncludeEventNumber;

            //Entrants
            holdString = data["Tournament"]["IncludeEntrants"];
            IncludeEntrants = bool.Parse(holdString);
            holdString = data["Tournament"]["EntrantsX"];
            EntrantsX = Int32.Parse(holdString);
            holdString = data["Tournament"]["EntrantsY"];
            EntrantsY = Int32.Parse(holdString);
            holdString = data["Tournament"]["IncludeEntrantsAfter"];
            IncludeEntrantsAfter = bool.Parse(holdString);
            EntrantsFontName = data["Tournament"]["EntrantsFontName"];
            EntrantsJustification = data["Tournament"]["EntrantsJustification"];
            holdString = data["Tournament"]["EntrantsTextSize"];
            EntrantsTextSize = Int32.Parse(holdString);
            EntrantsColor = data["Tournament"]["EntrantsColor"];

            txt_entrants.Enabled = IncludeEntrants;

            //Date
            holdString = data["Tournament"]["IncludeDate"];
            IncludeDate = bool.Parse(holdString);
            holdString = data["Tournament"]["DateX"];
            DateX = Int32.Parse(holdString);
            holdString = data["Tournament"]["DateY"];
            DateY = Int32.Parse(holdString);
            DateFontName = data["Tournament"]["DateFontName"];
            DateJustification = data["Tournament"]["DateJustification"];
            holdString = data["Tournament"]["DateTextSize"];
            DateTextSize = Int32.Parse(holdString);
            DateColor = data["Tournament"]["DateColor"];

            txt_date.Enabled = IncludeDate;

            //Bracket URL
            holdString = data["Tournament"]["IncludeBracket"];
            IncludeBracket = bool.Parse(holdString);
            holdString = data["Tournament"]["BracketX"];
            BracketX = Int32.Parse(holdString);
            holdString = data["Tournament"]["BracketY"];
            BracketY = Int32.Parse(holdString);
            BracketFontName = data["Tournament"]["BracketFontName"];
            BracketJustification = data["Tournament"]["BracketJustification"];
            holdString = data["Tournament"]["BracketTextSize"];
            BracketTextSize = Int32.Parse(holdString);
            BracketColor = data["Tournament"]["BracketColor"];

            txt_bracket_url.Enabled = IncludeBracket;

            //Stream URL
            holdString = data["Tournament"]["IncludeStream"];
            IncludeStream = bool.Parse(holdString);
            holdString = data["Tournament"]["StreamX"];
            StreamX = Int32.Parse(holdString);
            holdString = data["Tournament"]["StreamY"];
            StreamY = Int32.Parse(holdString);
            StreamFontName = data["Tournament"]["StreamFontName"];
            StreamJustification = data["Tournament"]["StreamJustification"];
            holdString = data["Tournament"]["StreamTextSize"];
            StreamTextSize = Int32.Parse(holdString);
            StreamColor = data["Tournament"]["StreamColor"];

            txt_stream_url.Enabled = IncludeStream;

            //VoDs URL
            holdString = data["Tournament"]["IncludeVods"];
            IncludeVods = bool.Parse(holdString);
            holdString = data["Tournament"]["VodsX"];
            VodsX = Int32.Parse(holdString);
            holdString = data["Tournament"]["VodsY"];
            VodsY = Int32.Parse(holdString);
            VodsFontName = data["Tournament"]["VodsFontName"];
            VodsJustification = data["Tournament"]["VodsJustification"];
            holdString = data["Tournament"]["VodsTextSize"];
            VodsTextSize = Int32.Parse(holdString);
            VodsColor = data["Tournament"]["VodsColor"];

            txt_vods_url.Enabled = IncludeVods;

            holdString = data["Players"]["EnableTwitter"];
            EnableTwitter = bool.Parse(holdString);

            //Character Images
            CharacterImageType = data["Players"]["CharacterImageType"];
            holdString = data["Players"]["CharacterImageWidth"];
            CharacterImageWidth = Int32.Parse(holdString);
            holdString = data["Players"]["CharacterImageHeight"];
            CharacterImageHeight = Int32.Parse(holdString);
            holdString = data["Players"]["CharacterXOffset"];
            CharacterXOffset = Int32.Parse(holdString);
            holdString = data["Players"]["CharacterYOffset"];
            CharacterYOffset = Int32.Parse(holdString);

            //First Place Player
            holdString = data["Players"]["FirstPlaceTagX"];
            FirstPlaceTagX = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceTagY"];
            FirstPlaceTagY = Int32.Parse(holdString);
            FirstPlaceTagFontName = data["Players"]["FirstPlaceTagFontName"];
            FirstPlaceTagJustification = data["Players"]["FirstPlaceTagJustification"];
            holdString = data["Players"]["FirstPlaceTagTextSize"];
            FirstPlaceTagTextSize = Int32.Parse(holdString);
            FirstPlaceTagColor = data["Players"]["FirstPlaceTagColor"];
            holdString = data["Players"]["FirstPlaceTagXOffset"];
            FirstPlaceTagXOffset = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceTagYOffset"];
            FirstPlaceTagYOffset = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceTwitterX"];
            FirstPlaceTwitterX = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceTwitterY"];
            FirstPlaceTwitterY = Int32.Parse(holdString);
            FirstPlaceTwitterFontName = data["Players"]["FirstPlaceTwitterFontName"];
            FirstPlaceTwitterJustification = data["Players"]["FirstPlaceTwitterJustification"];
            holdString = data["Players"]["FirstPlaceTwitterTextSize"];
            FirstPlaceTwitterTextSize = Int32.Parse(holdString);
            FirstPlaceTwitterColor = data["Players"]["FirstPlaceTwitterColor"];

            holdString = data["Players"]["FullArtMain"];
            FullArtMain = bool.Parse(holdString);

            cbx_horizontal_cutoff.Enabled = FullArtMain;
            cbx_vertical_cutoff.Enabled = FullArtMain;
            txt_horizontal_cutoff.Enabled = FullArtMain;
            txt_vertical_cutoff.Enabled = FullArtMain;
            txt_fullart_scale.Enabled = FullArtMain;
            txt_horizontal_cutoff.Enabled = FullArtMain;
            txt_fullart_x.Enabled = FullArtMain;
            txt_fullart_y.Enabled = FullArtMain;
            

            holdString = data["Players"]["FullArtPreviewX1"];
            FullArtPreviewX1 = Int32.Parse(holdString);
            holdString = data["Players"]["FullArtPreviewY1"];
            FullArtPreviewY1 = Int32.Parse(holdString);
            holdString = data["Players"]["FullArtPreviewX2"];
            FullArtPreviewX2 = Int32.Parse(holdString);
            holdString = data["Players"]["FullArtPreviewY2"];
            FullArtPreviewY2 = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceCharacterX"];
            FirstPlaceCharacterX = Int32.Parse(holdString);
            holdString = data["Players"]["FirstPlaceCharacterY"];
            FirstPlaceCharacterY = Int32.Parse(holdString);

            //Remaining Players
            holdString = data["Players"]["RemainingTagX"];
            RemainingTagX = Int32.Parse(holdString);
            holdString = data["Players"]["RemainingTagY"];
            RemainingTagY = Int32.Parse(holdString);
            RemainingTagFontName = data["Players"]["RemainingTagFontName"];
            RemainingTagJustification = data["Players"]["RemainingTagJustification"];
            holdString = data["Players"]["RemainingTagTextSize"];
            RemainingTagTextSize = Int32.Parse(holdString);
            RemainingTagColor = data["Players"]["RemainingTagColor"];
            holdString = data["Players"]["RemainingTagXOffset"];
            RemainingTagXOffset = Int32.Parse(holdString);
            holdString = data["Players"]["RemainingTagYOffset"];
            RemainingTagYOffset = Int32.Parse(holdString);
            holdString = data["Players"]["RemainingTwitterX"];
            RemainingTwitterX = Int32.Parse(holdString);
            holdString = data["Players"]["RemainingTwitterY"];
            RemainingTwitterY = Int32.Parse(holdString);
            RemainingTwitterFontName = data["Players"]["RemainingTwitterFontName"];
            RemainingTwitterJustification = data["Players"]["RemainingTwitterJustification"];
            holdString = data["Players"]["RemainingTwitterTextSize"];
            RemainingTwitterTextSize = Int32.Parse(holdString);
            RemainingTwitterColor = data["Players"]["RemainingTwitterColor"];

            holdString = data["Players"]["RemainingCharacterX"];
            RemainingCharacterX = Int32.Parse(holdString);
            holdString = data["Players"]["RemainingCharacterY"];
            RemainingCharacterY = Int32.Parse(holdString);
            holdString = data["Players"]["PlayerOffsetX"];
            PlayerOffsetX = Int32.Parse(holdString);
            holdString = data["Players"]["PlayerOffsetY"];
            PlayerOffsetY = Int32.Parse(holdString);
            holdString = data["Players"]["XOffsetCharacters"];
            XOffsetCharacters = bool.Parse(holdString);


        }

        private void btn_template_Click(object sender, EventArgs e)
        {
            DialogResult open_jpg = ofd_jpg.ShowDialog();
            if (open_jpg == DialogResult.OK)
            {
                txt_template.Text = ofd_jpg.FileName;
            }
        }

        private void btn_edit_settings_Click(object sender, EventArgs e)
        {
            Process.Start("notepad.exe", top8_settings);
        }

        private void btn_refresh_settings_Click(object sender, EventArgs e)
        {
            read_ini(top8_settings);
        }

        private void btn_import_settings_Click(object sender, EventArgs e)
        {
            DialogResult open_ini = ofd_ini.ShowDialog();
            if (open_ini == DialogResult.OK)
            {
                top8_settings = ofd_ini.FileName;
                read_ini(top8_settings);
                XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                xml.Root.Element("general").Element("top8-settings").ReplaceWith(new XElement("top8-settings", top8_settings));
                xml.Save(SettingsFile.settingsFile);
            }
        }

        private void numeric_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
            (e.KeyChar != '-'))
            {
                e.Handled = true;
                return;
            }

            // only allow one negative sign
            if ((e.KeyChar == '-') && (((sender as TextBox).Text.IndexOf('-') > -1) || (sender as TextBox).SelectionStart != 0))
            {
                e.Handled = true;
                return;
            }
            update_first_preview();
        }

        private void nonnegative_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
            update_first_preview();
        }

        private void cutoff_SelectedIndexChanged(object sender, EventArgs e)
        {
            update_first_preview();
        }

        private void cbx_character_roster_SelectedIndexChanged(object sender, EventArgs e)
        {
            character_path = game_directories[((ComboBox)sender).SelectedIndex];
            string[] folders = DirectoryManagement.GetCharactersFromDirectory(character_path);
            character_names = cbx_characters.Items;
            character_names.Clear();
            foreach (string folder in folders)
            {
                character_names.Add(folder);
            }
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            string selectedGameName = DirectoryManagement.VerifyGameDirectory(cbx_character_roster.Text, (string)xml.Root.Element("directories").Element("character-directory"));

            player_roster = database_tools.load_players(selectedGameName, ref player_id);

            cbx_tag1.BeginUpdate();
            cbx_tag1.Items.Clear();
            foreach (PlayerRecordModel player in player_roster)
            {
                if(player != null)
                    cbx_tag1.Items.Add(player.unique_tag);
            }
            cbx_tag1.EndUpdate();
            for (int ii = 0; ii < 4; ii++)
            {
                player_boxes[0].character_image[ii] = "";
                player_boxes[0].characterButtons[ii].BackgroundImage = null;
                player_boxes[0].characterButtons[ii].Text = "Add Character";
            }
            player_names = cbx_tag1.Items;

            for (int i = 1; i < player_number; i++)
            {
                player_boxes[i].tag.Items.Clear();
                player_boxes[i].tag.Items.AddRange(player_names.Cast<Object>().ToArray());
                for (int ii = 0; ii < 4; ii++)
                {
                    player_boxes[i].character_image[ii] = "";
                    player_boxes[i].characterButtons[ii].BackgroundImage = null;
                    player_boxes[i].characterButtons[ii].Text = "Add Character";
                }
            }
        }

        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void check_for_sponsor(ref string player_name, ref string player_team)
        {
            //Check if the tag contains the seperator
            if (player_name.Contains(TextFileManagement.sponsorSeperator))
            {
                //Initialize the sponsor and tag checking variables
                string check_team = player_name;
                string check_name = player_name;

                //Check each index of the tag string
                for (int i = 0; i < check_team.Length; i++)
                {
                    //Check if the seperator is present at this index
                    if (check_team.Substring(i).StartsWith(TextFileManagement.sponsorSeperator) == true)
                    {
                        //Set the sponsor to be before this index
                        check_team = player_name.Substring(0, i);
                        //Set the tag to be after the seperator at this index
                        check_name = player_name.Substring(i + 3);

                        //Pass the sponsor and tag onto the actual variables
                        player_team = check_team;
                        player_name = check_name;
                        //Stop checking for the seperator
                        return;
                    }
                }
            }
        }

        private void playerSave(object sender, EventArgs e)
        {
            /*
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < player_boxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (player_boxes[i].save.Name == ((Button)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            PlayerRecordModel saveNewPlayer = new PlayerRecordModel();
            saveNewPlayer.sponsor = "";
            if (player_data[player_index] != null)
            {
                saveNewPlayer = player_data[player_index];
            }
            else
            {
                saveNewPlayer.region = "";
            }
            saveNewPlayer.tag = player_boxes[player_index].tag.Text;
            saveNewPlayer.twitter = player_boxes[player_index].twitter.Text;

            //Check the tag text for a sponsor
            check_for_sponsor(ref saveNewPlayer.tag, ref saveNewPlayer.sponsor);
            //Pull the sponsor from sponsor image filename if there is no text sponsor
            string check_sponsor_image = player_boxes[player_index].sponsor_image;
            if (File.Exists(check_sponsor_image) && (saveNewPlayer.sponsor == "" || saveNewPlayer.sponsor == null))
            {
                saveNewPlayer.sponsor = Path.GetFileNameWithoutExtension(check_sponsor_image);
            }

            bool character_check = false;
            //Loop through the 4 character slots
            for (int i = 0; i < 4; i++)
            {
                //Reset the character
                saveNewPlayer.characterName = "";
                saveNewPlayer.colorNumber = 0;
                //Grab the slot's character image and check if it exists
                string check_image = player_boxes[player_index].character_image[i];
                if (File.Exists(check_image))
                {
                    //Pull the character and color from the filepath
                    int dirlength = character_path.Length + 1;
                    string subdir = check_image.Substring(dirlength, check_image.Length - dirlength);
                    int cutoff = subdir.IndexOf(@"\");
                    saveNewPlayer.characterName = subdir.Substring(0, cutoff);
                    int color_cutoff = (subdir.Substring(cutoff + 1, subdir.Length - cutoff - 1)).IndexOf(@"\");
                    saveNewPlayer.colorNumber = Int32.Parse(subdir.Substring(cutoff + 1, color_cutoff));
                    //A character has been found
                    character_check = true;
                    return;
                }
            }
            //If no character is found, abort the save
            if (character_check == false)
            {
                MessageBox.Show("A player needs a character to be saved. Please add a character and try saving again.");
                return;
            }

            //Store global values and change the values to match the currently selected game
            string[] hold_characters = global_values.characters;
            string holdSelectedGame = GlobalSettings.selectedGame;
            global_values.characters = database_tools.get_characters(character_path);
            GlobalSettings.selectedGame = cbx_character_roster.Text;

            var PlayerRecordModel_box = new SavePlayer.SavePlayerForm(saveNewPlayer, saveNewPlayer.character[0], saveNewPlayer.color[0]);
            if (PlayerRecordModel_box.ShowDialog() == DialogResult.OK)
            {

                database_tools.add_player(PlayerRecordModel_box.outputPlayer, PlayerRecordModel_box.outputIsNewPlayer);
                player_roster = database_tools.load_players(GlobalSettings.selectedGame,  ref player_id);

                player_names = cbx_tag1.Items;
                player_names.Clear();
                foreach (PlayerRecordModel player in player_roster)
                {
                    if (player != null)
                        player_names.Add(player.unique_tag);
                }
                for (int i = 1; i < player_number; i++)
                {
                    player_boxes[i].tag.Items.Clear();
                    player_boxes[i].tag.Items.AddRange(player_names.Cast<Object>().ToArray());
                }


                player_boxes[player_index].tag.SelectedIndex = player_id.IndexOf(PlayerRecordModel_box.outputPlayer.id);     //Set the combobox index to 0
            }
            //Restore the global values
            global_values.characters = hold_characters;
            GlobalSettings.selectedGame = holdSelectedGame;
            */
        }

        private void playerText(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < player_boxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (player_boxes[i].tag.Name == ((ComboBox)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            if(player_data[player_index] != null)
            {
                if(player_boxes[player_index].tag.Text != player_data[player_index].tag &&
                    player_boxes[player_index].tag.Text != player_data[player_index].unique_tag)
                {
                    player_data[player_index] = new PlayerRecordModel();
                }
            }
        }

        private void playerSelect(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < player_boxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (player_boxes[i].tag.Name == ((ComboBox)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            if (player_boxes[player_index].tag.SelectedIndex >= 0)
            {
                int i = player_boxes[player_index].tag.SelectedIndex;
                if (player_roster[i].unique_tag == player_boxes[player_index].tag.Text)
                {
                    //Store the player information
                    player_data[player_index] = player_roster[i];

                    if (Directory.Exists(DirectoryManagement.sponsorDirectory))
                    {
                        string sponsor_location = DirectoryManagement.sponsorDirectory + @"\" + player_roster[i].sponsor + ".png";
                        if (File.Exists(sponsor_location))
                        {
                            player_boxes[player_index].sponsor_image = sponsor_location;
                            player_boxes[player_index].sponsor.BackgroundImage = Image.FromFile(sponsor_location);
                            player_boxes[player_index].sponsor.Text = "";
                        }
                        else
                        {
                            this.BeginInvoke((MethodInvoker)delegate { player_boxes[player_index].tag.Text = player_roster[i].unique_tag; });
                            player_boxes[player_index].sponsor_image = "";
                            player_boxes[player_index].sponsor.BackgroundImage = null;
                            player_boxes[player_index].sponsor.Text = "Add Sponsor Logo";
                        }
                    }
                    else
                    {
                        //Update the text of this ComboBox to the display text of this player's tag
                        this.BeginInvoke((MethodInvoker)delegate { player_boxes[player_index].tag.Text = player_roster[i].unique_tag; });
                        player_boxes[player_index].sponsor_image = "";
                        player_boxes[player_index].sponsor.BackgroundImage = null;
                        player_boxes[player_index].sponsor.Text = "Add Sponsor Logo";
                    }

                    //Update the Twitter ComboBox with this player data's twitter
                    player_boxes[player_index].twitter.Text = player_roster[i].twitter;

                    //Input the player's characters
                    for (int ii = 0; ii < 4; ii++)
                    {
                        if (player_roster[i].character[ii] == "" || player_roster[i].character[ii] == null)
                        {
                            player_boxes[player_index].character_image[ii] = "";
                            player_boxes[player_index].characterButtons[ii].BackgroundImage = null;
                            player_boxes[player_index].characterButtons[ii].Text = "Add Character";
                        }
                        else
                        {
                            string character_image = character_path + @"\" + player_roster[i].character[ii] + @"\" + player_roster[i].color[ii] + @"\" + CharacterImageType + ".png";
                            if (File.Exists(character_image))
                            {
                                player_boxes[player_index].character_image[ii] = character_image;
                                player_boxes[player_index].characterButtons[ii].BackgroundImage = Image.FromFile(character_image);
                                player_boxes[player_index].characterButtons[ii].Text = "";
                                CharacterData newCharacter = new CharacterData(player_roster[i].character[ii], player_roster[i].color[ii]);
                                player_boxes[player_index].playerCharacters[ii] = newCharacter;

                            }
                            else
                            {
                                player_boxes[player_index].character_image[ii] = "";
                                player_boxes[player_index].characterButtons[ii].BackgroundImage = null;
                                player_boxes[player_index].characterButtons[ii].Text = "Add Character";
                            }
                        }
                    }
                    if (player_index == 0)
                        update_first_preview();
                }
            }
        }

        private void sponsorSelect(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < player_boxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (player_boxes[i].sponsor.Name == ((Button)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            ofd_png.InitialDirectory = DirectoryManagement.sponsorDirectory;
            if(ofd_png.ShowDialog() == DialogResult.OK)
            {
                player_boxes[player_index].sponsor_image = ofd_png.FileName;
                player_boxes[player_index].sponsor.BackgroundImage = Image.FromFile(ofd_png.FileName);
                player_boxes[player_index].sponsor.Text = "";
            }
        }

        private void characterSelect(object sender, EventArgs e)
        {
            int top8_number = -1;
            int character_number = -1;
            //Find the player information
            for(int i = 0; i < player_number; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    if (player_boxes[i].characterButtons[ii] == sender)
                    {
                        top8_number = i;
                        character_number = ii;
                        break;
                    }
                }
                if (character_number != -1)
                    break;
            }
            if (top8_number == -1)
            {
                MessageBox.Show("Error selecting character. Selected character box not found?");
                return;
            }
            //Show the character selection dialog

            player_boxes[top8_number].playerCharacters[character_number] = selectCharacter(player_boxes[top8_number].playerCharacters[character_number], character_path);
            string getCharacter = GetCharacterStamp(player_boxes[top8_number].playerCharacters[character_number], character_path);

            if (getCharacter != null)
            {
                player_boxes[top8_number].character_image[character_number] = getCharacter;
                player_boxes[top8_number].characterButtons[character_number].BackgroundImage = Image.FromFile(getCharacter);
                player_boxes[top8_number].characterButtons[character_number].Text = "";
                if (get_character_image(0, 0) == player_boxes[top8_number].character_image[character_number])
                    update_first_preview();
            }
            /*
            frm_top8_character select = new frm_top8_character(character_names, player_boxes[top8_number].character_image[character_number], character_path, CharacterImageType);
            if(select.ShowDialog() == DialogResult.OK)
            {
                player_boxes[top8_number].character_image[character_number] = new_character_image;
                player_boxes[top8_number].character[character_number].BackgroundImage = Image.FromFile(new_character_image);
                player_boxes[top8_number].character[character_number].Text = "";
                if(get_character_image(0, 0) == player_boxes[top8_number].character_image[character_number])
                    update_first_preview();

            }
            */
        }
        public CharacterData selectCharacter(CharacterData character, string gameDirectory)
        {
            character = GenerateCharacters.GetCharacter(character, gameDirectory);
            return character;
        }

        public string GetCharacterStamp(CharacterData character, string gameDirectory)
        {
            if (character != null)
            {
                string newCharacter = gameDirectory + @"\" +
                                        character.characterName + @"\" + character.characterColor.ToString() + @"\stamp.png";

                return newCharacter;
            }
            else
                return null;
        }

        private void characterEnter(object sender, EventArgs e)
        {
            int top8_number = -1;
            int character_number = -1;
            //Find the player information
            for (int i = 0; i < player_number; i++)
            {
                for (int ii = 0; ii < 4; ii++)
                {
                    if (player_boxes[i].characterButtons[ii] == sender)
                    {
                        top8_number = i;
                        character_number = ii;
                        break;
                    }
                }
                if (character_number != -1)
                    break;
            }
            if (top8_number == -1)
            {
                MessageBox.Show("Error selecting character. Selected character box not found?");
                return;
            }
            enter_player = top8_number;
            enter_character = character_number;
        }

        private void sponsorEnter(object sender, EventArgs e)
        {
            int top8_number = -1;
            //Find the player information
            for (int i = 0; i < player_number; i++)
            {
                if (player_boxes[i].sponsor == sender)
                {
                    top8_number = i;
                    break;
                }
            }

            if (top8_number == -1)
            {
                MessageBox.Show("Error selecting character. Selected character box not found?");
                return;
            }
            enter_player = top8_number;
        }

        private void cms_rightclick_Opening(object sender, CancelEventArgs e)
        {
            int top8_number = enter_player;
            int character_number = enter_character;
            player_data[top8_number].character[character_number] = "";


            player_boxes[top8_number].character_image[character_number] = "";
            player_boxes[top8_number].characterButtons[character_number].BackgroundImage = null;
            player_boxes[top8_number].characterButtons[character_number].Text = "Add Character";
            e.Cancel = true;
        }

        private void frm_generator_FormClosed(object sender, FormClosedEventArgs e)
        {
            global_values.tools_form = null;
            close_form(2);
        }

        private void cms_sponsor_Opening(object sender, CancelEventArgs e)
        {
            int top8_number = enter_player;
            player_data[top8_number].sponsor = "";
            player_boxes[top8_number].sponsor_image = "";
            player_boxes[top8_number].sponsor.BackgroundImage = null;
            player_boxes[top8_number].sponsor.Text = "Add Sponsor Image";
            e.Cancel = true;
        }

        public static Bitmap ResizeImage(Image image, int scale)
        {
            float fullscale = scale;
            if (scale == 0)
                fullscale = 1.0f;
            int width = Convert.ToInt32(image.Width * (fullscale / 100));
            int height = Convert.ToInt32(image.Height * (fullscale / 100));

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }


        private void update_first_preview()
        {
            string first_char = get_character_image(0, 0);
            if (txt_fullart_x.Text != "" && txt_fullart_y.Text != "" && txt_fullart_x.Text != "-" && txt_fullart_y.Text != "-" &&
                txt_fullart_scale.Text != "" && txt_horizontal_cutoff.Text != "" && txt_vertical_cutoff.Text != "" &&
                File.Exists(txt_template.Text) && File.Exists(first_char) && Path.GetExtension(txt_template.Text) == ".jpg")
            {
                int preview_width = FullArtPreviewX2 - FullArtPreviewX1;
                int preview_height = FullArtPreviewY2 - FullArtPreviewY1;

                Image firstplace_image = new Bitmap(preview_width, preview_height);
                Graphics drawing = Graphics.FromImage(firstplace_image);
                drawing.InterpolationMode = InterpolationMode.High;
                drawing.SmoothingMode = SmoothingMode.HighQuality;
                drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                drawing.CompositingQuality = CompositingQuality.HighQuality;

                Image background = Image.FromFile(txt_template.Text);

                Image character_render = Image.FromFile(first_char.Substring(0, first_char.Length - CharacterImageType.Length - 4) + "1080.png");
                character_render.RotateFlip(RotateFlipType.RotateNoneFlipX);
                character_render = ResizeImage(character_render, Int32.Parse(txt_fullart_scale.Text));

                drawing.Clear(Color.White);

                int sourceX1 = 0;
                int sourceY1 = 0;
                int sourceX2 = character_render.Width;
                int sourceY2 = character_render.Height;
                int charx = 0;
                int chary = 0;

                if (cbx_horizontal_cutoff.SelectedIndex == 0)
                {
                    int cutoff_difference = Int32.Parse(txt_fullart_x.Text) + FullArtPreviewX1 - Int32.Parse(txt_horizontal_cutoff.Text);
                    if (cutoff_difference < 0)
                    {
                        sourceX1 -= cutoff_difference;
                        //sourceX2 += cutoff_difference;
                        charx = -cutoff_difference;
                    }
                }
                else
                {
                    int cutoff_difference = FullArtPreviewX1 + Int32.Parse(txt_fullart_x.Text) + sourceX2 - Int32.Parse(txt_horizontal_cutoff.Text);
                    if (cutoff_difference > 0)
                    {
                        sourceX2 -= cutoff_difference;
                    }
                }

                if (cbx_vertical_cutoff.SelectedIndex == 1)
                {
                    int cutoff_difference = Int32.Parse(txt_fullart_y.Text) + FullArtPreviewY1 - Int32.Parse(txt_vertical_cutoff.Text);
                    if (cutoff_difference < 0)
                    {
                        sourceY1 -= cutoff_difference;
                        //sourceY2 += cutoff_difference;
                        chary = -cutoff_difference;
                    }
                }
                else
                {
                    int cutoff_difference = FullArtPreviewY1 + Int32.Parse(txt_fullart_y.Text) + sourceY2 - Int32.Parse(txt_vertical_cutoff.Text);
                    if (cutoff_difference > 0)
                    {
                        sourceY2 -= cutoff_difference;
                    }
                }

                drawing.DrawImage(background, new Rectangle(0, 0, preview_width, preview_height), new Rectangle(FullArtPreviewX1, FullArtPreviewY1, preview_width, preview_height), GraphicsUnit.Pixel);
                drawing.DrawImage(character_render, charx + Int32.Parse(txt_fullart_x.Text), chary + Int32.Parse(txt_fullart_y.Text), new Rectangle(sourceX1, sourceY1, sourceX2, sourceY2), GraphicsUnit.Pixel);
                pic_fullart.BackgroundImage = firstplace_image;
            }
        }

        private void fullart_TextChanged(object sender, EventArgs e)
        {
            update_first_preview();
        }

        public class top8_player
        {
            public ComboBox tag;
            public TextBox twitter;
            public List<Button> characterButtons = new List<Button>();
            public Button sponsor;
            public string[] character_image = { "", "", "", "" };
            public string sponsor_image = "";
            public int index;
            public Button save;
            public List<CharacterData> playerCharacters = new List<CharacterData>();

            public top8_player(ComboBox totag, TextBox totwitter, Button Character1, Button Character2, Button Character3,
                Button Character4, Button tosponsor, int toindex, ContextMenuStrip cms, Button tosave )
            {
                tag = totag;
                twitter = totwitter;
                characterButtons.Add(Character1);
                characterButtons.Add(Character2);
                characterButtons.Add(Character3);
                characterButtons.Add(Character4);
                foreach (Button newcharacter in characterButtons)
                {
                    newcharacter.ContextMenuStrip = cms;
                }
                sponsor = tosponsor;
                index = toindex;
                save = tosave;
                for(int i = 0; i < 4; i++)
                {
                    CharacterData newCharacter = new CharacterData();
                    playerCharacters.Add(newCharacter);
                }
            }
        }

        private string get_character_image(int player_number, int character_number)
        {
            int character_count = -1;

            for(int i = 0; i < 4; i++)
                if(player_boxes[player_number].character_image[i] != "")
                {
                    character_count++;
                    if(character_count == character_number)
                        return player_boxes[player_number].character_image[i];
                }

            return null;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            //destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private StringAlignment get_align(string alignment)
        {
            switch(alignment)
            {
                case "left":
                    return StringAlignment.Near;

                case "right":
                    return StringAlignment.Far;

                case "center":
                    return StringAlignment.Center;
            }
            return StringAlignment.Near;
        }

        private void tab_top8_gen_SelectedIndexChanged(object sender, EventArgs e)
        {
            pic_preview.Image = generate_top8();
        }

        private Image generate_top8()
        {
            //Only perform this action if the top8 tab is selected.
            if (tab_top8_gen.SelectedTab != tab_preview_top8)
                return null;

            //Initialize Background and get image sizes
            Image background = Image.FromFile(txt_template.Text);
            int top8_width = background.Width;
            int top8_height = background.Height;

            //Initialize top8 graphics and drawing
            Image top8_bmp = new Bitmap(top8_width, top8_height);
            Graphics drawing = Graphics.FromImage(top8_bmp);
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            //Clear the drawing area
            drawing.Clear(Color.White);
            //Draw the background
            drawing.DrawImage(background, 0, 0, top8_width, top8_height);

            //First Place
            ////////////////////////////////////
            //Draw the large first place graphic
            if (FullArtMain == true)
            {
                if (!File.Exists(get_character_image(0, 0)))
                    return null;
                string first_char = get_character_image(0, 0);
                int preview_width = FullArtPreviewX2 - FullArtPreviewX1;
                int preview_height = FullArtPreviewY2 - FullArtPreviewY1;

                Image firstplace_image = new Bitmap(preview_width, preview_height);

                Image character_render = Image.FromFile(first_char.Substring(0, first_char.Length - CharacterImageType.Length - 4) + "1080.png");
                character_render.RotateFlip(RotateFlipType.RotateNoneFlipX);
                character_render = ResizeImage(character_render, Int32.Parse(txt_fullart_scale.Text));

                int sourceX1 = 0;
                int sourceY1 = 0;
                int sourceX2 = character_render.Width;
                int sourceY2 = character_render.Height;
                int charx = 0;
                int chary = 0;

                if (cbx_horizontal_cutoff.SelectedIndex == 0)
                {
                    int cutoff_difference = Int32.Parse(txt_fullart_x.Text) + FullArtPreviewX1 - Int32.Parse(txt_horizontal_cutoff.Text);
                    if (cutoff_difference < 0)
                    {
                        sourceX1 -= cutoff_difference;
                        charx = -cutoff_difference;
                    }
                }
                else
                {
                    int cutoff_difference = FullArtPreviewX1 + Int32.Parse(txt_fullart_x.Text) + sourceX2 - Int32.Parse(txt_horizontal_cutoff.Text);
                    if (cutoff_difference > 0)
                    {
                        sourceX2 -= cutoff_difference;
                    }
                }

                if (cbx_vertical_cutoff.SelectedIndex == 1)
                {
                    int cutoff_difference = Int32.Parse(txt_fullart_y.Text) + FullArtPreviewY1 - Int32.Parse(txt_vertical_cutoff.Text);
                    if (cutoff_difference < 0)
                    {
                        sourceY1 -= cutoff_difference;
                        chary = -cutoff_difference;
                    }
                }
                else
                {
                    int cutoff_difference = FullArtPreviewY1 + Int32.Parse(txt_fullart_y.Text) + sourceY2 - Int32.Parse(txt_vertical_cutoff.Text);
                    if (cutoff_difference > 0)
                    {
                        sourceY2 -= cutoff_difference;
                    }
                }

                drawing.DrawImage(character_render, FullArtPreviewX1 + charx + Int32.Parse(txt_fullart_x.Text), FullArtPreviewY1 + chary + Int32.Parse(txt_fullart_y.Text), new Rectangle(sourceX1, sourceY1, sourceX2, sourceY2), GraphicsUnit.Pixel);
            }


            //First Place Name and Sponsor
            {
                int image_xscale = 0;
                //Draw 1st Logo
                if (player_boxes[0].sponsor_image != "")
                {
                    Image sponsor_logo = Image.FromFile(player_boxes[0].sponsor_image);
                    image_xscale = decimal.ToInt32(decimal.Round((sponsor_logo.Width * FirstPlaceTagTextSize) / sponsor_logo.Height));
                    sponsor_logo = ResizeImage(sponsor_logo, image_xscale, FirstPlaceTagTextSize);
                    drawing.DrawImage(sponsor_logo, FirstPlaceTagX, FirstPlaceTagY);
                }

                string player_name = cbx_tag1.Text.ToUpper();

                Font first_place_font = new Font(FirstPlaceTagFontName, FirstPlaceTagTextSize, GraphicsUnit.Pixel);
                SolidBrush black = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + FirstPlaceTagColor));

                StringFormat drawFormat = new StringFormat();
                drawFormat.LineAlignment = StringAlignment.Near;
                drawFormat.Alignment = get_align(FirstPlaceTagJustification);

                drawing.DrawString(player_name, first_place_font, black, FirstPlaceTagX + image_xscale + FirstPlaceTagXOffset, FirstPlaceTagY + FirstPlaceTagYOffset, drawFormat);
            }

            //First Place Characters
            {
                for (int i = 1; i < 4; i++)
                {
                    string character_image = get_character_image(0, i);
                    if (character_image != null)
                    {
                        Image stock_icon = Image.FromFile(character_image);

                        int large_dimension = 0;
                        int image_x = 0;
                        int image_y = 0;
                        if (CharacterImageWidth > CharacterImageHeight)
                        {
                            large_dimension = CharacterImageWidth;
                            image_y = (CharacterImageWidth - CharacterImageHeight) / 2;
                        }
                        else
                        {
                            large_dimension = CharacterImageHeight;
                            image_x = (CharacterImageHeight - CharacterImageWidth) / 2;
                        }

                        stock_icon = ResizeImage(stock_icon, large_dimension, large_dimension);

                        drawing.DrawImage(stock_icon, FirstPlaceCharacterX + (CharacterXOffset * (i - 1)),
                                          FirstPlaceCharacterY + (CharacterYOffset * (i - 1)),
                                          new Rectangle(image_x, image_y, CharacterImageWidth, CharacterImageHeight),
                                          GraphicsUnit.Pixel);
                    }
                }
            }

            //First Place Twitter
            if (EnableTwitter)
            {

                string player_twitter = txt_twitter1.Text.ToUpper();

                Font first_place_font = new Font(FirstPlaceTwitterFontName, FirstPlaceTwitterTextSize, GraphicsUnit.Pixel);
                SolidBrush black = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + FirstPlaceTwitterColor));

                StringFormat drawFormat = new StringFormat();
                drawFormat.LineAlignment = StringAlignment.Near;
                drawFormat.Alignment = get_align(FirstPlaceTagJustification);

                drawing.DrawString(player_twitter, first_place_font, black, FirstPlaceTwitterX, FirstPlaceTwitterY, drawFormat);
            }

            //Remaining Players
            ///////////////////////////////////////////////////////////////
            for (int current_player = 1; current_player < player_number; current_player++)
            {
                //Remaining Name and Sponsor
                {
                    int image_xscale = 0;
                    //Draw 1st Logo
                    if (player_boxes[current_player].sponsor_image != "" && player_boxes[current_player].sponsor_image != null)
                    {
                        Image sponsor_logo = Image.FromFile(player_boxes[current_player].sponsor_image);
                        image_xscale = decimal.ToInt32(decimal.Round((sponsor_logo.Width * RemainingTagTextSize) / sponsor_logo.Height));
                        sponsor_logo = ResizeImage(sponsor_logo, image_xscale, RemainingTagTextSize);
                        drawing.DrawImage(sponsor_logo, RemainingTagX + (PlayerOffsetX * current_player), RemainingTagY + (PlayerOffsetY * current_player));
                    }

                    string player_name = player_boxes[current_player].tag.Text.ToUpper();

                    Font first_place_font = new Font(RemainingTagFontName, RemainingTagTextSize, GraphicsUnit.Pixel);
                    SolidBrush black = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + RemainingTagColor));

                    StringFormat drawFormat = new StringFormat();
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(RemainingTagJustification);

                    drawing.DrawString(player_name, first_place_font, black,
                        RemainingTagX + (PlayerOffsetX * current_player) + image_xscale + RemainingTagXOffset,
                        RemainingTagY + (PlayerOffsetY * current_player) + RemainingTagYOffset, drawFormat);
                }

                //Remaining Characters
                {
                    for (int i = 0; i < 4; i++)
                    {
                        string character_image = get_character_image(current_player, i);
                        if (character_image != null)
                        {
                            Image stock_icon = Image.FromFile(character_image);

                            int large_dimension = 0;
                            int image_x = 0;
                            int image_y = 0;
                            if (CharacterImageWidth > CharacterImageHeight)
                            {
                                large_dimension = CharacterImageWidth;
                                image_y = (CharacterImageWidth - CharacterImageHeight) / 2;
                            }
                            else
                            {
                                large_dimension = CharacterImageHeight;
                                image_x = (CharacterImageHeight - CharacterImageWidth) / 2;
                            }

                            stock_icon = ResizeImage(stock_icon, large_dimension, large_dimension);

                            drawing.DrawImage(stock_icon, RemainingCharacterX + (PlayerOffsetX * current_player) + (CharacterXOffset * (i)),
                                              RemainingCharacterY + (PlayerOffsetY * current_player) + (CharacterYOffset * (i)),
                                              new Rectangle(image_x, image_y, CharacterImageWidth, CharacterImageHeight),
                                              GraphicsUnit.Pixel);
                        }
                    }
                }

                //Remaining Twitters
                if (EnableTwitter)
                {
                    string player_twitter = player_boxes[current_player].twitter.Text.ToUpper();

                    using (StringFormat drawFormat = new StringFormat())
                    {
                        drawFormat.LineAlignment = StringAlignment.Near;
                        drawFormat.Alignment = get_align(RemainingTwitterJustification);

                        using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + RemainingTwitterColor)))
                        {
                            using (Font new_font = new Font(RemainingTwitterFontName, RemainingTwitterTextSize, GraphicsUnit.Pixel))
                            {
                                drawing.DrawString(player_twitter, new_font, brush, RemainingTwitterX + PlayerOffsetX * current_player,
                                    RemainingTwitterY + PlayerOffsetY * current_player, drawFormat);
                            }
                        }
                    }
                }
            }

            //Event number
            if (IncludeEventNumber)
            {
                string event_number = "";
                if (IncludeEventNameBefore)
                    event_number = txt_tournament_name.Text + " " + txt_event_number.Text;
                else
                    event_number = txt_event_number.Text;

                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(EventNumberJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + EventNumberColor)))
                    {
                        using (Font new_font = new Font(EventNumberFontName, EventNumberTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(event_number, new_font, brush, EventNumberX, EventNumberY, drawFormat);
                        }
                    }
                }
            }

            //Entrants number
            if (IncludeEntrants)
            {
                string entrants_number = "";
                if (IncludeEntrantsAfter)
                    entrants_number = txt_entrants.Text + " ENTRANTS";
                else
                    entrants_number = txt_entrants.Text;

                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(EntrantsJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + EntrantsColor)))
                    {
                        using (Font new_font = new Font(EntrantsFontName, EntrantsTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(entrants_number, new_font, brush, EntrantsX, EntrantsY, drawFormat);
                        }
                    }
                }
            }

            //Date
            if (IncludeDate)
            {
                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(DateJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + DateColor)))
                    {
                        using (Font new_font = new Font(DateFontName, DateTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(txt_date.Text, new_font, brush, DateX, DateY, drawFormat);
                        }
                    }
                }
            }

            //Bracket URL
            if (IncludeBracket)
            {
                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(BracketJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + BracketColor)))
                    {
                        using (Font new_font = new Font(BracketFontName, BracketTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(txt_bracket_url.Text, new_font, brush, BracketX, BracketY, drawFormat);
                        }
                    }
                }
            }

            //Stream URL
            if (IncludeBracket)
            {
                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(StreamJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + StreamColor)))
                    {
                        using (Font new_font = new Font(StreamFontName, StreamTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(txt_stream_url.Text, new_font, brush, StreamX, StreamY, drawFormat);
                        }
                    }
                }
            }

            //VoDs URL
            if (IncludeVods)
            {
                using (StringFormat drawFormat = new StringFormat())
                {
                    drawFormat.LineAlignment = StringAlignment.Near;
                    drawFormat.Alignment = get_align(VodsJustification);

                    using (SolidBrush brush = new SolidBrush(System.Drawing.ColorTranslator.FromHtml("#" + VodsColor)))
                    {
                        using (Font new_font = new Font(VodsFontName, VodsTextSize, GraphicsUnit.Pixel))
                        {
                            drawing.DrawString(txt_vods_url.Text, new_font, brush, VodsX, VodsY, drawFormat);
                        }
                    }
                }
            }

            return top8_bmp;
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset all Top 8 fields to their defaults?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }

            read_ini(top8_settings);

            txt_horizontal_cutoff.Text = DefaultHorizontalCutoffPosition;
            txt_vertical_cutoff.Text = DefaultVerticalCutoffPosition;
            txt_fullart_scale.Text = DefaultCharacterArtScaling;
            txt_fullart_x.Text = DefaultCharacterArtX;
            txt_fullart_y.Text = DefaultCharacterArtY;
            txt_date.Text = DateTime.Now.ToString("M/dd/yy");

            txt_entrants.Text = "";
            txt_event_number.Text = "";

            for(int player = 0; player < player_number; player++)
            {
                player_boxes[player].tag.Text = "";
                player_boxes[player].twitter.Text = "";
                player_boxes[player].sponsor_image = "";
                player_boxes[player].sponsor.BackgroundImage = null;
                player_boxes[player].sponsor.Text = "Add Sponsor Image";
                for(int i = 0; i < 4; i++)
                {
                    player_boxes[player].character_image[i] = "";
                    player_boxes[player].characterButtons[i].BackgroundImage = null;
                    player_boxes[player].characterButtons[i].Text = "Add Character";
                }
            }
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Image top8 = generate_top8();
            saveFileDialog1.FileName = txt_tournament_name.Text + " " + txt_event_number.Text;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                top8.Save(saveFileDialog1.FileName, ImageFormat.Png);
            }
        }
        #endregion Top8 Generator
    }

}
