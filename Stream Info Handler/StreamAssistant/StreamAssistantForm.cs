//////////////////////////////////////////////////////////////////////////////////////////
//Master Orders 
//Stream Information Management Tool
//Developed by Dan Sanchez
//For use by UGS Gaming only, at the developer's discretion
//Copyright 2019, Dan Sanchez, All rights reserved.
//////////////////////////////////////////////////////////////////////////////////////////
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

using YoutubeLibrary;
using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using Stream_Info_Handler.StreamAssistant.DataManagement;

namespace Stream_Info_Handler.StreamAssistant
{

    public partial class StreamAssistantForm : Form
    {
        //Create the event
        public event closedform_event close_form;
        public static string save_name;
        string uploadButtonText = "";

        private PlayerSettings[] playerBoxes;
        private Button updateButton;

        //Manages match advancing when the current match moves outside the queue
        int outside_queue = 0;

        List<string> playlist_items;
        List<string> playlist_names;

        public StreamAssistantForm()
        {
            InitializeComponent();

            updateButton = btn_update;

            AddTabEvents();

            playerBoxes = InitializeSettings();

            this.CenterToScreen();

            pic_game_logo.Image = Image.FromFile($@"{ DirectoryManagement.GetGameDirectory() }\game_logo.png");

            StreamAssistantControlUpdates.UpdateRounds(cbx_round);

            this.TopMost = global_values.keepWindowsOnTop;

            SetStartingSettings();

            IntializeOutputXml();

            ImageManagement.ResetAllImages(DirectoryManagement.outputDirectory);
        }

        private void frm_main_Shown(object sender, EventArgs e)
        {
            uploadButtonText = StreamAssistantControlUpdates.UpdateYoutube(YoutubeController.enableYoutubeFunctions, YoutubeController.enableVideoThumbnails, YoutubeController.copyVideoTitle);

            ConfigureUpdateButton();

            YoutubeController.InitializeVodMonitor(DirectoryManagement.vodsDirectory, btn_upload);


            //Update the player names
            PlayerDatabase.LoadPlayers(GlobalSettings.selectedGame);

            StreamAssistantControlUpdates.update_names(ref cbx_tag1);
            StreamAssistantControlUpdates.update_names(ref cbx_tag2);
            StreamAssistantControlUpdates.update_names(ref cbx_team1_name1);
            StreamAssistantControlUpdates.update_names(ref cbx_team1_name2);
            StreamAssistantControlUpdates.update_names(ref cbx_team2_name1);
            StreamAssistantControlUpdates.update_names(ref cbx_team2_name2);
            StreamAssistantControlUpdates.update_names(ref cbx_commentator_tag1);
            StreamAssistantControlUpdates.update_names(ref cbx_commentator_tag2);



            if (global_values.queue_id > -1)
            {
                btn_previous_match.Enabled = true;
                btn_previous_match.Visible = true;
                btn_team_previous.Enabled = true;
                btn_team_previous.Visible = true;
                if (global_values.format == "Singles")
                    tab_main.SelectedIndex = 1;
                else
                    tab_main.SelectedIndex = 2;

                load_queue(0);
                tab_main.SelectedIndex = 0;
            }
            //Set the date box to today's date
            txt_date.Text = DateTime.Now.ToString("M/dd/yy");

            //////////////Set tooltips
            #region Tooltips
            //Set tooltips for Tournament Setup tab
            //
            ttp_tooltip.SetToolTip(txt_tournament,
                    "Set the name of the tournament. Used in the\n" +
                    "name of YouTube uploads. YouTube video \n" +
                    "descriptions may also use it.");
            ttp_tooltip.SetToolTip(txt_bracket,
                "Set the link to the bracket to be displayed.");
            ttp_tooltip.SetToolTip(txt_date,
                "Change the date displayed in thumbnails \n" +
                "created and YouTube video descriptions.");
            //
            //Set tooltips for the In-Game Display tab
            ttp_tooltip.SetToolTip(cbx_tag1,
                "Set the name/tag for Player 1.");
            ttp_tooltip.SetToolTip(cbx_tag2,
                "Set the name/tag for Player 2.");
            ttp_tooltip.SetToolTip(txt_twitter1,
                "Set the twitter handle for Player 1.");
            ttp_tooltip.SetToolTip(txt_twitter2,
                "Set the twitter handle for Player 2.");
            ttp_tooltip.SetToolTip(btn_character1,
                "Set the character for Player 1. This affects\n" +
                "YouTube uploads and stock icons.");
            ttp_tooltip.SetToolTip(btn_character2,
                "Set the character for Player 2. This affects\n" +
                "YouTube uploads and stock icons.");
            ttp_tooltip.SetToolTip(btn_save1,
                "Save this configuration for Player 1 to the \n" +
                "link Google Sheet. This information can later\n" +
                "be loaded by selecting the player's name from\n" +
                "the Player Tag box.");
            ttp_tooltip.SetToolTip(btn_save2,
                "Save this configuration for Player 2 to the \n" +
                "link Google Sheet. This information can later\n" +
                "be loaded by selecting the player's name from\n" +
                "the Player Tag box.");
            //
            ttp_tooltip.SetToolTip(btn_swap,
                "Switch the player information between Player 1 \n" +
                "and Player 2.");
            ttp_tooltip.SetToolTip(btn_reset_scores,
                "Reset both player scores to 0.");
            ttp_tooltip.SetToolTip(btn_update,
                "Click to begin the set and enable score control.\n" +
                "Pushes player and match information into the\n" +
                "Stream Files Directory.");
            //
            ttp_tooltip.SetToolTip(btn_reset,
                "Queue up the next match. This will clear all player\n" +
                "information and reset the score. If the Google\n" +
                "Sheets integration is enabled and Master Orders\n" +
                "is set to load Player Info and Stream Queue,\n" +
                "These fields will populate with the next match's\n" +
                "information from the stream queue.");
            ttp_tooltip.SetToolTip(btn_previous_match,
                "Reset the match and pull information for the\n" +
                "previous match in the stream queue.");
            ttp_tooltip.SetToolTip(btn_upload,
                "Upload the VoD for this match to YouTube. The VoD\n" +
                "file will be pulled from the VoD Directory so long\n" +
                "as recording is finished.");
            //
            ttp_tooltip.SetToolTip(cbx_round,
                "Set the current round in bracket.");

            //Set tooltips for the Commentators tab
            ttp_tooltip.SetToolTip(cbx_commentator_tag1,
                "Set the name/tag for the left commentator.");
            ttp_tooltip.SetToolTip(cbx_commentator_tag2,
                "Set the name/tag for the right commentator.");
            ttp_tooltip.SetToolTip(txt_commentator_twitter1,
                "Set the twitter handle for the left commentator.");
            ttp_tooltip.SetToolTip(txt_commentator_twitter2,
                "Set the twitter handle for the right commentator.");
            //
            ttp_tooltip.SetToolTip(btn_update_commentators,
                "Push through any updates made to the commentator\n" +
                "information.");
            ttp_tooltip.SetToolTip(btn_swapcommentators,
                "Switch the information between two commentators.");
            #endregion Tooltips
        }




        #region Player Field Changes
        //The text of a tag is being updated
        private void tag_TextChanged(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            DataOutputCaller.updateTag(ref playerBoxes[index], ((ComboBox)sender).SelectedIndex);
            player_output(index);
        }

        private void twitter_TextChanged(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            DataOutputCaller.update_information(ref playerBoxes[index], "twitter", playerBoxes[index].twitter.Text);
        }

