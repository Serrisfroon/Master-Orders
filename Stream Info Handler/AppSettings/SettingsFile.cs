using SqlDatabaseLibrary;
using Stream_Info_Handler.AppSettings;
using Stream_Info_Handler.AppSettings.GeneralSettings;
using Stream_Info_Handler.Startup;
using Stream_Info_Handler.StreamAssistant.DataManagement;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeLibrary;

namespace Stream_Info_Handler.AppSettings
{
    public static class SettingsFile
    {
        public static readonly string settingsFileDirectory = @"C:\Users\Public\Stream Info Handler\";
        public static readonly string settingsFile = settingsFileDirectory + "settings.xml";
        public static readonly int settingsVersion = 16;

        /// <summary>
        /// Initialize login sequence. Returns whether the username/password should be remember and outputs the saved username and password if so
        /// </summary>
        /// <param name="username">Outputs the username if it should be remembered. Otherwise outputs and empty string.</param>
        /// <param name="password">Outputs the password if it should be remembered. Otherwise outputs and empty string.</param>
        /// <returns>whether or not the username/password is remembered</returns>
        public static bool Initialize(out string username, out string password)
        {
            //Create the setting file directory if it doesn't exist
            if (!Directory.Exists(settingsFileDirectory))
            {
                Directory.CreateDirectory(settingsFileDirectory);
            }

            username = "";
            password = "";
            bool rememberUsername = false;

            //Check if the settings file exists
            if (File.Exists(settingsFile))
            {
                //Update the settings
                UpdateSettings();

                //Read the settings
                XDocument xml = XDocument.Load(settingsFile);

                //Check if the login was set to be remembered
                rememberUsername = Convert.ToBoolean((string)xml.Root.Element("login").Element("remember-login"));
                if (rememberUsername)
                {
                    //Retrieve the login credentials
                    username = (string)xml.Root.Element("login").Element("username");
                    password = (string)xml.Root.Element("login").Element("password");
                }
            }
            else
            {
                //If the settings file doesn't exist, create it
                CreateSettings();
            }
            return rememberUsername;
        }

        /// <summary>
        /// Updates the username, password, and remember checkbox status as needed.
        /// </summary>
        public static void LogInUpdate(bool rememberLogIn, string username, string password)
        {
            XDocument xml = XDocument.Load(settingsFile);
            xml.Root.Element("login").Element("remember-login").ReplaceWith(new XElement("remember-login", rememberLogIn.ToString()));

            if ((string)xml.Root.Element("login").Element("username") != username)
            {
                if (rememberLogIn == true)
                {
                    xml.Root.Element("login").Element("username").ReplaceWith(new XElement("username", username));
                    xml.Root.Element("login").Element("password").ReplaceWith(new XElement("password", password));
                }
            }

            xml.Save(settingsFile);
        }

        /// <summary>
        /// Updates the settings file as needed to ensure it contains new settings as they are added. "roster-directories"
        /// Where x is the largest value case, update goto case to be case x+1
        /// Add case x+1 and add new updates there. end with goto case 0;
        /// </summary>
        public static void UpdateSettings()
        {
            //Load the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            //Read the settings version
            int version = (int)xml.Root.Element("general").Element("settings-version");

            //Update the settings file based on its current version
            switch (version)
            {
                case 14:
                    xml.Root.Add(new XElement("twitch", new XElement("channel-to-clip", ""), new XElement("remind-to-publish", "true")));
                    goto case 15;
                case 15:
                    string rawGameDirectories = (string)xml.Root.Element("database").Element("roster-directories");
                    xml.Root.Element("directories").Add(new XElement("roster-directories", rawGameDirectories));
                    xml.Root.Element("database").Element("roster-directories").Remove();
                    goto case 0;
                case 0:
                    //Update version
                    xml.Root.Element("general").Element("settings-version").ReplaceWith(new XElement("settings-version", settingsVersion));
                    break;
            }

            //Save the settings file
            xml.Save(SettingsFile.settingsFile);
        }

