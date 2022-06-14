
using SqlDatabaseLibrary.Models;
using System.Collections.Generic;

namespace Stream_Info_Handler
{
    /// <summary>
    /// Determines whether or not the record is linked to a Master Orders record.
    /// unlinked = the record in has no found associations in Master Orders.
    /// linked = the record has a designated association in Master Orders.
    /// chooselink = the record needs an association manually selected from a given pool.
    /// </summary>
    public enum linkStatus
    {
        unlinked,
        linked,
        chooselink
    };

    /// <summary>
    /// Holds data for a single player in a tournament
    /// </summary>
    class TournamentPlayer
    {
        /// <summary>
        /// The ID associated with the player used by the API linking to the tournament host (Challonge).
        /// </summary>
        public string apiId;
        /// <summary>
        /// The ID associated with the player used by Master Orders.
        /// </summary>
        public string masterordersId;
        /// <summary>
        /// The player's tag, first given by the API and confirmed within Master Orders
        /// </summary>
        public string tag;
        /// <summary>
        /// The prefix of a team (UGS) that was parsed out of the tag. Used to automatically choose a record between multiple possibilities.
        /// </summary>
        public string teamPrefix;
        /// <summary>
        /// Determines whether or not the record is linked to a Master Orders record.
        /// unlinked = the record in has no found associations in Master Orders.
        /// linked = the record has a designated association in Master Orders.
        /// chooselink = the record needs an association manually selected from a given pool.
        /// </summary>
        public linkStatus linkStatus;

        /// <summary>
        /// In the case that multiple records with the same name are found in the Master Orders playerbase, one will need 
        /// to be manually selected as the record to associate with this player record coming from the API.
        /// These possible links are stored in this list.
        /// </summary>
        public List<PlayerRecordModel> possibleLinks = new List<PlayerRecordModel>();
    }
}