        private void team_TextChanged(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            DataOutputCaller.update_information(ref playerBoxes[index], "sponsor-prefix", playerBoxes[index].getTeam());
        }

        private void character_Click(object sender, EventArgs e)
        {
            int index = (int)((Control)sender).Tag;
            StreamAssistantControlUpdates.update_character(ref playerBoxes[index]);
        }

        private void score_ValueChanged(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < playerBoxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (playerBoxes[i].score.Name == ((NumericUpDown)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            int current_point = (int)playerBoxes[player_index].score.Value;       //Pull the current game wins for the player

            //Keep the current point value at or below the match point value if image scoreboard is enabled
            if (current_point >= 3 && ImageManagement.enableImageScoreboard == true)
            {
                playerBoxes[player_index].score.Value = 3;
                current_point = 3;
            }

            //Check if automatic updates are enabled
            if (DataOutputCaller.automaticUpdates == true)
            {
                //Write the player's score to a file to be used by the stream program
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\score" + playerBoxes[player_index].player_number.ToString() + ".txt", current_point.ToString());
                DataOutputCaller.update_xml("match", 0, "score" + playerBoxes[player_index].player_number.ToString(), current_point.ToString());
                //Check if Image Scoreboard is enabled
                if (ImageManagement.enableImageScoreboard == true)
                {
                    //Store the location of the score image for the player used by the stream program
                    string score_file = DirectoryManagement.outputDirectory + @"\score" + playerBoxes[player_index].player_number.ToString() + ".png";

                    //Delete the score image if it exists
                    if (File.Exists(score_file))
                    {
                        File.Delete(score_file);
                    }

                    //Check the current value of the player's score
                    switch (current_point)
                    {
                        case 0:                     //Save an empty image for the player's score                                      
                            File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", score_file);
                            break;
                        default:                     //Copy the appropriate score image for the player's score
                            File.Copy(ImageManagement.scoreboardImages[playerBoxes[player_index].player_number - 1, current_point - 1], score_file);
                            break;
                    }
                }
            }
            else
            {
                playerBoxes[player_index].update.Enabled = true;
                playerBoxes[player_index].update.Image = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\blue.gif");    //Add a yellow glow to the update button
            }
        }
        #endregion Player Field Changes

        #region Swap Buttons

        private void btn_swap_Click(object sender, EventArgs e)
        {

            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            XDocument xml = XDocument.Load(xml_file);
            //Hold Player 1's information within temporary variables
            string hold_alt = txt_twitter1.Text;
            string hold_name = cbx_tag1.Text;
            decimal hold_score = nud_score1.Value;
            string hold_directory = playerBoxes[0].image_directory;
            string hold_team = txt_team1.Text;
            bool hold_L = ckb_loser1.Checked;
            Image hold_image = btn_character1.BackgroundImage;
            string hold_character = playerBoxes[0].characterName;
            int hold_color = playerBoxes[0].colorNumber;
            string hold_imageText = playerBoxes[0].character.Text;

            //Move Player 2's information to Player 1's slot
            cbx_tag1.Text = cbx_tag2.Text;
            txt_twitter1.Text = txt_twitter2.Text;
            nud_score1.Value = nud_score2.Value;
            playerBoxes[0].image_directory = playerBoxes[1].image_directory;
            ckb_loser1.Checked = ckb_loser2.Checked;
            txt_team1.Text = txt_team2.Text;
            btn_character1.BackgroundImage = btn_character2.BackgroundImage;
            playerBoxes[0].characterName = playerBoxes[1].characterName;
            playerBoxes[0].colorNumber = playerBoxes[1].colorNumber;
            playerBoxes[0].character.Text = playerBoxes[1].character.Text;

            //Move the information stored within temporary variables to Player 2's slot
            cbx_tag2.Text = hold_name;
            txt_twitter2.Text = hold_alt;
            nud_score2.Value = hold_score;
            playerBoxes[1].image_directory = hold_directory;
            ckb_loser2.Checked = hold_L;
            txt_team2.Text = hold_team;
            btn_character2.BackgroundImage = hold_image;
            playerBoxes[1].characterName = hold_character;
            playerBoxes[1].colorNumber = hold_color;
            playerBoxes[1].character.Text = hold_imageText;

            switch_xml("player-1", "player-2", xml);

        }

        private void btn_team_swap_Click(object sender, EventArgs e)
        {
            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            XDocument xml = XDocument.Load(xml_file);

            //Hold Player 1's information within temporary variables
            string hold_alt = txt_team1_twitter1.Text;
            string hold_name = cbx_team1_name1.Text;
            decimal hold_score = nud_team1_score.Value;
            string hold_directory = playerBoxes[2].image_directory;
            bool hold_L = ckb_team1_lose.Checked;
            string hold_team = txt_team1_team1.Text;
            Image hold_image = btn_team1_character1.BackgroundImage;
            string hold_character = playerBoxes[2].characterName;
            int hold_color = playerBoxes[2].colorNumber;
            string hold_imageText = playerBoxes[2].character.Text;

            //Move Player 2's information to Player 1's slot
            txt_team1_twitter1.Text = txt_team2_twitter1.Text;
            nud_team1_score.Value = nud_team2_score.Value;
            cbx_team1_name1.Text = cbx_team2_name1.Text;
            playerBoxes[2].image_directory = playerBoxes[3].image_directory;
            ckb_team1_lose.Checked = ckb_team2_lose.Checked;
            txt_team1_team1.Text = txt_team2_team1.Text;
            btn_team1_character1.BackgroundImage = btn_team2_character1.BackgroundImage;
            playerBoxes[2].characterName = playerBoxes[3].characterName;
            playerBoxes[2].colorNumber = playerBoxes[3].colorNumber;
            playerBoxes[2].character.Text = playerBoxes[3].character.Text;

            //Move the information stored within temporary variables to Player 2's slot
            cbx_team2_name1.Text = hold_name;
            txt_team2_twitter1.Text = hold_alt;
            nud_team2_score.Value = hold_score;
            playerBoxes[3].image_directory = hold_directory;
            ckb_team2_lose.Checked = hold_L;
            txt_team2_team1.Text = hold_team;
            btn_team2_character1.BackgroundImage = hold_image;
            playerBoxes[3].characterName = hold_character;
            playerBoxes[3].colorNumber = hold_color;
            playerBoxes[3].character.Text = hold_imageText;

            //Hold Player 1's information within temporary variables
            hold_name = cbx_team1_name2.Text;
            hold_alt = txt_team1_twitter2.Text;
            hold_directory = playerBoxes[4].image_directory;
            hold_team = txt_team1_team2.Text;
            hold_image = btn_team1_character2.BackgroundImage;
            hold_character = playerBoxes[4].characterName;
            hold_color = playerBoxes[4].colorNumber;
            hold_imageText = playerBoxes[4].character.Text;

            //Move Player 2's information to Player 1's slot
            cbx_team1_name2.Text = cbx_team2_name2.Text;
            txt_team1_twitter2.Text = txt_team2_twitter2.Text;
            playerBoxes[4].image_directory = playerBoxes[5].image_directory;
            txt_team1_team2.Text = txt_team2_team2.Text;
            btn_team1_character2.BackgroundImage = btn_team2_character2.BackgroundImage;
            playerBoxes[4].characterName = playerBoxes[5].characterName;
            playerBoxes[4].colorNumber = playerBoxes[5].colorNumber;
            playerBoxes[4].character.Text = playerBoxes[5].character.Text;

            //Move the information stored within temporary variables to Player 2's slot
            cbx_team2_name2.Text = hold_name;
            txt_team2_twitter2.Text = hold_alt;
            txt_team2_team2.Text = hold_team;
            playerBoxes[5].image_directory = hold_directory;
            btn_team2_character2.BackgroundImage = hold_image;
            playerBoxes[5].characterName = hold_character;
            playerBoxes[5].colorNumber = hold_color;
            playerBoxes[5].character.Text = hold_imageText;

            switch_xml("player-1", "player-2", xml);
            switch_xml("player-3", "player-4", xml);
        }

        private void btn_swapcommentators_Click(object sender, EventArgs e)
        {
            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            XDocument xml = XDocument.Load(xml_file);

            //Hold Commentator 1's information in temporary variables
            string holdTag = cbx_commentator_tag1.Text;
            string holdTwitter = txt_commentator_twitter1.Text;
            string holdTeam = txt_commentator_team1.Text;

            //Move Commentator 2's information to Commentator 1's fields
            cbx_commentator_tag1.Text = cbx_commentator_tag2.Text;
            txt_commentator_twitter1.Text = txt_commentator_twitter2.Text;
            txt_commentator_team1.Text = txt_commentator_team2.Text;

            //Move the information in the temporary variables to Commentator 2's fields
            cbx_commentator_tag2.Text = holdTag;
            txt_commentator_twitter2.Text = holdTwitter;
            txt_commentator_team2.Text = holdTeam;

            switch_xml("commentator-1", "commentator-2", xml);
        }

        #endregion Swap Buttons

        #region Match Settings Changes

        private void round_TextChanged(object sender, EventArgs e)
        {
            if (cbx_round.Text == "Grand Finals")
            {
                ckb_loser1.Enabled = true;
                ckb_loser1.Visible = true;
                ckb_loser2.Enabled = true;
                ckb_loser2.Visible = true;
                ckb_team1_lose.Enabled = true;
                ckb_team1_lose.Visible = true;
                ckb_team2_lose.Enabled = true;
                ckb_team2_lose.Visible = true;
            }
            else
            {
                ckb_loser1.Checked = false;
                ckb_loser1.Enabled = false;
                ckb_loser1.Visible = false;
                ckb_loser2.Checked = false;
                ckb_loser2.Enabled = false;
                ckb_loser2.Visible = false;
                ckb_team1_lose.Enabled = false;
                ckb_team1_lose.Visible = false;
                ckb_team1_lose.Checked = false;
                ckb_team2_lose.Enabled = false;
                ckb_team2_lose.Visible = false;
                ckb_team2_lose.Checked = false;
            }
            StreamAssistantControlUpdates.update_match_setting(updateButton, "round", ((ComboBox)sender).Text);
        }

        private void txt_tournament_TextChanged(object sender, EventArgs e)
        {
            StreamAssistantControlUpdates.update_match_setting(updateButton, "tournament", ((TextBox)sender).Text);
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("general").Element("tournament-name").ReplaceWith(new XElement("tournament-name", ((TextBox)sender).Text));
            xml.Save(SettingsFile.settingsFile);
        }

        private void txt_bracket_TextChanged(object sender, EventArgs e)
        {
            StreamAssistantControlUpdates.update_match_setting(updateButton, "bracketurl", ((TextBox)sender).Text);
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("general").Element("bracket-link").ReplaceWith(new XElement("bracket-link", ((TextBox)sender).Text));
            xml.Save(SettingsFile.settingsFile);
        }
        #endregion Match Settings Changed

        #region Update Buttons
        private void player_output(int player_index)
        {
            string number = playerBoxes[player_index].player_number.ToString();
            if (playerBoxes[player_index].isPlayer == true)
            {
                save_sponsor_image(playerBoxes[player_index].team.Text, player_index);
                string output_name = DataOutputCaller.get_output_name(ref playerBoxes[player_index]);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\playertag" + number + ".txt", output_name);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\playertwitter" + number + ".txt", playerBoxes[player_index].twitter.Text);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\score" + number + ".txt", playerBoxes[player_index].score.Value.ToString());
                Image stock_icon = Image.FromFile(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png");

                if (playerBoxes[player_index].characterName != "")
                {
                    stock_icon = Image.FromFile(playerBoxes[player_index].image_directory + @"\stock.png");
                }
                stock_icon.Save(DirectoryManagement.outputDirectory + @"\stockicon" + playerBoxes[player_index].player_number + ".png", System.Drawing.Imaging.ImageFormat.Png);
            }
            else
            {
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\commentatortag" + number + ".txt", playerBoxes[player_index].tag.Text);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\commentatortwitter" + number + ".txt", playerBoxes[player_index].twitter.Text);
            }
            string target = "player";
            if (playerBoxes[player_index].isPlayer == false)
                target = "commentator";

            //Update the xml file
            PlayerRecordModel output_player;
            if (playerBoxes[player_index].roster_number == -1)
            {
                output_player = new PlayerRecordModel(playerBoxes[player_index].tag.Text, playerBoxes[player_index].twitter.Text,
                                       playerBoxes[player_index].characterName, playerBoxes[player_index].colorNumber, PlayerRecordModel.defaultElo, "");
            }
            else
            {
                output_player = PlayerDatabase.playerRecords[playerBoxes[player_index].roster_number];
            }
            DataOutputCaller.update_xml(target, ref playerBoxes[player_index], output_player);
            if ((playerBoxes[player_index].player_number == 1 || playerBoxes[player_index].player_number == 2)&&playerBoxes[player_index].isPlayer == true)
                DataOutputCaller.update_xml("match", playerBoxes[player_index].player_number, "score" + number, playerBoxes[player_index].score.Value.ToString());
        }

        private void update_match(Button update_button, ComboBox round_box, NumericUpDown score1_box, NumericUpDown score2_box)
        {
            //Reset the image of the update button to default
            update_button.Image = null;

            List<NumericUpDown> score_box = new List<NumericUpDown> { score1_box, score2_box };

            //Output the player information
            if (update_button == btn_update)
            {
                player_output(0);
                player_output(1);
            }
            else
            {
                player_output(2);
                player_output(3);
                player_output(4);
                player_output(5);
                string team_value = DataOutputCaller.get_output_name(ref playerBoxes[2]) + " + " + DataOutputCaller.get_output_name(ref playerBoxes[4]);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name1.txt", team_value);
                team_value = DataOutputCaller.get_output_name(ref playerBoxes[3]) + " + " + DataOutputCaller.get_output_name(ref playerBoxes[5]);
                System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name2.txt", team_value);
            }

            //Save the Tournament information to seperate files to be used by the stream program
            System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\round.txt", round_box.Text);
            System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\bracketurl.txt", txt_bracket.Text);
            System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\tournament.txt", txt_tournament.Text);
            DataOutputCaller.update_xml("match", 0, "round", round_box.Text);
            DataOutputCaller.update_xml("match", 0, "bracketurl", txt_bracket.Text);
            DataOutputCaller.update_xml("match", 0, "tournament", txt_tournament.Text);
            DataOutputCaller.update_xml("match", 0, "date", txt_date.Text);

            switch (btn_update.Text)                        //Perform action based on the current text of the button
            {
                case "Start":                               //Start the match
                    score1_box.Enabled = true;              //Enable score control for Player 1
                    score2_box.Enabled = true;              //Enable score control for Player 2

                    update_button.Enabled = false;             //Disable this button until further action is needed
                    update_button.Text = "Update";             //Update the text of this button
                    ttp_tooltip.SetToolTip(update_button,
                        "Pushes updates to the player and match information " +
                        "into the Stream Files Directory.");

                    break;
                case "Update":                              //Update the stream files with the new information provided
                    update_button.Enabled = false;             //Disable this button until further action is needed
                    //Check if Image Scoreboard is enabled
                    if (ImageManagement.enableImageScoreboard == true)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            //Store the location of the score image for Player 1 used by the stream program
                            string score_file = DirectoryManagement.outputDirectory + @"\score" + (i + 1).ToString() + ".png";

                            //Delete the score image if it exists
                            if (File.Exists(score_file))
                            {
                                File.Delete(score_file);
                            }

                            //Check the current value of Player 1's score
                            switch (score_box[i].Value)
                            {
                                case 0:                     //Save an empty image for Player 1's score                                      
                                    File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", score_file);
                                    break;
                                default:                     //Copy the Player 1 Score 1 image for Player 1's score
                                    File.Copy(ImageManagement.scoreboardImages[i, (int)score_box[i].Value - 1], score_file);
                                    break;
                            }
                        }
                    }

                    break;
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            update_match(btn_update, cbx_round, nud_score1, nud_score2);
        }

