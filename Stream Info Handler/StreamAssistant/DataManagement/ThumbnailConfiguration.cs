using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    public class ThumbnailConfiguration
    {
        public string backgroundImage { get; set; }
        public string foregroundImage { get; set; }
        public string thumbnailFont { get; set; }
        public int[] characterXOffset { get; set; }
        public int[] characterYOffset { get; set; }
        public int[] playerNameXOffset { get; set; }
        public int[] playerNameYOffset { get; set; }
        public int[] playerNameSize { get; set; }
        public int roundXOffset { get; set; }
        public int roundYOffset { get; set; }
        public int roundSize { get; set; }
        public bool showDateOnThumbnail { get; set; }
        public int dateXOffset { get; set; }
        public int dateYOffset { get; set; }
        public int dateSize { get; set; }
        public string patchVersion { get; set; }
        public int patchXOffset { get; set; }
        public int patchYOffset { get; set; }
        public int patchSize { get; set; }
        
        public ThumbnailConfiguration()
        {
            int playerNameCount = 2;
            int characterCount = 2;
            characterXOffset = new int[characterCount];
            characterYOffset = new int[characterCount];
            playerNameXOffset = new int[playerNameCount];
            playerNameYOffset = new int[playerNameCount];
            playerNameSize = new int[playerNameCount];
        }
    }
}
