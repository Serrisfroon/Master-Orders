using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TournamentDataLibrary.Models.TournamentData
{
    /// <summary>
    /// Contains data and methods related to the single character used by a participant in a match.
    /// </summary>
    public class CharacterDataModel
    {
        public string characterName { get; set; }
        public string directory { get; set; }

        public string Get1080()
        {
            return directory + @"\1080.png";
        }
        public string GetStamp()
        {
            return directory + @"\stamp.png";
        }
        public string GetStock()
        {
            return directory + @"\stock.png";
        }

    }
}
