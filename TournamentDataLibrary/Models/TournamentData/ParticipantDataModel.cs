using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentDataLibrary.Models.PlayerData;

namespace TournamentDataLibrary.Models.TournamentData
{
    /// <summary>
    /// Contains the data for a single participant in a tournament match.
    /// </summary>
    public class ParticipantDataModel
    {
        public PlayerRecordModel playerRecord { get; set; }
        public List<CharacterDataModel> characters { get; set; }
    }
}
