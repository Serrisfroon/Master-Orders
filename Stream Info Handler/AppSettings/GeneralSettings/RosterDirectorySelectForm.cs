using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class RosterDirectorySelectForm : Form
    {
        string[] gameDirectories;
        public string outputDirectory;
        public RosterDirectorySelectForm(string[] gameDirectoryiesInput, string gameName)
        {
            this.CenterToScreen();
            this.TopMost = true;
            InitializeComponent();
            gameDirectories = gameDirectoryiesInput;

            lbl_text.Text = "Select the character data directory for the game '" + gameName + "' to use.";

            List<string> shortenedDirectoryList = new List<string>();
            foreach (string gameDirectory in gameDirectoryiesInput)
            {
                shortenedDirectoryList.Add(ShrinkPath(gameDirectory, 30));
            }

            cbx_tables.BeginUpdate();
            foreach(string directory in shortenedDirectoryList)
            {
                cbx_tables.Items.Add(directory);
            }
            cbx_tables.EndUpdate();
        }

        private void btn_okay_Click(object sender, EventArgs e)
        {
            //Pass the directory to back database tools
            outputDirectory = gameDirectories[cbx_tables.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Shortens the given directory's path as needed.
        /// </summary>
        /// <param name="fullDirectoryPath">The directory path to shorten</param>
        /// <param name="characterLimit">The minimum number of characters needed for shortening to occur</param>
        /// <param name="delimiter">A string to be appended to the front of the shortened directory path</param>
        /// <returns>The shortened directory path</returns>
        public static string ShrinkPath(string fullDirectoryPath, int characterLimit, string delimiter = "…")
        {
            //no path provided
            if (string.IsNullOrEmpty(fullDirectoryPath))
            {
                return "";
            }

            var name = Path.GetFileName(fullDirectoryPath); 
            int namelen = name.Length;
            int pathlen = fullDirectoryPath.Length;
            var dir = fullDirectoryPath.Substring(0, pathlen - namelen);

            int delimlen = delimiter.Length;
            int idealminlen = namelen + delimlen;

            var slash = (fullDirectoryPath.IndexOf("/") > -1 ? "/" : "\\");

            //less than the minimum amt
            if (characterLimit < ((2 * delimlen) + 1))
            {
                return "";
            }

            //fullpath
            if (characterLimit >= pathlen)
            {
                return fullDirectoryPath;
            }

            //file name condensing
            if (characterLimit < idealminlen)
            {
                return delimiter + name.Substring(0, (characterLimit - (2 * delimlen))) + delimiter;
            }

            //whole name only, no folder structure shown
            if (characterLimit == idealminlen)
            {
                return delimiter + name;
            }

            return dir.Substring(0, (characterLimit - (idealminlen + 1))) + delimiter + slash + name;
        }
    }
}
