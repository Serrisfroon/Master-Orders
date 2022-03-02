using System.IO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using Stream_Info_Handler.AppSettings;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    public static class ImageManagement
    {
        public static bool enableRegionImages { get; set; }
        public static bool enableSponsorImages { get; set; }
        public static bool enableImageScoreboard { get; set; }

        public static string[,] scoreboardImages = new string[2, 3] { { "", "", "" }, { "", "", "" } };

        public static ThumbnailConfigurationModel thumbnailConfiguration = new ThumbnailConfigurationModel();

        /// <summary>
        /// Resets all images in the output directory to a blank placeholder image.
        /// </summary>
        /// <param name="directory">The direct to perform the reset in</param>
        public static void ResetAllImages(string directory)
        {
            //Reset all image files
            string[] image_files = { @"\score1.png", @"\score2.png",
                                     @"\stockicon1.png", @"\stockicon2.png", @"\stockicon3.png", @"\stockicon4.png",
                                     @"\sponsor1.png", @"\sponsor2.png", @"\sponsor3.png", @"\sponsor4.png",
                                     @"\region1.png", @"\region2.png", @"\region3.png", @"\region4.png" };
            foreach (string replace_image in image_files)
            {
                if (File.Exists(directory + replace_image))
                {
                    File.Delete(directory + replace_image);
                }
                File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", directory + replace_image);
            }
        }

        /// <summary>
        /// Create a thumbnail image using the information input for the players and tournament
        /// </summary>
        /// <param name="player_count">The number of players in the thumbnail</param>
        /// <param name="player_name1">The first player/team's name(s)</param>
        /// <param name="player_name2">The second player/team's name(s)</param>
        /// <param name="round_text">The round in bracket</param>
        /// <param name="match_date">The date of the match</param>
        /// <param name="characterImages">An array containing image file paths for the characters used</param>
        /// <returns></returns>
        public static string CreateThumbnailImage(ThumbnailDataModel thumbnailData, ThumbnailConfigurationModel thumbnailConfiguration)
        {
            //Create a new bitmap image
            Image thumbnail_bmp = new Bitmap(1920, 1080);
            //Create a new drawing surface from the bitmap
            Graphics drawing = Graphics.FromImage(thumbnail_bmp);
            //Configure the surface to be higher quality
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            //Create an image resource for the background and overlay of the tumbnail
            Image background = Image.FromFile(thumbnailConfiguration.backgroundImage);
            Image foreground = Image.FromFile(thumbnailConfiguration.foregroundImage);

            //Create an image resource for each player's character
            Image left_character;
            Image right_character;
            Image left_character2;
            Image right_character2;

            switch (player_count)
            {
                case 2:
                    left_character = Image.FromFile(characterImages[0]);
                    right_character = Image.FromFile(characterImages[1]);
                    right_character.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    drawing.Clear(Color.White);                                         //Clear the surface of all data

                    drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

                    drawing.DrawImage(left_character, thumbnailConfiguration.characterXOffset[0], thumbnailConfiguration.characterYOffset[0], 1920, 1080);                //Draw Player 1's character
                    drawing.DrawImage(right_character, thumbnailConfiguration.characterXOffset[1], thumbnailConfiguration.characterYOffset[1], 1920, 1080);               //Draw Player 2's character

                    drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters
                    break;
                case 4:
                    left_character = Image.FromFile(characterImages[0]);
                    right_character = Image.FromFile(characterImages[1]);
                    right_character.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    left_character2 = Image.FromFile(characterImages[2]);
                    right_character2 = Image.FromFile(characterImages[3]);
                    right_character2.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    drawing.Clear(Color.White);                                         //Clear the surface of all data

                    drawing.DrawImage(background, 0, 0, 1920, 1080);                    //Draw the background

                    drawing.DrawImage(left_character2, thumbnailConfiguration.characterXOffset[0] - 100, thumbnailConfiguration.characterYOffset[0], 1920, 1080);                //Draw Player 3's character
                    drawing.DrawImage(right_character2, thumbnailConfiguration.characterXOffset[1] + 100, thumbnailConfiguration.characterYOffset[1], 1920, 1080);               //Draw Player 4's character

                    drawing.DrawImage(left_character, thumbnailConfiguration.characterXOffset[0], thumbnailConfiguration.characterYOffset[0] + 200, 1920, 1080);                //Draw Player 1's character
                    drawing.DrawImage(right_character, thumbnailConfiguration.characterXOffset[1], thumbnailConfiguration.characterXOffset[1] + 200, 1920, 1080);               //Draw Player 2's character

                    drawing.DrawImage(foreground, 0, 0, 1920, 1080);                    //Draw the overlay over the characters

                    break;
            }

            //Convert each player's name and the round in bracket to all capital letters and store them seperately
            player_name1 = player_name1.ToUpper();
            player_name2 = player_name2.ToUpper();
            round_text = round_text.ToUpper();

            //Create a drawing path for the text of the date, each player's name, and the round in bracket
            GraphicsPath draw_date = new GraphicsPath();
            GraphicsPath draw_name1 = new GraphicsPath();
            GraphicsPath draw_name2 = new GraphicsPath();
            GraphicsPath draw_round = new GraphicsPath();
            GraphicsPath draw_patch = new GraphicsPath();
            //Create a brush for Black and White
            Brush white_text = new SolidBrush(Color.White);
            Brush black_text = new SolidBrush(Color.Black);
            //Create two pen brushes- one normal thiccness, one thiccer
            Pen black_stroke = new Pen(black_text, 14);
            Pen light_stroke = new Pen(black_text, 10);
            //Create a text format with center alignments
            StringFormat text_center = new StringFormat();
            text_center.Alignment = StringAlignment.Center;
            text_center.LineAlignment = StringAlignment.Center;
            //Create font resources
            FontFamily keepcalm = new FontFamily(thumbnailConfiguration.thumbnailFont);
            Font calmfont = new Font(thumbnailConfiguration.thumbnailFont, 110, FontStyle.Regular);

            int font_size = thumbnailConfiguration.playerNameSize[0] + 5;                                                //Create a variable for adjustable font size
            Size namesize = TextRenderer.MeasureText(player_name1, calmfont);   //Create a variable to hold the size of player tags

            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font(thumbnailConfiguration.thumbnailFont, font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name1, calmfont);            //Measure the width of Player 1's name with this font size
            } while (namesize.Width >= 1000);      //1100                                     //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 1's name to its drawing path
            draw_name1.AddString(
                player_name1,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(420 + thumbnailConfiguration.playerNameXOffset[0], 160 + thumbnailConfiguration.playerNameYOffset[0]),          //110                                  //drawing location 480
                text_center);                                                   //text alignment
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name1);
            drawing.FillPath(white_text, draw_name1);

            font_size = thumbnailConfiguration.playerNameSize[1] + 5;                      //115                                      //Reset the font size
            //Start a loop
            do
            {
                font_size -= 5;                                                         //Reduce the font size
                calmfont = new Font(thumbnailConfiguration.thumbnailFont, font_size, FontStyle.Regular);     //Create a new font with this new size
                namesize = TextRenderer.MeasureText(player_name2, calmfont);            //Measure the width of Player 2's name with this font size
            } while (namesize.Width >= 1000);        //1100                                   //End the loop when the name fits within its boundaries
            //Adjust the thiccness of the outline to match the size of the text
            black_stroke.Width = font_size / 11 + 4;

            //Add Player 2's name to its drawing path
            draw_name2.AddString(
                player_name2,                                                   //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                font_size,                                                      //font size (drawing.DpiY * 120 / 72)
                new Point(1500 + thumbnailConfiguration.playerNameXOffset[1], 160 + thumbnailConfiguration.playerNameYOffset[1]), //110                                          //drawing location 1440
                text_center);                                                   //text alignment                                        // text to draw
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(black_stroke, draw_name2);
            drawing.FillPath(white_text, draw_name2);

            //Add the round in bracket to its drawing path
            draw_round.AddString(
                round_text,                                                      //text to draw
                keepcalm,                                                       //font family
                (int)FontStyle.Regular,                                         //font style
                thumbnailConfiguration.roundSize,                                                             //font size (drawing.DpiY * 120 / 72)
                new Point(960 + thumbnailConfiguration.roundXOffset, 720 + thumbnailConfiguration.roundYOffset), //620                                           //drawing location
                text_center);                                                   //text alignment     
            //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(light_stroke, draw_round);
            drawing.FillPath(white_text, draw_round);

            if (thumbnailConfiguration.showDateOnThumbnail == true)
            {
                //Add the date to its drawing path
                draw_date.AddString(
                    match_date,                                                     //text to draw
                    keepcalm,                                                       //font family
                    (int)FontStyle.Regular,                                         //font style
                    thumbnailConfiguration.dateSize,                                                      //font size (drawing.DpiY * 120 / 72)
                    new Point(300 + thumbnailConfiguration.dateXOffset, 940 + thumbnailConfiguration.dateYOffset),              //drawing location
                    text_center);                                                   //text alignment
                                                                                    //Set the outline and filling to the appropriate colors
                drawing.DrawPath(black_stroke, draw_date);
                drawing.FillPath(white_text, draw_date);
            }

            draw_patch.AddString(
                thumbnailConfiguration.patchVersion,                                                     //text to draw
                keepcalm,                                                         //font family
                (int)FontStyle.Regular,                                           //font style
                thumbnailConfiguration.patchSize,                                                       //font size (drawing.DpiY * 120 / 72)
                new Point(300 + thumbnailConfiguration.patchXOffset, 1020 + thumbnailConfiguration.patchYOffset), //620       //drawing location
                text_center);                                                     //text alignment     
                                                                                  //Draw the outline and filling in the appropriate colors
            drawing.DrawPath(light_stroke, draw_patch);
            drawing.FillPath(white_text, draw_patch);

            //Save the drawing surface back to the bitmap image
            drawing.Save();
            //Dispose the drawing surface
            drawing.Dispose();

            DateTime date = DateTime.Now;                                       //Find the current date and time
            string thumbnail_time = date.ToString("MMddyyyyHHmmss");            //Format the date and time in a string
            //Create a title for the bitmap image using the information provided by the user
            player_name1 = CleanFileName(player_name1);
            player_name2 = CleanFileName(player_name2);

            string thumbnail_image_name = round_text + @" " + player_name1 + @" Vs " + player_name2 + @" " + thumbnail_time + @".jpg";
            //Save the bitmap image as a JPG
            thumbnail_bmp.Save(AppSettings.DirectoryManagement.thumbnailDirectory + @"\" + thumbnail_image_name, System.Drawing.Imaging.ImageFormat.Jpeg);
            //Return the title of the image file
            return thumbnail_image_name;
        }

        /// <summary>
        /// Cleans a string of any characters that would cause errors in a file name
        /// </summary>
        /// <param name="unfilteredString"></param>
        /// <returns>The cleaned version of ths input string</returns>
        private static string CleanFileName(string unfilteredString)
        {
            return Regex.Replace(unfilteredString, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }
    }
}
