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
        public static SqlDatabaseConnection Instance()
        {
            if (_instance == null) //remove this line if issues arise
                _instance = new SqlDatabaseConnection();
            return _instance;
        }

        public bool IsConnect()
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

        public void Close()
        {
            _connection.Close();
            _connection = null;
        }

        public void Insert(string strSQL, List<MySqlParameter> paramslist)
        {
            if (IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, _connection);
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
