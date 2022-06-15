using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Xml.Linq;
using SqlDatabaseLibrary.Models;


namespace SqlDatabaseLibrary
{
    static public class PlayerDatabase
    {
        /// <summary>
        /// The master list of players used by different tools.
        /// </summary>
        public static List<PlayerRecordModel> playerRecords { get; set; }

        public enum SearchProperty
        {
            id,
            tag,
            uniqueTag
        }

        /// <summary>
        /// Find a player record based off a string value
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="searchProperty"></param>
        /// <returns></returns>
        public static PlayerRecordModel FindRecordFromString(string inputString, SearchProperty searchProperty)
        {
            if(playerRecords == null)
            {
                return null;
            }
            switch(searchProperty)
            {
                case SearchProperty.id:
                    return playerRecords.Find(x => x.id == inputString);
                case SearchProperty.tag:
                    return playerRecords.Find(x => x.tag == inputString);
                case SearchProperty.uniqueTag:
                    return playerRecords.Find(x => x.uniqueTag == inputString);
                default:
                    return null;
            }
        }

        /// <summary>
        /// Returns if a record exists based off a string value
        /// </summary>
        /// <param name="inputString"></param>
        /// <param name="searchProperty"></param>
        /// <returns></returns>
        public static bool RecordExistsFromString(string inputString, SearchProperty searchProperty)
        {
            PlayerRecordModel foundRecord = FindRecordFromString(inputString, searchProperty);
            return foundRecord != null;
        }

        public static string GetUniqueTagFromId(string inputId)
        {
            PlayerRecordModel foundPlayerRecord = FindRecordFromString(inputId, SearchProperty.id);
                //TryGetValue(inputId, out PlayerRecordModel foundPlayerRecord);
            //Set the tag of the player. Other fields will populate off of this
            if (foundPlayerRecord == null)
            {
                return inputId;
            }
            return foundPlayerRecord.uniqueTag;
        }

