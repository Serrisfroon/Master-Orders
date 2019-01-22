//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2019, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using System.Data.SqlClient;
using MySql.Data.MySqlClient;
using MySql.Data;

namespace Stream_Info_Handler
{
    public partial class frm_main : Form
    {
        //Initialize the variables to contain the character image file directories
        public string image_directory1 = Directory.GetCurrentDirectory();
        public string image_directory2 = Directory.GetCurrentDirectory();
        public string image_directory3 = Directory.GetCurrentDirectory();
        public string image_directory4 = Directory.GetCurrentDirectory();
        public static string save_name;

        public static bool update_check = true;


        public static player_fields[] player_boxes = new player_fields[5];


        public static Color warning_color = Color.FromArgb(234, 153, 153);

        bool ignore_settings;


        private static int _get_character_slot;
        public static int get_character_slot
        {
            get // this makes you to access value in form2
            {
                return _get_character_slot;
            }
            set // this makes you to change value in form2
            {
                _get_character_slot = value;
            }
        }

        private static player_info _get_new_player;
        public static player_info get_new_player
        {
            get // this makes you to access value in form2
            {
                return _get_new_player;
            }
            set // this makes you to change value in form2
            {
                _get_new_player = value;
            }
        }

        //Initialize the variables containing YouTube Playlist information
        List<string> playlist_items = new List<string>();
        List<string> playlist_names = new List<string>();
        public static int MAX_PLAYERS = 200;


        public frm_main()
        {
            InitializeComponent();
            cbx_name1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_name2.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_characters1.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
            cbx_characters2.MouseWheel += new MouseEventHandler(comboBox_MouseWheel);
        }

