using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TournamentDataLibrary.Models.TournamentData;
using TournamentDataLibrary.Models.PlayerData;

namespace TournamentDataLibrary.Models
{
    public class TournamentDataModel
    {
        public string tournamentName { get; set; }
        /// <summary>
        /// Enable to have matches recorded to the database for win-loss, elo changes, etc
        /// </summary>
        public bool recordMatchData { get; set; }
        public int totalMatches { get; set; }
        public List<MatchDataModel> tournamentMatches { get; set; }
        public PlayerPoolModel playerPool { get; set; }


        public MatchDataModel AddTournamentMatch(string newMatchID)
        {
            totalMatches += 1;
            MatchDataModel newMatch = new MatchDataModel(newMatchID);
            tournamentMatches.Add(newMatch);
            return newMatch;
        }
    }
}
