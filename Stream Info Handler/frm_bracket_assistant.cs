using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_bracket_assistant : Form
    {
        public event closedform_event close_form;

        //AutoSeed
        ListViewItem draggedItem;
        List<tournament_player> seeded_players;


        //Define match status colors
        Color current_match_color = Color.FromArgb(138, 240, 146);
        Color next_match_color = Color.FromArgb(234, 153, 153);
        Color dual_match_color = Color.FromArgb(202, 182, 151);

        //Hold the number of players
        int numberOfPlayers;

        //Hold the player IDs
        string[] selected_id = { "", "", "", "" };

        //Initialize a list containing the queue information
        List<QueueEntryModel> loaded_queue;

        private System.Windows.Forms.Timer refresh_queue;
        public void init_refresh()
        {
            refresh_queue = new System.Windows.Forms.Timer();
            refresh_queue.Tick += new EventHandler(queue_Tick);
            refresh_queue.Interval = 12000; // in miliseconds
            refresh_queue.Start();
        }
        private void queue_Tick(object sender, EventArgs e)
        {
            load_queue(0, true);
        }

        public frm_bracket_assistant()
        {
            this.CenterToScreen();

            InitializeComponent();

            this.TopMost = global_values.keepWindowsOnTop;

            string[] importedRounds = SettingsFile.LoadBracketRounds();
            cbx_round.Items.Clear();
            cbx_round.BeginUpdate();
            cbx_round.Items.AddRange(importedRounds);
            cbx_round.EndUpdate();


            reload_settings();
            init_refresh();
            init_autoseed();
        }
        #region Queue Manager

        public void reload_settings()
        {
            if (global_values.format == "Singles")
            {
                lbl_player1.Text = "Player 1";
                lbl_player2.Text = "Player 2";
                lvw_playerinfo.Columns[3].Width = 0;
                lvw_playerinfo.Columns[4].Width = 0;
                lvw_matches.Columns[4].Width = 0;
                lvw_matches.Columns[5].Width = 0;
                lvw_playerinfo.Width = 428;
                cbx_player3.Enabled = false;
                cbx_player3.Visible = false;
                cbx_player4.Enabled = false;
                cbx_player4.Visible = false;
                btn_player3.Enabled = false;
                btn_player3.Visible = false;
                btn_player4.Enabled = false;
                btn_player4.Visible = false;

                lbl_bracket.Top = 36;
                lbl_player1.Top = 36;
                lbl_player2.Top = 36;
                cbx_round.Top = 52;
                cbx_player1.Top = 52;
                cbx_player2.Top = 52;
                btn_add.Top = 52;

                numberOfPlayers = 2;
            }
            else
            {
                lbl_player1.Text = "Team 1";
                lbl_player2.Text = "Team 2";
                lvw_playerinfo.Columns[3].Width = 170;
                lvw_playerinfo.Columns[4].Width = 170;
                lvw_matches.Columns[4].Width = 191;
                lvw_matches.Columns[5].Width = 191;
                lvw_playerinfo.Width = 767;
                cbx_player3.Enabled = true;
                cbx_player3.Visible = true;
                cbx_player4.Enabled = true;
                cbx_player4.Visible = true;
                btn_player3.Enabled = true;
                btn_player3.Visible = true;
                btn_player4.Enabled = true;
                btn_player4.Visible = true;

                lbl_bracket.Top = 16;
                lbl_player1.Top = 16;
                lbl_player2.Top = 16;
                cbx_round.Top = 32;
                cbx_player1.Top = 32;
                cbx_player2.Top = 32;
                btn_add.Top = 32;

                numberOfPlayers = 4;
            }

            load_queue(0, false);
        }

        public void load_queue(int differential, bool is_monitor)
        {


            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            loaded_queue = database_tools.load_queue(global_values.queue_id, is_monitor);
            QueueEntryModel[] order_queue = new QueueEntryModel[loaded_queue.Count];
            loaded_queue.CopyTo(order_queue);
            for (int i = 0; i < loaded_queue.Count; i++)
            {
                int check_number = order_queue[i].number - 1;
                loaded_queue[check_number] = order_queue[i];
            }

            //Hold the ID of each player entered
            string[] hold_player_name = { cbx_player1.Text, cbx_player2.Text, cbx_player3.Text, cbx_player4.Text };


            PlayerDatabase.LoadPlayers(GlobalSettings.selectedGame);

            if (cbx_player1.Focused == false && cbx_player2.Focused == false
                && cbx_player3.Focused == false && cbx_player4.Focused == false)
            {
                cbx_player1.BeginUpdate();                                            //Begin
                cbx_player1.Items.Clear();                                            //Empty the item list
                for (int i = 0; i < global_values.roster.Count; i++)
                {
                    cbx_player1.Items.Add(global_values.roster[i].unique_tag);
                }
                cbx_player1.EndUpdate();                                              //End
                object[] playerarray = cbx_player1.Items.Cast<Object>().ToArray();
                cbx_player2.BeginUpdate();                                            //Begin
                cbx_player2.Items.Clear();
                cbx_player2.Items.AddRange(playerarray);
                cbx_player2.EndUpdate();                                              //End

                if (global_values.format == "Doubles")
                {
                    cbx_player3.BeginUpdate();                                            //Begin
                    cbx_player3.Items.Clear();
                    cbx_player3.Items.AddRange(playerarray);
                    cbx_player3.EndUpdate();                                              //End
                    cbx_player4.BeginUpdate();                                            //Begin
                    cbx_player4.Items.Clear();
                    cbx_player4.Items.AddRange(playerarray);
                    cbx_player4.EndUpdate();                                              //End
                }

                cbx_player1.SelectedIndex = cbx_player1.FindStringExact(hold_player_name[0]);
                cbx_player2.SelectedIndex = cbx_player2.FindStringExact(hold_player_name[1]);
                if (global_values.format == "Doubles")
                {
                    cbx_player3.SelectedIndex = cbx_player3.FindStringExact(hold_player_name[2]);
                    cbx_player4.SelectedIndex = cbx_player4.FindStringExact(hold_player_name[3]);
                }
            }



            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            for (int i = loaded_queue.Count - 1; i >= 0; i--)
            {
                ListViewItem add_item = new ListViewItem(loaded_queue[i].number.ToString());
                add_item.SubItems.Add(loaded_queue[i].round);
                foreach (string queueplayer in loaded_queue[i].player)
                {
                    add_item.SubItems.Add(queueplayer);
                }
                switch (loaded_queue[i].status)
                {
                    case 1:
                        add_item.BackColor = current_match_color;
                        break;
                    case 2:
                        add_item.BackColor = next_match_color;
                        break;
                    case 3:
                        add_item.BackColor = dual_match_color;
                        break;
                }
                lvw_matches.Items.Add(add_item);
            }
            lvw_matches.EndUpdate();

            load_view(hold_index, topItemIndex, differential);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            //Verify that all needed match information is present
            if (cbx_round.Text == "" || cbx_player1.Text == "" || cbx_player2.Text == "" ||
                (global_values.format == "Doubles" && (cbx_player3.Text == "" || cbx_player4.Text == "")))
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            //Store the new match info and clear the boxes
            string new_round = cbx_round.Text;
            int[] player_index = { cbx_player1.SelectedIndex, cbx_player2.SelectedIndex, cbx_player3.SelectedIndex, cbx_player4.SelectedIndex };
            string[] player_name = { cbx_player1.Text, cbx_player2.Text, cbx_player3.Text, cbx_player4.Text };
            string[] new_player = { "", "", "", "" };
            cbx_round.Text = "";
            cbx_player1.Text = "";
            cbx_player2.Text = "";
            cbx_player3.Text = "";
            cbx_player4.Text = "";

            //Loop through and find the ID of each selected player
            for (int ii = 0; ii < numberOfPlayers; ii++)
                if (player_index[ii] >= 0)
                {
                    new_player[ii] = global_values.roster[player_index[ii]].id;
                }


            //Check if each player exists
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (new_player[i] == "")
                {
                    //Ask the user if they want to create a new record for the player
                    if (MessageBox.Show(player_name[i] + " is not found in the player database. " +
                        "Create a new record for the player?", "New Player Detected",
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //Show the user the new player form
                        var savePlayerForm = new SavePlayer.SavePlayerForm(player_name[i]);
                        if (savePlayerForm.ShowDialog() == DialogResult.OK)
                        {
                            //Add the new player to the database
                            database_tools.add_player(savePlayerForm.outputPlayer, true);
                            new_player[i] = savePlayerForm.outputPlayer.id;
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        return;
                    }
                }
            }

            //Update the queue as a failsafe
            loaded_queue = database_tools.load_queue(global_values.queue_id, false);
            //Add the new match to the queue
            QueueEntryModel new_match = new QueueEntryModel(loaded_queue.Count + 1, 0, new_round, new_player[0], new_player[1], new_player[2], new_player[3]);
            database_tools.add_match(new_match, true);
            //Reload the queue
            load_queue(0, false);
        }

        private void btn_movedown_Click(object sender, EventArgs e)
        {
            //Disable the movement buttons and exit this function if no match is selected
            if (lvw_matches.SelectedItems.Count == 0)
            {
                btn_movedown.Enabled = false;
                btn_moveup.Enabled = false;
                return;
            }
            //Alert the user if the selected match cannot move up
            if (lvw_matches.SelectedItems[0].Index == 0)
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            //Get the index of the match
            int selected_index = loaded_queue.Count - 1 - lvw_matches.SelectedItems[0].Index;

            //Switch the round numbers in the queue
            loaded_queue[selected_index].number -= 1;
            loaded_queue[selected_index - 1].number += 1;
            //Write the switched rounds to the database
            database_tools.add_match(loaded_queue[selected_index], false);
            database_tools.add_match(loaded_queue[selected_index - 1], false);
            //Reload the queue
            load_queue(1, false);
        }

        private void btn_moveup_Click(object sender, EventArgs e)
        {
            //Disable the movement buttons and exit this function if no match is selected
            if (lvw_matches.SelectedItems.Count == 0)
            {
                btn_movedown.Enabled = false;
                btn_moveup.Enabled = false;
                return;
            }
            //Alert the user if the selected match cannot move down
            if (lvw_matches.SelectedItems[0].Index == lvw_matches.Items.Count - 1)
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            //Get the index of the match
            int selected_index = loaded_queue.Count - 1 - lvw_matches.SelectedItems[0].Index;

            //Switch the round numbers in the queue
            loaded_queue[selected_index].number += 1;
            loaded_queue[selected_index + 1].number -= 1;
            //Write the switched rounds to the database
            database_tools.add_match(loaded_queue[selected_index], false);
            database_tools.add_match(loaded_queue[selected_index + 1], false);
            //Reload the queue
            load_queue(-1, false);

        }

        private void btn_push_Click(object sender, EventArgs e)
        {
            //Get the selected index in the match list
            int selected_index = loaded_queue.Count - lvw_matches.SelectedIndices[0] - 1;
            int queued_match = -1;
            //Cycle through the queue matches
            for (int i = 0; i < loaded_queue.Count(); i++)
            {
                //Check if any match is currently queued as the next match
                //Reset the match status if it is and end the loop
                //Save the index of the match
                if (loaded_queue[i].status == 2)
                {
                    loaded_queue[i].status = 0;
                    queued_match = i;
                    break;
                }
                if (loaded_queue[i].status == 3)
                {
                    loaded_queue[i].status = 1;
                    queued_match = i;
                    break;
                }
            }
            //Set the status of the currently selected match to queued
            if (loaded_queue[selected_index].status == 1)
                loaded_queue[selected_index].status = 3;
            else
                loaded_queue[selected_index].status = 2;

            //Write the updated rounds to the database
            if (queued_match != -1)
                database_tools.add_match(loaded_queue[queued_match], false);
            database_tools.add_match(loaded_queue[selected_index], false);

            //Reload the queue
            load_queue(0, false);
        }

        private void btn_remove_Click(object sender, EventArgs e)
        {
            //Verify that the user wants to remove this match
            if (MessageBox.Show("Are you sure you want to remove this match?\n" +
                "Match #" + lvw_matches.SelectedItems[0].SubItems[0].Text + " " + lvw_matches.SelectedItems[0].SubItems[1].Text + " - " +
                lvw_matches.SelectedItems[0].SubItems[2].Text + " vs. " + lvw_matches.SelectedItems[0].SubItems[3].Text, "Confirm Match Removal",
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            btn_edit.Enabled = false;
            btn_movedown.Enabled = false;
            btn_moveup.Enabled = false;
            btn_remove.Enabled = false;
            btn_push.Enabled = false;

            //Get the selected index in the match list
            int selected_index = loaded_queue.Count - lvw_matches.SelectedIndices[0] - 1;

            //Shift all matches after the current match down 1
            for (int i = selected_index + 1; i < loaded_queue.Count(); i++)
            {
                loaded_queue[i].number -= 1;
                database_tools.add_match(loaded_queue[i], false);
            }

            //Remove the final match from the queue
            database_tools.remove_match(loaded_queue.Count());

            //Refresh the queue
            load_queue(0, false);
        }

        private void lvw_matches_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Verify that at least one match is in the queue
            if (lvw_matches.SelectedIndices.Count > 0 && cbx_player1.Focused == false && cbx_player2.Focused == false &&
                cbx_player3.Focused == false && cbx_player4.Focused == false)
            {
                //Enable all match editing buttons
                btn_edit.Enabled = true;
                btn_movedown.Enabled = true;
                btn_moveup.Enabled = true;
                btn_remove.Enabled = true;
                btn_push.Enabled = true;


                //Cycle through the player roster to check each player.
                //Pass their information to the player info box
                int match_index = loaded_queue.Count - lvw_matches.SelectedIndices[0] - 1;
                for (int ii = 1; ii <= numberOfPlayers; ii++)
                {
                    string playerBeingUpdated = loaded_queue[match_index].player[ii - 1];
                    PlayerRecordModel playerRecord = new PlayerRecordModel();
                    if (PlayerDatabase.playerRecords.TryGetValue(playerBeingUpdated, out playerRecord))
                    {
                        update_info(ii, playerRecord);
                    }
                }
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            //Verify that the user wants to reset the queue
            if (MessageBox.Show("Are you sure you want to reset the stream queue? This will clear out all matches currently enterred!", "Confirm Queue Reset",
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            //Disable all buttons affecting the queue
            btn_edit.Enabled = false;
            btn_movedown.Enabled = false;
            btn_moveup.Enabled = false;
            btn_remove.Enabled = false;
            btn_push.Enabled = false;

            //Loop through and remove all queue matches
            for (int i = 0; i < loaded_queue.Count(); i++)
            {
                database_tools.remove_match(i + 1);
            }

            loaded_queue.Clear();

            //Clear out the Match Queue pane
            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            lvw_matches.EndUpdate();
        }

        private void save_view(ref int holding, ref int top)
        {
            if (lvw_matches.SelectedItems.Count > 0)
            {
                holding = lvw_matches.SelectedItems[0].Index;
                try
                {
                    top = lvw_matches.TopItem.Index;
                }
                catch (Exception ex)
                { }
            }
        }

        private void load_view(int holding, int top, int differential)
        {
            if (holding != -1)
            {
                int selected_index = holding + differential;
                if (selected_index > lvw_matches.Items.Count - 1)
                    selected_index = lvw_matches.Items.Count - 1;
                if (selected_index >= 0 && selected_index < lvw_matches.Items.Count)
                    lvw_matches.Items[selected_index].Selected = true;
                try
                {
                    lvw_matches.TopItem = lvw_matches.Items[top];
                }
                catch (Exception ex)
                { }
                lvw_matches.Items[selected_index].EnsureVisible();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            int match_index = loaded_queue.Count - lvw_matches.SelectedIndices[0] - 1;

            var edit_match = new frm_edit_match(loaded_queue[match_index], cbx_player1.Items);
            if (edit_match.ShowDialog() == DialogResult.OK)
            {
                load_queue(0, false);
            }
        }

        private void frm_streamqueue_FormClosed(object sender, FormClosedEventArgs e)
        {
            global_values.bracket_assistant = null;
            close_form(1);
        }

        private void btn_player1_Click(object sender, EventArgs e)
        {
            string update_name = selected_id[0];
            if (update_name != "")
                update_player(update_name);
        }

        private void btn_player2_Click(object sender, EventArgs e)
        {
            string update_name = selected_id[1];
            if (update_name != "")
                update_player(update_name);
        }

        private void btn_player3_Click(object sender, EventArgs e)
        {
            string update_name = selected_id[2];
            if (update_name != "")
                update_player(update_name);
        }

        private void btn_player4_Click(object sender, EventArgs e)
        {
            string update_name = selected_id[3];
            if (update_name != "")
                update_player(update_name);
        }

        private void update_player(string playerBeingUpdated)
        {
            //Find the index of the player
            PlayerRecordModel playerRecord = new PlayerRecordModel();
            if (PlayerDatabase.playerRecords.TryGetValue(playerBeingUpdated, out playerRecord) == false)
            { 
                return;
            }

            //Hold the player's name
            string previousUniqueTag = playerRecord.uniqueTag;

            //Show the save player form
            var savePlayerForm = new SavePlayer.SavePlayerForm(playerRecord, playerRecord.characterName, playerRecord.colorNumber);
            if (savePlayerForm.ShowDialog() == DialogResult.OK)
            {
                PlayerRecordModel newPlayerRecord = savePlayerForm.outputPlayer;

                if (savePlayerForm.outputIsNewPlayer == false)
                    PlayerDatabase.playerRecords[playerBeingUpdated] = newPlayerRecord;
                database_tools.add_player(savePlayerForm.outputPlayer, savePlayerForm.outputIsNewPlayer);

                //Check all queued matches for the player
                for (int i = 0; i < loaded_queue.Count; i++)
                {
                    bool updated_match = false;
                    for (int ii = 0; ii < 4; ii++)
                    {
                        //Update any instance of the player
                        if (loaded_queue[i].player[ii] == playerBeingUpdated)
                        {
                            loaded_queue[i].player[ii] = savePlayerForm.outputPlayer.id;
                            updated_match = true;
                        }
                    }
                    //Update the match if it had the player
                    if (updated_match)
                        database_tools.add_match(loaded_queue[i], false);
                }

                //reload the queue
                load_queue(0, false);

                //Update the tag in a combobox of the previous player
                if (cbx_player1.Text == previousUniqueTag)
                {
                    cbx_player1.Text = newPlayerRecord.uniqueTag;
                    update_info(1, newPlayerRecord);
                }
                if (cbx_player2.Text == previousUniqueTag)
                {
                    cbx_player2.Text = newPlayerRecord.uniqueTag;
                    update_info(2, newPlayerRecord);
                }
                if (global_values.format == "Doubles")
                {
                    if (cbx_player3.Text == previousUniqueTag)
                    {
                        cbx_player3.Text = newPlayerRecord.uniqueTag;
                        update_info(3, newPlayerRecord);
                    }
                    if (cbx_player4.Text == previousUniqueTag)
                    {
                        cbx_player4.Text = newPlayerRecord.uniqueTag;
                        update_info(4, newPlayerRecord);
                    }
                }


            }
        }

        private void update_new_match(object sender, EventArgs e)
        {
            //Create an empty player record
            PlayerRecordModel emptyPlayerRecord = new PlayerRecordModel();
            emptyPlayerRecord.tag = "";
            emptyPlayerRecord.twitter = "";
            emptyPlayerRecord.region = "";
            emptyPlayerRecord.fullSponsor = "";
            emptyPlayerRecord.characterName = "";

            string[] checkPlayerNames = { cbx_player1.Text, cbx_player2.Text, cbx_player3.Text, cbx_player4.Text };

            //Cycle through the player roster to check if each enterred player
            //is part of the roster. Pass their information to the player info
            //box if they exist
            for (int i = 0; i < numberOfPlayers; i++)
            {
                if (checkPlayerNames[i] != "")
                    update_info(i + 1, PlayerDatabase.playerRecords[checkPlayerNames[i]]);
                else
                    update_info(i + 1, emptyPlayerRecord);
            }
        }

        private void update_info(int player_number, PlayerRecordModel newPlayerInfo)
        {
            lvw_playerinfo.Items[0].SubItems[player_number].Text = newPlayerInfo.tag;
            lvw_playerinfo.Items[1].SubItems[player_number].Text = newPlayerInfo.twitter;
            lvw_playerinfo.Items[2].SubItems[player_number].Text = newPlayerInfo.region;
            lvw_playerinfo.Items[3].SubItems[player_number].Text = newPlayerInfo.fullSponsor;
            lvw_playerinfo.Items[4].SubItems[player_number].Text = newPlayerInfo.characterName;
            selected_id[player_number - 1] = newPlayerInfo.id;
        }
        #endregion

        #region AutoSeed
       
        public class tournament_player
        {
            public string id;
            public string tag;
            public int elo;
            public tournament_player() { }
            public tournament_player(string totag)
            {
                tag = totag;
            }

            public void get_id()
            {
                id = global_values.roster.Find(x => x.tag == tag).id;
            }
        }

        private void init_autoseed()
        {
            seeded_players = new List<tournament_player>();
            //Load test players
            txt_players.Text =
                "Serris\n" +
                "Frosty\n" +
                "Bushi\n" +
                "Ravenking\n" +
                "IX\n" +
                "Super\n" +
                "Super Dan\n" +
                "NickRoy\n" +
                "Swanner\n" +
                "Hydra\n" +
                "Ethanfo\n" +
                "Polaroid\n" +
                "TAYNE\n" +
                "testplayer69";
            foreach(string textline in txt_players.Lines)
            {
                tournament_player add_player = new tournament_player(textline);
                seeded_players.Add(add_player);
                
            }

            //Read the players from website
            //Prompt for any conflicting tags

            //Sort based on Elo

            build_seedlist();
        }

        private void build_seedlist()
        {
            //Add the players to the listview
            lvw_seeding.BeginUpdate();
            lvw_seeding.Items.Clear();
            for (int i = 0; i < seeded_players.Count(); i++)
            {
                ListViewItem add_item = new ListViewItem((i + 1).ToString());
                add_item.SubItems.Add(seeded_players[i].elo.ToString());
                add_item.SubItems.Add(seeded_players[i].tag);
                lvw_seeding.Items.Add(add_item);
            }
            lvw_seeding.EndUpdate();
        }

        private void lvw_seeding_ItemDrag(object sender, ItemDragEventArgs e)
        {
            draggedItem = ((ListViewItem)e.Item);
            
            DoDragDrop(e.Item, DragDropEffects.Link);
        }

        private void lvw_seeding_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Link;
        }

        private void lvw_seeding_DragDrop(object sender, DragEventArgs e)
        {
            //Find the mouse point
            Point cp = lvw_seeding.PointToClient(new Point(e.X, e.Y));
            //Fidn the itme at the mouse point an just below it
            ListViewItem dragToItem = lvw_seeding.GetItemAt(cp.X, cp.Y);
            ListViewItem checkItem = lvw_seeding.GetItemAt(cp.X, cp.Y+7);
            int dropIndex = -1;
            //If the 2 itmes are different, place the dragged item between
            if (dragToItem != checkItem)
                dropIndex = checkItem.Index;
            else
            {
                if (dragToItem != null)
                    dropIndex = dragToItem.Index;
                else
                    dropIndex = lvw_seeding.Items.Count - 1;
            }

            int dragIndex = Int32.Parse(draggedItem.SubItems[0].Text) - 1;

            //Recorder
            tournament_player move_player = seeded_players[dragIndex];
            int shift = 0;
            if (dragIndex < dropIndex)
                shift = -1;
            seeded_players.RemoveAt(dragIndex);
            seeded_players.Insert(dropIndex+shift, move_player);

            build_seedlist();
        }


        #endregion


        private async void btn_link_event_Click(object sender, EventArgs e)
        {
            bool success = await Tournament.LinkTournament(txt_tournament_url.Text);
            /*
            //The Challonge API key
            string APIKEY = "2HbljZB6H72nVTYdH7xqNwmG8cEFr8gNuE7NXeBZ";

            //url is the tournament ID
            //subdomain is the subdomain
            string url = txt_tournament_url.Text;
            string subdomain = "";

            //Check for necessary URL formatting
            if(url.ToLower().Contains("challonge") && !(url.Contains(".") && url.Contains("/")))
            {
                MessageBox.Show("This is not a properly formatted Challonge URL. Please check the URL and try again.");
                return;
            }

            //Parse the subdomain
            int starting_cutoff = 0;
            if(url.Contains("://"))
            {
                //Find the location of :// and add 3, to get the location after it
                starting_cutoff = url.IndexOf("://") + 3;
            }
            int ending_cutoff = url.IndexOf(".");
            int substr_length = ending_cutoff - starting_cutoff;
            subdomain = url.Substring(starting_cutoff, substr_length);

            //Check for cases where there isn't a subdomain
            if(subdomain.ToLower() == "www" || subdomain.ToLower() == "challonge")
            {
                subdomain = "";
            }


            //Parse the tournament ID
            //Find the divider before the ID
            starting_cutoff = url.LastIndexOf("/") + 1;
            //Make sure you aren't grabbing the slash at the very end of the URL
            if(starting_cutoff == url.Length)
            {
                url = url.Substring(0, url.Length - 1);
                starting_cutoff = url.LastIndexOf("/") + 1;
            }
            ending_cutoff = url.Length;
            substr_length = ending_cutoff - starting_cutoff;
            url = url.Substring(starting_cutoff, substr_length);

            //combine the two if there's a subdomain
            if(subdomain != "")
            {
                url = subdomain + "-" + url;
            }

            MessageBox.Show("'" + url + "'");

            Tournament.config = new ChallongeConfig(APIKEY);
            Tournament.caller = new ChallongeHTTPClientAPICaller(Tournament.config);
            Tournament.tournaments = new Tournaments(Tournament.caller);

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            //Get the tournament based on the combined subdomain and url
            TournamentQueryResult tournamentResult = await Tournament.caller.GET<TournamentQueryResult>(@"tournaments/" + url, new ChallongeQueryParameters());
            Tournament.id = tournamentResult.tournament.id;
            Tournament.info = tournamentResult.tournament;
            Tournament._tournament = new ChallongeCSharpDriver.Main.Objects.TournamentObject(Tournament.info, Tournament.caller);
            Tournament._tournament.
            */
        }
    }
}