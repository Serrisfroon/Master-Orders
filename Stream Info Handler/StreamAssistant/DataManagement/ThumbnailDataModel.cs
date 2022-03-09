using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    public class ThumbnailDataModel
    {
        public int playerCount { get; set; }
        public List<int> charactersPerPlayer { get; set; }
        public List<string> playerName { get; set; }
        public string matchDate { get; set; }
        public int[,] characterImages { get; set; }

        public ThumbnailDataModel()
        {

        }
    }
}
