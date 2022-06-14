using CharacterLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using Stream_Info_Handler.CharacterSelect;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Stream_Info_Handler.SavePlayer
{
    class updatePlayerInformation
    {
        public List<string> sponsorNames;
        public List<string> sponsorPrefixes;

        public string holdName;
        public string playerId = "";
        public string ownerId;
        public string playerGame = "";
        public bool isCopy = false;
        public bool newPlayer = true;
        public bool needsUnique = false;
        public string gamePath;
        public string saveCharacter;
        public int saveColor;
        public string originalCharacter;
        public int originalColor;

        public updatePlayerInformation(PlayerRecordModel savePlayer, string toPath, string toCharacter, int toColor)
        {
            gamePath = toPath;
            isCopy = savePlayer.duplicateRecord;
            if (savePlayer.uniqueTag != savePlayer.tag)
                needsUnique = true;


            //Set playerid and ownerid
            if (savePlayer.id != null && savePlayer.id != "")
            {
                playerId = savePlayer.id;
                ownerId = savePlayer.owningUserId;
                playerGame = savePlayer.game;
                if (ownerId == global_values.user_id.ToString())
                {
                    newPlayer = false;
                }
            }
            else
            {
                ownerId = global_values.user_id.ToString();
                playerGame = DirectoryManagement.GetGameDirectory();
            }

            saveCharacter = savePlayer.characterName;
            saveColor = savePlayer.colorNumber;
            originalCharacter = toCharacter;
            originalColor = toColor;
        }

        public updatePlayerInformation(string savePlayer, string toPath)
        {
            gamePath = toPath;
            isCopy = false;
            ownerId = global_values.user_id.ToString();
            playerGame = DirectoryManagement.GetGameDirectory();

            saveCharacter = "Random";
            saveColor = 1;
            originalCharacter = saveCharacter;
            originalColor = saveColor;
        }

        public void load_sponsors()
        {
            string sponsorfile = DirectoryManagement.outputDirectory + @"\sponsors.txt";
            if (File.Exists(sponsorfile))
            {
                using (var reader = new StreamReader(sponsorfile))
                {
                    List<string> listA = new List<string>();
                    List<string> listB = new List<string>();
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(',');

                        listA.Add(values[0]);
                        listB.Add(values[1]);
                    }
                    sponsorNames = listA;
                    sponsorPrefixes = listB;
                }
            }
        }

        public void savePlayer(PlayerRecordModel playerInfo, bool newPlayer, bool updateCharacter, SavePlayerForm parentForm)
        {
            PlayerRecordModel savePlayer = playerInfo;
            //Generate a new playerid if this is a new player
            if (newPlayer == true)
                playerId = database_tools.get_new_playerid();
            savePlayer.id = playerId;
            savePlayer.owningUserId = ownerId;
            //Check if this is being saved as a copy
            if (ownerId == global_values.user_id.ToString())
                savePlayer.duplicateRecord = false;
            else
                savePlayer.duplicateRecord = true;


            savePlayer.game = playerGame;

            if (updateCharacter)
            {
                savePlayer.characterName = saveCharacter;
                savePlayer.colorNumber = saveColor;
            }
            else
            {
                savePlayer.characterName = originalCharacter;
                savePlayer.colorNumber = originalColor;
            }


            parentForm.outputPlayer = savePlayer;
            parentForm.outputIsNewPlayer = newPlayer;

            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }

        public Image selectCharacter()
        {
            CharacterData playerCharacter = new CharacterData(saveCharacter, saveColor);
            playerCharacter = GenerateCharacters.GetCharacter(playerCharacter, DirectoryManagement.GetGameDirectory());
            saveCharacter = playerCharacter.characterName;
            saveColor = playerCharacter.characterColor;

            if (saveCharacter != null && saveColor > 0)
            {
                Image newCharacter = Image.FromFile(DirectoryManagement.GetGameDirectory() + @"\" +
                                        saveCharacter + @"\" + saveColor.ToString() + @"\stamp.png");

                return newCharacter;
            }
            else
                return null;
        }
    }
}
