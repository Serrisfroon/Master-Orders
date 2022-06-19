﻿using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_edit_match : Form
    {
        QueueEntryModel match;

        public frm_edit_match(QueueEntryModel edit_match, ComboBox.ObjectCollection players)
        {
            this.CenterToScreen();

            InitializeComponent();

            string[] importedRounds = SettingsFile.LoadBracketRounds();
            if (importedRounds != null)
            {
                cbx_round.Items.Clear();
                cbx_round.BeginUpdate();
                cbx_round.Items.AddRange(importedRounds);
                cbx_round.EndUpdate();
            }
            this.TopMost = global_values.keepWindowsOnTop;

            match = edit_match;
            cbx_round.Text = match.roundInBracket;
            cbx_player1.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player2.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player3.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player4.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player1.SelectedIndex = cbx_player1.FindStringExact(PlayerDatabase.GetUniqueTagFromId(match.playerIds[0]));
            cbx_player2.SelectedIndex = cbx_player2.FindStringExact(PlayerDatabase.GetUniqueTagFromId(match.playerIds[1]));
            cbx_player3.SelectedIndex = cbx_player3.FindStringExact(PlayerDatabase.GetUniqueTagFromId(match.playerIds[2]));
            cbx_player4.SelectedIndex = cbx_player4.FindStringExact(PlayerDatabase.GetUniqueTagFromId(match.playerIds[3]));

            if (GlobalSettings.bracketFormat == "Singles")
            {
                cbx_player3.Enabled = false;
                cbx_player3.Visible = false;
                cbx_player4.Enabled = false;
                cbx_player4.Visible = false;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            //Verify that all needed match information is present
            if (cbx_round.Text == "" || cbx_player1.Text == "" || cbx_player2.Text == "" ||
                (GlobalSettings.bracketFormat == "Doubles" && (cbx_player3.Text == "" || cbx_player4.Text == "")))
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            //Set the player count
            int player_count = 2;
            if (GlobalSettings.bracketFormat == "Doubles")
                player_count = 4;

            string new_round = cbx_round.Text;
            string[] player_name = { cbx_player1.Text, cbx_player2.Text, cbx_player3.Text, cbx_player4.Text };
            string[] new_player = { "", "", "", "" };
            cbx_round.Text = "";
            cbx_player1.Text = "";
            cbx_player2.Text = "";
            cbx_player3.Text = "";
            cbx_player4.Text = "";

            //Loop through and find the ID of each selected player
            for (int ii = 0; ii < player_count; ii++)
            {
                PlayerRecordModel foundRecord = PlayerDatabase.FindRecordFromString(PlayerDatabase.playerRecords, player_name[ii], PlayerDatabase.SearchProperty.uniqueTag);
                if (foundRecord != null)
                {
                    new_player[ii] = foundRecord.id;
                }
            }

            //Check if each player exists
            for (int i = 0; i < player_count; i++)
            {
                if (new_player[i] == "")
                {
                    //Ask the user if they want to create a new record for the player
                    if (MessageBox.Show(player_name[i] + " is not found in the player database. " +
                        "Create a new record for the player?", "New Player Detected",
                        MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //Show the user the new player form
                        var PlayerRecordModel_box = new SavePlayer.SavePlayerForm(player_name[i]);
                        if (PlayerRecordModel_box.ShowDialog() == DialogResult.OK)
                        {
                            //Add the new player to the database                            
                            PlayerDatabase.AddPlayer(PlayerRecordModel_box.outputPlayer, true);
                            new_player[i] = PlayerRecordModel_box.outputPlayer.id;
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

            //Update the match info
            match.playerIds[0] = new_player[0];
            match.playerIds[1] = new_player[1];
            match.playerIds[2] = new_player[2];
            match.playerIds[3] = new_player[3];

            match.roundInBracket = new_round;

            //Add the new match to the queue
            StreamQueueManager.AddEntryToQueue(match, StreamQueueManager.queueId, false);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
