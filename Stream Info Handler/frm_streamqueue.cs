using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_streamqueue : Form
    {
        public static string sheets_id;
        Color current_match_color = Color.FromArgb(138, 240, 146);
        Color next_match_color = Color.FromArgb(234, 153, 153);

        private System.Windows.Forms.Timer refresh_queue;
        public void init_refresh()
        {
            refresh_queue = new System.Windows.Forms.Timer();
            refresh_queue.Tick += new EventHandler(queue_Tick);
            refresh_queue.Interval = 15000; // in miliseconds
            refresh_queue.Start();
        }
        private void queue_Tick(object sender, EventArgs e)
        {
            load_queue();
        }

        public frm_streamqueue(string sheet_id)
        {
            InitializeComponent();

            sheets_id = sheet_id;

            load_queue();
            init_refresh();
        }

        public void load_queue()
        {
            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            var service = connect_to_sheets();

            List<string> ranges = new List<string>(new string[] { "Upcoming Matches!A1:E56", "Player Information!A2:O" + (frm_main.MAX_PLAYERS + 1).ToString() });

            SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(sheets_id);
            request.Ranges = ranges;

            Google.Apis.Sheets.v4.Data.BatchGetValuesResponse response = request.Execute();
            IList<IList<Object>> upcoming_matches = response.ValueRanges[0].Values;
            IList<IList<Object>> player_information = response.ValueRanges[1].Values;

            //Populate the player roster with information from the player information sheet
            for (int i = 0; i <= frm_main.MAX_PLAYERS; i++)
            {
                if (player_information[i][0].ToString() == "")
                {
                    global_values.roster_size = i - 1;
                    break;
                }
                global_values.roster[i] = new player_info();
                global_values.roster[i].tag = player_information[i][0].ToString();
                global_values.roster[i].twitter = player_information[i][1].ToString();
                global_values.roster[i].region = player_information[i][2].ToString();
                global_values.roster[i].sponsor = player_information[i][3].ToString();
                for (int ii = 0; ii <= 4; ii++)
                {
                    global_values.roster[i].character[ii] = player_information[i][4 + ii].ToString();
                }
                for (int ii = 0; ii <= 4; ii++)
                {
                    if (player_information[i][9 + ii] != null && player_information[i][9 + ii] != "")
                    {
                        string add_color = player_information[i][9 + ii].ToString();
                        global_values.roster[i].color[ii] = Int32.Parse(add_color);
                    }
                    else
                    {
                        global_values.roster[i].color[ii] = 1;
                    }
                }
            }

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            int next_match = get_next_match(upcoming_matches[1][3].ToString());

            string[] player_name = new string[5];
            player_name[1] = upcoming_matches[4 + round_number][2].ToString();
            player_name[2] = upcoming_matches[4 + round_number][3].ToString();

            player_info[] player = new player_info[5];

            for (int player_number = 1; player_number <= 2; player_number++)
            {
                for (int i = 0; i <= global_values.roster_size; i++)
                {
                    if (player_name[player_number] == "")
                    {
                        player[player_number] = new player_info();
                        player[player_number].tag = "";
                        player[player_number].twitter = "";
                        player[player_number].region = "";
                        player[player_number].sponsor = "";
                        for (int ii = 0; ii <= 4; ii++)
                        {
                            player[player_number].character[ii] = "";
                            player[player_number].color[ii] = 1;
                        }
                        int character_count = Int32.Parse(global_values.game_info[1]);      //Store the number of characters
                        break;
                    }
                    if (global_values.roster[i].tag == player_name[player_number])
                    {
                        player[player_number] = global_values.roster[i];
                        break;
                    }
                }
            }


            cbx_player1.BeginUpdate();                                            //Begin
            cbx_player1.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_player1.Items.Add(global_values.roster[i].tag);
            }
            cbx_player1.EndUpdate();                                              //End
            //cbx_player1.SelectedIndex = cbx_player1.Items.IndexOf(player[1].tag);   //Set the combobox index to 0

            cbx_player2.BeginUpdate();                                            //Begin
            cbx_player2.Items.Clear();                                            //Empty the item list
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                cbx_player2.Items.Add(global_values.roster[i].tag);
            }
            cbx_player2.EndUpdate();                                              //End
            //cbx_player2.SelectedIndex = cbx_player2.Items.IndexOf(player[2].tag);   //Set the combobox index to 0

            //cbx_round.Text = upcoming_matches[4 + round_number][1].ToString();


            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            for (int i = 1; i <= 50; i++)
            {
                if (upcoming_matches[3 + i][1].ToString() != "")
                {
                    ListViewItem add_item = new ListViewItem(upcoming_matches[3 + i][0].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][1].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][2].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][3].ToString());
                    if (i == round_number)
                    {
                        add_item.BackColor = current_match_color;
                    }
                    if (i == next_match)
                    {
                        add_item.BackColor = next_match_color;
                    }
                    lvw_matches.Items.Add(add_item);
                }
            }
            lvw_matches.EndUpdate();

            load_view(hold_index, topItemIndex, 0);
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            if (cbx_round.Text == "" || cbx_player1.Text == "" || cbx_player2.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            int new_match = 0;
            string new_round = cbx_round.Text;
            string new_player1 = cbx_player1.Text;
            string new_player2 = cbx_player2.Text;
            bool check_player1 = false;
            bool check_player2 = false;

            cbx_round.Text = "";
            cbx_player1.Text = "";
            cbx_player2.Text = "";

            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == new_player1)
                {
                    check_player1 = true;
                }
                if (global_values.roster[i].tag == new_player2)
                {
                    check_player2 = true;
                }
            }

            if (check_player1 == false)
            {
                if (MessageBox.Show(new_player1 + " is not found in the player database. " +
                    "Create a new record for the player?", "New Player Detected",
                    MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var player_info_box = new frm_save_player(new_player1);
                    if (player_info_box.ShowDialog() == DialogResult.OK)
                    {
                        add_to_sheets(frm_main.get_new_player);
                        new_player1 = frm_main.get_new_player.tag;
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

            if (check_player2 == false)
            {
                if (MessageBox.Show(new_player2 + " is not found in the player database. " +
                "Create a new record for the player?", "New Player Detected",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var player_info_box = new frm_save_player(new_player2);
                    if (player_info_box.ShowDialog() == DialogResult.OK)
                    {
                        add_to_sheets(frm_main.get_new_player);
                        new_player2 = frm_main.get_new_player.tag;
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

            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            var service = connect_to_sheets();

            IList<IList<Object>> upcoming_matches = load_queue_info(service);

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            int next_match = get_next_match(upcoming_matches[1][3].ToString());

            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            for (int i = 1; i <= 50; i++)
            {
                if (upcoming_matches[3 + i][1].ToString() != "")
                {
                    ListViewItem add_item = new ListViewItem(upcoming_matches[3 + i][0].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][1].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][2].ToString());
                    add_item.SubItems.Add(upcoming_matches[3 + i][3].ToString());
                    if (i == round_number)
                    {
                        add_item.BackColor = current_match_color;
                    }
                    if (i == next_match)
                    {
                        add_item.BackColor = next_match_color;
                    }
                    lvw_matches.Items.Add(add_item);
                }
                else
                {
                    ListViewItem add_item = new ListViewItem(upcoming_matches[3 + i][0].ToString());
                    add_item.SubItems.Add(new_round);
                    add_item.SubItems.Add(new_player1);
                    add_item.SubItems.Add(new_player2);
                    if (i == round_number)
                    {
                        add_item.BackColor = current_match_color;
                    }
                    if (i == next_match)
                    {
                        add_item.BackColor = next_match_color;
                    }
                    lvw_matches.Items.Add(add_item);
                    new_match = i + 4;
                    break;
                }
            }
            lvw_matches.EndUpdate();

            load_view(hold_index, topItemIndex, 0);


            var oblist = new List<object>() { new_round, new_player1, new_player2 };

            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>> { oblist };
            upcoming.Range = "Upcoming Matches!B" + new_match.ToString() + ":D" + new_match.ToString();
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        public void add_to_sheets(player_info new_player)
        {
            var service = connect_to_sheets();

            //Set the range to be only the player information
            string range = "Player Information!A2:O" + (frm_main.MAX_PLAYERS + 1).ToString();

            //Set up the request for the sheet
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(sheets_id, range);

            //Receive the player information from the request
            Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
            //Place the information in an array
            IList<IList<Object>> player_information = response.Values;

            //Cycle through the player information until the new player's name is found, or the end of the list is found.
            int player_index = -1;                  //Intialize the index of the new player in the list
            for (int i = 0; i <= frm_main.MAX_PLAYERS; i++)
            {
                if (player_information[i][0].ToString() == new_player.tag)
                {
                    player_index = i;               //Set the index to the current position
                    global_values.roster[i].sponsor = new_player.sponsor;
                    global_values.roster[i].twitter = new_player.twitter;
                    global_values.roster[i].character[0] = new_player.character[0];
                    global_values.roster[i].character[1] = new_player.character[1];
                    global_values.roster[i].character[2] = new_player.character[2];
                    global_values.roster[i].character[3] = new_player.character[3];
                    global_values.roster[i].character[4] = new_player.character[4];
                    global_values.roster[i].color[0] = new_player.color[0];
                    global_values.roster[i].color[1] = new_player.color[1];
                    global_values.roster[i].color[2] = new_player.color[2];
                    global_values.roster[i].color[3] = new_player.color[3];
                    global_values.roster[i].color[4] = new_player.color[4];
                    break;
                }
                if (player_information[i][0].ToString() == "")
                {
                    player_index = i;               //Set the index to the current position
                    global_values.roster[i] = new_player;
                    global_values.roster_size += 1;
                    break;
                }
            }



            //Add the new player's information to an array.
            var oblist = new List<object>() { global_values.roster[player_index].tag,
                                                global_values.roster[player_index].twitter,
                                                global_values.roster[player_index].region,
                                                global_values.roster[player_index].sponsor,
                                                global_values.roster[player_index].character[0],
                                                global_values.roster[player_index].character[1],
                                                global_values.roster[player_index].character[2],
                                                global_values.roster[player_index].character[3],
                                                global_values.roster[player_index].character[4],
                                                global_values.roster[player_index].color[0],
                                                global_values.roster[player_index].color[1],
                                                global_values.roster[player_index].color[2],
                                                global_values.roster[player_index].color[3],
                                                global_values.roster[player_index].color[4]};

            //Create a data set from the array
            Google.Apis.Sheets.v4.Data.ValueRange data = new Google.Apis.Sheets.v4.Data.ValueRange();
            data.Values = new List<IList<object>> { oblist };
            //Set the range's row to the player index"
            string range2 = "Player Information!A" + (player_index + 2).ToString() + ":N" + (player_index + 2).ToString();
            data.MajorDimension = "ROWS";

            //Process the update
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(data, sheets_id, range2);
            update.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.RAW;
            Google.Apis.Sheets.v4.Data.UpdateValuesResponse result2 = update.Execute();

            MessageBox.Show("The following player information has been added to the database:" +
                                "\n Tag: " + global_values.roster[player_index].tag +
                                "\n Twitter: " + global_values.roster[player_index].twitter +
                                "\n Region: " + global_values.roster[player_index].region +
                                "\n Sponsor: " + global_values.roster[player_index].sponsor +
                                "\n Main: " + global_values.roster[player_index].character[0] +
                                "\n Color: " + global_values.roster[player_index].color[0]);

        }

        private void btn_moveup_Click(object sender, EventArgs e)
        {
            if (lvw_matches.SelectedItems[0].Text == "1")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            int selected_index = lvw_matches.SelectedItems[0].Index + 1;
            int move_index = 0;

            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            var service = connect_to_sheets();

            IList<IList<Object>> upcoming_matches = load_queue_info(service);

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            int next_match = get_next_match(upcoming_matches[1][3].ToString());

            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            ListViewItem add_item = new ListViewItem();
            for (int i = 1; i <= 50; i++)
            {
                if (upcoming_matches[3 + i][1].ToString() == "")
                {
                    continue;
                }
                if (i == selected_index)
                {
                    add_to_queue(i, 2, round_number, next_match, upcoming_matches);
                    move_index = i;
                    continue;
                }
                if (i == selected_index - 1)
                {
                    add_to_queue(i, 4, round_number, next_match, upcoming_matches);
                    continue;
                }
                add_to_queue(i, 3, round_number, next_match, upcoming_matches);
            }
            lvw_matches.EndUpdate();

            load_view(hold_index, topItemIndex, -1);

            var oblist = new List<object>() { upcoming_matches[3 + move_index][1].ToString(), upcoming_matches[3 + move_index][2].ToString(), upcoming_matches[3 + move_index][3].ToString() };
            var oblist2 = new List<object>() { upcoming_matches[2 + move_index][1].ToString(), upcoming_matches[2 + move_index][2].ToString(), upcoming_matches[2 + move_index][3].ToString() };

            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>> { oblist, oblist2 };
            upcoming.Range = "Upcoming Matches!B" + (move_index + 3).ToString() + ":D" + (move_index + 4).ToString();
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        private void btn_movedown_Click(object sender, EventArgs e)
        {
            if (lvw_matches.SelectedItems[0].Text == lvw_matches.Items.Count.ToString())
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }

            int selected_index = lvw_matches.SelectedItems[0].Index + 1;
            int move_index = 0;

            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            var service = connect_to_sheets();

            IList<IList<Object>> upcoming_matches = load_queue_info(service);

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            int next_match = get_next_match(upcoming_matches[1][3].ToString());

            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            ListViewItem add_item = new ListViewItem();
            for (int i = 1; i <= 50; i++)
            {
                if (upcoming_matches[3 + i][1].ToString() == "")
                {
                    continue;
                }
                if (i == selected_index + 1)
                {
                    add_to_queue(i, 2, round_number, next_match, upcoming_matches);
                    move_index = i;
                    continue;
                }
                if (i == selected_index)
                {
                    add_to_queue(i, 4, round_number, next_match, upcoming_matches);
                    continue;
                }
                add_to_queue(i, 3, round_number, next_match, upcoming_matches);
            }
            lvw_matches.EndUpdate();

            load_view(hold_index, topItemIndex, 1);

            var oblist = new List<object>() { upcoming_matches[3 + move_index][1].ToString(), upcoming_matches[3 + move_index][2].ToString(), upcoming_matches[3 + move_index][3].ToString() };
            var oblist2 = new List<object>() { upcoming_matches[2 + move_index][1].ToString(), upcoming_matches[2 + move_index][2].ToString(), upcoming_matches[2 + move_index][3].ToString() };

            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>> { oblist, oblist2 };
            upcoming.Range = "Upcoming Matches!B" + (move_index + 3).ToString() + ":D" + (move_index + 4).ToString();
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        private void btn_push_Click(object sender, EventArgs e)
        {
            int selected_index = lvw_matches.SelectedItems[0].Index + 1;

            for (int i = 0; i < lvw_matches.Items.Count; i++)
            {
                if (lvw_matches.Items[i].BackColor == next_match_color)
                {
                    lvw_matches.Items[i].BackColor = Color.White;
                }
            }

            lvw_matches.SelectedItems[0].BackColor = next_match_color;

            var service = connect_to_sheets();

            var oblist = new List<object>() { selected_index };

            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>> { oblist };
            upcoming.Range = "Upcoming Matches!D2";
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        private void button1_Click(object sender, EventArgs e)
        {
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

            int selected_index = lvw_matches.SelectedItems[0].Index + 1;

            int hold_index = -1;
            int topItemIndex = 0;
            save_view(ref hold_index, ref topItemIndex);

            var service = connect_to_sheets();

            IList<IList<Object>> upcoming_matches = load_queue_info(service);

            int round_number = Int32.Parse(upcoming_matches[1][1].ToString());
            int next_match = get_next_match(upcoming_matches[1][3].ToString());
            int delete_index = -1;

            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            ListViewItem add_item = new ListViewItem();
            for (int i = 1; i <= 50; i++)
            {
                if (upcoming_matches[3 + i][1].ToString() == "")
                {
                    continue;
                }
                if (i == selected_index)
                {
                    delete_index = i;
                    continue;
                }
                int next_write = i;
                if (delete_index != -1)
                {
                    next_write--;
                    add_item = new ListViewItem(next_write.ToString());
                    if (i == round_number + 1)
                    {
                        add_item.BackColor = current_match_color;
                    }
                    if (i == next_match + 1)
                    {
                        add_item.BackColor = next_match_color;
                    }
                }
                else
                {
                    add_item = new ListViewItem(next_write.ToString());
                    if (i == round_number)
                    {
                        add_item.BackColor = current_match_color;
                    }
                    if (i == next_match)
                    {
                        add_item.BackColor = next_match_color;
                    }
                }
                add_item.SubItems.Add(upcoming_matches[3 + i][1].ToString());
                add_item.SubItems.Add(upcoming_matches[3 + i][2].ToString());
                add_item.SubItems.Add(upcoming_matches[3 + i][3].ToString());

                lvw_matches.Items.Add(add_item);
            }
            lvw_matches.EndUpdate();

            try
            {
                lvw_matches.TopItem = lvw_matches.Items[topItemIndex];
            }
            catch (Exception ex)
            { }

            lvw_matches.Items[hold_index + 1].EnsureVisible();


            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>>();

            for (int i = 1; i <= lvw_matches.Items.Count; i++)
            {
                var oblist = new List<object>() { lvw_matches.Items[i-1].SubItems[1].Text,
                    lvw_matches.Items[i-1].SubItems[2].Text, lvw_matches.Items[i-1].SubItems[3].Text };
                upcoming.Values.Add(oblist);
            }
            var lastlist = new List<object>() { "", "", "" };
            upcoming.Values.Add(lastlist);

            upcoming.Range = "Upcoming Matches!B5:D" + (lvw_matches.Items.Count + 5).ToString();
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        private void lvw_matches_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvw_matches.SelectedIndices.Count > 0)
            {
                btn_edit.Enabled = true;
                btn_movedown.Enabled = true;
                btn_moveup.Enabled = true;
                btn_remove.Enabled = true;
                btn_push.Enabled = true;
            }
        }

        private void btn_reset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to reset the stream queue? This will clear out all matches currently enterred!", "Confirm Queue Reset",
                MessageBoxButtons.OKCancel) != DialogResult.OK)
            {
                return;
            }

            btn_edit.Enabled = false;
            btn_movedown.Enabled = false;
            btn_moveup.Enabled = false;
            btn_remove.Enabled = false;
            btn_push.Enabled = false;

            lvw_matches.BeginUpdate();
            lvw_matches.Items.Clear();
            lvw_matches.EndUpdate();

            var service = connect_to_sheets();

            var oblist = new List<object>() { "1", "Next Match to Stream", "" };
            var oblist2 = new List<object>() { "", "", "" };
            var oblist3 = new List<object>() { "Round", "Player 1", "Player 2" };

            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>> { oblist, oblist2, oblist3 };
            for(int i = 1; i < 51; i++)
            {
                upcoming.Values.Add(oblist2);
            }
            upcoming.Range = "Upcoming Matches!B2:D54";
            upcoming.MajorDimension = "ROWS";

            update_queue(upcoming, sheets_id, service);
        }

        private SheetsService connect_to_sheets()
        {
            UserCredential credential;

            using (var stream =
                new FileStream(global_values.json_file, FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                    System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/sheets.googleapis.com-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    new[] { SheetsService.Scope.Spreadsheets },
                    global_values.youtube_username,
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = this.GetType().ToString()
            });

            return service;
        }

        private IList<IList<object>> load_queue_info(SheetsService connect_service)
        {
            String spreadsheetId = sheets_id;

            List<string> ranges = new List<string>(new string[] { "Upcoming Matches!A1:E56" });

            SpreadsheetsResource.ValuesResource.BatchGetRequest request = connect_service.Spreadsheets.Values.BatchGet(spreadsheetId);
            request.Ranges = ranges;

            Google.Apis.Sheets.v4.Data.BatchGetValuesResponse response = request.Execute();
            return response.ValueRanges[0].Values;
        }

        private void add_to_queue(int queue_number, int differential, int current_round, int next_round, IList<IList<object>> sheet_info)
        {
            ListViewItem add_item = new ListViewItem(sheet_info[3 + queue_number][0].ToString());
            add_item.SubItems.Add(sheet_info[differential + queue_number][1].ToString());
            add_item.SubItems.Add(sheet_info[differential + queue_number][2].ToString());
            add_item.SubItems.Add(sheet_info[differential + queue_number][3].ToString());
            if (queue_number == current_round)
            {
                add_item.BackColor = current_match_color;
            }
            if (queue_number == next_round)
            {
                add_item.BackColor = next_match_color;
            }
            lvw_matches.Items.Add(add_item);
        }

        private void update_queue(Google.Apis.Sheets.v4.Data.ValueRange data_range, string sheetid, SheetsService connect_service)
        {

            List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>() { data_range };  // TODO: Update placeholder value.

            // TODO: Assign values to desired properties of `requestBody`:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "RAW";
            requestBody.Data = data;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = connect_service.Spreadsheets.Values.BatchUpdate(requestBody, sheetid);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();
        }

        private int get_next_match(string match_text)
        {
            if (match_text != "")
            {
                return Int32.Parse(match_text);
            }
            else
            {
                return -1;
            }
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
                lvw_matches.Items[holding + differential].Selected = true;
                try
                {
                    lvw_matches.TopItem = lvw_matches.Items[top];
                }
                catch (Exception ex)
                { }
                lvw_matches.Items[holding + differential].EnsureVisible();
            }
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            string match = lvw_matches.SelectedItems[0].SubItems[0].Text;
            string round = lvw_matches.SelectedItems[0].SubItems[1].Text;
            string player1 = lvw_matches.SelectedItems[0].SubItems[2].Text;
            string player2 = lvw_matches.SelectedItems[0].SubItems[3].Text;
            
            var edit_match = new frm_edit_match(match, round, player1, player2, cbx_player1.Items);
            if (edit_match.ShowDialog() == DialogResult.OK)
            {
                load_queue();
            }
        }
    }
}