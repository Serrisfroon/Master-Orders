using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace TournamentDataLibrary
{
    static public class UserSession
    {
        static public int userId;

        public static string LogIn(string table, string user, string pass)
        {
            var dbCon = SqlDatabaseConnection.Instance();
            dbCon.databaseName = table;
            dbCon.databaseUserName = user;
            dbCon.databasePassword = pass;
            try
            {
                //Connect to the database. Fails if incorrect credentials
                dbCon.IsConnect();

                List<MySqlParameter> playerparameters = new List<MySqlParameter>();
                playerparameters.Add(new MySqlParameter("@user", dbCon.databaseUserName));

                //Verify the user is active
                string query = "SELECT ACTIVE, ID FROM Mastercore.USERS WHERE USERNAME=@user";
                var cmd = new MySqlCommand(query, dbCon.connection);
                foreach (MySqlParameter param in playerparameters)
                    cmd.Parameters.Add(param);

                var reader = cmd.ExecuteReader();
                reader.Read();
                bool activeUser = reader.GetInt32(0) != 0;
                int readUserId = reader.GetInt32(1);
                reader.Close();
                dbCon.Close();

                //Return false if the user is inactive
                if (activeUser == false)
                {
                    return $"{ dbCon.databaseUserName } is not currently an active Master Orders account. If you are the account owner, please contact Serris via twitter @serrisfroon";
                }
                userId = readUserId;

                return "Success";
            }
            catch (Exception ex)
            {
                dbCon.Close();
                return ex.ToString();
            }
        }
    }
}
