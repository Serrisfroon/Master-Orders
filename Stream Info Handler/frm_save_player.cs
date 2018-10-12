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
        public frm_save_player(player_info save_player)
        {
            InitializeComponent();
            load_combobox(ref cbx_main, false);
            load_combobox(ref cbx_secondary2, true);
            load_combobox(ref cbx_secondary3, true);
            load_combobox(ref cbx_secondary4, true);
            load_combobox(ref cbx_secondary5, true);
            txt_tag.Text = save_player.tag;
            txt_region.Text = save_player.region;
            txt_sponsor.Text = save_player.sponsor;
            txt_twitter.Text = save_player.twitter;
            cbx_main.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[0]);
            cbx_secondary2.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[1]);
            cbx_secondary3.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[2]);
            cbx_secondary4.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[3]);
            cbx_secondary5.SelectedIndex = cbx_main.Items.IndexOf(save_player.character[4]);
        }

        private void load_combobox(ref ComboBox sender, bool add_character)
        {
            sender.BeginUpdate();                                      //Begin
            sender.Items.Clear();                                      //Empty the item list
            if(add_character == true)
            {
                sender.Items.Add("Add a Secondary");                   
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

        }
    }
}
