using System.Collections.Generic;
using TournamentDataLibrary.Models.StreamQueue;

namespace Stream_Info_Handler.AppSettings
{
    static public class GlobalSettings
    {
        /// <summary>
        /// A list of all games that Master Orders is configured to support
        /// </summary>
        public static readonly string[] availableGames =
        {
            "Super Smash Bros Melee",
            "Super Smash Bros for Wii U",
            "Super Smash Bros for Ultimate",
            "Project Plus",
            "Rivals of Aether",
            "Nickelodeon All Star Brawl",
            "Guilty Gear Strive",
            "UNDER NIGHT IN BIRTH Exe Late cl r",
            "Melty Blood Type Lumina"
        };
        /// <summary>
        /// The current game selected to pull characters and players for
        /// </summary>
        public static string selectedGame { get; set; }

        /// <summary>
        /// Determines whether or not Master Orders windows remain on top of all other windows.
        /// </summary>
        public static bool keepWindowsOnTop { get; set; }
        /// <summary>
        /// The current format of the bracket matches being played. Modifies the Stream and Bracket assistant windows to accomodate the format.
        /// Currently accepts:
        /// Singles
        /// Doubles
        /// </summary>
        public static string bracketFormat { get; set; }
        /// <summary>
        /// The file path to the file containing the names of bracket rounds to load in
        /// </summary>
        public static string bracketRounds { get; set; }

        /// <summary>
        /// Contains all stream queues and indentifying data for them
        /// </summary>
        public static List<StreamQueueModel> streamQueueList = new List<StreamQueueModel>();
        /// <summary>
        /// The numeric ID of the currently selected stream queue
        /// </summary>
        public static int streamQueueId { get; set; }
    }
}
