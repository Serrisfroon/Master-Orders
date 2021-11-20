using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_top8_character : Form
    {
        string character_path;
        string CharacterImageType;
        public frm_top8_character(ComboBox.ObjectCollection characters, string old_image, string path, string type)
        {
            this.CenterToScreen();

            InitializeComponent();

            cbx_character.Items.AddRange(characters.Cast<Object>().ToArray());
            character_path = path;
            CharacterImageType = type;

            if (File.Exists(old_image))
            {
                int dirlength = path.Length+1;
                string subdir = old_image.Substring(dirlength, old_image.Length - dirlength);
                int cutoff = subdir.IndexOf(@"\");
                string character_name = subdir.Substring(0, cutoff);
                int color_cutoff = (subdir.Substring(cutoff + 1, subdir.Length - cutoff - 1)).IndexOf(@"\");
                int character_color = Int32.Parse(subdir.Substring(cutoff+1, color_cutoff))-1;
                cbx_character.SelectedIndex = cbx_character.FindString(character_name);
                cbx_color.SelectedIndex = character_color;
            }
            else
                cbx_character.SelectedIndex = 0;
        }

        private void cbx_character_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selected_path = character_path + @"\" + cbx_character.Text;

            int colors_count = Directory.GetDirectories(selected_path).Length;

            //Create an array of all possible colors for this character 
            Image[] colors = new Image[colors_count];
            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = Image.FromFile(selected_path + @"\" + (i + 1).ToString() + @"\stamp.png");
            }
            //Display all possible colors in the combobox
            cbx_color.DisplayImages(colors, 82);
            cbx_color.SelectedIndex = 0;
        }

        private void btn_okay_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            frm_othertools.new_character_image = character_path + @"\" + cbx_character.Text + @"\" + (cbx_color.SelectedIndex + 1).ToString() + @"\" + CharacterImageType + ".png";
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            
        }

        private void cbx_character_Leave(object sender, EventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb.FindString(cb.Text) < 0)
            {
                cb.SelectedIndex = 0;
            }
        }

        private void cbx_character_KeyUp(object sender, KeyEventArgs e)
        {
            ComboBox cb = (ComboBox)sender;
            string nextkey = "";

            while (cb.FindString(cb.Text) < 0 && cb.Text.Length > 0)

            {
                //Find the previously enterred text
                string subStringText = cb.Text.Substring(0, cb.Text.Length - 1);
                if (subStringText.Length > 0)
                {
                    nextkey = subStringText.Substring(subStringText.Length - 1, 1);
                }
                else
                {
                    nextkey = "";
                }
                cb.Text = subStringText;
                cb.Select(subStringText.Length, 0);


            }
            if (nextkey != "")
            {
                cb.Text = cb.Text.Substring(0, cb.Text.Length - 1);
                //Need to remove most recent key
                SendKeys.Send(nextkey);
            }
        }
    }
}