        /// <summary>
        /// Create the settings file with default values.
        /// </summary>
        public static void CreateSettings()
        {
            string default_youtube_description =
                "*tournament* | *date* \r\n" +
                "*bracket*\r\nRomeoville, Illinois \r\n" +
                "Organized and streamed by UGS Gaming \r\n" +
                "Watch live at https://www.twitch.tv/ugsgaming \r\n" +
                "Follow us and our players on Twitter! \r\n" +
                "@UGSGamingLLC \r\n" +
                "*player1*: *twitter1* \r\n" +
                "*player2*: *twitter2*";
            default_youtube_description = Regex.Replace(default_youtube_description, @"\r\n|\n|\r", Environment.NewLine);
            XDocument doc = new XDocument(
                new XElement("Master-Orders-Settings",
                new XElement("login",
                     new XElement("remember-login", false),
                     new XElement("username", ""),
                     new XElement("password", ""),
                     new XElement("keep-open", false)
                     ),
                new XElement("database",
                     new XElement("queue-id", -1),
                     new XElement("game-id", -1)
                     ),
                new XElement("directories",
                     new XElement("character-directory", ""),
                     new XElement("roster-directories", InitializeRosterDirectories()),
                     new XElement("roster-selection", ""),
                     new XElement("stream-directory", ""),
                     new XElement("thumbnail-directory", ""),
                     new XElement("vods-directory", ""),
                     new XElement("uldata-directory", ""),
                     new XElement("enable-sponsor", false),
                     new XElement("sponsor-directory", ""),
                     new XElement("enable-region", false),
                     new XElement("region-directory", "")
                     ),
                new XElement("youtube",
                     new XElement("enable-youtube", false),
                     new XElement("username", "Master Orders"),
                     new XElement("json-file", ""),
                     new XElement("playlist-name", ""),
                     new XElement("playlist-id", ""),
                     new XElement("default-description", default_youtube_description),
                     new XElement("tags", ""),
                     new XElement("title-template", @"*tournament* - *round* - *player1*(*character1*) Vs. *player2*(*character2*)")
                    ),
                new XElement("image-scoring",
                     new XElement("enable-image-scoring", false),
                     new XElement("player1-1", ""),
                     new XElement("player1-2", ""),
                     new XElement("player1-3", ""),
                     new XElement("player2-1", ""),
                     new XElement("player2-2", ""),
                     new XElement("player2-3", "")
                    ),
                new XElement("thumbnail-layout",
                     new XElement("background-image", ""),
                     new XElement("foreground-image", ""),
                     new XElement("font", "Arial"),
                     new XElement("character-1-xoffset", "0"),
                     new XElement("character-1-yoffset", "0"),
                     new XElement("character-2-xoffset", "0"),
                     new XElement("character-2-yoffset", "0"),
                     new XElement("name-1-xoffset", "0"),
                     new XElement("name-1-yoffset", "0"),
                     new XElement("name-1-size", "120"),
                     new XElement("name-2-xoffset", "0"),
                     new XElement("name-2-yoffset", "0"),
                     new XElement("name-2-size", "120"),
                     new XElement("round-xoffset", "0"),
                     new XElement("round-yoffset", "0"),
                     new XElement("round-size", "60"),
                     new XElement("enable-date", false),
                     new XElement("date-xoffset", "0"),
                     new XElement("date-yoffset", "0"),
                     new XElement("date-size", "80"),
                     new XElement("enable-patch", false),
                     new XElement("patch-xoffset", "0"),
                     new XElement("patch-yoffset", "0"),
                     new XElement("patch-size", "80"),
                     new XElement("patch-version", "v3.0.0")
                    ),
                new XElement("general",
                     new XElement("automatic-updates", true),
                     new XElement("stream-software", "XSplit"),
                     new XElement("format", "Singles"),
                     new XElement("settings-version", settingsVersion),
                     new XElement("enable-thumbnails", false),
                     new XElement("copy-title", false),
                     new XElement("shorten-title", "0"),
                     new XElement("tournament-name", ""),
                     new XElement("bracket-link", ""),
                     new XElement("keep-on-top", false),
                     new XElement("sponsor-seperator", @" | "),
                     new XElement("rounds-file", ""),
                     new XElement("top8-settings", "")
                    ),
                new XElement("seeding",
                    new XElement("base-elo", "1600"),
                    new XElement("bo3-kfactor", "50"),
                    new XElement("bo5-kfactor", "75"),
                    new XElement("point-differential", "400"),
                    new XElement("new-player-multiplier", "1.8"),
                    new XElement("new-player-games", "25"),
                    new XElement("verteran-elo", "2400")
                    ),
                new XElement("bracket-integration",
                    new XElement("enable-bracket", "false"),
                    new XElement("api-key", ""),
                    new XElement("website-type", "50")
                    ),
                new XElement("twitch",
                    new XElement("channel-to-clip", ""),
                    new XElement("remind-to-publish", "true")
                    )
                ));

            doc.Save(SettingsFile.settingsFile);
        }

        public static string InitializeRosterDirectories()
        {
            string rawDirectories = "";
            //Loop through each accessible game
            foreach (string game in GlobalSettings.availableGames)
            {
                rawDirectories = rawDirectories + ";" + game + ",";
            }
            rawDirectories = rawDirectories.Remove(0, 1);
            return rawDirectories;
        }

