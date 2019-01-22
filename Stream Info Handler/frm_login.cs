using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler
{
    public partial class frm_login : Form
    {
        //Create a flag for checking if this is the first time logging in on this device
        bool first_login = false;

        public frm_login()
        {
            InitializeComponent();

            this.CenterToScreen();

            //Create the setting file directory if it doesn't exist
            if (!Directory.Exists(@"C:\Users\Public\Stream Info Handler"))
            {
                Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler");
            }

            //Check if the settings file exists
            if (File.Exists(global_values.settings_file))
            {
                //Read the settings
                XDocument xml = XDocument.Load(global_values.settings_file);

                //Check if the login was set to be remembered
                ckb_remember.Checked = Convert.ToBoolean((string)xml.Root.Element("login").Element("remember-login"));
                if(ckb_remember.Checked)
                {
                    //Retrieve the login credentials
                    txt_username.Text = (string)xml.Root.Element("login").Element("username");
                    txt_password.Text = (string)xml.Root.Element("login").Element("password");
                }
            }
            else
                //If the settings file doesn't exist, this is the first login
                first_login = true;
        }

        private void btn_login_Click(object sender, EventArgs e)
        {
            var dbCon = DBConnection.Instance();
            dbCon.DatabaseName = "Master Orders Global Playerbase";
            dbCon.DatabaseUserName = txt_username.Text;
            dbCon.DatabasePassword = txt_password.Text;
            try
            {
                dbCon.IsConnect();
                global_values.database_username = dbCon.DatabaseUserName;
                global_values.database_password = dbCon.DatabasePassword;
                this.DialogResult = DialogResult.OK;
                if (first_login == true)
                    create_settings();
                else
                {
                    XDocument xml = XDocument.Load(global_values.settings_file);
                    xml.Root.Element("login").Element("remember-login").ReplaceWith(new XElement("remember-login", ckb_remember.Checked.ToString()));
                    if (ckb_remember.Checked == true)
                    {
                        xml.Root.Element("login").Element("username").ReplaceWith(new XElement("username", txt_username.Text));
                        xml.Root.Element("login").Element("password").ReplaceWith(new XElement("password", txt_password.Text));
                    }
                    xml.Save(global_values.settings_file);

                }
                Close();
            }
            catch (Exception ex)
            {
                lbl_error.Text = "Login Failed! Make sure the credentials provided are correct.";
                dbCon.Close();
            }
        }

        private void llb_forgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If you've forgotten your Master Orders login credentials, please contact Serris via Twitter @serrisfroon");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    btn_login_Click(new object(), new EventArgs());
                break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            return true;
        }

        private void txt_field_Enter(object sender, EventArgs e)
        {
            TextBox field = (TextBox)sender;
            field.SelectionStart = 0;
            field.SelectionLength = field.Text.Length;
        }

        private void create_settings()
        {
            string default_youtube_description =
                "INFO_TOURNAMENT | INFO_DATE \r\n" +
                "INFO_BRACKET\r\nRomeoville, Illinois \r\n" +
                "Organized and streamed by UGS Gaming \r\n" +
                "Watch live at https://www.twitch.tv/ugsgaming \r\n" +
                "Follow us and our players on Twitter! \r\n" +
                "@UGSGamingLLC \r\n" +
                "INFO_PLAYER1: INFO_TWITTER1 \r\n" +
                "INFO_PLAYER2: INFO_TWITTER2";
            default_youtube_description = Regex.Replace(default_youtube_description, @"\r\n|\n|\r", Environment.NewLine);
            XDocument doc = new XDocument(
                new XElement("Master-Orders-Settings",
                new XElement("login",
                     new XElement("remember-login", ckb_remember.Checked.ToString()),
                     new XElement("username", global_values.database_username),
                     new XElement("password", global_values.database_password),
                     new XElement("keep-open", "False")
                     ), 
                new XElement("directories",
                     new XElement("game-directory", ""),
                     new XElement("stream-directory", ""),
                     new XElement("thumbnail-directory", ""),
                     new XElement("vods-directory", "")
                     ),
                new XElement("youtube",
                     new XElement("enable-youtube", "false"),
                     new XElement("username", "Master Orders Integration"),
                     new XElement("json-file", ""),
                     new XElement("copy-title", "false"),
                     new XElement("playlist-name", ""),
                     new XElement("playlist-id", ""),
                     new XElement("default-description", default_youtube_description)
                    ),
                new XElement("google-sheets",
                     new XElement("enable-sheets", "false"),
                     new XElement("sheet-style", "info-and-queue"),
                     new XElement("sheet-info", "info-and-queue"),
                     new XElement("sheets-id", "")
                    ),
                new XElement("image-scoring",
                     new XElement("enable-image-scoring", "false"),
                     new XElement("player1-1", ""),
                     new XElement("player1-2", ""),
                     new XElement("player1-3", ""),
                     new XElement("player2-1", ""),
                     new XElement("player2-2", ""),
                     new XElement("player2-3", "")
                    ),
                new XElement("sponsor-and-region",
                     new XElement("enable-sponsor", "False"),
                     new XElement("sponsor-directory", ""),
                     new XElement("enable-region", "False"),
                     new XElement("region-directory", "")
                    ),
                new XElement("etc",
                     new XElement("automatic-updates", "True"),
                     new XElement("stream-software", "XSplit"),
                     new XElement("format", "Singles"),
                     new XElement("settings-version", "4")
                    )));

            doc.Save(global_values.settings_file);
        }
    }
}
