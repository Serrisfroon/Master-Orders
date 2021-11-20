using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.Startup;
using Stream_Info_Handler.StreamAssistant.DataManagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeLibrary;

namespace Stream_Info_Handler.AppSettings.GeneralSettings
{
    public partial class GeneralSettingsForm : Form
    {
        public EditableSettings editableSettings = new EditableSettings();

        string[] characters;
        string playlist_id;
        string stream_software;
        string[] score_image = { "", "", "", "", "", "" };

        string image_directory1;
        string image_directory2;


        //Initialize the variables containing YouTube Playlist information
        List<string> playlist_items = new List<string>();
        List<string> playlist_names = new List<string>();

        public GeneralSettingsForm(FormManagement.FormNames originForm)
        {
            this.CenterToScreen();

            InitializeComponent();

            //Load stream queues from the database and add them to the combobox
            LoadSettingsFormControls.InitializeImageScoreControls();
            LoadSettingsFormControls.settingsForm = this;
            LoadSettingsFormControls.LoadStreamQueues(cbx_queues, StreamQueue.queueList);

            //Load the character rosters
            LoadSettingsFormControls.LoadGameTitles(cbx_characters, GlobalSettings.availableGames, "(Select a Game)");
            LoadSettingsFormControls.LoadGameTitles(cbx_queuegame, GlobalSettings.availableGames, "(Not Assigned)");

            //Load the settings
            LoadSettingsFormControls.LoadSettingsFields();

            {

                #region Tooltips
                /*
                 *             //Set tooltips for the Settings tab
            //
            //Set the tooltips for the Directories subtab
            /*
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

            ttp_tooltip.SetToolTip(btn_dashboard,
                "Open the Stream Queue Dashboard. The Google Sheets\n" +
                "integration must be enabled and set to use Info and\n" +
                "Queue to use this.");
            ttp_tooltip.SetToolTip(btn_addplayer,
                "Add a new player to the Google Sheet database. The\n" +
                "Google Sheets integration must be enabled to use this.");
              */
                #endregion Tooltips
            }

        }

        private void btn_font_Click(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;

            DialogResult newfont = ftd_thumbnail.ShowDialog();
            if (newfont == DialogResult.OK)
            {
                editableSettings.thumbnailFont = ftd_thumbnail.Font;
                lbl_font.Text = editableSettings.thumbnailFont.Name + " " + editableSettings.thumbnailFont.SizeInPoints + "pt " + editableSettings.thumbnailFont.Style.ToString();
            }
        }

        private void txt_characters_TextChanged(object sender, EventArgs e)
        {
            //Reset errors
            lbl_characters.Text = "";
            txt_characters.BackColor = Color.White;
            cbx_characters.Enabled = true;
            btn_apply.Enabled = true;

            //Verify that a directory has been provided

            if (txt_characters.Text != "")
            {
                if (Directory.Exists(txt_characters.Text))
                {
                    string[] folders;
                    try
                    {
                        folders = Directory.GetDirectories(txt_characters.Text);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        lbl_characters.Text = "You do not have access to this folder's contents.";
                        txt_characters.BackColor = EditableSettings.warningColor;
                        cbx_characters.Enabled = false;
                        return;
                    }
                    editableSettings.gameRosterDirectories = new List<string>();
                    foreach (string folder in folders)
                    {
                        editableSettings.gameRosterDirectories.Add(folder);
                    }
                }
                else
                {
                    lbl_characters.Text = "This directory does not exist.";
                    txt_characters.BackColor = EditableSettings.warningColor;
                    cbx_characters.Enabled = false;
                    return;
                }
            }
            else
            {
                //If a directory has not been provided, mark the field for an error and switch tabs to show it
                txt_characters.BackColor = EditableSettings.warningColor;
                cbx_characters.Enabled = false;
            }
        }

