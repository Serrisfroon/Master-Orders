using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    public class ThumbnailConfigurationModel
    {
        const int maxPlayerNamecount = 4;
        
        const int maxCharactersPerPlayer = 4;
        public string backgroundImage { get; set; }
        public string foregroundImage { get; set; }
        public string thumbnailFont { get; set; }
        public int[,] characterXOffset { get; set; }
        public int[,] characterYOffset { get; set; }
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
        public int playerNameCount { get; set; }
        public int charactersPerPlayer { get; set; }
        public ThumbnailConfigurationModel()
        {
            characterXOffset = new int[maxPlayerNamecount, maxCharactersPerPlayer];
            characterYOffset = new int[maxPlayerNamecount, maxCharactersPerPlayer];
            playerNameXOffset = new int[maxPlayerNamecount];
            playerNameYOffset = new int[maxPlayerNamecount];
            playerNameSize = new int[maxPlayerNamecount];
        }
    }
}
