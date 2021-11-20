using SqlDatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler.AppSettings.GeneralSettings
{
    public class EditableSettings
    {
        public static Color warningColor = Color.FromArgb(234, 153, 153);
        public Font thumbnailFont = new Font("Arial", 12, FontStyle.Regular);
        public string playlistId;
        public List<ScoreControlModel> scoreControls = new List<ScoreControlModel>();
        public  List<string> gameRosterDirectories { get; set; }


    }
}