        /// <summary>
        /// Add a new player to the Sql database, or update an existing player already in the database.
        /// </summary>
        /// <param name="newPlayerRecord">The record to be added or updated</param>
        /// <param name="createNewRecord">If true, a new record will be made. If false, the existing record will be updated.</param>
        public static void AddPlayer(PlayerRecordModel newPlayerRecord, bool createNewRecord)
        {
            //Adds a new player's information to the database set within the settings
            var dbCon = SqlDatabaseConnection.Instance();
            if (dbCon.IsConnect())
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
                    dbCon.Insert("INSERT INTO PLAYERS (ID, ACTIVE, CHANGEDATE, OWNERID, LOCALCOPY, GAME, TAG, ELO, CHARACTER1, COLOR1, WIRELESS, " +
                        "TWITTER, REGION, SPONSORPREFIX, SPONSOR, FULLNAME, PRONOUNS MISC)" +
                        "VALUES(@id, 1, @date, @owningUserId, @duplicateRecord, @game, @tag, @elo, @characterName, @colorNumber, @usingWirelessController, " +
                        "@twitter, @region, @sponsor, @fullSponsor, @fullName, @pronouns, @misc)", playerParameters);
                }
                else
                {
                    dbCon.Insert("UPDATE PLAYERS SET CHANGEDATE=@date, OWNERID=@owningUserId, LOCALCOPY=@duplicateRecord, TAG=@tag, ELO=@elo, FULLNAME=@fullName," +
                        "TWITTER=@twitter, REGION=@region, SPONSOR=@fullSponsor, SPONSORPREFIX=@sponsor, MISC=@misc, WIRELESS=@usingWirelessController, " +
                        "CHARACTER1=@characterName, COLOR1=@colorNumber, PRONOUNS=@pronouns " +
                        "WHERE ID=@id", playerParameters);
                }
                dbCon.Close();
            }
        }

        /// <summary>
        /// Deactivates a player record in the Sql database.
        /// </summary>
        /// <param name="removePlayerRecord">The record to be removed. Uses its Id to identify the record on the database</param>
        public static void RemovePlayer(PlayerRecordModel removePlayerRecord)
        {
            //Marks a player as inactive based on their ID
            var dbCon = SqlDatabaseConnection.Instance();
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", removePlayerRecord.id));

                dbCon.Insert("UPDATE PLAYERS SET ACTIVE=0 WHERE ID=@id", playerParameters);

                dbCon.Close();
            }
        }

        /// <summary>
        /// Returns the next open Id to ensure a newly created player record will not use an existing Id.
        /// </summary>
        /// <returns>The new record's Id</returns>
        public static string GetNewPlayerId()
        {
            var dbCon = SqlDatabaseConnection.Instance();

            int playerId = -1;

            //Select a connection
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", UserSession.userId));

                string query = "SELECT count(ID) FROM PLAYERS WHERE OWNERID=@id";
                var cmd = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerParameters)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                int playerRecords = reader.GetInt32(0);
                reader.Close();
                playerId = (UserSession.userId * 1000000) + playerRecords;
                dbCon.Close();
            }
            return playerId.ToString();
        }

        /// <summary>
        /// Returns the name of a user based on the Id provided.
        /// </summary>
        /// <param name="ownerId">The Id to check the username of</param>
        /// <returns>The Id's username</returns>
        public static string GetOwnerName(string ownerId)
        {
            var dbCon = SqlDatabaseConnection.Instance();

            string ownerName = "";

            //Select a connection
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@id", ownerId));

                string query = "SELECT USERNAME FROM Mastercore.USERS WHERE ID=@id";
                var cmd = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerParameters)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                ownerName = reader.GetString(0);
                reader.Close();
                dbCon.Close();
            }
            return ownerName;
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
            var dbCon = SqlDatabaseConnection.Instance();

            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerParameters = new List<MySqlParameter>();

                playerParameters.Add(new MySqlParameter("@game", playerGame));
                playerParameters.Add(new MySqlParameter("@tag", playerTag));
                playerParameters.Add(new MySqlParameter("@name", playerName));


                //Find all active Non-copies
                string query = "SELECT count(ID) " +
                                "FROM PLAYERS WHERE GAME=@game AND ACTIVE=1 AND " +
                                "TAG=@tag AND FULLNAME=@name";
                var cmd = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerParameters)
                    cmd.Parameters.Add(param);
                var reader = cmd.ExecuteReader();
                reader.Read();
                int result = reader.GetInt32(0);

                reader.Close();
                dbCon.Close();

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
        /// <returns>A list of player records</returns>
        public static void LoadPlayers(string gameName)
        {
            var dbCon = SqlDatabaseConnection.Instance();
            List<PlayerRecordModel> loadedPlayers = new List<PlayerRecordModel>();

            if (dbCon.IsConnect())
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
                var cmd = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerParameters)
                    cmd.Parameters.Add(param);
                var reader = cmd.ExecuteReader();
                for (int i = 0; reader.Read(); i++)
                {
                    PlayerRecordModel newPlayer = new PlayerRecordModel();

                    newPlayer.id = reader.GetString(0);
                    newPlayer.owningUserId = reader.GetString(1);
                    newPlayer.game = reader.GetString(2);
                    newPlayer.duplicateRecord = reader.GetBoolean(3);
                    newPlayer.tag = reader.GetString(4);
                    newPlayer.elo = Int32.Parse(reader.GetString(5));
                    newPlayer.fullName = reader.GetString(6);
                    newPlayer.twitter = reader.GetString(7);
                    newPlayer.region = reader.GetString(8);
                    newPlayer.fullSponsor = reader.GetString(9);
                    newPlayer.sponsor = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        newPlayer.misc = reader.GetString(11);
                    newPlayer.usingWirelessController = reader.GetBoolean(12);
                    newPlayer.characterName = reader.GetString(13);
                    newPlayer.colorNumber = reader.GetInt32(14);
                    newPlayer.pronouns = reader.GetString(15);

                    //Check if this is a duplicate tag
                    PlayerRecordModel foundRecord = FindRecordFromString(newPlayer.tag, SearchProperty.tag);
                    if (foundRecord != null)
                    {
                        //Add the new player's name to the unqiue tag
                        newPlayer.uniqueTag = newPlayer.tag + "(" + newPlayer.fullName + ")";

                        // Remove the found record from the list, update the unique tag, re-add it
                        loadedPlayers.Remove(foundRecord);
                        foundRecord.uniqueTag = foundRecord.tag + "(" + foundRecord.id + ")";
                        loadedPlayers.Add(foundRecord);
                    }
                    else
                    {
                        //The player's unique tag is the tag
                        newPlayer.uniqueTag = newPlayer.tag;
                    }

                    loadedPlayers.Add(newPlayer);
                }
                reader.Close();
                dbCon.Close();
            }

            playerRecords = loadedPlayers;
        }

    }
}
