using System.Windows.Forms;
using CharacterLibrary;

namespace Stream_Info_Handler.CharacterSelect
{
    public partial class ColorSelect : Form
    {
        GenerateColors showColors = null;
        public ColorSelect(string toName, string toDirectory)
        {
            this.CenterToScreen();
            InitializeComponent();

            showColors = new GenerateColors(toName, toDirectory, this);
            showColors.importColors();
        }
    }
}
