using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDatabaseLibrary
{
    static public class UserSession
    {
        /// <summary>
        /// The user ID provided by the SQL database. Used to pull data associated to the user in shared spaces.
        /// </summary>
        static public int userId;

        public static string LogIn(string table, string user, string pass)
        {
            var databaseConnection = SqlDatabaseConnection.Instance();
            databaseConnection.databaseName = table;
            databaseConnection.databaseUserName = user;
            databaseConnection.databasePassword = pass;
            try
            {
                //Connect to the database. Fails if incorrect credentials
                databaseConnection.TryDatabaseConnection();

                List<MySqlParameter> playerparameters = new List<MySqlParameter>();
                playerparameters.Add(new MySqlParameter("@user", databaseConnection.databaseUserName));

                //Verify the user is active
                string query = "SELECT ACTIVE, ID FROM Mastercore.USERS WHERE USERNAME=@user";
                var cmd = new MySqlCommand(query, databaseConnection.connection);
                foreach (MySqlParameter param in playerparameters)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                bool activeUser = reader.GetInt32(0) != 0;
                int readUserId = reader.GetInt32(1);
                reader.Close();
                databaseConnection.Close();

                //Return false if the user is inactive
                if (activeUser == false)
                {
                    return $"{ databaseConnection.databaseUserName } is not currently an active Master Orders account. If you are the account owner, please contact Serris via twitter @serrisfroon";
                }
                userId = readUserId;

                return "Success";
            }
            catch (Exception ex)
            {
                databaseConnection.Close();
                return ex.ToString();
            }
        }
    }
}