        private void btn_update_commentators_Click(object sender, EventArgs e)
        {
            btn_update_commentators.Enabled = false;                //Disable the button until information is updated
            player_output(6);
            player_output(7);
        }

        private void btn_team_update_Click(object sender, EventArgs e)
        {
            update_match(btn_team_update, cbx_team_round, nud_team1_score, nud_team2_score);
        }

        private void save_Click(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int playerIndex = (int)((Button)sender).Tag;

            //Create a new player
            PlayerRecordModel saveNewPlayerRecord = new PlayerRecordModel();

            //Set a default character and color
            string originalCharacter = "Random";
            int originalColor = 1;

            //Check if a player in the roster has been selected from the combobox. 
            saveNewPlayerRecord = PlayerDatabase.FindRecordFromString(playerBoxes[playerIndex].tag.Text, PlayerDatabase.SearchProperty.uniqueTag);
            if (saveNewPlayerRecord != null)
            {
                playerBoxes[playerIndex].isPlayer = true;
            }
            else
            {
                playerBoxes[playerIndex].isPlayer = false;
            }

            //Character check. Perform only for players.
            if (playerBoxes[playerIndex].isPlayer == true)
            {
                originalCharacter = saveNewPlayerRecord.characterName;
                originalColor = saveNewPlayerRecord.colorNumber;
                saveNewPlayerRecord.characterName = playerBoxes[playerIndex].characterName;
                saveNewPlayerRecord.colorNumber = playerBoxes[playerIndex].colorNumber;
            }

            //Set its tag, twitter, sponsor, and region to the enterred information
            saveNewPlayerRecord.tag = playerBoxes[playerIndex].tag.Text; 
            saveNewPlayerRecord.sponsor = playerBoxes[playerIndex].team.Text;
            saveNewPlayerRecord.twitter = playerBoxes[playerIndex].twitter.Text;
            saveNewPlayerRecord.region = "";


            var savePlayerForm = new SavePlayer.SavePlayerForm(saveNewPlayerRecord, originalCharacter, originalColor);
            if (savePlayerForm.ShowDialog() == DialogResult.OK)
            {
                database_tools.add_player(savePlayerForm.outputPlayer, savePlayerForm.outputIsNewPlayer);
                PlayerDatabase.LoadPlayers(GlobalSettings.selectedGame);

                for (int i = 0; i < 6; i++)
                {
                    string holdTag = playerBoxes[i].tag.Text;
                    StreamAssistantControlUpdates.update_names(ref playerBoxes[i].tag);
                    if (i != playerIndex)
                    {
                        playerBoxes[i].tag.SelectedIndex = playerBoxes[i].tag.FindStringExact(holdTag);
                    }
                }
                playerBoxes[playerIndex].tag.SelectedIndex = playerBoxes[playerIndex].tag.FindStringExact(savePlayerForm.outputPlayer.uniqueTag);     //Set the combobox index to 0
            }
        }
        #endregion Update Buttons

