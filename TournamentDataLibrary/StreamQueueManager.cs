using MySql.Data.MySqlClient;
using TournamentDataLibrary.Models.StreamQueue;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentDataLibrary
{
    public static class StreamQueueManager
    {
        /// <summary>
        /// Contains all the matches and their data in the currently selected queue
        /// </summary>
        public static List<QueueEntryModel> matchQueue = new List<QueueEntryModel>();
        /// <summary>
        /// Contains all queues and indentifying data for them, but does not contain match data.
        /// </summary>
        public static List<StreamQueueModel> queueList = new List<StreamQueueModel>();
        /// <summary>
        /// The numeric ID of the currently selected stream queue
        /// </summary>
        public static int queueId { get; set; }
        /// <summary>
        /// Checks the designated directory for a list of subdirectories. Each of these subdirectories is treated as its own game.
        /// </summary>
        public static void ImportStreamQueues()
        {
            var databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.IsConnect())
            {
                //check stream queue settings
                queueList = new List<StreamQueueModel>();
                //Load Queue information for the user

                string query = "SELECT * FROM QUEUEADMIN";
                var cmd = new MySqlCommand(query, databaseConnection.connection);
                var reader = cmd.ExecuteReader();

                //Read all the queues that exist
                while(reader.Read())
                {
                    StreamQueueModel addqueue = new StreamQueueModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    queueList.Add(addqueue);
                }
                if (reader.IsClosed == false)
                    reader.Close();
                databaseConnection.Close();
            }
        }

        /// <summary>
        /// Adjusts either the game or the name of a queue.
        /// </summary>
        /// <param name="queueId">The ID of the queue in question</param>
        /// <param name="updateText">The updated game or name</param>
        /// <param name="updateType">Must be either GAME or NAME and will adjust that field</param>
        public static void AdjustQueueSettings(int queueId, string updateText, string updateType)
        {
            var databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@text", updateText));
                playerparams.Add(new MySqlParameter("@id", queueId));

                switch (updateType)
                {
                    case "GAME":
                        databaseConnection.Insert("UPDATE QUEUEADMIN SET QUEUEGAME=@text WHERE QUEUEID=@id", playerparams);
                        break;
                    case "NAME":
                        databaseConnection.Insert("UPDATE QUEUEADMIN SET QUEUENAME=@text WHERE QUEUEID=@id", playerparams);
                        break;
                }
                databaseConnection.Close();
            }

        }
    }
}
