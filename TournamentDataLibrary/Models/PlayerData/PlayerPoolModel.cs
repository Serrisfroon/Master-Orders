using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentDataLibrary.Models.PlayerData
{
    public class PlayerPoolModel
    {
        /// <summary>
        /// The master list of players used by different tools.
        /// </summary>
        public List<PlayerRecordModel> playerRecords { get; set; }
        /// <summary>
        /// A companion list to the above for quick reference to each player's tag
        /// </summary>
        public Dictionary<string, PlayerRecordModel> playerTagsRecords { get; set; }

        public PlayerRecordModel FindPlayerRecordFromId(string inputId)
        {
            //lambda expression to search the playerRecords list by a class attribute(id)
            PlayerRecordModel foundPlayer = playerRecords.SingleOrDefault(x => x.id == inputId);

            if(foundPlayer == null)
            {
                throw new NullReferenceException("Tried searching for a player Id that doesn't exist");
            }

            return foundPlayer;
        }
        public PlayerRecordModel FindPlayerRecordFromSmashggId(string inputId)
        {
            //lambda expression to search the playerRecords list by a class attribute(id)
            PlayerRecordModel foundPlayer = playerRecords.SingleOrDefault(x => x.smashggId == inputId);

            if (foundPlayer == null)
            {
                throw new NullReferenceException($"Tried searching for Smashgg Id { inputId }, but no result was found.");
            }

            return foundPlayer;
        }
    }
}