        #region Loser Checkboxes and Reset Buttons

        private void loser_CheckedChanged(object sender, EventArgs e)
        {
            //Track the index of the associated player settings
            int player_index = -1;
            //Loop through the player settings array to locate the associated player settings
            for (int i = 0; i < playerBoxes.Length; i++)
            {
                //Compare the name of this ComboBox to the tag ComboBox of the current index
                if (playerBoxes[i].loser.Name == ((CheckBox)sender).Name)
                {
                    //This is the correct index of the player
                    player_index = i;
                    break;
                }
            }

            string output_name = DataOutputCaller.get_output_name(ref playerBoxes[player_index]);
            DataOutputCaller.update_xml("player", playerBoxes[player_index].player_number, "losers-side", playerBoxes[player_index].loser.Checked.ToString());
            System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\playertag" + playerBoxes[player_index].player_number.ToString() + ".txt", output_name);
            if (global_values.format == "Doubles")
            {
                if (playerBoxes[player_index].player_number == 1 || playerBoxes[player_index].player_number == 3)
                {
                    string team_value = DataOutputCaller.get_output_name(ref playerBoxes[2]) + " + " + DataOutputCaller.get_output_name(ref playerBoxes[4]);
                    System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name1.txt", team_value);
                }
                else
                {
                    string team_value = DataOutputCaller.get_output_name(ref playerBoxes[3]) + " + " + DataOutputCaller.get_output_name(ref playerBoxes[5]);
                    System.IO.File.WriteAllText(DirectoryManagement.outputDirectory + @"\team_name2.txt", team_value);
                }
            }
        }

        private void reset_Click(object sender, EventArgs e)
        {
            nud_score1.Value = 0;
            nud_score2.Value = 0;
            nud_team1_score.Value = 0;
            nud_team2_score.Value = 0;
        }

        #endregion Loser Checkboxes and Reset Buttons

        #region Match Advancement and Uploading

