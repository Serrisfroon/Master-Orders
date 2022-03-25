using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;
using TournamentDataLibrary.Models.StreamQueue;

namespace SqlDatabaseLibrary
{
    public static class StreamQueue
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
        public static void AdjustQueueSettings(int queueId, string updateText, string updateType)
        {
            var databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
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

        public static void add_match(QueueEntryModel new_match, bool new_entry)
        {
            //Adds a new player's information to the database set within the settings
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@queueid", global_values.queue_id));
                playerparams.Add(new MySqlParameter("@number", new_match.number));
                playerparams.Add(new MySqlParameter("@status", new_match.status));
                playerparams.Add(new MySqlParameter("@round", new_match.round));
                playerparams.Add(new MySqlParameter("@player1", new_match.player[0]));
                playerparams.Add(new MySqlParameter("@player2", new_match.player[1]));
                playerparams.Add(new MySqlParameter("@player3", new_match.player[2]));
                playerparams.Add(new MySqlParameter("@player4", new_match.player[3]));

                if (new_entry == true)
                {
                    dbCon.Insert("INSERT INTO QUEUE (QUEUEID, MATCHID, STATUS, ROUND, PLAYER1, PLAYER2, PLAYER3, PLAYER4) " +
                               "VALUES(@queueid, @number, @status, @round, @player1, @player2, @player3, @player4)", playerparams);
                }
                else
                {
                    dbCon.Insert("UPDATE QUEUE SET STATUS=@status," +
                        "ROUND=@round,PLAYER1=@player1,PLAYER2=@player2,PLAYER3=@player3," +
                        "PLAYER4=@player4 WHERE MATCHID=@number AND QUEUEID=@queueid", playerparams);
                }
                dbCon.Close();
            }

        }

        public static void remove_match(int old_match)
        {
            //Adds a new player's information to the database set within the settings
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();
                MySqlParameter add_player_param = new MySqlParameter("", "");

                playerparams.Add(new MySqlParameter("@number", old_match));
                playerparams.Add(new MySqlParameter("@queueid", global_values.queue_id));

                dbCon.Insert("DELETE FROM QUEUE WHERE MATCHID=@number AND QUEUEID=@queueid", playerparams);
                dbCon.Close();
            }

        }

        public static List<QueueEntryModel> load_queue(int queue_id, bool is_monitor)
        {
            var dbCon = DBConnection.Instance();

            List<QueueEntryModel> loaded_queue = new List<QueueEntryModel>();

            //Select a connection
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@queueid", queue_id));

                string query = "SELECT MATCHID, STATUS, ROUND, PLAYER1, PLAYER2, PLAYER3, PLAYER4" +
                                " FROM QUEUE WHERE QUEUEID = @queueid";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                for (int i = 0; reader.Read(); i++)
                {
                    QueueEntryModel new_match = new QueueEntryModel();
                    new_match.number = reader.GetInt32(0);
                    new_match.status = reader.GetInt32(1);
                    new_match.round = reader.GetString(2);
                    new_match.player[0] = reader.GetString(3);
                    new_match.player[1] = reader.GetString(4);
                    new_match.player[2] = reader.GetString(5);
                    new_match.player[3] = reader.GetString(6);
                    loaded_queue.Add(new_match);
                }
                reader.Close();
                dbCon.Close();
            }
            return loaded_queue;
        }

        public static bool regame_queue(string queue_name, string game_name, int queueid)
        {
            if (MessageBox.Show("This queue will need to be cleared before changing the selected game for it. Okay to clear the ''" + queue_name + "'' queue?", "Clear the Queue", MessageBoxButtons.OKCancel)
                    == DialogResult.OK)
            {
                var dbCon = DBConnection.Instance();

                if (dbCon.IsConnect())
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(dbCon.Connection);
                    // Sanitize the table name
                    DbCommandBuilder commandBuilder = factory.CreateCommandBuilder();
                    string tablename = commandBuilder.UnquoteIdentifier(commandBuilder.QuoteIdentifier("QUEUEADMIN"));

                    List<MySqlParameter> playerparams = new List<MySqlParameter>();
                    playerparams.Add(new MySqlParameter("@queue", queue_name));
                    playerparams.Add(new MySqlParameter("@game", game_name));
                    playerparams.Add(new MySqlParameter("@queueid", queueid));

                    dbCon.Insert("UPDATE " + tablename + " SET QUEUEGAME = @game WHERE QUEUENAME = @queue", playerparams);

                    tablename = commandBuilder.UnquoteIdentifier(commandBuilder.QuoteIdentifier("QUEUE"));

                    dbCon.Insert("DELETE FROM " + tablename + " WHERE QUEUEID = @queueid", playerparams);

                    dbCon.Close();

                    StreamQueue.queueList[queueid].name = game_name;
                    return true;
                }
            }
            return false;
        }
    }
}
