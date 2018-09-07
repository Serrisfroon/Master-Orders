//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2018, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;

using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Upload;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using System.Reflection;

namespace Stream_Info_Handler
{
    public delegate void enable_button_event(string check_reenable);
    public partial class frm_uploading : Form
    {
        public event enable_button_event enable_button;
        public string video_id;
        public string thumbnail_file;
        public string old_thumbnail;
        public string youtube_data;
        public long video_size;
        public long video_multiplier = 100;
        public string reenable_button;

        public frm_uploading(string title, string description, string thumbnail, string vodfile, string reenable)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            txt_videotitle.Text = title;
            txt_description.Text = description;
            thumbnail_file = "upload_thumbnail_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".jpg";
            if (File.Exists(thumbnail_file))
            {
                File.Delete(thumbnail_file);
            }
            File.Copy(thumbnail, thumbnail_file);

            old_thumbnail = thumbnail;
            pic_thumbnail.Image = Image.FromFile(thumbnail);
            pic_thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;

            txt_videofile.Text = vodfile;

            youtube_data = global_values.current_youtube_data;

            reenable_button = reenable;
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            pic_thumbnail.Image.Dispose();
            File.Delete(old_thumbnail);
            File.Delete(thumbnail_file);
            enable_button(reenable_button);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(btn_upload_cancel.Text == "Finish")
            {
                DialogResult dialogResult = MessageBox.Show("Do you want to delete the local upload data?", "Upload Complete", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (File.Exists(youtube_data))
                    {
                        File.Delete(youtube_data);
                    }
                }
            }
            this.Close();
        }

        private void btn_videofile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = global_values.vods_directory;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_videofile.Text = openFileDialog1.FileName;
            }
        }

        private void txt_videofile_TextChanged(object sender, EventArgs e)
        {
            if (txt_videofile.Text != @"")
            {
                if (File.Exists(txt_videofile.Text))
                {
                    btn_upload_video.Enabled = true;
                    lbl_progress.Text = @"Ready to Upload!";
                }
                else
                {
                    btn_upload_video.Enabled = false;
                }
            }
        }

        private void btn_upload_video_Click(object sender, EventArgs e)
        {
            if (global_values.allow_upload == true)
            {
                if(global_values.stream_software == "OBS")
                {
                    if (MessageBox.Show("Please ensure that you have ended the video recording.", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                this.ControlBox = false;
                btn_upload_video.Enabled = false;
                btn_upload_cancel.Enabled = false;
                txt_videofile.Enabled = false;
                txt_videotitle.Enabled = false;
                txt_description.Enabled = false;
                btn_videofile.Enabled = false;
                lbl_progress.Text = @"Starting YouTube upload...";
                FileInfo videofile = new FileInfo(txt_videofile.Text);
                video_size = videofile.Length;
                try
                {
                    Thread thead = new Thread(() =>
                    {
                        Run().Wait();
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
                System.Media.SystemSounds.Asterisk.Play();
            }

        }
        //                    pgb_upload.Value = (int)(progress.BytesSent / video_size * video_multiplier);
        
        private async Task Run()
        {
            UserCredential credential;
            using (var stream = new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows an application to upload files to the
                    // authenticated user's YouTube channel, but doesn't allow other types of access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.Youtubepartner,
                    YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubepartnerChannelAudit,
                    YouTubeService.Scope.YoutubeReadonly  },
                    global_values.youtube_username,
                    CancellationToken.None
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
            });

            var video = new Video();
            video.Snippet = new VideoSnippet();
            video.Snippet.Title = txt_videotitle.Text;
            video.Snippet.Description = txt_description.Text;
            string[] video_tags = new string[] {"Wii U", "UGS", "UGS Gaming", "Smash 4"};
            video.Snippet.Tags = video_tags;
            video.Snippet.CategoryId = "20"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
            video.Status = new VideoStatus();
            video.Status.PrivacyStatus = "public"; // or "private" or "public"
            var filePath = txt_videofile.Text; // Replace with path to actual movie file.

            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var videosInsertRequest = youtubeService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                await videosInsertRequest.UploadAsync();
            }

            if (global_values.enable_playlists == true)
            {
                // Add a video to the newly created playlist.
                var newPlaylistItem = new PlaylistItem();
                newPlaylistItem.Snippet = new PlaylistItemSnippet();
                newPlaylistItem.Snippet.PlaylistId = global_values.playlist_id;
                newPlaylistItem.Snippet.ResourceId = new ResourceId();
                newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                newPlaylistItem.Snippet.ResourceId.VideoId = video_id;
                newPlaylistItem = await youtubeService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
            }

            using (var tStream = new FileStream(thumbnail_file, FileMode.Open))
            {
                var tInsertRequest = youtubeService.Thumbnails.Set(video_id, tStream, "image/jpeg");
                tInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                tInsertRequest.ResponseReceived += thumbnailInsertRequest_ResponseReceived;
                

                await tInsertRequest.UploadAsync();
            }
        }

        void videosInsertRequest_ProgressChanged(Google.Apis.Upload.IUploadProgress progress)
        {
            switch (progress.Status)
            {
                case UploadStatus.Uploading:
                    int percentage_complete = Convert.ToInt32(Math.Round(Convert.ToDecimal(progress.BytesSent) / Convert.ToDecimal(video_size) * 100));
                    lbl_progress.Text = String.Format("Uploading... {0}% complete.", percentage_complete);
                    pgb_upload.Value = percentage_complete;
                    break;

                case UploadStatus.Failed:
                    txt_description.Text = String.Format("An error prevented the upload from completing. \n{0}", progress.Exception);
                    btn_upload_video.Enabled = true;
                    btn_upload_cancel.Enabled = true;
                    break;
            }
        }

        void videosInsertRequest_ResponseReceived(Video video)
        {
            lbl_progress.Text = string.Format("Uploading Thumbnail...", video.Id);
            pgb_upload.Value = 100;
            video_id = video.Id;
        }

        void thumbnailInsertRequest_ResponseReceived(ThumbnailSetResponse response)
        {
            lbl_progress.Text = "Youtube Upload Successful!";
            btn_upload_cancel.Enabled = true;
            btn_upload_cancel.Text = @"Finish";
        }

    }

}