        private void change_match(int advancement)
        {
            XmlManagement.ResetXml(DirectoryManagement.outputDirectory, cbx_gamecount.Text);
            List<NumericUpDown> scorebox = new List<NumericUpDown> { nud_score1, nud_score2, nud_team1_score, nud_team2_score };
            List<Button> update_button = new List<Button> { btn_update, btn_team_update };
            List<Button> upload_button = new List<Button> { btn_upload, btn_team_upload };
            //Reset all image files
            ImageManagement.ResetAllImages(DirectoryManagement.outputDirectory);
            foreach (Button upload in upload_button)
            {
                if (YoutubeController.enableYoutubeFunctions == true)
                {
                    global_values.reenable_upload = "";
                    upload.Enabled = true;
                    upload.Text = uploadButtonText;
                }
            }
            foreach (NumericUpDown score in scorebox)
            {
                score.Value = 0;
                score.Enabled = false;
            }
            foreach (Button update in update_button)
            {
                update.Text = "Start";
                ttp_tooltip.SetToolTip(update,
                    "Click to begin the set and enable score control.\n" +
                    "Pushes player and match information into the\n" +
                    "Stream Files Directory.");
                update.Image = null;
                update.Enabled = true;
            }

            List<ComboBox> tags = new List<ComboBox> { cbx_tag1, cbx_tag2, cbx_team1_name1, cbx_team1_name2, cbx_team2_name1, cbx_team2_name2 };
            List<TextBox> twitters = new List<TextBox> { txt_twitter1, txt_twitter2, txt_team1_twitter1, txt_team1_twitter2, txt_team2_twitter1, txt_team2_twitter2 };
            List<TextBox> teams = new List<TextBox> { txt_team1, txt_team2, txt_team1_team1, txt_team1_team2, txt_team2_team1, txt_team2_team2 };
            List<Button> characters = new List<Button> { btn_character1, btn_character2, btn_team1_character1, btn_team1_character2, btn_team2_character1, btn_team2_character2 };

            foreach (ComboBox tag in tags)
            {
                tag.Text = "";
                tag.SelectedIndex = -1;
            }
            foreach (TextBox twitter in twitters)
                twitter.Text = "";
            foreach (TextBox team in teams)
                team.Text = "";
            foreach (Button character in characters)
            {
                character.BackgroundImage = null;
                character.Text = "Click to Select a Character";
            }

            for (int i = 0; i < 6; i++)
            {
                playerBoxes[i].image_directory = DirectoryManagement.GetGameDirectory() + @"\" + playerBoxes[i].characterName + @"\" + (playerBoxes[i].colorNumber).ToString() + @"\";
                playerBoxes[i].characterName = "";
                playerBoxes[i].colorNumber = -1;
            }

            for (int i = 0; i < playerBoxes.Length; i++)
            {
                playerBoxes[i].roster_number = -1;
            }

            //Load the players
            PlayerDatabase.LoadPlayers(GlobalSettings.selectedGame);

            if (global_values.format == "Singles")
            {
                StreamAssistantControlUpdates.update_names(ref cbx_tag1);
                StreamAssistantControlUpdates.update_names(ref cbx_tag2);
            }
            else
            {
                StreamAssistantControlUpdates.update_names(ref cbx_team1_name1);
                StreamAssistantControlUpdates.update_names(ref cbx_team1_name2);
                StreamAssistantControlUpdates.update_names(ref cbx_team2_name1);
                StreamAssistantControlUpdates.update_names(ref cbx_team2_name2);
            }
            //Hold the commentator name
            string hold_name = cbx_commentator_tag1.Text;
            //Update the names for commentator box
            StreamAssistantControlUpdates.update_names(ref cbx_commentator_tag1);
            //Restore the commentator name
            cbx_commentator_tag1.Text = hold_name;
            //Repeat the process for the second commentator name
            hold_name = cbx_commentator_tag2.Text;
            StreamAssistantControlUpdates.update_names(ref cbx_commentator_tag2);
            cbx_commentator_tag2.Text = hold_name;


            if (global_values.queue_id > -1)
            {
                if (load_queue(advancement) == false)
                {
                    //Reset the characters for every box since no players were loaded in for the next match
                    for (int i = 0; i < playerBoxes.Length; i++)
                    {
                        if (playerBoxes[i].isPlayer == true)
                        {
                            if ((i < 2 && global_values.format == "Singles") ||
                                    (i > 1 && global_values.format == "Doubles"))
                            {
                                //control_updates.refreshCharacterBox(ref player_boxes[i].character, global_values.characters);
                            }
                        }
                    }
                    cbx_round.Text = "";
                    cbx_team_round.Text = "";
                }
            }
        }

        private void next_match_Click(object sender, EventArgs e)
        {
            change_match(1);
        }

        private void previous_match_Click(object sender, EventArgs e)
        {
            change_match(-1);
        }

        private void upload_Click(object sender, EventArgs e)
        {
            Button upload_button = (Button)sender;
            string thumbnail_image_name = "";
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            string video_title = (string)xml.Root.Element("youtube").Element("title-template"); ;
            if (sender == btn_upload)
            {
                if (YoutubeController.enableVideoThumbnails == true)
                {
                    string[] playerImages = new string[2] { playerBoxes[0].image_directory + @"\1080.png", playerBoxes[1].image_directory + @"\1080.png" };
                    thumbnail_image_name = ImageManagement.CreateThumbnailImage(2,
                        cbx_tag1.Text,
                        cbx_tag2.Text,
                        cbx_round.Text,
                        txt_date.Text,
                        playerImages);
                }

                video_title = video_title.Replace("*tournament*", txt_tournament.Text)
                         .Replace("*round*", cbx_round.Text)
                         .Replace("*date*", txt_date.Text)
                         .Replace("*bracket*", txt_bracket.Text)
                         .Replace("*player1*", cbx_tag1.Text)
                         .Replace("*player2*", cbx_tag2.Text)
                         .Replace("*character1*", playerBoxes[0].characterName)
                         .Replace("*character2*", playerBoxes[1].characterName);

                //video_title = shorten_title(video_title);
                if (video_title.Length > 100)
                {
                    video_title = txt_tournament.Text + @"-" + cbx_team_round.Text + @"-" +
                        cbx_tag1.Text + " Vs. " +
                        cbx_tag2.Text;
                    video_title = shorten_title(video_title);
                }
            }
            else
            {
                string team_name1 = cbx_team1_name1.Text + " + " + cbx_team1_name2.Text;
                string team_name2 = cbx_team2_name1.Text + " + " + cbx_team2_name2.Text;
                string characters1 = playerBoxes[2].characterName + "+" + playerBoxes[4].characterName;
                string characters2 = playerBoxes[3].characterName + "+" + playerBoxes[5].characterName;
                if (YoutubeController.enableVideoThumbnails == true)
                {
                    string[] playerImages = new string[4] { playerBoxes[2].image_directory + @"\1080.png", playerBoxes[3].image_directory + @"\1080.png",
                                                            playerBoxes[4].image_directory + @"\1080.png", playerBoxes[5].image_directory + @"\1080.png"};

                    thumbnail_image_name = ImageManagement.CreateThumbnailImage(4,
                        team_name1,
                        team_name2,
                        cbx_team_round.Text,
                        txt_date.Text,
                        playerImages);
                }


                video_title = video_title.Replace("*tournament*", txt_tournament.Text)
                     .Replace("*round*", cbx_round.Text)
                     .Replace("*date*", txt_date.Text)
                     .Replace("*bracket*", txt_bracket.Text)
                     .Replace("*player1*", team_name1)
                     .Replace("*player2*", team_name2)
                     .Replace("*character1*", characters1)
                     .Replace("*character2*", characters2);
                video_title = shorten_title(video_title);
                if (video_title.Length > 100)
                {
                    video_title = txt_tournament.Text + @"-" + cbx_team_round.Text + @"-" +
                        team_name1 + " Vs. " +
                        team_name2;
                    video_title = shorten_title(video_title);
                }
            }

            if (YoutubeController.enableYoutubeFunctions == false)
            {
                string message_text = "";
                if (YoutubeController.enableVideoThumbnails == true)
                {
                    message_text = "A thumbnail image has been generated.\r\n";
                }
                if (YoutubeController.copyVideoTitle == true)
                {
                    Clipboard.SetText(video_title);
                    message_text += "Video title copied to clipboard: \n" + video_title;
                }
                MessageBox.Show(message_text);
            }
            else
            {
                //Pass the event upload_form_enable_button_event() to the new form as the function "enable_button()"
                upload_button.Text = "Upload Window Open";
                upload_button.Enabled = false;             //Disable this button until further action is needed
                global_values.reenable_upload = DateTime.Now.ToString("MMddyyyyHHmmss");   //Set the flag to allow the button to be re-abled on form close

                if (YoutubeController.copyVideoTitle == true)
                {
                    Clipboard.SetText(video_title);
                    MessageBox.Show("Video title copied to clipboard: \n" + video_title);
                }

                string video_description = get_video_description();

                //Create a new form and provide it with a Video title based off the provided information,
                //as well as a description and the thumbnail image created
                var upload_form = new frm_uploading(video_title,
                    video_description,
                    DirectoryManagement.thumbnailDirectory + @"\" + thumbnail_image_name,
                    YoutubeController.vodMonitor.GetDetectedVod(),
                    global_values.reenable_upload, false);
                upload_form.enable_button += new enable_button_event(upload_form_enable_button_event);
                upload_form.Show();                     //Show the form        
            }
        }

