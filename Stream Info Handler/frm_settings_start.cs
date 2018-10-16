//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2018, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
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
    public partial class frm_settings_start : Form
    {
        public string auto_update = "true";
        public string software = "XSplit";

        public frm_settings_start()
        {
            InitializeComponent();
            this.ControlBox = false;
        }

        private void btn_browse_roster_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_roster_directory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void btn_output_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_stream_directory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void rdb_automatic_CheckedChanged(object sender, EventArgs e)
        {
            auto_update = "true";
        }

        private void rdb_manual_CheckedChanged(object sender, EventArgs e)
        {
            auto_update = "false";
        }

        private void btn_thumb_directory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_thumbnail_directory.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void txt_thumbnail_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_thumbnail_directory.Text != @"")
            {
                if (Directory.Exists(txt_thumbnail_directory.Text))
                {
                    txt_thumbnail_directory.BackColor = Color.White;
                }
                else
                {
                    txt_thumbnail_directory.BackColor = Color.Red;
                }
            }
        }


        private void txt_roster_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_roster_directory.Text != @"")
            {
                if (Directory.Exists(txt_roster_directory.Text))
                {
                    txt_roster_directory.BackColor = Color.White;
                }
                else
                {
                    txt_roster_directory.BackColor = Color.Red;
                }
            }
        }

        private void txt_stream_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_stream_directory.Text != @"")
            {
                if (Directory.Exists(txt_stream_directory.Text))
                {
                    txt_stream_directory.BackColor = Color.White;
                }
                else
                {
                    txt_stream_directory.BackColor = Color.Red;
                }
            }
        }

        private void btn_finish_Click(object sender, EventArgs e)
        {
            bool complete_update = true;

            if (txt_stream_directory.Text != @"")
            {
                if (!Directory.Exists(txt_stream_directory.Text))
                {
                    txt_stream_directory.BackColor = Color.Red;
                    complete_update = false;
                }
            }
            if (txt_roster_directory.Text != @"")
            {
                if (!Directory.Exists(txt_roster_directory.Text))
                {
                    txt_roster_directory.BackColor = Color.Red;
                    complete_update = false;
                }
            }
            if (txt_thumbnail_directory.Text != @"")
            {
                if (!Directory.Exists(txt_thumbnail_directory.Text))
                {
                    txt_thumbnail_directory.BackColor = Color.Red;
                    complete_update = false;
                }
            }
            if (txt_vods.Text != @"")
            {
                if (!Directory.Exists(txt_vods.Text))
                {
                    txt_vods.BackColor = Color.Red;
                    complete_update = false;
                }
            }

            if (complete_update == true)
            {
                string default_youtube_description =
                    "INFO_TOURNAMENT | INFO_DATE \r\nINFO_BRACKET\r\nRomeoville, Illinois \r\nOrganized and streamed by UGS Gaming \r\nWatch live at https://www.twitch.tv/ugsgaming \r\nFollow us and our players on Twitter! \r\n@UGS_GAMlNG \r\nINFO_PLAYER1: INFO_TWITTER1 \r\nINFO_PLAYER2: INFO_TWITTER2";
                default_youtube_description = Regex.Replace(default_youtube_description, @"\r\n|\n|\r", Environment.NewLine);
                XDocument doc = new XDocument(
                    new XElement("Master-Orders-Settings",
                    new XElement("directories",
                         new XElement("game-directory", txt_roster_directory.Text),
                         new XElement("stream-directory", txt_stream_directory.Text),
                         new XElement("thumbnail-directory", txt_thumbnail_directory.Text),
                         new XElement("vods-directory", txt_vods.Text)
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
                         new XElement("startup-sheets", "false"),
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
                    new XElement("etc",
                         new XElement("automatic-updates", auto_update),
                         new XElement("stream-software", software),
                         new XElement("settings-version", "2")
                        )));

                doc.Save(global_values.settings_file);
                this.Close();
            }
        }

        private void txt_vods_TextChanged(object sender, EventArgs e)
        {
            if (txt_vods.Text != @"")
            {
                if (Directory.Exists(txt_vods.Text))
                {
                    txt_vods.BackColor = Color.White;
                }
                else
                {
                    txt_vods.BackColor = Color.Red;
                }
            }
        }

        private void btn_vods_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_vods.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            software = "XSplit";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            software = "OBS";
        }
    }
}
