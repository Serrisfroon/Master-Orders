using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Stream_Info_Handler.AppSettings;
using SqlDatabaseLibrary.Models;

namespace Stream_Info_Handler
{
    public static class database_tools
    {
        static string master_db = "Mastercore";

        private static string _pass_directory;
        public static string pass_directory
        {
            get // this makes you to access value in form2
            {
                return _pass_directory;
            }
            set // this makes you to change value in form2
            {
                _pass_directory = value;
            }
        }

        public static void add_player(PlayerRecordModel new_player, bool new_entry)
        {
            //Adds a new player's information to the database set within the settings
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@id", new_player.id));
                playerparams.Add(new MySqlParameter("@date", DateTime.Today.ToString("yyyy-MM-dd")));
                playerparams.Add(new MySqlParameter("@ownerid", new_player.ownerid));
                playerparams.Add(new MySqlParameter("@game", new_player.game));
                playerparams.Add(new MySqlParameter("@copy", new_player.iscopy));
                playerparams.Add(new MySqlParameter("@tag", new_player.tag));
                playerparams.Add(new MySqlParameter("@elo", new_player.elo));
                playerparams.Add(new MySqlParameter("@fullname", new_player.fullname));
                playerparams.Add(new MySqlParameter("@twitter", new_player.twitter));
                playerparams.Add(new MySqlParameter("@region", new_player.region));
                playerparams.Add(new MySqlParameter("@sponsor", new_player.fullsponsor));
                playerparams.Add(new MySqlParameter("@sponsorprefix", new_player.sponsor));
                playerparams.Add(new MySqlParameter("@misc", new_player.misc));
                playerparams.Add(new MySqlParameter("@wireless", new_player.wireless_controller));
                playerparams.Add(new MySqlParameter("@character1", new_player.character[0]));
                playerparams.Add(new MySqlParameter("@color1", new_player.color[0]));





                if (new_entry == true)
                {
                    dbCon.Insert("INSERT INTO PLAYERS (ID, ACTIVE, CHANGEDATE, OWNERID, GAME, LOCALCOPY, TAG, ELO, FULLNAME, " +
                        "TWITTER, REGION, SPONSOR, SPONSORPREFIX, MISC, WIRELESS, " +
                        "CHARACTER1, COLOR1) " +
                        "VALUES(@id, 1, @date, @ownerid, @game, @copy, @tag, @elo, @fullname, @twitter, @region, @sponsor, @sponsorprefix, @misc, @wireless," +
                        "@character1, @color1)", playerparams);
                }
                else
                {
                    dbCon.Insert("UPDATE PLAYERS SET CHANGEDATE=@date, OWNERID=@ownerid, LOCALCOPY=@copy, TAG=@tag, ELO=@elo, FULLNAME=@fullname," +
                        "TWITTER=@twitter, REGION=@region, SPONSOR=@sponsor, SPONSORPREFIX=@sponsorprefix, MISC=@misc, WIRELESS=@wireless, " +
                        "CHARACTER1=@character1, COLOR1=@color1 " +
                        "WHERE ID=@id", playerparams);
                }
                dbCon.Close();
            }
        }