        private void frm_main_Shown(object sender, EventArgs e)
        {

            
            string url = "http://masterorders.org/masterorders.html";
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if(result != "enabled")
                        {
                            MessageBox.Show("Master Orders is either out of date or not enabled for your organization. " +
                                "Please reach out to Serris via Twitter @serrisfroon for further support.");
                            if (System.Windows.Forms.Application.MessageLoop)
                            {
                                // WinForms app
                                System.Windows.Forms.Application.Exit();
                            }
                            else
                            {
                                // Console app
                                System.Environment.Exit(1);
                            }
                            return;
                        }

                    }
                }
            }

            //Set the player fields to the default values
            player_boxes[1] = new player_fields();
            player_boxes[2] = new player_fields();
            player_boxes[3] = new player_fields();
            player_boxes[4] = new player_fields();
            player_boxes[1].tag = cbx_name1;
            player_boxes[1].twitter = txt_alt1;
            player_boxes[1].character = cbx_characters1;
            player_boxes[1].color = cbx_colors1;
            player_boxes[2].tag = cbx_name2;
            player_boxes[2].twitter = txt_alt2;
            player_boxes[2].character = cbx_characters2;
            player_boxes[2].color = cbx_colors2;

            player_boxes[3].tag = cbx_team1_name2;
            player_boxes[3].twitter = txt_team1_twitter2;
            player_boxes[3].character = cbx_team1_character2;
            player_boxes[3].color = cbx_team1_color2;
            player_boxes[4].tag = cbx_team2_name2;
            player_boxes[4].twitter = txt_team2_twitter2;
            player_boxes[4].character = cbx_team2_character2;
            player_boxes[4].color = cbx_team2_color2;


            global_values.roster = new player_info[MAX_PLAYERS];
            global_values.player_roster_number = new int[5] { -1, -1, -1, -1, -1 };

            //Check if a settings file exists
            if (!Directory.Exists(@"C:\Users\Public\Stream Info Handler"))
            {
                Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler");
            }

            if (!File.Exists(global_values.settings_file))
            {
                //Show the settings initial setup window to create a settings file
                var settings_box = new frm_settings_start();
                Point starting_location = this.Location;
                starting_location = Point.Add(starting_location, new Size(0, -200));
                settings_box.Location = starting_location;
                settings_box.ShowDialog();
            }
           


            //Load the settings file data
            XDocument xml = XDocument.Load(global_values.settings_file);

            string version = (string)xml.Root.Element("etc").Element("settings-version");
            switch(version)
            {
                case "4":
                    break;
                case "3":
                    xml.Root.Element("etc").Add(new XElement("format", "Singles"));
                    xml.Root.Element("etc").Element("settings-version").ReplaceWith(new XElement("settings-version", "4"));
                    xml.Root.Element("google-sheets").Element("startup-sheets").Remove();
                    xml.Save(global_values.settings_file);
                    MessageBox.Show("The settings file has been updated for use with this version of Master Orders.");

                    break;
                case "2":
                    xml.Root.Element("etc").Add(new XElement("format", "Singles"));
                    xml.Root.Element("google-sheets").Element("startup-sheets").Remove();

                    xml.Root.Add(new XElement("sponsor-and-region",
                                 new XElement("enable-sponsor", "False"),
                                 new XElement("sponsor-directory", ""),
                                 new XElement("enable-region", "False"),
                                 new XElement("region-directory", "")));
                    xml.Root.Element("etc").Element("settings-version").ReplaceWith(new XElement("settings-version", "4"));
                    xml.Save(global_values.settings_file);
                    MessageBox.Show("The settings file has been updated for use with this version of Master Orders.");
                    break;
                default:
                    MessageBox.Show("The settings file is out of date and must be recreated.");
                    File.Delete(global_values.settings_file);
                    //Show the settings initial setup window to create a settings file
                    var settings_box = new frm_settings_start();
                    Point starting_location = this.Location;
                    starting_location = Point.Add(starting_location, new Size(0, -200));
                    settings_box.Location = starting_location;
                    settings_box.ShowDialog();
                    xml = XDocument.Load(global_values.settings_file);
                    break;
            }

            cbx_format.Text = (string)xml.Root.Element("etc").Element("format");

            //Create a directory monitor
            global_values.vod_monitor = new FileSystemWatcher();
            global_values.vod_monitor.Created += FileSystemWatcher_Created;             //Associate the file creation event to the monitor
            global_values.vod_monitor.Deleted += FileSystemWatcher_Deleted;             //Associate the file deletion event to the monitor
            global_values.vod_monitor.Renamed += FileSystemWatcher_Renamed;             //Associate the file deletion event to the monitor

            //Read the stream file and thumbnail output directories from the data
            txt_stream_directory.Text = (string)xml.Root.Element("directories").Element("stream-directory");
            txt_thumbnail_directory.Text = (string)xml.Root.Element("directories").Element("thumbnail-directory");
            txt_roster_directory.Text = (string)xml.Root.Element("directories").Element("game-directory");
            txt_vods.Text = (string)xml.Root.Element("directories").Element("vods-directory");

            //Read the region setting and directory from the data
            txt_region.Text = (string)xml.Root.Element("sponsor-and-region").Element("region-directory");
            ckb_region.Checked = Convert.ToBoolean((string)xml.Root.Element("sponsor-and-region").Element("enable-region"));

            //Read the sponsor setting and directory from the data
            txt_sponsor.Text = (string)xml.Root.Element("sponsor-and-region").Element("sponsor-directory");
            ckb_sponsor.Checked = Convert.ToBoolean((string)xml.Root.Element("sponsor-and-region").Element("enable-sponsor"));

            //Read the automatic updates flag from the data
            rdb_automatic.Checked = Convert.ToBoolean((string)xml.Root.Element("etc").Element("automatic-updates"));
            rdb_manual.Checked = !rdb_automatic.Checked;


            txt_json.Text = (string)xml.Root.Element("youtube").Element("json-file");
            global_values.youtube_username = (string)xml.Root.Element("youtube").Element("username");

            //Read the Youtube Information from the data
            ckb_youtube.Checked = Convert.ToBoolean((string)xml.Root.Element("youtube").Element("enable-youtube"));
            txt_description.Text = (string)xml.Root.Element("youtube").Element("default-description");
            ckb_clipboard.Checked = Convert.ToBoolean((string)xml.Root.Element("youtube").Element("copy-title"));

            //Read the YouTube Playlist flag from the data
            string playlist_name = (string)xml.Root.Element("youtube").Element("playlist-name");
            //Check if playlists are enabled
            if (playlist_name != "")
            {
                global_values.playlist_name = playlist_name;
                global_values.playlist_id = (string)xml.Root.Element("youtube").Element("playlist-id");
                txt_playlist.Text = playlist_name;
            }


            //Read the Google Sheets Information from the data
            ckb_sheets.Checked = Convert.ToBoolean((string)xml.Root.Element("google-sheets").Element("enable-sheets"));
            txt_sheets.Text = (string)xml.Root.Element("google-sheets").Element("sheets-id");

            global_values.sheets_style = (string)xml.Root.Element("google-sheets").Element("sheet-style");
            global_values.sheets_info = (string)xml.Root.Element("google-sheets").Element("sheet-info");
            if (global_values.sheets_info == "info-only")
            {
                rdb_infoonly.Checked = true;
            }
            else
            {
                if (global_values.sheets_info == "info-and-queue")
                {
                    rdb_fullsheet.Checked = true;
                    btn_previous_match.Enabled = true;
                    btn_previous_match.Visible = true;
                }
                else
                {
                    rdb_infoonly.Checked = true;
                }
            }
            btn_test_sheet.Enabled = false;




            //Check if the Google Sheets integration is enabled
            if (ckb_sheets.Checked = true && File.Exists(txt_json.Text))
            {                    
                //Check if the Google Sheet ID is empty
                if (txt_sheets.Text == "")
                {
                    //Mark the field for an error and switch tabs to show it
                    txt_sheets.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_integrations.SelectedIndex = 2;
                }
                else
                {             
                    if (global_values.sheets_style == "info-and-queue")
                    {
                        rdb_fullsheet.Enabled = true;
                        rdb_infoonly.Enabled = true;
                        cbx_format.Enabled = false;
                    }
                    btn_test_sheet.Enabled = false;

                    xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", ""));
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", ""));
                    xml.Save(global_values.settings_file);
                    ignore_settings = true;

                    info_from_sheets();

                    ignore_settings = false;
                    xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", global_values.json_file));
                    xml.Save(global_values.settings_file);
                    //Enable the player data save buttons
                    btn_save1.Enabled = true;
                    btn_save1.Visible = true;
                    btn_save2.Visible = true;
                    btn_save2.Enabled = true;
                }
            }

            global_values.stream_software = (string)xml.Root.Element("etc").Element("stream-software");

            if (global_values.stream_software == "OBS")
            {
                rdb_obs.Checked = true;
            }


            //Read the scoreboard flag from the data
            string use_scoreboard = (string)xml.Root.Element("image-scoring").Element("enable-image-scoring");
            //Read each score image location from the data
            global_values.score1_image1 = (string)xml.Root.Element("image-scoring").Element("player1-1");
            global_values.score1_image2 = (string)xml.Root.Element("image-scoring").Element("player1-2");
            global_values.score1_image3 = (string)xml.Root.Element("image-scoring").Element("player1-3");
            global_values.score2_image1 = (string)xml.Root.Element("image-scoring").Element("player2-1");
            global_values.score2_image2 = (string)xml.Root.Element("image-scoring").Element("player2-2");
            global_values.score2_image3 = (string)xml.Root.Element("image-scoring").Element("player2-3");


            //Check if Image Scoreboard use is enabled
            if (use_scoreboard == "True")
            {
                ckb_scoreboad.Checked = true;                                           //Check the setting box
                //Verify that all images exist
                if (!File.Exists(global_values.score1_image1))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score1_image1.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);

                if (!File.Exists(global_values.score1_image2))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score1_image2.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);

                if (!File.Exists(global_values.score1_image3))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score1_image3.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);

                if (!File.Exists(global_values.score2_image1))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score2_image1.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);

                if (!File.Exists(global_values.score2_image2))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score2_image2.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);

                if (!File.Exists(global_values.score2_image3))
                {
                    //Mark the button for an error and switch tabs to show it
                    btn_score2_image3.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 1;
                }
                else pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);

            }



            //Set the date box to today's date
            DateTime date = DateTime.Now;
            txt_date.Text = date.ToString("M/dd/yy");

            //////////////Set tooltips
            //Set tooltips for Tournament Setup tab
                                                                //
            ttp_tooltip.SetToolTip(txt_tournament, 
                "Set the name of the tournament. Used in the\n" +
                "name of YouTube uploads. YouTube video \n" +
                "descriptions may also use it.");
            ttp_tooltip.SetToolTip(txt_bracket, 
                "Set the link to the bracket to be displayed.");
            ttp_tooltip.SetToolTip(txt_date, 
                "Change the date displayed in thumbnails \n" +
                "created and YouTube video descriptions.");
                                                                //
            //Set tooltips for the In-Game Display tab
            ttp_tooltip.SetToolTip(cbx_name1, 
                "Set the name/tag for Player 1.");
            ttp_tooltip.SetToolTip(cbx_name2, 
                "Set the name/tag for Player 2.");
            ttp_tooltip.SetToolTip(txt_alt1, 
                "Set the twitter handle for Player 1.");
            ttp_tooltip.SetToolTip(txt_alt2,
                "Set the twitter handle for Player 2.");
            ttp_tooltip.SetToolTip(cbx_characters1, 
                "Set the character for Player 1. This affects\n" +
                "YouTube uploads and stock icons.");
            ttp_tooltip.SetToolTip(cbx_characters2,
                "Set the character for Player 2. This affects\n" +
                "YouTube uploads and stock icons.");
            ttp_tooltip.SetToolTip(cbx_colors1,
                "Set the color of Player 1's character.");
            ttp_tooltip.SetToolTip(cbx_colors2,
                "Set the color of Player 2's character.");
            ttp_tooltip.SetToolTip(cbx_colors1,
                "Add [L] next to the player 1's name in the stream\n" +
                "file to signify they are on loser's side of Grand\n" +
                "Finals.");
            ttp_tooltip.SetToolTip(cbx_colors2,
                "Add [L] next to the player 2's name in the stream\n" +
                "file to signify they are on loser's side of Grand\n" +
                "Finals.");
            ttp_tooltip.SetToolTip(btn_save1,
                "Save this configuration for Player 1 to the \n" +
                "link Google Sheet. This information can later\n" +
                "be loaded by selecting the player's name from\n" +
                "the Player Tag box.");
            ttp_tooltip.SetToolTip(btn_save2,
                "Save this configuration for Player 2 to the \n" +
                "link Google Sheet. This information can later\n" +
                "be loaded by selecting the player's name from\n" +
                "the Player Tag box.");
                                                                //
            ttp_tooltip.SetToolTip(btn_swap,
                "Switch the player information between Player 1 \n" +
                "and Player 2.");
            ttp_tooltip.SetToolTip(btn_reset_scores,
                "Reset both player scores to 0.");
            ttp_tooltip.SetToolTip(btn_update,
                "Click to begin the set and enable score control.\n" +
                "Pushes player and match information into the\n" +
                "Stream Files Directory.");
                                                                //
            ttp_tooltip.SetToolTip(btn_reset, 
                "Queue up the next match. This will clear all player\n" +
                "information and reset the score. If the Google\n" +
                "Sheets integration is enabled and Master Orders\n" +
                "is set to load Player Info and Stream Queue,\n" +
                "These fields will populate with the next match's\n" +
                "information from the stream queue.");
            ttp_tooltip.SetToolTip(btn_previous_match,
                "Reset the match and pull information for the\n" +
                "previous match in the stream queue.");
            ttp_tooltip.SetToolTip(btn_upload,
                "Upload the VoD for this match to YouTube. The VoD\n" +
                "file will be pulled from the VoD Directory so long\n" +
                "as recording is finished. VoD data will be stored\n" +
                "in a .uldata file in the Thumbnail Directory.");
                                                                //
            ttp_tooltip.SetToolTip(cbx_round, 
                "Set the current round in bracket.");

            //Set tooltips for the Commentators tab
            ttp_tooltip.SetToolTip(txt_tag1, 
                "Set the name/tag for the left commentator.");
            ttp_tooltip.SetToolTip(txt_tag2, 
                "Set the name/tag for the right commentator.");
            ttp_tooltip.SetToolTip(txt_commentatoralt1, 
                "Set the twitter handle for the left commentator.");
            ttp_tooltip.SetToolTip(txt_commentatoralt2, 
                "Set the twitter handle for the right commentator.");
                                                                //
            ttp_tooltip.SetToolTip(btn_update_commentators, 
                "Push through any updates made to the commentator\n" +
                "information.");
            ttp_tooltip.SetToolTip(btn_swapcommentators, 
                "Switch the information between two commentators.");

            //Set tooltips for the Settings tab
                                                                //
            //Set the tooltips for the Directories subtab
            ttp_tooltip.SetToolTip(txt_roster_directory, 
                "Set the directory to the Character Roster.");
            ttp_tooltip.SetToolTip(btn_browse_roster, 
                "Choose the directory to the Character Roster.");
            ttp_tooltip.SetToolTip(txt_stream_directory, 
                "Set the output directory for the stream\n" +
                "information files.");
            ttp_tooltip.SetToolTip(btn_output, 
                "Choose the output directory for the stream\n" +
                "information files.");
            ttp_tooltip.SetToolTip(txt_thumbnail_directory, 
                "Set the output directory for YouTube thumbnail\n" +
                "images and .uldata files.");
            ttp_tooltip.SetToolTip(btn_thumb_directory,
                "Choose the output directory for YouTube thumbnail\n" +
                "images and .uldata files.");
            ttp_tooltip.SetToolTip(txt_vods, 
                "Set the directory where VoDs will be created by\n" +
                "the streaming program. The directory will be\n" +
                "monitored to populate the most recently added VoD\n" +
                "for uploading to YouTube.");
            ttp_tooltip.SetToolTip(btn_vods, 
                "Choose the directory where VoDs will be created by\n" +
                "the streaming program. The directory will be\n" +
                "monitored to populate the most recently added VoD\n" +
                "for uploading to YouTube.");
                                                                //
            //Set the tooltips for the Scoreboard subtab
            ttp_tooltip.SetToolTip(rdb_automatic,
                "Use automatic stream file updating. Changes to\n" +
                "player and tournament information will automatically\n" +
                "be pushed to the Stream Files Directory.");
            ttp_tooltip.SetToolTip(rdb_manual,
                "Use manual stream file updating. Changes to player\n" +
                "and tournament information will only be pushed to\n" +
                "the Stream Files Directory when clicking Update.");
            ttp_tooltip.SetToolTip(ckb_scoreboad,
                "Toggle the use of image scoreboard. Enabling it\n" +
                "limits the max score to 3 but updates images in the\n" +
                "Stream File Directory to reflect each player's score.");
            ttp_tooltip.SetToolTip(btn_score1_image1,
                "Change the image for Player 1's score at 1 point.");
            ttp_tooltip.SetToolTip(btn_score1_image2,
                "Change the image for Player 1's score at 2 points.");
            ttp_tooltip.SetToolTip(btn_score1_image3,
                "Change the image for Player 1's score at 3 points.");
            ttp_tooltip.SetToolTip(btn_score2_image1,
                "Change the image for Player 2's score at 1 point.");
            ttp_tooltip.SetToolTip(btn_score2_image2,
                "Change the image for Player 2's score at 2 points.");
            ttp_tooltip.SetToolTip(btn_score2_image3,
                "Change the image for Player 2's score at 3 points.");
                                                                //
            //Set the tooltips for Google Integrations subtab
            ttp_tooltip.SetToolTip(txt_json, 
                "Set the path to the .json file used for YouTube\n" +
                "uploads. This file is obtained through the Google\n" +
                "Developer Console.");
            ttp_tooltip.SetToolTip(btn_browse_json,
                "Select the path to the .json file used for YouTube\n" +
                "uploads. This file is obtained through the Google\n" +
                "Developer Console.");
            ttp_tooltip.SetToolTip(ckb_youtube,
                "Enable YouTube VoD uploading. See the YouTube tab for\n" +
                "related settings.");
            ttp_tooltip.SetToolTip(ckb_sheets,
                "Enable the importing of data from Google Sheets. See\n" +
                "the Google Sheets tab for further required setup and\n" +
                "settings.");
            ttp_tooltip.SetToolTip(ckb_clipboard,
                "Copy the YouTube title to the clipboard whenever a\n" +
                "thumbnail is created.");
                                                                //
            //Set the tooltips for the YouTube subtab
            ttp_tooltip.SetToolTip(txt_playlist, 
                "Enter the name of the YouTube playlist. Leave this\n" +
                "empty to disable playlists.");
            ttp_tooltip.SetToolTip(btn_playlist, 
                "Update the YouTube playlist to the entered playlist name.");
            ttp_tooltip.SetToolTip(txt_description, 
                "Set the default description for uploaded YouTube VoDs.\n" +
                "You can use the following keywords to insert data from\n" +
                "Master Orders:\n" +
                "INFO_TOURNAMENT INFO_DATE INFO_BRACKET INFO_ROUND\n" +
                "INFO_PLAYER1 INFO_PLAYER2 INFO_CHARACTER1 INFO_CHARACTER2\n" +
                "INFO_TWITTER1 INFO_TWITTER2");
            ttp_tooltip.SetToolTip(rdb_xsplit,
                "Set the streaming application to XSplit.");
            ttp_tooltip.SetToolTip(rdb_obs,
                "Set the streaming application to OBS Studio.");
                                                                //
            //Set the tooltips for the Google Sheets subtab
            ttp_tooltip.SetToolTip(txt_sheets,
                "Enter the Google Sheet ID of the desired Sheet. In the\n" +
                "website URL, this is the string of characters after:\n" +
                "https://docs.google.com/spreadsheets/d/\n" +
                "and before /edit");
            ttp_tooltip.SetToolTip(btn_test_sheet,
                "Test the above entered Sheet ID. This will verify\n" +
                "that the associated sheet is formatted for use with\n" +
                "Master Orders and will determine the type of information\n" +
                "it contains.");
            ttp_tooltip.SetToolTip(rdb_fullsheet,
                "Set Master Orders to load both player information and\n" +
                "the stream queue from the Google Sheet.");
            ttp_tooltip.SetToolTip(rdb_infoonly,
                "Set Master Orders to load only player information from\n" +
                "the Google Sheet.");
                                                                //
            //Set the tooltips for the Other Tools tab
            ttp_tooltip.SetToolTip(btn_top8,
                "Open a new window to create a Top 8 graphic.");
            ttp_tooltip.SetToolTip(btn_thumbnail, 
                "Create a thumbnail image based on the current\n" +
                "player and tournament information.");
            ttp_tooltip.SetToolTip(btn_upload_vod,
                "Select a .uldata file to import data for a\n" +
                "YouTube upload.");
            ttp_tooltip.SetToolTip(btn_dashboard,
                "Open the Stream Queue Dashboard. The Google Sheets\n" +
                "integration must be enabled and set to use Info and\n" +
                "Queue to use this.");
            ttp_tooltip.SetToolTip(btn_addplayer,
                "Add a new player to the Google Sheet database. The\n" +
                "Google Sheets integration must be enabled to use this.");

        }

        //Create a thumbnail image using the information input for the players and tournament
        public string create_thumbnail(int player_count, 
            string player_name1, string player_name2, string round_text, string match_date)
        {
            //Create a new bitmap image
            Image thumbnail_bmp = new Bitmap(1920, 1080);
            //Create a new drawing surface from the bitmap
            Graphics drawing = Graphics.FromImage(thumbnail_bmp);
            //Configure the surface to be higher quality
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            //Create an image resource for the background and overlay of the tumbnail
            Image background = Image.FromFile(global_values.game_path + @"\thumbnail_background.jpg");
            Image foreground = Image.FromFile(global_values.game_path + @"\thumbnail_overlay.png");


            //Create an image resource for each player's character
            Image left_character;
            Image right_character;
            Image left_character2;
            Image right_character2;

            switch(player_count)
            {
                case 2:
                    left_character = Image.FromFile(image_directory1 + @"\left.png");
                    right_character = Image.FromFile(image_directory2 + @"\right.png");
                    drawing.Clear(Color.White);                                         //Clear the surface of all data

                    drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

                    drawing.DrawImage(left_character, 0, 0, 1920, 1080);                //Draw Player 1's character
                    drawing.DrawImage(right_character, 0, 0, 1920, 1080);               //Draw Player 2's character

                    drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters
                    break;
                case 4:
                    left_character = Image.FromFile(image_directory1 + @"\left.png");
                    right_character = Image.FromFile(image_directory2 + @"\right.png");
                    left_character2 = Image.FromFile(image_directory3 + @"\left.png");
                    right_character2 = Image.FromFile(image_directory4 + @"\right.png");
                    drawing.Clear(Color.White);                                         //Clear the surface of all data

                    drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

                    drawing.DrawImage(left_character2, -100, 0, 1920, 1080);                //Draw Player 3's character
                    drawing.DrawImage(right_character2, 100, 0, 1920, 1080);               //Draw Player 4's character

                    drawing.DrawImage(left_character, 0, 200, 1920, 1080);                //Draw Player 1's character
                    drawing.DrawImage(right_character, 0, 200, 1920, 1080);               //Draw Player 2's character

                    drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters

                    break;
            }

            //Convert each player's name and the round in bracket to all capital letters and store them seperately
            player_name1 = player_name1.ToUpper();
            player_name2 = player_name2.ToUpper();
            round_text = round_text.ToUpper();

            //Create a drawing path for the text of the date, each player's name, and the round in bracket
            GraphicsPath draw_date = new GraphicsPath();
            GraphicsPath draw_name1 = new GraphicsPath();
            GraphicsPath draw_name2 = new GraphicsPath();
            GraphicsPath draw_round = new GraphicsPath();
            //Create a brush for Black and White
            Brush white_text = new SolidBrush(Color.White);
            Brush black_text = new SolidBrush(Color.Black);
            //Create two pen brushes- one normal thiccness, one thiccer
            Pen black_stroke = new Pen(black_text, 14);
            Pen light_stroke = new Pen(black_text, 10);
            //Create a text format with center alignments
            StringFormat text_center = new StringFormat();
            text_center.Alignment = StringAlignment.Center;
            text_center.LineAlignment = StringAlignment.Center;
            //Create font resources
            FontFamily keepcalm = new FontFamily("Keep Calm Med");
            Font calmfont = new Font("Keep Calm Med", 110, FontStyle.Regular);

            int font_size = 115;                                                //Create a variable for adjustable font size
            Size namesize = TextRenderer.MeasureText(player_name1, calmfont);   //Create a variable to hold the size of player tags

            //Add the date to its drawing path
            draw_date.AddString(
                match_date,                                                     //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                110,                                                            //font size (drawing.DpiY * 120 / 72)
                new Point(300, 980),                                            //drawing location
                text_center);                                                   //text alignment
            //Set the outline and filling to the appropriate colors
            //drawing.DrawPath(black_stroke, draw_date);
            //drawing.FillPath(white_text, draw_date);

            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name1, calmfont);            //Measure the width of Player 1's name with this font size
            } while (namesize.Width >= 1000);      //1100                                     //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 1's name to its drawing path
            draw_name1.AddString(
                player_name1,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(420, 160),          //110                                  //drawing location 480
                text_center);                                                   //text alignment
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name1);
            drawing.FillPath(white_text, draw_name1);

            font_size = 115;                      //115                                      //Reset the font size
            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name2, calmfont);            //Measure the width of Player 2's name with this font size
            } while (namesize.Width >= 1000);        //1100                                   //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 2's name to its drawing path
            draw_name2.AddString(
                player_name2,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(1500, 160), //110                                          //drawing location 1440
                text_center);                                                   //text alignment                                        // text to draw
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name2);
            drawing.FillPath(white_text, draw_name2);

            //Add the round in bracket to its drawing path
            draw_round.AddString(
               round_text,                                                      //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                60,                                                             //font size (drawing.DpiY * 120 / 72)
                new Point(960, 720), //620                                           //drawing location
                text_center);                                                   //text alignment     
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(light_stroke, draw_round);
            drawing.FillPath(white_text, draw_round);

            //Save the drawing surface back to the bitmap image
            drawing.Save();
            //Dispose the drawing surface
            drawing.Dispose();

            DateTime date = DateTime.Now;                                       //Find the current date and time
            string thumbnail_time = date.ToString("MMddyyyyHHmmss");            //Format the date and time in a string
            //Create a title for the bitmap image using the information provided by the user
            string thumbnail_image_name = txt_tournament.Text + @" " + cbx_round.Text + @" " + cbx_characters1.Text + @" Vs " + cbx_characters2.Text + @" " + thumbnail_time + @".jpg";
            //Save the bitmap image as a JPG
            thumbnail_bmp.Save(global_values.thumbnail_directory + @"\" + thumbnail_image_name, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Return the title of the image file
            return thumbnail_image_name;
        }

        private void btn_swap_Click(object sender, EventArgs e)
        {
            //Hold Player 1's information within temporary variables
            string hold_alt = txt_alt1.Text;
            string hold_name = cbx_name1.Text;
            decimal hold_score = nud_score1.Value;
            string hold_character = cbx_characters1.Text;
            int hold_color = cbx_colors1.SelectedIndex;
            string hold_directory = image_directory1;
            bool hold_L = ckb_loser1.Checked;

            //Move Player 2's information to Player 1's slot
            txt_alt1.Text = txt_alt2.Text;
            cbx_name1.Text = cbx_name2.Text;
            nud_score1.Value = nud_score2.Value;
            cbx_characters1.Text = cbx_characters2.Text;
            cbx_colors1.SelectedIndex = cbx_colors2.SelectedIndex;
            image_directory1 = image_directory2;
            ckb_loser1.Checked = ckb_loser2.Checked;

            //Move the information stored within temporary variables to Player 2's slot
            txt_alt2.Text = hold_alt;
            cbx_name2.Text = hold_name;
            nud_score2.Value = hold_score;
            cbx_characters2.Text = hold_character;
            cbx_colors2.SelectedIndex = hold_color;
            image_directory2 = hold_directory;
            ckb_loser2.Checked = hold_L;

        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder containing the character roster
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_roster_directory.Text = folderBrowserDialog1.SelectedPath;                            //Update the setting text
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //Reset the image of the update button to default
            btn_update.Image = null;
            string output_name = "";

            switch (btn_update.Text)                        //Perform action based on the current text of the button
            {
                case "Start":                               //Start the match
                    nud_score1.Enabled = true;              //Enable score control for Player 1
                    nud_score2.Enabled = true;              //Enable score control for Player 2
                    btn_update.Enabled = false;             //Disable this button until further action is needed
                    btn_update.Text = "Update";             //Update the text of this button
                    ttp_tooltip.SetToolTip(btn_update,
                        "Pushes updates to the player and match information " +
                        "into the Stream Files Directory.");
                    //Create a ULdata for the new match
                    global_values.current_youtube_data = create_uldata(2);
                    //Save Player 1's information to seperate files to be used by the stream program
                    output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                    //Save Player 2's information to seperate files to be used by the stream program
                    output_name = get_output_name(cbx_name2.Text, ckb_loser2.Checked, 2);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", cbx_characters2.Text);
                    //Save the Tournament information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);

                    break;
                case "Update":                              //Update the stream files with the new information provided
                    btn_update.Enabled = false;             //Disable this button until further action is needed
                    update_uldata(1, global_values.current_youtube_data);
                    update_uldata(2, global_values.current_youtube_data);
                    update_uldata(5, global_values.current_youtube_data);
                    //Save Player 1's information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                    //Save Player 2's information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    output_name = get_output_name(cbx_name2.Text, ckb_loser2.Checked, 2);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", cbx_characters2.Text);
                    //Save the Tournament information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);

                    //Check if Image Scoreboard is enabled
                    if (ckb_scoreboad.Checked == true)
                    {
                        //Store the location of the score image for Player 1 used by the stream program
                        string score_file = global_values.output_directory + @"\score1.png";

                        //Delete the score image if it exists
                        if (File.Exists(score_file))
                        {
                            File.Delete(score_file);
                        }

                        //Check the current value of Player 1's score
                        switch (nud_score1.Value)
                        {
                            case 0:                     //Save an empty image for Player 1's score                                      
                                File.Copy(@"left.png", score_file);
                                break;
                            case 1:                     //Copy the Player 1 Score 1 image for Player 1's score
                                File.Copy(global_values.score1_image1, score_file);
                                break;
                            case 2:                     //Copy the Player 1 Score 2 image for Player 1's score
                                File.Copy(global_values.score1_image2, score_file);
                                break;
                            case 3:                     //Copy the Player 1 Score 3 image for Player 1's score
                                File.Copy(global_values.score1_image3, score_file);
                                break;
                        }

                        score_file = global_values.output_directory + @"\score2.png";

                        if (File.Exists(score_file))
                        {
                            File.Delete(score_file);
                        }

                        switch (nud_score2.Value)
                        {
                            case 0:
                                File.Copy(@"left.png", score_file);
                                break;
                            case 1:
                                File.Copy(global_values.score2_image1, score_file);
                                break;
                            case 2:
                                File.Copy(global_values.score2_image2, score_file);
                                break;
                            case 3:
                                File.Copy(global_values.score2_image3, score_file);
                                break;

                        }
                    }

                    break;
            }
        }

        //This function is passed to frm_upload to control the update button on the main form
        void upload_form_enable_button_event(string check_reenable)
        {
            //Check if the reenable value from the upload form matches the current value stored
            if (check_reenable == global_values.reenable_upload)
            {
                btn_upload.Enabled = true;                  //Re-enable the button
                btn_upload.Text = "Upload to YouTube";
            }
        }

        private void btn_output_Click_1(object sender, EventArgs e)
        {
            //Ask the user to select the folder to store stream files in
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_stream_directory.Text = folderBrowserDialog1.SelectedPath;                 //Update the global value with the new directory
            }
        }

        private void btn_swapcommentators_Click(object sender, EventArgs e)
        {
            //Hold Commentator 1's information in temporary variables
            string hold_name = txt_tag1.Text;
            string hold_alt = txt_commentatoralt1.Text;

            //Move Commentator 2's information to Commentator 1's fields
            txt_tag1.Text = txt_tag2.Text;
            txt_commentatoralt1.Text = txt_commentatoralt2.Text;

            //Move the information in the temporary variables to Commentator 2's fields
            txt_tag2.Text = hold_name;          
            txt_commentatoralt2.Text = hold_alt;

            //Write Commentator 1's information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name1.txt", txt_tag1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt1.txt", txt_commentatoralt1.Text);
            //Write Commentator 2's information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name2.txt", txt_tag2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt2.txt", txt_commentatoralt2.Text);
        }

        private void btn_update_commentators_Click(object sender, EventArgs e)
        {
            btn_update_commentators.Enabled = false;                //Disable the button until information is updated
            //Write Commentator 1's information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name1.txt", txt_tag1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt1.txt", txt_commentatoralt1.Text);
            //Write Commentator 2's information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name2.txt", txt_tag2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt2.txt", txt_commentatoralt2.Text);
        }

        private void btn_thumbnail_Click(object sender, EventArgs e)
        {
            string video_title = txt_tournament.Text + @" - " + cbx_round.Text + @" - " + cbx_name1.Text + @" (" + cbx_characters1.Text + @") Vs. " + cbx_name2.Text + @" (" + cbx_characters2.Text + @")";
            if (global_values.copy_video_title == true)
            {
                Clipboard.SetText(video_title);
                MessageBox.Show("Video title copied to clipboard: \n" + video_title);
            }

            //Create a thumbnail image
            string thumbnail_image_name = create_thumbnail(2,
                cbx_name1.Text,
                cbx_name2.Text,
                cbx_round.Text,
                txt_date.Text);
        }

        private void btn_thumb_directory_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder to store thumbnail images in
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_thumbnail_directory.Text = folderBrowserDialog1.SelectedPath;                   //Update the setting with the new information
            }
        }

        private void nud_score1_ValueChanged(object sender, EventArgs e)
        {
            decimal current_point = nud_score1.Value;       //Pull the current game wins for Player 1

            //Keep the current point value at or below the match point value
            if (current_point >= 3 && ckb_scoreboad.Checked == true)
            {
                nud_score1.Value = 3;
            }
            //Check if automatic updates are enabled
            if (global_values.auto_update == true)
            {
                //Write Player 1's score to a file to be used by the stream program
                System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                //Check if Image Scoreboard is enabled
                if (ckb_scoreboad.Checked == true)
                {
                    //Store the location of the score image for Player 1 used by the stream program
                    string score_file = global_values.output_directory + @"\score1.png";

                    //Delete the score image if it exists
                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    //Check the current value of Player 1's score
                    switch (nud_score1.Value)
                    {
                        case 0:                     //Save an empty image for Player 1's score                                      
                            File.Copy(@"left.png", score_file);                     
                            break;
                        case 1:                     //Copy the Player 1 Score 1 image for Player 1's score
                            File.Copy(global_values.score1_image1, score_file);
                            break;
                        case 2:                     //Copy the Player 1 Score 2 image for Player 1's score
                            File.Copy(global_values.score1_image2, score_file);
                            break;
                        case 3:                     //Copy the Player 1 Score 3 image for Player 1's score
                            File.Copy(global_values.score1_image3, score_file);
                            break;
                    }
                }
            }
            else
            {

                btn_update.Enabled = true;                                                              //Unable the update button
                btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");    //Add a yellow glow to the update button
            }
        }

        private void nud_score2_ValueChanged(object sender, EventArgs e)
        {
            decimal current_point = nud_score2.Value;

            if (current_point >= 3 && ckb_scoreboad.Checked == true)
            {
                nud_score2.Value = 3;
            }
            if (global_values.auto_update == true)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                if (ckb_scoreboad.Checked == true)
                {
                    string score_file = global_values.output_directory + @"\score2.png";

                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    switch (nud_score2.Value)
                    {
                        case 0:
                            File.Copy(@"left.png", score_file);
                            break;
                        case 1:
                            File.Copy(global_values.score2_image1, score_file);
                            break;
                        case 2:
                            File.Copy(global_values.score2_image2, score_file);
                            break;
                        case 3:
                            File.Copy(global_values.score2_image3, score_file);
                            break;
                    }
                }
            }
            else
            {
                btn_update.Enabled = true;
                btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
            }
        }

        private void cbx_name2_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            string output_name = get_output_name(cbx_name2.Text, ckb_loser2.Checked, 2);
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
                        update_uldata(2, global_values.current_youtube_data);                        
                    }
                    break;
            }
        }

        private void cbx_name1_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            string output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                    }
                    break;
            }
        }

        private void cbx_round_TextChanged(object sender, EventArgs e)
        {
             btn_update.Enabled = true;
            if(cbx_round.Text == "Grand Finals")
            {
                ckb_loser1.Enabled = true;
                ckb_loser1.Visible = true;
                ckb_loser2.Enabled = true;
                ckb_loser2.Visible = true;
            }
            else
            {
                ckb_loser1.Checked = false;
                ckb_loser1.Enabled = false;
                ckb_loser1.Visible = false;
                ckb_loser2.Checked = false;
                ckb_loser2.Enabled = false;
                ckb_loser2.Visible = false;
            }
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                        update_uldata(5, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void txt_tournament_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
                        update_uldata(5, global_values.current_youtube_data);

                    }
                    break;
            }
        }

        private void txt_bracket_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                        update_uldata(5, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (global_values.enable_youtube == true)
            {
                global_values.reenable_upload = "";
                btn_upload.Enabled = true;
                btn_upload.Text = "Upload to YouTube";
            }

            nud_score1.Value = 0;
            nud_score1.Enabled = false;
            nud_score2.Value = 0;
            nud_score2.Enabled = false;

            btn_update.Text = @"Start";
            ttp_tooltip.SetToolTip(btn_update,
                "Click to begin the set and enable score control.\n" +
                "Pushes player and match information into the\n" +
                "Stream Files Directory.");

            btn_update.Image = null;
            btn_update.Enabled = true;

            string[] image_files = { @"\score1.png", @"\score2.png", @"\Stock Icon 1.png", @"\Stock Icon 2.png",
                                     @"\sponsor 1.png", @"\sponsor 2.png" };
            foreach(string replace_image in image_files)
            {
                if (File.Exists(global_values.output_directory + replace_image))
                {
                    File.Delete(global_values.output_directory + replace_image);
                }
                File.Copy(@"left.png", global_values.output_directory + replace_image);
            }

            global_values.player_roster_number[1] = -1;
            global_values.player_roster_number[2] = -1;


            if (global_values.enable_sheets == true && txt_sheets.Text != "" && txt_sheets.Text != null)
            {
                if (global_values.sheets_info == "info-and-queue")
                {
                    import_from_sheets(false, 2);
                }
                else
                {
                    info_from_sheets();
                    txt_alt1.Text = @"";
                    txt_alt2.Text = @"";

                    image_directory1 = Directory.GetCurrentDirectory();
                    image_directory2 = Directory.GetCurrentDirectory();

                    cbx_characters1.SelectedIndex = 0;
                    cbx_characters2.SelectedIndex = 0;
                    cbx_colors1.SelectedIndex = 0;
                    cbx_colors2.SelectedIndex = 0;
                }
                //Enable the player data save buttons
                btn_save1.Enabled = true;
                btn_save1.Visible = true;
                btn_save2.Visible = true;
                btn_save2.Enabled = true;
            }
            else
            {
                cbx_name1.Text = @"";
                cbx_name2.Text = @"";

                txt_alt1.Text = @"";
                txt_alt2.Text = @"";

                image_directory1 = Directory.GetCurrentDirectory();
                image_directory2 = Directory.GetCurrentDirectory();

                cbx_characters1.SelectedIndex = 0;
                cbx_characters2.SelectedIndex = 0;
                cbx_colors1.SelectedIndex = 0;
                cbx_colors2.SelectedIndex = 0;
            }
        }

        private void cbx_alt1_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        update_uldata(1, global_values.current_youtube_data);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    }
                    break;
            }
        }

        private void cbx_alt2_TextChanged(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                        update_uldata(2, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_tag1_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag1.Text != @"" && txt_tag2.Text != @"")
            {

                btn_update_commentators.Enabled = true;
            }
            else
            {
                btn_update_commentators.Enabled = false;
            }
        }

        private void cbx_tag2_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag1.Text != @"" && txt_tag2.Text != @"")
            {

                btn_update_commentators.Enabled = true;
            }
            else
            {
                btn_update_commentators.Enabled = false;
            }
        }

        private void btn_browse_json_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_json.Text = openFileDialog1.FileName;
            }
        }

        private void txt_json_TextChanged(object sender, EventArgs e)
        {
            txt_json.BackColor = warning_color;
            if (txt_json.Text != @"")
            {
                if (File.Exists(txt_json.Text))
                {
                    txt_json.BackColor = Color.White;
                    global_values.json_file = txt_json.Text;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", txt_json.Text));
                    xml.Save(global_values.settings_file);
                    if (ckb_youtube.Enabled == false)
                    {
                        if (txt_json.BackColor == Color.White &&
                            txt_json.Text != "")
                        {
                            ckb_youtube.Enabled = true;
                            ckb_sheets.Enabled = true;
                        }
                    }
                }
                else
                {
                    ckb_youtube.Checked = false;
                    ckb_sheets.Checked = false;
                    ckb_youtube.Enabled = false;
                    ckb_sheets.Enabled = false;
                    tab_main.SelectedIndex = 3;
                }
            }
        }

        private void txt_stream_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_stream_directory.Text != @"")
            {
                if (Directory.Exists(txt_stream_directory.Text) &&
                    global_values.vods_directory != txt_stream_directory.Text)
                {
                    txt_stream_directory.BackColor = Color.White;
                    global_values.output_directory = txt_stream_directory.Text;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("directories").Element("stream-directory").ReplaceWith(new XElement("stream-directory", txt_stream_directory.Text));
                    xml.Save(global_values.settings_file);
                }
                else
                {
                    txt_stream_directory.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                }
            }
            else
            {
                txt_stream_directory.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }
        }

        private void txt_thumbnail_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_thumbnail_directory.Text != @"")
            {
                if (Directory.Exists(txt_thumbnail_directory.Text) &&
                    global_values.vods_directory != txt_thumbnail_directory.Text)
                {
                    txt_thumbnail_directory.BackColor = Color.White;
                    global_values.thumbnail_directory = txt_thumbnail_directory.Text;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("directories").Element("thumbnail-directory").ReplaceWith(new XElement("thumbnail-directory", txt_thumbnail_directory.Text));
                    xml.Save(global_values.settings_file);
                }
                else
                {
                    txt_thumbnail_directory.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                }
            }
            else
            {
                txt_thumbnail_directory.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }
        }

        private void txt_roster_directory_TextChanged(object sender, EventArgs e)
        {
            //Verify that a directory has been provided
            if (txt_roster_directory.Text != "")
            {
                //Verify that the necessary files for a roster directory are present
                if (File.Exists(txt_roster_directory.Text + @"\game info.txt") && 
                    File.Exists(txt_roster_directory.Text + @"\characters.txt") &&
                    global_values.vods_directory != txt_roster_directory.Text)
                {
                    txt_roster_directory.BackColor = Color.White;
                    global_values.game_path = txt_roster_directory.Text;                    //Save the directory

                    //Save the setting to the settings file
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("directories").Element("game-directory").ReplaceWith(new XElement("game-directory", txt_roster_directory.Text));
                    xml.Save(global_values.settings_file);

                    //Read the files for the game information and character roster
                    global_values.game_info = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\game info.txt");
                    global_values.characters = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\characters.txt");
                    pic_game_logo.Image = Image.FromFile(txt_roster_directory.Text + @"\game_logo.png");

                    //Update the character list combobox
                    update_characters(ref cbx_characters1);
                    update_characters(ref cbx_characters2);
                    update_characters(ref cbx_team1_character1);
                    update_characters(ref cbx_team1_character2);
                    update_characters(ref cbx_team2_character1);
                    update_characters(ref cbx_team2_character2);
                }
                else
                {
                    //If the files are not present, mark the field for an error and switch tabs to show it
                    txt_roster_directory.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                }
            }
            else
            {
                //If a directory has not been provided, mark the field for an error and switch tabs to show it
                txt_roster_directory.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }            
        }
















































        private void tab_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(ignore_settings == false)
            {
                if (tab_main.SelectedIndex != 3)
                {
                    //Check if any directory settings are invalid
                    if (txt_roster_directory.BackColor == warning_color ||
                        txt_stream_directory.BackColor == warning_color ||
                        txt_thumbnail_directory.BackColor == warning_color ||
                        txt_vods.BackColor == warning_color)
                    {
                        tab_main.SelectedIndex = 3;
                        tab_mainsettings.SelectedIndex = 0;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    //Check if any scoreboard settings are invalid
                    if (ckb_scoreboad.Checked == true &&
                        (btn_score1_image1.BackColor == warning_color ||
                        btn_score1_image2.BackColor == warning_color ||
                        btn_score1_image3.BackColor == warning_color ||
                        btn_score2_image1.BackColor == warning_color ||
                        btn_score2_image2.BackColor == warning_color ||
                        btn_score2_image3.BackColor == warning_color))
                    {
                        tab_main.SelectedIndex = 3;
                        tab_mainsettings.SelectedIndex = 1;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    if ((ckb_sponsor.Checked == true &&
                        (txt_sponsor.BackColor == warning_color ||
                         txt_sponsor.Text == "")) ||
                        (ckb_region.Checked == true &&
                        (txt_region.BackColor == warning_color ||
                         txt_region.Text == "")))
                    {
                        tab_main.SelectedIndex = 3;
                        tab_mainsettings.SelectedIndex = 2;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    if (txt_json.BackColor == warning_color)
                    {
                        tab_main.SelectedIndex = 3;
                        tab_integrations.SelectedIndex = 0;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    if (btn_playlist.Enabled == true)
                    {
                        tab_main.SelectedIndex = 3;
                        tab_integrations.SelectedIndex = 1;
                        btn_playlist.BackColor = warning_color;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                    if (btn_test_sheet.Enabled == true)
                    {
                        btn_test_sheet.BackColor = warning_color;
                        tab_main.SelectedIndex = 3;
                        tab_integrations.SelectedIndex = 2;
                        System.Media.SystemSounds.Asterisk.Play();
                    }
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ckb_scoreboad_CheckedChanged(object sender, EventArgs e)
        {
            if(ckb_scoreboad.Checked == true)
            {
                nud_score1.Value = 0;
                nud_score2.Value = 0;
                nud_score1.Maximum = 3;
                nud_score2.Maximum = 3;
                btn_score1_image1.Enabled = true;
                btn_score1_image2.Enabled = true;
                btn_score1_image3.Enabled = true;
                btn_score2_image1.Enabled = true;
                btn_score2_image2.Enabled = true;
                btn_score2_image3.Enabled = true;

                if (!File.Exists(global_values.score1_image1))
                {
                    btn_score1_image1.BackColor = warning_color;
                }
                else pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);

                if (!File.Exists(global_values.score1_image2))
                {
                    btn_score1_image2.BackColor = warning_color;
                }
                else pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);

                if (!File.Exists(global_values.score1_image3))
                {
                    btn_score1_image3.BackColor = warning_color;
                }
                else pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);

                if (!File.Exists(global_values.score2_image1))
                {
                    btn_score2_image1.BackColor = warning_color;
                }
                else pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);

                if (!File.Exists(global_values.score2_image2))
                {
                    btn_score2_image2.BackColor = warning_color;
                }
                else pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);

                if (!File.Exists(global_values.score2_image3))
                {
                    btn_score2_image3.BackColor = warning_color;
                }
                else pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);


                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", "True"));
                xml.Save(global_values.settings_file);
            }
            else
            {
                nud_score1.Maximum = 99;
                nud_score2.Maximum = 99;
                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", "False"));
                xml.Save(global_values.settings_file);
                btn_score1_image1.Enabled = false;
                btn_score1_image2.Enabled = false;
                btn_score1_image3.Enabled = false;
                btn_score2_image1.Enabled = false;
                btn_score2_image2.Enabled = false;
                btn_score2_image3.Enabled = false;

                btn_score1_image1.BackColor = Color.Transparent;
                btn_score1_image2.BackColor = Color.Transparent;
                btn_score1_image3.BackColor = Color.Transparent;
                btn_score2_image1.BackColor = Color.Transparent;
                btn_score2_image2.BackColor = Color.Transparent;
                btn_score2_image3.BackColor = Color.Transparent;

                btn_score1_image1.Image = null;
                btn_score1_image2.Image = null;
                btn_score1_image3.Image = null;
                btn_score2_image1.Image = null;
                btn_score2_image2.Image = null;
                btn_score2_image3.Image = null;
            }
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var top8 = new frm_results();
            top8.Show();
        }

        private void rdb_automatic_CheckedChanged(object sender, EventArgs e)
        {
            global_values.auto_update = true;
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("etc").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", "true"));
            xml.Save(global_values.settings_file);
        }

        private void rdb_manual_CheckedChanged(object sender, EventArgs e)
        {
            global_values.auto_update = false;
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("etc").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", "false"));
            xml.Save(global_values.settings_file);
        }


        private async Task get_playlists()
        {
            playlist_items = new List<string>();
            playlist_names = new List<string>();
            UserCredential credential;
            using (var stream = new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.Youtubepartner,
                    YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubepartnerChannelAudit,
                    YouTubeService.Scope.YoutubeReadonly  },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // var channelsListRequest = youtubeService.Channels.List("contentDetails");
            var playlistListRequest = youtubeService.Playlists.List("snippet");
            playlistListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var playlistListResponse = await playlistListRequest.ExecuteAsync();

            foreach (var playlist in playlistListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var playlistListId = playlist.Id;
                playlist_items.Add(playlistListId);
                playlist_names.Add(playlist.Snippet.Title);
            }

            check_playlists();
        }

        private async Task add_playlist(string new_playlist)
        {
            UserCredential credential;
            using (var stream = new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.YoutubeUpload  },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = new_playlist;
            newPlaylist.Snippet.Description = "";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";

            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();
        }

        private void btn_playlist_Click(object sender, EventArgs e)
        {
            btn_playlist.BackColor = Color.Transparent;
            btn_playlist.Enabled = false;
            txt_playlist.Enabled = false;
            try
            {
                Thread thead = new Thread(() =>
                {
                    get_playlists().Wait();
                });
                thead.IsBackground = true;
                thead.Start();

            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        delegate void check_playlists_callback();

        private void check_playlists()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txt_playlist.InvokeRequired)
            {
                check_playlists_callback d = new check_playlists_callback(check_playlists);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (playlist_names.Contains(txt_playlist.Text))
                {
                    global_values.playlist_id = playlist_items[playlist_names.IndexOf(txt_playlist.Text)];
                    MessageBox.Show("The playlist has been set to " + txt_playlist.Text + ". \n" +
                                    "The playlist ID is " + global_values.playlist_id);
                    txt_playlist.Enabled = true;
                    global_values.playlist_name = txt_playlist.Text;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", global_values.playlist_name));
                    xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", global_values.playlist_id));
                    xml.Save(global_values.settings_file);
                }
                else
                {
                    if (txt_playlist.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        global_values.playlist_name = "";
                        global_values.playlist_id = "";

                        XDocument xml = XDocument.Load(global_values.settings_file);
                        xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", global_values.playlist_name));
                        xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", global_values.playlist_id));
                        xml.Save(global_values.settings_file);
                    }
                    else
                    {
                        if (MessageBox.Show("A playlist with the name '" + txt_playlist.Text + "' has not been found. Create a new playlist?", "No Playlist Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                Thread thead = new Thread(() =>
                                {
                                    add_playlist(txt_playlist.Text).ContinueWith(task => get_playlists());
                                });
                                thead.IsBackground = true;
                                thead.Start();

                            }
                            catch (AggregateException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            btn_playlist.Enabled = true;
                            txt_playlist.Enabled = true;
                        }
                    }
                }

            }
            
        }

        private void ckb_sheets_CheckedChanged(object sender, EventArgs e)
        {
            //Get the checked status of this checkbox
            bool status = ckb_sheets.Checked;

            ckb_region.Enabled = status;
            //Set the enable status of all sheets settings to the checked status
            txt_sheets.Enabled = status;
            btn_test_sheet.Enabled = status;

            //Enable/Disable the player save buttons accordingly
            btn_save1.Enabled = status;
            btn_save1.Visible = status;
            btn_save2.Visible = status;
            btn_save2.Enabled = status;

            if (status == false)
            {
                btn_previous_match.Enabled = false;
                btn_previous_match.Visible = false;
                cbx_format.Enabled = true;
            }

            //Update the global toggle and settings file
            global_values.enable_sheets = status;
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("google-sheets").Element("enable-sheets").ReplaceWith(new XElement("enable-sheets", status.ToString()));
            xml.Save(global_values.settings_file);
        }

        public void add_to_sheets(player_info new_player)
        {
            /*
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Define request parameters.
            String spreadsheetId = txt_sheets.Text;

            //Set the range to be only the player information
            string range = "Player Information!A2:O" + (MAX_PLAYERS+1).ToString();

            //Set up the request for the sheet
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

            //Receive the player information from the request
            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            //Place the information in an array
            IList<IList<Object>> player_information = response.Values;

            //Cycle through the player information until the new player's name is found, or the end of the list is found.
            int player_index = -1;                  //Intialize the index of the new player in the list
            for (int i = 0; i <= MAX_PLAYERS; i++)
            {
                if (player_information[i][0].ToString() == new_player.tag)
                {
                    player_index = i;               //Set the index to the current position
                    global_values.roster[i].sponsor = new_player.sponsor;
                    global_values.roster[i].twitter = new_player.twitter;
                    global_values.roster[i].character[0] = new_player.character[0];
                    global_values.roster[i].character[1] = new_player.character[1];
                    global_values.roster[i].character[2] = new_player.character[2];
                    global_values.roster[i].character[3] = new_player.character[3];
                    global_values.roster[i].character[4] = new_player.character[4];
                    global_values.roster[i].color[0] = new_player.color[0];
                    global_values.roster[i].color[1] = new_player.color[1];
                    global_values.roster[i].color[2] = new_player.color[2];
                    global_values.roster[i].color[3] = new_player.color[3];
                    global_values.roster[i].color[4] = new_player.color[4];
                    break;
                }
                if (player_information[i][0].ToString() == "")
                {
                    player_index = i;               //Set the index to the current position
                    global_values.roster[i] = new_player;
                    global_values.roster_size += 1;
                    break;
                }
            }

            

            //Add the new player's information to an array.
            var oblist = new List<object>() { global_values.roster[player_index].tag,
                                                global_values.roster[player_index].twitter,
                                                global_values.roster[player_index].region,
                                                global_values.roster[player_index].sponsor,
                                                global_values.roster[player_index].character[0],
                                                global_values.roster[player_index].character[1],
                                                global_values.roster[player_index].character[2],
                                                global_values.roster[player_index].character[3],
                                                global_values.roster[player_index].character[4],
                                                global_values.roster[player_index].color[0],
                                                global_values.roster[player_index].color[1],
                                                global_values.roster[player_index].color[2],
                                                global_values.roster[player_index].color[3],
                                                global_values.roster[player_index].color[4]};

            //Create a data set from the array
            Google.Apis.Sheets.v4.Data.ValueRange data = new Google.Apis.Sheets.v4.Data.ValueRange();
            data.Values = new List<IList<object>> { oblist };
            //Set the range's row to the player index"
            string range2 = "Player Information!A" + (player_index+2).ToString() + ":N" + (player_index + 2).ToString();
            data.MajorDimension = "ROWS";

            //Process the update
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(data, spreadsheetId, range2);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            Google.Apis.Sheets.v4.Data.UpdateValuesResponse result2 = update.Execute();
            


            MessageBox.Show("The following player information has been added to the database:" +
                                "\n Tag: " + global_values.roster[player_index].tag +
                                "\n Twitter: " + global_values.roster[player_index].twitter +
                                "\n Region: " + global_values.roster[player_index].region +
                                "\n Sponsor: " + global_values.roster[player_index].sponsor +
                                "\n Main: " + global_values.roster[player_index].character[0] +
                                "\n Color: " + global_values.roster[player_index].color[0]);
            */               
            
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "Master Orders Global Playerbase";
            dbCon.DatabaseUserName = global_values.database_username;
            dbCon.DatabasePassword = global_values.database_password;
            List<MySqlParameter> playerparams = new List<MySqlParameter>();            
            MySqlParameter add_player_param = new MySqlParameter("", "");
            string tablename = "`Super Smash Bros. Ultimate`";
            playerparams.Add(new MySqlParameter("@tag", new_player.tag));
            playerparams.Add(new MySqlParameter("@fullname", new_player.fullname));
            playerparams.Add(new MySqlParameter("@twitter", new_player.twitter));
            playerparams.Add(new MySqlParameter("@region", new_player.region));
            playerparams.Add(new MySqlParameter("@sponsor", new_player.fullsponsor));
            playerparams.Add(new MySqlParameter("@sponsorprefix", new_player.sponsor));
            playerparams.Add(new MySqlParameter("@character1", new_player.character[0]));
            playerparams.Add(new MySqlParameter("@character2", new_player.character[1]));
            playerparams.Add(new MySqlParameter("@character3", new_player.character[2]));
            playerparams.Add(new MySqlParameter("@character4", new_player.character[3]));
            playerparams.Add(new MySqlParameter("@character5", new_player.character[4]));
            playerparams.Add(new MySqlParameter("@color1", new_player.color[0]));
            playerparams.Add(new MySqlParameter("@color2", new_player.color[1]));
            playerparams.Add(new MySqlParameter("@color3", new_player.color[2]));
            playerparams.Add(new MySqlParameter("@color4", new_player.color[3]));
            playerparams.Add(new MySqlParameter("@color5", new_player.color[4]));

            dbCon.Insert("INSERT INTO " + tablename +" (`Tag`, `Full Name`, `Twitter`, `Region`, `Sponsor`, `Sponsor Prefix`, " +
                "`Character 1`, `Character 2`, `Character 3`, `Character 4`, " +
                "`Character 5`, `Color 1`, `Color 2`, `Color 3`, `Color 4`, `Color 5`) " +
                "VALUES(@tag, @fullname, @twitter, @region, @sponsor, @sponsorprefix, " +
                "@character1, @character2, @character3, @character4, @character5, " +
                "@color1, @color2, @color3, @color4, @color5)", playerparams );
            


            if (dbCon.IsConnect())
            {
                //suppose col0 and col1 are defined as VARCHAR in the DB
                string query = "SELECT `Tag`,`Full Name` FROM `Super Smash Bros. Ultimate`";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string someStringFromColumnZero = reader.GetString(0);
                    string someStringFromColumnOne = reader.GetString(1);
                    MessageBox.Show(someStringFromColumnZero + "," + someStringFromColumnOne);
                }
                dbCon.Close();
            }
            
        }

        private void import_from_sheets( bool reverse, int player_count )
        {
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Define request parameters.
            String spreadsheetId = txt_sheets.Text;

            List<string> ranges = new List<string>(new string[] { "Current Round Info!A1:D18", "Upcoming Matches!A1:G56", "Player Information!A2:O" + (MAX_PLAYERS + 1).ToString() });
            
            SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(spreadsheetId);
            request.Ranges = ranges;

            Google.Apis.Sheets.v4.Data.BatchGetValuesResponse response = request.Execute();
            IList<IList<Object>> current_round_info = response.ValueRanges[0].Values;
            IList<IList<Object>> upcoming_matches = response.ValueRanges[1].Values;
            IList<IList<Object>> player_information = response.ValueRanges[2].Values;

            //Populate the player roster with information from the player information sheet
            for (int i = 0; i <= MAX_PLAYERS; i++)
            {
                if(player_information[i][0].ToString() == "")
                {
                    global_values.roster_size = i-1;
                    break;
                }
                global_values.roster[i] = new player_info();
                global_values.roster[i].tag = player_information[i][0].ToString();
                global_values.roster[i].twitter = player_information[i][1].ToString();
                global_values.roster[i].region = player_information[i][2].ToString();
                global_values.roster[i].sponsor = player_information[i][3].ToString();
                for (int ii = 0; ii <= 4; ii++)
                {
                    global_values.roster[i].character[ii] = player_information[i][4+ii].ToString();
                }
                for (int ii = 0; ii <= 4; ii++)
                {
                    if (player_information[i][9 + ii] != null && player_information[i][9 + ii] != "")
                    {
                        string add_color = player_information[i][9 + ii].ToString();
                        global_values.roster[i].color[ii] = Int32.Parse(add_color);
                    }
                    else
                    {
                        global_values.roster[i].color[ii] = 1;
                    }
                }
            }

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            string manual_update = upcoming_matches[1][3].ToString();
            if ((manual_update == "" || manual_update == null))
            {
                if (global_values.first_match == false)
                {
                    if (reverse == true)
                    {
                        round_number -= 1;

                    }
                    else
                    {
                        round_number += 1;
                    }
                }
            }
            else
            {
                round_number = Int32.Parse(manual_update);
            }

            if(round_number == 0)
            {
                round_number = 1;
            }

            global_values.first_match = false;


            string[] player_name = new string[9];
            player_info[] player = new player_info[9];

            switch(player_count)
            {
                case 2:
                    player_name[1] = upcoming_matches[3 + round_number][2].ToString();
                    player_name[2] = upcoming_matches[3 + round_number][3].ToString();
                    player_name[3] = upcoming_matches[4 + round_number][2].ToString();
                    player_name[4] = upcoming_matches[4 + round_number][3].ToString();
                    break;
                case 4:
                    player_name[1] = upcoming_matches[3 + round_number][2].ToString();
                    player_name[2] = upcoming_matches[3 + round_number][4].ToString();
                    player_name[3] = upcoming_matches[3 + round_number][3].ToString();
                    player_name[4] = upcoming_matches[3 + round_number][5].ToString();

                    player_name[5] = upcoming_matches[4 + round_number][2].ToString();
                    player_name[6] = upcoming_matches[4 + round_number][4].ToString();
                    player_name[7] = upcoming_matches[4 + round_number][3].ToString();
                    player_name[8] = upcoming_matches[4 + round_number][5].ToString();
                    break;
            }




            for (int player_number = 1; player_number <= player_count*2; player_number++)
            {
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    if(player_name[player_number] == "")
                    {
                        player[player_number] = new player_info();
                        player[player_number].tag = "";
                        player[player_number].twitter = "";
                        player[player_number].region = "";
                        player[player_number].sponsor = "";
                        for (int ii = 0; ii <= 4; ii++)
                        {
                            player[player_number].character[ii] = "";
                            player[player_number].color[ii] = 1;
                        }
                        switch (player_count)
                        {
                            case 2:
                                switch (player_number)
                                {
                                    case 1:
                                        txt_alt1.Text = "";
                                        update_characters(ref cbx_characters1);
                                        break;
                                    case 2:
                                        txt_alt2.Text = "";
                                        update_characters(ref cbx_characters2);
                                        break;
                                }
                                break;
                            case 4:
                                switch (player_number)
                                {
                                    case 1:
                                        txt_team1_twitter1.Text = "";
                                        update_characters(ref cbx_team1_character1);
                                        break;
                                    case 2:
                                        txt_team2_twitter1.Text = "";
                                        update_characters(ref cbx_team2_character1);
                                        break;
                                    case 3:
                                        txt_team1_twitter2.Text = "";
                                        update_characters(ref cbx_team1_character2);
                                        break;
                                    case 4:
                                        txt_team2_twitter2.Text = "";
                                        update_characters(ref cbx_team2_character2);
                                        break;
                                }
                                break;
                        }
                        break;
                    }
                    if (global_values.roster[i].tag == player_name[player_number])
                    {
                        player[player_number] = global_values.roster[i];
                        break;
                    }
                }
            }

            Google.Apis.Sheets.v4.Data.ValueRange currentmatch = new Google.Apis.Sheets.v4.Data.ValueRange();

            switch (player_count)
            {
                case 2:
                    update_names(ref cbx_name1);
                    cbx_name1.SelectedIndex = cbx_name1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0
                    cbx_name1.Text = player[1].get_display_name();

                    update_names(ref cbx_name2);
                    cbx_name2.SelectedIndex = cbx_name2.Items.IndexOf(player[2].tag);   //Set the combobox index to 0
                    cbx_name2.Text = player[2].get_display_name();

                    cbx_round.Text = upcoming_matches[3 + round_number][1].ToString();

                    cbx_colors1.SelectedIndex = player[1].color[0] - 1;
                    cbx_colors2.SelectedIndex = player[2].color[0] - 1;



                    var oblist = new List<object>() { "Tournament Name", "Bracket URL", ".", "Current Match", "P1",  "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", "Next Match", "P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist2 = new List<object>() { "", "", ".", "", "", player[1].tag, player[1].twitter, player[1].region, player[1].sponsor, player[1].character[0] ,
                                                "", "", "", player[3].tag, player[3].twitter, player[3].region, player[3].sponsor, player[3].character[0]};
                    var oblist3 = new List<object>() { txt_tournament.Text, txt_bracket.Text, "", cbx_round.Text, "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", upcoming_matches[5 + round_number][1].ToString(), "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist4 = new List<object>() { "", "", ".", "", "", player[2].tag, player[2].twitter, player[2].region, player[2].sponsor, player[2].character[0] ,
                                                "", "", "", player[4].tag, player[4].twitter, player[4].region, player[4].sponsor, player[4].character[0] };

                    // The new values to apply to the spreadsheet.
                    currentmatch.Values = new List<IList<object>> { oblist, oblist2, oblist3, oblist4 };
                    currentmatch.Range = "Current Round Info!A1:D18";
                    currentmatch.MajorDimension = "COLUMNS";
                    break;
                case 4:
                    update_names(ref cbx_team1_name1);
                    cbx_team1_name1.SelectedIndex = cbx_team1_name1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0
                    cbx_team1_name1.Text = player[1].get_display_name();

                    update_names(ref cbx_team2_name1);
                    cbx_team2_name1.SelectedIndex = cbx_team2_name1.Items.IndexOf(player[2].tag);   //Set the combobox index to 0
                    cbx_team2_name1.Text = player[2].get_display_name();

                    update_names(ref cbx_team1_name2);
                    cbx_team1_name2.SelectedIndex = cbx_team1_name2.Items.IndexOf(player[3].tag);   //Set the combobox index to 0
                    cbx_team1_name2.Text = player[3].get_display_name();

                    update_names(ref cbx_team2_name2);
                    cbx_team2_name2.SelectedIndex = cbx_team2_name2.Items.IndexOf(player[4].tag);   //Set the combobox index to 0
                    cbx_team2_name2.Text = player[4].get_display_name();


                    cbx_team_round.Text = upcoming_matches[3 + round_number][1].ToString();

                    cbx_team1_color1.SelectedIndex = player[1].color[0] - 1;
                    cbx_team2_color1.SelectedIndex = player[2].color[0] - 1;
                    cbx_team1_color2.SelectedIndex = player[3].color[0] - 1;
                    cbx_team2_color2.SelectedIndex = player[4].color[0] - 1;



                    var oblist11 = new List<object>() { "Tournament Name", "Bracket URL", ".", "Current Match", "TEAM1 P1",  "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", "Next Match", "TEAM1 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist22 = new List<object>() { "", "", ".", "", "", player[1].tag, player[1].twitter, player[1].region, player[1].sponsor, player[1].character[0] ,
                                                "", "", "", player[5].tag, player[5].twitter, player[5].region, player[5].sponsor, player[5].character[0]};
                    var oblist33 = new List<object>() { txt_tournament.Text, txt_bracket.Text, "", cbx_team_round.Text, "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", upcoming_matches[5 + round_number][1].ToString(), "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist44 = new List<object>() { "", "", ".", "", "", player[3].tag, player[3].twitter, player[3].region, player[3].sponsor, player[3].character[0] ,
                                                "", "", "", player[7].tag, player[7].twitter, player[7].region, player[7].sponsor, player[7].character[0] };
                    var oblist55 = new List<object>() { "", "", "", "", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", ".", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist66 = new List<object>() { "", "", ".", "", "", player[2].tag, player[2].twitter, player[2].region, player[2].sponsor, player[2].character[0] ,
                                                "", "", "", player[6].tag, player[6].twitter, player[6].region, player[6].sponsor, player[6].character[0] };
                    var oblist77 = new List<object>() { "", "", "", "", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", ".", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                    var oblist88 = new List<object>() { "", "", ".", "", "", player[4].tag, player[4].twitter, player[4].region, player[4].sponsor, player[4].character[0] ,
                                                "", "", "", player[8].tag, player[8].twitter, player[8].region, player[8].sponsor, player[8].character[0] };

                    // The new values to apply to the spreadsheet.
                    currentmatch.Values = new List<IList<object>> { oblist11, oblist22, oblist33, oblist44, oblist55, oblist66, oblist77, oblist88 };
                    currentmatch.Range = "Current Round Info!A1:H18";
                    currentmatch.MajorDimension = "COLUMNS";
                    break;
            }





            var oblist5 = new List<object>() { (round_number).ToString(), "Next Match to Stream", "" };

            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Range = "Upcoming Matches!B2:D2";
            upcoming.Values = new List<IList<object>> { oblist5 }; ;
            upcoming.MajorDimension = "ROWS";

            List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>() { currentmatch , upcoming };  // TODO: Update placeholder value.

            // TODO: Assign values to desired properties of `requestBody`:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "RAW";
            requestBody.Data = data;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = service.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();



            if (global_values.auto_update == true)
            {
                //Save Player 1's information to seperate files to be used by the stream program
                string output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                //Save Player 2's information to seperate files to be used by the stream program
                output_name = get_output_name(cbx_name2.Text, ckb_loser2.Checked, 2);
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
                System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", cbx_characters2.Text);
                //Save the Tournament information to seperate files to be used by the stream program
                System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
            }
        }

        private void info_from_sheets()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Define request parameters.
            String spreadsheetId = txt_sheets.Text;
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId,
                "Player Information!A2:O" + (MAX_PLAYERS + 1).ToString());
            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();

            IList<IList<Object>> player_information = response.Values;

            //Populate the player roster with information from the player information sheet
            for (int i = 0; i <= MAX_PLAYERS; i++)
            {
                if (player_information[i][0].ToString() == "")
                {
                    global_values.roster_size = i - 1;
                    break;
                }
                global_values.roster[i] = new player_info();
                global_values.roster[i].tag = player_information[i][0].ToString();
                global_values.roster[i].twitter = player_information[i][1].ToString();
                global_values.roster[i].region = player_information[i][2].ToString();
                global_values.roster[i].sponsor = player_information[i][3].ToString();
                for (int ii = 0; ii <= 4; ii++)
                {
                    global_values.roster[i].character[ii] = player_information[i][4 + ii].ToString();
                }
                for (int ii = 0; ii <= 4; ii++)
                {
                    if (player_information[i][9 + ii] != null && player_information[i][9 + ii] != "")
                    {
                        string add_color = player_information[i][9 + ii].ToString();
                        global_values.roster[i].color[ii] = Int32.Parse(add_color);
                    }
                    else
                    {
                        global_values.roster[i].color[ii] = 1;
                    }
                }
            }

            update_names(ref cbx_name1);
            update_names(ref cbx_name2);
            update_names(ref cbx_team1_name1);
            update_names(ref cbx_team1_name2);
            update_names(ref cbx_team2_name1);
            update_names(ref cbx_team2_name2);
        }

        private void check_sheets()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Define request parameters.
            String spreadsheetId = txt_sheets.Text;
            SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(spreadsheetId);
            Google.Apis.Sheets.v4.Data.Spreadsheet response;
            try
            {
                response = request.Execute();
            }
            catch (Exception ex)
            {
                MessageBox.Show("The Entered Sheet ID is not valid! Please ensure that the ID is correct and the provided Google Username has viewing and editing rights.");
                txt_sheets.BackColor = warning_color;
                return;
            }
            //Values.Get(spreadsheetId,
            //"Player Information!A2:O" + (MAX_PLAYERS + 1).ToString());

            IList<Google.Apis.Sheets.v4.Data.Sheet> sheet_information = response.Sheets;

            btn_previous_match.Enabled = false;
            btn_previous_match.Visible = false;
            cbx_format.Enabled = true;

            int sheet_number = sheet_information.Count;
            switch(sheet_number)
            {
                case 5:
                    if (sheet_information[0].Properties.Title == "Current Round Info" && 
                        sheet_information[1].Properties.Title == "Upcoming Matches" &&
                        sheet_information[2].Properties.Title == "Player Information" &&
                        sheet_information[3].Properties.Title == "Characters and Rounds")
                    {
                        switch(sheet_information[4].Properties.Title)
                        {
                            case "Singles":
                                MessageBox.Show("The designated Google Sheet contains the following information:\n\n" +
                                                "Player Database\n" +
                                                "Stream Queue for Singles\n\n" +
                                                "Master Orders will use adapt to its information.");
                                cbx_format.Text = "Singles";
                                break;
                            case "Doubles":
                                MessageBox.Show("The designated Google Sheet contains the following information:\n\n" +
                                                "Player Database\n" +
                                                "Stream Queue for Doubles\n\n" +
                                                "Master Orders will use adapt to its information.");
                                cbx_format.Text = "Doubles";
                                break;
                            default:
                                MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
                                txt_sheets.Text = "";
                                return;
                        }
                        cbx_format.Enabled = false;
                        rdb_fullsheet.Checked = true;
                        rdb_fullsheet.Enabled = true;
                        rdb_infoonly.Enabled = true;
                        btn_previous_match.Enabled = true;
                        btn_previous_match.Visible = true;
                        global_values.sheets_style = "info-and-queue";
                        XDocument xml = XDocument.Load(global_values.settings_file);
                        xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                        xml.Root.Element("google-sheets").Element("sheet-style").ReplaceWith(new XElement("sheet-style", global_values.sheets_style));
                        xml.Save(global_values.settings_file);
                        info_from_sheets();
                    }
                    else
                    {
                        MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
                        txt_sheets.Text = "";
                    }
                    break;
                case 2:
                    if (sheet_information[0].Properties.Title == "Player Information" &&
                        sheet_information[1].Properties.Title == "Characters")
                    {
                        rdb_infoonly.Checked = true;
                        rdb_fullsheet.Enabled = false;
                        MessageBox.Show("The designated Google Sheet contains only player information. " +
                                        "Master Orders will use adapt to its information.");
                        global_values.sheets_style = "info-only";
                        XDocument xml = XDocument.Load(global_values.settings_file);
                        xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                        xml.Root.Element("google-sheets").Element("sheet-style").ReplaceWith(new XElement("sheet-style", global_values.sheets_style));
                        xml.Save(global_values.settings_file);
                        info_from_sheets();
                    }
                    else
                    {
                        MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
                        txt_sheets.Text = "";
                    }
                    break;
                default:
                    MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
                    txt_sheets.Text = "";
                    break;
            }
        }

        private void cbx_characters1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_colors1.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_characters1.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for(int i = 0; i < colors_count; i++)
            {
                colors[i] = Image.FromFile(character_path + @"\" + (i+1).ToString() + @"\stamp.png");
            }
            cbx_colors1.DisplayImages(colors);
            cbx_colors1.SelectedIndex = 0;

            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[1]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_characters1.Text)
                    {
                        cbx_colors1.SelectedIndex = check_player.color[i]-1;
                        break;
                    }
                }
            }
            cbx_colors1.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_update.Text == "Update")
            {
                update_uldata(1, global_values.current_youtube_data);
            }
        }

        private void cbx_characters2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_colors2.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_characters2.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = Image.FromFile(character_path + @"\" + (i + 1).ToString() + @"\stamp.png");
            }
            cbx_colors2.DisplayImages(colors);
            cbx_colors2.SelectedIndex = 0;
            if (global_values.player_roster_number[2] != -1)
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[2]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_characters2.Text)
                    {
                        cbx_colors2.SelectedIndex = check_player.color[i]-1;
                        break;
                    }
                }
            }
            cbx_colors2.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_update.Text == "Update")
            {
                update_uldata(2, global_values.current_youtube_data);
            }
        }

        private void cbx_colors1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = global_values.game_path + @"\" + cbx_characters1.Text + @"\" + (cbx_colors1.SelectedIndex+1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory1 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 1.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_update.Text == "Update")
            {
                update_uldata(1, global_values.current_youtube_data);
            }
        }

        private void cbx_colors2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = global_values.game_path + @"\" + cbx_characters2.Text + @"\" + (cbx_colors2.SelectedIndex+1).ToString() + @"\";
            Image stock_icon2 = Image.FromFile(image_directory2 + @"\stock.png");
            stock_icon2.Save(global_values.output_directory + @"\Stock Icon 2.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_update.Text == "Update")
            {
                update_uldata(2, global_values.current_youtube_data);
            }
        }

        private void txt_sheets_TextChanged(object sender, EventArgs e)
        {
            if (txt_sheets.Text != @"")
            {
                btn_test_sheet.Enabled = true;
            }
            else
            {
                btn_test_sheet.BackColor = Color.Transparent;
                btn_test_sheet.Enabled = false;
            }
        }

        private void txt_vods_TextChanged(object sender, EventArgs e)
        {
            if (txt_vods.Text != @"")
            {
                if (Directory.Exists(txt_vods.Text) && 
                    txt_vods.Text != txt_roster_directory.Text && 
                    txt_vods.Text != txt_stream_directory.Text &&
                    txt_vods.Text != txt_thumbnail_directory.Text)
                {
                    txt_vods.BackColor = Color.White;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("directories").Element("vods-directory").ReplaceWith(new XElement("vods-directory", txt_vods.Text));
                    xml.Save(global_values.settings_file);

                    global_values.vods_directory = txt_vods.Text;
                    global_values.vod_monitor.Path = global_values.vods_directory;
                    global_values.vod_monitor.EnableRaisingEvents = true;                       //Enable to monitor to trigger these events

                }
                else
                {
                    txt_vods.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                }
            }
            else
            {
                txt_vods.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }
        }

        private void FileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            if (global_values.enable_youtube == true)
            {
                if (Path.GetExtension(e.Name) == @".mp4")
                {
                    global_values.new_vod_detected = e.Name;
                    if (File.Exists(global_values.current_youtube_data))
                    {
                        update_uldata(1, global_values.current_youtube_data);
                        update_uldata(2, global_values.current_youtube_data);
                        update_uldata(3, global_values.current_youtube_data);
                        update_uldata(4, global_values.current_youtube_data);
                        update_uldata(5, global_values.current_youtube_data);
                    }
                    btn_upload.BeginInvoke((Action)delegate ()
                    {
                        if (global_values.stream_software == "XSplit")
                        {
                            global_values.allow_upload = false;
                            btn_upload.Text = "Recording in Progress";
                            btn_upload.Enabled = false;
                        }
                    });
                }
                else
                {
                    global_values.temp_file = e.Name;

                }
            }
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            /*
            if (global_values.enable_youtube == true)
            {
                if (e.OldName == global_values.new_vod_detected)
                {
                    global_values.allow_upload = true;
                    btn_upload.BeginInvoke((Action)delegate ()
                    {
                        btn_upload.Text = "Upload to YouTube";
                        btn_upload.Enabled = true;
                    });
                }
            }
            */
        }

        private void FileSystemWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            if (global_values.enable_youtube == true)
            {
                if (e.OldName == global_values.new_vod_detected)
                {
                    global_values.new_vod_detected = e.Name;
                    global_values.allow_upload = true;
                    btn_upload.Invoke((Action)delegate ()
                    {
                        btn_upload.Text = "Upload to YouTube";
                        btn_upload.Enabled = true;
                    });
                }
            }
        }
        private void btn_vods_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_vods.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            if (global_values.enable_youtube == false)
            {
                string thumbnail_image_name = create_thumbnail(2,
                        cbx_name1.Text,
                        cbx_name2.Text,
                        cbx_round.Text,
                        txt_date.Text);
                string video_title = txt_tournament.Text + @" - " + cbx_round.Text + @" - " + cbx_name1.Text + @" (" + cbx_characters1.Text + @") Vs. " + cbx_name2.Text + @" (" + cbx_characters2.Text + @")";
                if (global_values.copy_video_title == true)
                {
                    Clipboard.SetText(video_title);
                    MessageBox.Show("A thumbnail image has been generated. \r\nVideo title copied to clipboard: \n" + video_title);
                }
                else
                {
                    MessageBox.Show("A thumbnail image has been generated.");
                }
            }
            else
            {
                //Create a thumbnail image and save its name
                string thumbnail_image_name = create_thumbnail(2,
                    cbx_name1.Text,
                    cbx_name2.Text,
                    cbx_round.Text,
                    txt_date.Text);

                //Pass the event upload_form_enable_button_event() to the new form as the function "enable_button()"
                btn_upload.Text = "Upload Window Open";
                btn_upload.Enabled = false;             //Disable this button until further action is needed
                global_values.reenable_upload = DateTime.Now.ToString("MMddyyyyHHmmss");   //Set the flag to allow the button to be re-abled on form close

                string video_title = txt_tournament.Text + @" - " + cbx_round.Text + @" - " + cbx_name1.Text + @" (" + cbx_characters1.Text + @") Vs. " + cbx_name2.Text + @" (" + cbx_characters2.Text + @")";
                if (global_values.copy_video_title == true)
                {
                    Clipboard.SetText(video_title);
                    MessageBox.Show("Video title copied to clipboard: \n" + video_title);
                }

                string video_description = get_video_description();


                //Create a new form and provide it with a Video title based off the provided information,
                //as well as a description and the thumbnail image created
                var upload_form = new frm_uploading(video_title,
                    video_description,
                    global_values.thumbnail_directory + @"\" + thumbnail_image_name,
                    global_values.vods_directory + @"\" + global_values.new_vod_detected,
                    global_values.reenable_upload, false);
                upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                upload_form.Show();                     //Show the form        
            }
        }

        private string get_video_description()
        {
            string description = txt_description.Text.Replace("INFO_TOURNAMENT", txt_tournament.Text);
            switch (cbx_format.Text)
            {
                case "Singles":
                    description = description.Replace("INFO_DATE", txt_date.Text);
                    description = description.Replace("INFO_ROUND", cbx_round.Text);
                    description = description.Replace("INFO_DATE", txt_date.Text);
                    description = description.Replace("INFO_ROUND", cbx_round.Text);
                    description = description.Replace("INFO_BRACKET", txt_bracket.Text);
                    description = description.Replace("INFO_PLAYER1", cbx_name1.Text);
                    description = description.Replace("INFO_PLAYER2", cbx_name2.Text);
                    description = description.Replace("INFO_TWITTER1", txt_alt1.Text);
                    description = description.Replace("INFO_TWITTER2", txt_alt2.Text);
                    description = description.Replace("INFO_CHARACTER1", cbx_characters1.Text);
                    description = description.Replace("INFO_CHARACTER2", cbx_characters2.Text);
                    break;
                case "Doubles":
                    description = description.Replace("INFO_DATE", txt_date.Text);
                    description = description.Replace("INFO_ROUND", cbx_round.Text);
                    description = description.Replace("INFO_DATE", txt_date.Text);
                    description = description.Replace("INFO_ROUND", cbx_round.Text);
                    description = description.Replace("INFO_BRACKET", txt_bracket.Text);
                    description = description.Replace("INFO_PLAYER1", cbx_team1_name1.Text);
                    description = description.Replace("INFO_PLAYER2", cbx_team2_name1.Text);
                    description = description.Replace("INFO_TWITTER1", txt_team1_twitter1.Text);
                    description = description.Replace("INFO_TWITTER2", txt_team2_twitter1.Text);
                    description = description.Replace("INFO_CHARACTER1", cbx_team1_character1.Text);
                    description = description.Replace("INFO_CHARACTER2", cbx_team2_character1.Text);
                    description = description.Replace("INFO_PLAYER3", cbx_team1_name2.Text);
                    description = description.Replace("INFO_PLAYER4", cbx_team2_name2.Text);
                    description = description.Replace("INFO_TWITTER3", txt_team1_twitter2.Text);
                    description = description.Replace("INFO_TWITTER4", txt_team2_twitter2.Text);
                    description = description.Replace("INFO_CHARACTER3", cbx_team1_character2.Text);
                    description = description.Replace("INFO_CHARACTER4", cbx_team2_character2.Text);
                    break;
            }
            return description;

        }

        private void btn_upload_vod_Click(object sender, EventArgs e)
        {
            XDocument doc = new XDocument(
                new XElement("YouTube-Upload-Data",
                new XElement("Player-Information",
                        new XElement("Player-1",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-2",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-3",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-4",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", ""))
                        ),
                new XElement("Match-Information",
                        new XElement("Game", global_values.game_info[0]),
                        new XElement("Format", cbx_format.Text),
                        new XElement("Round", cbx_round.Text),
                        new XElement("Bracket-URL", txt_bracket.Text),
                        new XElement("Tournament-Name", txt_tournament.Text),
                        new XElement("Date", txt_date.Text),
                        new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected))
                ));
            openFileDialog2.InitialDirectory = global_values.thumbnail_directory;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //Load the settings file data
                XDocument xml = XDocument.Load(openFileDialog2.FileName);

                string game = (string)xml.Root.Element("Match-Information").Element("Game");
                if(game != global_values.game_info[0])
                {
                    MessageBox.Show("This ULData file is not associated with the game that Master Orders is currently configured to! To use this ULdata, go to the Settings tab and update the Character Roster Directory to the following game: \n" + game);
                    return;
                }
                string format = (string)xml.Root.Element("Match-Information").Element("Format");
                string round = (string)xml.Root.Element("Match-Information").Element("Round");
                string bracket_url = (string)xml.Root.Element("Match-Information").Element("Bracket-URL");
                string tournament_name = (string)xml.Root.Element("Match-Information").Element("Tournament-Name");
                string date = (string)xml.Root.Element("Match-Information").Element("Date");
                string vod_file = (string)xml.Root.Element("Match-Information").Element("VoD-File");

                string player1_name = (string)xml.Root.Element("Player-Information").Element("Player-1").Element("Name");
                string player1_twitter = (string)xml.Root.Element("Player-Information").Element("Player-1").Element("Twitter");
                string player1_character = (string)xml.Root.Element("Player-Information").Element("Player-1").Element("Character");
                int player1_color = (int)xml.Root.Element("Player-Information").Element("Player-1").Element("Color");
                string player2_name = (string)xml.Root.Element("Player-Information").Element("Player-2").Element("Name");
                string player2_twitter = (string)xml.Root.Element("Player-Information").Element("Player-2").Element("Twitter");
                string player2_character = (string)xml.Root.Element("Player-Information").Element("Player-2").Element("Character");
                int player2_color = (int)xml.Root.Element("Player-Information").Element("Player-2").Element("Color");

                switch (format)
                {
                    case "Singles":
                        string thumbnail_image_name = create_thumbnail(2,
                            player1_name,
                            player2_name,
                            round,
                            date);
                        var upload_form = new frm_uploading(tournament_name + @" - " + round + @" - " + player1_name + @" (" + player1_character + @") Vs. " + player2_name + @" (" + player2_character + @")",
                            get_video_description(),
                            global_values.thumbnail_directory + @"\" + thumbnail_image_name,
                            vod_file,
                            "0", true);
                        upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                        upload_form.Show();                     //Show the form 
                        break;
                    case "Doubles":
                        string player3_name = (string)xml.Root.Element("Player-Information").Element("Player-3").Element("Name");
                        string player3_twitter = (string)xml.Root.Element("Player-Information").Element("Player-3").Element("Twitter");
                        string player3_character = (string)xml.Root.Element("Player-Information").Element("Player-3").Element("Character");
                        int player3_color = (int)xml.Root.Element("Player-Information").Element("Player-3").Element("Color");
                        string player4_name = (string)xml.Root.Element("Player-Information").Element("Player-4").Element("Name");
                        string player4_twitter = (string)xml.Root.Element("Player-Information").Element("Player-4").Element("Twitter");
                        string player4_character = (string)xml.Root.Element("Player-Information").Element("Player-4").Element("Character");
                        int player4_color = (int)xml.Root.Element("Player-Information").Element("Player-4").Element("Color");

                        string team_name1 = player1_name + " + " + player3_name;
                        string team_name2 = player2_name + " + " + player4_name;

                        string thumbnail_team_name = create_thumbnail(4,
                            team_name1,
                            team_name2,
                            round,
                            date);
                        var team_form = new frm_uploading(tournament_name + @" - " + round + @" - " + team_name1 + @" (" + player1_character + " + " + player3_character + @") Vs. " + team_name2 + @" (" + player2_character + " + " + player4_character + @")",
                            get_video_description(),
                            global_values.thumbnail_directory + @"\" + thumbnail_team_name,
                            vod_file,
                            "0", true);
                        team_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                        team_form.Show();                     //Show the form 
                        break;
                }



       
            }
        }

        private void rdb_xsplit_CheckedChanged(object sender, EventArgs e)
        {
            global_values.stream_software = @"XSplit";
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("etc").Element("stream-software").ReplaceWith(new XElement("stream-software", "XSplit"));
            xml.Save(global_values.settings_file);
        }

        private void btn_previous_match_Click(object sender, EventArgs e)
        {
            if(global_values.enable_sheets == true)
            {
                import_from_sheets(true, 2);
            }
        }

        private void rdb_obs_CheckedChanged(object sender, EventArgs e)
        {
            global_values.stream_software = @"OBS";
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("etc").Element("stream-software").ReplaceWith(new XElement("stream-software", "OBS"));
            xml.Save(global_values.settings_file);
        }

        private void cbx_name1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i <= global_values.roster_size; i++)
            {
                if(global_values.roster[i].tag == cbx_name1.Text)
                {
                    update_check = false;
                    global_values.player_roster_number[1] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_name1.Text = global_values.roster[i].get_display_name(); });
                    txt_alt1.Text = global_values.roster[i].twitter;

                    cbx_characters1.BeginUpdate();                                      //Begin
                    cbx_characters1.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_characters1.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters1.EndUpdate();                                        //End
                    cbx_characters1.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_colors1.SelectedIndex = global_values.roster[i].color[0] - 1;
                    update_check = true;
                    if (global_values.auto_update == true && btn_update.Text == "Update")
                    {
                        update_uldata(1, global_values.current_youtube_data);
                    }
                    return;
                }
            }
        }

        private void cbx_name2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == cbx_name2.Text)
                {
                    update_check = false;
                    global_values.player_roster_number[2] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_name2.Text = global_values.roster[i].get_display_name(); });
                    txt_alt2.Text = global_values.roster[i].twitter;

                    cbx_characters2.BeginUpdate();                                      //Begin
                    cbx_characters2.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_characters2.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters2.EndUpdate();                                        //End
                    cbx_characters2.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_colors2.SelectedIndex = global_values.roster[i].color[0] - 1;
                    update_check = true;
                    if (global_values.auto_update == true && btn_update.Text == "Update")
                    {
                        update_uldata(2, global_values.current_youtube_data);
                    }
                    return;
                }
            }
        }

        private void check_for_sponsor(ref string player_name, ref string player_team)
        {
            //Set the list of possible seperators between sponsor and tag
            string[] check_seperators = { " | ", " / ", @" \ " };
            foreach (string element in check_seperators)
            {
                //Check if the tag contains the seperator
                if (player_name.Contains(element))
                {
                    //Initialize the sponsor and tag checking variables
                    string check_team = player_name;
                    string check_name = player_name;

                    //Check each index of the tag string
                    for (int i = 0; i < check_team.Length; i++)
                    {
                        //Check if the seperator is present at this index
                        if (check_team.Substring(i).StartsWith(element) == true)
                        {
                            //Set the sponsor to be before this index
                            check_team = player_name.Substring(0, i);
                            //Set the tag to be after the seperator at this index
                            check_name = player_name.Substring(i + 3);

                            //Verify that this is a seperator
                            //if (MessageBox.Show("Does this player have the sponsor '" + check_team + "'?",
                            //    "Sponsor Name Detected", MessageBoxButtons.YesNo)
                            //    == DialogResult.Yes)
                            //{
                                //Pass the sponsor and tag onto the actual variables
                                player_team = check_team;
                                player_name = check_name;
                            //}
                            //Stop checking for the seperator
                            return;
                        }
                    }
                }
            }
        }

        private void btn_save1_Click(object sender, EventArgs e)
        {
            //Create a player profile and set its tag and twitter to the enterred information
            player_info save_player = new player_info();
            save_player.tag = cbx_name1.Text;
            save_player.twitter = txt_alt1.Text;
            save_player.region = "";
            for (int i = 1; i < 5; i++)
            {
                save_player.character[i] = "";
                save_player.color[i] = 1;
            }

            //This variable controls the slot that the enterred character gets input into.
            int overwrite_slot = 0;
            //Test to see if a seperator is present in the tag and seperate the tag and sponsor appropriately
            check_for_sponsor(ref save_player.tag, ref save_player.sponsor);



            //Check if a player in the roster has been selected from the combobox. 
            //Also ensure that the sheets integration is enabled
            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                if (global_values.roster[global_values.player_roster_number[1]].tag == save_player.tag)
                {
                    //store the roster player's info locally
                    player_info grab_info = global_values.roster[global_values.player_roster_number[1]];
                    //Set the player profile's characters, colors, and region to that of the roster player
                    save_player.character = grab_info.character;
                    save_player.color = grab_info.color;
                    save_player.region = grab_info.region;
                    //Check if the selected character is one of the player profile's characters. If not...
                    if (!save_player.character.Contains(cbx_characters1.Text))
                    {
                        //Loop through each character slot of the player
                        for (int i = 0; i < 5; i++)
                        {
                            //If there is no character in the slot, set the slot as the overwrite slot and break the loop
                            if (save_player.character[i] == "")
                            {
                                overwrite_slot = i;
                                break;
                            }
                            else
                            {
                                //If no slot is empty...
                                if (i == 4)
                                {
                                    //Show the window to select an overwrite slot
                                    var check_character = new frm_replace_character(save_player, cbx_characters1.Text);
                                    check_character.ShowDialog();
                                    overwrite_slot = get_character_slot;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Set the overwrite slot to the slot that contains it.
                        overwrite_slot = Array.IndexOf(save_player.character, cbx_characters1.Text);
                    }
                }
            }

            if (overwrite_slot != -1)
            {
                save_player.character[overwrite_slot] = cbx_characters1.Text;
                save_player.color[overwrite_slot] = cbx_colors1.SelectedIndex + 1;
            }

            var player_info_box = new frm_save_player(save_player);
            if(player_info_box.ShowDialog() == DialogResult.OK)
            {
                add_to_sheets(get_new_player);
            
                int hold_index = cbx_name2.SelectedIndex;

                cbx_name1.BeginUpdate();                                            //Begin
                cbx_name1.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name1.Items.Add(global_values.roster[i].tag);
                }
                cbx_name1.EndUpdate();                                              //End
                cbx_name1.SelectedIndex = cbx_name1.Items.IndexOf(get_new_player.tag);     //Set the combobox index to 0

                cbx_name2.BeginUpdate();                                            //Begin
                cbx_name2.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name2.Items.Add(global_values.roster[i].tag);
                }
                cbx_name2.EndUpdate();                                              //End
                cbx_name2.SelectedIndex = hold_index;   //Set the combobox index to 0
            }
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            //Create a player profile and set its tag and twitter to the enterred information
            player_info save_player = new player_info();
            save_player.tag = cbx_name2.Text;
            save_player.twitter = txt_alt2.Text;
            save_player.region = "";
            for (int i = 1; i < 5; i++)
            {
                save_player.character[i] = "";
                save_player.color[i] = 1;
            }

            //This variable controls the slot that the enterred character gets input into.
            int overwrite_slot = 0;


            //Test to see if each seperator is present in the tag and seperate the tag and sponsor appropriately
            check_for_sponsor(ref save_player.tag, ref save_player.sponsor);



            //Check if a player in the roster has been selected from the combobox. 
            //Also ensure that the sheets integration is enabled
            if (global_values.player_roster_number[2] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                if (global_values.roster[global_values.player_roster_number[2]].tag == save_player.tag)
                {
                    //store the roster player's info locally
                    player_info grab_info = global_values.roster[global_values.player_roster_number[2]];
                    //Set the player profile's characters, colors, and region to that of the roster player
                    save_player.character = grab_info.character;
                    save_player.color = grab_info.color;
                    save_player.region = grab_info.region;
                    //Check if the selected character is one of the player profile's characters. If not...
                    if (!save_player.character.Contains(cbx_characters2.Text))
                    {
                        //Loop through each character slot of the player
                        for (int i = 0; i < 5; i++)
                        {
                            //If there is no character in the slot, set the slot as the overwrite slot and break the loop
                            if (save_player.character[i] == "")
                            {
                                overwrite_slot = i;
                                break;
                            }
                            else
                            {
                                //If no slot is empty...
                                if (i == 4)
                                {
                                    //Show the window to select an overwrite slot
                                    var check_character = new frm_replace_character(save_player, cbx_characters2.Text);
                                    check_character.ShowDialog();
                                    overwrite_slot = get_character_slot;
                                }
                            }
                        }
                    }
                    else
                    {
                        //Set the overwrite slot to the slot that contains it.
                        overwrite_slot = Array.IndexOf(save_player.character, cbx_characters2.Text);
                    }
                }
            }

            if (overwrite_slot != -1)
            {
                save_player.character[overwrite_slot] = cbx_characters2.Text;
                save_player.color[overwrite_slot] = cbx_colors2.SelectedIndex + 1;
            }

            var player_info_box = new frm_save_player(save_player);
            if (player_info_box.ShowDialog() == DialogResult.OK)
            {
                add_to_sheets(get_new_player);

                int hold_index = cbx_name1.SelectedIndex;

                cbx_name1.BeginUpdate();                                            //Begin
                cbx_name1.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name1.Items.Add(global_values.roster[i].tag);
                }
                cbx_name1.EndUpdate();                                              //End
                cbx_name1.SelectedIndex = hold_index;   //Set the combobox index to 0

                cbx_name2.BeginUpdate();                                            //Begin
                cbx_name2.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name2.Items.Add(global_values.roster[i].tag);
                }
                cbx_name2.EndUpdate();                                              //End
                cbx_name2.SelectedIndex = cbx_name2.Items.IndexOf(get_new_player.tag);     //Set the combobox index to 0
            }
        }

        private void btn_score1_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image1 = openFileDialog3.FileName;
                pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);
                btn_score1_image1.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player1-1").ReplaceWith(new XElement("player1-1", global_values.score1_image1));
                xml.Save(global_values.settings_file);
            }
        }

        private void btn_score1_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image2 = openFileDialog3.FileName;
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
                btn_score1_image2.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player1-2").ReplaceWith(new XElement("player1-2", global_values.score1_image2));
                xml.Save(global_values.settings_file);
            }
        }

        private void btn_score1_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image3 = openFileDialog3.FileName;
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
                btn_score1_image3.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player1-3").ReplaceWith(new XElement("player1-3", global_values.score1_image3));
                xml.Save(global_values.settings_file);
            }
        }

        private void btn_score2_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image1 = openFileDialog3.FileName;
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
                btn_score2_image1.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player2-1").ReplaceWith(new XElement("player2-1", global_values.score2_image1));
                xml.Save(global_values.settings_file);
            }
        }

        private void btn_score2_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image2 = openFileDialog3.FileName;
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
                btn_score2_image2.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player2-2").ReplaceWith(new XElement("player2-2", global_values.score2_image2));
                xml.Save(global_values.settings_file);
            }
        }

        private void btn_score2_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image3 = openFileDialog3.FileName;
                pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);
                btn_score2_image3.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("image-scoring").Element("player2-3").ReplaceWith(new XElement("player2-3", global_values.score2_image3));
                xml.Save(global_values.settings_file);
            }
        }

        private void ckb_clipboard_CheckedChanged(object sender, EventArgs e)
        {
            //Set the value of the global value to reflect the checked status of this checkbox
            global_values.copy_video_title = ckb_clipboard.Checked;

            //Update the settings file
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("youtube").Element("copy-title").ReplaceWith(new XElement("copy-title", ckb_clipboard.Checked.ToString()));
            xml.Save(global_values.settings_file);
        }

        private void ckb_youtube_CheckedChanged(object sender, EventArgs e)
        {
            //Set the status of the Youtube enable flag to the status of this checkbox
            global_values.enable_youtube = ckb_youtube.Checked;

            //Enable/Disable youtube setting control to reflect this
            txt_playlist.Enabled = global_values.enable_youtube;
            txt_description.Enabled = global_values.enable_youtube;
            rdb_xsplit.Enabled = global_values.enable_youtube;
            rdb_obs.Enabled = global_values.enable_youtube;

            if (ckb_youtube.Checked == false)
            {
                btn_upload.Text = "Create Thumbnail";
                btn_team_upload.Text = "Create Thumbnail";
            }
            else
            {
                btn_upload.Text = "Upload to YouTube";
                btn_team_upload.Text = "Upload to YouTube";
            }

            //Update the settings file
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("youtube").Element("enable-youtube").ReplaceWith(new XElement("enable-youtube", ckb_youtube.Checked.ToString()));
            xml.Save(global_values.settings_file);
        }

        private void txt_description_TextChanged(object sender, EventArgs e)
        {
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("youtube").Element("default-description").ReplaceWith(new XElement("default-description", txt_description.Text));
            xml.Save(global_values.settings_file);
        }

        private void txt_playlist_TextChanged(object sender, EventArgs e)
        {
            if(txt_playlist.Text != global_values.playlist_name)
            {
                btn_playlist.Enabled = true;
            }
            else
            {
                btn_playlist.Enabled = false;
            }
        }

        private void rdb_fullsheet_CheckedChanged(object sender, EventArgs e)
        {
            global_values.sheets_info = "info-and-queue";

            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("google-sheets").Element("sheet-info").ReplaceWith(new XElement("sheet-info", global_values.sheets_info));
            xml.Save(global_values.settings_file);
        }

        private void rdb_infoonly_CheckedChanged(object sender, EventArgs e)
        {
            global_values.sheets_info = "info-only";

            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("google-sheets").Element("sheet-info").ReplaceWith(new XElement("sheet-info", global_values.sheets_info));
            xml.Save(global_values.settings_file);
        }

        private void btn_test_sheet_Click(object sender, EventArgs e)
        {
            btn_test_sheet.BackColor = Color.Transparent;
            txt_sheets.BackColor = Color.White;
            btn_test_sheet.Enabled = false;
            check_sheets();
        }

        private void tab_integrations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_json.BackColor == warning_color)
            {
                tab_integrations.SelectedIndex = 0;
                System.Media.SystemSounds.Asterisk.Play();
            }
            if (btn_playlist.Enabled == true)
            {
                tab_integrations.SelectedIndex = 1;
                btn_playlist.BackColor = warning_color;
                System.Media.SystemSounds.Asterisk.Play();
            }
            if (btn_test_sheet.Enabled == true)
            {
                tab_integrations.SelectedIndex = 2;
                btn_test_sheet.BackColor = warning_color;
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void tab_mainsettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (txt_roster_directory.BackColor == warning_color ||
                txt_stream_directory.BackColor == warning_color ||
                txt_thumbnail_directory.BackColor == warning_color ||
                txt_vods.BackColor == warning_color)
            {
                tab_mainsettings.SelectedIndex = 0;
                System.Media.SystemSounds.Asterisk.Play();
            }
            if (ckb_scoreboad.Checked == true &&
                (btn_score1_image1.BackColor == warning_color ||
                 btn_score1_image2.BackColor == warning_color ||
                 btn_score1_image3.BackColor == warning_color ||
                 btn_score2_image1.BackColor == warning_color ||
                 btn_score2_image2.BackColor == warning_color ||
                 btn_score2_image3.BackColor == warning_color))
            {
                tab_mainsettings.SelectedIndex = 1;
                System.Media.SystemSounds.Asterisk.Play();
            }
            if ((ckb_sponsor.Checked == true &&
                (txt_sponsor.BackColor == warning_color ||
                 txt_sponsor.Text == "")) ||
                (ckb_region.Checked == true &&
                (txt_region.BackColor == warning_color ||
                 txt_region.Text == "")))
            {
                tab_mainsettings.SelectedIndex = 2;
                System.Media.SystemSounds.Asterisk.Play();
            }
        }

        private void btn_reset_scores_Click(object sender, EventArgs e)
        {
            nud_score1.Value = 0;
            nud_score2.Value = 0;
        }



        private void cbx_characters1_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string nextkey = "";

            while (cb.FindString(cb.Text) < 0 && cb.Text.Length > 0)

            {
                //Find the previously enterred text
                string subStringText = cb.Text.Substring(0, cb.Text.Length - 1);
                if (subStringText.Length > 0)
                {
                    nextkey = subStringText.Substring(subStringText.Length - 1, 1);
                }
                else
                {
                    nextkey = "";
                }
                cb.Text = subStringText;
                cb.Select(subStringText.Length, 0);
               

            }
            if (nextkey != "")
            {
                cb.Text = cb.Text.Substring(0, cb.Text.Length - 1);
                //Need to remove most recent key
                SendKeys.Send(nextkey);
            }
        }

        private void cbx_characters1_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.FindString(cb.Text) < 0)
            {
                cb.SelectedIndex = 0;
            }
        }

        private void cbx_characters2_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string nextkey = "";

            while (cb.FindString(cb.Text) < 0 && cb.Text.Length > 0)

            {
                //Find the previously enterred text
                string subStringText = cb.Text.Substring(0, cb.Text.Length - 1);
                if (subStringText.Length > 0)
                {
                    nextkey = subStringText.Substring(subStringText.Length - 1, 1);
                }
                else
                {
                    nextkey = "";
                }
                cb.Text = subStringText;
                cb.Select(subStringText.Length, 0);


            }
            if (nextkey != "")
            {
                cb.Text = cb.Text.Substring(0, cb.Text.Length - 1);
                //Need to remove most recent key
                SendKeys.Send(nextkey);
            }
        }

        private void cbx_characters2_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.FindString(cb.Text) < 0)
            {
                cb.SelectedIndex = 0;
            }
        }

        private void cbx_name1_TextChanged_1(object sender, EventArgs e)
        {
            btn_update.Enabled = true;
            string output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
            switch (btn_update.Text)
            {
                case "Start":
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_update.Enabled = false;
                        update_uldata(1, global_values.current_youtube_data);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                    }
                    break;
            }
        }

        private void ckb_loser1_CheckedChanged(object sender, EventArgs e)
        {
            string output_name = get_output_name(cbx_name1.Text, ckb_loser1.Checked, 1);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
        }

        private void ckb_loser2_CheckedChanged(object sender, EventArgs e)
        {
            string output_name = get_output_name(cbx_name2.Text, ckb_loser2.Checked, 2);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
        }

        private void txt_playlist_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_playlist_Click(this, new EventArgs());
            }
        }

        private void btn_dashboard_Click(object sender, EventArgs e)
        {
            if (global_values.sheets_info == "info-and-queue" && global_values.enable_sheets == true && txt_sheets.Text != "" && txt_sheets.Text != null)
            {
                if (cbx_format.Text == "Singles")
                {
                    var dashboard = new frm_streamqueue(txt_sheets.Text);
                    dashboard.Show();
                }
                else
                {
                    var dashboard = new frm_streamqueue_dubs(txt_sheets.Text);
                    dashboard.Show();
                }
            }
            else
            {
                MessageBox.Show("The Google Sheets integration must be enabled and set to use both Info and Queue in order to use this feature. Please check the settings to ensure this is set up correctly.");
            }
        }

        private void btn_addplayer_Click(object sender, EventArgs e)
        {
            if (global_values.enable_sheets == true && txt_sheets.Text != "" && txt_sheets.Text != null)
            {
                var get_tag = new frm_newplayer();
                get_tag.ShowDialog();

                var player_info_box = new frm_save_player(save_name);
                if (player_info_box.ShowDialog() == DialogResult.OK)
                {
                    add_to_sheets(get_new_player);
                    string new_player = get_new_player.tag;
                    MessageBox.Show(new_player + " has been added to the player database within the connected Google Sheet.");
                }
            }
            else
            {
                MessageBox.Show("The Google Sheets integration must be enabled in order to use this feature. Please check the settings to ensure this is set up correctly.");
            }
        }

        void comboBox_MouseWheel(object sender, MouseEventArgs e)
        {
            ((HandledMouseEventArgs)e).Handled = true;
        }

        private void ckb_sponsor_CheckedChanged(object sender, EventArgs e)
        {
            global_values.enable_sponsor = ckb_sponsor.Checked;
            txt_sponsor.Enabled = ckb_sponsor.Checked;
            btn_sponsor.Enabled = ckb_sponsor.Checked;

            if (txt_sponsor.Text == "")
            {
                txt_sponsor.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
                tab_mainsettings.SelectedIndex = 2;
            }

            //Update the settings file
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("sponsor-and-region").Element("enable-sponsor").ReplaceWith(new XElement("enable-sponsor", ckb_sponsor.Checked.ToString()));
            xml.Save(global_values.settings_file);
        }

        private void ckb_region_CheckedChanged(object sender, EventArgs e)
        {
            global_values.enable_region = ckb_region.Checked;
            txt_region.Enabled = ckb_region.Checked;
            btn_region.Enabled = ckb_region.Checked;

            if(txt_region.Text == "")
            {
                txt_region.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
                tab_mainsettings.SelectedIndex = 2;
            }

            //Update the settings file
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("sponsor-and-region").Element("enable-region").ReplaceWith(new XElement("enable-region", ckb_region.Checked.ToString()));
            xml.Save(global_values.settings_file);
        }

        private void txt_sponsor_TextChanged(object sender, EventArgs e)
        {
                if (Directory.Exists(txt_sponsor.Text) &&
                    global_values.vods_directory != txt_sponsor.Text)
                {
                    txt_sponsor.BackColor = Color.White;
                    global_values.sponsor_directory = txt_sponsor.Text;
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("sponsor-and-region").Element("sponsor-directory").ReplaceWith(new XElement("sponsor-directory", txt_sponsor.Text));
                    xml.Save(global_values.settings_file);
                }
                else
                {
                    txt_sponsor.BackColor = warning_color;
                    tab_main.SelectedIndex = 3;
                    tab_mainsettings.SelectedIndex = 2;
                }
        }

        private void txt_region_TextChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txt_region.Text) &&
                global_values.vods_directory != txt_region.Text)
            {
                txt_region.BackColor = Color.White;
                global_values.region_directory = txt_region.Text;
                XDocument xml = XDocument.Load(global_values.settings_file);
                xml.Root.Element("sponsor-and-region").Element("region-directory").ReplaceWith(new XElement("region-directory", txt_region.Text));
                xml.Save(global_values.settings_file);
            }
            else
            {
                txt_region.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
                tab_mainsettings.SelectedIndex = 2;
            }
        }

        private void btn_sponsor_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder that sponsor files are stored in
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_sponsor.Text = folderBrowserDialog1.SelectedPath;                 //Update the global value with the new directory
            }
        }

        private void btn_region_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder that region files are stored in
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_region.Text = folderBrowserDialog1.SelectedPath;                 //Update the global value with the new directory
            }
        }

        private string get_output_name(string raw_name, bool loser, int player_number)
        {
            string[] image_files = { @"\sponsor " + player_number.ToString() + ".png",
                                    @"\region " + player_number.ToString() + ".png" };
            foreach (string replace_image in image_files)
            {
                if (File.Exists(global_values.output_directory + replace_image))
                {
                    File.Delete(global_values.output_directory + replace_image);
                }
                File.Copy(@"left.png", global_values.output_directory + replace_image);
            }
            string output_name = raw_name;
            string sponsor_name = "";
            if (global_values.enable_sponsor == true)
            {
                check_for_sponsor(ref output_name, ref sponsor_name);
                if(File.Exists(global_values.sponsor_directory + @"\" + sponsor_name + @".png"))
                {
                    Image sponsor_image = Image.FromFile(global_values.sponsor_directory + @"\" + sponsor_name + @".png");
                    sponsor_image.Save(global_values.output_directory + @"\sponsor " + player_number.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            //Check if the region contains the seperator
            if (global_values.enable_region == true && global_values.player_roster_number[player_number] != -1)
            {
                string find_region = global_values.roster[global_values.player_roster_number[player_number]].region;

                if (find_region.Contains(" - "))
                {
                    //Check each index of the region string
                    for (int i = 0; i < find_region.Length; i++)
                    {
                        //Check if the seperator is present at this index
                        if (find_region.Substring(i).StartsWith(" - ") == true)
                        {
                            //Set the region to be before this index
                            find_region = find_region.Substring(0, i);
                            break;
                        }
                    }
                }

                if (File.Exists(global_values.region_directory + @"\" + find_region + @".png"))
                {
                    Image region_image = Image.FromFile(global_values.region_directory + @"\" + find_region + @".png");
                    region_image.Save(global_values.output_directory + @"\region " + player_number.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            if (loser == false)
            {
                return output_name;
            }
            else
            {
                return output_name + " [L]";
            }
        }

        private void cbx_format_SelectedIndexChanged(object sender, EventArgs e)
        {
            tab_main.TabPages.Remove(tab_ingame_display);
            tab_main.TabPages.Remove(tab_doubles_display);
            if (cbx_format.Text == "Singles")
            {
                tab_main.TabPages.Insert(1, tab_ingame_display);
                player_boxes[1].tag = cbx_name1;
                player_boxes[1].twitter = txt_alt1;
                player_boxes[1].character = cbx_characters1;
                player_boxes[1].color = cbx_colors1;
                player_boxes[2].tag = cbx_name2;
                player_boxes[2].twitter = txt_alt2;
                player_boxes[2].character = cbx_characters2;
                player_boxes[2].color = cbx_colors2;

            }
            if (cbx_format.Text == "Doubles")
            {
                tab_main.TabPages.Insert(1, tab_doubles_display);
                player_boxes[1].tag = cbx_team1_name1;
                player_boxes[1].twitter = txt_team1_twitter1;
                player_boxes[1].character = cbx_team1_character1;
                player_boxes[1].color = cbx_team1_color1;
                player_boxes[2].tag = cbx_team2_name1;
                player_boxes[2].twitter = txt_team2_twitter1;
                player_boxes[2].character = cbx_team2_character1;
                player_boxes[2].color = cbx_team2_color1;
            }
            XDocument xml = XDocument.Load(global_values.settings_file);
            xml.Root.Element("etc").Element("format").ReplaceWith(new XElement("format", cbx_format.Text));
            xml.Save(global_values.settings_file);
        }

        private void update_characters(ref ComboBox update_box)
        {
            //Update the character list combobox
            update_box.BeginUpdate();                                      //Begin
            update_box.Items.Clear();                                      //Empty the item list
            int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                //Loop through every character
            for (int x = 0; x < character_count; x++)
            {
                update_box.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
            }
            update_box.EndUpdate();                                        //End
            update_box.SelectedIndex = 0;                                  //Set the combobox index to 0

        }
        
        private void update_names(ref ComboBox update_box)
        {
            update_box.BeginUpdate();                                            //Begin
            update_box.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                update_box.Items.Add(global_values.roster[i].tag);
            }
            update_box.EndUpdate();                                              //End
            update_box.Text = "";
        }

        private void cbx_team1_name1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == cbx_team1_name1.Text)
                {
                    global_values.player_roster_number[1] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_team1_name1.Text = global_values.roster[i].get_display_name(); });
                    txt_team1_twitter1.Text = global_values.roster[i].twitter;

                    cbx_team1_character1.BeginUpdate();                                      //Begin
                    cbx_team1_character1.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_team1_character1.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_team1_character1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_team1_character1.EndUpdate();                                        //End
                    cbx_team1_character1.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_team1_color1.SelectedIndex = global_values.roster[i].color[0] - 1;
                    return;
                }
            }
        }

        private void cbx_team1_character1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_team1_color1.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_team1_character1.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = ResizeImage(Image.FromFile(character_path + @"\" + (i + 1).ToString() + @"\stock.png"), 32, 32);
            }
            cbx_team1_color1.DisplayImages(colors);
            cbx_team1_color1.SelectedIndex = 0;

            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[1]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_team1_character1.Text)
                    {
                        cbx_team1_color1.SelectedIndex = check_player.color[i] - 1;
                        break;
                    }
                }
            }
            cbx_team1_color1.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(1, global_values.current_youtube_data);
            }
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
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

                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void cbx_team1_name1_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        string output_name = get_output_name(cbx_team1_name1.Text, ckb_team1_lose.Checked, 1);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
                        output_name = get_output_name(cbx_team1_name1.Text, false, 1) + " + " +
                                        get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 3);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\team name1.txt", output_name);
                        update_uldata(1, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void txt_team1_twitter1_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_team1_twitter1.Text);
                        update_uldata(1, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team1_color1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = global_values.game_path + @"\" + cbx_team1_character1.Text + @"\" + (cbx_team1_color1.SelectedIndex + 1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory1 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 1.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(1, global_values.current_youtube_data);
            }
        }

        private void nud_team1_score_ValueChanged(object sender, EventArgs e)
        {
            decimal current_point = nud_team1_score.Value;       //Pull the current game wins for Player 1

            //Keep the current point value at or below the match point value
            if (current_point >= 3 && ckb_scoreboad.Checked == true)
            {
                nud_team1_score.Value = 3;
            }
            //Check if automatic updates are enabled
            if (global_values.auto_update == true)
            {
                //Write Player 1's score to a file to be used by the stream program
                System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_team1_score.Value.ToString());
                //Check if Image Scoreboard is enabled
                if (ckb_scoreboad.Checked == true)
                {
                    //Store the location of the score image for Player 1 used by the stream program
                    string score_file = global_values.output_directory + @"\score1.png";

                    //Delete the score image if it exists
                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    //Check the current value of Player 1's score
                    switch (nud_team1_score.Value)
                    {
                        case 0:                     //Save an empty image for Player 1's score                                      
                            File.Copy(@"left.png", score_file);
                            break;
                        case 1:                     //Copy the Player 1 Score 1 image for Player 1's score
                            File.Copy(global_values.score1_image1, score_file);
                            break;
                        case 2:                     //Copy the Player 1 Score 2 image for Player 1's score
                            File.Copy(global_values.score1_image2, score_file);
                            break;
                        case 3:                     //Copy the Player 1 Score 3 image for Player 1's score
                            File.Copy(global_values.score1_image3, score_file);
                            break;
                    }
                }
            }
            else
            {

                btn_team_update.Enabled = true;                                                              //Unable the update button
                btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");    //Add a yellow glow to the update button
            }
        }

        private void ckb_team1_lose_CheckedChanged(object sender, EventArgs e)
        {
            string output_name = get_output_name(cbx_team1_name1.Text, ckb_team1_lose.Checked, 1);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
            output_name = get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name3.txt", output_name);
            output_name = get_output_name(cbx_team1_name1.Text, false, 1) + " + " + 
                get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\team name1.txt", output_name);
        }

        private void cbx_team1_name2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == cbx_team1_name2.Text)
                {
                    global_values.player_roster_number[1] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_team1_name2.Text = global_values.roster[i].get_display_name(); });
                    txt_team1_twitter2.Text = global_values.roster[i].twitter;

                    cbx_team1_character2.BeginUpdate();                                      //Begin
                    cbx_team1_character2.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_team1_character2.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_team1_character2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_team1_character2.EndUpdate();                                        //End
                    cbx_team1_character2.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_team1_color2.SelectedIndex = global_values.roster[i].color[0] - 1;
                    return;
                }
            }
        }

        private void txt_team1_twitter2_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text3.txt", txt_team1_twitter2.Text);
                        update_uldata(3, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team1_character2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_team1_color2.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_team1_character2.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = ResizeImage(Image.FromFile(character_path + @"\" + (i + 1).ToString() + @"\stock.png"), 32, 32);
            }
            cbx_team1_color2.DisplayImages(colors);
            cbx_team1_color2.SelectedIndex = 0;

            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[1]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_team1_character2.Text)
                    {
                        cbx_team1_color2.SelectedIndex = check_player.color[i] - 1;
                        break;
                    }
                }
            }
            cbx_team1_color2.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(3, global_values.current_youtube_data);
            }
        }

        private void cbx_team1_color2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory3 = global_values.game_path + @"\" + cbx_team1_character2.Text + @"\" + (cbx_team1_color2.SelectedIndex + 1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory3 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 3.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(3, global_values.current_youtube_data);
            }
        }

        private void cbx_team2_name1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == cbx_team2_name1.Text)
                {
                    global_values.player_roster_number[1] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_team2_name1.Text = global_values.roster[i].get_display_name(); });
                    txt_team2_twitter1.Text = global_values.roster[i].twitter;

                    cbx_team2_character1.BeginUpdate();                                      //Begin
                    cbx_team2_character1.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_team2_character1.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_team2_character1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_team2_character1.EndUpdate();                                        //End
                    cbx_team2_character1.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_team2_color1.SelectedIndex = global_values.roster[i].color[0] - 1;
                    return;
                }
            }
        }

        private void cbx_team2_name1_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        string output_name = get_output_name(cbx_team2_name1.Text, ckb_team1_lose.Checked, 1);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
                        output_name = get_output_name(cbx_team2_name1.Text, false, 1) + " + " +
                                        get_output_name(cbx_team2_name2.Text, ckb_team1_lose.Checked, 3);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\team name2.txt", output_name);
                        update_uldata(2, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void txt_team2_twitter1_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_team2_twitter1.Text);
                        update_uldata(2, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team2_character1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_team2_color1.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_team2_character1.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = ResizeImage(Image.FromFile(character_path + @"\" + (i + 1).ToString() + @"\stock.png"), 32, 32);
            }
            cbx_team2_color1.DisplayImages(colors);
            cbx_team2_color1.SelectedIndex = 0;

            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[1]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_team2_character1.Text)
                    {
                        cbx_team2_color1.SelectedIndex = check_player.color[i] - 1;
                        break;
                    }
                }
            }
            cbx_team2_color1.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(2, global_values.current_youtube_data);
            }
        }

        private void cbx_team2_color1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = global_values.game_path + @"\" + cbx_team2_character1.Text + @"\" + (cbx_team2_color1.SelectedIndex + 1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory2 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 2.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(2, global_values.current_youtube_data);
            }
        }

        private void cbx_team2_name2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == cbx_team2_name2.Text)
                {
                    global_values.player_roster_number[1] = i;
                    this.BeginInvoke((MethodInvoker)delegate { this.cbx_team2_name2.Text = global_values.roster[i].get_display_name(); });
                    txt_team2_twitter2.Text = global_values.roster[i].twitter;

                    cbx_team2_character2.BeginUpdate();                                      //Begin
                    cbx_team2_character2.Items.Clear();                                      //Empty the item list
                    for (int ii = 0; ii <= 4; ii++)
                    {
                        string character_name = global_values.roster[i].character[ii];
                        if (character_name != "")
                        {
                            cbx_team2_character2.Items.Add(character_name);
                        }
                    }
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                        //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_team2_character2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_team2_character2.EndUpdate();                                        //End
                    cbx_team2_character2.SelectedIndex = 0;                                  //Set the combobox index to 0

                    cbx_team2_color2.SelectedIndex = global_values.roster[i].color[0] - 1;
                    return;
                }
            }
        }

        private void cbx_team1_name2_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        string output_name = get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 1);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name3.txt", output_name);
                        output_name = get_output_name(cbx_team1_name1.Text, false, 1) + " + " +
                                        get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 3);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\team name1.txt", output_name);
                        update_uldata(3, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team2_name2_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        string output_name = get_output_name(cbx_team2_name2.Text, ckb_team1_lose.Checked, 1);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name3.txt", output_name);
                        output_name = get_output_name(cbx_team2_name1.Text, false, 1) + " + " +
                                        get_output_name(cbx_team2_name2.Text, ckb_team1_lose.Checked, 3);
                        System.IO.File.WriteAllText(global_values.output_directory + @"\team name1.txt", output_name);
                        update_uldata(4, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team2_twitter2_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text4.txt", txt_team2_twitter2.Text);
                        update_uldata(4, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void cbx_team2_character2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_team2_color2.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_team2_character2.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = ResizeImage(Image.FromFile(character_path + @"\" + (i + 1).ToString() + @"\stock.png"), 32, 32);
            }
            cbx_team2_color2.DisplayImages(colors);
            cbx_team2_color2.SelectedIndex = 0;

            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
            {
                player_info check_player = global_values.roster[global_values.player_roster_number[1]];
                for (int i = 0; i < 5; i++)
                {
                    if (check_player.character[i] == cbx_team2_character2.Text)
                    {
                        cbx_team2_color2.SelectedIndex = check_player.color[i] - 1;
                        break;
                    }
                }
            }
            cbx_team2_color2.DropDownHeight = 400;
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(4, global_values.current_youtube_data);
            }
        }

        private void cbx_team2_color2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory4 = global_values.game_path + @"\" + cbx_team2_character2.Text + @"\" + (cbx_team2_color2.SelectedIndex + 1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory4 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 4.png", System.Drawing.Imaging.ImageFormat.Png);
            if (global_values.auto_update == true && btn_team_update.Text == "Update")
            {
                update_uldata(4, global_values.current_youtube_data);
            }
        }

        private void cbx_team1_character1_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string nextkey = "";

            while (cb.FindString(cb.Text) < 0 && cb.Text.Length > 0)

            {
                //Find the previously enterred text
                string subStringText = cb.Text.Substring(0, cb.Text.Length - 1);
                if (subStringText.Length > 0)
                {
                    nextkey = subStringText.Substring(subStringText.Length - 1, 1);
                }
                else
                {
                    nextkey = "";
                }
                cb.Text = subStringText;
                cb.Select(subStringText.Length, 0);


            }
            if (nextkey != "")
            {
                cb.Text = cb.Text.Substring(0, cb.Text.Length - 1);
                //Need to remove most recent key
                SendKeys.Send(nextkey);
            }
        }

        private void cbx_team_round_TextChanged(object sender, EventArgs e)
        {
            btn_team_update.Enabled = true;
            if (cbx_team_round.Text == "Grand Finals")
            {
                ckb_team1_lose.Enabled = true;
                ckb_team1_lose.Visible = true;
                ckb_team2_lose.Enabled = true;
                ckb_team2_lose.Visible = true;
            }
            else
            {
                ckb_team1_lose.Checked = false;
                ckb_team1_lose.Enabled = false;
                ckb_team1_lose.Visible = false;
                ckb_team2_lose.Checked = false;
                ckb_team2_lose.Enabled = false;
                ckb_team2_lose.Visible = false;
            }
            switch (btn_team_update.Text)
            {
                case "Start":
                    btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                    break;
                case "Update":
                    if (global_values.auto_update == false)
                    {
                        btn_team_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
                    }
                    else
                    {
                        btn_team_update.Enabled = false;
                        System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_team_round.Text);
                        update_uldata(5, global_values.current_youtube_data);
                    }
                    break;
            }
        }

        private void btn_team_swap_Click(object sender, EventArgs e)
        {
            //Hold Player 1's information within temporary variables
            string hold_alt = txt_team1_twitter1.Text;
            string hold_name = cbx_team1_name1.Text;
            decimal hold_score = nud_team1_score.Value;
            string hold_character = cbx_team1_character1.Text;
            int hold_color = cbx_team1_color1.SelectedIndex;
            string hold_directory = image_directory1;
            bool hold_L = ckb_team1_lose.Checked;

            //Move Player 2's information to Player 1's slot
            txt_team1_twitter1.Text = txt_team2_twitter1.Text;
            cbx_team1_name1.Text = cbx_team2_name1.Text;
            nud_team1_score.Value = nud_team2_score.Value;
            cbx_team1_character1.Text = cbx_team2_character1.Text;
            cbx_team1_color1.SelectedIndex = cbx_team2_color1.SelectedIndex;
            image_directory1 = image_directory2;
            ckb_team1_lose.Checked = ckb_team2_lose.Checked;

            //Move the information stored within temporary variables to Player 2's slot
            txt_team2_twitter1.Text = hold_alt;
            cbx_team2_name1.Text = hold_name;
            nud_team2_score.Value = hold_score;
            cbx_team2_character1.Text = hold_character;
            cbx_team2_color1.SelectedIndex = hold_color;
            image_directory2 = hold_directory;
            ckb_team2_lose.Checked = hold_L;

            //Hold Player 1's information within temporary variables
            hold_alt = txt_team1_twitter2.Text;
            hold_name = cbx_team1_name2.Text;
            hold_character = cbx_team1_character2.Text;
            hold_color = cbx_team1_color2.SelectedIndex;
            hold_directory = image_directory3;

            //Move Player 2's information to Player 1's slot
            txt_team1_twitter2.Text = txt_team2_twitter2.Text;
            cbx_team1_name2.Text = cbx_team2_name2.Text;
            cbx_team1_character2.Text = cbx_team2_character2.Text;
            cbx_team1_color2.SelectedIndex = cbx_team2_color2.SelectedIndex;
            image_directory3 = image_directory4;

            //Move the information stored within temporary variables to Player 2's slot
            txt_team2_twitter2.Text = hold_alt;
            cbx_team2_name2.Text = hold_name;
            cbx_team2_character2.Text = hold_character;
            cbx_team2_color2.SelectedIndex = hold_color;
            image_directory4 = hold_directory;
        }

        private void btn_team_reset_Click(object sender, EventArgs e)
        {
                nud_team1_score.Value = 0;
                nud_team2_score.Value = 0;
        }

        private void btn_team_update_Click(object sender, EventArgs e)
        {
            btn_team_update.Enabled = false;             //Disable this button until further action is needed
            //Reset the image of the update button to default
            btn_team_update.Image = null;
            string output_name = "";

            //Save Player 1's information to seperate files to be used by the stream program
            output_name = get_output_name(cbx_team1_name1.Text, ckb_team1_lose.Checked, 1);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
            System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_team1_twitter1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_team1_score.Value.ToString());
            System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_team1_character1.Text);
            //Save Player 2's information to seperate files to be used by the stream program
            output_name = get_output_name(cbx_team2_name1.Text, ckb_team2_lose.Checked, 2);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
            System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_team2_twitter1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_team2_score.Value.ToString());
            System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", cbx_team2_character1.Text);
            //Save Player 3's information to seperate files to be used by the stream program
            output_name = get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 1);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name3.txt", output_name);
            System.IO.File.WriteAllText(global_values.output_directory + @"\alt text3.txt", txt_team1_twitter2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\character name3.txt", cbx_team1_character2.Text);
            //Save Player 4's information to seperate files to be used by the stream program
            output_name = get_output_name(cbx_team2_name2.Text, ckb_team2_lose.Checked, 2);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name4.txt", output_name);
            System.IO.File.WriteAllText(global_values.output_directory + @"\alt text4.txt", txt_team2_twitter2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\character name4.txt", cbx_team2_character2.Text);
            //Save the Tournament information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_team_round.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
            output_name = get_output_name(cbx_team1_name1.Text, false, 1) + " + " +
                get_output_name(cbx_team1_name2.Text, ckb_team1_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\team name1.txt", output_name);
            output_name = get_output_name(cbx_team2_name1.Text, false, 1) + " + " +
                get_output_name(cbx_team2_name2.Text, ckb_team2_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\team name2.txt", output_name);

            switch (btn_team_update.Text)                        //Perform action based on the current text of the button
            {
                case "Start":                               //Start the match
                    nud_team1_score.Enabled = true;              //Enable score control for Player 1
                    nud_team2_score.Enabled = true;              //Enable score control for Player 2
                    btn_team_update.Text = "Update";             //Update the text of this button
                    ttp_tooltip.SetToolTip(btn_team_update,
                        "Pushes updates to the player and match information " +
                        "into the Stream Files Directory.");
                    global_values.current_youtube_data = create_uldata(4);
                    break;
                case "Update":                              //Update the stream files with the new information provided
                    //Check if Image Scoreboard is enabled
                    update_uldata(1, global_values.current_youtube_data);
                    update_uldata(2, global_values.current_youtube_data);
                    update_uldata(3, global_values.current_youtube_data);
                    update_uldata(4, global_values.current_youtube_data);
                    update_uldata(5, global_values.current_youtube_data);
                    if (ckb_scoreboad.Checked == true)
                    {
                        //Store the location of the score image for Player 1 used by the stream program
                        string score_file = global_values.output_directory + @"\score1.png";

                        //Delete the score image if it exists
                        if (File.Exists(score_file))
                        {
                            File.Delete(score_file);
                        }

                        //Check the current value of Player 1's score
                        switch (nud_team1_score.Value)
                        {
                            case 0:                     //Save an empty image for Player 1's score                                      
                                File.Copy(@"left.png", score_file);
                                break;
                            case 1:                     //Copy the Player 1 Score 1 image for Player 1's score
                                File.Copy(global_values.score1_image1, score_file);
                                break;
                            case 2:                     //Copy the Player 1 Score 2 image for Player 1's score
                                File.Copy(global_values.score1_image2, score_file);
                                break;
                            case 3:                     //Copy the Player 1 Score 3 image for Player 1's score
                                File.Copy(global_values.score1_image3, score_file);
                                break;
                        }

                        score_file = global_values.output_directory + @"\score2.png";

                        if (File.Exists(score_file))
                        {
                            File.Delete(score_file);
                        }

                        switch (nud_team2_score.Value)
                        {
                            case 0:
                                File.Copy(@"left.png", score_file);
                                break;
                            case 1:
                                File.Copy(global_values.score2_image1, score_file);
                                break;
                            case 2:
                                File.Copy(global_values.score2_image2, score_file);
                                break;
                            case 3:
                                File.Copy(global_values.score2_image3, score_file);
                                break;

                        }
                    }

                    break;
            }
        }

        private void nud_team2_score_ValueChanged(object sender, EventArgs e)
        {
            decimal current_point = nud_team2_score.Value;

            if (current_point >= 3 && ckb_scoreboad.Checked == true)
            {
                nud_team2_score.Value = 3;
            }
            if (global_values.auto_update == true)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_team2_score.Value.ToString());
                if (ckb_scoreboad.Checked == true)
                {
                    string score_file = global_values.output_directory + @"\score2.png";

                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    switch (nud_team2_score.Value)
                    {
                        case 0:
                            File.Copy(@"left.png", score_file);
                            break;
                        case 1:
                            File.Copy(global_values.score2_image1, score_file);
                            break;
                        case 2:
                            File.Copy(global_values.score2_image2, score_file);
                            break;
                        case 3:
                            File.Copy(global_values.score2_image3, score_file);
                            break;
                    }
                }
            }
            else
            {
                btn_update.Enabled = true;
                btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
            }
        }

        private void ckb_team2_lose_CheckedChanged(object sender, EventArgs e)
        {
            string output_name = get_output_name(cbx_team2_name1.Text, ckb_team2_lose.Checked, 1);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
            output_name = get_output_name(cbx_team2_name2.Text, ckb_team2_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\player name4.txt", output_name);
            output_name = get_output_name(cbx_team2_name1.Text, false, 1) + " + " +
                get_output_name(cbx_team2_name2.Text, ckb_team2_lose.Checked, 3);
            System.IO.File.WriteAllText(global_values.output_directory + @"\team name2.txt", output_name);
        }

        private void btn_team_upload_Click(object sender, EventArgs e)
        {
            string team_name1 = cbx_team1_name1.Text + " + " + cbx_team1_name2.Text;
            string team_name2 = cbx_team2_name1.Text + " + " + cbx_team2_name2.Text;

            if (global_values.enable_youtube == false)
            {
                string thumbnail_image_name = create_thumbnail(4,
                        team_name1,
                        team_name2,
                        cbx_team_round.Text,
                        txt_date.Text);
                string video_title = txt_tournament.Text + @" - " + cbx_team_round.Text + @" - " + 
                    team_name1 + @" (" + cbx_team1_character1.Text + " + " + cbx_team1_character2.Text + @") Vs. " + 
                    team_name2 + @" (" + cbx_team2_character1.Text + " + " + cbx_team2_character2.Text + @")";
                if (global_values.copy_video_title == true)
                {
                    Clipboard.SetText(video_title);
                    MessageBox.Show("A thumbnail image has been generated. \r\nVideo title copied to clipboard: \n" + video_title);
                }
                else
                {
                    MessageBox.Show("A thumbnail image has been generated.");
                }
            }
            else
            {
                //Create a thumbnail image and save its name
                string thumbnail_image_name = create_thumbnail(4,
                    team_name1,
                    team_name2,
                    cbx_team_round.Text,
                    txt_date.Text);

                XDocument doc = new XDocument(
                    new XElement("YouTube-Upload-Data",
                    new XElement("Player-Information",
                            new XElement("Player-1-Name", cbx_team1_name1.Text),
                            new XElement("Player-1-Twitter", txt_team1_twitter1.Text),
                            new XElement("Player-1-Character", cbx_team1_character1.Text),
                            new XElement("Player-1-Color", cbx_team1_color1.SelectedIndex),
                            new XElement("Player-2-Name", cbx_team1_name2.Text),
                            new XElement("Player-2-Twitter", txt_team1_twitter2.Text),
                            new XElement("Player-2-Character", cbx_team1_character2.Text),
                            new XElement("Player-2-Color", cbx_team1_color2.SelectedIndex),
                            new XElement("Player-3-Name", cbx_team2_name1.Text),
                            new XElement("Player-3-Twitter", txt_team2_twitter1.Text),
                            new XElement("Player-3-Character", cbx_team2_character1.Text),
                            new XElement("Player-3-Color", cbx_team2_color1.SelectedIndex),
                            new XElement("Player-4-Name", cbx_team2_name2.Text),
                            new XElement("Player-4-Twitter", txt_team2_twitter2.Text),
                            new XElement("Player-4-Character", cbx_team2_character2.Text),
                            new XElement("Player-4-Color", cbx_team2_color2.SelectedIndex)
                            ),
                    new XElement("Match-Information",
                            new XElement("Format", cbx_format.Text),
                            new XElement("Round", cbx_team_round.Text),
                            new XElement("Bracket-URL", txt_bracket.Text),
                            new XElement("Tournament-Name", txt_tournament.Text),
                            new XElement("Date", txt_date.Text),
                            new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected))
                    ));

                global_values.current_youtube_data =
                    txt_tournament.Text + "_" +
                    cbx_team_round.Text + "_" +
                    team_name1 + "_VS_" +
                    team_name2 + "_" +
                    DateTime.Now.ToString("MMddyyyyHHmmss") + @".uldata";

                global_values.current_youtube_data = global_values.current_youtube_data.Replace("|", "l")
                        .Replace(@"\", "l").Replace(@"/", "l").Replace(":", "").Replace("*", "").Replace("?", "")
                        .Replace('"'.ToString(), "").Replace("'", "").Replace(">", "").Replace("<", "");

                global_values.current_youtube_data = global_values.thumbnail_directory + @"\" + global_values.current_youtube_data;

                if (global_values.current_youtube_data.Length > 250)
                {
                    global_values.current_youtube_data = global_values.current_youtube_data.Substring(0, 250);
                }

                doc.Save(global_values.current_youtube_data);

                //Pass the event upload_form_enable_button_event() to the new form as the function "enable_button()"
                btn_upload.Text = "Upload Window Open";
                btn_upload.Enabled = false;             //Disable this button until further action is needed
                global_values.reenable_upload = DateTime.Now.ToString("MMddyyyyHHmmss");   //Set the flag to allow the button to be re-abled on form close

                string video_title = txt_tournament.Text + @" - " + cbx_team_round.Text + @" - " +
                    team_name1 + @" (" + cbx_team1_character1.Text + " + " + cbx_team1_character2.Text + @") Vs. " +
                    team_name2 + @" (" + cbx_team2_character1.Text + " + " + cbx_team2_character2.Text + @")";

                if (global_values.copy_video_title == true)
                {
                    Clipboard.SetText(video_title);
                    MessageBox.Show("Video title copied to clipboard: \n" + video_title);
                }

                string video_description = txt_description.Text.Replace("INFO_TOURNAMENT", txt_tournament.Text);
                video_description = video_description.Replace("INFO_DATE", txt_date.Text);
                video_description = video_description.Replace("INFO_ROUND", cbx_team_round.Text);
                video_description = video_description.Replace("INFO_BRACKET", txt_bracket.Text);
                video_description = video_description.Replace("INFO_PLAYER1", team_name1);
                video_description = video_description.Replace("INFO_PLAYER2", team_name2);
                video_description = video_description.Replace("INFO_TWITTER1", txt_team1_twitter1.Text + " + " + txt_team1_twitter2.Text);
                video_description = video_description.Replace("INFO_TWITTER2", txt_team2_twitter1.Text + " + " + txt_team2_twitter2.Text);
                video_description = video_description.Replace("INFO_CHARACTER1", cbx_team1_character1.Text + " + " + cbx_team1_character2.Text);
                video_description = video_description.Replace("INFO_CHARACTER2", cbx_team2_character1.Text + " + " + cbx_team2_character2.Text);


                //Create a new form and provide it with a Video title based off the provided information,
                //as well as a description and the thumbnail image created
                var upload_form = new frm_uploading(video_title,
                    video_description,
                    global_values.thumbnail_directory + @"\" + thumbnail_image_name,
                    global_values.vods_directory + @"\" + global_values.new_vod_detected,
                    global_values.reenable_upload, false);
                upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                upload_form.Show();                     //Show the form        
            }
        }

        private void btn_team_next_Click(object sender, EventArgs e)
        {
            if (global_values.enable_youtube == true)
            {
                global_values.reenable_upload = "";
                btn_team_upload.Enabled = true;
                btn_team_upload.Text = "Upload to YouTube";
            }

            nud_team1_score.Value = 0;
            nud_team1_score.Enabled = false;
            nud_team2_score.Value = 0;
            nud_team2_score.Enabled = false;

            btn_team_update.Text = @"Start";
            ttp_tooltip.SetToolTip(btn_team_update,
                "Click to begin the set and enable score control.\n" +
                "Pushes player and match information into the\n" +
                "Stream Files Directory.");

            btn_team_update.Image = null;
            btn_team_update.Enabled = true;

            string[] image_files = { @"\score1.png", @"\score2.png", @"\Stock Icon 1.png", @"\Stock Icon 2.png",
                 @"\Stock Icon 3.png", @"\Stock Icon 4.png", @"\sponsor 1.png", @"\sponsor 2.png", @"\sponsor 3.png",
                 @"\sponsor 4.png", @"\region 1.png" , @"\region 2.png" , @"\region 3.png" , @"\region 4.png" };
            foreach (string replace_image in image_files)
            {
                if (File.Exists(global_values.output_directory + replace_image))
                {
                    File.Delete(global_values.output_directory + replace_image);
                }
                File.Copy(@"left.png", global_values.output_directory + replace_image);
            }

            global_values.player_roster_number[1] = -1;
            global_values.player_roster_number[2] = -1;
            global_values.player_roster_number[3] = -1;
            global_values.player_roster_number[4] = -1;

            cbx_team1_name1.Text = "";
            txt_team1_twitter1.Text = "";
            cbx_team1_character1.SelectedIndex = 0;
            cbx_team1_color1.SelectedIndex = 0;

            cbx_team1_name2.Text = "";
            txt_team1_twitter2.Text = "";
            cbx_team1_character2.SelectedIndex = 0;
            cbx_team1_color2.SelectedIndex = 0;

            cbx_team2_name1.Text = "";
            txt_team2_twitter1.Text = "";
            cbx_team2_character1.SelectedIndex = 0;
            cbx_team2_color1.SelectedIndex = 0;

            cbx_team2_name2.Text = "";
            txt_team2_twitter2.Text = "";
            cbx_team2_character2.SelectedIndex = 0;
            cbx_team2_color2.SelectedIndex = 0;



            if (global_values.enable_sheets == true && txt_sheets.Text != "" && txt_sheets.Text != null)
            {
                if (global_values.sheets_info == "info-and-queue")
                {
                    import_from_sheets(false, 4);
                }
                else
                {
                    info_from_sheets();
                }
            }
        }

        private string create_uldata(int player_number)
        {
            //Creates a ULdata file that contains information on the ongoing match.
            //Used to upload a video to YouTube using the information of a previous
            //match in the situation where an upload failed or was missed.
            //Returns a string of the file location.
            ///////////////////////////////////////////////////////////////////////

            //Initialize the file name
            string uldata_file = "";

            //Create the uldata
            XDocument doc = new XDocument(
                new XElement("YouTube-Upload-Data",
                new XElement("Player-Information",
                        new XElement("Player-1",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-2",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-3",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", "")),
                        new XElement("Player-4",
                            new XElement("Name", ""),
                            new XElement("Twitter", ""),
                            new XElement("Character", ""),
                            new XElement("Color", ""))
                        ),
                new XElement("Match-Information",
                        new XElement("Game", global_values.game_info[0]),
                        new XElement("Format", cbx_format.Text),
                        new XElement("Round", cbx_round.Text),
                        new XElement("Bracket-URL", txt_bracket.Text),
                        new XElement("Tournament-Name", txt_tournament.Text),
                        new XElement("Date", txt_date.Text),
                        new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected))
                ));

            //Insert player information depending on the number of players
            switch(player_number)
            {
                case 2:
                    doc.Root.Element("Player-Information").Element("Player-1").ReplaceWith(
                            new XElement("Player-1",
                                new XElement("Name", cbx_name1.Text),
                                new XElement("Twitter", txt_alt1.Text),
                                new XElement("Character", cbx_characters1.Text),
                                new XElement("Color", cbx_colors1.SelectedIndex + 1)));
                    doc.Root.Element("Player-Information").Element("Player-2").ReplaceWith(
                            new XElement("Player-2",
                                new XElement("Name", cbx_name2.Text),
                                new XElement("Twitter", txt_alt2.Text),
                                new XElement("Character", cbx_characters2.Text),
                                new XElement("Color", cbx_colors2.SelectedIndex + 1)));
                    uldata_file =
                            txt_tournament.Text + "_" +
                            cbx_round.Text + "_" +
                            cbx_name1.Text + "_VS_" +
                            cbx_name2.Text + "_" +
                            DateTime.Now.ToString("MMddyyyyHHmmss") + @".uldata";
                    break;
                case 4:
                    doc.Root.Element("Player-Information").Element("Player-1").ReplaceWith(
                            new XElement("Player-1",
                                new XElement("Name", cbx_team1_name1.Text),
                                new XElement("Twitter", txt_team1_twitter1.Text),
                                new XElement("Character", cbx_team1_character1.Text),
                                new XElement("Color", cbx_team1_color1.SelectedIndex + 1)));
                    doc.Root.Element("Player-Information").Element("Player-2").ReplaceWith(
                            new XElement("Player-2",
                                new XElement("Name", cbx_team2_name1.Text),
                                new XElement("Twitter", txt_team2_twitter1.Text),
                                new XElement("Character", cbx_team2_character1.Text),
                                new XElement("Color", cbx_team2_color1.SelectedIndex + 1)));
                    doc.Root.Element("Player-Information").Element("Player-3").ReplaceWith(
                            new XElement("Player-3",
                                new XElement("Name", cbx_team1_name2.Text),
                                new XElement("Twitter", txt_team1_twitter2.Text),
                                new XElement("Character", cbx_team1_character2.Text),
                                new XElement("Color", cbx_team1_color2.SelectedIndex + 1)));
                    doc.Root.Element("Player-Information").Element("Player-4").ReplaceWith(
                            new XElement("Player-4",
                                new XElement("Name", cbx_team2_name2.Text),
                                new XElement("Twitter", txt_team2_twitter2.Text),
                                new XElement("Character", cbx_team2_character2.Text),
                                new XElement("Color", cbx_team2_color2.SelectedIndex + 1)));
                    string team_name1 = cbx_team1_name1.Text + " + " + cbx_team1_name2.Text;
                    string team_name2 = cbx_team2_name1.Text + " + " + cbx_team2_name2.Text;

                    uldata_file =
                            txt_tournament.Text + "_" +
                            cbx_round.Text + "_" +
                            team_name1 + "_VS_" +
                            team_name2 + "_" +
                            DateTime.Now.ToString("MMddyyyyHHmmss") + @".uldata";
                    break;
            }

            //Remove invalid characters from the name
            uldata_file = uldata_file.Replace("|", "-")
                    .Replace(@"\", "-").Replace(@"/", "-").Replace(":", "").Replace("*", "").Replace("?", "")
                    .Replace('"'.ToString(), "").Replace("'", "").Replace(">", "").Replace("<", "");

            uldata_file = global_values.thumbnail_directory + @"\" + uldata_file;

            if (uldata_file.Length > 250)
            {
                uldata_file = uldata_file.Substring(0, 250);
            }

            doc.Save(uldata_file);

            return uldata_file;
        }

        private void update_uldata(int player_number, string uldata)
        {
            //updates a uldata file with new info for a player or round settings.
            //player_number is the number of the player according to the main window(1-4)
            /////////Setting this value to 5 will instead update the round settings.
            //uldata is the path to the uldata file to update
            //format is Singles or Doubles and will pull information from the fields
            /////////related to the specified format.
            //////////////////////////////////////////////////////////////////////////////

            //lock this to a single execution
            if (update_check == true)
            { 
                //Load the uldata file
                XDocument doc = XDocument.Load(uldata);

                switch(player_number)
                {
                    case 5:
                        doc.Root.Element("Match-Information").ReplaceWith(new XElement("Match-Information",
                            new XElement("Game", global_values.game_info[0]),
                            new XElement("Format", cbx_format.Text),
                            new XElement("Round", cbx_round.Text),
                            new XElement("Bracket-URL", txt_bracket.Text),
                            new XElement("Tournament-Name", txt_tournament.Text),
                            new XElement("Date", txt_date.Text),
                            new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected)));
                        break;
                    default:
                        string player_text = "Player-" + player_number.ToString();
                        doc.Root.Element("Player-Information").Element(player_text).ReplaceWith(
                            new XElement(player_text,
                            new XElement("Name", player_boxes[player_number].tag.Text),
                            new XElement("Twitter", player_boxes[player_number].twitter.Text),
                            new XElement("Character", player_boxes[player_number].character.Text),
                            new XElement("Color", player_boxes[player_number].color.SelectedIndex + 1)));
                        break;
                }

                //Save the uldata file with the new information.
                doc.Save(uldata);
            }
        }

        private void btn_database_login_Click(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "Master Orders Global Playerbase";
            dbCon.DatabaseUserName = txt_database_username.Text;
            dbCon.DatabasePassword = txt_database_password.Text;
            try
            {
                dbCon.IsConnect();
                MessageBox.Show("Database Login Successful.");
                global_values.database_username = dbCon.DatabaseUserName;
                global_values.database_password = dbCon.DatabasePassword;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Database Login Failed. Please ensure that your username and password are correct.");
                dbCon.Close();
            }
        }
    }



    
 
}
