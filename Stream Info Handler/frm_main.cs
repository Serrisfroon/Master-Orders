//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only
//Copyright 2018, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_main : Form
    {
        public string image_directory1 = Directory.GetCurrentDirectory();
        public string image_directory2 = Directory.GetCurrentDirectory();
        List<string> playlist_items = new List<string>();
        List<string> playlist_names = new List<string>();

        public frm_main()
        {
            InitializeComponent();

            if(!Directory.Exists(@"C:\Users\Public\Stream Info Handler\"))
            {
                Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler\");
            }
            //////////////Set tooltips

            //Set tooltips for the In-Game Display tab
            ttp_tooltip.SetToolTip(btn_character1,  "Select the character and color for Player 1.");
            ttp_tooltip.SetToolTip(btn_character2,  "Select the character and color for Player 2.");
            ttp_tooltip.SetToolTip(txt_name1,       "Set the name/tag for Player 1.");
            ttp_tooltip.SetToolTip(txt_name2,       "Set the name/tag for Player 2.");
            ttp_tooltip.SetToolTip(txt_alt1,        "Set the alt text, such as a twitter handle, \nfor Player 1.");
            ttp_tooltip.SetToolTip(txt_alt2,        "Set the alt text, such as a twitter handle, \nfor Player 2.");

            ttp_tooltip.SetToolTip(btn_swap,        "Switch the player information between Player 1 \nand Player 2.");
            ttp_tooltip.SetToolTip(btn_thumbnail,   "Create a thumbnail image based on the current \nplayer and tournament information.");
            ttp_tooltip.SetToolTip(btn_reset,       "Reset the player information back to default \nvalues. Click this to start a new match.");
                
            ttp_tooltip.SetToolTip(cbx_round,       "Set the current round in bracket. The round will \n" +
                                                    "determine the number of game wins needed to \n" +
                                                    "take a set. This can be changed on the Settings tab.");
            ttp_tooltip.SetToolTip(txt_bracket,     "Set the link to the bracket to be displayed.");
            ttp_tooltip.SetToolTip(txt_tournament,  "Set the name of the tournament. Used in the \nname of YouTube uploads.");

            //Set tooltips for the Commentators tab
            ttp_tooltip.SetToolTip(txt_tag1,            "Set the name/tag for the left commentator.");
            ttp_tooltip.SetToolTip(txt_tag2,            "Set the name/tag for the right commentator.");
            ttp_tooltip.SetToolTip(txt_commentatoralt1, "Set the alt text, such as a twitter handle, \nfor the left commentator.");
            ttp_tooltip.SetToolTip(txt_commentatoralt2, "Set the alt text, such as a twitter handle, \nfor the right commentator.");

            ttp_tooltip.SetToolTip(btn_update_commentators, "Push through any updates made to the \ncommentator information");
            ttp_tooltip.SetToolTip(btn_swapcommentators, "Switch the information between two commentators.");

            //Set tooltips for the Settings tab
            ttp_tooltip.SetToolTip(txt_smashgglink, "Set the URL to the bracket being run.");
            ttp_tooltip.SetToolTip(btn_import_smashgg, "Set the URL to the bracket being run.");

            ttp_tooltip.SetToolTip(txt_roster_directory, "Set directory to the Character Roster.");
            ttp_tooltip.SetToolTip(btn_browse_roster, "Choose the directory to the Character Roster.");

            ttp_tooltip.SetToolTip(txt_thumbnail_directory, "Set the output directory for YouTube \nthumbnail images.");
            ttp_tooltip.SetToolTip(btn_thumb_directory, "Choose the output directory for YouTube \nthumbnail images.");

            ttp_tooltip.SetToolTip(txt_stream_directory, "Set the output directory for the \nstream information files.");
            ttp_tooltip.SetToolTip(btn_output, "Choose the output directory for the \nstream information files.");

            ttp_tooltip.SetToolTip(txt_json,                "Set the path to the .json file used for \n" +
                                                            "YouTube uploads. This file is obtained through \n" +
                                                            "the Google Developer Console.");
            ttp_tooltip.SetToolTip(btn_browse_json,         "Select the .json file used for YouTube uploads. \n" +
                                                            "This file is obtained through the Google \n" +
                                                            "Developer Console.");
            ttp_tooltip.SetToolTip(txt_youtube_username,    "Set the username used by the YouTube API Project \n" +
                                                            "created in the Google Developer Console.");

            ttp_tooltip.SetToolTip(ckb_scoreboad, "Toggle the use of images to display \nthe player scores.");
            ttp_tooltip.SetToolTip(lbl_bestof5, "Choose when Best of 5 sets begin.");
            ttp_tooltip.SetToolTip(cbx_bestof5, "Choose when Best of 5 sets begin.");
            ttp_tooltip.SetToolTip(txt_date, "Change the date displayed in thumbnails \ncreated and YouTube video descriptions.");



            //Set the date box to today's date
            DateTime date = DateTime.Now;
            txt_date.Text = date.ToString("M/dd/yy");
          

            //Initialize Directories and Files
            global_values.output_directory = Directory.GetCurrentDirectory();
            global_values.thumbnail_directory = Directory.GetCurrentDirectory();
            global_values.game_path = Directory.GetCurrentDirectory();
            global_values.json_file = null;
            global_values.youtube_username = "";

            //Check if a Best of 5 file has been saved
            string check_file = @"C:\Users\Public\Stream Info Handler\best of 5.txt";
            if (File.Exists(check_file))
            {
                //Set the username to the information in the file               
                cbx_bestof5.SelectedIndex = Int32.Parse(System.IO.File.ReadAllText(check_file));
            }
            else
            {
                cbx_bestof5.SelectedIndex = 0;
            }

            //Check if a username file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\youtube username.txt";
            if (File.Exists(check_file))
            {
                //Set the username to the information in the file
                global_values.youtube_username = System.IO.File.ReadAllText(check_file);
                txt_youtube_username.Text = global_values.youtube_username;
                if(txt_youtube_username.Text == @"")
                {
                    //Alert the user that no username file exists
                    txt_youtube_username.BackColor = Color.Red;
                    tab_main.SelectedIndex = 2;
                }
            }
            else
            {
                //Alert the user that no username file exists
                txt_youtube_username.BackColor = Color.Red;
                tab_main.SelectedIndex = 2;
            }

            //Check if an output directory file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\output directory.txt";
            if (File.Exists(check_file))
            {
                //Set the output directory to the information in the file
                global_values.output_directory = System.IO.File.ReadAllText(check_file);
                txt_stream_directory.Text = global_values.output_directory;
                //Check if the output directory still exists
                if (!Directory.Exists(global_values.output_directory))
                {
                    //Alert the user that the directory does not exist
                    txt_stream_directory.BackColor = Color.Red;
                    tab_main.SelectedIndex = 2;
                }
            }
            else
            {
                //Alert the user that no directory file exists
                txt_stream_directory.BackColor = Color.Red;
                tab_main.SelectedIndex = 2;
            }

            //Check if a thumbnail directory file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\thumbnail directory.txt";
            if (File.Exists(check_file))
            {
                //Set the thumbnail directory to the information in the file
                global_values.thumbnail_directory = System.IO.File.ReadAllText(check_file);
                txt_thumbnail_directory.Text = global_values.thumbnail_directory;
                //Check if the thumbnail directory still exists
                if (!Directory.Exists(global_values.thumbnail_directory))
                {
                    //Alert the user that the directory does not exist
                    txt_thumbnail_directory.BackColor = Color.Red;
                    tab_main.SelectedIndex = 2;
                }
            }
            else
            {
                //Alert the user that no directory file exists
                txt_thumbnail_directory.BackColor = Color.Red;
                tab_main.SelectedIndex = 2;
            }

            //Check if a json file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\json file.txt";
            if (File.Exists(check_file))
            {
                //Set the json file to the information in the file
                global_values.json_file = System.IO.File.ReadAllText(check_file);
                txt_json.Text = global_values.json_file;
                //Check if the json file still exists
                if (!File.Exists(global_values.json_file))
                {
                    //Alert the user that the directory does not exist
                    txt_json.BackColor = Color.Red;
                    tab_main.SelectedIndex = 2;
                }
            }
            else
            {
                //Alert the user that no directory file exists
                txt_json.BackColor = Color.Red;
                tab_main.SelectedIndex = 2;
            }

            //Check if a roster directory file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\game directory.txt";
            if (File.Exists(check_file))
            {
                //Set the roster directory to the information in the file
                global_values.game_path = System.IO.File.ReadAllText(check_file);
                txt_roster_directory.Text = global_values.game_path;
                //Check if the roster directory still exists
                if (!Directory.Exists(global_values.game_path) || !File.Exists(global_values.game_path + @"\game info.txt"))
                {
                    //Alert the user that the directory does not exist
                    txt_roster_directory.BackColor = Color.Red;
                    tab_main.SelectedIndex = 2;
                }
                else
                {
                    //Update the game info and characters
                    global_values.game_info = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\game info.txt");
                    global_values.characters = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\characters.txt");
                    lbl_gamename.Text = global_values.game_info[0];
                    lbl_characters.Text = global_values.game_info[1] + " Characters";
                }
            }
            else
            {
                //Alert the user that no directory file exists
                txt_stream_directory.BackColor = Color.Red;
                tab_main.SelectedIndex = 2;
            }

            //Check if a image scoring file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\image scoring.txt";
            if (File.Exists(check_file))
            {
                //Set the score1_image1 file to the information in the file
                if(System.IO.File.ReadAllText(check_file) == @"true")
                {
                    ckb_scoreboad.Checked = true;
                    btn_scoreboard.Enabled = true;
                }
            }

            //Check if a score1_image1 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score1_image1.txt";
            if (File.Exists(check_file))
            {
                //Set the score1_image1 file to the information in the file
                global_values.score1_image1 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image1 file still exists
                if (!File.Exists(global_values.score1_image1))
                {
                    global_values.score1_image1 = @"file";
                    if ( btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }

            //Check if a score1_image2 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score1_image2.txt";
            if (File.Exists(check_file))
            {
                //Set the score1_image2 file to the information in the file
                global_values.score1_image2 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image2 file still exists
                if (!File.Exists(global_values.score1_image2))
                {
                    global_values.score1_image2 = @"file";
                    if (btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }

            //Check if a score1_image3 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score1_image3.txt";
            if (File.Exists(check_file))
            {
                //Set the score1_image3 file to the information in the file
                global_values.score1_image3 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image3 file still exists
                if (!File.Exists(global_values.score1_image3))
                {
                    global_values.score1_image3 = @"file";
                    if (btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }

            //Check if a score2_image1 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score2_image1.txt";
            if (File.Exists(check_file))
            {
                //Set the score2_image1 file to the information in the file
                global_values.score2_image1 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image1 file still exists
                if (!File.Exists(global_values.score2_image1))
                {
                    global_values.score2_image1 = @"file";
                    if (btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }

            //Check if a score2_image2 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score2_image2.txt";
            if (File.Exists(check_file))
            {
                //Set the score2_image2 file to the information in the file
                global_values.score2_image2 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image2 file still exists
                if (!File.Exists(global_values.score2_image2))
                {
                    global_values.score2_image2 = @"file";
                    if (btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }

            //Check if a score2_image3 file has been saved
            check_file = @"C:\Users\Public\Stream Info Handler\score2_image3.txt";
            if (File.Exists(check_file))
            {
                //Set the score2_image3 file to the information in the file
                global_values.score2_image3 = System.IO.File.ReadAllText(check_file);
                //Check if the score1_image3 file still exists
                if (!File.Exists(global_values.score2_image3))
                {
                    global_values.score2_image3 = @"file";
                    if (btn_scoreboard.Enabled == true)
                    {
                        //Alert the user that the file does not exist
                        btn_scoreboard.BackColor = Color.Red;
                        tab_main.SelectedIndex = 2;
                    }
                }
            }
        }

        public void enable_button()
        {
            btn_update.Enabled = true;
        }

        public string create_thumbnail()
        {
            Image thumbnail_bmp = new Bitmap(1920, 1080);
            Graphics drawing = Graphics.FromImage(thumbnail_bmp);
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            Image background = Image.FromFile(global_values.game_path + @"\thumbnail_background.jpg");
            Image foreground = Image.FromFile(global_values.game_path + @"\thumbnail_overlay.png");

            Image left_character = Image.FromFile(image_directory1 + @"\left.png");
            Image right_character = Image.FromFile(image_directory2 + @"\right.png");

            drawing.Clear(Color.White);

            drawing.DrawImage(background, 0, 0, 1920, 1080);

            drawing.DrawImage(left_character, 0, 0, 1920, 1080);
            drawing.DrawImage(right_character, 0, 0, 1920, 1080);

            drawing.DrawImage(foreground, 0, 0, 1920, 1080);

            string player_name1 = txt_name1.Text.ToUpper();
            string player_name2 = txt_name2.Text.ToUpper();
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

        public void update_picture(string image_location)
        {
            switch(global_values.player_number)
            {
                case 1:
                    pic_character1.Image = Image.FromFile(image_location + @"stamp.png");
                    break;
                case 2:
                    pic_character2.Image = Image.FromFile(image_location + @"stamp.png");
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string hold_alt = txt_alt1.Text;
            string hold_name = txt_name1.Text;
            Image hold_image = pic_character1.Image;
            decimal hold_score = nud_score1.Value;
            string hold_character = lbl_character1.Text;
            string hold_directory = image_directory1;

            txt_alt1.Text = txt_alt2.Text;
            txt_name1.Text = txt_name2.Text;
            pic_character1.Image = pic_character2.Image;
            nud_score1.Value = nud_score2.Value;
            lbl_character1.Text = lbl_character2.Text;
            image_directory1 = image_directory2;

            txt_alt2.Text = hold_alt;
            txt_name2.Text = hold_name;
            pic_character2.Image = hold_image;
            nud_score2.Value = hold_score;
            lbl_character2.Text = hold_character;
            image_directory2 = hold_directory;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var myForm = new frm_characters(1);
            Point starting_location = this.Location;
            starting_location = Point.Add(starting_location, new Size(130, 0));
            myForm.Location = starting_location;
            if (myForm.ShowDialog() == DialogResult.OK)
            {
                image_directory1 = myForm.image_location;
                lbl_character1.Text = myForm.character_name;
                pic_character1.Image = Image.FromFile(image_directory1 + @"\stamp.png");
                Image stock_icon1 = Image.FromFile(image_directory1 + @"\stock.png");
                stock_icon1.Save(global_values.output_directory + @"\Stock Icon 1.png", System.Drawing.Imaging.ImageFormat.Png);
            }
        }

        private void btn_import_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.game_path = folderBrowserDialog1.SelectedPath;
                txt_roster_directory.Text = global_values.game_path;


                System.IO.Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler\");
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\game directory.txt", global_values.game_path);
            }
        }

        private void btn_character2_Click(object sender, EventArgs e)
        {
            var myForm = new frm_characters(2);
            Point starting_location = this.Location;
            starting_location = Point.Add(starting_location, new Size(480, 0));
            myForm.Location = starting_location;
            if (myForm.ShowDialog() == DialogResult.OK)
            {
                image_directory2 = myForm.image_location;
                lbl_character2.Text = myForm.character_name;
                pic_character2.Image = Image.FromFile(image_directory2 + @"\stamp.png");
                Image stock_icon2 = Image.FromFile(image_directory2 + @"\stock.png");
                stock_icon2.Save(global_values.output_directory + @"\Stock Icon 2.png", System.Drawing.Imaging.ImageFormat.Png);
            }

        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            btn_update.Image = null;

            switch (btn_update.Text)
            {
                case "Start":
                    nud_score1.Enabled = true;
                    nud_score2.Enabled = true;
                    btn_update.Enabled = false;
                    btn_update.Text = "Update";

                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", txt_name1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", lbl_character1.Text);

                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", txt_name2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", lbl_character2.Text);

                    System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
                    break;
                case "Update":
                    btn_update.Enabled = false;

                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", txt_name1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text1.txt", txt_alt1.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name1.txt", lbl_character1.Text);

                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", txt_name2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\alt text2.txt", txt_alt2.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                    System.IO.File.WriteAllText(global_values.output_directory + @"\character name2.txt", lbl_character2.Text);

                    System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                    System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
                    break;
                case "Upload to YouTube":
                    btn_update.Enabled = false;
                    string thumbnail_image_name = create_thumbnail();

                    var upload_form = new frm_uploading(txt_tournament.Text + @" - " + cbx_round.Text + @" - " + txt_name1.Text + @" (" + lbl_character1.Text + @") Vs. " + txt_name2.Text + @" (" + lbl_character2.Text + @")",
                        txt_tournament.Text + @" | " + txt_date.Text + "\r\nRomeoville, Illinois \r\nOrganized and streamed by UGS Gaming \r\nWatch live at https://www.twitch.tv/ugsgaming",
                        global_values.thumbnail_directory + @"\" + thumbnail_image_name);
                    upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                    upload_form.Show();
                    break;
            }
        }

        void upload_form_enable_button_event()
        {
            if (btn_update.Text == @"Upload to YouTube")
            {
                btn_update.Enabled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.output_directory = folderBrowserDialog1.SelectedPath;
                txt_stream_directory.Text = global_values.output_directory;
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\output directory.txt", global_values.output_directory);
            }
        }

        private void btn_swapcommentators_Click(object sender, EventArgs e)
        {
            string hold_name = txt_tag1.Text;
            string hold_alt = txt_commentatoralt1.Text;

            txt_tag1.Text = txt_tag2.Text;
            txt_commentatoralt1.Text = txt_commentatoralt2.Text;

            txt_tag2.Text = hold_name;          
            txt_commentatoralt2.Text = hold_alt;

            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name1.txt", txt_tag1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt1.txt", txt_commentatoralt1.Text);

            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name2.txt", txt_tag2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt2.txt", txt_commentatoralt2.Text);
        }

        private void btn_update_commentators_Click(object sender, EventArgs e)
        {
            btn_update_commentators.Enabled = false;
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name1.txt", txt_tag1.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt1.txt", txt_commentatoralt1.Text);

            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator name2.txt", txt_tag2.Text);
            System.IO.File.WriteAllText(global_values.output_directory + @"\commentator alt2.txt", txt_commentatoralt2.Text);
        }

        private void btn_thumbnail_Click(object sender, EventArgs e)
        {
            string thumbnail_image_name = create_thumbnail();

        }

        private void btn_thumb_directory_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                global_values.thumbnail_directory = folderBrowserDialog1.SelectedPath;
                txt_thumbnail_directory.Text = global_values.thumbnail_directory;
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\thumbnail directory.txt", global_values.thumbnail_directory);
            }
        }

        private void nud_score1_ValueChanged(object sender, EventArgs e)
        {
            decimal match_point = 2;
            decimal current_point = nud_score1.Value;
            switch (cbx_round.Text)
            {
                case "Grand Finals":
                case "Winners Finals":
                case "Losers Finals":
                    match_point = 3;
                    break;
                case "Losers Semifinals":
                    if (cbx_bestof5.SelectedIndex > 0)
                    {
                        match_point = 3;
                    }
                    break;
                case "Losers Quarters":
                    if (cbx_bestof5.SelectedIndex > 1)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Semifinals":
                case "Losers Top 8":
                    if (cbx_bestof5.SelectedIndex > 2)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 16":
                case "Losers Top 16":
                    if (cbx_bestof5.SelectedIndex > 3)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 24":
                case "Losers Top 24":
                    if (cbx_bestof5.SelectedIndex > 4)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 32":
                case "Losers Top 32":
                    if (cbx_bestof5.SelectedIndex > 5)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 48":
                case "Losers Top 48":
                    if (cbx_bestof5.SelectedIndex > 6)
                    {
                        match_point = 3;
                    }
                    break;
                default:
                    match_point = 2;
                    break;
            }
            if (current_point >= match_point)
            {
                nud_score1.Value = match_point;
            }
            if (global_values.auto_update == true)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\score1.txt", nud_score1.Value.ToString());
                if (ckb_scoreboad.Checked == true)
                {
                    string score_file = global_values.output_directory + @"\score1.png";

                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    switch (nud_score1.Value)
                    {
                        case 0:
                            File.Copy(@"left.png", score_file);
                            break;
                        case 1:
                            File.Copy(global_values.score1_image1, score_file);
                            break;
                        case 2:
                            File.Copy(global_values.score1_image2, score_file);
                            break;
                        case 3:
                            File.Copy(global_values.score1_image3, score_file);
                            break;
                    }
                }
                if (current_point >= match_point)
                {
                    btn_update.Enabled = true;
                    nud_score1.Value = match_point;
                    btn_update.Text = @"Upload to YouTube";
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\red.gif");
                }
            }
            else
            {
                btn_update.Enabled = true;
                btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
            }
        }

        private void nud_score2_ValueChanged(object sender, EventArgs e)
        {
            decimal match_point = 2;
            decimal current_point = nud_score2.Value;
            switch (cbx_round.Text)
            {
                case "Grand Finals":
                case "Winners Finals":
                case "Losers Finals":
                    match_point = 3;
                    break;
                case "Losers Semifinals":
                    if (cbx_bestof5.SelectedIndex > 0)
                    {
                        match_point = 3;
                    }
                    break;
                case "Losers Quarters":
                    if (cbx_bestof5.SelectedIndex > 1)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Semifinals":
                case "Losers Top 8":
                    if (cbx_bestof5.SelectedIndex > 2)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 16":
                case "Losers Top 16":
                    if (cbx_bestof5.SelectedIndex > 3)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 24":
                case "Losers Top 24":
                    if (cbx_bestof5.SelectedIndex > 4)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 32":
                case "Losers Top 32":
                    if (cbx_bestof5.SelectedIndex > 5)
                    {
                        match_point = 3;
                    }
                    break;
                case "Winners Top 48":
                case "Losers Top 48":
                    if (cbx_bestof5.SelectedIndex > 6)
                    {
                        match_point = 3;
                    }
                    break;
                default:
                    match_point = 2;
                    break;
            }
            if (current_point >= match_point)
            {
                nud_score2.Value = match_point;
            }
            if (global_values.auto_update == true)
            {
                System.IO.File.WriteAllText(global_values.output_directory + @"\score2.txt", nud_score2.Value.ToString());
                if (ckb_scoreboad.Checked == true)
                {
                    string score_file = global_values.output_directory + @"\score2.png";

                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    switch (nud_score2.Value)
                    {
                        case 0:
                            File.Copy(@"left.png", score_file);
                            break;
                        case 1:
                            File.Copy(global_values.score2_image1, score_file);
                            break;
                        case 2:
                            File.Copy(global_values.score2_image2, score_file);
                            break;
                        case 3:
                            File.Copy(global_values.score2_image3, score_file);
                            break;
                    }
                }
                if (current_point >= match_point)
                {
                    btn_update.Enabled = true;
                    nud_score2.Value = match_point;
                    btn_update.Text = @"Upload to YouTube";
                    btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\red.gif");
                }
            }
            else
            {
                btn_update.Enabled = true;
                btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
            }
        }

        private void cbx_name2_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch(btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        if (global_values.auto_update == false)
                        {
                            btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        }
                        else
                        {
                            btn_update.Enabled = false;
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name2.txt", txt_name2.Text);
                        }
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void cbx_name1_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        if (global_values.auto_update == false)
                        {
                            btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        }
                        else
                        {
                            btn_update.Enabled = false;
                            System.IO.File.WriteAllText(global_values.output_directory + @"\player name1.txt", txt_name1.Text);
                        }
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void cbx_round_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        if (global_values.auto_update == false)
                        {
                            btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        }
                        else
                        {
                            btn_update.Enabled = false;
                            System.IO.File.WriteAllText(global_values.output_directory + @"\round.txt", cbx_round.Text);
                        }
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void txt_tournament_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        if (global_values.auto_update == false)
                        {
                            btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        }
                        else
                        {
                            btn_update.Enabled = false;
                            System.IO.File.WriteAllText(global_values.output_directory + @"\tournament.txt", txt_tournament.Text);
                        }
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void txt_bracket_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        if (global_values.auto_update == false)
                        {
                            btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        }
                        else
                        {
                            btn_update.Enabled = false;
                            System.IO.File.WriteAllText(global_values.output_directory + @"\bracket url.txt", txt_bracket.Text);
                        }
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (global_values.enable_sheets == false)
            {
                txt_name1.Text = @"";
                txt_name2.Text = @"";

                txt_alt1.Text = @"";
                txt_alt2.Text = @"";

                nud_score1.Value = 0;
                nud_score1.Enabled = false;
                nud_score2.Value = 0;
                nud_score2.Enabled = false;

                btn_update.Text = @"Start";
                btn_update.Enabled = false;
                btn_update.Image = null;

                lbl_character1.Text = @"Character Name";
                lbl_character2.Text = @"Character Name";
                pic_character1.Image = null;
                pic_character2.Image = null;
                image_directory1 = Directory.GetCurrentDirectory();
                image_directory2 = Directory.GetCurrentDirectory();
            }
            else
            {
                import_from_sheets();
            }
        }

        private void cbx_alt1_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void cbx_alt2_TextChanged(object sender, EventArgs e)
        {
            if (txt_name1.Text != @"" && txt_name2.Text != @"" && cbx_round.Text != @"" && txt_bracket.Text != @"" && txt_tournament.Text != @"")
            {
                btn_update.Enabled = true;
                switch (btn_update.Text)
                {
                    case "Start":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\green.gif");
                        break;
                    case "Update":
                        btn_update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\yellow.gif");
                        break;
                }
            }
            else
            {
                btn_update.Enabled = false;
            }
        }

        private void cbx_tag1_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag1.Text != @"" && txt_tag2.Text != @"")
            {

                btn_update_commentators.Enabled = true;
            }
            else
            {
                btn_update_commentators.Enabled = false;
            }
        }

        private void cbx_tag2_TextChanged(object sender, EventArgs e)
        {
            if (txt_tag1.Text != @"" && txt_tag2.Text != @"")
            {

                btn_update_commentators.Enabled = true;
            }
            else
            {
                btn_update_commentators.Enabled = false;
            }
        }

        private void btn_browse_json_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txt_json.Text = openFileDialog1.FileName;
                global_values.json_file = txt_json.Text;
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\json file.txt", global_values.json_file);
            }
        }

        private void txt_json_TextChanged(object sender, EventArgs e)
        {
            if (txt_json.Text != @"")
            {
                if (File.Exists(txt_json.Text))
                {
                    txt_json.BackColor = Color.White;
                    global_values.json_file = txt_json.Text;
                    System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\json file.txt", global_values.json_file);
                }
                else
                {
                    txt_json.BackColor = Color.Red;
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
                    global_values.output_directory = txt_stream_directory.Text;
                    System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\output directory.txt", global_values.output_directory);
                }
                else
                {
                    txt_stream_directory.BackColor = Color.Red;
                }
            }
        }

        private void txt_thumbnail_directory_TextChanged(object sender, EventArgs e)
        {
            if (txt_thumbnail_directory.Text != @"")
            {
                if (Directory.Exists(txt_thumbnail_directory.Text))
                {
                    txt_thumbnail_directory.BackColor = Color.White;
                    global_values.thumbnail_directory = txt_thumbnail_directory.Text;
                    System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\thumbnail directory.txt", global_values.thumbnail_directory);
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

                    global_values.game_path = txt_roster_directory.Text;
                    if (File.Exists(global_values.game_path + @"\game info.txt"))
                    {
                        global_values.game_info = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\game info.txt");
                        lbl_gamename.Text = global_values.game_info[0];
                        lbl_characters.Text = global_values.game_info[1] + " Characters";
                    }
                    else
                    {
                        txt_roster_directory.BackColor = Color.Red;
                    }
                    if (File.Exists(global_values.game_path + @"\characters.txt"))
                    {
                        global_values.characters = System.IO.File.ReadAllLines(txt_roster_directory.Text + @"\characters.txt");
                    }
                    else
                    {
                        txt_roster_directory.BackColor = Color.Red;
                    }

                    System.IO.Directory.CreateDirectory(@"C:\Users\Public\Stream Info Handler\");
                    System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\game directory.txt", global_values.game_path);
                }
                else
                {
                    txt_roster_directory.BackColor = Color.Red;
                }
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            global_values.youtube_username = txt_youtube_username.Text;
            System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\youtube username.txt", global_values.youtube_username);
            if (txt_youtube_username.Text != @"")
            {
                txt_youtube_username.BackColor = Color.White;
            }
            else
            {
                txt_youtube_username.BackColor = Color.Red;
            }
        }

        private void tab_main_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tab_main.SelectedIndex != 2)
            {
                if (txt_thumbnail_directory.BackColor == Color.Red ||
                    txt_roster_directory.BackColor == Color.Red ||
                    txt_stream_directory.BackColor == Color.Red ||
                    txt_youtube_username.BackColor == Color.Red ||
                    txt_json.BackColor == Color.Red ||
                    btn_scoreboard.BackColor == Color.Red)
                {
                    tab_main.SelectedIndex = 2;
                    System.Media.SystemSounds.Asterisk.Play();
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ckb_scoreboad_CheckedChanged(object sender, EventArgs e)
        {
            if(ckb_scoreboad.Checked == true)
            {
                btn_scoreboard.Enabled = true;
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\image scoring.txt", @"true");
            }
            else
            {
                System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\image scoring.txt", @"false");
                btn_scoreboard.Enabled = false;
                btn_scoreboard.BackColor = Color.Transparent;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            System.IO.File.WriteAllText(@"C:\Users\Public\Stream Info Handler\best of 5.txt", cbx_bestof5.SelectedIndex.ToString());
        }

        private void btn_scoreboard_Click(object sender, EventArgs e)
        {
            var score_box = new frm_score_images();
            Point starting_location = this.Location;
            starting_location = Point.Add(starting_location, new Size(250, 0));
            score_box.Location = starting_location;
            score_box.ShowDialog();

                if(File.Exists(global_values.score1_image1) &&
                    File.Exists(global_values.score1_image2) &&
                    File.Exists(global_values.score1_image3) &&
                    File.Exists(global_values.score2_image1) &&
                    File.Exists(global_values.score2_image2) &&
                    File.Exists(global_values.score2_image3) )
                {
                    btn_scoreboard.BackColor = Color.Transparent;
                }
                else
                {
                    btn_scoreboard.BackColor = Color.Red;
                }
           
                
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            var top8 = new frm_results();
            top8.Show();
        }

        private void rdb_automatic_CheckedChanged(object sender, EventArgs e)
        {
            global_values.auto_update = true;
        }

        private void rdb_manual_CheckedChanged(object sender, EventArgs e)
        {
            global_values.auto_update = false;
        }

        private void ckb_playlist_CheckedChanged(object sender, EventArgs e)
        {
            if (ckb_playlist.Checked == true)
            {
                global_values.enable_playlists = true;
                lbx_playlist.Enabled = true;
                try
                {
                    Thread thead = new Thread(() =>
                    {
                        get_playlists().Wait();
                    });
                    thead.IsBackground = true;
                    thead.Start();

                }
                catch (AggregateException ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                global_values.enable_playlists = false;
                lbx_playlist.Enabled = false;
                btn_playlist.Enabled = false;
                txt_playlist.Enabled = false;
            }
        }

        private async Task get_playlists()
        {
            playlist_items = new List<string>();
            playlist_names = new List<string>();
            UserCredential credential;
            using (var stream = new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.Youtubepartner,
                    YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubepartnerChannelAudit,
                    YouTubeService.Scope.YoutubeReadonly  },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // var channelsListRequest = youtubeService.Channels.List("contentDetails");
            var playlistListRequest = youtubeService.Playlists.List("snippet");
            playlistListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var playlistListResponse = await playlistListRequest.ExecuteAsync();

            foreach (var playlist in playlistListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var playlistListId = playlist.Id;
                playlist_items.Add(playlistListId);
                playlist_names.Add(playlist.Snippet.Title);
            }

            update_playlists();
        }

        private async Task add_playlist(string new_playlist)
        {
            UserCredential credential;
            using (var stream = new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                    // user's account, but not other types of account access.
                    new[] { YouTubeService.Scope.Youtube,
                    YouTubeService.Scope.Youtubepartner,
                    YouTubeService.Scope.YoutubeUpload,
                    YouTubeService.Scope.YoutubepartnerChannelAudit,
                    YouTubeService.Scope.YoutubeReadonly  },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(this.GetType().ToString())
                );
            }

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = new_playlist;
            newPlaylist.Snippet.Description = "Tripoint Smash";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";

            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();
        }

        private void btn_playlist_Click(object sender, EventArgs e)
        {
            btn_playlist.Enabled = false;
            txt_playlist.Enabled = false;
            try
            {
                Thread thead = new Thread(() =>
                {
                    add_playlist(txt_playlist.Text).ContinueWith(task => get_playlists());
                });
                thead.IsBackground = true;
                thead.Start();

            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        delegate void update_playlists_callback();

        private void update_playlists()
        {
            lbx_playlist.DataSource = null;
            lbx_playlist.DataSource = playlist_names;
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.lbx_playlist.InvokeRequired)
            {
                update_playlists_callback d = new update_playlists_callback(update_playlists);
                this.Invoke(d, new object[] { });
            }
            else
            {
                this.lbx_playlist.Refresh();
                btn_playlist.Enabled = true;
                txt_playlist.Enabled = true;
            }
            
        }

        private void lbx_playlist_SelectedIndexChanged(object sender, EventArgs e)
        {
            global_values.playlist_id = playlist_items[lbx_playlist.SelectedIndex];
            lbl_playlist_id.Text = @"Playlist ID: " + global_values.playlist_id;
        }

        private void ckb_sheets_CheckedChanged(object sender, EventArgs e)
        {
            if(ckb_sheets.Checked == true)
            {
                txt_sheets.Enabled = true;
                global_values.enable_sheets = true;
            }
            else
            {
                txt_sheets.Enabled = false;
                global_values.enable_sheets = false;
            }
        }

        private void import_from_sheets()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            // Define request parameters.
            String spreadsheetId = txt_sheets.Text;
            String range = "Sheet1!A3:E8";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);

            // Prints the names and majors of students in a sample spreadsheet:
            // https://docs.google.com/spreadsheets/d/1BxiMVs0XRA5nFMdKvBdBZjgmUUqptlbs74OgvE2upms/edit
            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            IList<IList<Object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                string player_tag = values[3][1].ToString();
                if (player_tag != "")
                {
                    txt_name1.Text = player_tag + @" | " + values[0][1].ToString();
                }
                else
                {
                    txt_name1.Text = values[0][1].ToString();
                }
                txt_alt1.Text = values[1][1].ToString();

                player_tag = values[3][3].ToString();
                if (player_tag != "")
                {
                    txt_name2.Text = player_tag + @" | " + values[0][3].ToString();
                }
                else
                {
                    txt_name2.Text = values[0][3].ToString();
                }
                txt_alt2.Text = values[1][3].ToString();
            }
            else
            {
                MessageBox.Show("No data found.");
            }

        }
    }

    public static class global_values
    {
        public static string[] characters;
        public static string[] game_info;
        public static string score1_image1 = @"file";
        public static string score1_image2 = @"file";
        public static string score1_image3 = @"file";
        public static string score2_image1 = @"file";
        public static string score2_image2 = @"file";
        public static string score2_image3 = @"file";
        public static string game_path;
        public static string output_directory;
        public static string thumbnail_directory;
        public static string json_file;
        public static string youtube_username;
        public static bool auto_update = true;
        public static int player_number;
        public static int[] player_image;
        public static bool enable_playlists = false;
        public static string playlist_id;
        public static bool enable_sheets;
    }
}
