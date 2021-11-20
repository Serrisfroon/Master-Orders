using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Windows.Forms;
using GraphicsLibrary;
using CharacterLibrary;

namespace Stream_Info_Handler.CharacterSelect
{
    public class GenerateColors
    {
        private int colorWidth = 128;
        private int colorHeight = 128;
        private int colorCount;
        private List<string> colorImages = new List<string>();
        private string characterName;
        private string colorBackground;
        private Form parentForm;
        private string gameDirectory;
        public GenerateColors(string toName, string toDirectory, Form toParent)
        {
            characterName = toName;
            parentForm = toParent;
            gameDirectory = toDirectory;
            colorBackground = gameDirectory + @"\colorbackground.png";
        }

        public void importColors()
        {
            string characterPath = gameDirectory + @"\" + characterName;
            string[] colorImages = Directory.GetDirectories(characterPath, "?");
            colorCount = colorImages.Length;
            parentForm.BackgroundImage = generateBackground();

            for (int i = 0; i < colorCount; i++)
            {
                int rowPosition;
                int columnPosition = Math.DivRem(i, 4, out rowPosition);
                int buttonX = colorWidth * rowPosition;
                int buttonY = colorHeight * columnPosition;

                Button createButton = new Button();
                createButton.Location = new Point(buttonX, buttonY);
                createButton.Size = new Size(colorWidth, colorHeight);
                createButton.Tag = (i + 1).ToString();
                createButton.Click += new EventHandler(colorClick);

                createButton.FlatAppearance.BorderSize = 2;
                createButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                createButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, Color.Aqua); ;
                createButton.FlatStyle = FlatStyle.Flat;
                createButton.BackColor = Color.Transparent;

                parentForm.Controls.Add(createButton);
            }
            return;
        }
        /// <summary>
        /// Generate a background based on the color background imnage and the colors provided
        /// </summary>
        private Image generateBackground()
        {
            Image backImage = Image.FromFile(colorBackground);
            //Create a new bitmap image
            Image formBackground = new Bitmap(1920, 1080);
            //Create a new drawing surface from the bitmap
            Graphics drawing = Graphics.FromImage(formBackground);
            //Configure the surface to be higher quality
            drawing.InterpolationMode = InterpolationMode.High;
            drawing.SmoothingMode = SmoothingMode.HighQuality;
            drawing.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            drawing.CompositingQuality = CompositingQuality.HighQuality;

            drawing.Clear(Color.White);                                         //Clear the surface of all data

            string characterPath = gameDirectory + @"\" + characterName;
            string[] colorImages1 = Directory.GetDirectories(characterPath, "?");
            string[] colorImages2 = Directory.GetDirectories(characterPath, "1?");
            List<string> removeFirstColor = new List<string>(colorImages2);
            removeFirstColor.RemoveAt(0);
            colorImages2 = removeFirstColor.ToArray();

            string[] colorImages = new string[colorImages1.Length + colorImages2.Length];
            colorImages1.CopyTo(colorImages, 0);
            colorImages2.CopyTo(colorImages, colorImages1.Length);

            colorImages2.CopyTo(colorImages, colorImages1.Length);

            colorCount = colorImages.Length;
            for (int i = 0; i < colorCount; i++)
            {
                int rowPosition;
                int columnPosition = Math.DivRem(i, 4, out rowPosition);
                int buttonX = colorWidth * rowPosition;
                int buttonY = colorHeight * columnPosition;

                drawing.DrawImage(backImage, buttonX, buttonY, colorWidth, colorHeight);                                                        //Draw the background
                drawing.DrawImage(ImageTools.ResizeImage(Image.FromFile(colorImages[i] + @"\stamp.png"), colorWidth, colorHeight),
                    buttonX, buttonY, colorWidth, colorHeight);                                                                                 //Draw the character color
            }
            //Save the drawing surface back to the bitmap image
            drawing.Save();
            //Dispose the drawing surface
            drawing.Dispose();

            return formBackground;
        }

        private void colorClick(object sender, EventArgs e)
        {
            CharacterData returnCharacter = new CharacterData();
            returnCharacter.characterName = characterName;
            returnCharacter.characterColor = Int32.Parse((((Button)sender).Tag).ToString());
            parentForm.Tag = returnCharacter;
            parentForm.DialogResult = DialogResult.OK;
            parentForm.Close();
        }
    }
}
