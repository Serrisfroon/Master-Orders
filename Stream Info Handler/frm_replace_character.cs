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
    public partial class frm_replace_character : Form
    {
        public frm_replace_character(player_info player, string new_character)
        {
            InitializeComponent();
            CenterToScreen();
            lbl_character.Text = player.tag + " already has 5 characters! Select one to replace with " + new_character +".";
            btn_character1.Image = Image.FromFile(global_values.game_path + @"\" + player.character[0] + @"\" + player.color[0].ToString() + @"\stamp.png");
            btn_character2.Image = Image.FromFile(global_values.game_path + @"\" + player.character[1] + @"\" + player.color[1].ToString() + @"\stamp.png");
            btn_character3.Image = Image.FromFile(global_values.game_path + @"\" + player.character[2] + @"\" + player.color[2].ToString() + @"\stamp.png");
            btn_character4.Image = Image.FromFile(global_values.game_path + @"\" + player.character[3] + @"\" + player.color[3].ToString() + @"\stamp.png");
            btn_character5.Image = Image.FromFile(global_values.game_path + @"\" + player.character[4] + @"\" + player.color[4].ToString() + @"\stamp.png");
        }

        private void btn_okay_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = -1;
            this.Close();
        }

        private void btn_character1_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = 1;
            this.Close();
        }

        private void btn_character2_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = 2;
            this.Close();
        }

        private void btn_character3_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = 3;
            this.Close();
        }

        private void btn_character4_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = 4;
            this.Close();
        }

        private void btn_character5_Click(object sender, EventArgs e)
        {
            frm_main.get_character_slot = 5;
            this.Close();
        }
    }
}