        private string shorten_title(string long_title)
        {
            switch (YoutubeController.enableVideoTitleShortening)
            {
                case YoutubeController.VideoTitleOptions.doublesOnly:
                    if (global_values.format == "Doubles")
                    {
                        goto case YoutubeController.VideoTitleOptions.alwaysShorten;
                    }
                    return long_title;
                case YoutubeController.VideoTitleOptions.alwaysShorten:
                    string short_title = long_title.Replace("Captain Falcon", "C.Falcon")
                        .Replace("Pokemon Trainer", "P.Trainer")
                        .Replace("King K Rool", "K Rool")
                        .Replace("King Dedede", "Dedede")
                        .Replace("Mii ", "")
                        .Replace("Mr Game and Watch", "G&W")
                        .Replace("Zero Suit Samus", "ZSS")
                        .Replace("Wii Fit Trainer", "WFT")
                        .Replace("Young ", "Y.")
                        .Replace("Toon ", "T.")
                        .Replace("Jigglypuff", "Puff")
                        .Replace("Bayonetta", "Bayo")
                        .Replace("Donkey Kong", "DK")
                        .Replace("Diddy Kong", "Diddy")
                        .Replace("Dr Mario", "Doc")
                        .Replace("Ice Climbers", "ICs")
                        .Replace("Little Mac", "Mac")
                        .Replace("Meta Knight", "MK")
                        .Replace(" and Kazooie", "")
                        .Replace(" and Luma", "")
                        .Replace("Pyra and Mythra", "Aegis");
                    return short_title;
                case YoutubeController.VideoTitleOptions.neverShorten:
                    return long_title;
            }
            return long_title;
        }
        #endregion Match Advancement and Uploading

        //This function is passed to frm_upload to control the update button on the main form
        void upload_form_enable_button_event(string check_reenable)
        {
            //Check if the reenable value from the upload form matches the current value stored
            if (check_reenable == global_values.reenable_upload)
            {
                btn_upload.Enabled = true;                  //Re-enable the button
                btn_upload.Text = uploadButtonText;
            }
        }

        private string get_video_description()
        {
            string description = YoutubeController.videoDescription;
            switch (global_values.format)
            {
                case "Singles":
                    description = description.Replace("*tournament*", txt_tournament.Text)
                                             .Replace("*round*", cbx_round.Text)
                                             .Replace("*date*", txt_date.Text)
                                             .Replace("*bracket*", txt_bracket.Text)
                                             .Replace("*player1*", cbx_tag1.Text)
                                             .Replace("*player2*", cbx_tag2.Text)
                                             .Replace("*twitter1*", txt_twitter1.Text)
                                             .Replace("*twitter2*", txt_twitter2.Text)
                                             .Replace("*character1*", playerBoxes[0].characterName)
                                             .Replace("*character2*", playerBoxes[1].characterName);
                    break;
                case "Doubles":
                    description = description.Replace("*tournament*", txt_tournament.Text)
                                             .Replace("*round*", cbx_round.Text)
                                             .Replace("*date*", txt_date.Text)
                                             .Replace("*bracket*", txt_bracket.Text)
                                             .Replace("*player1*", cbx_team1_name1.Text)
                                             .Replace("*player2*", cbx_team2_name1.Text)
                                             .Replace("*twitter1*", txt_team1_twitter1.Text)
                                             .Replace("*twitter2*", txt_team2_twitter1.Text)
                                             .Replace("*character1*", playerBoxes[2].characterName)
                                             .Replace("*character2*", playerBoxes[3].characterName)
                                             .Replace("*player3*", cbx_team1_name2.Text)
                                             .Replace("*player4*", cbx_team2_name2.Text)
                                             .Replace("*twitter3*", txt_team1_twitter2.Text)
                                             .Replace("*twitter4*", txt_team2_twitter2.Text)
                                             .Replace("*character3*", playerBoxes[4].characterName)
                                             .Replace("*character4*", playerBoxes[5].characterName);
                    break;
            }
            return description;
        }

