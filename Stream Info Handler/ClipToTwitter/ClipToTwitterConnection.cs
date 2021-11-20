using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Tweetinvi;
using Tweetinvi.Models;
using TwitchLibrary;
using TwitterLibrary;

namespace Stream_Info_Handler.ClipToTwitter
{
    public static class ClipToTwitterConnection
    {
        private static Authentication twitchConnection;
        private static TwitchClip clipMaker;
        public static bool isInitialized = false;
        public static bool twitchEnabled = false;
        public static bool twitterEnabled = false;
        public static string channelToClip;
        private static string clipFile;
        public static bool wasUserNotifiedToPublish = false;

        async public static Task CheckAuthentication()
        {
            try
            {
                string[] accessTokens = File.ReadAllLines("TwitterUserAuth.txt");
                TwitterCredentials userCredentials = new TwitterCredentials(TwitterKeys.APIKey, TwitterKeys.APISecret, accessTokens[0], accessTokens[1]);

                TweetVideo.userClient = new TwitterClient(userCredentials);
                IAuthenticatedUser userAuth = TweetVideo.userClient.Users.GetAuthenticatedUserAsync().Result;
            }
            catch
            {
                await TweetVideo.PromptPinCode();
                // Ask the user to enter the pin code given by Twitter
                string pinCode = "";
                using (TwitterPinForm pinForm =  new TwitterPinForm())
                {
                    pinForm.ShowDialog();
                    pinCode = pinForm.Tag.ToString();
                }
                    await TweetVideo.AuthenticateUser(pinCode);
            }
        }

        public static void Initialize()
        {
            if (isInitialized == false)
            {
                twitchConnection = new Authentication();
                clipMaker = new TwitchClip();

                XDocument xml = XDocument.Load(SettingsFile.settingsFile);

                channelToClip = (string)xml.Root.Element("twitch").Element("channel-to-clip");
                wasUserNotifiedToPublish = !(bool)xml.Root.Element("twitch").Element("remind-to-publish");

            }
        }

        internal static void UpdateChannelToClip(string newChannel)
        {
            channelToClip = newChannel;
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("twitch").Element("channel-to-clip").ReplaceWith(new XElement("channel-to-clip", newChannel));
            xml.Save(SettingsFile.settingsFile);
        }

        public static void UpdateNotifyToPublish(bool setting)
        {
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("twitch").Element("remind-to-publish").ReplaceWith(new XElement("remind-to-publish", (setting).ToString()));
            xml.Save(SettingsFile.settingsFile);
        }

        public static string GetTwitchUser()
        {
            return clipMaker.GetUser();
        }

        public static string GetTwitterUser()
        {
            return TweetVideo.GetUser();
        }

        public static void ConnectToTwitch()
        {
            twitchConnection.GetOauthToken().Wait();
            clipMaker.EnableClipper(twitchConnection);
            twitchEnabled = true;
        }

        public static void ConnectToTwitter()
        {
            CheckAuthentication().Wait();
            twitterEnabled = true;
        }

        public static void DisconnectTwitter()
        {
            if (File.Exists("TwitterUserAuth.txt"))
            {
                File.Delete("TwitterUserAuth.txt");
            }
            twitterEnabled = false;
        }

        public static void GenerateClip()
        {
            if(twitchEnabled)
            {
                string channelId = clipMaker.GetBroadcasterId(channelToClip);
                string clipURL = clipMaker.GenerateClip(twitchConnection);
                clipMaker.OpenEditPage();
            }
        }

        public static void DownloadClip()
        {
            bool clipIsCreated = false;
            string thumbnail = "";
            while (!clipIsCreated)
            {
                try
                {
                    thumbnail = clipMaker.GetClipDownloadLink(clipMaker.clipId);
                    clipIsCreated = true;
                }
                catch
                {
                    clipIsCreated = false;
                }
            }
            clipFile = clipMaker.DownloadClip();
        }

        public static void SendTweet(string message)
        {
            CheckAuthentication().Wait();
            TweetVideo.SendVideo(clipFile, message).Wait();
        }

        public static void DisconnectTwitch()
        {
            MessageBox.Show("Twitch must be disconnected from the native website. Go to Connections in the user settings of the connected account, and click Disconnect next to Master Orders.");
            twitchEnabled = false;
        }

        public static void DeleteClip()
        {
            if(File.Exists(clipFile))
            {
                File.Delete(clipFile);
            }
        }
    }
}
