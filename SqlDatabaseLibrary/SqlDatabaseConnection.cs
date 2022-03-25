using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace SqlDatabaseLibrary
{
    public class SqlDatabaseConnection
    {
        private SqlDatabaseConnection()
        {
        }

        public string databaseUserName { get; set; }
        public string databasePassword { get; set; }
        public string databaseName { get; set; }

        private MySqlConnection _connection = null;
        public MySqlConnection connection
        {
            get { return _connection; }
        }

        private static SqlDatabaseConnection _instance = null;
        /// <summary>
        /// Initialize the singleton class on first run, return it on all subsequent runs
        /// </summary>
        /// <returns></returns>
        public static SqlDatabaseConnection Instance()
        {
            if (_instance == null) //remove this line if issues arise
                _instance = new SqlDatabaseConnection();
            return _instance;
        }

        /// <summary>
        /// Attempts to connect to SQL using the credentials held by the singleton class
        /// </summary>
        /// <returns>True is successful, false if not</returns>
        public bool TryDatabaseConnection()
        {
            if (connection == null)
            {
                if (String.IsNullOrEmpty(databaseName))
                    return false;
                string connstring = string.Format("Server=107.180.3.224; database={0}; UID={1}; password={2}", databaseName, databaseUserName, databasePassword);
                _connection = new MySqlConnection(connstring);
                _connection.Open();
            }

            return true;
        }

        /// <summary>
        /// Closes the active connection and removes it from the singleton class.
        /// </summary>
        public void Close()
        {
            _connection.Close();
            _connection = null;
        }

        /// <summary>
        /// Runs an Insert command on the connected database
        /// </summary>
        /// <param name="strSQL">The SQL command</param>
        /// <param name="paramslist">parameters to be used for the command</param>
        public void Insert(string strSQL, List<MySqlParameter> paramslist)
        {
            if (TryDatabaseConnection())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
                foreach (MySqlParameter param in paramslist)
                    cmd.Parameters.Add(param);

                int rows = cmd.ExecuteNonQuery();
                //MessageBox.Show(cmd.CommandText + "\n \nAffected Rows: " + rows.ToString());
            }
        }

        /// <summary>
        /// Runs a Select command on the connected database
        /// </summary>
        /// <param name="strSQL">The SQL command</param>
        /// <param name="paramslist">parameters to be used for the command</param>
        /// <returns>The result of the select command</returns>
        public MySqlDataReader Select(string strSQL, List<MySqlParameter> paramslist)
        {
            if (TryDatabaseConnection())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
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