        /// <summary>
        /// Checks to see if the entered tag has a sponsor, and saves the sponsor logo in the output directory.
        /// </summary>
        /// <param name="raw_name">The entered tag</param>
        /// <param name="player_index">The index of the player field modified</param>
        private void save_sponsor_image(string raw_name, int player_index)
        {
            int player_number = playerBoxes[player_index].player_number;
            string image_file = @"\sponsor" + player_number.ToString() + ".png";

            if (File.Exists(DirectoryManagement.outputDirectory + image_file))
            {
                File.Delete(DirectoryManagement.outputDirectory + image_file);
            }
            File.Copy(Directory.GetCurrentDirectory() + @"\Resources\Graphics\left.png", DirectoryManagement.outputDirectory + image_file);
            if (ImageManagement.enableSponsorImages == true)
            {
                if (raw_name != "")
                {
                    string sponsorfile = DirectoryManagement.sponsorDirectory + @"\" + raw_name + @".png";
                    if (File.Exists(sponsorfile))
                    {
                        Image sponsor_image = Image.FromFile(sponsorfile);
                        sponsor_image.Save(DirectoryManagement.outputDirectory + @"\sponsor" + player_number.ToString() + ".png", System.Drawing.Imaging.ImageFormat.Png);
                    }
                }
            }
        }
        private bool load_queue(int advancement)
        {
            //Load the queue from the database
            List<QueueEntryModel> loaded_queue = database_tools.load_queue(global_values.queue_id, false);
            if (loaded_queue.Count() == 0)
                return false;
            int next_match = -1;
            int current_match = -1;
            int new_match = -1;
            //Cycle through the queue to find the current match and the pushed match
            for (int i = 0; i < loaded_queue.Count(); i++)
            {
                switch (loaded_queue[i].matchStatus)
                {
                    case QueueEntryModel.status.currentMatch:
                        current_match = i;              //This is the current match
                        break;
                    case QueueEntryModel.status.nextMatch:
                    case QueueEntryModel.status.currentNextMatch:
                        next_match = i;                 //This match is being pushed to stream
                        break;
                    default:

                        break;
                }
                //Reset every match's status
                loaded_queue[i].matchStatus = 0;
            }

            if (next_match != -1)
            {
                //If a match is being pushed to stream, mark it as the next match
                loaded_queue[next_match].matchStatus = QueueEntryModel.status.currentMatch;
                new_match = next_match;
            }
            else
            {
                //Find the new match and advance to it so long as it exists
                //Otherwise exit this function
                if (current_match != -1)
                {
                    int current_match_number = loaded_queue[current_match].positionInQueue;
                    //Check if the match advancing to is outside the queue
                    if (current_match_number + advancement <= 0 || current_match_number + advancement > loaded_queue.Count())
                    {
                        //Mark the match as outside the queue
                        outside_queue = advancement;
                        return false;
                    }
                    else
                    {
                        int new_match_number = current_match_number + advancement;
                        //Check if the previous match was outside of the queue and move it to the edge of the queue
                        if (outside_queue != advancement)
                            new_match_number += outside_queue;
                        for (int i = 0; i < loaded_queue.Count(); i++)
                        {
                            if (loaded_queue[i].positionInQueue == new_match_number)
                            {
                                new_match = i;
                                loaded_queue[new_match].matchStatus = QueueEntryModel.status.currentMatch;
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < loaded_queue.Count(); i++)
                    {
                        if (loaded_queue[i].positionInQueue == 1)
                        {
                            current_match = i;
                            new_match = i;
                            loaded_queue[new_match].matchStatus = QueueEntryModel.status.currentMatch;
                        }
                    }
                }
                if (new_match < 0 || new_match >= loaded_queue.Count())
                {
                    if (new_match < 0)
                        outside_queue = -1;
                    else
                        outside_queue = 1;
                    return false;
                }
            }

            outside_queue = 0;
            //Update the previous current match and the new current match
            if (new_match != -1)
            {
                database_tools.add_match(loaded_queue[new_match], false);
            }
            database_tools.add_match(loaded_queue[current_match], false);

            //Assign each player's information
            //Cycle through each player in the match infomation
            for (int i = 0; i < 4; i++)
            {
                //Cycle through each box group
                for (int ii = 0; ii < playerBoxes.Length; ii++)
                {
                    //Check if the box group belongs to a player
                    if (playerBoxes[ii].isPlayer == true &&
                        playerBoxes[ii].player_number == i + 1)
                    {
                        if ((ii < 2 && global_values.format == "Singles") ||
                            (ii > 1 && global_values.format == "Doubles"))
                        {
                            playerBoxes[ii].tag.Text = PlayerDatabase.GetUniqueTagFromId(loaded_queue[new_match].playerNames[i]);
                        }
                    }
                }
            }

            cbx_round.Text = loaded_queue[new_match].roundInBracket;
            cbx_team_round.Text = loaded_queue[new_match].roundInBracket;
            return true;
        }

        private void frm_main_FormClosed(object sender, FormClosedEventArgs e)
        {
            close_form(0);
        }

        private void switch_xml(string first_element, string second_element, XDocument xml_copy)
        {
            string xml_file = DirectoryManagement.outputDirectory + @"\MasterOrdersOutput.xml";
            XDocument xml = XDocument.Load(xml_file);

            XElement element_1 = xml_copy.Root.Element(first_element);
            XElement element_2 = xml_copy.Root.Element(second_element);

            xml.Root.Element(first_element).ReplaceAll(from el in element_2.Elements() select new XElement(el.Name, el.Value));
            xml.Root.Element(second_element).ReplaceAll(from el in element_1.Elements() select new XElement(el.Name, el.Value));
            xml.Save(xml_file);

        }



        private void rdb_automatic_CheckedChanged(object sender, EventArgs e)
        {
            DataOutputCaller.automaticUpdates = rdb_automatic.Checked;
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("general").Element("automatic-updates").ReplaceWith(new XElement("automatic-updates", DataOutputCaller.automaticUpdates));
            xml.Save(SettingsFile.settingsFile);
        }

        private void cbx_queues_SelectedIndexChanged(object sender, EventArgs e)
        {
            string queuegame = "";
            int queueid = -1;
            if (cbx_queues.SelectedIndex == 0)
            {
                queueid = -1;
            }
            else
            {
                for (int i = 1; i < StreamQueue.queueList.Count; i++)
                {
                    if (StreamQueue.queueList[i].queueName == cbx_queues.Text)
                    {
                        queuegame = StreamQueue.queueList[i].queueGame;
                        queueid = i;
                    }
                }
            }
            if (queueid != -1)
            {
                //Compare the current queue's game to the new one. Update if there is a difference.
                if (queueid != 0)
                {
                    if (StreamQueue.queueList[global_values.queue_id].queueGame != queuegame)
                    {
                        if (database_tools.regame_queue(cbx_queues.Text, queuegame, queueid) == false)
                        {
                            cbx_queues.SelectedIndex = cbx_queues.FindStringExact(StreamQueue.queueList[global_values.queue_id].queueName);
                            return;
                        }
                    }
                }

                //Update the queue ID and write it to settings
                global_values.queue_id = queueid;

                XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                xml.Root.Element("database").Element("queue-id").ReplaceWith(new XElement("queue-id", global_values.queue_id));
                xml.Save(SettingsFile.settingsFile);


                if (global_values.bracket_assistant != null)
                {
                    global_values.bracket_assistant.reload_settings();
                }
            }
        }

        private void cbx_format_SelectedIndexChanged(object sender, EventArgs e)
        {
            global_values.format = cbx_format.Text;

            switch (StreamAssistantControlUpdates.UpdateFormat(global_values.format, ref tab_main, ref tab_ingame_display, ref tab_doubles_display))
            {
                case "Singles":
                    updateButton = btn_update;
                    break;
                case "Doubles":
                    updateButton = btn_team_update;
                    break;
            }


            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("general").Element("format").ReplaceWith(new XElement("format", global_values.format));
            xml.Save(SettingsFile.settingsFile);
        }

        private void txt_playlist_TextChanged(object sender, EventArgs e)
        {
            if (txt_playlist.Text != YoutubeController.playlistName)
            {
                btn_playlist.Enabled = true;
            }
            else
            {
                btn_playlist.Enabled = false;
            }
        }

        #region Playlist Handling
        private void btn_playlist_Click(object sender, EventArgs e)
        {
            btn_playlist.Enabled = false;
            txt_playlist.Enabled = false;
            try
            {
                Thread thead = new Thread(() =>
                {
                    get_playlists().Wait();
                });
                thead.IsBackground = true;
                thead.Start();

            }
            catch (AggregateException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async Task get_playlists()
        {
            playlist_items = new List<string>();
            playlist_names = new List<string>();
            await get_credential();



            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = global_values.youtubeCredential,
                ApplicationName = this.GetType().ToString()
            });

            // var channelsListRequest = youtubeService.Channels.List("contentDetails");
            var playlistListRequest = youtubeService.Playlists.List("snippet");
            playlistListRequest.Mine = true;

            // Retrieve the contentDetails part of the channel resource for the authenticated user's channel.
            var playlistListResponse = await playlistListRequest.ExecuteAsync();

            foreach (var playlist in playlistListResponse.Items)
            {
                // From the API response, extract the playlist ID that identifies the list
                // of videos uploaded to the authenticated user's channel.
                var playlistListId = playlist.Id;
                playlist_items.Add(playlistListId);
                playlist_names.Add(playlist.Snippet.Title);
            }

            check_playlists();
        }

        delegate void check_playlists_callback();

        private void check_playlists()
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.txt_playlist.InvokeRequired)
            {
                check_playlists_callback d = new check_playlists_callback(check_playlists);
                this.Invoke(d, new object[] { });
            }
            else
            {
                if (playlist_names.Contains(txt_playlist.Text))
                {
                    YoutubeController.playlistId = playlist_items[playlist_names.IndexOf(txt_playlist.Text)];
                    MessageBox.Show("The playlist has been set to " + txt_playlist.Text + ". \n" +
                                    "The playlist ID is " + YoutubeController.playlistId);
                    txt_playlist.Enabled = true;
                    YoutubeController.playlistName = txt_playlist.Text;
                    XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                    xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", YoutubeController.playlistName));
                    xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", YoutubeController.playlistId));
                    xml.Save(SettingsFile.settingsFile);
                }
                else
                {
                    if (txt_playlist.Text == "")
                    {
                        MessageBox.Show("Playlist usage has been disabled.");

                        YoutubeController.playlistName = "";
                        YoutubeController.playlistId = "";
                        XDocument xml = XDocument.Load(SettingsFile.settingsFile);
                        xml.Root.Element("youtube").Element("playlist-name").ReplaceWith(new XElement("playlist-name", YoutubeController.playlistName));
                        xml.Root.Element("youtube").Element("playlist-id").ReplaceWith(new XElement("playlist-id", YoutubeController.playlistId));
                        xml.Save(SettingsFile.settingsFile);

                    }
                    else
                    {
                        if (MessageBox.Show("A playlist with the name '" + txt_playlist.Text + "' has not been found. Create a new playlist?", "No Playlist Found", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            try
                            {
                                Thread thead = new Thread(() =>
                                {
                                    add_playlist(txt_playlist.Text).ContinueWith(task => get_playlists());
                                });
                                thead.IsBackground = true;
                                thead.Start();

                            }
                            catch (AggregateException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            btn_playlist.Enabled = true;
                            txt_playlist.Enabled = true;
                        }
                    }
                }

            }

        }

        private async Task add_playlist(string new_playlist)
        {
            await get_credential();

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = global_values.youtubeCredential,
                ApplicationName = this.GetType().ToString()
            });

            // Create a new, private playlist in the authorized user's channel.
            var newPlaylist = new Playlist();
            newPlaylist.Snippet = new PlaylistSnippet();
            newPlaylist.Snippet.Title = new_playlist;
            newPlaylist.Snippet.Description = "";
            newPlaylist.Status = new PlaylistStatus();
            newPlaylist.Status.PrivacyStatus = "public";

            newPlaylist = await youtubeService.Playlists.Insert(newPlaylist, "snippet,status").ExecuteAsync();
        }

