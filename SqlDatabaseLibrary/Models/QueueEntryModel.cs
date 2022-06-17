using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDatabaseLibrary.Models
{
    public class QueueEntryModel
    {
        public int positionInQueue { get; set; }

        public status matchStatus { get; set; }
        public string roundInBracket { get; set; }
        public List<string> playerIds { get; set; } = new List<string>();

        /// <summary>
        /// normalMatch = Normal status. Not the current match.
        /// currentMatch = The current match
        /// nextMatch = The match being pushed next
        /// currentNextMatch = The current match that is also being pushed next. Used to refresh match info.
        /// </summary>
        public enum status
        {
            normalMatch,
            currentMatch,
            nextMatch,
            currentNextMatch
        }

        public QueueEntryModel(int tonumber, status tostatus, string toround, List<string> toNames)
        {
            positionInQueue = tonumber;
            matchStatus = tostatus;
            roundInBracket = toround;
            playerIds = toNames;
        }
        public QueueEntryModel() 
        {

        }
    }
}
