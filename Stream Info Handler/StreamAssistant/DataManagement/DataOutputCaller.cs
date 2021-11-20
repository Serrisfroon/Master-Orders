using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler.StreamAssistant.DataManagement
{
    public static class DataOutputCaller
    {
        public static bool automaticUpdates { get; set; }
        /// <summary>
        /// Process an output update. Will also update the doubles team name if needed. Will also account for sponsor prefix and Grand Finals [L] if needed.
        /// </summary>
        /// <param name="playerBox">The Player Settings belonging to the player being updated.</param>
        /// <param name="outputName">The name of the field being updated</param>
        /// <param name="outputValue">The new value for the updating field</param>
        public static void update_information(ref PlayerSettings playerBox, string outputName, string outputValue)
        {
            //Determine the update button
            Button update_button = playerBox.update;
            //Reset the update button
            update_button.Enabled = true;
            switch (update_button.Text)
            {
                case "Start":
                    //If the match hasn't started yet, highlight the update button in green
                    update_button.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\green.gif");
                    break;
                case "Update":
                    //If the match has started...
                    //Highlight the update button in blue if auto update is disabled
                    if (DataOutputCaller.automaticUpdates == false)
                    {
                        update_button.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\blue.gif");
                    }
                    else
                    {
                        //Otherwise, Disable the update button and push through the update
                        update_button.Enabled = false;

                        string variable_name;
                        string info_type;

                        //create the text file name based on if this is a player, as well as their number
                        if (playerBox.isPlayer == true)
                        {
                            variable_name = "player" + outputName + playerBox.player_number;
                            info_type = "player";
                        }
                        else
                        {
                            variable_name = "commentator" + outputName + playerBox.player_number;
                            info_type = "commentator";
                        }
                        //Update the individual text file
                        System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\" + variable_name + ".txt", outputValue);

                        //Update the xml file. Only need to update here since overlay scripts will piece togetehr anything needed.
                        update_xml(info_type, playerBox.player_number, outputName, outputValue);

                        //If the sponsor prefix or tag were updated, adjust the tag output fil to include the team prefix and the grands loser signifier if needed.
                        if (outputName == "sponsor-prefix" || outputName == "tag")
                        {
                            if (playerBox.isPlayer == true)
                            {
                                //Force the variable name to tag since the sponsor prefix doesn't have its own output
                                variable_name = "player" + "tag" + playerBox.player_number;
                                info_type = "player";
                            }
                            else
                            {
                                //Force the variable name to tag since the sponsor prefix doesn't have its own output
                                variable_name = "commentator" + "tag" + playerBox.player_number;
                                info_type = "commentator";
                            }
                            string newname = get_output_name(ref playerBox);


                            System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\" + variable_name + ".txt", newname);

                            //If it's dubs and a player is being updated, do a team name update
                            if (global_values.format == "Doubles" && playerBox.isPlayer == true)
                            {
                                if (playerBox.player_number == 1 || playerBox.player_number == 3)
                                {
                                    //Pull both players' names from the text files
                                    string name1 = File.ReadAllText(DirectoryManagement.outputDirectory + @"\playetag1.txt");
                                    //Remove the loser signifier from the first name if needed
                                    if (name1.Contains("[L]"))
                                        name1 = name1.Replace("[L]", "");
                                    string name2 = File.ReadAllText(DirectoryManagement.outputDirectory + @"\playetag3.txt");
                                    string team_value = name1 + " + " + name2;
                                    //Write the name to the file
                                    System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name1.txt", team_value);
                                }
                                else
                                {
                                    //Pull both players' names from the text files
                                    string name1 = File.ReadAllText(DirectoryManagement.outputDirectory + @"\playetag2.txt");
                                    //Remove the loser signifier from the first name if needed
                                    if (name1.Contains("[L]"))
                                        name1 = name1.Replace("[L]", "");
                                    string name2 = File.ReadAllText(DirectoryManagement.outputDirectory + @"\playetag4.txt");
                                    string team_value = name1 + " + " + name2;
                                    //Write the name to the file                                        
                                    System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name2.txt", team_value);
                                }
                            }
                        }

                    }
                    break;
            }
        }


        /// <summary>
        /// Get the name to be displayed for a player or commentator. Considers the tag, team, and loser status
        /// </summary>
        /// <param name="playerBox">The player settings to pull the output name from</param>
        /// <returns></returns>
        public static string get_output_name(ref PlayerSettings playerBox)
        {
            int playerNumber = playerBox.player_number;
            //Create the name to be sent to output based on the tag, team, and loser status.
            string output_name = "";
            if (playerBox.getTeam() != "")
                output_name = playerBox.getTeam() + TextFileManagement.sponsorSeperator + playerBox.getTag() + playerBox.getLoser();
            else
                output_name = playerBox.getTag() + playerBox.getLoser();

            //Update the region as well. Not sure why thi is happening here.
            updateRegion(ref playerBox);

            return output_name;
        }

        /// <summary>
        /// Updates the region image used when the setting is enabled.
        /// </summary>
        /// <param name="playerBox">The player settings to check a region for.</param>
        public static void updateRegion(ref PlayerSettings playerBox)
        {
            //Check if the region contains the seperator
            if (ImageManagement.enableRegionImages == true && playerBox.roster_number != -1)
            {
                //Reset all images related to the player
                string[] image_files = {
                                    @"\region" + playerBox.player_number.ToString() + ".png" };
                foreach (string replace_image in image_files)
                {
                    if (File.Exists(DirectoryManagement.outputDirectory + replace_image))
                    {
                        File.Delete(DirectoryManagement.outputDirectory + replace_image);
                    }
                    File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", DirectoryManagement.outputDirectory + replace_image);
                }
                string find_region = global_values.roster[playerBox.roster_number].region;

                if (find_region.Contains(" - "))
                {
                    //Check each index of the region string
                    for (int i = 0; i < find_region.Length; i++)
                    {
                        //Check if the seperator is present at this index
                        if (find_region.Substring(i).StartsWith(" - ") == true)
                        {
                            //Set the region to be before this index
                            find_region = find_region.Substring(0, i);
                            break;
                        }
                    }
                }

                if (File.Exists(DirectoryManagement.regionDirectory + @"\" + find_region + @".png"))
                {
                    Image region_image = Image.FromFile(DirectoryManagement.regionDirectory + @"\" + find_region + @".png");
                    region_image.Save(DirectoryManagement.outputDirectory + @"\region" + playerBox.player_number.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        /// <summary>
        /// Updates the sponsor image for a given player based on their settings
        /// </summary>
        /// <param name="playerBox">The settings used to determine the sponsor image</param>
        public static void updateSponsorImage(ref PlayerSettings playerBox)
        {
            int player_number = playerBox.player_number;
            string playerSponsor = playerBox.getTeam();
            string image_file = @"\sponsor" + player_number.ToString() + ".png";

            if (File.Exists(DirectoryManagement.outputDirectory + image_file))
            {
                File.Delete(DirectoryManagement.outputDirectory + image_file);
            }
            File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", DirectoryManagement.outputDirectory + image_file);
            if (ImageManagement.enableSponsorImages == true)
            {
                if (playerSponsor != "")
                {
                    string sponsorfile = DirectoryManagement.sponsorDirectory + @"\" + playerSponsor + @".png";
                    if (File.Exists(sponsorfile))
                    {
                        Image sponsor_image = Image.FromFile(sponsorfile);
                        sponsor_image.Save(DirectoryManagement.outputDirectory + @"\sponsor" + player_number.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }

        /// <summary>
        /// Updates the tag of a player
        /// </summary>
        /// <param name="playerBox"></param>
        public static void updateTag(ref PlayerSettings playerBox, int newPlayerIndex)
        {
            //Check if this tag is included in the player index
            //if (playerBox.roster_number != -1)
            for(int i = 0; i < global_values.roster.Count; i++)
            {
                if (global_values.roster[i].unique_tag == playerBox.getTag())
                {
                    newPlayerIndex = i;
                }
            }
            if (newPlayerIndex >= 0)
            {
                if (global_values.roster[newPlayerIndex].unique_tag == playerBox.getTag())
                {
                    //Update the Twitter ComboBox with this player data's twitter
                        playerBox.twitter.Text = global_values.roster[newPlayerIndex].twitter;
                    playerBox.team.Text = global_values.roster[newPlayerIndex].sponsor;
                    //Update the text of this ComboBox to the display text of this player's tag
                    updateSponsorImage(ref playerBox);
                    playerBox.roster_number = newPlayerIndex;

                    if (playerBox.isPlayer == true)
                    {
                        Image updateCharacter = Image.FromFile(DirectoryManagement.GetGameDirectory() + @"\" +
                                                                global_values.roster[newPlayerIndex].character[0] + @"\" +
                                                                global_values.roster[newPlayerIndex].color[0].ToString() + @"\stamp.png");

                        playerBox.character.BackgroundImage = updateCharacter;
                        playerBox.character.Text = "";
                        playerBox.characterName = global_values.roster[newPlayerIndex].character[0];
                        playerBox.colorNumber = global_values.roster[newPlayerIndex].color[0];
                        playerBox.image_directory = DirectoryManagement.GetGameDirectory() + @"\" +
                        playerBox.characterName + @"\" + playerBox.colorNumber.ToString();
                    }

                    update_information(ref playerBox, "tag", playerBox.tag.Text);
                }
            }
            //Update the player_tag for this player
            else
            {
                playerBox.roster_number = -1;
            }
            if (DataOutputCaller.automaticUpdates == true && playerBox.update.Text == "Update")
            {
                update_information(ref playerBox, "tag", playerBox.tag.Text);
            }
            //return;
        }

        /// <summary>
        /// Updates a single field in the XML file.
        /// </summary>
        /// <param name="updateTarget">The type of update to be done. Accepted values: match player commentator</param>
        /// <param name="number">Used for player and commentator updates. The number corresponding to the player(1-4) or commentator(1-2)</param>
        /// <param name="elementName">The field to update</param>
        /// <param name="elementValue">The field's new value</param>
        public static void update_xml(string updateTarget, int number, string elementName, string elementValue)
        {
            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            XDocument xml = XDocument.Load(xml_file);
            switch (updateTarget)
            {
                case "match":
                    xml.Root.Element("match").Element(elementName).ReplaceWith(new XElement(elementName, elementValue));
                    break;
                case "player":
                    xml.Root.Element("player-" + number.ToString()).Element(elementName).ReplaceWith(new XElement(elementName, elementValue));
                    break;
                case "commentator":
                    xml.Root.Element("commentator-" + number.ToString()).Element(elementName).ReplaceWith(new XElement(elementName, elementValue));
                    break;
            }
            xml.Save(xml_file);
        }

        /// <summary>
        /// Updates all fields in the XML file for a single player or commentator
        /// </summary>
        /// <param name="updateTarget">The type of update to be done. Accepted values: player commentator</param>
        /// <param name="playerBox">The player settings to pull specfic field data from</param>
        /// <param name="update_player">The player record to pull remaining field data from</param>
        public static void update_xml(string updateTarget, ref PlayerSettings playerBox, PlayerRecordModel update_player)
        {
            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            string xml_element = "";
            int number = playerBox.player_number;

            XDocument xml = XDocument.Load(xml_file);
            switch (updateTarget)
            {
                case "match":
                    return;
                case "player":
                    xml_element = "player-" + number.ToString();
                    xml.Root.Element(xml_element).Element("losers-side").ReplaceWith(new XElement("losers-side", playerBox.isLoser()));
                    xml.Root.Element(xml_element).Element("tag").ReplaceWith(new XElement("tag", get_output_name(ref playerBox)));
                    xml.Root.Element(xml_element).Element("twitter").ReplaceWith(new XElement("twitter", update_player.twitter));
                    xml.Root.Element(xml_element).Element("base-tag").ReplaceWith(new XElement("base-tag", update_player.tag));
                    xml.Root.Element(xml_element).Element("full-name").ReplaceWith(new XElement("full-name", update_player.fullname));
                    xml.Root.Element(xml_element).Element("sponsor-full").ReplaceWith(new XElement("sponsor-full", update_player.fullsponsor));
                    xml.Root.Element(xml_element).Element("sponsor-prefix").ReplaceWith(new XElement("sponsor-prefix", update_player.sponsor));
                    xml.Root.Element(xml_element).Element("character-name").ReplaceWith(new XElement("character-name", update_player.character[0]));
                    xml.Root.Element(xml_element).Element("region-name").ReplaceWith(new XElement("region-name", update_player.region));
                    xml.Root.Element(xml_element).Element("character-icon").ReplaceWith(new XElement("character-icon",
                        DirectoryManagement.GetGameDirectory() + @"\" + update_player.character[0] + @"\" + update_player.color[0].ToString() + @"\stock.png"));
                    break;
                case "commentator":
                    xml_element = "commentator-" + number.ToString();
                    xml.Root.Element(xml_element).Element("tag").ReplaceWith(new XElement("tag", update_player.tag));
                    xml.Root.Element(xml_element).Element("sponsor-prefix").ReplaceWith(new XElement("sponsor-prefix", update_player.sponsor));
                    xml.Root.Element(xml_element).Element("twitter").ReplaceWith(new XElement("twitter", update_player.twitter));
                    break;
            }
            xml.Save(xml_file);
        }
    }
}