        public static void remove_player(PlayerRecordModel remove_player)
        {
            //Marks a player as inactive based on their ID
            var dbCon = DBConnection.Instance();
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@id", remove_player.id));

                dbCon.Insert("UPDATE PLAYERS SET ACTIVE=0 WHERE ID=@id", playerparams);

                dbCon.Close();
            }
        }

        public static string get_new_playerid()
        {
            var dbCon = DBConnection.Instance();

            int player_id = -1;

            //Select a connection
            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@id", global_values.user_id));

                string query = "SELECT count(ID) FROM PLAYERS WHERE OWNERID=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                int player_count = reader.GetInt32(0);
                reader.Close();
                player_id = (global_values.user_id * 1000000) + player_count;
                dbCon.Close();
            }
            return player_id.ToString();
        }

        public static string get_owner_name(string ownerid)
        {
            var dbCon = DBConnection.Instance();

            string ownername = "";

            //Select a connection
            if (dbCon.IsConnect())
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory(dbCon.Connection);
                // Sanitize the table name
                DbCommandBuilder commandBuilder = factory.CreateCommandBuilder();
                string tablename = commandBuilder.UnquoteIdentifier(commandBuilder.QuoteIdentifier(master_db + ".USERS"));

                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@id", ownerid));

                string query = "SELECT USERNAME FROM " + tablename + " WHERE ID=@id";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                ownername = reader.GetString(0);
                reader.Close();
                dbCon.Close();
            }
            return ownername;
        }


        public static bool player_exists(string playertag, string playername, string playergame)
        {
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@game", playergame));
                playerparams.Add(new MySqlParameter("@tag", playertag));
                playerparams.Add(new MySqlParameter("@name", playername));


                //Find all active Non-copies
                string query = "SELECT count(ID) " +
                                "FROM PLAYERS WHERE GAME=@game AND ACTIVE=1 AND " +
                                "TAG=@tag AND FULLNAME=@name";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
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

        public static List<PlayerRecordModel> load_players(string gamename, ref List<string> rosterid)
        {
            var dbCon = DBConnection.Instance();
            List<PlayerRecordModel> loaded_players = new List<PlayerRecordModel>();

            if (dbCon.IsConnect())
            {
                List<MySqlParameter> playerparams = new List<MySqlParameter>();

                playerparams.Add(new MySqlParameter("@game", gamename));
                playerparams.Add(new MySqlParameter("@ownerid", global_values.user_id));


                //Find all active Non-copies
                string query = "SELECT ID, OWNERID, GAME, LOCALCOPY, TAG, ELO, FULLNAME, " +
                                "TWITTER, REGION, SPONSOR, SPONSORPREFIX, MISC, WIRELESS, " +
                                "CHARACTER1, COLOR1" +
                                " FROM PLAYERS WHERE GAME=@game AND ACTIVE=1 AND" + //Load only active players for this game
                                "(OWNERID=@ownerid OR LOCALCOPY=1 OR " +    //Load your own players and any local copies you've made
                                "(LOCALCOPY=0 AND OWNERID<>@ownerid AND ID NOT IN (SELECT ID FROM PLAYERS WHERE LOCALCOPY=1)" + //Also load any players not owned by you that don't have a local copy
                                " AND ID NOT IN (SELECT A.ID FROM PLAYERS A, PLAYERS B WHERE A.TAG = B.TAG AND" +
                                " A.FULLNAME = B.FULLNAME AND A.GAME=@game AND B.GAME=@game))) " + //Dont pull any players with the same name and tag as ones that you have
                                "ORDER BY TAG";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
                    cmd.Parameters.Add(param);
                var reader = cmd.ExecuteReader();
                loaded_players = new List<PlayerRecordModel>();
                List<string> player_names = new List<string>();
                rosterid = new List<string>();
                for (int i = 0; reader.Read(); i++)
                {

                    loaded_players.Add(new PlayerRecordModel());


                    loaded_players[i].id = reader.GetString(0);
                    rosterid.Add(loaded_players[i].id);
                    loaded_players[i].ownerid = reader.GetString(1);
                    loaded_players[i].game = reader.GetString(2);
                    loaded_players[i].iscopy = reader.GetBoolean(3);
                    loaded_players[i].tag = reader.GetString(4);
                    loaded_players[i].elo = Int32.Parse(reader.GetString(5));
                    loaded_players[i].fullname = reader.GetString(6);
                    loaded_players[i].twitter = reader.GetString(7);
                    loaded_players[i].region = reader.GetString(8);
                    loaded_players[i].fullsponsor = reader.GetString(9);
                    loaded_players[i].sponsor = reader.GetString(10);
                    if (!reader.IsDBNull(11))
                        loaded_players[i].misc = reader.GetString(11);
                    loaded_players[i].wireless_controller = reader.GetBoolean(12);
                    loaded_players[i].character[0] = reader.GetString(13);
                    loaded_players[i].color[0] = reader.GetInt32(14);

                    //Check if this is a duplicate tag
                    if (player_names.Contains(loaded_players[i].tag))
                    {
                        //Add the player's name to the unqiue tag
                        loaded_players[i].unique_tag = loaded_players[i].tag + "(" + loaded_players[i].fullname + ")";
                        int copy_tag = player_names.IndexOf(loaded_players[i].tag);
                        loaded_players[copy_tag].unique_tag = loaded_players[copy_tag].tag + "(" + loaded_players[copy_tag].fullname + ")";
                        player_names.Add("");
                    }
                    else
                    {
                        //The player's unique tag is the tag
                        loaded_players[i].unique_tag = loaded_players[i].tag;
                        player_names.Add(loaded_players[i].tag);
                    }
                }
                reader.Close();
                dbCon.Close();
            }

            return loaded_players;
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

        public static void update_directories()
        {
            //Build the raw comma-semicolon seperate string of games and their directories
            string rawdirectories = "";
            for (int i = 0; i < GlobalSettings.availableGames.Length; i++)
            {
                rawdirectories = rawdirectories + ";" + GlobalSettings.availableGames[i] + "," + DirectoryManagement.gameDirectories[GlobalSettings.availableGames[i]];
            }
            rawdirectories = rawdirectories.Remove(0, 1);   //Remove the starting semicolon

            //Save to settings
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("directories").Element("roster-directories").ReplaceWith(new XElement("roster-directories", rawdirectories));
            xml.Save(SettingsFile.settingsFile);
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

        public static bool login(string table, string user, string pass)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = table;
            dbCon.DatabaseUserName = user;
            dbCon.DatabasePassword = pass;
            try
            {
                //Connect to the database. Fails if incorrect credentials
                dbCon.IsConnect();

                DbProviderFactory factory = DbProviderFactories.GetFactory(dbCon.Connection);
                // Sanitize the table name
                DbCommandBuilder commandBuilder = factory.CreateCommandBuilder();
                string tablename = commandBuilder.UnquoteIdentifier(commandBuilder.QuoteIdentifier(master_db + ".USERS"));

                List<MySqlParameter> playerparams = new List<MySqlParameter>();
                playerparams.Add(new MySqlParameter("@user", dbCon.DatabaseUserName));

                //Verify the user is active
                string query = "SELECT ACTIVE, ID FROM " + tablename + " WHERE USERNAME=@user";
                var cmd = new MySqlCommand(query, dbCon.Connection);
                foreach (MySqlParameter param in playerparams)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                bool active_user = reader.GetInt32(0) != 0;
                int userid = reader.GetInt32(1);
                reader.Close();
                dbCon.Close();

                //Return false if the user is inactive
                if (active_user == false)
                {
                    MessageBox.Show(dbCon.DatabaseUserName + " is not currently an active Master Orders account. If you are the account owner, please contact Serris via twitter @serrisfroon");
                    return false;
                }
                global_values.user_id = userid;

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                dbCon.Close();
                return false;
            }
        }

        /// <summary>
        /// A structure that contains all relevant information to a stream queue.
        /// </summary>
        public class streamQueue
        {
            public int id;
            public string name;
            public string game;

            public streamQueue(int toid, string toname, string togame)
            {
                id = toid;
                name = toname;
                game = togame;
            }
            public streamQueue() { }
        }


    }

    public class DBConnection
    {
        private DBConnection()
        {
        }

        private string databaseUsername = string.Empty;
        public string DatabaseUserName
        {
            get { return databaseUsername; }
            set { databaseUsername = value; }
        }

        private string databasePassword = string.Empty;
        public string DatabasePassword
        {
            get { return databasePassword; }
            set { databasePassword = value; }
        }

        private string databaseName = string.Empty;
        public string DatabaseName
        {
            get { return databaseName; }
            set { databaseName = value; }
        }

        public string Password { get; set; }
        private MySqlConnection connection = null;
        public MySqlConnection Connection
        {
            get { return connection; }
        }

        private static DBConnection _instance = null;
        public static DBConnection Instance()
        {
            if (_instance == null) //remove this line if issues arise
                _instance = new DBConnection();
            return _instance;
        }

        public bool IsConnect()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=107.180.3.224; database={0}; UID={1}; password={2}", databaseName, databaseUsername, databasePassword);
                connection = new MySqlConnection(connstring);
                connection.Open();
            }

            return true;
        }

        public void Close()
        {
            connection.Close();
            connection = null;
        }

        private MySqlConnection mconnection = null;
        public MySqlConnection MConnection
        {
            get { return mconnection; }
        }

        public bool IsMonitor()
        {
            if (Connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=107.180.3.224; database={0}; UID={1}; password={2}", databaseName, databaseUsername, databasePassword);
                mconnection = new MySqlConnection(connstring);
                mconnection.Open();
            }

            return true;
        }

        public void Insert(string strSQL, List<MySqlParameter> paramslist)
        {
            if (IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                foreach (MySqlParameter param in paramslist)
                    cmd.Parameters.Add(param);

                int rows = cmd.ExecuteNonQuery();
                //MessageBox.Show(cmd.CommandText + "\n \nAffected Rows: " + rows.ToString());
            }
        }

        public MySqlDataReader Select(string strSQL, List<MySqlParameter> paramslist)
        {
            if (IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                foreach (MySqlParameter param in paramslist)
                    cmd.Parameters.Add(param);

                MySqlDataReader reader = cmd.ExecuteReader();
                return reader;
            }
            else
            {
                return null;
            }

        }
    }
}

