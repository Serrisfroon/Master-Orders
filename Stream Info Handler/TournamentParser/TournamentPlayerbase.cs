
/*
using System.Collections.Generic;

namespace Stream_Info_Handler
{
    /// <summary>
    /// Holds all players in the tournament.
    /// </summary>
    class TournamentPlayerbase
    {
        /// <summary>
        /// The list of all players in the tournament. Obtained from the tournament host API(Challonge).
        /// </summary>
        List<TournamentPlayer> players = new List<TournamentPlayer>();

        /// <summary>
        /// Gets a list of all players from the tournament host API
        /// </summary>
        /// <param name="seperator">The string used to denote a seperation from team prefix and tag</param>
        /// <returns></returns>
        public List<TournamentPlayer> GetPlayers(string seperator)
        {
            //Pull all players from API
            List<TournamentPlayer> foundPlayers = new List<TournamentPlayer>();
            //parse the tag out from the given tag.
            //Store the tag and team prefix seperate

            //Link all players with an identical tag
            foundPlayers.ForEach(linkPlayer(ref ));
            foreach(TournamentPlayer foundPlayer in foundPlayers)
            {
                linkPlayer(foundPlayer, )
            }
        }

        /// <summary>
        /// Attempts to link a new player record from the Tournament Host API to a Master Orders record.
        /// </summary>
        /// <param name="player">The API player record</param>
        public void linkPlayer(ref TournamentPlayer player)
        {
            //Find all index of the tag within the playerbase
            //Needs to account for team prefix
            List<PlayerRecordModel> playerIndex = global_values.roster.FindAll();

            int foundPlayers = playerIndex.Count;


            switch(foundPlayers)
            {
                //If no records with the same name have been found, mark as unlinked and return
                case 0:
                    player.linkStatus = linkStatus.unlinked;
                    return;
                //If exactly 1 player with the same name is found, link them, mark as linked, and return
                case 1:
                    player.masterordersId = playerIndex[0].id;
                    player.linkStatus = linkStatus.linked;
                    return;
                //If more than 1 player with the same name is found, store the player records, mark as such, and return
                default:
                    //Check the possible links to see if the team prefix matches between any of them



                    player.possibleLinks.AddRange(playerIndex);
                    player.linkStatus = linkStatus.chooselink;
                    return;
            }

        }
    }
}
*/