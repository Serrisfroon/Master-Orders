using SqlDatabaseLibrary;
using Stream_Info_Handler.StreamAssistant.DataManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YoutubeLibrary;

namespace Stream_Info_Handler.AppSettings
{
    public static class EnableSettings
    {
        public static List<string> EnableGeneralSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            GlobalSettings.keepWindowsOnTop = (bool)xml.Root.Element("general").Element("keep-on-top");
            string loadGameName = (string)xml.Root.Element("database").Element("game-id");
            DirectoryManagement.characterRostersDirectory = (string)xml.Root.Element("directories").Element("character-directory");
            string checkGameName = DirectoryManagement.VerifyGameDirectory(loadGameName, DirectoryManagement.characterRostersDirectory);
            if (checkGameName == "")
            {
                errorMessages.Add($"{ loadGameName } and its directory did not load correctly. Directory: \n{ DirectoryManagement.characterRostersDirectory }");
                GlobalSettings.selectedGame = "";
            }
            else
            {
                GlobalSettings.selectedGame = checkGameName;
            }
            return errorMessages;
        }
        public static List<string> EnableAssistantSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            StreamQueueManager.queueId = (int)xml.Root.Element("database").Element("queue-id");
            GlobalSettings.bracketFormat = (string)xml.Root.Element("general").Element("format");
            GlobalSettings.bracketRounds = (string)xml.Root.Element("general").Element("rounds-file");
            return errorMessages;
        }
        public static List<string> EnableYoutubeSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            YoutubeController.streamSoftware = (string)xml.Root.Element("general").Element("stream-software");
            YoutubeController.enableYoutubeFunctions = (bool)xml.Root.Element("youtube").Element("enable-youtube");
            YoutubeController.enableVideoTitleShortening = (YoutubeController.VideoTitleOptions)(int)xml.Root.Element("general").Element("shorten-title");
            YoutubeController.videoDescription = (string)xml.Root.Element("youtube").Element("default-description");
            YoutubeController.videoTags = (string)xml.Root.Element("youtube").Element("tags");
            YoutubeController.enableVideoThumbnails = (bool)xml.Root.Element("general").Element("enable-thumbnails");
            YoutubeController.titleTemplate = (string)xml.Root.Element("youtube").Element("title-template");
            YoutubeController.copyVideoTitle = (bool)xml.Root.Element("general").Element("copy-title");
            DirectoryManagement.vodsDirectory = (string)xml.Root.Element("directories").Element("vods-directory");
            DirectoryManagement.thumbnailDirectory = (string)xml.Root.Element("directories").Element("thumbnail-directory");
            YoutubeController.playlistName = (string)xml.Root.Element("youtube").Element("playlist-name");
            YoutubeController.playlistId = (string)xml.Root.Element("youtube").Element("playlist-id");
            string backgroundImage = (string)xml.Root.Element("thumbnail-layout").Element("background-image");
            string foregroundImage = (string)xml.Root.Element("thumbnail-layout").Element("foreground-image");
            if (YoutubeController.enableYoutubeFunctions == true && (!File.Exists(YoutubeController.jsonFile)))
            {
                errorMessages.Add($"The JSON file containing YouTube connection information is missing.\nJSON File path: { YoutubeController.jsonFile }");
            }
            if (YoutubeController.enableVideoThumbnails == true && !(File.Exists(backgroundImage) && File.Exists(foregroundImage)))
            {
                errorMessages.Add($"The background and/or foreground thumbnail images are missing.\nBackground image file path: { backgroundImage }\nForeground image file path: { foregroundImage }");
            }
            if (!Directory.Exists(DirectoryManagement.thumbnailDirectory))
            {
                errorMessages.Add($"The thumbnail directory does not exist.\n{ DirectoryManagement.thumbnailDirectory }");
            }
            if (!Directory.Exists(DirectoryManagement.vodsDirectory))
            {
                errorMessages.Add($"The VoDs directory does not exist.\n{ DirectoryManagement.vodsDirectory }");
            }
            return errorMessages;
        }
        public static List<string> EnableOutputSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            TextFileManagement.sponsorSeperator = (string)xml.Root.Element("general").Element("sponsor-seperator");
            DataOutputCaller.automaticUpdates = (bool)xml.Root.Element("general").Element("automatic-updates");
            ImageManagement.enableImageScoreboard = (bool)xml.Root.Element("image-scoring").Element("enable-image-scoring");
            ImageManagement.scoreboardImages[0, 0] = (string)xml.Root.Element("image-scoring").Element("player1-1");
            ImageManagement.scoreboardImages[0, 1] = (string)xml.Root.Element("image-scoring").Element("player1-2");
            ImageManagement.scoreboardImages[0, 2] = (string)xml.Root.Element("image-scoring").Element("player1-3");
            ImageManagement.scoreboardImages[1, 0] = (string)xml.Root.Element("image-scoring").Element("player2-1");
            ImageManagement.scoreboardImages[1, 1] = (string)xml.Root.Element("image-scoring").Element("player2-2");
            ImageManagement.scoreboardImages[1, 2] = (string)xml.Root.Element("image-scoring").Element("player2-3");
            DirectoryManagement.outputDirectory = (string)xml.Root.Element("directories").Element("stream-directory");
            if (ImageManagement.enableImageScoreboard == true && !(File.Exists(ImageManagement.scoreboardImages[0, 0]) && File.Exists(ImageManagement.scoreboardImages[0, 1]) && File.Exists(ImageManagement.scoreboardImages[0, 2]) &&
                File.Exists(ImageManagement.scoreboardImages[1, 0]) && File.Exists(ImageManagement.scoreboardImages[1, 1]) && File.Exists(ImageManagement.scoreboardImages[1, 2])))
            {
                errorMessages.Add($"One or more of the scoreboard images do not exist\n{ ImageManagement.scoreboardImages[0, 0] }\n{ ImageManagement.scoreboardImages[0, 1] }\n{ ImageManagement.scoreboardImages[0, 2] }\n{ ImageManagement.scoreboardImages[1, 0] }\n{ ImageManagement.scoreboardImages[1, 1] }\n{ ImageManagement.scoreboardImages[1, 2] }");
            }
            if (!Directory.Exists(DirectoryManagement.outputDirectory))
            {
                errorMessages.Add($"The output directory does not exist.\n{ DirectoryManagement.outputDirectory }");
            }
            return errorMessages;
        }
        public static List<string> EnableSponsorSettings(bool checkForSponsorEnabled)
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            ImageManagement.enableSponsorImages = (bool)xml.Root.Element("directories").Element("enable-sponsor");
            DirectoryManagement.sponsorDirectory = (string)xml.Root.Element("directories").Element("sponsor-directory");

            if (Directory.Exists(DirectoryManagement.sponsorDirectory) == false && (ImageManagement.enableSponsorImages == true || checkForSponsorEnabled == false))
            {
                errorMessages.Add($"The sponsor directory does not exist.\n{ DirectoryManagement.outputDirectory }");
            }
            return errorMessages;
        }
        public static List<string> EnableRegionSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            ImageManagement.enableRegionImages = (bool)xml.Root.Element("directories").Element("enable-region");
            DirectoryManagement.regionDirectory = (string)xml.Root.Element("directories").Element("region-directory");
            if (Directory.Exists(DirectoryManagement.regionDirectory) == false && ImageManagement.enableRegionImages == true)
            {
                errorMessages.Add($"The region directory does not exist.\n{ DirectoryManagement.outputDirectory }");
            }
            return errorMessages;
        }
        public static List<string> EnableThumbnailSettings()
        {
            List<string> errorMessages = new List<string>();
            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            ImageManagement.thumbnailConfiguration.backgroundImage = (string)xml.Root.Element("thumbnail-layout").Element("background-image");
            ImageManagement.thumbnailConfiguration.foregroundImage = (string)xml.Root.Element("thumbnail-layout").Element("foreground-image");
            ImageManagement.thumbnailConfiguration.thumbnailFont = (string)xml.Root.Element("thumbnail-layout").Element("font");
            ImageManagement.thumbnailConfiguration.characterXOffset[0] = (int)xml.Root.Element("thumbnail-layout").Element("character-1-xoffset");
            ImageManagement.thumbnailConfiguration.characterYOffset[0] = (int)xml.Root.Element("thumbnail-layout").Element("character-1-yoffset");
            ImageManagement.thumbnailConfiguration.characterXOffset[1] = (int)xml.Root.Element("thumbnail-layout").Element("character-2-xoffset");
            ImageManagement.thumbnailConfiguration.characterXOffset[1] = (int)xml.Root.Element("thumbnail-layout").Element("character-2-yoffset");
            ImageManagement.thumbnailConfiguration.playerNameXOffset[0] = (int)xml.Root.Element("thumbnail-layout").Element("name-1-xoffset");
            ImageManagement.thumbnailConfiguration.playerNameYOffset[0] = (int)xml.Root.Element("thumbnail-layout").Element("name-1-yoffset");
            ImageManagement.thumbnailConfiguration.playerNameSize[0] = (int)xml.Root.Element("thumbnail-layout").Element("name-1-size");
            ImageManagement.thumbnailConfiguration.playerNameXOffset[1] = (int)xml.Root.Element("thumbnail-layout").Element("name-2-xoffset");
            ImageManagement.thumbnailConfiguration.playerNameYOffset[1] = (int)xml.Root.Element("thumbnail-layout").Element("name-2-yoffset");
            ImageManagement.thumbnailConfiguration.playerNameSize[1] = (int)xml.Root.Element("thumbnail-layout").Element("name-2-size");

            ImageManagement.thumbnailConfiguration.roundXOffset = (int)xml.Root.Element("thumbnail-layout").Element("round-xoffset");
            ImageManagement.thumbnailConfiguration.roundYOffset = (int)xml.Root.Element("thumbnail-layout").Element("round-yoffset");
            ImageManagement.thumbnailConfiguration.roundSize = (int)xml.Root.Element("thumbnail-layout").Element("round-size");
            ImageManagement.thumbnailConfiguration.showDateOnThumbnail = (bool)xml.Root.Element("thumbnail-layout").Element("enable-date");
            ImageManagement.thumbnailConfiguration.dateXOffset = (int)xml.Root.Element("thumbnail-layout").Element("date-xoffset");
            ImageManagement.thumbnailConfiguration.dateYOffset = (int)xml.Root.Element("thumbnail-layout").Element("date-yoffset");
            ImageManagement.thumbnailConfiguration.dateSize = (int)xml.Root.Element("thumbnail-layout").Element("date-size");
            ImageManagement.thumbnailConfiguration.patchVersion = (string)xml.Root.Element("thumbnail-layout").Element("patch-version");
            ImageManagement.thumbnailConfiguration.patchXOffset = (int)xml.Root.Element("thumbnail-layout").Element("patch-xoffset");
            ImageManagement.thumbnailConfiguration.patchYOffset = (int)xml.Root.Element("thumbnail-layout").Element("patch-yoffset");
            ImageManagement.thumbnailConfiguration.patchSize = (int)xml.Root.Element("thumbnail-layout").Element("patch-size");

            return errorMessages;
        }
    }
}