        private async Task get_credential()
        {
            try
            {
                if (global_values.youtubeCredential == null)
                    using (var stream = new FileStream(YoutubeController.jsonFile, FileMode.Open, FileAccess.Read))
                    {
                        global_values.youtubeCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                            GoogleClientSecrets.Load(stream).Secrets,
                            // This OAuth 2.0 access scope allows for read-only access to the authenticated 
                            // user's account, but not other types of account access.
                            new[] { YouTubeService.Scope.Youtube,
                        YouTubeService.Scope.YoutubeUpload },
                            global_values.youtube_username,
                            CancellationToken.None,
                            global_values.store
                        );
                    }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show(ex.ToString());

            }
        }

        #endregion Playlist Handling

        private void cbx_gamecount_SelectedIndexChanged(object sender, EventArgs e)
        {
            StreamAssistantControlUpdates.update_match_setting(updateButton, "format", ((ComboBox)sender).Text);
        }

        private void Txt_message1_TextChanged(object sender, EventArgs e)
        {
            DataOutputCaller.update_xml("match", 1, "message-1", txt_message1.Text);
        }

        private void Txt_message2_TextChanged(object sender, EventArgs e)
        {
            DataOutputCaller.update_xml("match", 1, "message-2", txt_message2.Text);
        }

        private void AddTabEvents()
        {
            txt_team1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_tag1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_tag2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_twitter1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_twitter2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            nud_score1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            nud_score2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_round.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_character1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_character2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team1_team1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team1_team2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team2_team1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team2_team2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_team1_name1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_team1_name2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team1_twitter1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team1_twitter2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_team1_character1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_team1_character2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_team2_name1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_team2_name2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team2_twitter1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_team2_twitter2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_team2_character1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            btn_team2_character2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            nud_team1_score.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            nud_team2_score.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            ckb_loser1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            ckb_loser2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            ckb_team1_lose.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            ckb_team2_lose.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_tournament.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_date.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_bracket.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_commentator_tag1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_commentator_twitter1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_commentator_team1.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            cbx_commentator_tag2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_commentator_twitter2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
            txt_commentator_team2.KeyDown += new KeyEventHandler(StreamAssistantControlUpdates.tab_Next);
        }

        private PlayerSettings[] InitializeSettings()
        {
            PlayerSettings[] playerSettings = new PlayerSettings[8];

            //Set the player fields to the default values
            playerSettings[0] = new PlayerSettings(0, 1, cbx_tag1, txt_twitter1, btn_character1, btn_update, nud_score1, ckb_loser1, btn_save1, txt_team1, 82);
            playerSettings[1] = new PlayerSettings(1, 2, cbx_tag2, txt_twitter2, btn_character2, btn_update, nud_score2, ckb_loser2, btn_save2, txt_team2, 82);
            playerSettings[2] = new PlayerSettings(2, 1, cbx_team1_name1, txt_team1_twitter1, btn_team1_character1, btn_team_update, nud_team1_score, ckb_team1_lose, btn_save1, txt_team1_team1, 40);
            playerSettings[3] = new PlayerSettings(3, 2, cbx_team2_name1, txt_team2_twitter1, btn_team2_character1, btn_team_update, nud_team2_score, ckb_team2_lose, btn_save2, txt_team1_team2, 40);
            playerSettings[4] = new PlayerSettings(4, 3, cbx_team1_name2, txt_team1_twitter2, btn_team1_character2, btn_team_update, nud_team1_score, ckb_team1_lose, btn_save1, txt_team2_team1, 40);
            playerSettings[5] = new PlayerSettings(5, 4, cbx_team2_name2, txt_team2_twitter2, btn_team2_character2, btn_team_update, nud_team2_score, ckb_team2_lose, btn_save2, txt_team2_team2, 40);
            playerSettings[6] = new PlayerSettings(6, false, 1, cbx_commentator_tag1, txt_commentator_twitter1, btn_update_commentators, txt_commentator_team1, btn_commentator_save1);
            playerSettings[7] = new PlayerSettings(7, false, 2, cbx_commentator_tag2, txt_commentator_twitter2, btn_update_commentators, txt_commentator_team2, btn_commentator_save2);

            return playerSettings;
        }

        private void SetStartingSettings()
        {
            rdb_automatic.Checked = DataOutputCaller.automaticUpdates;
            cbx_format.Text = global_values.format;
            cbx_gamecount.SelectedIndex = 0;

            //Add Queues to the dropdown
            StreamAssistantControlUpdates.UpdateQueues(cbx_queues, StreamQueue.queueList, global_values.queue_id);

            if (YoutubeController.enableYoutubeFunctions == true)
            {
                txt_playlist.Enabled = true;
                txt_playlist.Text = YoutubeController.playlistName;
            }

        }

        private void IntializeOutputXml()
        {
            XmlManagement.ResetXml(DirectoryManagement.outputDirectory, cbx_gamecount.Text);

            //Load the tournament name and bracket url
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            txt_tournament.Text = (string)xml.Root.Element("general").Element("tournament-name");
            txt_bracket.Text = (string)xml.Root.Element("general").Element("bracket-link");
        }

        private void ConfigureUpdateButton()
        {
            switch (StreamAssistantControlUpdates.UpdateFormat(global_values.format, ref tab_main, ref tab_ingame_display, ref tab_doubles_display))
            {
                case "Singles":
                    updateButton = btn_update;
                    btn_upload.Text = uploadButtonText;
                    break;
                case "Doubles":
                    updateButton = btn_team_update;
                    btn_team_upload.Text = uploadButtonText;
                    break;
            }
        }

        private void cbx_tag2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}