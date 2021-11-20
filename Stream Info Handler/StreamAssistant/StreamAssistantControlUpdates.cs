using Stream_Info_Handler.CharacterSelect;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using CharacterLibrary;
using Stream_Info_Handler.AppSettings;

namespace Stream_Info_Handler.StreamAssistant
{
    //This class contains any classes that would cause updates to the appearance an options provided from controls.
    //None of this should be data management
    public static class StreamAssistantControlUpdates
    {
        /// <summary>
        /// Read a list of rounds from a text file, line break-seperated, and add them to the chosen ComboBox as items.
        /// </summary>
        /// <param name="roundBox">The ComboBox to add rounds to</param>
        /// <param name="fileName">The file to read rounds from</param>
        public static void UpdateRounds(ComboBox roundBox)
        {
            string[] importedRounds = SettingsFile.LoadBracketRounds();
            roundBox.Items.Clear();
            roundBox.BeginUpdate();
            roundBox.Items.AddRange(importedRounds);
            roundBox.EndUpdate();
        }

        /// <summary>
        /// Reads a List of queues from a List and adds them to the Chose comboBox. Changes the ComboBox index to match the desired index.
        /// </summary>
        /// <param name="queueBox">The ComboBox to add queues to</param>
        /// <param name="queues">The List to pull names from</param>
        /// <param name="selectedQueue"></param>
        public static void UpdateQueues(ComboBox queueBox, List<database_tools.streamQueue> queues, int selectedQueue)
        {
            //Add Queues to the dropdown
            queueBox.BeginUpdate();
            queueBox.Items.Clear();                                            //Empty the item list
            queueBox.Items.Add("None");
            for (int i = 0; i < queues.Count; i++)
            {
                queueBox.Items.Add(queues[i].name);
            }
            queueBox.EndUpdate();
            if (selectedQueue != -1)
                queueBox.Text = queues[selectedQueue].name;

        }

        /// <summary>
        /// Update tab pages based on the format given. Remove the unneeded tab and keep the needed one.
        /// </summary>
        /// <param name="newFormat">The new format. "Singles" or "Doubles"</param>
        /// <param name="tabCollection">the TabControl containing the relevant TabPages</param>
        /// <param name="tabSingle">Singles TabPage</param>
        /// <param name="tabDouble">Doubles TabPage</param>
        /// <returns></returns>
        public static string UpdateFormat(string newFormat, ref TabControl tabCollection, ref TabPage tabSingle, ref TabPage tabDouble)
        {
            if (newFormat == "Singles")
            {
                if (!tabCollection.TabPages.Contains(tabSingle))
                    tabCollection.TabPages.Insert(1, tabSingle);
                tabCollection.TabPages.Remove(tabDouble);
            }
            if (newFormat == "Doubles")
            {
                if (!tabCollection.TabPages.Contains(tabDouble))
                    tabCollection.TabPages.Insert(1, tabDouble);
                tabCollection.TabPages.Remove(tabSingle);
            }
            if (global_values.bracket_assistant != null)
            {
                global_values.bracket_assistant.reload_settings();
            }
            return newFormat;
        }

        /// <summary>
        /// Updates the text of the VoD button depending on the flags that are enabled
        /// </summary>
        /// <param name="youtube">YouTube uploads enabled</param>
        /// <param name="thumbnail">Thumbnail creation enabled</param>
        /// <param name="title">Video title copying enabled</param>
        /// <returns></returns>
        public static string UpdateYoutube(bool youtube, bool thumbnail, bool title)
        {
            string uploadButtonText = "";
            if (youtube == true)
            {
                uploadButtonText = "Upload to YouTube";
            }
            else
            {
                if (thumbnail == true)
                {
                    if (title == true)
                        uploadButtonText = "Create Thumbnail and Copy Video Title";
                    else
                        uploadButtonText = "Create Thumbnail";
                }
                else
                {
                    if (title == true)
                        uploadButtonText = "Copy Video Title";
                }
            }
            return uploadButtonText;
        }


        /// <summary>
        /// Update a ComboBox to include all unique tags from the roster array. 
        /// </summary>
        /// <param name="update_box">The ComboBox to update</param>
        public static void update_names(ref ComboBox update_box)
        {
            update_box.BeginUpdate();                                            //Begin
            update_box.Items.Clear();                                            //Empty the item list
            for (int i = 0; i < global_values.roster.Count; i++)
            {
                update_box.Items.Add(global_values.roster[i].unique_tag);
            }
            update_box.EndUpdate();                                              //End
            update_box.Text = "";
        }

        /// <summary>
        /// When Enter is pressed, focus is shifted over to the next control. Used to have a different key function as Tab would.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void tab_Next(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                Control p = ((Control)sender).Parent;
                Control ctl = (Control)sender;
                p.SelectNextControl(ctl, true, true, true, true);
            }
        }

        public static void update_match_setting(Button updateButton, string variableName, string variableValue)
        {
            switch (updateButton.Text)
            {
                case "Start":
                    updateButton.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\green.gif");
                    updateButton.Enabled = true;
                    break;
                case "Update":
                    if (DataOutputCaller.automaticUpdates == false)
                    {
                        updateButton.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\blue.gif");
                        updateButton.Enabled = true;
                    }
                    else
                    {
                        updateButton.Enabled = false;
                        System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\" + variableName + ".txt", variableValue);
                        DataManagement.DataOutputCaller.update_xml("match", 1, variableName, variableValue);
                    }
                    break;
            }
        }
       
        public static void update_character(ref PlayerSettings playerBox)
        {
            CharacterData playerCharacter = new CharacterData(playerBox.characterName, playerBox.colorNumber);
            playerCharacter = GenerateCharacters.GetCharacter(playerCharacter, DirectoryManagement.GetGameDirectory());
            playerBox.characterName = playerCharacter.characterName;
            playerBox.colorNumber = playerCharacter.characterColor;
            if (playerBox.characterName != null && playerBox.colorNumber > 0)
            {
                Image newCharacter = Image.FromFile(DirectoryManagement.GetGameDirectory() + @"\" +
                                        playerBox.characterName + @"\" + playerBox.colorNumber.ToString() + @"\stamp.png");
                playerBox.image_directory = DirectoryManagement.GetGameDirectory() + @"\" +
                                        playerBox.characterName + @"\" + playerBox.colorNumber.ToString();
                playerBox.character.BackgroundImage = newCharacter;
                playerBox.character.Text = "";
            }
        }
    }
}
