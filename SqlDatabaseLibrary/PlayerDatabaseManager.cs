using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml.Linq;
using TournamentDataLibrary.Models.PlayerData;


namespace SqlDatabaseLibrary
{
    /// <summary>
    /// Static class containing methods to interact with player records in the SQL database
    /// </summary>
    static public class PlayerDatabaseManager
    {
        /// <summary>
        /// Add a new player to the Sql database, or update an existing player already in the database.
        /// </summary>
        /// <param name="newPlayerRecord">The record to be added or updated</param>
        /// <param name="createNewRecord">If true, a new record will be made. If false, the existing record will be updated.</param>
        public static void AddPlayer(PlayerRecordModel newPlayerRecord, bool createNewRecord)
        {
            //Adds a new player's information to the database set within the settings
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();
            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", newPlayerRecord.id));
                playerParameters.Add(new MySqlParameter("@date", DateTime.Today.ToString("yyyy-MM-dd")));
                playerParameters.Add(new MySqlParameter("@owningUserId", newPlayerRecord.owningUserId));
                playerParameters.Add(new MySqlParameter("@duplicateRecord", newPlayerRecord.duplicateRecord));
                playerParameters.Add(new MySqlParameter("@game", newPlayerRecord.game));
                playerParameters.Add(new MySqlParameter("@tag", newPlayerRecord.tag));
                playerParameters.Add(new MySqlParameter("@elo", newPlayerRecord.elo));
                playerParameters.Add(new MySqlParameter("@characterName", newPlayerRecord.characterName));
                playerParameters.Add(new MySqlParameter("@colorNumber", newPlayerRecord.colorNumber));
                playerParameters.Add(new MySqlParameter("@usingWirelessController", newPlayerRecord.usingWirelessController));
                playerParameters.Add(new MySqlParameter("@twitter", newPlayerRecord.twitter));
                playerParameters.Add(new MySqlParameter("@region", newPlayerRecord.region));
                playerParameters.Add(new MySqlParameter("@sponsor", newPlayerRecord.sponsor));
                playerParameters.Add(new MySqlParameter("@fullSponsor", newPlayerRecord.fullSponsor));
                playerParameters.Add(new MySqlParameter("@fullName", newPlayerRecord.fullName));
                playerParameters.Add(new MySqlParameter("@pronouns", newPlayerRecord.pronouns));
                playerParameters.Add(new MySqlParameter("@misc", newPlayerRecord.misc));


                if (createNewRecord == true)
                {
                    databaseConnection.Insert("INSERT INTO PLAYERS (ID, ACTIVE, CHANGEDATE, OWNERID, LOCALCOPY, GAME, TAG, ELO, CHARACTER1, COLOR1, WIRELESS, " +
                        "TWITTER, REGION, SPONSORPREFIX, SPONSOR, FULLNAME, PRONOUNS MISC)" +
                        "VALUES(@id, 1, @date, @owningUserId, @duplicateRecord, @game, @tag, @elo, @characterName, @colorNumber, @usingWirelessController, " +
                        "@twitter, @region, @sponsor, @fullSponsor, @fullName, @pronouns, @misc)", playerParameters);
                }
                else
                {
                    databaseConnection.Insert("UPDATE PLAYERS SET CHANGEDATE=@date, OWNERID=@owningUserId, LOCALCOPY=@duplicateRecord, TAG=@tag, ELO=@elo, FULLNAME=@fullName," +
                        "TWITTER=@twitter, REGION=@region, SPONSOR=@fullSponsor, SPONSORPREFIX=@sponsor, MISC=@misc, WIRELESS=@usingWirelessController, " +
                        "CHARACTER1=@characterName, COLOR1=@colorNumber, PRONOUNS=@pronouns " +
                        "WHERE ID=@id", playerParameters);
                }
                databaseConnection.Close();
            }
        }

