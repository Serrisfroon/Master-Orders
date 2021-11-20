using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler.AppSettings
{
    public static class DirectoryManagement
    {
        public static string vodsDirectory { get; set; }
        public static string thumbnailDirectory { get; set; }
        public static string sponsorDirectory { get; set; }
        public static string regionDirectory { get; set; }
        public static string characterRostersDirectory { get; set; }
        public static string outputDirectory { get; set; }
        /// <summary>
        /// A dictionary that associates each game name with the directory where its character information is held
        /// </summary>
        public static Dictionary<string, string> gameDirectories { get; set; }
        /// <summary>
        /// Returns the directory associated with the currently selected game
        /// </summary>
        /// <returns></returns>
        public static string GetGameDirectory()
        {
            if (gameDirectories.TryGetValue(GlobalSettings.selectedGame, out string outputDirectoryHold))
            {
                return gameDirectories[GlobalSettings.selectedGame];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Checks a specific game name and directory to make sure it exists and is formatted correctly
        /// </summary>
        /// <param name="selectedGameName">The name of the game to check</param>
        /// <param name="gameRostersDirectory">The directory to check</param>
        /// <returns></returns>
        public static string VerifyGameDirectory(string selectedGameName, string gameRostersDirectory)
        {
            //Return fail if the game directory doesn't exist
            if (!Directory.Exists(gameRostersDirectory))
            {
                return "";
            }

            //Return fail if the game name is not listed as an available game
            string tryFindGameName = Array.Find(GlobalSettings.availableGames, element => element == selectedGameName);
            if (tryFindGameName is null || tryFindGameName == "")
            {
                return "";
            }

            //Check the settings file for the rosters directory
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            //Get an array of the character roster directories
            string[] characterDirectories = Directory.GetDirectories(gameRostersDirectory);

            //Determine the selected game's directory
            string selectedGameDirectory = gameDirectories[selectedGameName];
            //Check if the directory exists
            if (!Directory.Exists(selectedGameDirectory))
            {
                //Show a window to select a new directory
                frm_tables selectNewDirectory = new frm_tables(characterDirectories, selectedGameName);
                //Return fail if a new directory is not selected
                if (selectNewDirectory.ShowDialog() == DialogResult.OK)
                {
                    selectedGameDirectory = selectNewDirectory.outputDirectory;
                }
                else
                {
                    return "";
                }
            }

            //Verify the directory has correct character data
            string[] characters = GetCharactersFromDirectory(selectedGameDirectory);
            //Continually check to verify the character
            while (!CheckCharacterDirectories(selectedGameDirectory))
            {
                //If the characters are not verified, have the user choose a new directory
                MessageBox.Show("The selected directory does not have correct character information for the selected game. Please choose a new directory for " + selectedGameName);
                frm_tables selectNewDirectory = new frm_tables(characterDirectories, selectedGameName);
                //Return fail if a new directory is not selected
                if (selectNewDirectory.ShowDialog() == DialogResult.OK)
                {
                    selectedGameDirectory = selectNewDirectory.outputDirectory;
                }
                else
                {
                    return "";
                }
                //Update the character list before checking again
                characters = GetCharactersFromDirectory(selectedGameDirectory);
            }

            //Update the settings to include the new directory
            gameDirectories[selectedGameName] = selectedGameDirectory;
            SettingsFile.UpdateGameDirectories();

            //Return the game ID
            return selectedGameName;
        }

        /// <summary>
        /// Pulls an array of all characters associated to a game based on a given directory. 
        /// Treats each folder in that directory as a character and each folder name is treated as a character name.
        /// </summary>
        /// <param name="gameCharactersDirectory">The directory to pull characters from</param>
        /// <returns>An array of character names associated to the input directory</returns>
        public static string[] GetCharactersFromDirectory(string gameCharactersDirectory)
        {
            string[] characterList = { "" };

            try
            {
                characterList = Directory.GetDirectories(gameCharactersDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You do not have access to" + gameCharactersDirectory + ". Please update this in the settings and try again.");
                Application.Exit();
            }

            int baseDirectoryLength = gameCharactersDirectory.Length + 1;
            for (int i = 0; i < characterList.Length; i++)
            {
                //Remove the entire directory path to leave only the folder name, which should be the character's name.
                characterList[i] = characterList[i].Substring(baseDirectoryLength, characterList[i].Length - baseDirectoryLength);
            }

            return characterList;
        }


        /// <summary>
        /// Verifies that a roster directory is properly formatted.
        /// </summary>
        /// <param name="gameRosterDirectory">The directory selected to verify</param>
        /// <returns>True if the directory structure is correct.</returns>
        public static bool CheckCharacterDirectories(string gameRosterDirectory)
        {
            List<string> errorMessages = new List<string>();
            bool characterDirectoryCheckPassed = true;

            //Verify the game logo image exists
            if (!File.Exists(gameRosterDirectory + @"\game_logo.png"))
            {
                errorMessages.Add("The selected character roster directory is missing the game logo image (game_logo.png). Please add this file and try again. \nTarget Directory: " + gameRosterDirectory);
                characterDirectoryCheckPassed = false;
            }
            //Pull all the subdirectories. Each one is a character.
            string[] characters = { "" };
            try
            {
                characters = Directory.GetDirectories(gameRosterDirectory);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("You do not have access to" + gameRosterDirectory + ". Please update this in the settings and try again.");
                Application.Exit();
                return false;
            }

            //Loop through each character
            foreach (string character in characters)
            {
                //Pull all subdirectories. Each oene is an alt color.
                string[] colors = Directory.GetDirectories(character);

                //Get the current character's name.
                int dir_length = gameRosterDirectory.Length + 1;
                string character_name = character.Substring(dir_length, character.Length - dir_length);

                //Verify that there is at least one color
                if (colors.Length == 0)
                {
                    errorMessages.Add("A character included in the selected character roster directory does not have any colors in their directory. Please add the colors as subdirectories and try again. \nCharacter Affected: " + character_name + "\nCharacter Directory: " + character);
                    characterDirectoryCheckPassed = false;
                }

                //Loop through each color
                foreach (string color in colors)
                {
                    //Verify that the color directory contains all 3 necessary images
                    if (!File.Exists(color + @"\1080.png"))
                    {
                        errorMessages.Add("A file is missing from a character's color directory. Please ensure that each color directory has all needed image files. \nFile Missing: 1080.png \nCharacter Affected: " + character_name + "\nColor Directory: " + color);
                        characterDirectoryCheckPassed = false;
                    }
                    if (!File.Exists(color + @"\stamp.png"))
                    {
                        errorMessages.Add("A file is missing from a character's color directory. Please ensure that each color directory has all needed image files. \nFile Missing: stamp.png \nCharacter Affected: " + character_name + "\nColor Directory: " + color);
                        characterDirectoryCheckPassed = false;
                    }
                    if (!File.Exists(color + @"\stock.png"))
                    {
                        errorMessages.Add("A file is missing from a character's color directory. Please ensure that each color directory has all needed image files. \nFile Missing: stock.png \nCharacter Affected: " + character_name + "\nColor Directory: " + color);
                        characterDirectoryCheckPassed = false;
                    }
                }
            }

            //Display errors if false
            if(characterDirectoryCheckPassed == false)
            {
                string fullErrorMessage = "Unable to verify the selected directory. The following issues have been found: \n";
                foreach(string error in errorMessages)
                {
                    fullErrorMessage += error + "\n";
                }
            }

            //If all checks are passed, return true.
            return characterDirectoryCheckPassed;
        }
    }
}
