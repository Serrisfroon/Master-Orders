using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler.AppSettings.GeneralSettings
{
    public class ScoreControlModel
    {
        public Button scoreButton { get; set; }
        public PictureBox scorePicturePreview { get; set; }
        public string pictureFileLocation { get; set; }

        public ScoreControlModel(int toIndex, Button toButton, PictureBox toPicturePreview)
        {
            scoreButton = toButton;
            scoreButton.Tag = toIndex;
            scorePicturePreview = toPicturePreview;
        }
        public void UpdateImage(string toFileLocation)
        {
            if (File.Exists(toFileLocation))
            {
                pictureFileLocation = toFileLocation;
                scorePicturePreview.Image = Image.FromFile(pictureFileLocation);
            }
        }
    }
}
