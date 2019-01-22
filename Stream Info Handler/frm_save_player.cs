using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_save_player : Form
    {
        public static string secondary_text = "Add a Secondary";
        string hold_name;
        public frm_save_player(player_info save_player)
        {
            InitializeComponent();
            load_combobox(ref cbx_main, false);
            load_combobox(ref cbx_secondary2, true);
            load_combobox(ref cbx_secondary3, true);
            load_combobox(ref cbx_secondary4, true);
            load_combobox(ref cbx_secondary5, true);
            txt_tag.Text = save_player.tag;
            cbx_region.Text = save_player.region;
            txt_sponsor.Text = save_player.sponsor;
            txt_twitter.Text = save_player.twitter;
            cbx_main.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[0]);
            cbx_colors1.SelectedIndex = save_player.color[0] - 1;

            if (save_player.character[1] != "")
            {
                cbx_secondary2.SelectedIndex = cbx_secondary2.Items.IndexOf(save_player.character[1]);
                cbx_colors2.SelectedIndex = save_player.color[1] - 1;
            }
            if (save_player.character[2] != "")
            {
                cbx_secondary3.SelectedIndex = cbx_secondary3.Items.IndexOf(save_player.character[2]);
                cbx_colors3.SelectedIndex = save_player.color[2] - 1;
            }
            if (save_player.character[3] != "")
            {
                cbx_secondary4.SelectedIndex = cbx_secondary4.Items.IndexOf(save_player.character[3]);
                cbx_colors4.SelectedIndex = save_player.color[3] - 1;
            }
            if (save_player.character[4] != "")
            {
                cbx_secondary5.SelectedIndex = cbx_secondary5.Items.IndexOf(save_player.character[4]);
                cbx_colors5.SelectedIndex = save_player.color[4] - 1;
            }

        }
        public frm_save_player(string new_name)
        {
            InitializeComponent();
            load_combobox(ref cbx_main, false);
            load_combobox(ref cbx_secondary2, true);
            load_combobox(ref cbx_secondary3, true);
            load_combobox(ref cbx_secondary4, true);
            load_combobox(ref cbx_secondary5, true);
            txt_tag.Text = new_name;
        }

        private void load_combobox(ref ComboBox sender, bool add_character)
        {
            sender.BeginUpdate();                                      //Begin
            sender.Items.Clear();                                      //Empty the item list
            if(add_character == true)
            {
                sender.Items.Add(secondary_text);                   
            }
            int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                                                                                //Loop through every character
            for (int x = 0; x < character_count; x++)
            {
                sender.Items.Add(global_values.characters[x]);         //Add the character's name to the combobox
            }
            sender.EndUpdate();                                        //End
            sender.SelectedIndex = 0;                                  //Set the combobox index to 0
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(txt_tag.Text == "")
            {
                txt_tag.BackColor = frm_main.warning_color;
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            player_info save_player = new player_info();
            save_player.tag = txt_tag.Text;
            save_player.twitter = txt_twitter.Text;
            save_player.region = cbx_region.Text;
            save_player.sponsor = txt_sponsor.Text;
            save_player.fullname = txt_name.Text;
            save_player.fullsponsor = txt_fullsponsor.Text;

            save_player.character[0] = cbx_main.Text;
            save_player.color[0] = cbx_colors1.SelectedIndex + 1;

            for(int i = 1; i < 5; i ++)
            {
                save_player.character[i] = "";
                save_player.color[i] = 1;
            }

            int next_slot = 1;

            if (cbx_secondary2.Text != secondary_text)
            {
                save_player.character[next_slot] = cbx_secondary2.Text;
                save_player.color[next_slot] = cbx_colors2.SelectedIndex + 1;
                next_slot++;
            }
            if (cbx_secondary3.Text != secondary_text)
            {
                save_player.character[next_slot] = cbx_secondary3.Text;
                save_player.color[next_slot] = cbx_colors3.SelectedIndex + 1;
                next_slot++;
            }
            if (cbx_secondary4.Text != secondary_text)
            {
                save_player.character[next_slot] = cbx_secondary4.Text;
                save_player.color[next_slot] = cbx_colors4.SelectedIndex + 1;
                next_slot++;
            }
            if (cbx_secondary5.Text != secondary_text)
            {
                save_player.character[next_slot] = cbx_secondary5.Text;
                save_player.color[next_slot] = cbx_colors5.SelectedIndex + 1;
                next_slot++;
            }

            frm_main.get_new_player = save_player;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void load_combobox_images(ref ComboBox sender, string character_name)
        {
            sender.Items.Clear();
            string character_path = global_values.game_path + @"\" + character_name;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
            Image[] colors = new Image[colors_count];
            
            for(int i = 0; i < colors_count; i ++)
            {
                colors[i] = Image.FromFile(character_path + @"\" + (i+1).ToString() + @"\stamp.png");
            }

            sender.DisplayImages(colors);
            sender.SelectedIndex = 0;
            sender.DropDownHeight = 400;
        }

        private void cbx_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox characters = (ComboBox)sender;
            ComboBox colors = cbx_colors1;
            if (characters.Text != secondary_text)
            {
                colors.Enabled = true;
                load_combobox_images(ref colors, characters.Text);
                colors.SelectedIndex = 0;
            }
            else
            {
                colors.Items.Clear();
                colors.Enabled = false;
            }
        }

        private void cbx_secondary2_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox characters = (ComboBox)sender;
            ComboBox colors = cbx_colors2;
            if (characters.Text != secondary_text)
            {
                colors.Enabled = true;
                load_combobox_images(ref colors, characters.Text);
                colors.SelectedIndex = 0;
            }
            else
            {
                colors.Items.Clear();
                colors.Enabled = false;
            }
        }

        private void cbx_secondary3_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox characters = (ComboBox)sender;
            ComboBox colors = cbx_colors3;
            if (characters.Text != secondary_text)
            {
                colors.Enabled = true;
                load_combobox_images(ref colors, characters.Text);
                colors.SelectedIndex = 0;
            }
            else
            {
                colors.Items.Clear();
                colors.Enabled = false;
            }
        }

        private void cbx_secondary4_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox characters = (ComboBox)sender;
            ComboBox colors = cbx_colors4;
            if (characters.Text != secondary_text)
            {
                colors.Enabled = true;
                load_combobox_images(ref colors, characters.Text);
                colors.SelectedIndex = 0;
            }
            else
            {
                colors.Items.Clear();
                colors.Enabled = false;
            }
        }

        private void cbx_secondary5_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox characters = (ComboBox)sender;
            ComboBox colors = cbx_colors5;
            if (characters.Text != secondary_text)
            {
                colors.Enabled = true;
                load_combobox_images(ref colors, characters.Text);
                colors.SelectedIndex = 0;
            }
            else
            {
                colors.Items.Clear();
                colors.Enabled = false;
            }
        }

        private void txt_tag_TextChanged(object sender, EventArgs e)
        {
            if(txt_tag.Text != "")
            {
                txt_tag.BackColor = Color.White;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
