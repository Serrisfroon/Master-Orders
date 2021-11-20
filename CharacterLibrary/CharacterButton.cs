using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace CharacterLibrary
{
    public class CharacterButton
    {
        public int buttonX { get; set; }
        public int buttonY { get; set; }
        public int buttonWidth { get; set; }
        public int buttonHeight { get; set; }

        public Region buttonRegion { get; set; }
        public string characterName { get; set; }
    }
}
