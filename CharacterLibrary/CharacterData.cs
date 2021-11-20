using System;
using System.Collections.Generic;
using System.Text;

namespace CharacterLibrary
{
    public class CharacterData
    {
        public string characterName { get; set; }
        public int characterColor { get; set; }

        public CharacterData()
        {

        }
        public CharacterData(string toName, int toColor)
        {
            characterName = toName;
            characterColor = toColor;
        }
    }
}
