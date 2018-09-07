﻿//////////////////////////////////////////////////////////////////////////////////////////
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
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_results : Form
    {
        public string[,] character_path = new string[9, 5];
        public string[] team_path = new string[9];
        public string first_place_image;

        public frm_results()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Resize the image to the specified width and height.
        /// </summary>
        /// <param name="image">The image to resize.</param>
        /// <param name="width">The width to resize to.</param>
        /// <param name="height">The height to resize to.</param>
        /// <returns>The resized image.</returns>
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            //destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        public void make_top8(string filename)
        {
            Image top8_bmp = new Bitmap(1667, 2000);
            Graphics drawing = Graphics.FromImage(top8_bmp);
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            Image background = Image.FromFile(txt_template.Text);

            drawing.Clear(Color.White);

            drawing.DrawImage(background, 0, 0, 1667, 2000);
            Image first_place_render = Image.FromFile(first_place_image);
            drawing.DrawImage(first_place_render, 1020 + Int32.Parse(txt_first_addx.Text), 490 + Int32.Parse(txt_first_addy.Text), first_place_render.Width, first_place_render.Height);

            //Draw 1st Characters
            for (int i = 1; i <= nud_characters_1.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[1, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_1.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1144, 680);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1097 + (94 * (i - 1)), 680);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 680);
                        break;
                }
            }

            //Draw 2nd Characters
            for (int i = 1; i <= nud_characters_2.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[2, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_2.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 807);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 807);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 807);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 807);
                        break;
                }
            }

            //Draw 3rd Characters
            for (int i = 1; i <= nud_characters_3.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[3, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_3.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 933);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 933);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 933);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 933);
                        break;
                }
            }

            //Draw 4th Characters
            for (int i = 1; i <= nud_characters_4.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[4, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_4.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 1059);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 1059);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 1059);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 1059);
                        break;
                }
            }

            //Draw 5th Characters
            for (int i = 1; i <= nud_characters_5.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[5, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_5.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 1185);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 1185);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 1185);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 1185);
                        break;
                }
            }

            //Draw 6th Characters
            for (int i = 1; i <= nud_characters_6.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[6, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_6.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 1311);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 1311);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 1311);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 1311);
                        break;
                }
            }

            //Draw 7th Characters
            for (int i = 1; i <= nud_characters_7.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[7, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_7.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 1437);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 1437);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 1437);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 1437);
                        break;
                }
            }

            //Draw 8th Characters
            for (int i = 1; i <= nud_characters_8.Value; i++)
            {
                Image stock_icon = Image.FromFile(character_path[8, i]);
                stock_icon = ResizeImage(stock_icon, 94, 94);

                switch (nud_characters_8.Value)
                {
                    case 1:
                        drawing.DrawImage(stock_icon, 1238, 1563);
                        break;
                    case 2:
                        drawing.DrawImage(stock_icon, 1191 + (94 * (i - 1)), 1563);
                        break;
                    case 3:
                        drawing.DrawImage(stock_icon, 1144 + (94 * (i - 1)), 1563);
                        break;
                    case 4:
                        drawing.DrawImage(stock_icon, 1050 + (94 * (i - 1)), 1563);
                        break;
                }
            }

            int[] image_xscaled = new int[9];
            //Draw 1st Logo
            if (ckb_team_image_1.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[1]);
                image_xscaled[1] = decimal.ToInt32(decimal.Round((team_logo.Width * 132) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[1], 132);
                drawing.DrawImage(team_logo, 400, 622);
                image_xscaled[1] += 10;
            }

            //Draw 2nd logo
            if (ckb_team_image_2.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[2]);
                image_xscaled[2] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[2], 93);
                drawing.DrawImage(team_logo, 400, 807);
                image_xscaled[2] += 10;
            }

            //Draw 3rd Logo
            if (ckb_team_image_3.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[3]);
                image_xscaled[3] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[3], 93);
                drawing.DrawImage(team_logo, 400, 933);
                image_xscaled[3] += 10;
            }

            //Draw 4th Logo
            if (ckb_team_image_4.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[4]);
                image_xscaled[4] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[4], 93);
                drawing.DrawImage(team_logo, 400, 1059);
                image_xscaled[4] += 10;
            }

            //Draw 5th Logo
            if (ckb_team_image_5.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[5]);
                image_xscaled[5] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[5], 93);
                drawing.DrawImage(team_logo, 400, 1185);
                image_xscaled[5] += 10;
            }

            //Draw 6th Logo
            if (ckb_team_image_6.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[6]);
                image_xscaled[6] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[6], 93);
                drawing.DrawImage(team_logo, 400, 1311);
                image_xscaled[6] += 10;
            }

            //Draw 7th Logo
            if (ckb_team_image_7.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[7]);
                image_xscaled[7] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[7], 93);
                drawing.DrawImage(team_logo, 400, 1437);
                image_xscaled[7] += 10;
            }

            //Draw 8th Logo
            if (ckb_team_image_8.Checked == true)
            {
                Image team_logo = Image.FromFile(team_path[8]);
                image_xscaled[8] = decimal.ToInt32(decimal.Round((team_logo.Width * 93) / team_logo.Height));
                team_logo = ResizeImage(team_logo, image_xscaled[8], 93);
                drawing.DrawImage(team_logo, 400, 1563);
                image_xscaled[8] += 10;
            }

            string player_name1 = txt_tag_1.Text.ToUpper();
            string player_name2 = txt_tag_2.Text.ToUpper();
            string player_name3 = txt_tag_3.Text.ToUpper();
            string player_name4 = txt_tag_4.Text.ToUpper();
            string player_name5 = txt_tag_5.Text.ToUpper();
            string player_name6 = txt_tag_6.Text.ToUpper();
            string player_name7 = txt_tag_7.Text.ToUpper();
            string player_name8 = txt_tag_8.Text.ToUpper();

            string bracket_site = txt_bracket_url.Text.ToUpper();
            string stream_site = txt_stream_url .Text.ToUpper();

            Font keepcalm = new Font("Keep Calm Med", 45);
            Font first_place_font = new Font("Built Titling Sb", 110);
            Font player_font = new Font("Built Titling Sb", 70);
            Font entrants_font = new Font("Built Titling Sb", 34);
            Font urls_font = new Font("Built Titling Sb", 40);
            SolidBrush blackBrush = new SolidBrush(Color.Black);
            SolidBrush whiteBrush = new SolidBrush(Color.White);

            StringFormat drawFormat = new StringFormat();
            StringFormat text_right = new StringFormat();
            text_right.Alignment = StringAlignment.Far;
            text_right.LineAlignment = StringAlignment.Far;
            drawFormat.LineAlignment = StringAlignment.Far;

            drawing.DrawString(player_name1, first_place_font, blackBrush, 370 + image_xscaled[1], 797, drawFormat);
            drawing.DrawString(player_name2, player_font, blackBrush, 380 + image_xscaled[2], 923, drawFormat);
            drawing.DrawString(player_name3, player_font, blackBrush, 380 + image_xscaled[3], 1053, drawFormat);
            drawing.DrawString(player_name4, player_font, blackBrush, 380 + image_xscaled[4], 1175, drawFormat);
            drawing.DrawString(player_name5, player_font, blackBrush, 380 + image_xscaled[5], 1301, drawFormat);
            drawing.DrawString(player_name6, player_font, blackBrush, 380 + image_xscaled[6], 1427, drawFormat);
            drawing.DrawString(player_name7, player_font, blackBrush, 380 + image_xscaled[7], 1553, drawFormat);
            drawing.DrawString(player_name8, player_font, blackBrush, 380 + image_xscaled[8], 1679, drawFormat);

            drawing.DrawString(bracket_site, urls_font, whiteBrush, 11, 89, drawFormat);
            drawing.DrawString(stream_site, urls_font, whiteBrush, 1657, 1990, text_right);

            drawing.DrawString(txt_event_number.Text, keepcalm, whiteBrush, 1065, 491, drawFormat);

            drawFormat.Alignment = StringAlignment.Center;

            drawing.DrawString(txt_entrants_number.Text + @" ENTRANTS", entrants_font, whiteBrush, 833, 1774, drawFormat);



            top8_bmp.Save(filename);

            keepcalm.Dispose();
            first_place_font.Dispose();
            player_font.Dispose();
            entrants_font.Dispose();
            urls_font.Dispose();
            blackBrush.Dispose();
            whiteBrush.Dispose();
            top8_bmp.Dispose();

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
            btn_team_browse_1.BackColor = default(Color);
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
                btn_character_1_1.BackColor = default(Color);
                character_path[1, 1] = openFileDialog1.FileName;
                pic_character_1_1.Image = Image.FromFile(character_path[1, 1]);
            }
        }

        private void btn_character_1_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_1_2.BackColor = default(Color);
                character_path[1, 2] = openFileDialog1.FileName;
                pic_character_1_2.Image = Image.FromFile(character_path[1, 2]);
            }
        }

        private void btn_character_1_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_1_3.BackColor = default(Color);
                character_path[1, 3] = openFileDialog1.FileName;
                pic_character_1_3.Image = Image.FromFile(character_path[1, 3]);
            }
        }

        private void btn_team_browse_1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_team_browse_1.BackColor = default(Color);
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
            btn_team_browse_2.BackColor = default(Color);
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
                btn_character_2_1.BackColor = default(Color);
                character_path[2, 1] = openFileDialog1.FileName;
                pic_character_2_1.Image = Image.FromFile(character_path[2, 1]);
            }
        }

        private void btn_character_2_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_2_2.BackColor = default(Color);
                character_path[2, 2] = openFileDialog1.FileName;
                pic_character_2_2.Image = Image.FromFile(character_path[2, 2]);
            }
        }

        private void btn_character_2_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_2_3.BackColor = default(Color);
                character_path[2, 3] = openFileDialog1.FileName;
                pic_character_2_3.Image = Image.FromFile(character_path[2, 3]);
            }
        }

        private void btn_character_2_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_2_4.BackColor = default(Color);
                character_path[2, 4] = openFileDialog1.FileName;
                pic_character_2_4.Image = Image.FromFile(character_path[2, 4]);
            }
        }

        private void btn_team_browse_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_team_browse_2.BackColor = default(Color);
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
                btn_character_3_1.BackColor = default(Color);
                character_path[3, 1] = openFileDialog1.FileName;
                pic_character_3_1.Image = Image.FromFile(character_path[3, 1]);
            }
        }

        private void btn_character_3_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_3_2.BackColor = default(Color);
                character_path[3, 2] = openFileDialog1.FileName;
                pic_character_3_2.Image = Image.FromFile(character_path[3, 2]);
            }
        }

        private void btn_character_3_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_3_3.BackColor = default(Color);
                character_path[3, 3] = openFileDialog1.FileName;
                pic_character_3_3.Image = Image.FromFile(character_path[3, 3]);
            }
        }

        private void btn_character_3_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_3_4.BackColor = default(Color);
                character_path[3, 4] = openFileDialog1.FileName;
                pic_character_3_4.Image = Image.FromFile(character_path[3, 4]);
            }
        }

        private void ckb_team_image_3_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_3.BackColor = default(Color);
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
                btn_team_browse_3.BackColor = default(Color);
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
                btn_character_4_1.BackColor = default(Color);
                character_path[4, 1] = openFileDialog1.FileName;
                pic_character_4_1.Image = Image.FromFile(character_path[4, 1]);
            }
        }

        private void btn_character_4_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_4_2.BackColor = default(Color);
                character_path[4, 2] = openFileDialog1.FileName;
                pic_character_4_2.Image = Image.FromFile(character_path[4, 2]);
            }
        }

        private void btn_character_4_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_4_3.BackColor = default(Color);
                character_path[4, 3] = openFileDialog1.FileName;
                pic_character_4_3.Image = Image.FromFile(character_path[4, 3]);
            }
        }

        private void btn_character_4_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_4_4.BackColor = default(Color);
                character_path[4, 4] = openFileDialog1.FileName;
                pic_character_4_4.Image = Image.FromFile(character_path[4, 4]);
            }
        }

        private void ckb_team_image_4_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_4.BackColor = default(Color);
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
                btn_team_browse_4.BackColor = default(Color);
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
                btn_character_5_1.BackColor = default(Color);
                character_path[5, 1] = openFileDialog1.FileName;
                pic_character_5_1.Image = Image.FromFile(character_path[5, 1]);
            }
        }

        private void btn_character_5_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_5_2.BackColor = default(Color);
                character_path[5, 2] = openFileDialog1.FileName;
                pic_character_5_2.Image = Image.FromFile(character_path[5, 2]);
            }
        }

        private void btn_character_5_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_5_3.BackColor = default(Color);
                character_path[5, 3] = openFileDialog1.FileName;
                pic_character_5_3.Image = Image.FromFile(character_path[5, 3]);
            }
        }

        private void btn_character_5_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_5_4.BackColor = default(Color);
                character_path[5, 4] = openFileDialog1.FileName;
                pic_character_5_4.Image = Image.FromFile(character_path[5, 4]);
            }
        }

        private void btn_team_browse_5_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_team_browse_5.BackColor = default(Color);
                team_path[5] = openFileDialog1.FileName;
                pic_team_image_5.Image = Image.FromFile(team_path[5]);
            }
        }

        private void ckb_team_image_5_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_5.BackColor = default(Color);
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
                btn_character_6_1.BackColor = default(Color);
                character_path[6, 1] = openFileDialog1.FileName;
                pic_character_6_1.Image = Image.FromFile(character_path[6, 1]);
            }
        }

        private void btn_character_6_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_6_2.BackColor = default(Color);
                character_path[6, 2] = openFileDialog1.FileName;
                pic_character_6_2.Image = Image.FromFile(character_path[6, 2]);
            }
        }

        private void btn_character_6_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_6_3.BackColor = default(Color);
                character_path[6, 3] = openFileDialog1.FileName;
                pic_character_6_3.Image = Image.FromFile(character_path[6, 3]);
            }
        }

        private void btn_character_6_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_6_4.BackColor = default(Color);
                character_path[6, 4] = openFileDialog1.FileName;
                pic_character_6_4.Image = Image.FromFile(character_path[6, 4]);
            }
        }

        private void ckb_team_image_6_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_6.BackColor = default(Color);
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
                btn_team_browse_6.BackColor = default(Color);
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
                btn_character_7_1.BackColor = default(Color);
                character_path[7, 1] = openFileDialog1.FileName;
                pic_character_7_1.Image = Image.FromFile(character_path[7, 1]);
            }
        }

        private void btn_character_7_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_7_2.BackColor = default(Color);
                character_path[7, 2] = openFileDialog1.FileName;
                pic_character_7_2.Image = Image.FromFile(character_path[7, 2]);
            }
        }

        private void btn_character_7_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_7_3.BackColor = default(Color);
                character_path[7, 3] = openFileDialog1.FileName;
                pic_character_7_3.Image = Image.FromFile(character_path[7, 3]);
            }
        }

        private void btn_character_7_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_7_4.BackColor = default(Color);
                character_path[7, 4] = openFileDialog1.FileName;
                pic_character_7_4.Image = Image.FromFile(character_path[7, 4]);
            }
        }

        private void ckb_team_image_7_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_7.BackColor = default(Color);
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
                btn_team_browse_7.BackColor = default(Color);
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
                btn_character_8_1.BackColor = default(Color);
                character_path[8, 1] = openFileDialog1.FileName;
                pic_character_8_1.Image = Image.FromFile(character_path[8, 1]);
            }
        }

        private void btn_character_8_2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_8_2.BackColor = default(Color);
                character_path[8, 2] = openFileDialog1.FileName;
                pic_character_8_2.Image = Image.FromFile(character_path[8, 2]);
            }
        }

        private void btn_character_8_3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_8_3.BackColor = default(Color);
                character_path[8, 3] = openFileDialog1.FileName;
                pic_character_8_3.Image = Image.FromFile(character_path[8, 3]);
            }
        }

        private void btn_character_8_4_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_character_8_4.BackColor = default(Color);
                character_path[8, 4] = openFileDialog1.FileName;
                pic_character_8_4.Image = Image.FromFile(character_path[8, 4]);
            }
        }

        private void ckb_team_image_8_CheckedChanged(object sender, EventArgs e)
        {
            btn_team_browse_8.BackColor = default(Color);
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
                btn_team_browse_8.BackColor = default(Color);
                team_path[8] = openFileDialog1.FileName;
                pic_team_image_8.Image = Image.FromFile(team_path[8]);
            }
        }

        private void btn_template_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                txt_template.Text = openFileDialog2.FileName;
                if (txt_template.Text != @"")
                {
                    if (File.Exists(txt_template.Text))
                    {
                        txt_template.BackColor = Color.White;
                        btn_firstplace_browse.Enabled = true;
                        if (first_place_image != @"")
                        {
                            if (File.Exists(first_place_image))
                            {
                                txt_first_addx.Enabled = true;
                                txt_first_addy.Enabled = true;
                            }
                        }
                    }
                    else
                    {
                        txt_template.BackColor = Color.Red;
                        btn_firstplace_browse.Enabled = false;
                        txt_first_addx.Enabled = false;
                        txt_first_addy.Enabled = false;
                    }
                }
            }
        }

        private void txt_first_addx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void txt_first_addy_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) &&
                (e.KeyChar != '-'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '-') && ((sender as TextBox).Text.IndexOf('-') > -1))
            {
                e.Handled = true;
            }
        }

        private void btn_firstplace_browse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                btn_firstplace_browse.BackColor = default(Color);
                first_place_image = openFileDialog1.FileName;
                if (first_place_image != @"")
                {
                    if (File.Exists(first_place_image))
                    {
                        txt_first_addx.Enabled = true;
                        txt_first_addy.Enabled = true;
                        if (txt_first_addx.Text != "" && txt_first_addy.Text != "" && txt_first_addx.Text != "-" && txt_first_addy.Text != "-")
                        {
                            Image firstplace_image = new Bitmap(485, 310);
                            Graphics drawing = Graphics.FromImage(firstplace_image);
                            drawing.InterpolationMode = InterpolationMode.High;
                            drawing.SmoothingMode = SmoothingMode.HighQuality;
                            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                            drawing.CompositingQuality = CompositingQuality.HighQuality;

                            Image background = Image.FromFile(txt_template.Text);
                            Image character_render = Image.FromFile(first_place_image);
                            drawing.Clear(Color.White);
                            drawing.DrawImage(background, new Rectangle(0, 0, 485, 310), new Rectangle(1020, 490, 485, 310), GraphicsUnit.Pixel);

                            drawing.DrawImage(character_render, 0 + Int32.Parse(txt_first_addx.Text), 0 + Int32.Parse(txt_first_addy.Text), character_render.Width, character_render.Height);
                            pic_firstplace.Image = firstplace_image;
                        }
                    }
                    else
                    {
                        txt_first_addx.Enabled = false;
                        txt_first_addy.Enabled = false;
                    }
                }
            }
        }

        private void txt_first_addx_TextChanged(object sender, EventArgs e)
        {
            if (txt_first_addx.Text != "" && txt_first_addy.Text != "" && txt_first_addx.Text != "-" && txt_first_addy.Text != "-")
            {
                Image firstplace_image = new Bitmap(485, 310);
                Graphics drawing = Graphics.FromImage(firstplace_image);
                drawing.InterpolationMode = InterpolationMode.High;
                drawing.SmoothingMode = SmoothingMode.HighQuality;
                drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                drawing.CompositingQuality = CompositingQuality.HighQuality;

                Image background = Image.FromFile(txt_template.Text);
                Image character_render = Image.FromFile(first_place_image);
                drawing.Clear(Color.White);
                drawing.DrawImage(background, new Rectangle(0, 0, 485, 310), new Rectangle(1020, 490, 485, 310), GraphicsUnit.Pixel);
                drawing.DrawImage(character_render, 0 + Int32.Parse(txt_first_addx.Text), 0 + Int32.Parse(txt_first_addy.Text), character_render.Width, character_render.Height);
                pic_firstplace.Image = firstplace_image;
            }
        }

        private void txt_first_addy_TextChanged(object sender, EventArgs e)
        {
            if (txt_first_addx.Text != "" && txt_first_addy.Text != "" && txt_first_addx.Text != "-" && txt_first_addy.Text != "-")
            {
                Image firstplace_image = new Bitmap(485, 310);
                Graphics drawing = Graphics.FromImage(firstplace_image);
                drawing.InterpolationMode = InterpolationMode.High;
                drawing.SmoothingMode = SmoothingMode.HighQuality;
                drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                drawing.CompositingQuality = CompositingQuality.HighQuality;

                Image background = Image.FromFile(txt_template.Text);
                Image character_render = Image.FromFile(first_place_image);
                drawing.Clear(Color.White);
                drawing.DrawImage(background, new Rectangle(0, 0, 485, 310), new Rectangle(1020, 490, 485, 310), GraphicsUnit.Pixel);
                drawing.DrawImage(character_render, 0 + Int32.Parse(txt_first_addx.Text), 0 + Int32.Parse(txt_first_addy.Text), character_render.Width, character_render.Height);
                pic_firstplace.Image = firstplace_image;
            }
        }

        private void txt_template_TextChanged(object sender, EventArgs e)
        {
            if (txt_template.Text != @"")
            {
                if (File.Exists(txt_template.Text))
                {
                    txt_template.BackColor = Color.White;
                    btn_firstplace_browse.Enabled = true;
                    if (first_place_image != @"")
                    {
                        if (File.Exists(first_place_image))
                        {
                            txt_first_addx.Enabled = true;
                            txt_first_addy.Enabled = true;
                        }
                    }
                }
                else
                {
                    txt_template.BackColor = Color.Red;
                    btn_firstplace_browse.Enabled = false;
                    txt_first_addx.Enabled = false;
                    txt_first_addy.Enabled = false;
                }
            }
        }

        private void btn_save_file_Click(object sender, EventArgs e)
        {
            bool save_top8 = true;
            if (txt_event_number.Text == "")
            {
                txt_event_number.BackColor = Color.Red;
                save_top8 = false;
            }
            if (txt_entrants_number.Text == "")
            {
                txt_entrants_number.BackColor = Color.Red;
                save_top8 = false;
            }
            if (!File.Exists(txt_template.Text))
            {
                txt_template.BackColor = Color.Red;
                save_top8 = false;
            }

            if (ckb_team_image_8.Checked == true && !File.Exists(team_path[8]))
            {
                btn_team_browse_8.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (txt_tag_8.Text == "")
            {
                txt_tag_8.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (!File.Exists(character_path[8, 1]))
            {
                btn_character_8_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (!File.Exists(character_path[8, 2]) && nud_characters_8.Value > 1)
            {
                btn_character_8_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (!File.Exists(character_path[8, 3]) && nud_characters_8.Value > 2)
            {
                btn_character_8_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (!File.Exists(character_path[8, 4]) && nud_characters_8.Value > 3)
            {
                btn_character_8_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 7;
            }
            if (ckb_team_image_7.Checked == true && !File.Exists(team_path[7]))
            {
                btn_team_browse_7.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (txt_tag_7.Text == "")
            {
                txt_tag_7.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (!File.Exists(character_path[7, 1]))
            {
                btn_character_7_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (!File.Exists(character_path[7, 2]) && nud_characters_7.Value > 1)
            {
                btn_character_7_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (!File.Exists(character_path[7, 3]) && nud_characters_7.Value > 2)
            {
                btn_character_7_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (!File.Exists(character_path[7, 4]) && nud_characters_7.Value > 3)
            {
                btn_character_7_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 6;
            }
            if (ckb_team_image_6.Checked == true && !File.Exists(team_path[6]))
            {
                btn_team_browse_6.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (txt_tag_6.Text == "")
            {
                txt_tag_6.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (!File.Exists(character_path[6, 1]))
            {
                btn_character_6_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (!File.Exists(character_path[6, 2]) && nud_characters_6.Value > 1)
            {
                btn_character_6_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (!File.Exists(character_path[6, 3]) && nud_characters_6.Value > 2)
            {
                btn_character_6_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (!File.Exists(character_path[6, 4]) && nud_characters_6.Value > 3)
            {
                btn_character_6_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 5;
            }
            if (ckb_team_image_5.Checked == true && !File.Exists(team_path[5]))
            {
                btn_team_browse_5.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (txt_tag_5.Text == "")
            {
                txt_tag_5.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (!File.Exists(character_path[5, 1]))
            {
                btn_character_5_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (!File.Exists(character_path[5, 2]) && nud_characters_5.Value > 1)
            {
                btn_character_5_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (!File.Exists(character_path[5, 3]) && nud_characters_5.Value > 2)
            {
                btn_character_5_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (!File.Exists(character_path[5, 4]) && nud_characters_5.Value > 3)
            {
                btn_character_5_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 4;
            }
            if (ckb_team_image_4.Checked == true && !File.Exists(team_path[4]))
            {
                btn_team_browse_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (txt_tag_4.Text == "")
            {
                txt_tag_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (!File.Exists(character_path[4, 1]))
            {
                btn_character_4_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (!File.Exists(character_path[4, 2]) && nud_characters_4.Value > 1)
            {
                btn_character_4_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (!File.Exists(character_path[4, 3]) && nud_characters_4.Value > 2)
            {
                btn_character_4_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (!File.Exists(character_path[4, 4]) && nud_characters_4.Value > 3)
            {
                btn_character_4_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 3;
            }
            if (ckb_team_image_3.Checked == true && !File.Exists(team_path[3]))
            {
                btn_team_browse_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (txt_tag_3.Text == "")
            {
                txt_tag_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (!File.Exists(character_path[3, 1]))
            {
                btn_character_3_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (!File.Exists(character_path[3, 2]) && nud_characters_3.Value > 1)
            {
                btn_character_3_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (!File.Exists(character_path[3, 3]) && nud_characters_3.Value > 2)
            {
                btn_character_3_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (!File.Exists(character_path[3, 4]) && nud_characters_3.Value > 3)
            {
                btn_character_3_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 2;
            }
            if (ckb_team_image_2.Checked == true && !File.Exists(team_path[2]))
            {
                btn_team_browse_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (txt_tag_2.Text == "")
            {
                txt_tag_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (!File.Exists(character_path[2, 1]))
            {
                btn_character_2_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (!File.Exists(character_path[2, 2]) && nud_characters_2.Value > 1)
            {
                btn_character_2_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (!File.Exists(character_path[2, 3]) && nud_characters_2.Value > 2)
            {
                btn_character_2_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (!File.Exists(character_path[2, 4]) && nud_characters_2.Value > 3)
            {
                btn_character_2_4.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 1;
            }
            if (ckb_team_image_1.Checked == true && !File.Exists(team_path[1]))
            {
                btn_team_browse_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 0;
            }
            if (txt_tag_1.Text == "")
            {
                txt_tag_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 0;
            }
            if (!File.Exists(character_path[1, 1]) && nud_characters_1.Value > 0)
            {
                btn_character_1_1.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 0;
            }
            if (!File.Exists(character_path[1, 2]) && nud_characters_1.Value > 1)
            {
                btn_character_1_2.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 0;
            }
            if (!File.Exists(character_path[1, 3]) && nud_characters_1.Value > 2)
            {
                btn_character_1_3.BackColor = Color.Red;
                save_top8 = false;
                tabControl1.SelectedIndex = 0;
            }



            if (!File.Exists(first_place_image))
            {
                btn_firstplace_browse.BackColor = Color.Red;
                save_top8 = false;
            }
            if (save_top8 == true)
            {
                saveFileDialog1.FileName = @"Tripoint Wii U " + txt_event_number.Text;
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    make_top8(saveFileDialog1.FileName);
                }
            }
            else
            {
                SystemSounds.Asterisk.Play();
            }
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {

            switch(tabControl1.SelectedIndex)
            {
                case 0:
                    txt_tag_1.Select();
                    txt_tag_1.Focus();
                    break;
                case 1:
                    txt_tag_2.Select();
                    txt_tag_2.Focus();
                    break;
                case 2:
                    txt_tag_3.Select();
                    txt_tag_3.Focus();
                    break;
                case 3:
                    txt_tag_4.Select();
                    txt_tag_4.Focus();
                    break;
                case 4:
                    txt_tag_5.Select();
                    txt_tag_5.Focus();
                    break;
                case 5:
                    txt_tag_6.Select();
                    txt_tag_6.Focus();
                    break;
                case 6:
                    txt_tag_7.Select();
                    txt_tag_7.Focus();
                    break;
                case 7:
                    txt_tag_8.Select();
                    txt_tag_8.Focus();
                    break;
            }
        }

        private void txt_first_addx_Leave(object sender, EventArgs e)
        {
            if (txt_first_addx.Text == @"")
            {
                txt_first_addx.Text = "0";
            }
            if (txt_first_addx.Text == @"-")
            {
                txt_first_addx.Text = "0";
            }
        }

        private void txt_first_addy_Leave(object sender, EventArgs e)
        {
            if (txt_first_addy.Text == @"")
            {
                txt_first_addy.Text = "0";
            }
            if (txt_first_addy.Text == @"-")
            {
                txt_first_addy.Text = "0";
            }
        }

        private void txt_event_number_TextChanged(object sender, EventArgs e)
        {
            if (txt_event_number.Text == "")
            {
                txt_event_number.BackColor = Color.Red;
            }
            else
            {
                txt_event_number.BackColor = Color.White;
            }
        }

        private void txt_entrants_number_TextChanged(object sender, EventArgs e)
        {
            if (txt_entrants_number.Text == "")
            {
                txt_entrants_number.BackColor = Color.Red;
            }
            else
            {
                txt_entrants_number.BackColor = Color.White;
            }
        }

        private void txt_tag_1_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_1.Text == "")
            {
                txt_tag_1.BackColor = Color.Red;
            }
            else
            {
                txt_tag_1.BackColor = Color.White;
            }
        }

        private void txt_tag_2_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_2.Text == "")
            {
                txt_tag_2.BackColor = Color.Red;
            }
            else
            {
                txt_tag_2.BackColor = Color.White;
            }
        }

        private void txt_tag_3_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_3.Text == "")
            {
                txt_tag_3.BackColor = Color.Red;
            }
            else
            {
                txt_tag_3.BackColor = Color.White;
            }
        }

        private void txt_tag_4_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_4.Text == "")
            {
                txt_tag_4.BackColor = Color.Red;
            }
            else
            {
                txt_tag_4.BackColor = Color.White;
            }
        }

        private void txt_tag_5_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_5.Text == "")
            {
                txt_tag_5.BackColor = Color.Red;
            }
            else
            {
                txt_tag_5.BackColor = Color.White;
            }
        }

        private void txt_tag_6_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_6.Text == "")
            {
                txt_tag_6.BackColor = Color.Red;
            }
            else
            {
                txt_tag_6.BackColor = Color.White;
            }
        }

        private void txt_tag_7_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_7.Text == "")
            {
                txt_tag_7.BackColor = Color.Red;
            }
            else
            {
                txt_tag_7.BackColor = Color.White;
            }
        }

        private void txt_tag_8_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag_8.Text == "")
            {
                txt_tag_8.BackColor = Color.Red;
            }
            else
            {
                txt_tag_8.BackColor = Color.White;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Reset all fields?", "Warning", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }
            txt_bracket_url.Text = "";
            txt_entrants_number.Text = "";
            txt_event_number.Text = "";
            txt_first_addx.Text = "";
            txt_first_addy.Text = "";
            txt_stream_url.Text = "";
            txt_tag_1.Text = "";
            txt_tag_2.Text = "";
            txt_tag_3.Text = "";
            txt_tag_4.Text = "";
            txt_tag_5.Text = "";
            txt_tag_6.Text = "";
            txt_tag_7.Text = "";
            txt_tag_8.Text = "";
            txt_template.Text = "";
            pic_character_1_1.Image = null;
            pic_character_1_2.Image = null;
            pic_character_1_3.Image = null;
            pic_character_2_1.Image = null;
            pic_character_2_1.Image = null;
            pic_character_2_2.Image = null;
            pic_character_2_3.Image = null;
            pic_character_2_4.Image = null;
            pic_character_3_1.Image = null;
            pic_character_3_2.Image = null;
            pic_character_3_3.Image = null;
            pic_character_3_4.Image = null;
            pic_character_4_1.Image = null;
            pic_character_4_2.Image = null;
            pic_character_4_3.Image = null;
            pic_character_4_4.Image = null;
            pic_character_5_1.Image = null;
            pic_character_5_2.Image = null;
            pic_character_5_3.Image = null;
            pic_character_5_4.Image = null;
            pic_character_6_1.Image = null;
            pic_character_6_2.Image = null;
            pic_character_6_3.Image = null;
            pic_character_6_4.Image = null;
            pic_character_7_1.Image = null;
            pic_character_7_2.Image = null;
            pic_character_7_3.Image = null;
            pic_character_7_4.Image = null;
            pic_character_8_1.Image = null;
            pic_character_8_2.Image = null;
            pic_character_8_3.Image = null;
            pic_character_8_4.Image = null;
            pic_firstplace.Image = null;
            pic_team_image_1.Image = null;
            pic_team_image_2.Image = null;
            pic_team_image_3.Image = null;
            pic_team_image_4.Image = null;
            pic_team_image_5.Image = null;
            pic_team_image_6.Image = null;
            pic_team_image_7.Image = null;
            pic_team_image_8.Image = null;
            ckb_team_image_1.Checked = false;
            ckb_team_image_2.Checked = false;
            ckb_team_image_3.Checked = false;
            ckb_team_image_4.Checked = false;
            ckb_team_image_5.Checked = false;
            ckb_team_image_6.Checked = false;
            ckb_team_image_7.Checked = false;
            ckb_team_image_8.Checked = false;
            nud_characters_1.Value = 0;
            nud_characters_2.Value = 1;
            nud_characters_3.Value = 1;
            nud_characters_4.Value = 1;
            nud_characters_5.Value = 1;
            nud_characters_6.Value = 1;
            nud_characters_7.Value = 1;
            nud_characters_8.Value = 1;

            txt_first_addx.Enabled = false;
            txt_first_addy.Enabled = false;
            btn_firstplace_browse.Enabled = false;
        }
    }
}
