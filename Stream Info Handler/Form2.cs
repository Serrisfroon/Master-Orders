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
    public partial class Form2 : Form
    {
        private string image_directory;

        public string image_location
        {
            get { return image_directory; }
            set { image_directory = value; }
        }

        public string character_name;

        public Form2(int num)
        {
            InitializeComponent();
            global_values.player_number = num;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int color_choice = cbx_colors.SelectedIndex + 1;
            lbl_color.Text = color_choice.ToString();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbx_colors.Items.Clear();
            string character_path = global_values.game_path + @"\" + lbx_characters.Text;
            int colors_count = Int32.Parse(System.IO.File.ReadAllText(character_path + @"\colors.txt"));
           
            switch(colors_count)
            {
                case 1:
                    Image[] colors1 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors1);
                    break;
                case 2:
                    Image[] colors2 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors2);
                    break;
                case 3:
                    Image[] colors3 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors3);
                    break;
                case 4:
                    Image[] colors4 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors4);
                    break;
                case 5:
                    Image[] colors5 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors5);
                    break;
                case 6:
                    Image[] colors6 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors6);
                    break;
                case 8:
                    Image[] colors8 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors8);
                    break;
                case 16:
                    Image[] colors16 =
                    {
                        Image.FromFile(character_path + @"\1\stamp.png"),
                        Image.FromFile(character_path + @"\2\stamp.png"),
                        Image.FromFile(character_path + @"\3\stamp.png"),
                        Image.FromFile(character_path + @"\4\stamp.png"),
                        Image.FromFile(character_path + @"\5\stamp.png"),
                        Image.FromFile(character_path + @"\6\stamp.png"),
                        Image.FromFile(character_path + @"\7\stamp.png"),
                        Image.FromFile(character_path + @"\8\stamp.png"),
                        Image.FromFile(character_path + @"\9\stamp.png"),
                        Image.FromFile(character_path + @"\10\stamp.png"),
                        Image.FromFile(character_path + @"\11\stamp.png"),
                        Image.FromFile(character_path + @"\12\stamp.png"),
                        Image.FromFile(character_path + @"\13\stamp.png"),
                        Image.FromFile(character_path + @"\14\stamp.png"),
                        Image.FromFile(character_path + @"\15\stamp.png"),
                        Image.FromFile(character_path + @"\16\stamp.png"),
                    };
                    cbx_colors.DisplayImages(colors16);
                    break;
            }
            cbx_colors.SelectedIndex = 0;
            cbx_colors.DropDownHeight = 400;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lbx_characters.BeginUpdate();
            int character_count = Int32.Parse(global_values.game_info[1]);
            for (int x = 0; x < character_count; x++)
            {
                lbx_characters.Items.Add(global_values.characters[x]);
            }
            lbx_characters.EndUpdate();
            lbx_characters.SelectedIndex = 0;
        }

        private void lbl_color_Click(object sender, EventArgs e)
        {

        }

        private void btn_updatecharacter_Click(object sender, EventArgs e)
        {
            image_location = global_values.game_path + @"\" + lbx_characters.Text + @"\" + lbl_color.Text + @"\";
            character_name = lbx_characters.Text;
        }
    }
}
