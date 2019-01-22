using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_startup : Form
    {
        public frm_startup()
        {
            InitializeComponent();

            this.CenterToScreen();

            //Show the login box
            var login_box = new frm_login();
            login_box.ShowDialog();
            //Check if logged in successfully
            switch(login_box.DialogResult)
            {
                case DialogResult.OK:

                    break;
                case DialogResult.Yes:

                    break;
                default:
                    //If not logged in, close the program
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(1);
                    }
                    return;
                    break;
            }

            //Check for updates
            string url = "http://masterorders.org/masterorders.html";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        string result = content.ReadAsStringAsync().Result;
                        if (result != "enabled")
                        {
                            MessageBox.Show("Master Orders is either out of date or not enabled for your organization. " +
                                "Please reach out to Serris via Twitter @serrisfroon for further support.");
                            if (System.Windows.Forms.Application.MessageLoop)
                            {
                                // WinForms app
                                System.Windows.Forms.Application.Exit();
                            }
                            else
                            {
                                // Console app
                                System.Environment.Exit(1);
                            }
                            return;
                        }

                    }
                }
            }
        }

        private void btn_stream_Click(object sender, EventArgs e)
        {

        }

        private void btn_settings_Click(object sender, EventArgs e)
        {
            var settings = new frm_settings();
            settings.ShowDialog();
        }
    }

    public class player_fields
    {
        public ComboBox tag;
        public TextBox twitter;
        public ComboBox character;
        public ComboBox color;
    }

    public class player_info
    {
        public string tag;
        public string twitter;
        public string region;
        public string sponsor;
        public string fullsponsor;
        public string fullname;
        public string[] character = new string[5];
        public int[] color = new int[5];
        public string get_display_name()
        {
            if (sponsor != "")
            {
                return sponsor + " | " + tag;
            }
            else
            {
                return tag;
            }
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

        public void Insert(string strSQL, List<MySqlParameter> paramslist)
        {
            if (IsConnect())
            {
                MySqlCommand cmd = new MySqlCommand(strSQL, connection);
                foreach (MySqlParameter param in paramslist)
                    cmd.Parameters.Add(param);

                cmd.ExecuteNonQuery();
                Close();
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
                Close();

                return reader;
            }
            else
            {
                return null;
            }

        }
    }

    public static class global_values
    {
        public static string database_username;
        public static string database_password;
        public static string database_name;

        public static bool enable_region;
        public static string region_directory;
        public static bool enable_sponsor;
        public static string sponsor_directory;
        public static string settings_file = @"C:\Users\Public\Stream Info Handler\settings.xml";
        public static int[] player_roster_number;
        public static string sheets_style;
        public static string sheets_info;
        public static bool enable_youtube;
        public static bool copy_video_title;
        public static int roster_size;
        public static player_info[] roster;
        public static bool first_match = true;
        public static string reenable_upload = "";
        public static string stream_software = @"XSplit";
        public static string temp_file;
        public static bool allow_upload = true;
        public static string current_youtube_data;
        public static FileSystemWatcher vod_monitor;
        public static string new_vod_detected = "";
        public static string[] characters;
        public static string[] game_info = { "", "" };
        public static string score1_image1 = @"file";
        public static string score1_image2 = @"file";
        public static string score1_image3 = @"file";
        public static string score2_image1 = @"file";
        public static string score2_image2 = @"file";
        public static string score2_image3 = @"file";
        public static string game_path;
        public static string output_directory;
        public static string thumbnail_directory;
        public static string json_file;
        public static string youtube_username;
        public static string vods_directory;
        public static bool auto_update = true;
        public static int player_number;
        public static int[] player_image;
        public static string playlist_name;
        public static string playlist_id;
        public static bool enable_sheets;
    }
}
