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

        string[] characterNames;
        string playlistId;
        string selectedStreamSoftware;
        string[] score_image = { "", "", "", "", "", "" };

        string image_directory1;
        string image_directory2;


        //Initialize the variables containing YouTube Playlist information
        List<string> playerlistItems = new List<string>();
        List<string> playlistNames = new List<string>();

        public GeneralSettingsForm(FormManagement.FormNames originForm)
        {
            this.CenterToScreen();

            InitializeComponent();

            //Load stream queues from the database and add them to the combobox
            LoadSettingsFormControls.InitializeImageScoreControls();
            LoadSettingsFormControls.settingsForm = this;
            LoadSettingsFormControls.LoadStreamQueues(cbxStreamQueues, StreamQueueManager.queueList);

            //Load the character rosters
            LoadSettingsFormControls.LoadGameTitles(cbxCharacterRosters, GlobalSettings.availableGames, "(Select a Game)");
            LoadSettingsFormControls.LoadGameTitles(cbxAssignedStreamQueueGame, GlobalSettings.availableGames, "(Not Assigned)");

            //Load the settings
            LoadSettingsFormControls.LoadSettingsFields();
            LoadToolTips.Initialize(this);
        }

        #region General

        private void txtCharacterDatabasesDirectory_TextChanged(object sender, EventArgs e)
        {
            //Reset errors
            lblCharacterErrors.Text = "";
            txtCharacterDatabasesDirectory.BackColor = Color.White;
            cbxCharacterRosters.Enabled = true;
            btnApplyChanges.Enabled = true;

            //Verify that a directory has been provided

            if (txtCharacterDatabasesDirectory.Text != "")
            {
                if (Directory.Exists(txtCharacterDatabasesDirectory.Text))
                {
                    string[] gameFolders;
                    try
                    {
                        gameFolders = Directory.GetDirectories(txtCharacterDatabasesDirectory.Text);
                    }
                    catch (UnauthorizedAccessException)
                    {
                        editableSettings.DisplaySettingsError("You do not have access to this folder's contents.", lblCharacterErrors, txtCharacterDatabasesDirectory, EditableSettings.ErrorType.characterError, cbxCharacterRosters);
                        return;
                    }
                    editableSettings.gameRosterDirectories = new List<string>();
                    foreach (string gameFolder in gameFolders)
                    {
                        editableSettings.gameRosterDirectories.Add(gameFolder);
                    }
                }
                else
                {
                    editableSettings.DisplaySettingsError("This directory does not exist.", lblCharacterErrors, txtCharacterDatabasesDirectory, EditableSettings.ErrorType.characterError, cbxCharacterRosters);
                    return;
                }
            }
            else
            {
                editableSettings.DisplaySettingsError("A directory must be provided for Character Databases.", lblCharacterErrors, txtCharacterDatabasesDirectory, EditableSettings.ErrorType.characterError, cbxCharacterRosters);
            }
        }
        private void btnBrowseCharacterRostersDirectory_Click(object sender, EventArgs e)
        {
            editableSettings.SelectDirectory(fbdBrowserForDirectory, txtCharacterDatabasesDirectory);
        }
        private void cbxCharacterRosters_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Directory.Exists(txtCharacterDatabasesDirectory.Text))
            {
                btnReassignCharacterDirectory.Enabled = false;
                if (DirectoryManagement.VerifyGameDirectory(cbxCharacterRosters.Text))
                {
                    btnReassignCharacterDirectory.Enabled = true;
                    btnApplyChanges.Enabled = true;
                    btnPreviewThumbnail.Enabled = VerifyPreviewIsReady();

                    characterNames = DirectoryManagement.GetCharactersFromDirectory(DirectoryManagement.GetGameDirectory());

                    editableSettings.UpdateCharacterComboBox(cbxThumbnailPreviewCharacter1, characterNames);
                    editableSettings.UpdateCharacterComboBox(cbxThumbnailPreviewCharacter2, characterNames);
                }
                else
                {
                    cbxCharacterRosters.SelectedIndex = -1;
                }
            }
        }
        private void cbxAssignedStreamQueueGame_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queueGame = "";
            int queueId = -1;
            for (int i = 0; i < StreamQueueManager.queueList.Count; i++)
            {
                if (StreamQueueManager.queueList[i].queueName == cbxStreamQueues.Text)
                {
                    queueGame = StreamQueueManager.queueList[i].queueGame;
                    queueId = i;
                }
            }
            //Only update the queue if this is a new game
            if (cbxAssignedStreamQueueGame.Text != queueGame)
            {
                if (database_tools.regame_queue(cbxStreamQueues.Text, cbxAssignedStreamQueueGame.Text, queueId) == false)
                    cbxAssignedStreamQueueGame.SelectedIndex = cbxAssignedStreamQueueGame.FindStringExact(queueGame);
            }
        }

        private void btnBracketRoundsFile_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder containing the character roster
            if (ofdBrowseForTxt.ShowDialog() == DialogResult.OK)
            {
                txtBracketRoundsFile.Text = ofdBrowseForTxt.FileName;
            }
        }

        private void txtBracketRoundsFile_TextChanged(object sender, EventArgs e)
        {
            txtBracketRoundsFile.BackColor = EditableSettings.warningColor;
            if (txtBracketRoundsFile.Text != @"")
            {
                if (File.Exists(txtBracketRoundsFile.Text))
                {
                    if (Path.GetExtension(txtBracketRoundsFile.Text) == ".txt")
                    {
                        txtBracketRoundsFile.BackColor = Color.White;
                    }
                    else
                    {
                        txtBracketRoundsFile.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txtBracketRoundsFile.BackColor = EditableSettings.warningColor;
                }
            }
            else
                txtBracketRoundsFile.BackColor = Color.White;
        }
        private void ckbKeepWindowOnTop_CheckedChanged(object sender, EventArgs e)
        {
            this.TopMost = ((CheckBox)sender).Checked;
        }
        private void btnRenameStreamQueue_Click(object sender, EventArgs e)
        {
            int holdindex = cbxStreamQueues.SelectedIndex;
            if (holdindex == 0)
            {
                SystemSounds.Beep.Play();
                return;
            }

            frm_rename_queue rename = new frm_rename_queue(cbxStreamQueues.SelectedIndex, this.TopMost);
            if (rename.ShowDialog() == DialogResult.OK)
            {
                cbxStreamQueues.BeginUpdate();
                cbxStreamQueues.Items.Clear();                                            //Empty the item list
                cbxStreamQueues.Items.Add("None");
                for (int i = 0; i < StreamQueueManager.queueList.Count; i++)
                {
                    cbxStreamQueues.Items.Add(StreamQueueManager.queueList[i].queueName);
                }
                cbxStreamQueues.EndUpdate();
            }
            cbxStreamQueues.SelectedIndex = holdindex;
        }

        private void btnReassignCharacterDirectory_Click(object sender, EventArgs e)
        {
            if (cbxCharacterRosters.Text == "")
            {
                SystemSounds.Beep.Play();
                return;
            }

            //Find the ID of the selected game
            string selectedGameName = cbxCharacterRosters.Text;
            string[] character_directories = Directory.GetDirectories(txtCharacterDatabasesDirectory.Text);

            //Determine the selected game's directory
            string selected_directory = DirectoryManagement.gameDirectories[selectedGameName];

            //Show a window to select a new directory
            RosterDirectorySelectForm selection = new RosterDirectorySelectForm(character_directories, cbxCharacterRosters.Text);
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
                RosterDirectorySelectForm selectioncheck = new RosterDirectorySelectForm(character_directories, selectedGameName);
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
        private void cbxStreamQueues_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
            string queuename = "";
            for (int i = 0; i < StreamQueueManager.queueList.Count; i++)
            {
                if (StreamQueueManager.queueList[i].queueName == cbxStreamQueues.Text)
                    queuename = StreamQueueManager.queueList[i].queueGame;
            }
            cbxAssignedStreamQueueGame.SelectedIndex = cbxAssignedStreamQueueGame.FindStringExact(queuename);
        }
        #endregion General

        #region Stream Assistant

        #region General

        private void txtStreamFilesDirectory_TextChanged(object sender, EventArgs e)
        {
            lblDirectoryErrors.Text = "";
            btnApplyChanges.Enabled = true;

            if (txtStreamFilesDirectory.Text != @"")
            {
                if (Directory.Exists(txtStreamFilesDirectory.Text))
                {
                    txtStreamFilesDirectory.BackColor = Color.White;
                }
                else
                {
                    txtStreamFilesDirectory.BackColor = EditableSettings.warningColor;
                    lblDirectoryErrors.Text = "The directory enterred for Stream Files does not exist.";
                }
            }
            else
            {
                txtStreamFilesDirectory.BackColor = EditableSettings.warningColor;
                lblDirectoryErrors.Text = "Please provide a directory for Stream Files.";
            }
        }

        private void btnBrowseStreamFilesDirectory_Click(object sender, EventArgs e)
        {
            //Ask the user to select the folder to store stream files in
            if (fbdBrowserForDirectory.ShowDialog() == DialogResult.OK)
            {
                txtStreamFilesDirectory.Text = fbdBrowserForDirectory.SelectedPath;                 //Update the global value with the new directory
            }
        }
        #endregion General

        #region YouTube Uploads
        private void txtVodsDirectory_TextChanged(object sender, EventArgs e)
        {
            lblDirectoryErrors.Text = "";
            btnApplyChanges.Enabled = true;

            if (txtVodsDirectory.Text != @"")
            {
                if (Directory.Exists(txtVodsDirectory.Text))
                {
                    if (txtVodsDirectory.Text != DirectoryManagement.GetGameDirectory() &&
                    txtVodsDirectory.Text != txtStreamFilesDirectory.Text &&
                    txtVodsDirectory.Text != txtRegionImagesDirectory.Text &&
                    txtVodsDirectory.Text != txtSponsorImagesDirectory.Text)
                    {
                        txtVodsDirectory.BackColor = Color.White;
                    }
                    else
                    {
                        txtVodsDirectory.BackColor = EditableSettings.warningColor;
                        lblDirectoryErrors.Text = "The VoD directory cannot be the same as any other directory used by Master Orders. Choose a new directory.";
                    }
                }
                else
                {
                    txtVodsDirectory.BackColor = EditableSettings.warningColor;
                    lblDirectoryErrors.Text = "The directory enterred for VoDs does not exist.";
                }
            }
            else
            {
                txtVodsDirectory.BackColor = EditableSettings.warningColor;
                lblDirectoryErrors.Text = "Please provide a directory for VoDs.";
            }
        }

        private void btnBrowseVodsDirectory_Click(object sender, EventArgs e)
        {
            if (fbdBrowserForDirectory.ShowDialog() == DialogResult.OK)
            {
                txtVodsDirectory.Text = fbdBrowserForDirectory.SelectedPath;
            }
        }

        private void txtPlaylistName_TextChanged(object sender, EventArgs e)
        {
            btnUpdatePlaylistName.Enabled = true;
        }

        private void btnUpdatePlaylistName_Click(object sender, EventArgs e)
        {
            btnUpdatePlaylistName.Enabled = false;
            txtPlaylistName.Enabled = false;
            btnApplyChanges.Enabled = true;
            try
            {
                Thread thread = new Thread(() =>
                {
                    GetPlaylistNames(txtPlaylistName.Text).Wait();
                });
                thread.IsBackground = true;
                thread.Start();

            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task GetPlaylistNames(string providedPlaylistName)
        {
            playerlistItems = new List<string>();
            playlistNames = new List<string>();
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
                playerlistItems.Add(playlistListId);
                playlistNames.Add(playlist.Snippet.Title);
            }

            CheckPlaylistNameExists(providedPlaylistName);
        }

        delegate void CheckPlaylistNameExists_callback(string providedPlaylistName);

        private void CheckPlaylistNameExists(string providedPlaylistName)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txtPlaylistName.InvokeRequired)
            {
                CheckPlaylistNameExists_callback callbackCheck = new CheckPlaylistNameExists_callback(CheckPlaylistNameExists);
                this.Invoke(callbackCheck, new object[] { providedPlaylistName });
            }
            else
            {
                if (playlistNames.Contains(providedPlaylistName))
                {
                    playlistId = playerlistItems[playlistNames.IndexOf(providedPlaylistName)];
                    MessageBox.Show("The playlist has been set to " + providedPlaylistName + ". \n" +
                                    "The playlist ID is " + playlistId);
                    txtPlaylistName.Enabled = true;
                }
                else
                {
                    if (txtPlaylistName.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        playlistId = "";
                    }
                    else
                    {
                        if (MessageBox.Show("A playlist with the name '" + providedPlaylistName + "' has not been found. Create a new playlist?", "No Playlist Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                Thread thread = new Thread(() =>
                                {
                                    AddNewPlaylist(providedPlaylistName).ContinueWith(task => GetPlaylistNames(providedPlaylistName));
                                });
                                thread.IsBackground = true;
                                thread.Start();

                            }
                            catch (AggregateException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            btnUpdatePlaylistName.Enabled = true;
                            txtPlaylistName.Enabled = true;
                        }
                    }
                }

            }

        }
        private async Task AddNewPlaylist(string new_playlist)
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
        private void rdbStreamSoftwareXsplit_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
            selectedStreamSoftware = @"XSplit";
        }
        private void rdbStreamSoftwareObs_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
            selectedStreamSoftware = @"OBS";
        }

        #endregion YouTube Uploads

        #region Images
        private void ckbEnableImageScoreboard_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
            btnScoreboardPlayer1Image1.Enabled = ckbEnableImageScoreboard.Checked;
            btnScoreboardPlayer1Image2.Enabled = ckbEnableImageScoreboard.Checked;
            btnScoreboardPlayer1Image3.Enabled = ckbEnableImageScoreboard.Checked;
            btnScoreboardPlayer2Image1.Enabled = ckbEnableImageScoreboard.Checked;
            btnScoreboardPlayer2Image2.Enabled = ckbEnableImageScoreboard.Checked;
            btnScoreboardPlayer2Image3.Enabled = ckbEnableImageScoreboard.Checked;
        }

        private void ImageScoreBoardButton_Click(object sender, EventArgs e)
        {
            Button clickedImageButton = (Button)sender;
            if (ofdBrowseForPng.ShowDialog() == DialogResult.OK)
            {
                btnApplyChanges.Enabled = true;
                int scoreControlIndex = (int)(clickedImageButton.Tag);
                editableSettings.scoreControls[scoreControlIndex].UpdateImage(ofdBrowseForPng.FileName);
            }
        }
        private void txtSponsorImagesDirectory_TextChanged(object sender, EventArgs e)
        {
            lblDirectoryErrors.Text = "";
            btnApplyChanges.Enabled = true;

            if (txtSponsorImagesDirectory.Text != @"")
            {
                if (Directory.Exists(txtSponsorImagesDirectory.Text))
                {
                    txtSponsorImagesDirectory.BackColor = Color.White;
                }
                else
                {
                    txtSponsorImagesDirectory.BackColor = EditableSettings.warningColor;
                    lblDirectoryErrors.Text = "The directory enterred for Sponsor images does not exist.";

                }
            }
            else
            {
                txtSponsorImagesDirectory.BackColor = EditableSettings.warningColor;
                lblDirectoryErrors.Text = "Please provide a directory for Sponsor images.";
            }
        }

        private void btnBrowserSponsorImagesDirectory_Click(object sender, EventArgs e)
        {
            if (fbdBrowserForDirectory.ShowDialog() == DialogResult.OK)
            {
                txtSponsorImagesDirectory.Text = fbdBrowserForDirectory.SelectedPath;
            }
        }

        private void txtRegionImagesDirectory_TextChanged(object sender, EventArgs e)
        {
            lblDirectoryErrors.Text = "";
            btnApplyChanges.Enabled = true;

            if (txtRegionImagesDirectory.Text != @"")
            {
                if (Directory.Exists(txtRegionImagesDirectory.Text))
                {
                    txtRegionImagesDirectory.BackColor = Color.White;
                }
                else
                {
                    txtRegionImagesDirectory.BackColor = EditableSettings.warningColor;
                    lblDirectoryErrors.Text = "The directory enterred for Region images does not exist.";

                }
            }
            else
            {
                txtRegionImagesDirectory.BackColor = EditableSettings.warningColor;
                lblDirectoryErrors.Text = "Please provide a directory for Region images.";
            }
        }

        private void btnBrowseRegionImagesDirectory_Click(object sender, EventArgs e)
        {
            if (fbdBrowserForDirectory.ShowDialog() == DialogResult.OK)
            {
                txtRegionImagesDirectory.Text = fbdBrowserForDirectory.SelectedPath;
            }
        }

        private void ckbEnableSponsorImages_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
        }

        private void ckbEnableRegionImages_CheckedChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
        }

        #endregion Images

        #region Thumbnails
        private bool VerifyPreviewIsReady()
        {
            if (txtThumbnailBackground.Text != "" && txtThumbnailForeground.Text != "" &&
                txtThumbnailBackground.BackColor != EditableSettings.warningColor && txtThumbnailForeground.BackColor != EditableSettings.warningColor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private void btnSelectThumbnailFont_Click(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;

            DialogResult newfont = ftdPromptForFont.ShowDialog();
            if (newfont == DialogResult.OK)
            {
                editableSettings.thumbnailFont = ftdPromptForFont.Font;
                lblThumnailFont.Text = editableSettings.thumbnailFont.Name + " " + editableSettings.thumbnailFont.SizeInPoints + "pt " + editableSettings.thumbnailFont.Style.ToString();
            }
        }

        private void txtThumbnailBackground_TextChanged(object sender, EventArgs e)
        {
            txtThumbnailBackground.BackColor = EditableSettings.warningColor;
            btnPreviewThumbnail.Enabled = false;
            if (txtThumbnailBackground.Text != @"")
            {
                if (File.Exists(txtThumbnailBackground.Text))
                {
                    if (Path.GetExtension(txtThumbnailBackground.Text) == ".jpg")
                    {
                        btnApplyChanges.Enabled = true;
                        txtThumbnailBackground.BackColor = Color.White;
                        if (txtThumbnailForeground.Text != "" && txtThumbnailForeground.BackColor != EditableSettings.warningColor &&
                            cbxCharacterRosters.BackColor != EditableSettings.warningColor && cbxCharacterRosters.Text != "")
                            btnPreviewThumbnail.Enabled = true;
                    }
                    else
                    {
                        txtThumbnailBackground.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txtThumbnailBackground.BackColor = EditableSettings.warningColor;
                }
            }
        }

        private void btnThumbnailBackground_Click(object sender, EventArgs e)
        {
            if (ofdBrowseForJpg.ShowDialog() == DialogResult.OK)
            {
                txtThumbnailBackground.Text = ofdBrowseForJpg.FileName;
            }
        }

        private void txtThumbnailForeground_TextChanged(object sender, EventArgs e)
        {
            txtThumbnailForeground.BackColor = EditableSettings.warningColor;
            btnPreviewThumbnail.Enabled = false;

            if (txtThumbnailForeground.Text != @"")
            {
                if (File.Exists(txtThumbnailForeground.Text))
                {
                    if (Path.GetExtension(txtThumbnailForeground.Text) == ".png")
                    {
                        btnApplyChanges.Enabled = true;
                        txtThumbnailForeground.BackColor = Color.White;
                        if (txtThumbnailBackground.Text != "" && txtThumbnailBackground.BackColor != EditableSettings.warningColor &&
                            cbxCharacterRosters.BackColor != EditableSettings.warningColor && cbxCharacterRosters.Text != "")
                            btnPreviewThumbnail.Enabled = true;
                    }
                    else
                    {
                        txtThumbnailForeground.BackColor = EditableSettings.warningColor;
                    }
                }
                else
                {
                    txtThumbnailForeground.BackColor = EditableSettings.warningColor;
                }
            }
        }

        private void btnThumbnailForeground_Click(object sender, EventArgs e)
        {
            if (ofdBrowseForPng.ShowDialog() == DialogResult.OK)
            {
                txtThumbnailForeground.Text = ofdBrowseForPng.FileName;
            }
        }

        #endregion Thumbnails

        #endregion Stream Assistant
        private void SettingFieldChanged(object sender, EventArgs e)
        {
            btnApplyChanges.Enabled = true;
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

            btnApplyChanges.Enabled = true;
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
            Image background = Image.FromFile(txtThumbnailBackground.Text);
            Image foreground = Image.FromFile(txtThumbnailForeground.Text);

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

        private void cbxThumbnailPreviewCharacter1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = DirectoryManagement.GetGameDirectory() + @"\" + cbxThumbnailPreviewCharacter1.Text + @"\1\";
        }

        private void cbxThumbnailPreviewCharacter2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = DirectoryManagement.GetGameDirectory() + @"\" + cbxThumbnailPreviewCharacter2.Text + @"\1\";
        }

        private void btnPreviewThumbnail_Click(object sender, EventArgs e)
        {
            create_thumbnail();
        }
        private void btnApplyChanges_Click(object sender, EventArgs e)
        {
            if (save_settings() == true)
                btnApplyChanges.Enabled = false;
        }

        private void btnOkConfirmChanges_Click(object sender, EventArgs e)
        {
            if (save_settings() == true)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void btnCancelChanges_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool save_settings()
        {
            //Check all settings to ensure there are no conflicts, and ensure all needed fields are provided
            {
                if (txtCharacterDatabasesDirectory.BackColor == EditableSettings.warningColor ||
                    txtCharacterDatabasesDirectory.Text == "")
                {
                    MessageBox.Show("The provided directory for Character Databases is incorrect. Please correct this under the General settings.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (ckbEnableImageScoreboard.Checked == true && (
                    score_image[0] == "" || score_image[1] == "" ||
                    score_image[2] == "" || score_image[3] == "" ||
                    score_image[4] == "" || score_image[5] == ""))
                {
                    if (MessageBox.Show("Image Scoreboard is enabled, but not all images have been provided. Click OK to disable Image Scoreboard, or click Cancel to cancel saving changes and add the needed images.", "Image Files Missing", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckbEnableImageScoreboard.Checked = false;
                    else
                        return false;
                }
                if (ckb_thumbnails.Checked == true &&
                    (!(File.Exists(txtThumbnailBackground.Text) &&
                    File.Exists(txtThumbnailForeground.Text))))

                {
                    if (MessageBox.Show("Thumbnail Generation is enabled, but a working path to a background and/or foreground image has not been provided. Click OK to disable Thumbnail Generation, or click Cancel to cancel saving changes and add a working path to a background and/or foreground image.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckb_thumbnails.Checked = false;
                    else
                        return false;
                }
                if (ckbEnableSponsorImages.Checked == true && (
                    txtSponsorImagesDirectory.Text == "" ||
                    txtSponsorImagesDirectory.BackColor == EditableSettings.warningColor))
                {
                    if (MessageBox.Show("Sponsor Images are enabled, but a working directory has not been provided. Click OK to disable Sponsor Images, or click Cancel to cancel saving changes and add a working directory.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckbEnableSponsorImages.Checked = false;
                    else
                        return false;
                }
                if (ckbEnableRegionImages.Checked == true && (
                    txtRegionImagesDirectory.Text == "" ||
                    txtRegionImagesDirectory.BackColor == EditableSettings.warningColor))
                {
                    if (MessageBox.Show("Region Images are enabled, but a working directory has not been provided. Click OK to disable Region Images, or click Cancel to cancel saving changes and add a working directory.", "Invalid Directory Provided", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                        == DialogResult.OK)
                        ckbEnableRegionImages.Checked = false;
                    else
                        return false;
                }
                if (txtVodsDirectory.BackColor == EditableSettings.warningColor)
                {
                    MessageBox.Show("The provided directory for VoDs is invalid. Please correct this under the Stream Assistant settings' Directories tab. Keep in mind that the VoDs directory cannot be the same as any other directory used by Master Orders.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                //DID THIS RECENTLY LOL
                ////if (save_checking == 1)
                {
                    if (txtStreamFilesDirectory.BackColor == EditableSettings.warningColor ||
                        txtStreamFilesDirectory.Text == "")
                    {
                        MessageBox.Show("The provided directory for Stream Files is invalid. Please correct this under the Stream Assistant settings' Directories tab.", "Invalid Directory Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                if(cbxAssignedStreamQueueGame.Text != cbxCharacterRosters.Text && cbxStreamQueues.Text != "None")
                {
                    if (MessageBox.Show("The selected character roster does not match the game that the selected queue is set to use. Master Orders will need to match the queue's game to the selected roster. Okay to Proceed?", "Roster Game Mismatch", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning)
                       == DialogResult.OK)
                    {
                        int queueidd = -1;
                        for (int i = 0; i < StreamQueueManager.queueList.Count; i++)
                        {
                            if (StreamQueueManager.queueList[i].queueName == cbxStreamQueues.Text)
                            {
                                queueidd = i;
                            }
                        }
                        if (database_tools.regame_queue(cbxStreamQueues.Text, cbxCharacterRosters.Text, queueidd) == false)
                            return false;
                    }
                    else
                        return false;
                }

                if (txtStreamFilesDirectory.BackColor == EditableSettings.warningColor)
                {
                    MessageBox.Show("The provided Bracket Rounds File is invalid. Select a valid file or clear the textbox to use the default rounds.", "Invalid File Provided", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            int queueid = -1;
            for (int i = 0; i < StreamQueueManager.queueList.Count; i++)
            {
                if (StreamQueueManager.queueList[i].queueName == cbxStreamQueues.Text)
                {
                    queueid = i;
                }
            }

            //Apply all changes to the settings file
            {
                XDocument xml = XDocument.Load(SettingsFile.settingsFile);


                xml.Root.Element("database").Element("queue-id").ReplaceWith(new XElement("queue-id", queueid));
                xml.Root.Element("database").Element("game-id").ReplaceWith(new XElement("game-id", cbxCharacterRosters.Text));

                xml.Root.Element("directories").Element("character-directory").ReplaceWith(new XElement("character-directory", txtCharacterDatabasesDirectory.Text));
                xml.Root.Element("directories").Element("stream-directory").ReplaceWith(new XElement("stream-directory", txtStreamFilesDirectory.Text));
                xml.Root.Element("directories").Element("vods-directory").ReplaceWith(new XElement("vods-directory", txtVodsDirectory.Text));
                xml.Root.Element("directories").Element("enable-sponsor").ReplaceWith(new XElement("enable-sponsor", ckbEnableSponsorImages.Checked));
                xml.Root.Element("directories").Element("enable-region").ReplaceWith(new XElement("enable-region", ckbEnableRegionImages.Checked));
                xml.Root.Element("directories").Element("sponsor-directory").ReplaceWith(new XElement("sponsor-directory", txtSponsorImagesDirectory.Text));
                xml.Root.Element("directories").Element("region-directory").ReplaceWith(new XElement("region-directory", txtRegionImagesDirectory.Text));

                xml.Root.Element("youtube").Element("enable-youtube").ReplaceWith(new XElement("enable-youtube", ckbEnableVodUploads.Checked));
                xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", txtPlaylistName.Text));
                xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", playlistId));
                xml.Root.Element("youtube").Element("default-description").ReplaceWith(new XElement("default-description", txt_description.Text));
                xml.Root.Element("youtube").Element("tags").ReplaceWith(new XElement("tags", txt_tags.Text));
                xml.Root.Element("youtube").Element("title-template").ReplaceWith(new XElement("title-template", txt_titletemplate.Text));

                xml.Root.Element("image-scoring").Element("enable-image-scoring").ReplaceWith(new XElement("enable-image-scoring", ckbEnableImageScoreboard.Checked));
                xml.Root.Element("image-scoring").Element("player1-1").ReplaceWith(new XElement("player1-1", score_image[0]));
                xml.Root.Element("image-scoring").Element("player1-2").ReplaceWith(new XElement("player1-2", score_image[1]));
                xml.Root.Element("image-scoring").Element("player1-3").ReplaceWith(new XElement("player1-3", score_image[2]));
                xml.Root.Element("image-scoring").Element("player2-1").ReplaceWith(new XElement("player2-1", score_image[3]));
                xml.Root.Element("image-scoring").Element("player2-2").ReplaceWith(new XElement("player2-2", score_image[4]));
                xml.Root.Element("image-scoring").Element("player2-3").ReplaceWith(new XElement("player2-3", score_image[5]));

                xml.Root.Element("thumbnail-layout").Element("background-image").ReplaceWith(new XElement("background-image", txtThumbnailBackground.Text));
                xml.Root.Element("thumbnail-layout").Element("foreground-image").ReplaceWith(new XElement("foreground-image", txtThumbnailForeground.Text));
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

                xml.Root.Element("general").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", rdbAutomaticStreamUpdates.Checked));
                xml.Root.Element("general").Element("stream-software").ReplaceWith(new XElement("stream-software", selectedStreamSoftware));
                xml.Root.Element("general").Element("enable-thumbnails").ReplaceWith(new XElement("enable-thumbnails", ckb_thumbnails.Checked));
                xml.Root.Element("general").Element("copy-title").ReplaceWith(new XElement("copy-title", ckb_clipboard.Checked));
                xml.Root.Element("general").Element("shorten-title").ReplaceWith(new XElement("shorten-title", cbx_shorten_video.SelectedIndex));
                xml.Root.Element("general").Element("format").ReplaceWith(new XElement("format", cbx_format.Text));
                xml.Root.Element("general").Element("keep-on-top").ReplaceWith(new XElement("keep-on-top", ckbKeepWindowOnTop.Checked));
                xml.Root.Element("general").Element("rounds-file").ReplaceWith(new XElement("rounds-file", txtBracketRoundsFile.Text));
                xml.Root.Element("general").Element("sponsor-seperator").ReplaceWith(new XElement("sponsor-seperator", txt_seperator.Text));


                xml.Save(SettingsFile.settingsFile);
            }

            //Apply all changes to the global values
            {
                ImageManagement.enableRegionImages = ckbEnableRegionImages.Checked;
                DirectoryManagement.regionDirectory = txtRegionImagesDirectory.Text;
                ImageManagement.enableSponsorImages = ckbEnableSponsorImages.Checked;
                DirectoryManagement.sponsorDirectory = txtSponsorImagesDirectory.Text; ;
                YoutubeLibrary.YoutubeController.enableYoutubeFunctions = ckbEnableVodUploads.Checked;
                YoutubeController.enableVideoThumbnails = ckb_thumbnails.Checked;
                YoutubeController.copyVideoTitle = ckb_clipboard.Checked;
                YoutubeLibrary.YoutubeController.streamSoftware = selectedStreamSoftware;
                ImageManagement.enableImageScoreboard = ckbEnableImageScoreboard.Checked;
                ImageManagement.scoreboardImages[0, 0] = score_image[0];
                ImageManagement.scoreboardImages[0, 1] = score_image[1];
                ImageManagement.scoreboardImages[0, 2] = score_image[2];
                ImageManagement.scoreboardImages[1, 0] = score_image[3];
                ImageManagement.scoreboardImages[1, 1] = score_image[4];
                ImageManagement.scoreboardImages[1, 2] = score_image[5];
                DirectoryManagement.outputDirectory = txtStreamFilesDirectory.Text;
                DirectoryManagement.vodsDirectory = txtVodsDirectory.Text;
                DataOutputCaller.automaticUpdates = rdbAutomaticStreamUpdates.Checked;
                YoutubeController.playlistName = txtPlaylistName.Text;
                YoutubeController.playlistId = playlistId;
                global_values.queue_id = queueid;
                GlobalSettings.selectedGame = cbxCharacterRosters.Text;
            }

            return true;
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
    }
}
