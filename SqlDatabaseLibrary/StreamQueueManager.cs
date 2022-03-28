using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TournamentDataLibrary.Models.StreamQueue;

namespace SqlDatabaseLibrary
{
    public static class StreamQueueManager
    {
        /// <summary>
        /// Contains all queues and indentifying data for them, but does not contain match data.
        /// </summary>
        public static List<StreamQueueModel> queueList = new List<StreamQueueModel>();
        /// <summary>
        /// The numeric ID of the currently selected stream queue
        /// </summary>
        public static int queueId { get; set; }
        public enum queueAdjustmentTypes
        {
            adjustName,
            adjustGame
        };
        /// <summary>
        /// Checks the designated directory for a list of subdirectories. Each of these subdirectories is treated as its own game.
        /// </summary>
        public static void ImportStreamQueues()
        {
            var databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
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
        public static void AdjustQueueSettings(int queueId, string updateText, queueAdjustmentTypes updateType)
        {
            var databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@text", updateText));
                playerparams.Add(new MySqlParameter("@id", queueId));

                switch (updateType)
                {
                    case queueAdjustmentTypes.adjustGame:
                        databaseConnection.Insert("UPDATE QUEUEADMIN SET QUEUEGAME=@text WHERE QUEUEID=@id", playerparams);
                        break;
                    case queueAdjustmentTypes.adjustName:
                        databaseConnection.Insert("UPDATE QUEUEADMIN SET QUEUENAME=@text WHERE QUEUEID=@id", playerparams);
                        databaseConnection.Insert("DELETE FROM QUEUE WHERE QUEUEID = @id", playerparams);
                        break;
                }
                databaseConnection.Close();
            }

        }

        /// <summary>
        /// Adds a new entry into the specified stream queue
        /// </summary>
        /// <param name="queueEntry">The Queue Entry to add</param>
        /// <param name="queueId">The ID of the queue to add to</param>
        /// <param name="isNewEntry">Whether or not the queue entry is new(if it's not new, it'll update the existing entry instead)</param>
        public static void AddEntryToQueue(QueueEntryModel queueEntry, int queueId, bool isNewEntry)
        {
            //Transfer player Ids to a seperate struct 
            List<string> playerIds = new List<string>();
            for(int i = 0; i < 12; i++)
            {
                if (queueEntry.playerIds.Count > i)
                {
                    playerIds.Add(queueEntry.playerIds[i]);
                }
                else
                {
                    playerIds.Add("");
                }
            }

            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@queueid", queueId));
                playerparams.Add(new MySqlParameter("@number", queueEntry.positionInQueue));
                playerparams.Add(new MySqlParameter("@status", (int)queueEntry.matchStatus));
                playerparams.Add(new MySqlParameter("@round", queueEntry.roundInBracket));
                playerparams.Add(new MySqlParameter("@player1", playerIds[0]));
                playerparams.Add(new MySqlParameter("@player2", playerIds[1]));
                playerparams.Add(new MySqlParameter("@player3", playerIds[2]));
                playerparams.Add(new MySqlParameter("@player4", playerIds[3]));
                playerparams.Add(new MySqlParameter("@player5", playerIds[4]));
                playerparams.Add(new MySqlParameter("@player6", playerIds[5]));
                playerparams.Add(new MySqlParameter("@player7", playerIds[6]));
                playerparams.Add(new MySqlParameter("@player8", playerIds[7]));
                playerparams.Add(new MySqlParameter("@player9", playerIds[8]));
                playerparams.Add(new MySqlParameter("@player10", playerIds[9]));
                playerparams.Add(new MySqlParameter("@player11", playerIds[10]));
                playerparams.Add(new MySqlParameter("@player12", playerIds[11]));

                if (isNewEntry == true)
                {
                    databaseConnection.Insert("INSERT INTO QUEUE (QUEUEID, MATCHID, STATUS, ROUND, PLAYER1, PLAYER2, PLAYER3, PLAYER4, PLAYER5, PLAYER6, PLAYER7, PLAYER8, PLAYER9, PLAYER10, PLAYER11, PLAYER12) " +
                               "VALUES(@queueid, @number, @status, @round, @player1, @player2, @player3, @player4, @player5, @player6, @player7, @player8, @player9, @player10, @player11, @player12)", playerparams);
                }
                else
                {
                    databaseConnection.Insert("UPDATE QUEUE SET STATUS=@status," +
                        "ROUND=@round,PLAYER1=@player1,PLAYER2=@player2,PLAYER3=@player3,PLAYER4=@player4,PLAYER5=@player5,PLAYER6=@player6" +
                        ",PLAYER7=@player7,PLAYER8=@player8,PLAYER9=@player9,PLAYER10=@player10,PLAYER11=@player11,PLAYER12=@player12" +
                        " WHERE MATCHID=@number AND QUEUEID=@queueid", playerparams);
                }

                foreach( string playerId in queueEntry.playerIds)
                {
                    playerparams = new List<MySqlParameter>();

                }
                databaseConnection.Close();
            }
        }
        /// <summary>
        /// Removes a match from a stream queue.
        /// </summary>
        /// <param name="matchId">ID of the match to remove</param>
        /// <param name="queueId">ID of the queue to remove the match from</param>
        public static void RemoveEntryFromQueue(int matchId, int queueId)
        {
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();
                MySqlParameter add_player_param = new MySqlParameter("", "");

                playerparams.Add(new MySqlParameter("@number", matchId));
                playerparams.Add(new MySqlParameter("@queueid", queueId));

                databaseConnection.Insert("DELETE FROM QUEUE WHERE MATCHID=@number AND QUEUEID=@queueid", playerparams);
                databaseConnection.Close();
            }
        }
        /// <summary>
        /// Loads all entries associated to a stream queue
        /// </summary>
        /// <param name="queueId">The ID of the queue to load</param>
        /// <returns>A list of all entries associated to the given queue, ordered by positionInQueue</returns>
        public static List<QueueEntryModel> LoadStreamQueue(int queueId)
        {
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();

            List<QueueEntryModel> loadedStreamQueue = new List<QueueEntryModel>();

            //Select a connection
            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> queueParameters = new List<MySqlParameter>();

                queueParameters.Add(new MySqlParameter("@queueid", queueId));

                string query = "SELECT MATCHID, STATUS, ROUND, PLAYER1, PLAYER2, PLAYER3, PLAYER4" +
                                " FROM QUEUE WHERE QUEUEID = @queueid";
                MySqlCommand sqlCommand = new MySqlCommand(query, databaseConnection.connection);
                foreach (MySqlParameter param in queueParameters)
                    sqlCommand.Parameters.Add(param);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();
                for (int i = 0; sqlReader.Read(); i++)
                {
                    QueueEntryModel newQueueEntry = new QueueEntryModel();
                    newQueueEntry.positionInQueue = sqlReader.GetInt32(0);
                    newQueueEntry.matchStatus = (QueueEntryModel.status)sqlReader.GetInt32(1);
                    newQueueEntry.roundInBracket = sqlReader.GetString(2);
                    for(int ii = 0; ii < 12; ii++)
                    {
                        string readPlayerId = sqlReader.GetString(3 + ii);
                        if(readPlayerId != "")
                        {
                            newQueueEntry.playerIds.Add(readPlayerId);
                        }
                    }
                    loadedStreamQueue.Add(newQueueEntry);
                }
                sqlReader.Close();
                databaseConnection.Close();
            }
            //sorts the queue to be ordered by their position in queue
            loadedStreamQueue.Sort((x, y) => x.positionInQueue.CompareTo(y.positionInQueue));
            return loadedStreamQueue;
        }

    }
}
