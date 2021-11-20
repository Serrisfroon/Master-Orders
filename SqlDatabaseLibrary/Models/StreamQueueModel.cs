using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDatabaseLibrary.Models
{
    public class StreamQueueModel
    {
        /// <summary>
        /// The numeric ID of the queue. Used in database to pull correct games
        /// </summary>
        public int queueId { get; set; }
        /// <summary>
        /// The name of the queue in question. Displayed in app
        /// </summary>
        public string queueName { get; set; }
        /// <summary>
        /// The game that the queue is using. Has to be assigned to a game to pull correct player data
        /// </summary>
        public string queueGame { get; set; }


        public StreamQueueModel(int toid, string toname, string togame)
        {
            queueId = toid;
            queueName = toname;
            queueGame = togame;
        }
        public StreamQueueModel() { }
    }
}