#region old database functions
/*
        public static List<string> get_user_tables()
        {
            //returns a list of all tables in the database under the username
            var dbCon = DBConnection.Instance();


            List<string> user_tables = new List<string>();

            if (dbCon.IsConnect())
            {

                List<MySqlParameter> tableparam = new List<MySqlParameter>();

                DbProviderFactory factory = DbProviderFactories.GetFactory(dbCon.Connection);
                // Sanitize the table name
                DbCommandBuilder commandBuilder = factory.CreateCommandBuilder();
                string tablename = commandBuilder.QuoteIdentifier(dbCon.DatabaseUserName);


                var reader = dbCon.Select("SELECT table_name FROM information_schema.tables WHERE table_schema = '" + tablename + "'",
                                            tableparam);

                for (int i = 0; reader.Read(); i++)
                {
                    user_tables.Add(reader.GetString(0));
                }
                reader.Close();
            }

            return user_tables;
        }
        public static string check_table(List<string> table_list, string table_name, string table_type)
        {
            string new_table = table_type+table_name;
            string old_table = "";

            //Checks to see if a table is included within a list of table names.
            //If not, it will prompt the user to select a table name to replace.
            var dbCon = DBConnection.Instance();

            if (dbCon.IsConnect())
            {
                //Check if the list contains the table
                if(!table_list.Contains(new_table))
                {
                    //If not, prompt the user to select a table to replace

                    //Remove any names from the list that are not of the indicated type
                    List<string> clean_list = new List<string>();
                    foreach(string checktable in table_list)
                    {
                        string cleaned_name = checktable.Substring(0, table_type.Length);

                        if (cleaned_name == table_type)
                        {
                            clean_list.Add(checktable.Substring(cleaned_name.Length, checktable.Length-cleaned_name.Length));
                        }
                    }

                    frm_tables selection = new frm_tables(clean_list, table_name);

                    if (selection.ShowDialog() != DialogResult.OK)
                        return null;

                    //Build the complete old table name
                    old_table = table_type + pass_table;


                    //Execute SQL to change the rename the table of the old name with the new name
                    List<MySqlParameter> playerparams = new List<MySqlParameter>();

                    DbProviderFactory factory = DbProviderFactories.GetFactory(dbCon.Connection);
                    // Sanitize the table name
                    DbCommandBuilder commandBuilder = factory.CreateCommandBuilder();
                    string oldTable = commandBuilder.QuoteIdentifier(dbCon.DatabaseUserName + "." + old_table);
                    string newTable = commandBuilder.QuoteIdentifier(new_table);

                    dbCon.Insert("ALTER TABLE " + oldTable + " RENAME TO " + newTable + ";", playerparams);
                }
            }

            return table_name;
        }
        */
