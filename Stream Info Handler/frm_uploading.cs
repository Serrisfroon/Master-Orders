//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only
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
    public delegate void enable_button_event();
    public partial class frm_uploading : Form
    {
        public event enable_button_event enable_button;
        public static string video_id;
        public static string thumbnail_file;

        public long video_size;
        public long video_multiplier = 100;

        public frm_uploading(string title, string description, string thumbnail)
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            txt_videotitle.Text = title;
            txt_description.Text = description;
            thumbnail_file = "upload_thumbnail_" + date.ToString("MMddyyyyHHmmss") + ".jpg";
            if (File.Exists(thumbnail_file))
            {
                File.Delete(thumbnail_file);
            }
            File.Copy(thumbnail, thumbnail_file);

            pic_thumbnail.Image = Image.FromFile(thumbnail);
            pic_thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;        
        }

        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            enable_button();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_videofile_Click(object sender, EventArgs e)
        {
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

            using (var tStream = new FileStream(thumbnail_file, FileMode.Open))
            {
                var tInsertRequest = youtubeService.Thumbnails.Set(video_id, tStream, "image/jpeg");
                tInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;


                await tInsertRequest.UploadAsync();
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
            lbl_progress.Text = string.Format("Youtube Upload Successful!", video.Id);
            pgb_upload.Value = 100;
            video_id = video.Id;
            btn_upload_cancel.Enabled = true;
            btn_upload_cancel.Text = @"Finish";
        }

    }
    
}