        private void btn_characters_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder containing the character roster
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_characters.Text = fbd_directory.SelectedPath;                            //Update the setting text
            }
        }

        private void cbx_characters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txt_characters.Text))
            {
                btn_reassign.Enabled = false;
                string checkGameDirectory = DirectoryManagement.VerifyGameDirectory(cbx_characters.Text, txt_characters.Text);
                if (checkGameDirectory != "")
                {
                    btn_reassign.Enabled = true;
                    characters = DirectoryManagement.GetCharactersFromDirectory(DirectoryManagement.GetGameDirectory());
                    btn_apply.Enabled = true;

                    if (txt_background.Text != "" && txt_foreground.Text != "" &&
                        txt_background.BackColor != EditableSettings.warningColor && txt_foreground.BackColor != EditableSettings.warningColor)
                        btn_preview.Enabled = true;

                    cbx_char1.BeginUpdate();                                      //Begin
                    cbx_char1.Items.Clear();                                      //Empty the item list
                    cbx_char2.BeginUpdate();                                      //Begin
                    cbx_char2.Items.Clear();                                      //Empty the item list     
                    int character_count = characters.Length;                //Store the number of characters
                                                                            //Loop through every character
                    for (int x = 0; x < character_count; x++)
                    {
                        cbx_char1.Items.Add(characters[x]);         //Add the character's name to the combobox
                        cbx_char2.Items.Add(characters[x]);         //Add the character's name to the combobox
                    }
                    cbx_char1.EndUpdate();                                        //End
                    cbx_char1.SelectedIndex = 0;                                  //Set the combobox index to 0
                    cbx_char2.EndUpdate();                                        //End
                    cbx_char2.SelectedIndex = 0;                                  //Set the combobox index to 0
                }
            }
        }

        private void txt_streamfiles_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_streamfiles.Text != @"")
            {
                if (Directory.Exists(txt_streamfiles.Text))
                {
                    txt_streamfiles.BackColor = Color.White;
                }
                else
                {
                    txt_streamfiles.BackColor = EditableSettings.warningColor;
                    lbl_directories.Text = "The directory enterred for Stream Files does not exist.";
                }
            }
            else
            {
                txt_streamfiles.BackColor = EditableSettings.warningColor;
                lbl_directories.Text = "Please provide a directory for Stream Files.";
            }
        }

        private void btn_streamfiles_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder to store stream files in
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_streamfiles.Text = fbd_directory.SelectedPath;                 //Update the global value with the new directory
            }
        }

        private void txt_thumbnails_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_thumbnails.Text != @"")
            {
                if (Directory.Exists(txt_thumbnails.Text))
                {
                    txt_thumbnails.BackColor = Color.White;
                }
                else
                {
                    txt_thumbnails.BackColor = EditableSettings.warningColor;
                    lbl_directories.Text = "The directory enterred for Thumbnails does not exist.";

                }
            }
            else
            {
                txt_thumbnails.BackColor = EditableSettings.warningColor;
                lbl_directories.Text = "Please provide a directory for Thumbnails.";
            }
        }

        private void btn_thumbnails_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder to store thumbnail images in
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_thumbnails.Text = fbd_directory.SelectedPath;                   //Update the setting with the new information
            }
        }

        private void txt_vods_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_vods.Text != @"")
            {
                if (Directory.Exists(txt_vods.Text))
                {
                    if (txt_vods.Text != DirectoryManagement.GetGameDirectory() &&
                    txt_vods.Text != txt_streamfiles.Text &&
                    txt_vods.Text != txt_thumbnails.Text &&
                    txt_vods.Text != txt_regions.Text &&
                    txt_vods.Text != txt_sponsors.Text)
                    {
                        txt_vods.BackColor = Color.White;
                    }
                    else
                    {
                        txt_vods.BackColor = EditableSettings.warningColor;
                        lbl_directories.Text = "The VoD directory cannot be the same as any other directory used by Master Orders. Choose a new directory.";
                    }
                }
                else
                {
                    txt_vods.BackColor = EditableSettings.warningColor;
                    lbl_directories.Text = "The directory enterred for VoDs does not exist.";
                }
            }
            else
            {
                txt_vods.BackColor = EditableSettings.warningColor;
                lbl_directories.Text = "Please provide a directory for VoDs.";
            }
        }

        private void btn_vods_Click(object sender, EventArgs e)
        {
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_vods.Text = fbd_directory.SelectedPath;
            }
        }

        private void txt_sponsors_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_sponsors.Text != @"")
            {
                if (Directory.Exists(txt_sponsors.Text))
                {
                    txt_sponsors.BackColor = Color.White;
                }
                else
                {
                    txt_sponsors.BackColor = EditableSettings.warningColor;
                    lbl_directories.Text = "The directory enterred for Sponsor images does not exist.";

                }
            }
            else
            {
                txt_sponsors.BackColor = EditableSettings.warningColor;
                lbl_directories.Text = "Please provide a directory for Sponsor images.";
            }
        }

        private void btn_sponsors_Click(object sender, EventArgs e)
        {
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_sponsors.Text = fbd_directory.SelectedPath;
            }
        }

        private void txt_regions_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_regions.Text != @"")
            {
                if (Directory.Exists(txt_regions.Text))
                {
                    txt_regions.BackColor = Color.White;
                }
                else
                {
                    txt_regions.BackColor = EditableSettings.warningColor;
                    lbl_directories.Text = "The directory enterred for Region images does not exist.";

                }
            }
            else
            {
                txt_regions.BackColor = EditableSettings.warningColor;
                lbl_directories.Text = "Please provide a directory for Region images.";
            }
        }

        private void btn_regions_Click(object sender, EventArgs e)
        {
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_regions.Text = fbd_directory.SelectedPath;
            }
        }

        private void ckb_sponsors_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void ckb_regions_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void ckb_vod_uploads_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        public void btn_oauth_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thead = new Thread(() =>
                {
                    authorize().Wait();
                });
                thead.IsBackground = true;
                thead.Start();

            }
            catch (TokenResponseException ex)
            {
                MessageBox.Show("There was an error refreshing the token. Try again, or try revoking the token. \r\n Detals: \r\n" + ex.Message);
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task authorize()
        {
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            string old_json = (string)xml.Root.Element("youtube").Element("json-file");

            await get_credential();


            await global_values.youtubeCredential.RefreshTokenAsync(CancellationToken.None);
            //await credential.RevokeTokenAsync(CancellationToken.None);

        }

        private void btn_revoke_oauth_Click(object sender, EventArgs e)
        {
            try
            {
                Thread thead = new Thread(() =>
                {
                    revoke().Wait();
                });
                thead.IsBackground = true;
                thead.Start();

            }
            catch (TokenResponseException ex)
            {
                MessageBox.Show("There was an error revoking the token. Try again, or there may be an issue with the credentials in the JSON file. \r\n Detals: \r\n" + ex.Message);
            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task revoke()
        {
            await get_credential();


            await global_values.youtubeCredential.RefreshTokenAsync(CancellationToken.None);

            await global_values.youtubeCredential.RevokeTokenAsync(CancellationToken.None);

            global_values.youtubeCredential = null;

        }

        private void txt_playlist_TextChanged(object sender, EventArgs e)
        {
            btn_playlist.Enabled = true;           
        }

        private void btn_playlist_Click(object sender, EventArgs e)
        {
            btn_playlist.Enabled = false;
            txt_playlist.Enabled = false;
            btn_apply.Enabled = true;
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

        private async Task get_playlists()
        {
            playlist_items = new List<string>();
            playlist_names = new List<string>();
            await get_credential();


            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = global_values.youtubeCredential,
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
                    playlist_id = playlist_items[playlist_names.IndexOf(txt_playlist.Text)];
                    MessageBox.Show("The playlist has been set to " + txt_playlist.Text + ". \n" +
                                    "The playlist ID is " + playlist_id);
                    txt_playlist.Enabled = true;
                }
                else
                {
                    if (txt_playlist.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        playlist_id = "";
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

        private async Task add_playlist(string new_playlist)
        {
            await get_credential();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = global_values.youtubeCredential,
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

        private void rdb_xsplit_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            stream_software = @"XSplit";
        }

        private void rdb_obs_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            stream_software = @"OBS";
        }

        private void rdb_automatic_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void ckb_scoreboad_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            btn_score1_image1.Enabled = ckb_scoreboad.Checked;
            btn_score1_image2.Enabled = ckb_scoreboad.Checked;
            btn_score1_image3.Enabled = ckb_scoreboad.Checked;
            btn_score2_image1.Enabled = ckb_scoreboad.Checked;
            btn_score2_image2.Enabled = ckb_scoreboad.Checked;
            btn_score2_image3.Enabled = ckb_scoreboad.Checked;
        }

        private void score_button_Click(object sender, EventArgs e)
        {
            Button clickedImageButton = (Button)sender;
            if (ofd_png.ShowDialog() == DialogResult.OK)
            {
                btn_apply.Enabled = true;
                int scoreControlIndex = (int)(clickedImageButton.Tag);
                editableSettings.scoreControls[scoreControlIndex].UpdateImage(ofd_png.FileName);
            }
        }

        private void tab_stream_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_stream_tabs.SelectedIndex != 3)
                return;

        }

        private void txt_background_TextChanged(object sender, EventArgs e)
        {
            txt_background.BackColor = EditableSettings.warningColor;
            btn_preview.Enabled = false;
            if (txt_background.Text != @"")
            {
                if (File.Exists(txt_background.Text))
                {
                    if (Path.GetExtension(txt_background.Text) == ".jpg")
                    {
                        btn_apply.Enabled = true;
                        txt_background.BackColor = Color.White;
                        if (txt_foreground.Text != "" && txt_foreground.BackColor != EditableSettings.warningColor &&
                            cbx_characters.BackColor != EditableSettings.warningColor && cbx_characters.Text != "")
                            btn_preview.Enabled = true;
                    }
                    else
                    {
                        txt_background.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txt_background.BackColor = EditableSettings.warningColor;
                }
            }
        }

        private void btn_background_Click(object sender, EventArgs e)
        {
            if (ofd_jpg.ShowDialog() == DialogResult.OK)
            {
                txt_background.Text = ofd_jpg.FileName;
            }
        }

        private void txt_foreground_TextChanged(object sender, EventArgs e)
        {
            txt_foreground.BackColor = EditableSettings.warningColor;
            btn_preview.Enabled = false;

            if (txt_foreground.Text != @"")
            {
                if (File.Exists(txt_foreground.Text))
                {
                    if (Path.GetExtension(txt_foreground.Text) == ".png")
                    {
                        btn_apply.Enabled = true;
                        txt_foreground.BackColor = Color.White;
                        if (txt_background.Text != "" && txt_background.BackColor != EditableSettings.warningColor &&
                            cbx_characters.BackColor != EditableSettings.warningColor && cbx_characters.Text != "")
                            btn_preview.Enabled = true;
                    }
                    else
                    {
                        txt_foreground.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txt_foreground.BackColor = EditableSettings.warningColor;
                }
            }
        }

        private void btn_foreground_Click(object sender, EventArgs e)
        {
            if (ofd_png.ShowDialog() == DialogResult.OK)
            {
                txt_foreground.Text = ofd_png.FileName;
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

            btn_apply.Enabled = true;
        }

        private void numeric_FocusLeave(object sender, KeyPressEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == @"" ||
                textBox.Text == @"-")
            {
                textBox.Text = "0";
            }
        }

        public void create_thumbnail()
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
            Image background = Image.FromFile(txt_background.Text);
            Image foreground = Image.FromFile(txt_foreground.Text);

            //Create an image resource for each player's character        
            Image left_character = Image.FromFile(image_directory1 + @"\1080.png");
            Image right_character = Image.FromFile(image_directory2 + @"\1080.png");
            right_character.RotateFlip(RotateFlipType.RotateNoneFlipX);

            drawing.Clear(Color.White);                                         //Clear the surface of all data

            drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

            drawing.DrawImage(left_character, 0 + Int32.Parse(txt_char1_xoffset.Text), 0 + Int32.Parse(txt_char1_yoffset.Text), 1920, 1080);                //Draw Player 1's character
            drawing.DrawImage(right_character, 0 + Int32.Parse(txt_char2_xoffset.Text), 0 + Int32.Parse(txt_char2_yoffset.Text), 1920, 1080);               //Draw Player 2's character

            drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters


            //Convert each player's name and the round in bracket to all capital letters and store them seperately
            string player_name1 = txt_name1.Text.ToUpper();
            string player_name2 = txt_name2.Text.ToUpper();
            string round_text = "WINNERS QUARTERFINALS";
            string match_date = DateTime.Today.ToString("M/dd/yy");

            //Create a drawing path for the text of the date, each player's name, and the round in bracket
            GraphicsPath draw_date = new GraphicsPath();
            GraphicsPath draw_name1 = new GraphicsPath();
            GraphicsPath draw_name2 = new GraphicsPath();
            GraphicsPath draw_round = new GraphicsPath();
            GraphicsPath draw_patch = new GraphicsPath();
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
            FontFamily keepcalm = editableSettings.thumbnailFont.FontFamily;
            Font calmfont = editableSettings.thumbnailFont;

            int font_size = Int32.Parse(txt_name1_size.Text);                   //Create a variable for adjustable font size
            Size namesize = TextRenderer.MeasureText(player_name1, calmfont);   //Create a variable to hold the size of player tags

            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font(editableSettings.thumbnailFont.FontFamily, font_size, FontStyle.Regular);     //Create a new font with this new size
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
                new Point(420 + Int32.Parse(txt_name1_xoffset.Text), 160 + Int32.Parse(txt_name1_yoffset.Text)),          //110                                  //drawing location 480
                text_center);                                                   //text alignment
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name1);
            drawing.FillPath(white_text, draw_name1);

            font_size = Int32.Parse(txt_name2_size.Text); ;                      //115                                      //Reset the font size
            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font(editableSettings.thumbnailFont.FontFamily, font_size, FontStyle.Regular);     //Create a new font with this new size
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
                new Point(1500 + Int32.Parse(txt_name2_xoffset.Text), 160 + Int32.Parse(txt_name2_yoffset.Text)), //110                                          //drawing location 1440
                text_center);                                                   //text alignment                                        // text to draw
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name2);
            drawing.FillPath(white_text, draw_name2);


            //Add the round in bracket to its drawing path
            draw_round.AddString(
                round_text,                                                      //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                Int32.Parse(txt_round_size.Text),                               //font size (drawing.DpiY * 120 / 72)
                new Point(960 + Int32.Parse(txt_round_xoffset.Text), 720 + Int32.Parse(txt_round_yoffset.Text)), //620                                      //drawing location
                text_center);                                                   //text alignment     
                                                                                //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(light_stroke, draw_round);
            drawing.FillPath(white_text, draw_round);

            if (ckb_date.Checked == true)
            {
                //Add the date to its drawing path
                draw_date.AddString(
                    match_date,                                                     //text to draw
                    keepcalm,                                                       //font family
                    (int)FontStyle.Regular,                                         //font style
                    Int32.Parse(txt_date_size.Text),                                //font size (drawing.DpiY * 120 / 72)
                    new Point(300 + Int32.Parse(txt_date_xoffset.Text), 940 + Int32.Parse(txt_date_yoffset.Text)),                                            //drawing location
                    text_center);                                                   //text alignment
                                                                                    //Set the outline and filling to the appropriate colors
                drawing.DrawPath(black_stroke, draw_date);
                drawing.FillPath(white_text, draw_date);
            }

            draw_patch.AddString(
                txt_version.Text,                                                     //text to draw
                keepcalm,                                                         //font family
                (int)FontStyle.Regular,                                           //font style
                Int32.Parse(txt_patch_size.Text),                                 //font size (drawing.DpiY * 120 / 72)
                new Point(300 + Int32.Parse(txt_patch_xoffset.Text), 1020 + Int32.Parse(txt_patch_yoffset.Text)), //620                                        //drawing location
                text_center);                                                     //text alignment     
                                                                                  //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(light_stroke, draw_patch);
            drawing.FillPath(white_text, draw_patch);

            //Save the drawing surface back to the bitmap image
            drawing.Save();
            //Dispose the drawing surface
            drawing.Dispose();

            //Return the title of the image file
            pic_thumbnail.BackgroundImage = thumbnail_bmp;
        }

        private void cbx_char1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = DirectoryManagement.GetGameDirectory() + @"\" + cbx_char1.Text + @"\1\";
        }

        private void cbx_char2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = DirectoryManagement.GetGameDirectory() + @"\" + cbx_char2.Text + @"\1\";
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            create_thumbnail();
        }

        private void checkbox_Changed(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void ckb_thumbnails_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void ckb_clipboard_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void txt_description_TextChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (save_settings() == true)
                btn_apply.Enabled = false;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (save_settings() == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool save_settings()
        {
            //Check all settings to ensure there are no conflicts, and ensure all needed fields are provided
            {
                if (txt_characters.BackColor == EditableSettings.warningColor ||
                    txt_characters.Text == "")
                {
                    MessageBox.Show("The provided directory for Character Databases is incorrect. Please correct this under the General settings.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (ckb_scoreboad.Checked == true && (
                    score_image[0] == "" || score_image[1] == "" ||
                    score_image[2] == "" || score_image[3] == "" ||
                    score_image[4] == "" || score_image[5] == ""))
                {
                    if (MessageBox.Show("Image Scoreboard is enabled, but not all images have been provided. Click OK to disable Image Scoreboard, or click Cancel to cancel saving changes and add the needed images.", "Image Files Missing", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckb_scoreboad.Checked = false;
                    else
                        return false;
                }
                if (ckb_thumbnails.Checked == true &&
                    (!(File.Exists(txt_background.Text) &&
                    File.Exists(txt_foreground.Text))))

                {
                    if (MessageBox.Show("Thumbnail Generation is enabled, but a working path to a background and/or foreground image has not been provided. Click OK to disable Thumbnail Generation, or click Cancel to cancel saving changes and add a working path to a background and/or foreground image.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckb_thumbnails.Checked = false;
                    else
                        return false;
                }
                if (ckb_sponsors.Checked == true && (
                    txt_sponsors.Text == "" ||
                    txt_sponsors.BackColor == EditableSettings.warningColor))
                {
                    if (MessageBox.Show("Sponsor Images are enabled, but a working directory has not been provided. Click OK to disable Sponsor Images, or click Cancel to cancel saving changes and add a working directory.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckb_sponsors.Checked = false;
                    else
                        return false;
                }
                if (ckb_regions.Checked == true && (
                    txt_regions.Text == "" ||
                    txt_regions.BackColor == EditableSettings.warningColor))
                {
                    if (MessageBox.Show("Region Images are enabled, but a working directory has not been provided. Click OK to disable Region Images, or click Cancel to cancel saving changes and add a working directory.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckb_regions.Checked = false;
                    else
                        return false;
                }
                if (txt_vods.BackColor == EditableSettings.warningColor)
                {
                    MessageBox.Show("The provided directory for VoDs is invalid. Please correct this under the Stream Assistant settings' Directories tab. Keep in mind that the VoDs directory cannot be the same as any other directory used by Master Orders.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //DID THIS RECENTLY LOL
                ////if (save_checking == 1)
                {
                    if (txt_streamfiles.BackColor == EditableSettings.warningColor ||
                        txt_streamfiles.Text == "")
                    {
                        MessageBox.Show("The provided directory for Stream Files is invalid. Please correct this under the Stream Assistant settings' Directories tab.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (txt_thumbnails.BackColor == EditableSettings.warningColor ||
                        txt_thumbnails.Text == "")
                    {
                        MessageBox.Show("The provided directory for Thumbnails is invalid. Please correct this under the Stream Assistant settings' Directories tab.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }

                if(cbx_queuegame.Text != cbx_characters.Text && cbx_queues.Text != "None")
                {
                    if (MessageBox.Show("The selected character roster does not match the game that the selected queue is set to use. Master Orders will need to match the queue's game to the selected roster. Okay to Proceed?", "Roster Game Mismatch", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                       == DialogResult.OK)
                    {
                        int queueidd = -1;
                        for (int i = 0; i < StreamQueue.queueList.Count; i++)
                        {
                            if (StreamQueue.queueList[i].name == cbx_queues.Text)
                            {
                                queueidd = i;
                            }
                        }
                        if (database_tools.regame_queue(cbx_queues.Text, cbx_characters.Text, queueidd) == false)
                            return false;
                    }
                    else
                        return false;
                }

                if (txt_streamfiles.BackColor == EditableSettings.warningColor)
                {
                    MessageBox.Show("The provided Bracket Rounds File is invalid. Select a valid file or clear the textbox to use the default rounds.", "Invalid File Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            int queueid = -1;
            for (int i = 0; i < StreamQueue.queueList.Count; i++)
            {
                if (StreamQueue.queueList[i].name == cbx_queues.Text)
                {
                    queueid = i;
                }
            }

            //Apply all changes to the settings file
            {
                XDocument xml = XDocument.Load(SettingsFile.settingsFile);


                xml.Root.Element("database").Element("queue-id").ReplaceWith(new XElement("queue-id", queueid));
                xml.Root.Element("database").Element("game-id").ReplaceWith(new XElement("game-id", cbx_characters.Text));

                xml.Root.Element("directories").Element("character-directory").ReplaceWith(new XElement("character-directory", txt_characters.Text));
                xml.Root.Element("directories").Element("stream-directory").ReplaceWith(new XElement("stream-directory", txt_streamfiles.Text));
                xml.Root.Element("directories").Element("thumbnail-directory").ReplaceWith(new XElement("thumbnail-directory", txt_thumbnails.Text));
                xml.Root.Element("directories").Element("vods-directory").ReplaceWith(new XElement("vods-directory", txt_vods.Text));
                xml.Root.Element("directories").Element("enable-sponsor").ReplaceWith(new XElement("enable-sponsor", ckb_sponsors.Checked));
                xml.Root.Element("directories").Element("enable-region").ReplaceWith(new XElement("enable-region", ckb_regions.Checked));
                xml.Root.Element("directories").Element("sponsor-directory").ReplaceWith(new XElement("sponsor-directory", txt_sponsors.Text));
                xml.Root.Element("directories").Element("region-directory").ReplaceWith(new XElement("region-directory", txt_regions.Text));

                xml.Root.Element("youtube").Element("enable-youtube").ReplaceWith(new XElement("enable-youtube", ckb_vod_uploads.Checked));
                xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", txt_playlist.Text));
                xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", playlist_id));
                xml.Root.Element("youtube").Element("default-description").ReplaceWith(new XElement("default-description", txt_description.Text));
                xml.Root.Element("youtube").Element("tags").ReplaceWith(new XElement("tags", txt_tags.Text));
                xml.Root.Element("youtube").Element("title-template").ReplaceWith(new XElement("title-template", txt_titletemplate.Text));

                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", ckb_scoreboad.Checked));
                xml.Root.Element("image-scoring").Element("player1-1").ReplaceWith(new XElement("player1-1", score_image[0]));
                xml.Root.Element("image-scoring").Element("player1-2").ReplaceWith(new XElement("player1-2", score_image[1]));
                xml.Root.Element("image-scoring").Element("player1-3").ReplaceWith(new XElement("player1-3", score_image[2]));
                xml.Root.Element("image-scoring").Element("player2-1").ReplaceWith(new XElement("player2-1", score_image[3]));
                xml.Root.Element("image-scoring").Element("player2-2").ReplaceWith(new XElement("player2-2", score_image[4]));
                xml.Root.Element("image-scoring").Element("player2-3").ReplaceWith(new XElement("player2-3", score_image[5]));

                xml.Root.Element("thumbnail-layout").Element("background-image").ReplaceWith(new XElement("background-image", txt_background.Text));
                xml.Root.Element("thumbnail-layout").Element("foreground-image").ReplaceWith(new XElement("foreground-image", txt_foreground.Text));
                xml.Root.Element("thumbnail-layout").Element("font").ReplaceWith(new XElement("font", editableSettings.thumbnailFont.Name));
                xml.Root.Element("thumbnail-layout").Element("character-1-xoffset").ReplaceWith(new XElement("character-1-xoffset", txt_char1_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("character-1-yoffset").ReplaceWith(new XElement("character-1-yoffset", txt_char1_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("character-2-xoffset").ReplaceWith(new XElement("character-2-xoffset", txt_char2_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("character-2-yoffset").ReplaceWith(new XElement("character-2-yoffset", txt_char2_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("name-1-xoffset").ReplaceWith(new XElement("name-1-xoffset", txt_name1_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("name-1-yoffset").ReplaceWith(new XElement("name-1-yoffset", txt_name1_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("name-1-size").ReplaceWith(new XElement("name-1-size", txt_name1_size.Text));
                xml.Root.Element("thumbnail-layout").Element("name-2-xoffset").ReplaceWith(new XElement("name-2-xoffset", txt_name2_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("name-2-yoffset").ReplaceWith(new XElement("name-2-yoffset", txt_name2_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("name-2-size").ReplaceWith(new XElement("name-2-size", txt_name2_size.Text));
                xml.Root.Element("thumbnail-layout").Element("round-xoffset").ReplaceWith(new XElement("round-xoffset", txt_round_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("round-yoffset").ReplaceWith(new XElement("round-yoffset", txt_round_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("round-size").ReplaceWith(new XElement("round-size", txt_round_size.Text));
                xml.Root.Element("thumbnail-layout").Element("enable-date").ReplaceWith(new XElement("enable-date", ckb_date.Checked));
                xml.Root.Element("thumbnail-layout").Element("date-xoffset").ReplaceWith(new XElement("date-xoffset", txt_date_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("date-yoffset").ReplaceWith(new XElement("date-yoffset", txt_date_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("date-size").ReplaceWith(new XElement("date-size", txt_date_size.Text));
                xml.Root.Element("thumbnail-layout").Element("patch-version").ReplaceWith(new XElement("patch-version", txt_version.Text));
                xml.Root.Element("thumbnail-layout").Element("patch-xoffset").ReplaceWith(new XElement("patch-xoffset", txt_patch_xoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("patch-yoffset").ReplaceWith(new XElement("patch-yoffset", txt_patch_yoffset.Text));
                xml.Root.Element("thumbnail-layout").Element("patch-size").ReplaceWith(new XElement("patch-size", txt_patch_size.Text));

                xml.Root.Element("general").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", rdb_automatic.Checked));
                xml.Root.Element("general").Element("stream-software").ReplaceWith(new XElement("stream-software", stream_software));
                xml.Root.Element("general").Element("enable-thumbnails").ReplaceWith(new XElement("enable-thumbnails", ckb_thumbnails.Checked));
                xml.Root.Element("general").Element("copy-title").ReplaceWith(new XElement("copy-title", ckb_clipboard.Checked));
                xml.Root.Element("general").Element("shorten-title").ReplaceWith(new XElement("shorten-title", cbx_shorten_video.SelectedIndex));
                xml.Root.Element("general").Element("format").ReplaceWith(new XElement("format", cbx_format.Text));
                xml.Root.Element("general").Element("keep-on-top").ReplaceWith(new XElement("keep-on-top", ckb_ontop.Checked));
                xml.Root.Element("general").Element("rounds-file").ReplaceWith(new XElement("rounds-file", txt_bracketrounds.Text));
                xml.Root.Element("general").Element("sponsor-seperator").ReplaceWith(new XElement("sponsor-seperator", txt_seperator.Text));


                xml.Save(SettingsFile.settingsFile);
            }

            //Apply all changes to the global values
            {
                ImageManagement.enableRegionImages = ckb_regions.Checked;
                DirectoryManagement.regionDirectory = txt_regions.Text;
                ImageManagement.enableSponsorImages = ckb_sponsors.Checked;
                DirectoryManagement.sponsorDirectory = txt_sponsors.Text; ;
                YoutubeLibrary.YoutubeController.enableYoutubeFunctions = ckb_vod_uploads.Checked;
                YoutubeController.enableVideoThumbnails = ckb_thumbnails.Checked;
                YoutubeController.copyVideoTitle = ckb_clipboard.Checked;
                YoutubeLibrary.YoutubeController.streamSoftware = stream_software;
                ImageManagement.enableImageScoreboard = ckb_scoreboad.Checked;
                ImageManagement.scoreboardImages[0, 0] = score_image[0];
                ImageManagement.scoreboardImages[0, 1] = score_image[1];
                ImageManagement.scoreboardImages[0, 2] = score_image[2];
                ImageManagement.scoreboardImages[1, 0] = score_image[3];
                ImageManagement.scoreboardImages[1, 1] = score_image[4];
                ImageManagement.scoreboardImages[1, 2] = score_image[5];
                DirectoryManagement.outputDirectory = txt_streamfiles.Text;
                DirectoryManagement.thumbnailDirectory = txt_thumbnails.Text;
                DirectoryManagement.vodsDirectory = txt_vods.Text;
                DataOutputCaller.automaticUpdates = rdb_automatic.Checked;
                YoutubeController.playlistName = txt_playlist.Text;
                YoutubeController.playlistId = playlist_id;
                global_values.queue_id = queueid;
                GlobalSettings.selectedGame = cbx_characters.Text;
            }

            return true;
        }

        private void cbx_queues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            string queuename = "";
            for (int i = 0; i < StreamQueue.queueList.Count; i++)
            {
                if (StreamQueue.queueList[i].name == cbx_queues.Text)
                    queuename = StreamQueue.queueList[i].game;
            }
            cbx_queuegame.SelectedIndex = cbx_queuegame.FindStringExact(queuename);
        }

        private async Task get_credential()
        {
            if (global_values.youtubeCredential == null)
                using (var stream = new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
                {
                    global_values.youtubeCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        GoogleClientSecrets.Load(stream).Secrets,
                        // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                        // user's account, but not other types of account access.
                        new[] { YouTubeService.Scope.Youtube,
                        YouTubeService.Scope.YoutubeUpload },
                        global_values.youtube_username,
                        CancellationToken.None,
                        global_values.store
                    );
                }
        }

        private void ckb_ontop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ((CheckBox)sender).Checked;
        }

        private void txt_version_KeyPress(object sender, KeyPressEventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void btn_queue_rename_Click(object sender, EventArgs e)
        {
            int holdindex = cbx_queues.SelectedIndex;
            if (holdindex == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            frm_rename_queue rename = new frm_rename_queue(cbx_queues.SelectedIndex, this.TopMost);
            if (rename.ShowDialog() == DialogResult.OK)
            {
                cbx_queues.BeginUpdate();
                cbx_queues.Items.Clear();                                            //Empty the item list
                cbx_queues.Items.Add("None");
                for (int i = 0; i < StreamQueue.queueList.Count; i++)
                {
                    cbx_queues.Items.Add(StreamQueue.queueList[i].name);
                }
                cbx_queues.EndUpdate();
            }
            cbx_queues.SelectedIndex = holdindex;
        }

        private void btn_reassign_Click(object sender, EventArgs e)
        {
            if (cbx_characters.Text == "")
            {
                SystemSounds.Beep.Play();
                return;
            }

            //Find the ID of the selected game
            string selectedGameName = cbx_characters.Text;
            string[] character_directories = Directory.GetDirectories(txt_characters.Text);

            //Determine the selected game's directory
            string selected_directory = DirectoryManagement.gameDirectories[selectedGameName];

            //Show a window to select a new directory
            frm_tables selection = new frm_tables(character_directories, cbx_characters.Text);
            //Return fail if a new directory is not selected
            if (selection.ShowDialog() != DialogResult.OK)
                return;
            //Update the game directory
            selected_directory = database_tools.pass_directory;


            //Verify the directory has correct character data
            string[] characters = DirectoryManagement.GetCharactersFromDirectory(selected_directory);
            //Continually check to verify the character
            while (!DirectoryManagement.CheckCharacterDirectories(selected_directory))
            {
                //If the characters are not verified, have the user choose a new directory
                MessageBox.Show("The selected directory does not have correct character information for the selected game. Please choose a new directory for " + selectedGameName);
                frm_tables selectioncheck = new frm_tables(character_directories, selectedGameName);
                //Return fail if a new directory is not selected
                if (selectioncheck.ShowDialog() != DialogResult.OK)
                    return;
                //Update the game directory
                selected_directory = database_tools.pass_directory;
                //Update the character list before checking again
                characters = DirectoryManagement.GetCharactersFromDirectory(selected_directory);
            }

            DirectoryManagement.gameDirectories[selectedGameName] = selected_directory;

            //Update the settings to include the new directory
            SettingsFile.UpdateGameDirectories();
        }

        private void cbx_queuegame_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queuegame = "";
            int queueid = -1;
            for (int i = 0; i < StreamQueue.queueList.Count; i++)
            {
                if (StreamQueue.queueList[i].name == cbx_queues.Text)
                {
                    queuegame = StreamQueue.queueList[i].game;
                    queueid = i;
                }
            }
            //Only update the queue if this is a new game
            if (cbx_queuegame.Text != queuegame)
            {
                if (database_tools.regame_queue(cbx_queues.Text, cbx_queuegame.Text, queueid) == false)
                    cbx_queuegame.SelectedIndex = cbx_queuegame.FindStringExact(queuegame);
            }
        }

 
        private void txt_seperator_TextChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void btn_bracketrounds_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder containing the character roster
            if (ofd_txt.ShowDialog() == DialogResult.OK)
            {
                txt_bracketrounds.Text = ofd_txt.FileName;            
            }
        }

        private void txt_bracketrounds_TextChanged(object sender, EventArgs e)
        {
            txt_bracketrounds.BackColor = EditableSettings.warningColor;
            if (txt_bracketrounds.Text != @"")
            {
                if (File.Exists(txt_bracketrounds.Text))
                {
                    if (Path.GetExtension(txt_bracketrounds.Text) == ".txt")
                    {
                        txt_bracketrounds.BackColor = Color.White;
                    }
                    else
                    {
                        txt_bracketrounds.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txt_bracketrounds.BackColor = EditableSettings.warningColor;
                }
            }
            else
                txt_bracketrounds.BackColor = Color.White;
        }

        private void Txt_titletemplate_TextChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }
    }
}
