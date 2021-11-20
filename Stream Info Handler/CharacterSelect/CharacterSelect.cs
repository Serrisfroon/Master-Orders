using System.Drawing;
using System.Windows.Forms;
using CharacterLibrary;

namespace Stream_Info_Handler.CharacterSelect
{
    public partial class CharacterSelect : Form
    {
        GenerateCharacters characters;
        public string returnCharacter;
        public int returnColor;
        string gameDirectory;
        public CharacterSelect(string toDirectory)
        {
            this.CenterToScreen();
            InitializeComponent();

            gameDirectory = toDirectory;
            characters = new GenerateCharacters(this, gameDirectory);

            //Load and size the picture.
            Image CSSImage = characters.getImage();
            this.BackgroundImage = CSSImage;
            this.Size = new Size(CSSImage.Width, CSSImage.Height);


            characters.loadXML();
            characters.generateCSS();

        }
    }
}
