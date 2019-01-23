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
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler
{
    public partial class frm_settings : Form
    {
        Font thumbnail_font = new Font("Arial", 12, FontStyle.Regular);
        public static Color warning_color = Color.FromArgb(234, 153, 153);

        string game_path;                    //Save the directory
        List<string> game_directories;
        string[] game_info;
        string[] characters;
        string output_directory;
        string thumbnail_directory;
        string uldata_directory;
        string vods_directory;
        string sponsors_directory;
        string regions_directory;
        string json_file;
        string playlist_name;
        string playlist_id;
        bool enable_sponsors;
        bool enable_regions;
        bool enable_youtube;
        string youtube_username = "Master Orders";
        string stream_software;
        bool auto_update;
        bool image_scoreboard;
        string[] score_image = new string[6];
        bool copy_video_title;
        string default_description;

        string thumbnail_background;
        string thumbnail_foreground;

        string image_directory1;
        string image_directory2;

        //Initialize the variables containing YouTube Playlist information
        List<string> playlist_items = new List<string>();
        List<string> playlist_names = new List<string>();

        public class score_control
        {
            public Button score_button;
            public PictureBox score_picture;
            public int picture_index;
        }
        public List<string> score_boxes_names = new List<string>();
        public List<score_control> score_boxes = new List<score_control>();

        public void add_score_control(Button button, PictureBox pictureBox)
        {
            score_control new_score = new score_control();
            new_score.score_button = button;
            new_score.score_picture = pictureBox;
            new_score.picture_index = score_boxes.Count();
            score_boxes.Add(new_score);
            score_boxes_names.Add(button.Name);
        }

        public score_control find_score_control(string name)
        {
            int index = score_boxes_names.IndexOf(name);
            return score_boxes[index];
        }

        public frm_settings()
        {
            InitializeComponent();

            add_score_control(btn_score1_image1, pic_score1_image1);
            add_score_control(btn_score1_image2, pic_score1_image2);
            add_score_control(btn_score1_image3, pic_score1_image3);
            add_score_control(btn_score2_image1, pic_score2_image1);
            add_score_control(btn_score2_image2, pic_score2_image2);
            add_score_control(btn_score2_image3, pic_score2_image3);

        }

        private void btn_font_Click(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;

            DialogResult newfont = ftd_thumbnail.ShowDialog();
            if (newfont == DialogResult.OK)
            {
                thumbnail_font = ftd_thumbnail.Font;
                lbl_font.Text = thumbnail_font.Name + " " + thumbnail_font.SizeInPoints + "pt " + thumbnail_font.Style.ToString();
            }
        }

        private void txt_characters_TextChanged(object sender, EventArgs e)
        {
            //Reset errors
            lbl_characters.Text = "";
            txt_characters.BackColor = Color.White;
            btn_apply.Enabled = true;
            game_path = "";

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
                        txt_characters.BackColor = warning_color;
                        return;
                    }
                    game_directories = new List<string>();
                    cbx_characters.BeginUpdate();                                      //Begin
                    cbx_characters.Items.Clear();                                      //Empty the item list

                    foreach (string folder in folders)
                    {
                        if (!(File.Exists(folder + @"\game info.txt") &&
                            File.Exists(folder + @"\characters.txt")))
                        {
                            lbl_characters.Text = "The directory " + folder + " is not configured for Master Orders. " +
                                "Please correct or remove the folder from the selected directory.";
                            txt_characters.BackColor = warning_color;
                            cbx_characters.Items.Clear();                                      //Empty the item list
                            cbx_characters.EndUpdate();                                        //End
                            //cbx_characters.SelectedIndex = 0;                                  //Set the combobox index to 0

                            return;
                        }
                        string[] gamename = System.IO.File.ReadAllLines(folder + @"\game info.txt");
                        game_directories.Add(folder);
                        cbx_characters.Items.Add(gamename[0]);
                    }
                    cbx_characters.EndUpdate();                                        //End
                    cbx_characters.SelectedIndex = 0;                                  //Set the combobox index to 0
                }
                else
                {
                    lbl_characters.Text = "This directory does not exist.";
                    txt_characters.BackColor = warning_color;
                    return;
                }
            }
            else
            {
                //If a directory has not been provided, mark the field for an error and switch tabs to show it
                txt_characters.BackColor = warning_color;
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
            if (cbx_characters.Text != null)
            {
                game_path = game_directories[cbx_characters.SelectedIndex];
                game_info = System.IO.File.ReadAllLines(game_path + @"\game info.txt");
                characters = System.IO.File.ReadAllLines(game_path + @"\characters.txt");
                btn_apply.Enabled = true;

                if (thumbnail_background != "" && thumbnail_background != null &&
                    thumbnail_foreground != null && thumbnail_foreground != "")
                    btn_preview.Enabled = true;

                cbx_char1.BeginUpdate();                                      //Begin
                cbx_char1.Items.Clear();                                      //Empty the item list
                cbx_char2.BeginUpdate();                                      //Begin
                cbx_char2.Items.Clear();                                      //Empty the item list     
                int character_count = Int32.Parse(game_info[1]);      //Store the number of characters
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

        private void txt_streamfiles_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_streamfiles.Text != @"")
            {
                if (Directory.Exists(txt_streamfiles.Text))
                {
                    txt_streamfiles.BackColor = Color.White;
                    output_directory = txt_streamfiles.Text;
                }
                else
                {
                    txt_streamfiles.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for Stream Files does not exist.";
                }
            }
            else
            {
                txt_streamfiles.BackColor = warning_color;
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
                    thumbnail_directory = txt_thumbnails.Text;
                }
                else
                {
                    txt_thumbnails.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for Thumbnails does not exist.";

                }
            }
            else
            {
                txt_thumbnails.BackColor = warning_color;
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
                    if (txt_vods.Text != game_path &&
                    txt_vods.Text != output_directory &&
                    txt_vods.Text != thumbnail_directory &&
                    txt_vods.Text != uldata_directory &&
                    txt_vods.Text != regions_directory &&
                    txt_vods.Text != sponsors_directory)
                    {
                        txt_vods.BackColor = Color.White;
                        vods_directory = txt_vods.Text;
                    }
                    else
                    {
                        txt_vods.BackColor = warning_color;
                        lbl_directories.Text = "The VoD directory cannot be the same as any other directory used by Master Orders. Choose a new directory.";
                    }
                }
                else
                {
                    txt_vods.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for VoDs does not exist.";
                }
            }
            else
            {
                txt_vods.BackColor = warning_color;
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

        private void txt_uldata_TextChanged(object sender, EventArgs e)
        {
            lbl_directories.Text = "";
            btn_apply.Enabled = true;

            if (txt_uldata.Text != @"")
            {
                if (Directory.Exists(txt_uldata.Text))
                {
                    txt_uldata.BackColor = Color.White;
                    uldata_directory = txt_uldata.Text;
                }
                else
                {
                    txt_uldata.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for ULData files does not exist.";

                }
            }
            else
            {
                txt_uldata.BackColor = warning_color;
                lbl_directories.Text = "Please provide a directory for ULData files.";
            }
        }

        private void btn_uldata_Click(object sender, EventArgs e)
        {
            if (fbd_directory.ShowDialog() == DialogResult.OK)
            {
                txt_uldata.Text = fbd_directory.SelectedPath;
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
                    sponsors_directory = txt_sponsors.Text;
                }
                else
                {
                    txt_sponsors.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for Sponsor images does not exist.";

                }
            }
            else
            {
                txt_sponsors.BackColor = warning_color;
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
                    regions_directory = txt_regions.Text;
                }
                else
                {
                    txt_regions.BackColor = warning_color;
                    lbl_directories.Text = "The directory enterred for Region images does not exist.";

                }
            }
            else
            {
                txt_regions.BackColor = warning_color;
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
            enable_sponsors = ckb_sponsors.Checked;
        }

        private void ckb_regions_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            enable_regions = ckb_regions.Checked;
        }

        private void ckb_vod_uploads_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            txt_json.Enabled = ckb_vod_uploads.Checked;
            btn_json.Enabled = ckb_vod_uploads.Checked;
            enable_youtube = ckb_vod_uploads.Checked;
        }

        private void txt_json_TextChanged(object sender, EventArgs e)
        {
            txt_json.BackColor = warning_color;
            btn_ouauth.Enabled = false;
            if (txt_json.Text != @"")
            {
                if (File.Exists(txt_json.Text))
                {
                    if (Path.GetExtension(txt_json.Text) == ".json")
                    {
                        txt_json.BackColor = Color.White;
                        if (ckb_vod_uploads.Checked == true)
                        {
                            btn_ouauth.Enabled = true;
                            txt_playlist.Enabled = true;
                            txt_description.Enabled = true;
                            rdb_xsplit.Enabled = true;
                            rdb_obs.Enabled = true;
                        }
                        json_file = txt_json.Text;

                    }
                    else
                    {
                        txt_json.BackColor = warning_color;
                        lbl_youtube.Text = "The enterred file is not a JSON file.";
                    }
                }
                else
                {
                    txt_json.BackColor = warning_color;
                    lbl_youtube.Text = "Please provide the location of the JSON file obtained through the Google Developer Console.";
                }
            }
        }

        private void btn_json_Click(object sender, EventArgs e)
        {
            if (ofd_json.ShowDialog() == DialogResult.OK)
            {
                txt_json.Text = ofd_json.FileName;
            }
        }

        private void btn_oauth_Click(object sender, EventArgs e)
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
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task authorize()
        {
            UserCredential credential;
            using (var stream = new FileStream(json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                            YouTubeService.Scope.YoutubeUpload },
                    youtube_username,
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            await credential.RefreshTokenAsync(CancellationToken.None);
            //await credential.RevokeTokenAsync(CancellationToken.None);

        }

        private void txt_playlist_TextChanged(object sender, EventArgs e)
        {
            if (txt_playlist.Text != playlist_name)
            {
                btn_playlist.Enabled = true;
            }
            else
            {
                btn_playlist.Enabled = false;
            }
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
            UserCredential credential;
            using (var stream = new FileStream(json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.YoutubeUpload },
                    youtube_username,
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
                    playlist_name = txt_playlist.Text;
                }
                else
                {
                    if (txt_playlist.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        playlist_name = "";
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
            UserCredential credential;
            using (var stream = new FileStream(json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.YoutubeUpload  },
                    youtube_username,
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
            auto_update = rdb_automatic.Checked;
        }

        private void ckb_scoreboad_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            image_scoreboard = ckb_scoreboad.Checked;
            btn_score1_image1.Enabled = image_scoreboard;
            btn_score1_image2.Enabled = image_scoreboard;
            btn_score1_image3.Enabled = image_scoreboard;
            btn_score2_image1.Enabled = image_scoreboard;
            btn_score2_image2.Enabled = image_scoreboard;
            btn_score2_image3.Enabled = image_scoreboard;
        }

        private void score_button_Click(object sender, EventArgs e)
        {
            Button image_button = (Button)sender;
            if (ofd_png.ShowDialog() == DialogResult.OK)
            {
                btn_apply.Enabled = true;
                score_control clicked_control = find_score_control(image_button.Name);
                score_image[clicked_control.picture_index] = ofd_png.FileName;
                clicked_control.score_picture.Image = Image.FromFile(ofd_png.FileName);
                image_button.BackColor = Color.Transparent;
            }
        }

        private void tab_stream_tabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_stream_tabs.SelectedIndex != 3)
                return;

        }

        private void txt_background_TextChanged(object sender, EventArgs e)
        {
            txt_background.BackColor = warning_color;
            btn_preview.Enabled = false;
            if (txt_background.Text != @"")
            {
                if (File.Exists(txt_background.Text))
                {
                    if (Path.GetExtension(txt_background.Text) == ".jpg")
                    {
                        btn_apply.Enabled = true;
                        txt_background.BackColor = Color.White;
                        thumbnail_background = txt_background.Text;
                        if (thumbnail_foreground != "" && thumbnail_foreground != null &&
                            cbx_characters.Text != null && cbx_characters.Text != "")
                            btn_preview.Enabled = true;
                    }
                    else
                    {
                        txt_background.BackColor = warning_color;
                    }
                }
                else
                {
                    txt_background.BackColor = warning_color;
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
            txt_foreground.BackColor = warning_color;
            btn_preview.Enabled = false;

            if (txt_foreground.Text != @"")
            {
                if (File.Exists(txt_foreground.Text))
                {
                    if (Path.GetExtension(txt_foreground.Text) == ".png")
                    {
                        btn_apply.Enabled = true;
                        txt_foreground.BackColor = Color.White;
                        thumbnail_foreground = txt_foreground.Text;
                        if(thumbnail_background != "" && thumbnail_background != null &&
                            cbx_characters.Text != null && cbx_characters.Text != "")
                            btn_preview.Enabled = true;
                    }
                    else
                    {
                        txt_foreground.BackColor = warning_color;
                    }
                }
                else
                {
                    txt_foreground.BackColor = warning_color;
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
            Image left_character = Image.FromFile(image_directory1 + @"\left.png");
            Image right_character = Image.FromFile(image_directory2 + @"\right.png");

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
            string current_patch = "1.2.0";

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
            FontFamily keepcalm = thumbnail_font.FontFamily;
            Font calmfont = thumbnail_font;

            int font_size = Int32.Parse(txt_name1_size.Text);                   //Create a variable for adjustable font size
            Size namesize = TextRenderer.MeasureText(player_name1, calmfont);   //Create a variable to hold the size of player tags

            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font(thumbnail_font.FontFamily, font_size, FontStyle.Regular);     //Create a new font with this new size
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
                calmfont = new Font(thumbnail_font.FontFamily, font_size, FontStyle.Regular);     //Create a new font with this new size
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

            if (ckb_patch.Checked == true)
            {
                draw_patch.AddString(
                   current_patch,                                                     //text to draw
                    keepcalm,                                                         //font family
                    (int)FontStyle.Regular,                                           //font style
                    Int32.Parse(txt_patch_size.Text),                                 //font size (drawing.DpiY * 120 / 72)
                    new Point(300 + Int32.Parse(txt_patch_xoffset.Text), 1020 + Int32.Parse(txt_patch_yoffset.Text)), //620                                        //drawing location
                    text_center);                                                     //text alignment     
                                                                                      //Draw the outline and filling in the appropriate colors
                drawing.DrawPath(light_stroke, draw_patch);
                drawing.FillPath(white_text, draw_patch);
            }
            //Save the drawing surface back to the bitmap image
            drawing.Save();
            //Dispose the drawing surface
            drawing.Dispose();

            //Return the title of the image file
            pic_thumbnail.BackgroundImage = thumbnail_bmp;
        }

        private void cbx_char1_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory1 = game_path + @"\" + cbx_char1.Text + @"\1\";
        }

        private void cbx_char2_SelectedIndexChanged(object sender, EventArgs e)
        {
            image_directory2 = game_path + @"\" + cbx_char2.Text + @"\1\";
        }

        private void btn_preview_Click(object sender, EventArgs e)
        {
            create_thumbnail();
        }

        private void checkbox_Changed(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            btn_apply.Enabled = false;
        }

        private void ckb_clipboard_CheckedChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            copy_video_title = ckb_clipboard.Checked;
        }

        private void txt_description_TextChanged(object sender, EventArgs e)
        {
            btn_apply.Enabled = true;
            default_description = txt_description.Text;
        }
    }
}
