//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2018, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Upload;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Google.Apis.Services;
using System.Reflection;
using YoutubeLibrary;
using Stream_Info_Handler.AppSettings;

namespace Stream_Info_Handler
{
    public delegate void enable_button_event(string check_reenable);
    public partial class frm_uploading : Form
    {
        public event enable_button_event enable_button;
        public string thumbnail_file;
        public string old_thumbnail;
        public string youtube_data;
        public long video_size;
        public long video_multiplier = 100;
        public string reenable_button;
        public bool bypass_lock;
        public string videoId { get; set; }
        public YouTubeService videoUploadService { get; set; }
        public enum uploadStages
        {
            video,
            playlist,
            thumbnail
        }
        public uploadStages uploadProgress { get; set; } = uploadStages.video;

        public frm_uploading(string title, string description, string thumbnail, string vodfile, string reenable, bool bypass)
        {
            InitializeComponent();
            this.TopMost = global_values.keepWindowsOnTop;
            this.CenterToScreen();

            Control.CheckForIllegalCrossThreadCalls = false;
            txt_videotitle.Text = title;
            txt_description.Text = description.Replace("\n", "\r\n");

            if (YoutubeController.enableVideoThumbnails == true)
            {
                thumbnail_file = "upload_thumbnail_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".jpg";
                if (File.Exists(thumbnail_file))
                {
                    File.Delete(thumbnail_file);
                }
                File.Copy(thumbnail, thumbnail_file);

                old_thumbnail = thumbnail;
                pic_thumbnail.Image = Image.FromFile(thumbnail);
                pic_thumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            }

            txt_videofile.Text = vodfile;

            youtube_data = global_values.current_youtube_data;
            if(global_values.new_vod_detected != "")
            {
                btn_upload_video.Enabled = true;
            }
            reenable_button = reenable;
            bypass_lock = bypass;
        }

        private void Form4_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (YoutubeController.enableVideoThumbnails == true)
            {
                pic_thumbnail.Image.Dispose();
                File.Delete(old_thumbnail);
                File.Delete(thumbnail_file);
            }
            enable_button(reenable_button);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_videofile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = DirectoryManagement.vodsDirectory;
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

            if (global_values.allow_upload == true || bypass_lock == true)
            {
                if(YoutubeLibrary.YoutubeController.streamSoftware == "OBS")
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

                Thread videoThread = new Thread(() =>
                {
                    GetYoutubeCredential().Wait();
                    UploadVideo().Wait();
                    AssignPlaylist().Wait();
                    UploadThumbnail().Wait();
                });
                videoThread.IsBackground = true;
                videoThread.Start();
            }
            else
            {
                System.Media.SystemSounds.Asterisk.Play();
            }

        }
        //                    pgb_upload.Value = (int)(progress.BytesSent / video_size * video_multiplier);

        private async Task UploadThumbnail()
        {
            if(uploadProgress != uploadStages.thumbnail)
            {
                return;
            }

            try
            {
                if (YoutubeController.enableVideoThumbnails == true)
                {
                    using (var tStream = new FileStream(thumbnail_file, FileMode.Open))
                    {
                        var tInsertRequest = videoUploadService.Thumbnails.Set(videoId, tStream, "image/jpeg");
                        tInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                        tInsertRequest.ResponseReceived += thumbnailInsertRequest_ResponseReceived;


                        await tInsertRequest.UploadAsync();
                    }
                }
                else
                {
                    thumbnailInsertRequest_ResponseReceived(new ThumbnailSetResponse());
                }
            }
            catch(Exception)
            {
                lbl_progress.Text = string.Format("Error associating thumbnail to video. The video is uploaded. Wait a few minutes and click Retry.", videoId);
                btn_upload_cancel.Enabled = true;
                btn_upload_video.Enabled = true;
                btn_upload_video.Text = "Retry";
            }
        }

        private async Task AssignPlaylist()
        {
            if (uploadProgress != uploadStages.playlist)
            {
                return;
            }
            try
            {
                if (YoutubeController.playlistName != "" && YoutubeController.playlistName != null)
                {
                    // Add a video to the newly created playlist.
                    var newPlaylistItem = new PlaylistItem();
                    newPlaylistItem.Snippet = new PlaylistItemSnippet();
                    newPlaylistItem.Snippet.PlaylistId = YoutubeController.playlistId;
                    newPlaylistItem.Snippet.ResourceId = new ResourceId();
                    newPlaylistItem.Snippet.ResourceId.Kind = "youtube#video";
                    newPlaylistItem.Snippet.ResourceId.VideoId = videoId;
                    newPlaylistItem = await videoUploadService.PlaylistItems.Insert(newPlaylistItem, "snippet").ExecuteAsync();
                    uploadProgress = uploadStages.thumbnail;
                }
            }
            catch (Exception)
            {
                lbl_progress.Text = string.Format("Error associating playlist to video. The video is uploaded. Wait a few minutes and click Retry.", videoId);
                btn_upload_cancel.Enabled = true;
                btn_upload_video.Enabled = true;
                btn_upload_video.Text = "Retry";
            }

        }

        private async Task UploadVideo()
        {
            if (uploadProgress != uploadStages.video)
            {
                return;
            }
            try
            {
                videoUploadService = new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = global_values.youtubeCredential,
                    ApplicationName = Assembly.GetExecutingAssembly().GetName().Name
                });

                var video = new Video();
                video.Snippet = new VideoSnippet();
                video.Snippet.Title = txt_videotitle.Text;
                video.Snippet.Description = txt_description.Text;
                string[] video_tags = YoutubeController.videoTags.Split('\n');

                video.Snippet.Tags = video_tags;
                video.Snippet.CategoryId = "20"; // See https://developers.google.com/youtube/v3/docs/videoCategories/list
                video.Status = new VideoStatus();
                video.Status.PrivacyStatus = "public"; // or "private" or "public"
                var filePath = txt_videofile.Text; // Replace with path to actual movie file.

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    var videosInsertRequest = videoUploadService.Videos.Insert(video, "snippet,status", fileStream, "video/*");
                    videosInsertRequest.ProgressChanged += videosInsertRequest_ProgressChanged;
                    videosInsertRequest.ResponseReceived += videosInsertRequest_ResponseReceived;

                    await videosInsertRequest.UploadAsync();
                    uploadProgress = uploadStages.playlist;
                }
            }
            catch (Exception)
            {
                lbl_progress.Text = string.Format("Failed to upload the video. Is the video still being recorded? Try again.", videoId);
                btn_upload_cancel.Enabled = true;
                btn_upload_video.Enabled = true;
            }
        }

        private async Task GetYoutubeCredential()
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

            await global_values.youtubeCredential.RefreshTokenAsync(CancellationToken.None);
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
            videoId = video.Id;
        }

        void thumbnailInsertRequest_ResponseReceived(ThumbnailSetResponse response)
        {
            lbl_progress.Text = "Youtube Upload Successful!";
            btn_upload_cancel.Enabled = true;
            btn_upload_cancel.Text = @"Finish";
        }
    }

}
