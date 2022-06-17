using Stream_Info_Handler.AppSettings;
using System;
using System.Windows.Forms;

namespace Stream_Info_Handler.StreamAssistant
{
    /// <summary>
    /// Contains a set of controls assigned to a single player, as well as some extra data pull the image from file, etc.
    /// </summary>
    public class PlayerSettings
    {
        public ComboBox tag;
        public TextBox twitter;
        public Button character;
        public Button update;
        public NumericUpDown score;
        public CheckBox loser;
        public TextBox team;
        public Button save;
        public string image_directory;
        public int player_number;
        public int color_size;
        public bool isPlayer;
        public int roster_number = -1;
        public int colorNumber;
        public string characterName;

        public PlayerSettings(int toindex, int tonumber, ComboBox totag, TextBox totwitter, Button tocharacter, Button toupdate, NumericUpDown toscore, CheckBox toloser, Button tosave, TextBox toteam, int colorsize)
        {
            player_number = tonumber;
            tag = totag;
            twitter = totwitter;
            character = tocharacter;
            update = toupdate;
            score = toscore;
            loser = toloser;
            save = tosave;
            team = toteam;
            image_directory = DirectoryManagement.GetGameDirectory() + @"\Random\1";
            color_size = colorsize;
            isPlayer = true;
            setTags(toindex);
        }

        public PlayerSettings(int toindex, bool player, int tonumber, ComboBox totag, TextBox totwitter, Button toupdate, TextBox toteam, Button tosave)
        {
            isPlayer = player;
            tag = totag;
            twitter = totwitter;
            team = toteam;
            update = toupdate;
            player_number = tonumber;
            save = tosave;
            setTags(toindex);
        }

        private void setTags(int index)
        {
            tag.Tag = (tag.Tag == null) ? index : tag.Tag;
            twitter.Tag = (twitter.Tag == null) ? index : twitter.Tag;
            team.Tag = (team.Tag == null) ? index : team.Tag;
            update.Tag = (update.Tag == null) ? index : update.Tag;
            save.Tag = (save.Tag == null) ? index : save.Tag;
            if (isPlayer)
            {
                character.Tag = (character.Tag == null) ? index : character.Tag;
                loser.Tag = (loser.Tag == null) ? index : loser.Tag;
                score.Tag = (score.Tag == null) ? index : score.Tag;
            }
        }

        public string getTag()
        {
            return tag.Text;
        }
        public string getTwitter()
        {
            return twitter.Text;
        }
        public string getCharacter()
        {
            return characterName;
        }
        public int getColor()
        {
            return colorNumber;
        }
        public int getScore()
        {
            return Convert.ToInt32(score.Value);
        }
        public string getLoser()
        {
            if (loser == null)
                return "";
            if (loser.Checked == true)
                return "[L]";
            else
                return "";
        }
        public bool isLoser()
        {
            return loser.Checked;
        }
        public string getTeam()
        {
            return team.Text;
        }



        public string getSave()
        {
            return tag.Text;
        }
        public string getUpdate()
        {
            return tag.Text;
        }
    }
}