        public static bool LoadSettings(FormManagement.FormNames toolToLoad)
        {
            List<string> errorMessages = new List<string>();
            bool confirmSettings = true;

            DirectoryManagement.gameDirectories = LoadRosterDirectories();
            StreamQueueManager.ImportStreamQueues();

            switch (toolToLoad)
            {
                case FormManagement.FormNames.StreamAssistant:
                    errorMessages.AddRange(EnableSettings.EnableGeneralSettings());
                    errorMessages.AddRange(EnableSettings.EnableAssistantSettings());
                    errorMessages.AddRange(EnableSettings.EnableYoutubeSettings());
                    errorMessages.AddRange(EnableSettings.EnableOutputSettings());
                    errorMessages.AddRange(EnableSettings.EnableSponsorSettings(true));
                    errorMessages.AddRange(EnableSettings.EnableRegionSettings());
                    errorMessages.AddRange(EnableSettings.EnableThumbnailSettings());
                    break;
                case FormManagement.FormNames.BracketAssistant:
                    errorMessages.AddRange(EnableSettings.EnableGeneralSettings());
                    errorMessages.AddRange(EnableSettings.EnableAssistantSettings());
                    errorMessages.AddRange(EnableSettings.EnableOutputSettings());
                    break;
                case FormManagement.FormNames.Top8Generator:
                    errorMessages.AddRange(EnableSettings.EnableGeneralSettings());
                    errorMessages.AddRange(EnableSettings.EnableOutputSettings());
                    errorMessages.AddRange(EnableSettings.EnableSponsorSettings(false));
                    break;
                case FormManagement.FormNames.PlayerManager:

                    break;
                case FormManagement.FormNames.Settings:
                    EnableSettings.EnableGeneralSettings();
                    EnableSettings.EnableAssistantSettings();
                    EnableSettings.EnableYoutubeSettings();
                    EnableSettings.EnableOutputSettings();
                    EnableSettings.EnableSponsorSettings(true);
                    EnableSettings.EnableRegionSettings();
                    EnableSettings.EnableThumbnailSettings();
                    break;
            }

            //If not verified, have the user modify the settings
            if (errorMessages.Count > 0 && toolToLoad != FormManagement.FormNames.Settings)
            {
                confirmSettings = false;
                string errorMessage = "Master Orders' settings are not properly configured to use this feature. Errors found in settings:\n";
                foreach (string error in errorMessages)
                {
                    errorMessage += error + "\n";
                }
                errorMessage += "Now opening the settings window.";
                MessageBox.Show(errorMessage, "Incorrect Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                var settings = new GeneralSettingsForm(toolToLoad);
                if (settings.ShowDialog() != DialogResult.OK)
                    confirmSettings = true;
            }

            return confirmSettings;
        }
        public static void UpdateGameDirectories()
        {
            //Build the raw comma-semicolon seperate string of games and their directories
            string rawDirectories = "";
            for (int i = 0; i < GlobalSettings.availableGames.Length; i++)
            {
                // Fill out gameDirectiories with empty strings if not dictionary
                // Otherwise do a tryfind to not error out
                rawDirectories = rawDirectories + ";" + GlobalSettings.availableGames[i] + "," + DirectoryManagement.gameDirectories[GlobalSettings.availableGames[i]];
            }
            rawDirectories = rawDirectories.Remove(0, 1);   //Remove the starting semicolon

            //Save to settings
            XDocument xml = XDocument.Load(settingsFile);
            xml.Root.Element("directories").Element("roster-directories").ReplaceWith(new XElement("roster-directories", rawDirectories));
            xml.Save(settingsFile);
        }
        /// <summary>
        /// Pulls all bracket rounds from the text file saved to settings
        /// </summary>
        /// <returns>An array of all the rounds taken from the text file</returns>
        public static string[] LoadBracketRounds()
        {
            XDocument xml = XDocument.Load(settingsFile);
            string bracketRoundsFile = (string)xml.Root.Element("general").Element("rounds-file");
            if (File.Exists(bracketRoundsFile))
            {
                return File.ReadAllLines(bracketRoundsFile);
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Load all of the game roster directories saved into the settings file.
        /// </summary>
        /// <returns>A dictionary containing each directory keyed to its corresponding game's name</returns>
        public static Dictionary<string, string> LoadRosterDirectories()
        {
            Dictionary<string, string> outputRosterDirectories = new Dictionary<string, string>();

            XDocument xml = XDocument.Load(settingsFile);
            string rawGameDirectories = (string)xml.Root.Element("directories").Element("roster-directories");
            string[] splitGameDirectories = rawGameDirectories.Split(';');
            foreach (string game in splitGameDirectories)
            {
                string[] splitGamesFromDirectories = game.Split(',');
                if (Directory.Exists(splitGamesFromDirectories[1]) || splitGamesFromDirectories[1]=="")
                {
                    outputRosterDirectories.Add(splitGamesFromDirectories[0], splitGamesFromDirectories[1]);
                }
            }

            return outputRosterDirectories;
        }
    }
}
