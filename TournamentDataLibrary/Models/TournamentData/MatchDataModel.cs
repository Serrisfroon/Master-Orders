using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentDataLibrary.Models.TournamentData
{
    /// <summary>
    /// Contains the data for a tournament match
    /// </summary>
    public class MatchDataModel
    {
        public List<TeamDataModel> matchTeams { get; set; }
        public string matchDate { get; set; }
        public string matchTitle { get; set; }
        public string matchID {
            get
            {
                return _matchID;
            }
        }
        private string _matchID;
        public MatchDataModel(string setID)
        {
            _matchID = setID;
        }
    }
}
