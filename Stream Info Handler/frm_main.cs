//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2018, Dan Sanchez, All rights reserved.
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

namespace Stream_Info_Handler
{
    public partial class frm_main : Form
    {
        //Initialize the variables to contain the character image file directories
        public string image_directory1 = Directory.GetCurrentDirectory();
        public string image_directory2 = Directory.GetCurrentDirectory();

        Color warning_color = Color.FromArgb(234, 153, 153);

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

        //Initialize the variables containing YouTube Playlist information
        List<string> playlist_items = new List<string>();
        List<string> playlist_names = new List<string>();
        const int MAX_PLAYERS = 200;


        public frm_main()
        {
            InitializeComponent();
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
            global_values.roster = new player_info[MAX_PLAYERS];
            global_values.player_roster_number = new int[3];
            global_values.player_roster_number[1] = -1;
            global_values.player_roster_number[2] = -1;
            //Check if a settings file exists
            if (!Directory.Exists(@"C:\Users\Public\Stream Info Handler"))
            {
                Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler");
            }

            if (!File.Exists(@"settings.xml"))
            {
                //Show the settings initial setup window to create a settings file
                var settings_box = new frm_settings_start();
                Point starting_location = this.Location;
                starting_location = Point.Add(starting_location, new Size(0, -200));
                settings_box.Location = starting_location;
                settings_box.ShowDialog();
            }
           


            //Load the settings file data
            XDocument xml = XDocument.Load(@"settings.xml");



            //Read the stream file and thumbnail output directories from the data
            global_values.output_directory = (string)xml.Root.Element("directories").Element("stream-directory");
            txt_stream_directory.Text = global_values.output_directory;
            if (!Directory.Exists(global_values.output_directory))
            {
                txt_stream_directory.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }

            global_values.thumbnail_directory = (string)xml.Root.Element("directories").Element("thumbnail-directory");
            txt_thumbnail_directory.Text = global_values.thumbnail_directory;
            if (!Directory.Exists(global_values.thumbnail_directory))
            {
                txt_thumbnail_directory.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
            }

            //Read the character roster from the data
            global_values.game_path = (string)xml.Root.Element("directories").Element("game-directory");
            txt_roster_directory.Text = global_values.game_path;

            //Verify that a directory has been provided
            if (txt_roster_directory.Text != "")
            {
                //Verify that the necessary files for a roster directory are present
                if (File.Exists(txt_roster_directory.Text + @"\game info.txt") && File.Exists(txt_roster_directory.Text + @"\characters.txt"))
                {
                    //Read the files for the game information and character roster
                    global_values.game_info = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\game info.txt");
                    global_values.characters = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\characters.txt");
                    pic_game_logo.Image = Image.FromFile(txt_roster_directory.Text + @"\game_logo.png");

                    //Update the character list combobox
                    cbx_characters1.BeginUpdate();                                      //Begin
                    cbx_characters1.Items.Clear();                                      //Empty the item list
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                    //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters1.EndUpdate();                                        //End
                    cbx_characters1.SelectedIndex = 0;                                  //Set the combobox index to 0

                    //Update the character list combobox
                    cbx_characters2.BeginUpdate();                                      //Begin
                    cbx_characters2.Items.Clear();                                      //Empty the item list
                    //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters2.EndUpdate();                                        //End
                    cbx_characters2.SelectedIndex = 0;                                  //Set the combobox index to 0
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


            //Read the VoDs directory from the data
            global_values.vods_directory = (string)xml.Root.Element("directories").Element("vods-directory");
            //Create a directory monitor
            global_values.vod_monitor = new FileSystemWatcher();
            txt_vods.Text = global_values.vods_directory;                               //Set text of the VoDs directory setting
            if (!Directory.Exists(global_values.vods_directory))
            {
                //If a directory has not been provided, mark the field for an error and switch tabs to show it
                txt_vods.BackColor = warning_color;
                tab_main.SelectedIndex = 3;
                global_values.vod_monitor.Path = null;              //Set the path of the directory monitor to the VoDs directory
            }
            else
            {
                global_values.vod_monitor.Path = global_values.vods_directory;              //Set the path of the directory monitor to the VoDs directory
            }
            global_values.vod_monitor.Created += FileSystemWatcher_Created;             //Associate the file creation event to the monitor
            global_values.vod_monitor.Deleted += FileSystemWatcher_Deleted;             //Associate the file deletion event to the monitor
            global_values.vod_monitor.EnableRaisingEvents = true;                       //Enable to monitor to trigger these events

            //Read the video Title Copying flag from the data
            string copy_title = (string)xml.Root.Element("youtube").Element("copy-title");
            //Check if the video title should be added to the clipboard
            if (copy_title == "True")
            {
                ckb_clipboard.Checked = true;                                            //Check the setting box, triggering the related event
                global_values.copy_video_title = true;                                  //Set the global video title flag to true
            }
            else
            {
                global_values.copy_video_title = false;                                 //Set the global video title flag to false
            }

            //Read the Youtube Flag from the data
            string use_youtube = (string)xml.Root.Element("youtube").Element("enable-youtube");
            //Check if youtube is enabled
            if (use_youtube == "True")
            {
                ckb_youtube.Checked = true;
            }
            else
            {
                btn_upload.Text = "Create Thumbnail";
            }

            //Read the YouTube Playlist flag from the data
            string playlist_name = (string)xml.Root.Element("youtube").Element("playlist-name");
            //Check if playlists are enabled
            if (playlist_name != "")
            {
                global_values.playlist_name = playlist_name;
                global_values.playlist_id = (string)xml.Root.Element("youtube").Element("playlist-id");
                txt_playlist.Text = playlist_name;
            }

            //Read the Default youtube description from the data
            global_values.youtube_description = (string)xml.Root.Element("youtube").Element("default-description");
            txt_description.Text = System.Text.RegularExpressions.Regex.Replace(global_values.youtube_description, @"\r\n|\n|\r", Environment.NewLine);

            //Read the Google Sheets flag from the data
            string use_sheets = (string)xml.Root.Element("google-sheets").Element("enable-sheets");
            //Read the Google Sheet ID from the data
            txt_sheets.Text = (string)xml.Root.Element("google-sheets").Element("sheets-id");

            string startup_sheets = (string)xml.Root.Element("google-sheets").Element("startup-sheets");
            if (startup_sheets == "True")
            {
                ckb_startup_sheets.Checked = true;
            }

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


            //Read the automatic updates flag from the data
            string automatic_updates = (string)xml.Root.Element("etc").Element("automatic-updates");
            //Check if automatic updates are enabled
            if (automatic_updates == "true")
            {
                global_values.auto_update = true;                                       //Set the global automatic updates flag to true
            }
            else
            {
                global_values.auto_update = false;                                      //Set the global automatic updates flag to false
                rdb_automatic.Checked = false;                                          //Remove the check on the automatic update radio button
                rdb_manual.Checked = true;                                              //Add the check to the manual update radio button
            }

            //Read the username and json file from the data
            global_values.youtube_username = (string)xml.Root.Element("youtube").Element("username");
            global_values.json_file = (string)xml.Root.Element("youtube").Element("json-file");



            //Check if the Google Sheets integration is enabled
            if (use_sheets == "True" && File.Exists(global_values.json_file))
            {
                ckb_sheets.Checked = true;                                              //Check the setting box, triggering the related event
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
                    }
                    rdb_infoonly.Enabled = true;
                    xml.Root.Element("google-sheets").Element("startup-sheets").ReplaceWith(new XElement("startup-sheets", "False"));
                    xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", ""));
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", ""));
                    xml.Save(@"settings.xml");
                    ignore_settings = true;

                    if (startup_sheets == "True")
                    {
                        if (global_values.sheets_info == "info-and-queue")
                        {
                            import_from_sheets(false);
                        }
                        else
                        {
                            info_from_sheets();
                        }
                    }
                    else
                    {
                        info_from_sheets();
                    }

                    ignore_settings = false;
                    xml.Root.Element("google-sheets").Element("startup-sheets").ReplaceWith(new XElement("startup-sheets", "True"));
                    xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", global_values.json_file));
                    xml.Save(@"settings.xml");
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


            txt_json.Text = global_values.json_file;
            if (!File.Exists(global_values.json_file))
            {
                ckb_youtube.Checked = false;
                ckb_sheets.Checked = false;
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
            ttp_tooltip.SetToolTip(ckb_startup_sheets,
                "Enable stream queue information loading from the sheet\n" +
                "when Master Orders starts up.");
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
        }

        //Create a thumbnail image using the information input for the players and tournament
        public string create_thumbnail(string character_directory1, string character_directory2, 
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
            Image left_character = Image.FromFile(character_directory1 + @"\left.png");
            Image right_character = Image.FromFile(character_directory2 + @"\right.png");

            drawing.Clear(Color.White);                                         //Clear the surface of all data

            drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

            drawing.DrawImage(left_character, 0, 0, 1920, 1080);                //Draw Player 1's character
            drawing.DrawImage(right_character, 0, 0, 1920, 1080);               //Draw Player 2's character

            drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters

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
            drawing.DrawPath(black_stroke, draw_date);
            drawing.FillPath(white_text, draw_date);

            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name1, calmfont);            //Measure the width of Player 1's name with this font size
            } while (namesize.Width >= 1100);                                           //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 1's name to its drawing path
            draw_name1.AddString(
                player_name1,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(480, 110),                                            //drawing location
                text_center);                                                   //text alignment
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name1);
            drawing.FillPath(white_text, draw_name1);

            font_size = 115;                                                            //Reset the font size
            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name2, calmfont);            //Measure the width of Player 2's name with this font size
            } while (namesize.Width >= 1100);                                           //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 2's name to its drawing path
            draw_name2.AddString(
                player_name2,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(1440, 110),                                           //drawing location
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
                new Point(960, 620),                                            //drawing location
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

            //Move Player 2's information to Player 1's slot
            txt_alt1.Text = txt_alt2.Text;
            cbx_name1.Text = cbx_name2.Text;
            nud_score1.Value = nud_score2.Value;
            cbx_characters1.Text = cbx_characters2.Text;
            cbx_colors1.SelectedIndex = cbx_colors2.SelectedIndex;
            image_directory1 = image_directory2;

            //Move the information stored within temporary variables to Player 2's slot
            txt_alt2.Text = hold_alt;
            cbx_name2.Text = hold_name;
            nud_score2.Value = hold_score;
            cbx_characters2.Text = hold_character;
            cbx_colors2.SelectedIndex = hold_color;
            image_directory2 = hold_directory;
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder containing the character roster
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_roster_directory.Text = folderBrowserDialog1.SelectedPath;                            //Update the setting text
                if (File.Exists(txt_roster_directory.Text + @"\game info.txt") && 
                    File.Exists(txt_roster_directory.Text + @"\characters.txt") &&
                    global_values.vods_directory != txt_roster_directory.Text)
                {
                    txt_roster_directory.BackColor = Color.White;
                    global_values.game_path = txt_roster_directory.Text;                    //Save the directory

                    //Save the setting to the settings file
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("game-directory").ReplaceWith(new XElement("game-directory", txt_roster_directory.Text));
                    xml.Save(@"settings.xml");
                }
                else
                {
                    txt_roster_directory.BackColor = warning_color;
                }
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //Reset the image of the update button to default
            btn_update.Image = null;

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

                    //Save Player 1's information to seperate files to be used by the stream program
                    if (ckb_loser1.Checked == false)
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text + " [L]");
                    }
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                    //Save Player 2's information to seperate files to be used by the stream program
                    if (ckb_loser2.Checked == false)
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text + " [L]");
                    }
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

                    //Save Player 1's information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    if (ckb_loser1.Checked == false)
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text + " [L]");
                    }
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                    //Save Player 2's information to seperate files to be used by the stream program
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    if (ckb_loser2.Checked == false)
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text);
                    }
                    else
                    {
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text + " [L]");
                    }
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
                if (global_values.vods_directory != txt_stream_directory.Text)
                {
                    global_values.output_directory = txt_stream_directory.Text;         //Save the directory
                                                                                                //Save the setting to the settings file
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Element("stream-directory").ReplaceWith(new XElement("stream-directory", txt_stream_directory.Text));
                    xml.Save(@"settings.xml");
                }
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
            string thumbnail_image_name = create_thumbnail(global_values.game_path + @"\" + cbx_characters1.Text + @"\" + (cbx_colors1.SelectedIndex + 1).ToString() + @"\",
                global_values.game_path + @"\" + cbx_characters2.Text + @"\" + (cbx_colors2.SelectedIndex + 1).ToString() + @"\",
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
                if (global_values.vods_directory != txt_thumbnail_directory.Text)
                {
                    global_values.thumbnail_directory = txt_thumbnail_directory.Text;              //Save the directory
                                                                                                   //Save the information to the settings file
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("thumbnail-directory").ReplaceWith(new XElement("thumbnail-directory", txt_thumbnail_directory.Text));
                    xml.Save(@"settings.xml");
                }
            }
        }

        private void nud_score1_ValueChanged(object sender, EventArgs e)
        {
            decimal current_point = nud_score1.Value;       //Pull the current game wins for Player 1

            //Keep the current point value at or below the match point value
            if (current_point >= 3)
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

            if (current_point >= 3)
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
            switch(btn_update.Text)
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
                        if (ckb_loser2.Checked == false)
                        {
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text);
                        }
                        else
                        {
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text + " [L]");
                        }
                    }
                    break;
            }
        }

        private void cbx_name1_TextChanged(object sender, EventArgs e)
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
                        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
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

            string score_file = global_values.output_directory + @"\score1.png";
            if (File.Exists(score_file))
            {
                File.Delete(score_file);
            }
            File.Copy(@"left.png", score_file);

            score_file = global_values.output_directory + @"\score2.png";
            if (File.Exists(score_file))
            {
                File.Delete(score_file);
            }
            File.Copy(@"left.png", score_file);


            if (global_values.enable_sheets == true && txt_sheets.Text != "" && txt_sheets.Text != null)
            {
                if (global_values.sheets_info == "info-and-queue")
                {
                    import_from_sheets(false);
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
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
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
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\blue.gif");
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
                global_values.json_file = txt_json.Text;
                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", txt_json.Text));
                xml.Save(@"settings.xml");
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("youtube").Element("json-file").ReplaceWith(new XElement("json-file", txt_json.Text));
                    xml.Save(@"settings.xml");
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("stream-directory").ReplaceWith(new XElement("stream-directory", txt_stream_directory.Text));
                    xml.Save(@"settings.xml");
                }
                else
                {
                    txt_stream_directory.BackColor = warning_color;
                }
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("thumbnail-directory").ReplaceWith(new XElement("thumbnail-directory", txt_thumbnail_directory.Text));
                    xml.Save(@"settings.xml");
                }
                else
                {
                    txt_thumbnail_directory.BackColor = warning_color;
                }
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("game-directory").ReplaceWith(new XElement("game-directory", txt_roster_directory.Text));
                    xml.Save(@"settings.xml");

                    //Read the files for the game information and character roster
                    global_values.game_info = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\game info.txt");
                    global_values.characters = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\characters.txt");
                    pic_game_logo.Image = Image.FromFile(txt_roster_directory.Text + @"\game_logo.png");

                    //Update the character list combobox
                    cbx_characters1.BeginUpdate();                                      //Begin
                    cbx_characters1.Items.Clear();                                      //Empty the item list
                    int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                    //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters1.EndUpdate();                                        //End
                    cbx_characters1.SelectedIndex = 0;                                  //Set the combobox index to 0

                    //Update the character list combobox
                    cbx_characters2.BeginUpdate();                                      //Begin
                    cbx_characters2.Items.Clear();                                      //Empty the item list
                    //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_characters2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_characters2.EndUpdate();                                        //End
                    cbx_characters2.SelectedIndex = 0;                                  //Set the combobox index to 0
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


                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", "True"));
                xml.Save(@"settings.xml");
            }
            else
            {
                nud_score1.Maximum = 99;
                nud_score2.Maximum = 99;
                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", "False"));
                xml.Save(@"settings.xml");
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
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("etc").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", "true"));
            xml.Save(@"settings.xml");
        }

        private void rdb_manual_CheckedChanged(object sender, EventArgs e)
        {
            global_values.auto_update = false;
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("etc").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", "false"));
            xml.Save(@"settings.xml");
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", global_values.playlist_name));
                    xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", global_values.playlist_id));
                    xml.Save(@"settings.xml");
                }
                else
                {
                    if (txt_playlist.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        global_values.playlist_name = "";
                        global_values.playlist_id = "";

                        XDocument xml = XDocument.Load(@"settings.xml");
                        xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", global_values.playlist_name));
                        xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", global_values.playlist_id));
                        xml.Save(@"settings.xml");
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

            //Set the enable status of all sheets settings to the checked status
            txt_sheets.Enabled = status;
            ckb_startup_sheets.Enabled = status;

            //Enable/Disable the player save buttons accordingly
            btn_save1.Enabled = status;
            btn_save1.Visible = status;
            btn_save2.Visible = status;
            btn_save2.Enabled = status;

            if(status == false)
            {
                btn_previous_match.Enabled = false;
                btn_previous_match.Visible = false;
            }

            //Update the global toggle and settings file
            global_values.enable_sheets = status;
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("google-sheets").Element("enable-sheets").ReplaceWith(new XElement("enable-sheets", status.ToString()));
            xml.Save(@"settings.xml");
        }

        private void add_to_sheets(player_info new_player)
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
                    global_values.roster[i].color[0] = new_player.color[0];
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
                                                global_values.roster[player_index].color[0]};

            //Create a data set from the array
            Google.Apis.Sheets.v4.Data.ValueRange data = new Google.Apis.Sheets.v4.Data.ValueRange();
            data.Values = new List<IList<object>> { oblist };
            //Set the range's row to the player index"
            string range2 = "Player Information!A" + (player_index+2).ToString() + ":J" + (player_index + 2).ToString();
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

        }

        private void import_from_sheets( bool reverse )
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

            List<string> ranges = new List<string>(new string[] { "Current Round Info!A1:D18", "Upcoming Matches!A1:E46", "Player Information!A2:O" + (MAX_PLAYERS + 1).ToString() });
            
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
            string manual_update = upcoming_matches[1][2].ToString();
            if (manual_update == "" && global_values.first_match == false)
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

            if(round_number == -1)
            {
                round_number = 0;
            }

            global_values.first_match = false;


            string[] player_name = new string[5];
            player_name[1] = upcoming_matches[4 + round_number][2].ToString();
            player_name[2] = upcoming_matches[4 + round_number][3].ToString();
            player_name[3] = upcoming_matches[5 + round_number][2].ToString();
            player_name[4] = upcoming_matches[5 + round_number][3].ToString();

            player_info[] player = new player_info[5];


            for (int player_number = 1; player_number <= 4; player_number++)
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
                        int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                        switch (player_number)
                        {
                            case 1:
                                txt_alt1.Text = "";
                                cbx_characters1.Items.Clear();
                                cbx_characters1.BeginUpdate();
                                for (int x = 0; x < character_count; x++)
                                {
                                    cbx_characters1.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                                }
                                cbx_characters1.EndUpdate();                                        //End
                                cbx_characters1.SelectedIndex = 0;                                  //Set the combobox index to 0
                                break;
                            case 2:
                                txt_alt2.Text = "";
                                cbx_characters2.Items.Clear();
                                cbx_characters2.BeginUpdate();
                                for (int x = 0; x < character_count; x++)
                                {
                                    cbx_characters2.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
                                }
                                cbx_characters2.EndUpdate();                                        //End
                                cbx_characters2.SelectedIndex = 0;                                  //Set the combobox index to 0
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


            cbx_name1.BeginUpdate();                                            //Begin
            cbx_name1.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_name1.Items.Add(global_values.roster[i].tag);
            }
            cbx_name1.EndUpdate();                                              //End
            cbx_name1.SelectedIndex = cbx_name1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0
            int hold_index = tab_main.SelectedIndex;
            tab_main.SelectedIndex = 1;
            cbx_name1.Text = player[1].get_display_name();

            cbx_name2.BeginUpdate();                                            //Begin
            cbx_name2.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_name2.Items.Add(global_values.roster[i].tag);
            }
            cbx_name2.EndUpdate();                                              //End
            cbx_name2.SelectedIndex = cbx_name2.Items.IndexOf(player[2].tag);   //Set the combobox index to 0
            cbx_name2.Text = player[2].get_display_name();
            tab_main.SelectedIndex = hold_index;

            cbx_round.Text = upcoming_matches[4 + round_number][1].ToString();
            txt_bracket.Text = current_round_info[1][2].ToString();
            txt_tournament.Text = current_round_info[0][2].ToString();

            cbx_colors1.SelectedIndex = player[1].color[0] - 1;
            cbx_colors2.SelectedIndex = player[2].color[0] - 1;



            var oblist = new List<object>() { "Current Match", "P1",  "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", "Next Match", "P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist2 = new List<object>() { "", "", player[1].tag, player[1].twitter, player[1].region, player[1].sponsor, player[1].character[0] ,
                                                "", "", "", player[3].tag, player[3].twitter, player[3].region, player[3].sponsor, player[3].character[0]};
            var oblist3 = new List<object>() { cbx_round.Text, "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                                ".", upcoming_matches[5 + round_number][1].ToString(), "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist4 = new List<object>() { "", "", player[2].tag, player[2].twitter, player[2].region, player[2].sponsor, player[2].character[0] ,
                                                "", "", "", player[4].tag, player[4].twitter, player[4].region, player[4].sponsor, player[4].character[0] };



            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange currentmatch = new Google.Apis.Sheets.v4.Data.ValueRange();
            currentmatch.Values = new List<IList<object>> { oblist, oblist2, oblist3, oblist4 };
            currentmatch.Range = "Current Round Info!A4:D18";
            currentmatch.MajorDimension = "COLUMNS";

            oblist = new List<object>() { (round_number).ToString() };
            oblist2 = new List<object>() { "" };

            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Range = "Upcoming Matches!B2:C2";
            upcoming.Values = new List<IList<object>> { oblist, oblist2 }; ;
            upcoming.MajorDimension = "COLUMNS";

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
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_characters1.Text);
                //Save Player 2's information to seperate files to be used by the stream program
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text);
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

            cbx_name1.BeginUpdate();                                            //Begin
            cbx_name1.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_name1.Items.Add(global_values.roster[i].tag);
            }
            cbx_name1.EndUpdate();                                              //End
            cbx_name1.Text = "";

            cbx_name2.BeginUpdate();                                            //Begin
            cbx_name2.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_name2.Items.Add(global_values.roster[i].tag);
            }
            cbx_name2.EndUpdate();                                              //End
            cbx_name2.Text = "";

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

            int sheet_number = sheet_information.Count;
            switch(sheet_number)
            {
                case 4:
                    if (sheet_information[0].Properties.Title == "Current Round Info" &&
                        sheet_information[1].Properties.Title == "Upcoming Matches" &&
                        sheet_information[2].Properties.Title == "Player Information" &&
                        sheet_information[3].Properties.Title == "Characters and Rounds")
                    {
                        rdb_fullsheet.Checked = true;
                        rdb_fullsheet.Enabled = true;
                        rdb_infoonly.Enabled = true;

                        btn_test_sheet.Enabled = false;
                        MessageBox.Show("The designated Google Sheet contains both player information and a stream queue. " +
                                        "Master Orders will use adapt to its information.");
                        btn_previous_match.Enabled = true;
                        btn_previous_match.Visible = true;
                        XDocument xml = XDocument.Load(@"settings.xml");
                        xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                        xml.Save(@"settings.xml");
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
                        btn_test_sheet.Enabled = false;
                        MessageBox.Show("The designated Google Sheet contains only player information. " +
                                        "Master Orders will use adapt to its information.");
                        XDocument xml = XDocument.Load(@"settings.xml");
                        xml.Root.Element("google-sheets").Element("sheets-id").ReplaceWith(new XElement("sheets-id", txt_sheets.Text));
                        xml.Save(@"settings.xml");
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

            switch (colors_count)
            {
                case 1:
                    Image[] colors1 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors1);
                    break;
                case 2:
                    Image[] colors2 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors2);
                    break;
                case 3:
                    Image[] colors3 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors3);
                    break;
                case 4:
                    Image[] colors4 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors4);
                    break;
                case 5:
                    Image[] colors5 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors5);
                    break;
                case 6:
                    Image[] colors6 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors6);
                    break;
                case 8:
                    Image[] colors8 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors8);
                    break;
                case 16:
                    Image[] colors16 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                        Image.FromFile(character_path + @"\9\stamp.png"),
                        Image.FromFile(character_path + @"\10\stamp.png"),
                        Image.FromFile(character_path + @"\11\stamp.png"),
                        Image.FromFile(character_path + @"\12\stamp.png"),
                        Image.FromFile(character_path + @"\13\stamp.png"),
                        Image.FromFile(character_path + @"\14\stamp.png"),
                        Image.FromFile(character_path + @"\15\stamp.png"),
                        Image.FromFile(character_path + @"\16\stamp.png"),
                    };
                    cbx_colors1.DisplayImages(colors16);
                    break;
            }
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
        }

        private void cbx_characters2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_colors2.Items.Clear();
            string character_path = global_values.game_path + @"\" + cbx_characters2.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));

            switch (colors_count)
            {
                case 1:
                    Image[] colors1 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors1);
                    break;
                case 2:
                    Image[] colors2 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors2);
                    break;
                case 3:
                    Image[] colors3 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors3);
                    break;
                case 4:
                    Image[] colors4 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors4);
                    break;
                case 5:
                    Image[] colors5 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors5);
                    break;
                case 6:
                    Image[] colors6 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors6);
                    break;
                case 8:
                    Image[] colors8 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors8);
                    break;
                case 16:
                    Image[] colors16 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                        Image.FromFile(character_path + @"\9\stamp.png"),
                        Image.FromFile(character_path + @"\10\stamp.png"),
                        Image.FromFile(character_path + @"\11\stamp.png"),
                        Image.FromFile(character_path + @"\12\stamp.png"),
                        Image.FromFile(character_path + @"\13\stamp.png"),
                        Image.FromFile(character_path + @"\14\stamp.png"),
                        Image.FromFile(character_path + @"\15\stamp.png"),
                        Image.FromFile(character_path + @"\16\stamp.png"),
                    };
                    cbx_colors2.DisplayImages(colors16);
                    break;
            }
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
        }

        private void cbx_colors1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = global_values.game_path + @"\" + cbx_characters1.Text + @"\" + (cbx_colors1.SelectedIndex+1).ToString() + @"\";
            Image stock_icon1 = Image.FromFile(image_directory1 + @"\stock.png");
            stock_icon1.Save(global_values.output_directory + @"\Stock Icon 1.png", System.Drawing.Imaging.ImageFormat.Png);
        }

        private void cbx_colors2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = global_values.game_path + @"\" + cbx_characters2.Text + @"\" + (cbx_colors2.SelectedIndex+1).ToString() + @"\";
            Image stock_icon2 = Image.FromFile(image_directory2 + @"\stock.png");
            stock_icon2.Save(global_values.output_directory + @"\Stock Icon 2.png", System.Drawing.Imaging.ImageFormat.Png);

        }

        private void txt_sheets_TextChanged(object sender, EventArgs e)
        {
            if (txt_sheets.Text != @"")
            {
                btn_test_sheet.Enabled = true;
            }
            else
            {
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
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("vods-directory").ReplaceWith(new XElement("vods-directory", txt_vods.Text));
                    xml.Save(@"settings.xml");

                    global_values.vods_directory = txt_vods.Text;
                    global_values.vod_monitor.Path = global_values.vods_directory;
               }
                else
                {
                    txt_vods.BackColor = warning_color;
                }
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
                        XDocument xml = XDocument.Load(global_values.current_youtube_data);
                        xml.Root.Element("Match-Information").Element("VoD-File").ReplaceWith(new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected));
                        xml.Save(global_values.current_youtube_data);
                    }
                }
                else
                {
                    global_values.temp_file = e.Name;

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
        }

        private void FileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (global_values.enable_youtube == true)
            {
                if (e.Name == global_values.temp_file)
                {
                    global_values.allow_upload = true;
                    btn_upload.Text = "Upload to YouTube";
                    btn_upload.BeginInvoke((Action)delegate ()
                    {
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
                if (txt_vods.Text != txt_roster_directory.Text &&
                    txt_vods.Text != txt_stream_directory.Text &&
                    txt_vods.Text != txt_thumbnail_directory.Text)
                {
                    global_values.vods_directory = txt_vods.Text;
                    XDocument xml = XDocument.Load(@"settings.xml");
                    xml.Root.Element("directories").Element("vods-directory").ReplaceWith(new XElement("vods-directory", txt_vods.Text));
                    xml.Save(@"settings.xml");
                }
                else
                {
                    txt_vods.BackColor = warning_color;
                }
            }
        }

        private void btn_upload_Click(object sender, EventArgs e)
        {
            if (global_values.enable_youtube == false)
            {
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
                string thumbnail_image_name = create_thumbnail(global_values.game_path + @"\" + cbx_characters1.Text + @"\" + (cbx_colors1.SelectedIndex + 1).ToString() + @"\",
                    global_values.game_path + @"\" + cbx_characters2.Text + @"\" + (cbx_colors2.SelectedIndex + 1).ToString() + @"\",
                    cbx_name1.Text,
                    cbx_name2.Text,
                    cbx_round.Text,
                    txt_date.Text);

                XDocument doc = new XDocument(
                    new XElement("YouTube-Upload-Data",
                    new XElement("Player-Information",
                            new XElement("Player-1-Name", cbx_name1.Text),
                            new XElement("Player-1-Twitter", txt_alt1.Text),
                            new XElement("Player-1-Character", cbx_characters1.Text),
                            new XElement("Player-1-Color", cbx_colors1.SelectedIndex),
                            new XElement("Player-2-Name", cbx_name2.Text),
                            new XElement("Player-2-Twitter", txt_alt2.Text),
                            new XElement("Player-2-Character", cbx_characters2.Text),
                            new XElement("Player-2-Color", cbx_colors2.SelectedIndex)
                            ),
                    new XElement("Match-Information",
                            new XElement("Round", cbx_round.Text),
                            new XElement("Bracket-URL", txt_bracket.Text),
                            new XElement("Tournament-Name", txt_tournament.Text),
                            new XElement("Date", txt_date.Text),
                            new XElement("VoD-File", global_values.vods_directory + @"\" + global_values.new_vod_detected))
                    ));

                global_values.current_youtube_data = global_values.thumbnail_directory + @"\" +
                    txt_tournament.Text + "_" +
                    cbx_round.Text + "_" +
                    cbx_name1.Text + "_VS_" +
                    cbx_name2.Text + "_" +
                    DateTime.Now.ToString("MMddyyyyHHmmss") + @".uldata";

                doc.Save(global_values.current_youtube_data);

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

                string video_description = txt_description.Text.Replace("INFO_TOURNAMENT", txt_tournament.Text);
                video_description = video_description.Replace("INFO_DATE", txt_date.Text);
                video_description = video_description.Replace("INFO_ROUND", cbx_round.Text);
                video_description = video_description.Replace("INFO_DATE", txt_date.Text);
                video_description = video_description.Replace("INFO_ROUND", cbx_round.Text);
                video_description = video_description.Replace("INFO_BRACKET", txt_bracket.Text);
                video_description = video_description.Replace("INFO_PLAYER1", cbx_name1.Text);
                video_description = video_description.Replace("INFO_PLAYER2", cbx_name2.Text);
                video_description = video_description.Replace("INFO_TWITTER1", txt_alt1.Text);
                video_description = video_description.Replace("INFO_TWITTER2", txt_alt2.Text);
                video_description = video_description.Replace("INFO_CHARACTER1", cbx_characters1.Text);
                video_description = video_description.Replace("INFO_CHARACTER2", cbx_characters2.Text);


                //Create a new form and provide it with a Video title based off the provided information,
                //as well as a description and the thumbnail image created
                var upload_form = new frm_uploading(video_title,
                    video_description,
                    global_values.thumbnail_directory + @"\" + thumbnail_image_name,
                    global_values.vods_directory + @"\" + global_values.new_vod_detected,
                    global_values.reenable_upload);
                upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                upload_form.Show();                     //Show the form        
            }
        }

        private void btn_upload_vod_Click(object sender, EventArgs e)
        {
            openFileDialog2.InitialDirectory = global_values.thumbnail_directory;
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                //Load the settings file data
                XDocument xml = XDocument.Load(openFileDialog2.FileName);
                string player1_name = (string)xml.Root.Element("Player-Information").Element("Player-1-Name");
                string player1_twitter = (string)xml.Root.Element("Player-Information").Element("Player-1-Twitter");
                string player1_character = (string)xml.Root.Element("Player-Information").Element("Player-1-Character");
                int player1_color = (int)xml.Root.Element("Player-Information").Element("Player-1-Color");
                string player2_name = (string)xml.Root.Element("Player-Information").Element("Player-2-Name");
                string player2_twitter = (string)xml.Root.Element("Player-Information").Element("Player-2-Twitter");
                string player2_character = (string)xml.Root.Element("Player-Information").Element("Player-2-Character");
                int player2_color = (int)xml.Root.Element("Player-Information").Element("Player-2-Color");

                string round = (string)xml.Root.Element("Match-Information").Element("Round");
                string bracket_url = (string)xml.Root.Element("Match-Information").Element("Bracket-URL");
                string tournament_name = (string)xml.Root.Element("Match-Information").Element("Tournament-Name");
                string date = (string)xml.Root.Element("Match-Information").Element("Date");
                string vod_file = (string)xml.Root.Element("Match-Information").Element("VoD-File");


                string thumbnail_image_name = create_thumbnail(global_values.game_path + @"\" + player1_character + @"\" + (player1_color + 1).ToString() + @"\",
                    global_values.game_path + @"\" + player2_character + @"\" + (player2_color + 1).ToString() + @"\",
                    player1_name,
                    player2_name,
                    round,
                    date);
                var upload_form = new frm_uploading(tournament_name + @" - " + round + @" - " + player1_name + @" (" + player1_character + @") Vs. " + player2_name + @" (" + player2_character + @")",
                    tournament_name + @" | " + date + "\r\nRomeoville, Illinois \r\nOrganized and streamed by UGS Gaming \r\nWatch live at https://www.twitch.tv/ugsgaming \r\nFollow us and our players on Twitter! \r\n@UGS_GAMlNG \r\n" + player1_name + @": " + player1_twitter + " \r\n" + player2_name + @": " + player2_twitter,
                    global_values.thumbnail_directory + @"\" + thumbnail_image_name,
                    vod_file,
                    "0");
                upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                upload_form.Show();                     //Show the form        
            }
        }

        private void rdb_xsplit_CheckedChanged(object sender, EventArgs e)
        {
            global_values.stream_software = @"XSplit";
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("etc").Element("stream-software").ReplaceWith(new XElement("stream-software", "XSplit"));
            xml.Save(@"settings.xml");
        }

        private void btn_previous_match_Click(object sender, EventArgs e)
        {
            if(global_values.enable_sheets == true)
            {
                import_from_sheets(true);
            }
        }

        private void gbx_entrants_Enter(object sender, EventArgs e)
        {

        }

        private void ckb_startup_sheets_CheckedChanged(object sender, EventArgs e)
        {
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("google-sheets").Element("startup-sheets").ReplaceWith(new XElement("startup-sheets", ckb_startup_sheets.Checked.ToString()));
            xml.Save(@"settings.xml");
        }

        private void rdb_obs_CheckedChanged(object sender, EventArgs e)
        {
            global_values.stream_software = @"OBS";
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("etc").Element("stream-software").ReplaceWith(new XElement("stream-software", "OBS"));
            xml.Save(@"settings.xml");
        }

        private void cbx_name1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for(int i = 0; i <= global_values.roster_size; i++)
            {
                if(global_values.roster[i].tag == cbx_name1.Text)
                {
                    global_values.player_roster_number[1] = i;
                    cbx_name1.Text = global_values.roster[i].get_display_name();
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
                    global_values.player_roster_number[2] = i;
                    cbx_name2.Text = global_values.roster[i].get_display_name();
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

                    return;
                }
            }
        }

        private void check_for_sponsor(string check_divider, ref string player_name, ref string player_team)
        {
            //Check if the tag contains the seperator
            if (player_name.Contains(check_divider))
            {
                //Initialize the sponsor and tag checking variables
                string check_team = player_name;
                string check_name = player_name;

                //Check each index of the tag string
                for (int i = 0; i < check_team.Length; i++)
                {
                    //Check if the seperator is present at this index
                    if (check_team.Substring(i).StartsWith(check_divider) == true)
                    {
                        //Set the sponsor to be before this index
                        check_team = player_name.Substring(0, i);
                        //Set the tag to be after the seperator at this index
                        check_name = player_name.Substring(i + 3);

                        //Verify that this is a seperator
                        if (MessageBox.Show("Does this player have the sponsor '" + check_team + "'?",
                            "Sponsor Name Detected", MessageBoxButtons.OKCancel)
                            == DialogResult.OK)
                        {
                            //Pass the sponsor and tag onto the actual variables
                            player_team = check_team;
                            player_name = check_name;
                        }
                        //Stop checking for the seperator
                        break;
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

            //This variable controls the slot that the enterred character gets input into.
            int overwrite_slot = 0;

            //Set the list of possible seperators between sponsor and tag
            string[] check_seperators = { " I ", " | ", " / ", @" \ " };
            foreach(string element in check_seperators)
            {
                //Test to see if each seperator is present in the tag and seperate the tag and sponsor appropriately
                check_for_sponsor(element, ref save_player.tag, ref save_player.sponsor);
            }

            //Check if a player in the roster has been selected from the combobox. 
            //Also ensure that the sheets integration is enabled
            if (global_values.player_roster_number[1] != -1 && global_values.enable_sheets == true &&
                txt_sheets.Text != "")
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
            else
            {
                save_player.character[0] = cbx_characters1.Text;
                save_player.color[0] = cbx_colors1.SelectedIndex + 1;
                save_player.region = "";
                for (int i = 1; i < 5; i ++)
                {
                    save_player.character[i] = "";
                    save_player.color[i] = 1;
                }
            }

            var player_info_box = new frm_save_player(save_player);
            player_info_box.ShowDialog();
            
            /*

            if (player_exists == false)
            {
                first_line = "You are about to create a new player record.";
                var region_box = new frm_replace_character();
                region_box.ShowDialog();
                region_line = "\nRegion: " + get_player_region;
            }
            else
            {
                first_line = "You are about to update a player's record.";
            }

            if (MessageBox.Show(first_line +
                                "\nPlayer Tag: " + player_name +
                                "\nTwitter Handle: " + twitter_handle +
                                region_line +
                                "\nSponsor: " + player_team +
                                "\nMain: " + main_character +
                                "\nColor: " + main_color.ToString(),
                                "Save New Player", MessageBoxButtons.OKCancel)
                                == DialogResult.OK)
            {
                player_info new_player = new player_info();
                new_player.tag = player_name;
                new_player.twitter = twitter_handle;
                new_player.sponsor = player_team;
                new_player.region = get_player_region;
                new_player.character[0] = main_character;
                new_player.character[1] = "";
                new_player.character[2] = "";
                new_player.character[3] = "";
                new_player.character[4] = "";
                new_player.color[0] = main_color;
                new_player.color[1] = 1;
                new_player.color[2] = 1;
                new_player.color[3] = 1;
                new_player.color[4] = 1;

                add_to_sheets(new_player);

                int hold_index = cbx_name2.SelectedIndex;

                cbx_name1.BeginUpdate();                                            //Begin
                cbx_name1.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name1.Items.Add(global_values.roster[i].tag);
                }
                cbx_name1.EndUpdate();                                              //End
                cbx_name1.SelectedIndex = cbx_name1.Items.IndexOf(player_name);     //Set the combobox index to 0

                cbx_name2.BeginUpdate();                                            //Begin
                cbx_name2.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name2.Items.Add(global_values.roster[i].tag);
                }
                cbx_name2.EndUpdate();                                              //End
                cbx_name2.SelectedIndex = hold_index;   //Set the combobox index to 0
            }
            */
        }

        private void btn_save2_Click(object sender, EventArgs e)
        {
            /*
            string player_name = cbx_name2.Text;
            string twitter_handle = txt_alt2.Text;
            string main_character = cbx_characters2.Text;
            int main_color = cbx_colors2.SelectedIndex + 1;
            string player_team = "";

            if (player_name.Contains(" I "))
            {
                string check_team = player_name;
                string check_name = player_name;

                for (int i = 0; i < check_team.Length; i++)
                {
                    if (check_team.Substring(i).StartsWith(" I ") == true)
                    {
                        check_team = player_name.Substring(0, i);
                        check_name = player_name.Substring(i + 3);

                        if (MessageBox.Show("Does this player have the sponsor '" + check_team + "'?",
                            "Sponsor Name Detected", MessageBoxButtons.OKCancel)
                            == DialogResult.OK)
                        {
                            player_team = check_team;
                            player_name = check_name;
                        }
                        break;
                    }
                }
            }

            bool player_exists = false;
            for (int i = 0; i < global_values.roster_size; i++)
            {
                if (player_name == global_values.roster[i].tag)
                {
                    player_exists = true;
                    break;
                }
            }

            string first_line = "";
            string region_line = "";

            if (player_exists == false)
            {
                first_line = "You are about to create a new player record.";
                var region_box = new frm_replace_character();
                region_box.ShowDialog();
                region_line = "\nRegion: " + get_player_region;
            }
            else
            {
                first_line = "You are about to update a player's record.";
            }

            if (MessageBox.Show(first_line +
                                "\nPlayer Tag: " + player_name +
                                "\nTwitter Handle: " + twitter_handle +
                                region_line +
                                "\nSponsor: " + player_team +
                                "\nMain: " + main_character +
                                "\nColor: " + main_color.ToString(),
                                "Save New Player", MessageBoxButtons.OKCancel)
                                == DialogResult.OK)
            {
                player_info new_player = new player_info();
                new_player.tag = player_name;
                new_player.twitter = twitter_handle;
                new_player.sponsor = player_team;
                new_player.region = get_player_region;
                new_player.character[0] = main_character;
                new_player.character[1] = "";
                new_player.character[2] = "";
                new_player.character[3] = "";
                new_player.character[4] = "";
                new_player.color[0] = main_color;
                new_player.color[1] = 1;
                new_player.color[2] = 1;
                new_player.color[3] = 1;
                new_player.color[4] = 1;

                add_to_sheets(new_player);

                int hold_index = cbx_name1.SelectedIndex;

                cbx_name1.BeginUpdate();                                            //Begin
                cbx_name1.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name1.Items.Add(global_values.roster[i].tag);
                }
                cbx_name1.EndUpdate();                                              //End
                cbx_name1.SelectedIndex = hold_index;                               //Set the combobox index to 0

                cbx_name2.BeginUpdate();                                            //Begin
                cbx_name2.Items.Clear();                                            //Empty the item list
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    cbx_name2.Items.Add(global_values.roster[i].tag);
                }
                cbx_name2.EndUpdate();                                              //End
                cbx_name2.SelectedIndex = cbx_name2.Items.IndexOf(player_name);     //Set the combobox index to 0
            }
            */
        }

        private void btn_score1_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image1 = openFileDialog3.FileName;
                pic_score1_image1.Image = Image.FromFile(global_values.score1_image1);
                btn_score1_image1.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player1-1").ReplaceWith(new XElement("player1-1", global_values.score1_image1));
                xml.Save(@"settings.xml");
            }
        }

        private void btn_score1_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image2 = openFileDialog3.FileName;
                pic_score1_image2.Image = Image.FromFile(global_values.score1_image2);
                btn_score1_image2.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player1-2").ReplaceWith(new XElement("player1-2", global_values.score1_image2));
                xml.Save(@"settings.xml");
            }
        }

        private void btn_score1_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score1_image3 = openFileDialog3.FileName;
                pic_score1_image3.Image = Image.FromFile(global_values.score1_image3);
                btn_score1_image3.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player1-3").ReplaceWith(new XElement("player1-3", global_values.score1_image3));
                xml.Save(@"settings.xml");
            }
        }

        private void btn_score2_image1_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image1 = openFileDialog3.FileName;
                pic_score2_image1.Image = Image.FromFile(global_values.score2_image1);
                btn_score2_image1.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player2-1").ReplaceWith(new XElement("player2-1", global_values.score2_image1));
                xml.Save(@"settings.xml");
            }
        }

        private void btn_score2_image2_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image2 = openFileDialog3.FileName;
                pic_score2_image2.Image = Image.FromFile(global_values.score2_image2);
                btn_score2_image2.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player2-2").ReplaceWith(new XElement("player2-2", global_values.score2_image2));
                xml.Save(@"settings.xml");
            }
        }

        private void btn_score2_image3_Click(object sender, EventArgs e)
        {
            if (openFileDialog3.ShowDialog() == DialogResult.OK)
            {
                global_values.score2_image3 = openFileDialog3.FileName;
                pic_score2_image3.Image = Image.FromFile(global_values.score2_image3);
                btn_score2_image3.BackColor = Color.Transparent;

                XDocument xml = XDocument.Load(@"settings.xml");
                xml.Root.Element("image-scoring").Element("player2-3").ReplaceWith(new XElement("player2-3", global_values.score2_image3));
                xml.Save(@"settings.xml");
            }
        }

        private void ckb_clipboard_CheckedChanged(object sender, EventArgs e)
        {
            //Set the value of the global value to reflect the checked status of this checkbox
            global_values.copy_video_title = ckb_clipboard.Checked;

            //Update the settings file
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("youtube").Element("copy-title").ReplaceWith(new XElement("copy-title", ckb_clipboard.Checked.ToString()));
            xml.Save(@"settings.xml");
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

            //Update the settings file
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("youtube").Element("enable-youtube").ReplaceWith(new XElement("enable-youtube", ckb_youtube.Checked.ToString()));
            xml.Save(@"settings.xml");
        }

        private void txt_description_TextChanged(object sender, EventArgs e)
        {
            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("youtube").Element("default-description").ReplaceWith(new XElement("default-description", txt_description.Text));
            xml.Save(@"settings.xml");
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

            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("google-sheets").Element("sheet-info").ReplaceWith(new XElement("sheet-info", global_values.sheets_info));
            xml.Save(@"settings.xml");
        }

        private void rdb_infoonly_CheckedChanged(object sender, EventArgs e)
        {
            global_values.sheets_info = "info-only";

            XDocument xml = XDocument.Load(@"settings.xml");
            xml.Root.Element("google-sheets").Element("sheet-info").ReplaceWith(new XElement("sheet-info", global_values.sheets_info));
            xml.Save(@"settings.xml");
        }

        private void btn_test_sheet_Click(object sender, EventArgs e)
        {
            btn_test_sheet.BackColor = Color.Transparent;
            txt_sheets.BackColor = Color.White;
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
                        if (ckb_loser1.Checked == false)
                        {
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
                        }
                        else
                        {
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text + " [L]");
                        }
                    }
                    break;
            }
        }

        private void ckb_loser1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_loser1.Checked == false)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text);
            }
            else
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", cbx_name1.Text + " [L]");
            }
        }

        private void ckb_loser2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_loser2.Checked == false)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text);
            }
            else
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", cbx_name2.Text + " [L]");
            }
        }
    }

    public static class global_values
    {
        public static int[] player_roster_number;
        public static string sheets_style;
        public static string sheets_info;
        public static string youtube_description;
        public static bool enable_youtube;
        public static bool copy_video_title;
        public static int roster_size;
        public static player_info[] roster;
        public static bool first_match = true;
        public static string reenable_upload = "";
        public static string stream_software = @"XSplit";
        public static string temp_file;
        public static bool allow_upload = true;
        public static string current_youtube_data;
        public static FileSystemWatcher vod_monitor;
        public static string new_vod_detected = "";
        public static string[] characters;
        public static string[] game_info;
        public static string score1_image1 = @"file";
        public static string score1_image2 = @"file";
        public static string score1_image3 = @"file";
        public static string score2_image1 = @"file";
        public static string score2_image2 = @"file";
        public static string score2_image3 = @"file";
        public static string game_path;
        public static string output_directory;
        public static string thumbnail_directory;
        public static string json_file;
        public static string youtube_username;
        public static string vods_directory;
        public static bool auto_update = true;
        public static int player_number;
        public static int[] player_image;
        public static string playlist_name;
        public static string playlist_id;
        public static bool enable_sheets;
    }
    

    public class player_info
    {
        public string tag;
        public string twitter;
        public string region;
        public string sponsor;
        public string[] character = new string[5];
        public int[] color = new int[5];
        public string get_display_name()
        {
            if(sponsor != "")
            {
                return sponsor + " I " + tag;
            }
            else
            {
                return tag;
            }
        }
    }
}
