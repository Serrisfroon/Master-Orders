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


        public enum ErrorType
        {
            characterError
        }
        public void DisplaySettingsError(string errorMessage, Label errorLabel, TextBox inputTextBox, ErrorType errorType, object disableObject)
        {
            errorLabel.Text = errorMessage;
            inputTextBox.BackColor = warningColor;
            switch(errorType)
            {
                case ErrorType.characterError:
                    ((ComboBox)disableObject).Enabled = false;
                    break;
            }
        }

        public void SelectDirectory(FolderBrowserDialog dialogBox, TextBox outputBox)
        {
            if (dialogBox.ShowDialog() == DialogResult.OK)
            {
                outputBox.Text = dialogBox.SelectedPath;                //Update the setting text
            }
        }

        public void UpdateCharacterComboBox(ComboBox characterComboBox, string[] characterNames)
        {
            characterComboBox.BeginUpdate();                            //Begin
            characterComboBox.Items.Clear();                            //Empty the item list 
            int character_count = characterNames.Length;                //Store the number of characters
                                                                        //Loop through every character
            for (int x = 0; x < character_count; x++)
            {
                characterComboBox.Items.Add(characterNames[x]);         //Add the character's name to the combobox
            }
            characterComboBox.EndUpdate();                              //End
            characterComboBox.SelectedIndex = 0;                        //Set the combobox index to 0
        }
    }
}
