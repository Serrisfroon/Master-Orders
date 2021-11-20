using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_tables : Form
    {
        string[] directories;
        public string outputDirectory;
        public frm_tables(string[] table_names, string findname)
        {
            this.CenterToScreen();
            this.TopMost = true;
            InitializeComponent();
            directories = table_names;

            lbl_text.Text = "Select the character data directory for the game '" + findname + "' to use.";

            List<string> clean_list = new List<string>();
            foreach (string checktable in table_names)
            {
                clean_list.Add(ShrinkPath(checktable, 30));
            }

            cbx_tables.BeginUpdate();
            foreach(string name in clean_list)
            {
                cbx_tables.Items.Add(name);
            }
            cbx_tables.EndUpdate();
            //cbx_tables.SelectedIndex = 0;
        }

        private void btn_okay_Click(object sender, EventArgs e)
        {
            //Pass the directory to back database tools
            outputDirectory = directories[cbx_tables.SelectedIndex];
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public static string ShrinkPath(string absolutepath, int limit, string delimiter = "…")
        {
            //no path provided
            if (string.IsNullOrEmpty(absolutepath))
            {
                return "";
            }

            var name = Path.GetFileName(absolutepath); 
            int namelen = name.Length;
            int pathlen = absolutepath.Length;
            var dir = absolutepath.Substring(0, pathlen - namelen);

            int delimlen = delimiter.Length;
            int idealminlen = namelen + delimlen;

            var slash = (absolutepath.IndexOf("/") > -1 ? "/" : "\\");

            //less than the minimum amt
            if (limit < ((2 * delimlen) + 1))
            {
                return "";
            }

            //fullpath
            if (limit >= pathlen)
            {
                return absolutepath;
            }

            //file name condensing
            if (limit < idealminlen)
            {
                return delimiter + name.Substring(0, (limit - (2 * delimlen))) + delimiter;
            }

            //whole name only, no folder structure shown
            if (limit == idealminlen)
            {
                return delimiter + name;
            }

            return dir.Substring(0, (limit - (idealminlen + 1))) + delimiter + slash + name;
        }
    }
}
