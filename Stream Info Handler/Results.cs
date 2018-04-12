using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class Results : Form
    {
        public string[,] character_path = new string[9, 5];
        public string[] team_path = new string[9];

        public Results()
        {
            InitializeComponent();
        }

        public string make_top8()
        {
            Image thumbnail_bmp = new Bitmap(1667, 2000);
            Graphics drawing = Graphics.FromImage(thumbnail_bmp);
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            Image background = Image.FromFile(txt_template.Text);

            drawing.Clear(Color.White);

            drawing.DrawImage(background, 0, 0, 1667, 2000);

            //Draw 1st Characters
            for(int i = 1; i <= nud_characters_1.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[1, i]);

                int stock_width = 94;
                int stock_height = stock_width;

                decimal width_ratio = 94 / stock_icon.Width;
                decimal height_ratio = 94 / stock_icon.Height;

                if(width_ratio < height_ratio)
                {
                    height_ratio = width_ratio;
                }
                else
                {
                    width_ratio = height_ratio;
                }

                stock_width = Convert.ToInt32(stock_icon.Width * width_ratio);
                stock_height = Convert.ToInt32(stock_icon.Height * height_ratio);

                switch (nud_characters_1.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 908, 640, stock_width, stock_height);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, , 640, stock_width, stock_height);
                        break;
                    case 3:

                        break;
                }
                
                
            }


            drawing.DrawImage(left_character, 0, 0, 1920, 1080);
            drawing.DrawImage(right_character, 0, 0, 1920, 1080);

            drawing.DrawImage(foreground, 0, 0, 1920, 1080);

            string player_name1 = cbx_name1.Text.ToUpper();
            string player_name2 = cbx_name2.Text.ToUpper();
            string round_text = cbx_round.Text.ToUpper();

            GraphicsPath draw_date = new GraphicsPath();
            GraphicsPath draw_name1 = new GraphicsPath();
            GraphicsPath draw_name2 = new GraphicsPath();
            GraphicsPath draw_round = new GraphicsPath();
            Brush white_text = new SolidBrush(Color.White);
            Brush black_text = new SolidBrush(Color.Black);
            Pen black_stroke = new Pen(black_text, 14);
            Pen light_stroke = new Pen(black_text, 10);
            StringFormat text_center = new StringFormat();
            FontFamily keepcalm = new FontFamily("Keep Calm Med");
            int font_size = 115;
            Font calmfont = new Font("Keep Calm Med", 110, FontStyle.Regular);
            Size namesize = TextRenderer.MeasureText(player_name1, calmfont);

            text_center.Alignment = StringAlignment.Center;
            text_center.LineAlignment = StringAlignment.Center;

            draw_date.AddString(
                txt_date.Text,                     // text to draw
                keepcalm,                           // or any other font family
                (int)FontStyle.Regular,             // font style (bold, italic, etc.)
                110,                                // em size drawing.DpiY * 120 / 72
                new Point(300, 980),                 // location where to draw text
                text_center);                       // set options here (e.g. center alignment)

            drawing.DrawPath(black_stroke, draw_date);
            drawing.FillPath(white_text, draw_date);

            do
            {
                font_size -= 5;
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);
                namesize = TextRenderer.MeasureText(player_name1, calmfont);
            } while (namesize.Width >= 1100);

            black_stroke = new Pen(black_text, font_size / 11 + 4);

            draw_name1.AddString(
                player_name1,                     // text to draw
                keepcalm,                           // or any other font family
                (int)FontStyle.Regular,             // font style (bold, italic, etc.)
                font_size,                                // em size drawing.DpiY * 120 / 72
                new Point(480, 110),                 // location where to draw text
                text_center);                       // set options here (e.g. center alignment)

            drawing.DrawPath(black_stroke, draw_name1);
            drawing.FillPath(white_text, draw_name1);

            font_size = 115;
            do
            {
                font_size -= 5;
                calmfont = new Font("Keep Calm Med", font_size, FontStyle.Regular);
                namesize = TextRenderer.MeasureText(player_name2, calmfont);
            } while (namesize.Width >= 1100);

            black_stroke = new Pen(black_text, font_size / 11 + 4);

            draw_name2.AddString(
               player_name2,                     // text to draw
               keepcalm,                           // or any other font family
               (int)FontStyle.Regular,             // font style (bold, italic, etc.)
               font_size,                                // em size drawing.DpiY * 120 / 72
               new Point(1440, 110),                 // location where to draw text
               text_center);                       // set options here (e.g. center alignment)



            drawing.DrawPath(black_stroke, draw_name2);
            drawing.FillPath(white_text, draw_name2);

            draw_round.AddString(
               round_text,                     // text to draw
               keepcalm,                           // or any other font family
               (int)FontStyle.Regular,             // font style (bold, italic, etc.)
               60,                                // em size drawing.DpiY * 120 / 72
               new Point(960, 620),                 // location where to draw text
               text_center);                       // set options here (e.g. center alignment)

            drawing.DrawPath(light_stroke, draw_round);
            drawing.FillPath(white_text, draw_round);


            drawing.Save();

            drawing.Dispose();

            DateTime date = DateTime.Now;
            string thumbnail_time = date.ToString("MMddyyyyHHmmss");
            string thumbnail_image_name = txt_tournament.Text + @" " + cbx_round.Text + @" " + lbl_character1.Text + @" Vs " + lbl_character2.Text + @" " + thumbnail_time + @".jpg";
            thumbnail_bmp.Save(global_values.thumbnail_directory + @"\" + thumbnail_image_name, System.Drawing.Imaging.ImageFormat.Jpeg);

            return thumbnail_image_name;
        }

        private void nud_characters_1_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_1.Value)
            {
                case 0:
                    pic_character_1_1.Enabled = false;
                    pic_character_1_1.Visible = false;
                    pic_character_1_2.Enabled = false;
                    pic_character_1_2.Visible = false;
                    pic_character_1_3.Enabled = false;
                    pic_character_1_3.Visible = false;

                    btn_character_1_1.Enabled = false;
                    btn_character_1_1.Visible = false;
                    btn_character_1_2.Enabled = false;
                    btn_character_1_2.Visible = false;
                    btn_character_1_3.Enabled = false;
                    btn_character_1_3.Visible = false;
                    break;
                case 1:
                    pic_character_1_1.Enabled = true;
                    pic_character_1_1.Visible = true;
                    pic_character_1_2.Enabled = false;
                    pic_character_1_2.Visible = false;
                    pic_character_1_3.Enabled = false;
                    pic_character_1_3.Visible = false;

                    btn_character_1_1.Enabled = true;
                    btn_character_1_1.Visible = true;
                    btn_character_1_2.Enabled = false;
                    btn_character_1_2.Visible = false;
                    btn_character_1_3.Enabled = false;
                    btn_character_1_3.Visible = false;
                    break;
                case 2:
                    pic_character_1_1.Enabled = true;
                    pic_character_1_1.Visible = true;
                    pic_character_1_2.Enabled = true;
                    pic_character_1_2.Visible = true;
                    pic_character_1_3.Enabled = false;
                    pic_character_1_3.Visible = false;

                    btn_character_1_1.Enabled = true;
                    btn_character_1_1.Visible = true;
                    btn_character_1_2.Enabled = true;
                    btn_character_1_2.Visible = true;
                    btn_character_1_3.Enabled = false;
                    btn_character_1_3.Visible = false;
                    break;
                case 3:
                    pic_character_1_1.Enabled = true;
                    pic_character_1_1.Visible = true;
                    pic_character_1_2.Enabled = true;
                    pic_character_1_2.Visible = true;
                    pic_character_1_3.Enabled = true;
                    pic_character_1_3.Visible = true;

                    btn_character_1_1.Enabled = true;
                    btn_character_1_1.Visible = true;
                    btn_character_1_2.Enabled = true;
                    btn_character_1_2.Visible = true;
                    btn_character_1_3.Enabled = true;
                    btn_character_1_3.Visible = true;
                    break;
            }
        }

        private void ckb_team_image_1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_1.Checked == true)
            {
                pic_team_image_1.Enabled = true;
                btn_team_browse_1.Enabled = true;
            }
            else
            {
                pic_team_image_1.Enabled = true;
                btn_team_browse_1.Enabled = true;
            }
        }

        private void btn_character_1_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[1, 1] = openFileDialog1.FileName;
                pic_character_1_1.Image = Image.FromFile(character_path[1, 1]);
            }
        }

        private void btn_character_1_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[1, 2] = openFileDialog1.FileName;
                pic_character_1_2.Image = Image.FromFile(character_path[1, 2]);
            }
        }

        private void btn_character_1_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[1, 3] = openFileDialog1.FileName;
                pic_character_1_3.Image = Image.FromFile(character_path[1, 3]);
            }
        }

        private void btn_team_browse_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[1] = openFileDialog1.FileName;
                pic_team_image_1.Image = Image.FromFile(team_path[1]);
            }
        }

        private void nud_characters_2_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_2.Value)
            {
                case 1:
                    pic_character_2_1.Enabled = true;
                    pic_character_2_1.Visible = true;
                    pic_character_2_2.Enabled = false;
                    pic_character_2_2.Visible = false;
                    pic_character_2_3.Enabled = false;
                    pic_character_2_3.Visible = false;
                    pic_character_2_4.Enabled = false;
                    pic_character_2_4.Visible = false;

                    btn_character_2_1.Enabled = true;
                    btn_character_2_1.Visible = true;
                    btn_character_2_2.Enabled = false;
                    btn_character_2_2.Visible = false;
                    btn_character_2_3.Enabled = false;
                    btn_character_2_3.Visible = false;
                    btn_character_2_4.Enabled = false;
                    btn_character_2_4.Visible = false;
                    break;
                case 2:
                    pic_character_2_1.Enabled = true;
                    pic_character_2_1.Visible = true;
                    pic_character_2_2.Enabled = true;
                    pic_character_2_2.Visible = true;
                    pic_character_2_3.Enabled = false;
                    pic_character_2_3.Visible = false;
                    pic_character_2_4.Enabled = false;
                    pic_character_2_4.Visible = false;

                    btn_character_2_1.Enabled = true;
                    btn_character_2_1.Visible = true;
                    btn_character_2_2.Enabled = true;
                    btn_character_2_2.Visible = true;
                    btn_character_2_3.Enabled = false;
                    btn_character_2_3.Visible = false;
                    btn_character_2_4.Enabled = false;
                    btn_character_2_4.Visible = false;
                    break;
                case 3:
                    pic_character_2_1.Enabled = true;
                    pic_character_2_1.Visible = true;
                    pic_character_2_2.Enabled = true;
                    pic_character_2_2.Visible = true;
                    pic_character_2_3.Enabled = true;
                    pic_character_2_3.Visible = true;
                    pic_character_2_4.Enabled = false;
                    pic_character_2_4.Visible = false;

                    btn_character_2_1.Enabled = true;
                    btn_character_2_1.Visible = true;
                    btn_character_2_2.Enabled = true;
                    btn_character_2_2.Visible = true;
                    btn_character_2_3.Enabled = true;
                    btn_character_2_3.Visible = true;
                    btn_character_2_4.Enabled = false;
                    btn_character_2_4.Visible = false;
                    break;
                case 4:
                    pic_character_2_1.Enabled = true;
                    pic_character_2_1.Visible = true;
                    pic_character_2_2.Enabled = true;
                    pic_character_2_2.Visible = true;
                    pic_character_2_3.Enabled = true;
                    pic_character_2_3.Visible = true;
                    pic_character_2_4.Enabled = true;
                    pic_character_2_4.Visible = true;

                    btn_character_2_1.Enabled = true;
                    btn_character_2_1.Visible = true;
                    btn_character_2_2.Enabled = true;
                    btn_character_2_2.Visible = true;
                    btn_character_2_3.Enabled = true;
                    btn_character_2_3.Visible = true;
                    btn_character_2_4.Enabled = true;
                    btn_character_2_4.Visible = true;
                    break;

            }
        }

        private void ckb_team_image_2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_2.Checked == true)
            {
                pic_team_image_2.Enabled = true;
                btn_team_browse_2.Enabled = true;
            }
            else
            {
                pic_team_image_2.Enabled = true;
                btn_team_browse_2.Enabled = true;
            }
        }

        private void btn_character_2_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[2, 1] = openFileDialog1.FileName;
                pic_character_2_1.Image = Image.FromFile(character_path[2, 1]);
            }
        }

        private void btn_character_2_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[2, 2] = openFileDialog1.FileName;
                pic_character_2_2.Image = Image.FromFile(character_path[2, 2]);
            }
        }

        private void btn_character_2_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[2, 3] = openFileDialog1.FileName;
                pic_character_2_3.Image = Image.FromFile(character_path[2, 3]);
            }
        }

        private void btn_character_2_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[2, 4] = openFileDialog1.FileName;
                pic_character_2_4.Image = Image.FromFile(character_path[2, 4]);
            }
        }

        private void btn_team_browse_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[2] = openFileDialog1.FileName;
                pic_team_image_2.Image = Image.FromFile(team_path[2]);
            }
        }

        private void nud_characters_3_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_3.Value)
            {
                case 1:
                    pic_character_3_1.Enabled = true;
                    pic_character_3_1.Visible = true;
                    pic_character_3_2.Enabled = false;
                    pic_character_3_2.Visible = false;
                    pic_character_3_3.Enabled = false;
                    pic_character_3_3.Visible = false;
                    pic_character_3_4.Enabled = false;
                    pic_character_3_4.Visible = false;

                    btn_character_3_1.Enabled = true;
                    btn_character_3_1.Visible = true;
                    btn_character_3_2.Enabled = false;
                    btn_character_3_2.Visible = false;
                    btn_character_3_3.Enabled = false;
                    btn_character_3_3.Visible = false;
                    btn_character_3_4.Enabled = false;
                    btn_character_3_4.Visible = false;
                    break;
                case 2:
                    pic_character_3_1.Enabled = true;
                    pic_character_3_1.Visible = true;
                    pic_character_3_2.Enabled = true;
                    pic_character_3_2.Visible = true;
                    pic_character_3_3.Enabled = false;
                    pic_character_3_3.Visible = false;
                    pic_character_3_4.Enabled = false;
                    pic_character_3_4.Visible = false;

                    btn_character_3_1.Enabled = true;
                    btn_character_3_1.Visible = true;
                    btn_character_3_2.Enabled = true;
                    btn_character_3_2.Visible = true;
                    btn_character_3_3.Enabled = false;
                    btn_character_3_3.Visible = false;
                    btn_character_3_4.Enabled = false;
                    btn_character_3_4.Visible = false;
                    break;
                case 3:
                    pic_character_3_1.Enabled = true;
                    pic_character_3_1.Visible = true;
                    pic_character_3_2.Enabled = true;
                    pic_character_3_2.Visible = true;
                    pic_character_3_3.Enabled = true;
                    pic_character_3_3.Visible = true;
                    pic_character_3_4.Enabled = false;
                    pic_character_3_4.Visible = false;

                    btn_character_3_1.Enabled = true;
                    btn_character_3_1.Visible = true;
                    btn_character_3_2.Enabled = true;
                    btn_character_3_2.Visible = true;
                    btn_character_3_3.Enabled = true;
                    btn_character_3_3.Visible = true;
                    btn_character_3_4.Enabled = false;
                    btn_character_3_4.Visible = false;
                    break;
                case 4:
                    pic_character_3_1.Enabled = true;
                    pic_character_3_1.Visible = true;
                    pic_character_3_2.Enabled = true;
                    pic_character_3_2.Visible = true;
                    pic_character_3_3.Enabled = true;
                    pic_character_3_3.Visible = true;
                    pic_character_3_4.Enabled = true;
                    pic_character_3_4.Visible = true;

                    btn_character_3_1.Enabled = true;
                    btn_character_3_1.Visible = true;
                    btn_character_3_2.Enabled = true;
                    btn_character_3_2.Visible = true;
                    btn_character_3_3.Enabled = true;
                    btn_character_3_3.Visible = true;
                    btn_character_3_4.Enabled = true;
                    btn_character_3_4.Visible = true;
                    break;
            }
        }

        private void btn_character_3_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[3, 1] = openFileDialog1.FileName;
                pic_character_3_1.Image = Image.FromFile(character_path[3, 1]);
            }
        }

        private void btn_character_3_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[3, 2] = openFileDialog1.FileName;
                pic_character_3_2.Image = Image.FromFile(character_path[3, 2]);
            }
        }

        private void btn_character_3_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[3, 3] = openFileDialog1.FileName;
                pic_character_3_3.Image = Image.FromFile(character_path[3, 3]);
            }
        }

        private void btn_character_3_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[3, 4] = openFileDialog1.FileName;
                pic_character_3_4.Image = Image.FromFile(character_path[3, 4]);
            }
        }

        private void ckb_team_image_3_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_3.Checked == true)
            {
                pic_team_image_3.Enabled = true;
                btn_team_browse_3.Enabled = true;
            }
            else
            {
                pic_team_image_3.Enabled = true;
                btn_team_browse_3.Enabled = true;
            }
        }

        private void btn_team_browse_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[3] = openFileDialog1.FileName;
                pic_team_image_3.Image = Image.FromFile(team_path[3]);
            }
        }

        private void nud_characters_4_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_4.Value)
            {
                case 1:
                    pic_character_4_1.Enabled = true;
                    pic_character_4_1.Visible = true;
                    pic_character_4_2.Enabled = false;
                    pic_character_4_2.Visible = false;
                    pic_character_4_3.Enabled = false;
                    pic_character_4_3.Visible = false;
                    pic_character_4_4.Enabled = false;
                    pic_character_4_4.Visible = false;

                    btn_character_4_1.Enabled = true;
                    btn_character_4_1.Visible = true;
                    btn_character_4_2.Enabled = false;
                    btn_character_4_2.Visible = false;
                    btn_character_4_3.Enabled = false;
                    btn_character_4_3.Visible = false;
                    btn_character_4_4.Enabled = false;
                    btn_character_4_4.Visible = false;
                    break;
                case 2:
                    pic_character_4_1.Enabled = true;
                    pic_character_4_1.Visible = true;
                    pic_character_4_2.Enabled = true;
                    pic_character_4_2.Visible = true;
                    pic_character_4_3.Enabled = false;
                    pic_character_4_3.Visible = false;
                    pic_character_4_4.Enabled = false;
                    pic_character_4_4.Visible = false;

                    btn_character_4_1.Enabled = true;
                    btn_character_4_1.Visible = true;
                    btn_character_4_2.Enabled = true;
                    btn_character_4_2.Visible = true;
                    btn_character_4_3.Enabled = false;
                    btn_character_4_3.Visible = false;
                    btn_character_4_4.Enabled = false;
                    btn_character_4_4.Visible = false;
                    break;
                case 3:
                    pic_character_4_1.Enabled = true;
                    pic_character_4_1.Visible = true;
                    pic_character_4_2.Enabled = true;
                    pic_character_4_2.Visible = true;
                    pic_character_4_3.Enabled = true;
                    pic_character_4_3.Visible = true;
                    pic_character_4_4.Enabled = false;
                    pic_character_4_4.Visible = false;

                    btn_character_4_1.Enabled = true;
                    btn_character_4_1.Visible = true;
                    btn_character_4_2.Enabled = true;
                    btn_character_4_2.Visible = true;
                    btn_character_4_3.Enabled = true;
                    btn_character_4_3.Visible = true;
                    btn_character_4_4.Enabled = false;
                    btn_character_4_4.Visible = false;
                    break;
                case 4:
                    pic_character_4_1.Enabled = true;
                    pic_character_4_1.Visible = true;
                    pic_character_4_2.Enabled = true;
                    pic_character_4_2.Visible = true;
                    pic_character_4_3.Enabled = true;
                    pic_character_4_3.Visible = true;
                    pic_character_4_4.Enabled = true;
                    pic_character_4_4.Visible = true;

                    btn_character_4_1.Enabled = true;
                    btn_character_4_1.Visible = true;
                    btn_character_4_2.Enabled = true;
                    btn_character_4_2.Visible = true;
                    btn_character_4_3.Enabled = true;
                    btn_character_4_3.Visible = true;
                    btn_character_4_4.Enabled = true;
                    btn_character_4_4.Visible = true;
                    break;
            }
        }

        private void btn_character_4_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[4, 1] = openFileDialog1.FileName;
                pic_character_4_1.Image = Image.FromFile(character_path[4, 1]);
            }
        }

        private void btn_character_4_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[4, 2] = openFileDialog1.FileName;
                pic_character_4_2.Image = Image.FromFile(character_path[4, 2]);
            }
        }

        private void btn_character_4_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[4, 3] = openFileDialog1.FileName;
                pic_character_4_3.Image = Image.FromFile(character_path[4, 3]);
            }
        }

        private void btn_character_4_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[4, 4] = openFileDialog1.FileName;
                pic_character_4_4.Image = Image.FromFile(character_path[4, 4]);
            }
        }

        private void ckb_team_image_4_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_4.Checked == true)
            {
                pic_team_image_4.Enabled = true;
                btn_team_browse_4.Enabled = true;
            }
            else
            {
                pic_team_image_4.Enabled = true;
                btn_team_browse_4.Enabled = true;
            }
        }

        private void btn_team_browse_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[4] = openFileDialog1.FileName;
                pic_team_image_4.Image = Image.FromFile(team_path[4]);
            }
        }

        private void nud_characters_5_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_5.Value)
            {
                case 1:
                    pic_character_5_1.Enabled = true;
                    pic_character_5_1.Visible = true;
                    pic_character_5_2.Enabled = false;
                    pic_character_5_2.Visible = false;
                    pic_character_5_3.Enabled = false;
                    pic_character_5_3.Visible = false;
                    pic_character_5_4.Enabled = false;
                    pic_character_5_4.Visible = false;

                    btn_character_5_1.Enabled = true;
                    btn_character_5_1.Visible = true;
                    btn_character_5_2.Enabled = false;
                    btn_character_5_2.Visible = false;
                    btn_character_5_3.Enabled = false;
                    btn_character_5_3.Visible = false;
                    btn_character_5_4.Enabled = false;
                    btn_character_5_4.Visible = false;
                    break;
                case 2:
                    pic_character_5_1.Enabled = true;
                    pic_character_5_1.Visible = true;
                    pic_character_5_2.Enabled = true;
                    pic_character_5_2.Visible = true;
                    pic_character_5_3.Enabled = false;
                    pic_character_5_3.Visible = false;
                    pic_character_5_4.Enabled = false;
                    pic_character_5_4.Visible = false;

                    btn_character_5_1.Enabled = true;
                    btn_character_5_1.Visible = true;
                    btn_character_5_2.Enabled = true;
                    btn_character_5_2.Visible = true;
                    btn_character_5_3.Enabled = false;
                    btn_character_5_3.Visible = false;
                    btn_character_5_4.Enabled = false;
                    btn_character_5_4.Visible = false;
                    break;
                case 3:
                    pic_character_5_1.Enabled = true;
                    pic_character_5_1.Visible = true;
                    pic_character_5_2.Enabled = true;
                    pic_character_5_2.Visible = true;
                    pic_character_5_3.Enabled = true;
                    pic_character_5_3.Visible = true;
                    pic_character_5_4.Enabled = false;
                    pic_character_5_4.Visible = false;

                    btn_character_5_1.Enabled = true;
                    btn_character_5_1.Visible = true;
                    btn_character_5_2.Enabled = true;
                    btn_character_5_2.Visible = true;
                    btn_character_5_3.Enabled = true;
                    btn_character_5_3.Visible = true;
                    btn_character_5_4.Enabled = false;
                    btn_character_5_4.Visible = false;
                    break;
                case 4:
                    pic_character_5_1.Enabled = true;
                    pic_character_5_1.Visible = true;
                    pic_character_5_2.Enabled = true;
                    pic_character_5_2.Visible = true;
                    pic_character_5_3.Enabled = true;
                    pic_character_5_3.Visible = true;
                    pic_character_5_4.Enabled = true;
                    pic_character_5_4.Visible = true;

                    btn_character_5_1.Enabled = true;
                    btn_character_5_1.Visible = true;
                    btn_character_5_2.Enabled = true;
                    btn_character_5_2.Visible = true;
                    btn_character_5_3.Enabled = true;
                    btn_character_5_3.Visible = true;
                    btn_character_5_4.Enabled = true;
                    btn_character_5_4.Visible = true;
                    break;
            }
        }

        private void btn_character_5_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[5, 1] = openFileDialog1.FileName;
                pic_character_5_1.Image = Image.FromFile(character_path[5, 1]);
            }
        }

        private void btn_character_5_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[5, 2] = openFileDialog1.FileName;
                pic_character_5_2.Image = Image.FromFile(character_path[5, 2]);
            }
        }

        private void btn_character_5_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[5, 3] = openFileDialog1.FileName;
                pic_character_5_3.Image = Image.FromFile(character_path[5, 3]);
            }
        }

        private void btn_character_5_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[5, 4] = openFileDialog1.FileName;
                pic_character_5_4.Image = Image.FromFile(character_path[5, 4]);
            }
        }

        private void btn_team_browse_5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[5] = openFileDialog1.FileName;
                pic_team_image_5.Image = Image.FromFile(team_path[5]);
            }
        }

        private void ckb_team_image_5_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_5.Checked == true)
            {
                pic_team_image_5.Enabled = true;
                btn_team_browse_5.Enabled = true;
            }
            else
            {
                pic_team_image_5.Enabled = true;
                btn_team_browse_5.Enabled = true;
            }
        }

        private void nud_characters_6_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_6.Value)
            {
                case 1:
                    pic_character_6_1.Enabled = true;
                    pic_character_6_1.Visible = true;
                    pic_character_6_2.Enabled = false;
                    pic_character_6_2.Visible = false;
                    pic_character_6_3.Enabled = false;
                    pic_character_6_3.Visible = false;
                    pic_character_6_4.Enabled = false;
                    pic_character_6_4.Visible = false;

                    btn_character_6_1.Enabled = true;
                    btn_character_6_1.Visible = true;
                    btn_character_6_2.Enabled = false;
                    btn_character_6_2.Visible = false;
                    btn_character_6_3.Enabled = false;
                    btn_character_6_3.Visible = false;
                    btn_character_6_4.Enabled = false;
                    btn_character_6_4.Visible = false;
                    break;
                case 2:
                    pic_character_6_1.Enabled = true;
                    pic_character_6_1.Visible = true;
                    pic_character_6_2.Enabled = true;
                    pic_character_6_2.Visible = true;
                    pic_character_6_3.Enabled = false;
                    pic_character_6_3.Visible = false;
                    pic_character_6_4.Enabled = false;
                    pic_character_6_4.Visible = false;

                    btn_character_6_1.Enabled = true;
                    btn_character_6_1.Visible = true;
                    btn_character_6_2.Enabled = true;
                    btn_character_6_2.Visible = true;
                    btn_character_6_3.Enabled = false;
                    btn_character_6_3.Visible = false;
                    btn_character_6_4.Enabled = false;
                    btn_character_6_4.Visible = false;
                    break;
                case 3:
                    pic_character_6_1.Enabled = true;
                    pic_character_6_1.Visible = true;
                    pic_character_6_2.Enabled = true;
                    pic_character_6_2.Visible = true;
                    pic_character_6_3.Enabled = true;
                    pic_character_6_3.Visible = true;
                    pic_character_6_4.Enabled = false;
                    pic_character_6_4.Visible = false;

                    btn_character_6_1.Enabled = true;
                    btn_character_6_1.Visible = true;
                    btn_character_6_2.Enabled = true;
                    btn_character_6_2.Visible = true;
                    btn_character_6_3.Enabled = true;
                    btn_character_6_3.Visible = true;
                    btn_character_6_4.Enabled = false;
                    btn_character_6_4.Visible = false;
                    break;
                case 4:
                    pic_character_6_1.Enabled = true;
                    pic_character_6_1.Visible = true;
                    pic_character_6_2.Enabled = true;
                    pic_character_6_2.Visible = true;
                    pic_character_6_3.Enabled = true;
                    pic_character_6_3.Visible = true;
                    pic_character_6_4.Enabled = true;
                    pic_character_6_4.Visible = true;

                    btn_character_6_1.Enabled = true;
                    btn_character_6_1.Visible = true;
                    btn_character_6_2.Enabled = true;
                    btn_character_6_2.Visible = true;
                    btn_character_6_3.Enabled = true;
                    btn_character_6_3.Visible = true;
                    btn_character_6_4.Enabled = true;
                    btn_character_6_4.Visible = true;
                    break;
            }
        }

        private void btn_character_6_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[6, 1] = openFileDialog1.FileName;
                pic_character_6_1.Image = Image.FromFile(character_path[6, 1]);
            }
        }

        private void btn_character_6_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[6, 2] = openFileDialog1.FileName;
                pic_character_6_2.Image = Image.FromFile(character_path[6, 2]);
            }
        }

        private void btn_character_6_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[6, 3] = openFileDialog1.FileName;
                pic_character_6_3.Image = Image.FromFile(character_path[6, 3]);
            }
        }

        private void btn_character_6_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[6, 4] = openFileDialog1.FileName;
                pic_character_6_4.Image = Image.FromFile(character_path[6, 4]);
            }
        }

        private void ckb_team_image_6_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_6.Checked == true)
            {
                pic_team_image_6.Enabled = true;
                btn_team_browse_6.Enabled = true;
            }
            else
            {
                pic_team_image_6.Enabled = true;
                btn_team_browse_6.Enabled = true;
            }
        }

        private void btn_team_browse_6_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[6] = openFileDialog1.FileName;
                pic_team_image_6.Image = Image.FromFile(team_path[6]);
            }
        }

        private void nud_characters_7_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_7.Value)
            {
                case 1:
                    pic_character_7_1.Enabled = true;
                    pic_character_7_1.Visible = true;
                    pic_character_7_2.Enabled = false;
                    pic_character_7_2.Visible = false;
                    pic_character_7_3.Enabled = false;
                    pic_character_7_3.Visible = false;
                    pic_character_7_4.Enabled = false;
                    pic_character_7_4.Visible = false;

                    btn_character_7_1.Enabled = true;
                    btn_character_7_1.Visible = true;
                    btn_character_7_2.Enabled = false;
                    btn_character_7_2.Visible = false;
                    btn_character_7_3.Enabled = false;
                    btn_character_7_3.Visible = false;
                    btn_character_7_4.Enabled = false;
                    btn_character_7_4.Visible = false;
                    break;
                case 2:
                    pic_character_7_1.Enabled = true;
                    pic_character_7_1.Visible = true;
                    pic_character_7_2.Enabled = true;
                    pic_character_7_2.Visible = true;
                    pic_character_7_3.Enabled = false;
                    pic_character_7_3.Visible = false;
                    pic_character_7_4.Enabled = false;
                    pic_character_7_4.Visible = false;

                    btn_character_7_1.Enabled = true;
                    btn_character_7_1.Visible = true;
                    btn_character_7_2.Enabled = true;
                    btn_character_7_2.Visible = true;
                    btn_character_7_3.Enabled = false;
                    btn_character_7_3.Visible = false;
                    btn_character_7_4.Enabled = false;
                    btn_character_7_4.Visible = false;
                    break;
                case 3:
                    pic_character_7_1.Enabled = true;
                    pic_character_7_1.Visible = true;
                    pic_character_7_2.Enabled = true;
                    pic_character_7_2.Visible = true;
                    pic_character_7_3.Enabled = true;
                    pic_character_7_3.Visible = true;
                    pic_character_7_4.Enabled = false;
                    pic_character_7_4.Visible = false;

                    btn_character_7_1.Enabled = true;
                    btn_character_7_1.Visible = true;
                    btn_character_7_2.Enabled = true;
                    btn_character_7_2.Visible = true;
                    btn_character_7_3.Enabled = true;
                    btn_character_7_3.Visible = true;
                    btn_character_7_4.Enabled = false;
                    btn_character_7_4.Visible = false;
                    break;
                case 4:
                    pic_character_7_1.Enabled = true;
                    pic_character_7_1.Visible = true;
                    pic_character_7_2.Enabled = true;
                    pic_character_7_2.Visible = true;
                    pic_character_7_3.Enabled = true;
                    pic_character_7_3.Visible = true;
                    pic_character_7_4.Enabled = true;
                    pic_character_7_4.Visible = true;

                    btn_character_7_1.Enabled = true;
                    btn_character_7_1.Visible = true;
                    btn_character_7_2.Enabled = true;
                    btn_character_7_2.Visible = true;
                    btn_character_7_3.Enabled = true;
                    btn_character_7_3.Visible = true;
                    btn_character_7_4.Enabled = true;
                    btn_character_7_4.Visible = true;
                    break;
            }
        }

        private void btn_character_7_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[7, 1] = openFileDialog1.FileName;
                pic_character_7_1.Image = Image.FromFile(character_path[7, 1]);
            }
        }

        private void btn_character_7_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[7, 2] = openFileDialog1.FileName;
                pic_character_7_2.Image = Image.FromFile(character_path[7, 2]);
            }
        }

        private void btn_character_7_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[7, 3] = openFileDialog1.FileName;
                pic_character_7_3.Image = Image.FromFile(character_path[7, 3]);
            }
        }

        private void btn_character_7_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[7, 4] = openFileDialog1.FileName;
                pic_character_7_4.Image = Image.FromFile(character_path[7, 4]);
            }
        }

        private void ckb_team_image_7_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_7.Checked == true)
            {
                pic_team_image_7.Enabled = true;
                btn_team_browse_7.Enabled = true;
            }
            else
            {
                pic_team_image_7.Enabled = true;
                btn_team_browse_7.Enabled = true;
            }
        }

        private void btn_team_browse_7_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[7] = openFileDialog1.FileName;
                pic_team_image_7.Image = Image.FromFile(team_path[7]);
            }
        }

        private void nud_characters_8_ValueChanged(object sender, EventArgs e)
        {
            switch (nud_characters_8.Value)
            {
                case 1:
                    pic_character_8_1.Enabled = true;
                    pic_character_8_1.Visible = true;
                    pic_character_8_2.Enabled = false;
                    pic_character_8_2.Visible = false;
                    pic_character_8_3.Enabled = false;
                    pic_character_8_3.Visible = false;
                    pic_character_8_4.Enabled = false;
                    pic_character_8_4.Visible = false;

                    btn_character_8_1.Enabled = true;
                    btn_character_8_1.Visible = true;
                    btn_character_8_2.Enabled = false;
                    btn_character_8_2.Visible = false;
                    btn_character_8_3.Enabled = false;
                    btn_character_8_3.Visible = false;
                    btn_character_8_4.Enabled = false;
                    btn_character_8_4.Visible = false;
                    break;
                case 2:
                    pic_character_8_1.Enabled = true;
                    pic_character_8_1.Visible = true;
                    pic_character_8_2.Enabled = true;
                    pic_character_8_2.Visible = true;
                    pic_character_8_3.Enabled = false;
                    pic_character_8_3.Visible = false;
                    pic_character_8_4.Enabled = false;
                    pic_character_8_4.Visible = false;

                    btn_character_8_1.Enabled = true;
                    btn_character_8_1.Visible = true;
                    btn_character_8_2.Enabled = true;
                    btn_character_8_2.Visible = true;
                    btn_character_8_3.Enabled = false;
                    btn_character_8_3.Visible = false;
                    btn_character_8_4.Enabled = false;
                    btn_character_8_4.Visible = false;
                    break;
                case 3:
                    pic_character_8_1.Enabled = true;
                    pic_character_8_1.Visible = true;
                    pic_character_8_2.Enabled = true;
                    pic_character_8_2.Visible = true;
                    pic_character_8_3.Enabled = true;
                    pic_character_8_3.Visible = true;
                    pic_character_8_4.Enabled = false;
                    pic_character_8_4.Visible = false;

                    btn_character_8_1.Enabled = true;
                    btn_character_8_1.Visible = true;
                    btn_character_8_2.Enabled = true;
                    btn_character_8_2.Visible = true;
                    btn_character_8_3.Enabled = true;
                    btn_character_8_3.Visible = true;
                    btn_character_8_4.Enabled = false;
                    btn_character_8_4.Visible = false;
                    break;
                case 4:
                    pic_character_8_1.Enabled = true;
                    pic_character_8_1.Visible = true;
                    pic_character_8_2.Enabled = true;
                    pic_character_8_2.Visible = true;
                    pic_character_8_3.Enabled = true;
                    pic_character_8_3.Visible = true;
                    pic_character_8_4.Enabled = true;
                    pic_character_8_4.Visible = true;

                    btn_character_8_1.Enabled = true;
                    btn_character_8_1.Visible = true;
                    btn_character_8_2.Enabled = true;
                    btn_character_8_2.Visible = true;
                    btn_character_8_3.Enabled = true;
                    btn_character_8_3.Visible = true;
                    btn_character_8_4.Enabled = true;
                    btn_character_8_4.Visible = true;
                    break;
            }
        }

        private void btn_character_8_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[8, 1] = openFileDialog1.FileName;
                pic_character_8_1.Image = Image.FromFile(character_path[8, 1]);
            }
        }

        private void btn_character_8_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[8, 2] = openFileDialog1.FileName;
                pic_character_8_2.Image = Image.FromFile(character_path[8, 2]);
            }
        }

        private void btn_character_8_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[8, 3] = openFileDialog1.FileName;
                pic_character_8_3.Image = Image.FromFile(character_path[8, 3]);
            }
        }

        private void btn_character_8_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                character_path[8, 4] = openFileDialog1.FileName;
                pic_character_8_4.Image = Image.FromFile(character_path[8, 4]);
            }
        }

        private void ckb_team_image_8_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_team_image_8.Checked == true)
            {
                pic_team_image_8.Enabled = true;
                btn_team_browse_8.Enabled = true;
            }
            else
            {
                pic_team_image_8.Enabled = true;
                btn_team_browse_8.Enabled = true;
            }
        }

        private void btn_team_browse_8_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                team_path[8] = openFileDialog1.FileName;
                pic_team_image_8.Image = Image.FromFile(team_path[8]);
            }
        }

        private void btn_template_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txt_template.Text = openFileDialog2.FileName;
            }
        }
    }
}
