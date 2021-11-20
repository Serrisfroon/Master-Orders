using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Xml.Linq;
using CharacterLibrary;

namespace Stream_Info_Handler.CharacterSelect
{
    public class GenerateCharacters
    {
        private int buttonWidth;
        private int buttonHeight;
        private Region buttonRegion;
        private int startingX;
        private int startingY;
        private int padding;
        private string gameDirectory;
        private List<CharacterButton> characterList;
        Form parentForm;

        /// <summary>
        /// Goes through the process of prompting the user to select a character and color alt. 
        /// If cancelled, it returns the original character and alt.
        /// </summary>
        /// <param name="originalCharacter">Original character data</param>
        /// <param name="gameDirectory">The directory of the current selected game</param>
        /// <returns>The end result character data(new choices or original if cancelled)</returns>
        public static CharacterData GetCharacter(CharacterData originalCharacter, string gameDirectory)
        {
            using (CharacterSelect characterForm = new CharacterSelect(gameDirectory))
            {
                DialogResult result = characterForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    return (CharacterData)characterForm.Tag;
                }
                else
                {
                    return originalCharacter;
                }
            }
        }

        /// <summary>
        /// Generates buttons for every character into the given form. Buttons will be transparent and tagged with the character's name
        /// </summary>
        /// <param name="parentForm">The Form to add the buttons to</param>
        public void generateCSS()
        {
            foreach (CharacterButton createCharacter in characterList)
            {
                Button createButton = new Button();
                createButton.Location = new Point(createCharacter.buttonX, createCharacter.buttonY);
                createButton.Size = new Size(createCharacter.buttonWidth, createCharacter.buttonHeight);
                createButton.Region = createCharacter.buttonRegion;
                createButton.Tag = createCharacter.characterName;
                createButton.Click += new EventHandler(characterClick);
                createButton.FlatAppearance.BorderSize = 0;
                createButton.FlatAppearance.MouseDownBackColor = Color.Transparent;
                createButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(85, Color.Aqua); ;
                createButton.BackColor = Color.Transparent;
                createButton.FlatStyle = FlatStyle.Flat;

                parentForm.Controls.Add(createButton);
            }
        }

        public GenerateCharacters(Form toparentForm, string toDirectory)
        {
            parentForm = toparentForm;
            gameDirectory = toDirectory;
        }

        /// <summary>
        /// Loads all data from the current game's characterselect.xml . The file contains information on character names and button locations.
        /// </summary>
        public void loadXML()
        {
            //Reset the character list
            characterList = new List<CharacterButton>();

            //Open the CSS file
            XDocument xml = XDocument.Load(gameDirectory + @"\characterselect.xml");
            //Load general settings
            buttonWidth = (int)xml.Root.Element("general").Element("button-width");
            buttonHeight = (int)xml.Root.Element("general").Element("button-height");

            buttonRegion = LoadButtonRegion(xml);

            startingX = (int)xml.Root.Element("general").Element("starting-x");
            startingY = (int)xml.Root.Element("general").Element("starting-y");
            padding = (int)xml.Root.Element("general").Element("padding");

            //Loop through and create a character button for each character
            characterList = new List<CharacterButton>();
            for (int i = 1; ((string)xml.Root.Element("character-" + i.ToString())) != null; i++)
            {
                //Start filling out data for a new character button
                CharacterButton newCharacter = new CharacterButton();

                //Read the row and column of the character
                int newRow = (int)xml.Root.Element("character-" + i.ToString()).Element("row");
                int newColumn = (int)xml.Root.Element("character-" + i.ToString()).Element("column");

                //Set the starting X position
                int buttonStartingX = startingX;
                //Check if this row has a unique starting position
                if (((string)xml.Root.Element("row-" + newRow.ToString())) != null)
                {
                    if (((string)xml.Root.Element("row-" + newRow.ToString()).Element("starting-x")) != null)
                    {
                        buttonStartingX = (int)xml.Root.Element("row-" + newRow.ToString()).Element("starting-x");
                    }
                }
                //Set the starting X position
                int buttonStartingY = startingY + padding + ((padding * 2) + buttonHeight) * (newRow - 1);
                //Check if this row has a unique starting position
                if (((string)xml.Root.Element("row-" + newRow.ToString())) != null)
                {
                    if (((string)xml.Root.Element("row-" + newRow.ToString()).Element("starting-y")) != null)
                    {
                        buttonStartingY = (int)xml.Root.Element("row-" + newRow.ToString()).Element("starting-y");
                    }
                }


                //Assign attributes to the new character button
                newCharacter.characterName = (string)xml.Root.Element("character-" + i.ToString()).Element("name");
                newCharacter.buttonWidth = buttonWidth;
                newCharacter.buttonHeight = buttonHeight;
                newCharacter.buttonRegion = buttonRegion;
                newCharacter.buttonX = buttonStartingX + padding + ((padding * 2) + buttonWidth) * (newColumn - 1);
                newCharacter.buttonY = buttonStartingY;

                //Add the new character button to the list.
                characterList.Add(newCharacter);
            }



        }

        /// <summary>
        /// Load the button region information from the specified XML
        /// </summary>
        /// <param name="xml">The xml doc to load button info from</param>
        /// <returns>The resulting button region</returns>
        private Region LoadButtonRegion(XDocument xml)
        {
            GraphicsPath buttonPath = new GraphicsPath();
            int[] borderX = new int[] {
            (int)xml.Root.Element("general").Element("button-x1"),
            (int)xml.Root.Element("general").Element("button-x2"),
            (int)xml.Root.Element("general").Element("button-x3"),
            (int)xml.Root.Element("general").Element("button-x4")
            };
            int[] borderY = new int[] {
            (int)xml.Root.Element("general").Element("button-y1"),
            (int)xml.Root.Element("general").Element("button-y2"),
            (int)xml.Root.Element("general").Element("button-y3"),
            (int)xml.Root.Element("general").Element("button-y4")
            };

            buttonPath.AddLine(borderX[0], borderY[0], borderX[1], borderY[1]);
            buttonPath.AddLine(borderX[1], borderY[1], borderX[2], borderY[2]);
            buttonPath.AddLine(borderX[2], borderY[2], borderX[3], borderY[3]);
            buttonPath.AddLine(borderX[3], borderY[3], borderX[0], borderY[0]);

            Region returnRegion = new Region(buttonPath);

            return returnRegion;
        }

        /// <summary>
        /// Returns the Character Select Screen for the current game.
        /// </summary>
        /// <returns></returns>
        public Image getImage()
        {
            return Image.FromFile(gameDirectory + @"\characterselect.png");
        }

        private void characterClick(object sender, EventArgs e)
        {
            parentForm.Visible = false;
            using (ColorSelect colorForm = new ColorSelect((string)((Button)sender).Tag, gameDirectory))
            {
                DialogResult result = colorForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    parentForm.Tag = colorForm.Tag;
                    parentForm.DialogResult = DialogResult.OK;
                    parentForm.Close();
                }
            }
        }
    }
}