        /// <summary>
        /// Deactivates a player record in the Sql database.
        /// </summary>
        /// <param name="removePlayerRecord">The record to be removed. Uses its Id to identify the record on the database</param>
        public static void RemovePlayer(PlayerRecordModel removePlayerRecord)
        {
            //Marks a player as inactive based on their ID
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();
            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", removePlayerRecord.id));

                databaseConnection.Insert("UPDATE PLAYERS SET ACTIVE=0 WHERE ID=@id", playerParameters);

                databaseConnection.Close();
            }
        }

        /// <summary>
        /// Returns the next open Id to ensure a newly created player record will not use an existing Id.
        /// Counts the number of existing records in the database associated with the current user and uses that as the new ID.
        /// </summary>
        /// <returns>The new record's Id</returns>
        public static string GetNewPlayerId()
        {
            SqlDatabaseConnection dbCon = SqlDatabaseConnection.Instance();

            int playerId = -1;

            //Select a connection
            if (dbCon.TryDatabaseConnection())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", UserSession.userId));

                string query = "SELECT count(ID) FROM PLAYERS WHERE OWNERID=@id";
                MySqlCommand sqlCommand = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerParameters)
                    sqlCommand.Parameters.Add(param);

                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();
                sqlReader.Read();
                int playerRecords = sqlReader.GetInt32(0);
                sqlReader.Close();
                playerId = (UserSession.userId * 1000000) + playerRecords;
                dbCon.Close();
            }
            return playerId.ToString();
        }

        /// <summary>
        /// Checks if a player already exists with the same Tag and Name for the given Game
        /// </summary>
        /// <param name="playerTag"></param>
        /// <param name="playerName"></param>
        /// <param name="playerGame"></param>
        /// <returns>Returns true if the player already exists.</returns>
        public static bool PlayerExists(string playerTag, string playerName, string playerGame)
        {
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();

            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@game", playerGame));
                playerParameters.Add(new MySqlParameter("@tag", playerTag));
                playerParameters.Add(new MySqlParameter("@name", playerName));


                //Find all active Non-copies
                string query = "SELECT count(ID) " +
                                "FROM PLAYERS WHERE GAME=@game AND ACTIVE=1 AND " +
                                "TAG=@tag AND FULLNAME=@name";
                MySqlCommand sqlCommand = new MySqlCommand(query, databaseConnection.connection);
                foreach (MySqlParameter param in playerParameters)
                    sqlCommand.Parameters.Add(param);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();
                sqlReader.Read();
                int result = sqlReader.GetInt32(0);

                sqlReader.Close();
                databaseConnection.Close();

                if (result == 0)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Loads all player records in the Sql database fpr the given Game.
        /// </summary>
        /// <param name="gameName">The game to load players for</param>
        /// <param name="rosterId">A list to write player Ids to</param>
        /// <returns>A PlayerPoolModel containing all records</returns>
        public static PlayerPoolModel LoadAllPlayerRecords(string gameName)
        {
            SqlDatabaseConnection databaseConnection = SqlDatabaseConnection.Instance();
            List<PlayerRecordModel> loadedPlayers = new List<PlayerRecordModel>();
            Dictionary<string, PlayerRecordModel> loadedPlayerTagRecords = new Dictionary<string, PlayerRecordModel>();

            if (databaseConnection.TryDatabaseConnection())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@game", gameName));
                playerParameters.Add(new MySqlParameter("@ownerid", UserSession.userId));


                //Find all active Non-copies
                string query = "SELECT ID, OWNERID, GAME, LOCALCOPY, TAG, ELO, FULLNAME, " +
                                "TWITTER, REGION, SPONSOR, SPONSORPREFIX, MISC, WIRELESS, " +
                                "CHARACTER1, COLOR1, PRONOUNS" +
                                " FROM PLAYERS WHERE GAME=@game AND ACTIVE=1 AND" + //Load only active players for this game
                                "(OWNERID=@ownerid OR LOCALCOPY=1 OR " +    //Load your own players and any local copies you've made
                                "(LOCALCOPY=0 AND OWNERID<>@ownerid AND ID NOT IN (SELECT ID FROM PLAYERS WHERE LOCALCOPY=1)" + //Also load any players not owned by you that don't have a local copy
                                " AND ID NOT IN (SELECT A.ID FROM PLAYERS A, PLAYERS B WHERE A.TAG = B.TAG AND" +
                                " A.FULLNAME = B.FULLNAME AND A.GAME=@game AND B.GAME=@game))) " + //Dont pull any players with the same name and tag as ones that you have
                                "ORDER BY TAG";
                MySqlCommand sqlCommand = new MySqlCommand(query, databaseConnection.connection);
                foreach (MySqlParameter param in playerParameters)
                    sqlCommand.Parameters.Add(param);
                MySqlDataReader sqlReader = sqlCommand.ExecuteReader();
                for (int i = 0; sqlReader.Read(); i++)
                {
                    PlayerRecordModel newPlayer = new PlayerRecordModel();

                    newPlayer.id = sqlReader.GetString(0);
                    newPlayer.owningUserId = sqlReader.GetString(1);
                    newPlayer.game = sqlReader.GetString(2);
                    newPlayer.duplicateRecord = sqlReader.GetBoolean(3);
                    newPlayer.tag = sqlReader.GetString(4);
                    newPlayer.elo = Int32.Parse(sqlReader.GetString(5));
                    newPlayer.fullName = sqlReader.GetString(6);
                    newPlayer.twitter = sqlReader.GetString(7);
                    newPlayer.region = sqlReader.GetString(8);
                    newPlayer.fullSponsor = sqlReader.GetString(9);
                    newPlayer.sponsor = sqlReader.GetString(10);
                    if (!sqlReader.IsDBNull(11))
                        newPlayer.misc = sqlReader.GetString(11);
                    newPlayer.usingWirelessController = sqlReader.GetBoolean(12);
                    newPlayer.characterName = sqlReader.GetString(13);
                    newPlayer.colorNumber = sqlReader.GetInt32(14);
                    newPlayer.pronouns = sqlReader.GetString(15);

                    //Check if this is a duplicate tag
                    if(loadedPlayerTagRecords.ContainsKey(newPlayer.tag))
                    {
                        //Add the player's name to the unqiue tag
                        newPlayer.uniqueTag = newPlayer.tag + "(" + newPlayer.fullName + ")";
                        PlayerRecordModel replacePlayer = new PlayerRecordModel();
                        replacePlayer = loadedPlayerTagRecords[newPlayer.tag];
                        loadedPlayerTagRecords.Remove(newPlayer.tag);

                        replacePlayer.uniqueTag = replacePlayer.tag + "(" + replacePlayer.fullName + ")";
                        loadedPlayerTagRecords.Add(replacePlayer.uniqueTag, replacePlayer);

                    }
                    else
                    {
                        //The player's unique tag is the tag
                        newPlayer.uniqueTag = newPlayer.tag;
                    }

                    loadedPlayerTagRecords.Add(newPlayer.uniqueTag, newPlayer);
                    loadedPlayers.Add(newPlayer);
                }
                sqlReader.Close();
                databaseConnection.Close();
            }

            PlayerPoolModel outputPlayerPool = new PlayerPoolModel();
            outputPlayerPool.playerTagsRecords = loadedPlayerTagRecords;
            outputPlayerPool.playerRecords = loadedPlayers;

            return outputPlayerPool;
        }

    }
}
