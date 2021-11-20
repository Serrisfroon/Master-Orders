using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace YoutubeLibrary
{
    public static class YoutubeController
    {
        public static string jsonFile = Directory.GetCurrentDirectory() + @"\Resources\youtube_upload.json";
        public static VodMonitor vodMonitor { get; set; }
        public static string videoDescription { get; set; }
        public static string videoTags { get; set; }
        public static bool enableVideoThumbnails { get; set; }
        public static bool copyVideoTitle { get; set; }
        public static string playlistName { get; set; }
        public static string playlistId { get; set; }
        public static string titleTemplate { get; set; }
        public static VideoTitleOptions enableVideoTitleShortening { get; set; }
        public enum VideoTitleOptions
        {
            neverShorten,
            alwaysShorten,
            doublesOnly
        }

        private static bool _enableYoutubeFunctions;
        public static bool enableYoutubeFunctions 
        {
            get
            {
                return _enableYoutubeFunctions;
            }
            set
            {
                _enableYoutubeFunctions = value;
                if(vodMonitor != null)
                {
                    vodMonitor.enableFunctions = value;
                }
            }
        }

        private static string _streamSoftware;
        public static string streamSoftware 
        { 
            get
            {
                return _streamSoftware;
            }
            set
            {
                _streamSoftware = value;
                if(vodMonitor != null)
                {
                    vodMonitor.streamSoftware = value;
                }
            }
        }
        


        public static void InitializeVodMonitor(string toDirectoryToWatch, Control toTargetControl)
        {
            vodMonitor = new VodMonitor(toDirectoryToWatch, toTargetControl, enableYoutubeFunctions);
        }

    }
}
