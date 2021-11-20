using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using TwitchLib.Api;
using TwitchLib.Api.Helix;
using System.Diagnostics;
using TwitchLib.Api.Core.RateLimiter;
using TwitchLib.Api.Core;
using TwitchLib.Api.Core.HttpCallHandlers;
//using TwitchLib.Api.V5.Models.Users;
using TwitchLib.Api.V5;
using System.Net;
using System.IO;

namespace TwitchLibrary
{
    public class TwitchClip
    {
        public BypassLimiter rateLimiter;
        public TwitchWebRequest callHandler;
        public TwitchLib.Api.Helix.Clips clipGenerator;
        public TwitchLib.Api.V5.Users userModel;
        public string broadcasterName;
        private string broadcasterId;
        public string clipURL;
        public string clipId;
        public string clipDownloadFolder = Directory.GetCurrentDirectory();
        public string editUrl;
        public ApiSettings settings;

        //get broadcaster id based off username
        public void EnableClipper(Authentication toSettings)
        {
            rateLimiter = BypassLimiter.CreateLimiterBypassInstance();
            callHandler = new TwitchWebRequest();
            clipGenerator = new TwitchLib.Api.Helix.Clips(toSettings.api, rateLimiter, callHandler);
            userModel = new TwitchLib.Api.V5.Users(toSettings.api, rateLimiter, callHandler);
            settings = toSettings.api;
        }
        public string GetBroadcasterId(string username)
        {
            var userBroadcaster = userModel.GetUserByNameAsync(username).Result;
            broadcasterId = userBroadcaster.Matches[0].Id;
            return broadcasterId;
        }
        public string GenerateClip(Authentication twitchConnection)
        {        
            var createdClip = clipGenerator.CreateClipAsync(broadcasterId, twitchConnection.api.AccessToken).Result;

            clipId = createdClip.CreatedClips[0].Id;
            editUrl = createdClip.CreatedClips[0].EditUrl;
            return editUrl;
            
        }

        public string GetClipDownloadLink(string rawId)
        {
            var findClip = clipGenerator.GetClipsAsync(new List<string>() { rawId }).Result;
            string thumbnailUrl = findClip.Clips[0].ThumbnailUrl;
            //example thumbnail URL:
            //https://clips-media-assets2.twitch.tv/41624861374-offset-602-preview-480x272.jpg
            //https://production.assets.clips.twitchcdn.net/41624861374-offset-1444.mp4
            //Remove the last - and everything after
            int cutoffPoint = thumbnailUrl.LastIndexOf('-');
            clipURL = thumbnailUrl.Substring(0, cutoffPoint);

            //Remove -preview
            cutoffPoint = clipURL.LastIndexOf('-');
            clipURL = clipURL.Substring(0, cutoffPoint);

            //add .mp4
            clipURL = clipURL + ".mp4";

            return clipURL;
        }

        public string DownloadClip()
        {
            string clipFile = clipDownloadFolder + @"\" + clipId + ".mp4";
            using (var client = new WebClient())
            {
                client.DownloadFile(clipURL, clipFile);
            }
            return clipFile;
        }

        public void OpenEditPage()
        {
            Process.Start(editUrl);
        }
        public string GetUser()
        {
            var getUser = userModel.GetUserAsync(settings.AccessToken).Result;
            return getUser.Name;
        }
    }
}
