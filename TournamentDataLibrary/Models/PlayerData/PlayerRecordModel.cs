using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentDataLibrary.Models.PlayerData
{
    public class PlayerRecordModel
    {
        public static int defaultElo = 1600;
        public string id { get; set; }
        public string owningUserId { get; set; }
        public bool duplicateRecord { get; set; }
        public string game { get; set; }
        public string uniqueTag { get; set; }
        public string tag { get; set; }
        public int elo { get; set; }
        public string characterName { get; set; }
        public int colorNumber { get; set; }
        public bool usingWirelessController { get; set; }
        public string twitter { get; set; }
        public string region { get; set; }
        public string sponsor { get; set; }
        public string fullSponsor { get; set; }
        public string fullName { get; set; }
        public string misc { get; set; }
        public string pronouns { get; set; }

        public PlayerRecordModel()
        {
            elo = defaultElo;
        }
        public PlayerRecordModel(string toTag, string toTwitter, string toCharacterName, int toColorNumber, int toElo, string toMisc)
        {
            tag = toTag;
            elo = toElo;
            twitter = toTwitter;
            region = "";
            sponsor = "";
            fullSponsor = "";
            fullName = "";
            characterName = toCharacterName;
            colorNumber = toColorNumber;
            misc = toMisc;
        }

    }
}