#endregion
#region Google Sheets Info
/*
public static void add_to_sheets(PlayerRecordModel new_player)
{
    
    UserCredential credential;

    using (var stream =
        new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
    {
        string credPath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { SheetsService.Scope.Spreadsheets },
            global_values.youtube_username,
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
    }

    // Create Google Sheets API service.
    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = this.GetType().ToString()
    });

    // Define request parameters.
    String spreadsheetId = txt_sheets.Text;

    //Set the range to be only the player information
    string range = "Player Information!A2:O" + (MAX_PLAYERS+1).ToString();

    //Set up the request for the sheet
    SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

    //Receive the player information from the request
    Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
    //Place the information in an array
    IList<IList<Object>> PlayerRecordModelrmation = response.Values;

    //Cycle through the player information until the new player's name is found, or the end of the list is found.
    int player_index = -1;                  //Intialize the index of the new player in the list
    for (int i = 0; i <= MAX_PLAYERS; i++)
    {
        if (PlayerRecordModelrmation[i][0].ToString() == new_player.tag)
        {
            player_index = i;               //Set the index to the current position
            global_values.roster[i].sponsor = new_player.sponsor;
            global_values.roster[i].twitter = new_player.twitter;
            global_values.roster[i].character[0] = new_player.character[0];
            global_values.roster[i].character[1] = new_player.character[1];
            global_values.roster[i].character[2] = new_player.character[2];
            global_values.roster[i].character[3] = new_player.character[3];
            global_values.roster[i].character[4] = new_player.character[4];
            global_values.roster[i].color[0] = new_player.color[0];
            global_values.roster[i].color[1] = new_player.color[1];
            global_values.roster[i].color[2] = new_player.color[2];
            global_values.roster[i].color[3] = new_player.color[3];
            global_values.roster[i].color[4] = new_player.color[4];
            break;
        }
        if (PlayerRecordModelrmation[i][0].ToString() == "")
        {
            player_index = i;               //Set the index to the current position
            global_values.roster[i] = new_player;
            global_values.roster_size += 1;
            break;
        }
    }



    //Add the new player's information to an array.
    var oblist = new List<object>() { global_values.roster[player_index].tag,
                                        global_values.roster[player_index].twitter,
                                        global_values.roster[player_index].region,
                                        global_values.roster[player_index].sponsor,
                                        global_values.roster[player_index].character[0],
                                        global_values.roster[player_index].character[1],
                                        global_values.roster[player_index].character[2],
                                        global_values.roster[player_index].character[3],
                                        global_values.roster[player_index].character[4],
                                        global_values.roster[player_index].color[0],
                                        global_values.roster[player_index].color[1],
                                        global_values.roster[player_index].color[2],
                                        global_values.roster[player_index].color[3],
                                        global_values.roster[player_index].color[4]};

    //Create a data set from the array
    Google.Apis.Sheets.v4.Data.ValueRange data = new Google.Apis.Sheets.v4.Data.ValueRange();
    data.Values = new List<IList<object>> { oblist };
    //Set the range's row to the player index"
    string range2 = "Player Information!A" + (player_index+2).ToString() + ":N" + (player_index + 2).ToString();
    data.MajorDimension = "ROWS";

    //Process the update
    SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(data, spreadsheetId, range2);
    update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
    Google.Apis.Sheets.v4.Data.UpdateValuesResponse result2 = update.Execute();



    MessageBox.Show("The following player information has been added to the database:" +
                        "\n Tag: " + global_values.roster[player_index].tag +
                        "\n Twitter: " + global_values.roster[player_index].twitter +
                        "\n Region: " + global_values.roster[player_index].region +
                        "\n Sponsor: " + global_values.roster[player_index].sponsor +
                        "\n Main: " + global_values.roster[player_index].character[0] +
                        "\n Color: " + global_values.roster[player_index].color[0]);
}




















public static void load_players(bool reverse, int player_count)
{
    
    UserCredential credential;

    using (var stream =
        new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
    {
        string credPath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { SheetsService.Scope.Spreadsheets },
            global_values.youtube_username,
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
    }

    // Create Google Sheets API service.
    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = this.GetType().ToString()
    });

    // Define request parameters.
    String spreadsheetId = "DERPDERPDERPDERPDERP";

    List<string> ranges = new List<string>(new string[] { "Current Round Info!A1:D18", "Upcoming Matches!A1:G56", "Player Information!A2:O" + (global_values.max_players + 1).ToString() });

    SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(spreadsheetId);
    request.Ranges = ranges;

    Google.Apis.Sheets.v4.Data.BatchGetValuesResponse response = request.Execute();
    IList<IList<Object>> current_round_info = response.ValueRanges[0].Values;
    IList<IList<Object>> upcoming_matches = response.ValueRanges[1].Values;
    IList<IList<Object>> PlayerRecordModelrmation = response.ValueRanges[2].Values;

    //Populate the player roster with information from the player information sheet
    for (int i = 0; i <= global_values.max_players; i++)
    {
        if (PlayerRecordModelrmation[i][0].ToString() == "")
        {
            global_values.roster_size = i - 1;
            break;
        }
        global_values.roster[i] = new PlayerRecordModel();
        global_values.roster[i].tag = PlayerRecordModelrmation[i][0].ToString();
        global_values.roster[i].twitter = PlayerRecordModelrmation[i][1].ToString();
        global_values.roster[i].region = PlayerRecordModelrmation[i][2].ToString();
        global_values.roster[i].sponsor = PlayerRecordModelrmation[i][3].ToString();
        for (int ii = 0; ii <= 4; ii++)
        {
            global_values.roster[i].character[ii] = PlayerRecordModelrmation[i][4 + ii].ToString();
        }
        for (int ii = 0; ii <= 4; ii++)
        {
            if (PlayerRecordModelrmation[i][9 + ii] != null && PlayerRecordModelrmation[i][9 + ii] != "")
            {
                string add_color = PlayerRecordModelrmation[i][9 + ii].ToString();
                global_values.roster[i].color[ii] = Int32.Parse(add_color);
            }
            else
            {
                global_values.roster[i].color[ii] = 1;
            }
        }
    }

    int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
    string manual_update = upcoming_matches[1][3].ToString();
    if ((manual_update == "" || manual_update == null))
    {
        if (global_values.first_match == false)
        {
            if (reverse == true)
            {
                round_number -= 1;

            }
            else
            {
                round_number += 1;
            }
        }
    }
    else
    {
        round_number = Int32.Parse(manual_update);
    }

    if (round_number == 0)
    {
        round_number = 1;
    }

    global_values.first_match = false;


    string[] player_name = new string[9];
    PlayerRecordModel[] player = new PlayerRecordModel[9];

    switch (player_count)
    {
        case 2:
            player_name[1] = upcoming_matches[3 + round_number][2].ToString();
            player_name[2] = upcoming_matches[3 + round_number][3].ToString();
            player_name[3] = upcoming_matches[4 + round_number][2].ToString();
            player_name[4] = upcoming_matches[4 + round_number][3].ToString();
            break;
        case 4:
            player_name[1] = upcoming_matches[3 + round_number][2].ToString();
            player_name[2] = upcoming_matches[3 + round_number][4].ToString();
            player_name[3] = upcoming_matches[3 + round_number][3].ToString();
            player_name[4] = upcoming_matches[3 + round_number][5].ToString();

            player_name[5] = upcoming_matches[4 + round_number][2].ToString();
            player_name[6] = upcoming_matches[4 + round_number][4].ToString();
            player_name[7] = upcoming_matches[4 + round_number][3].ToString();
            player_name[8] = upcoming_matches[4 + round_number][5].ToString();
            break;
    }




    for (int player_number = 1; player_number <= player_count * 2; player_number++)
    {
        for (int i = 0; i <= global_values.roster_size; i++)
        {
            if (player_name[player_number] == "")
            {
                player[player_number] = new PlayerRecordModel();
                player[player_number].tag = "";
                player[player_number].twitter = "";
                player[player_number].region = "";
                player[player_number].sponsor = "";
                for (int ii = 0; ii <= 4; ii++)
                {
                    player[player_number].character[ii] = "";
                    player[player_number].color[ii] = 1;
                }
                switch (player_count)
                {
                    case 2:
                        switch (player_number)
                        {
                            case 1:
                                txt_twitter1.Text = "";
                                update_characters(ref cbx_character1);
                                break;
                            case 2:
                                txt_twitter2.Text = "";
                                update_characters(ref cbx_character2);
                                break;
                        }
                        break;
                    case 4:
                        switch (player_number)
                        {
                            case 1:
                                txt_team1_twitter1.Text = "";
                                update_characters(ref cbx_team1_character1);
                                break;
                            case 2:
                                txt_team2_twitter1.Text = "";
                                update_characters(ref cbx_team2_character1);
                                break;
                            case 3:
                                txt_team1_twitter2.Text = "";
                                update_characters(ref cbx_team1_character2);
                                break;
                            case 4:
                                txt_team2_twitter2.Text = "";
                                update_characters(ref cbx_team2_character2);
                                break;
                        }
                        break;
                }
                break;
            }
            if (global_values.roster[i].tag == player_name[player_number])
            {
                player[player_number] = global_values.roster[i];
                break;
            }
        }
    }

    Google.Apis.Sheets.v4.Data.ValueRange currentmatch = new Google.Apis.Sheets.v4.Data.ValueRange();

    switch (player_count)
    {
        case 2:
            update_names(ref cbx_tag1);
            cbx_tag1.SelectedIndex = cbx_tag1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0
            cbx_tag1.Text = player[1].get_display_name();

            update_names(ref cbx_tag2);
            cbx_tag2.SelectedIndex = cbx_tag2.Items.IndexOf(player[2].tag);   //Set the combobox index to 0
            cbx_tag2.Text = player[2].get_display_name();

            cbx_round.Text = upcoming_matches[3 + round_number][1].ToString();

            cbx_color1.SelectedIndex = player[1].color[0] - 1;
            cbx_color2.SelectedIndex = player[2].color[0] - 1;



            var oblist = new List<object>() { "Tournament Name", "Bracket URL", ".", "Current Match", "P1",  "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", "Next Match", "P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist2 = new List<object>() { "", "", ".", "", "", player[1].tag, player[1].twitter, player[1].region, player[1].sponsor, player[1].character[0] ,
                                        "", "", "", player[3].tag, player[3].twitter, player[3].region, player[3].sponsor, player[3].character[0]};
            var oblist3 = new List<object>() { txt_tournament.Text, txt_bracket.Text, "", cbx_round.Text, "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", upcoming_matches[5 + round_number][1].ToString(), "P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist4 = new List<object>() { "", "", ".", "", "", player[2].tag, player[2].twitter, player[2].region, player[2].sponsor, player[2].character[0] ,
                                        "", "", "", player[4].tag, player[4].twitter, player[4].region, player[4].sponsor, player[4].character[0] };

            // The new values to apply to the spreadsheet.
            currentmatch.Values = new List<IList<object>> { oblist, oblist2, oblist3, oblist4 };
            currentmatch.Range = "Current Round Info!A1:D18";
            currentmatch.MajorDimension = "COLUMNS";
            break;
        case 4:
            update_names(ref cbx_team1_name1);
            cbx_team1_name1.SelectedIndex = cbx_team1_name1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0
            cbx_team1_name1.Text = player[1].get_display_name();

            update_names(ref cbx_team2_name1);
            cbx_team2_name1.SelectedIndex = cbx_team2_name1.Items.IndexOf(player[2].tag);   //Set the combobox index to 0
            cbx_team2_name1.Text = player[2].get_display_name();

            update_names(ref cbx_team1_name2);
            cbx_team1_name2.SelectedIndex = cbx_team1_name2.Items.IndexOf(player[3].tag);   //Set the combobox index to 0
            cbx_team1_name2.Text = player[3].get_display_name();

            update_names(ref cbx_team2_name2);
            cbx_team2_name2.SelectedIndex = cbx_team2_name2.Items.IndexOf(player[4].tag);   //Set the combobox index to 0
            cbx_team2_name2.Text = player[4].get_display_name();


            cbx_team_round.Text = upcoming_matches[3 + round_number][1].ToString();

            cbx_team1_color1.SelectedIndex = player[1].color[0] - 1;
            cbx_team2_color1.SelectedIndex = player[2].color[0] - 1;
            cbx_team1_color2.SelectedIndex = player[3].color[0] - 1;
            cbx_team2_color2.SelectedIndex = player[4].color[0] - 1;



            var oblist11 = new List<object>() { "Tournament Name", "Bracket URL", ".", "Current Match", "TEAM1 P1",  "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", "Next Match", "TEAM1 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist22 = new List<object>() { "", "", ".", "", "", player[1].tag, player[1].twitter, player[1].region, player[1].sponsor, player[1].character[0] ,
                                        "", "", "", player[5].tag, player[5].twitter, player[5].region, player[5].sponsor, player[5].character[0]};
            var oblist33 = new List<object>() { txt_tournament.Text, txt_bracket.Text, "", cbx_team_round.Text, "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", upcoming_matches[5 + round_number][1].ToString(), "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist44 = new List<object>() { "", "", ".", "", "", player[3].tag, player[3].twitter, player[3].region, player[3].sponsor, player[3].character[0] ,
                                        "", "", "", player[7].tag, player[7].twitter, player[7].region, player[7].sponsor, player[7].character[0] };
            var oblist55 = new List<object>() { "", "", "", "", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", ".", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist66 = new List<object>() { "", "", ".", "", "", player[2].tag, player[2].twitter, player[2].region, player[2].sponsor, player[2].character[0] ,
                                        "", "", "", player[6].tag, player[6].twitter, player[6].region, player[6].sponsor, player[6].character[0] };
            var oblist77 = new List<object>() { "", "", "", "", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" ,
                                        ".", ".", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
            var oblist88 = new List<object>() { "", "", ".", "", "", player[4].tag, player[4].twitter, player[4].region, player[4].sponsor, player[4].character[0] ,
                                        "", "", "", player[8].tag, player[8].twitter, player[8].region, player[8].sponsor, player[8].character[0] };

            // The new values to apply to the spreadsheet.
            currentmatch.Values = new List<IList<object>> { oblist11, oblist22, oblist33, oblist44, oblist55, oblist66, oblist77, oblist88 };
            currentmatch.Range = "Current Round Info!A1:H18";
            currentmatch.MajorDimension = "COLUMNS";
            break;
    }





    var oblist5 = new List<object>() { (round_number).ToString(), "Next Match to Stream", "" };

    Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
    upcoming.Range = "Upcoming Matches!B2:D2";
    upcoming.Values = new List<IList<object>> { oblist5 }; ;
    upcoming.MajorDimension = "ROWS";

    List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>() { currentmatch, upcoming };  // TODO: Update placeholder value.

    // TODO: Assign values to desired properties of `requestBody`:
    Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
    requestBody.ValueInputOption = "RAW";
    requestBody.Data = data;

    SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = service.Spreadsheets.Values.BatchUpdate(requestBody, spreadsheetId);

    // To execute asynchronously in an async method, replace `request.Execute()` as shown:
    Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();



    if (global_values.auto_update == true)
    {
        //Save Player 1's information to seperate files to be used by the stream program
        string output_name = get_output_name(cbx_tag1.Text, ckb_loser1.Checked, 1);
        System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", output_name);
        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_twitter1.Text);
        System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
        System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", cbx_character1.Text);
        //Save Player 2's information to seperate files to be used by the stream program
        output_name = get_output_name(cbx_tag2.Text, ckb_loser2.Checked, 2);
        System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", output_name);
        System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_twitter2.Text);
        System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
        System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", cbx_character2.Text);
        //Save the Tournament information to seperate files to be used by the stream program
        System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
        System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
        System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
    }
}


















public static void info_from_sheets()
{
    UserCredential credential;

    using (var stream =
        new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
    {
        string credPath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { SheetsService.Scope.Spreadsheets },
            global_values.youtube_username,
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
    }

    // Create Google Sheets API service.
    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = this.GetType().ToString()
    });

    // Define request parameters.
    String spreadsheetId = "DERPDERPDERPDERPDERPDERP";
    SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId,
        "Player Information!A2:O" + (global_values.max_players + 1).ToString());
    Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();

    IList<IList<Object>> PlayerRecordModelrmation = response.Values;

    //Populate the player roster with information from the player information sheet
    for (int i = 0; i <= global_values.max_players; i++)
    {
        if (PlayerRecordModelrmation[i][0].ToString() == "")
        {
            global_values.roster_size = i - 1;
            break;
        }
        global_values.roster[i] = new PlayerRecordModel();
        global_values.roster[i].tag = PlayerRecordModelrmation[i][0].ToString();
        global_values.roster[i].twitter = PlayerRecordModelrmation[i][1].ToString();
        global_values.roster[i].region = PlayerRecordModelrmation[i][2].ToString();
        global_values.roster[i].sponsor = PlayerRecordModelrmation[i][3].ToString();
        for (int ii = 0; ii <= 4; ii++)
        {
            global_values.roster[i].character[ii] = PlayerRecordModelrmation[i][4 + ii].ToString();
        }
        for (int ii = 0; ii <= 4; ii++)
        {
            if (PlayerRecordModelrmation[i][9 + ii] != null && PlayerRecordModelrmation[i][9 + ii] != "")
            {
                string add_color = PlayerRecordModelrmation[i][9 + ii].ToString();
                global_values.roster[i].color[ii] = Int32.Parse(add_color);
            }
            else
            {
                global_values.roster[i].color[ii] = 1;
            }
        }
    }

    update_names(ref cbx_tag1);
    update_names(ref cbx_tag2);
    update_names(ref cbx_team1_name1);
    update_names(ref cbx_team1_name2);
    update_names(ref cbx_team2_name1);
    update_names(ref cbx_team2_name2);
}



















public static void check_sheets()
{
    UserCredential credential;

    using (var stream =
        new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
    {
        string credPath = System.Environment.GetFolderPath(
            System.Environment.SpecialFolder.Personal);
        credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

        credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            GoogleClientSecrets.Load(stream).Secrets,
            new[] { SheetsService.Scope.Spreadsheets },
            global_values.youtube_username,
            CancellationToken.None,
            new FileDataStore(credPath, true)).Result;
    }

    // Create Google Sheets API service.
    var service = new SheetsService(new BaseClientService.Initializer()
    {
        HttpClientInitializer = credential,
        ApplicationName = this.GetType().ToString()
    });

    // Define request parameters.
    String spreadsheetId = "DERPDERPDERPDERPDERPDERP";
    SpreadsheetsResource.GetRequest request = service.Spreadsheets.Get(spreadsheetId);
    Google.Apis.Sheets.v4.Data.Spreadsheet response;
    try
    {
        response = request.Execute();
    }
    catch (Exception ex)
    {
        MessageBox.Show("The Entered Sheet ID is not valid! Please ensure that the ID is correct and the provided Google Username has viewing and editing rights.");
        return;
    }
    //Values.Get(spreadsheetId,
    //"Player Information!A2:O" + (MAX_PLAYERS + 1).ToString());

    IList<Google.Apis.Sheets.v4.Data.Sheet> sheet_information = response.Sheets;

    btn_previous_match.Enabled = false;
    btn_previous_match.Visible = false;
    cbx_format.Enabled = true;

    int sheet_number = sheet_information.Count;
    switch (sheet_number)
    {
        case 5:
            if (sheet_information[0].Properties.Title == "Current Round Info" &&
                sheet_information[1].Properties.Title == "Upcoming Matches" &&
                sheet_information[2].Properties.Title == "Player Information" &&
                sheet_information[3].Properties.Title == "Characters and Rounds")
            {
                switch (sheet_information[4].Properties.Title)
                {
                    case "Singles":
                        MessageBox.Show("The designated Google Sheet contains the following information:\n\n" +
                                        "Player Database\n" +
                                        "Stream Queue for Singles\n\n" +
                                        "Master Orders will use adapt to its information.");
                        cbx_format.Text = "Singles";
                        break;
                    case "Doubles":
                        MessageBox.Show("The designated Google Sheet contains the following information:\n\n" +
                                        "Player Database\n" +
                                        "Stream Queue for Doubles\n\n" +
                                        "Master Orders will use adapt to its information.");
                        cbx_format.Text = "Doubles";
                        break;
                    default:
                        MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
                        return;
                }
                cbx_format.Enabled = false;
                btn_previous_match.Enabled = true;
                btn_previous_match.Visible = true;
                global_values.sheets_style = "info-and-queue";
                XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                xml.Root.Element("google-sheets").Element("sheet-style").ReplaceWith(new XElement("sheet-style", global_values.sheets_style));
                xml.Save(SettingsFile.settingsFile);
                info_from_sheets();
            }
            else
            {
                MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
            }
            break;
        case 2:
            if (sheet_information[0].Properties.Title == "Player Information" &&
                sheet_information[1].Properties.Title == "Characters")
            {
                MessageBox.Show("The designated Google Sheet contains only player information. " +
                                "Master Orders will use adapt to its information.");
                global_values.sheets_style = "info-only";
                XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                xml.Root.Element("google-sheets").Element("sheet-style").ReplaceWith(new XElement("sheet-style", global_values.sheets_style));
                xml.Save(SettingsFile.settingsFile);
                info_from_sheets();
            }
            else
            {
                MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
            }
            break;
        default:
            MessageBox.Show("The designated Google Sheet is not formatted to be used with Master Orders.");
            break;
    }
}
*/

#endregion Goog Sheets Info