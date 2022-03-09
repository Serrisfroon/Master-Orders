using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentDataLibrary.Models.TournamentData
{
    /// <summary>
    /// Contains data and methods for a single team in a match.
    /// </summary>
    public class TeamDataModel
    {
        public List<ParticipantDataModel> teamParticipants { get; set; }
        public string teamName { get; set; }

        /// <summary>
        /// Sets teamName to the default (playertag1 + playertag2) and returns it.
        /// </summary>
        /// <returns>teamName</returns>
        public string DefaultTeamName()
        {
            StringBuilder teamNameBuilder = new StringBuilder(); 
            foreach(ParticipantDataModel participant in teamParticipants)
            {
                teamNameBuilder.Append(participant.playerRecord.tag + " + ");
            }
            teamNameBuilder.Length -= 3;
            teamName = teamNameBuilder.ToString();

            return teamName;
        }
    }
}
