using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using Stream_Info_Handler.StreamAssistant;
using System.Collections.Generic;
using System.IO;
using SqlDatabaseLibrary.Models;

namespace Stream_Info_Handler
{
    public static class global_values
    {
        public static bool enable_region;
        public static string region_directory;
        public static bool enable_sponsor;
        public static string sponsor_directory;
        public static string settings_file = @"C:\Users\Public\Stream Info Handler\settings.xml";
        public static string youtube_description;
        public static bool enableYoutubeFunctions;
        public static bool enableYoutubeThumbnails;
        public static bool copyVideoTitle;
        public static string reenable_upload = "";
        public static bool allow_upload = true;
        public static string current_youtube_data;
        public static FileSystemWatcher vod_monitor;
        public static string new_vod_detected = "";
        public static string[] game_info = { "", "" };
        public static bool enable_image_scoreboard;
        public static string[,] score_image = new string[2, 3] { { "", "", "" }, { "", "", "" } };
        public static string outputDirectory;
        public static string thumbnail_directory;
        public static string json_file = Directory.GetCurrentDirectory() + @"\Resources\youtube_upload.json";
        public static string youtube_username = "Master Orders";
        public static string vods_directory;
        public static bool automaticUpdates = true;
        public static string playlist_name;
        public static string playlist_id;
        public static string format;
        public static UserCredential youtubeCredential;
        public static FileDataStore store = new FileDataStore("Master Orders OAuth");
        public static string youtube_tags;
        public static string bracketRoundsFile;
        public static string sponsor_seperator;


        public static int enable_shorten_title;
        public static bool keepWindowsOnTop;

        public static StreamAssistantForm stream_assistant;
        public static frm_bracket_assistant bracket_assistant;
        public static frm_othertools tools_form;
        public static frm_playermanager playermanager_form;
        public static int settings_version = 15;


        //User-Level Settings
        public static int user_id;
        public static int queue_id;
        //public static string[,] stream_queue;

    }
}
