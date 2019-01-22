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
    public partial class frm_edit_match_dubs : Form
    {
        string change_match;

        public frm_edit_match_dubs(string match, string round, string player1, string player2, string player3, string player4, ComboBox.ObjectCollection players)
        {
            InitializeComponent();
            cbx_round.Text = round;
            cbx_player1.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player2.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player3.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player4.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player1.Text = player1;
            cbx_player2.Text = player2;
            cbx_player3.Text = player3;
            cbx_player4.Text = player4;

            change_match = match;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (cbx_round.Text == "" || cbx_player1.Text == "" || cbx_player2.Text == "" || cbx_player3.Text == "" || cbx_player4.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
            int new_match = 0;
            string new_round = cbx_round.Text;
            string new_player1 = cbx_player1.Text;
            string new_player2 = cbx_player2.Text;
            string new_player3 = cbx_player3.Text;
            string new_player4 = cbx_player4.Text;
            bool check_player1 = false;
            bool check_player2 = false;
            bool check_player3 = false;
            bool check_player4 = false;

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
                if (global_values.roster[i].tag == new_player3)
                {
                    check_player3 = true;
                }
                if (global_values.roster[i].tag == new_player4)
                {
                    check_player4 = true;
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

            if (check_player3 == false)
            {
                if (MessageBox.Show(new_player3 + " is not found in the player database. " +
                "Create a new record for the player?", "New Player Detected",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var player_info_box = new frm_save_player(new_player3);
                    if (player_info_box.ShowDialog() == DialogResult.OK)
                    {
                        add_to_sheets(frm_main.get_new_player);
                        new_player3 = frm_main.get_new_player.tag;
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

            if (check_player4 == false)
            {
                if (MessageBox.Show(new_player4 + " is not found in the player database. " +
                "Create a new record for the player?", "New Player Detected",
                MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    var player_info_box = new frm_save_player(new_player4);
                    if (player_info_box.ShowDialog() == DialogResult.OK)
                    {
                        add_to_sheets(frm_main.get_new_player);
                        new_player4 = frm_main.get_new_player.tag;
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

            cbx_round.Text = "";
            cbx_player1.Text = "";
            cbx_player2.Text = "";
            cbx_player3.Text = "";
            cbx_player4.Text = "";

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

            String spreadsheetId = frm_streamqueue.sheets_id;

            List<string> ranges = new List<string>(new string[] { "Upcoming Matches!A1:G56" });

            SpreadsheetsResource.ValuesResource.BatchGetRequest request = service.Spreadsheets.Values.BatchGet(spreadsheetId);
            request.Ranges = ranges;

            Google.Apis.Sheets.v4.Data.BatchGetValuesResponse response = request.Execute();
            IList<IList<object>> matches = response.ValueRanges[0].Values;
            List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>();

            int updating_match = Int32.Parse(change_match);
            int this_match = Int32.Parse(matches[1][1].ToString());
            int next_match = 0;
            if (Int32.TryParse(matches[1][3].ToString(), out next_match) == false)
            {
                next_match = this_match + 1;
                for (int i = 1; i < 50; i++)
                {
                    if(matches[i+4][1].ToString() == "")
                    {
                        if(next_match > i)
                        {
                            next_match = this_match;
                        }
                        break;
                    }
                }
            }
            if (updating_match == this_match || updating_match == next_match)
            {
                // The new values to apply to the spreadsheet.
                Google.Apis.Sheets.v4.Data.ValueRange current_info = new Google.Apis.Sheets.v4.Data.ValueRange();

                string round = matches[this_match + 3][1].ToString();
                string nextround = matches[next_match + 3][1].ToString();
                string[] update_players = { };
                if (updating_match == this_match)
                {
                    round = new_round;
                    string[] new_players = { new_player1,
                                    new_player2,
                                    new_player3,
                                    new_player4,
                                    matches[next_match+3][2].ToString(),
                                    matches[next_match+3][3].ToString(),
                                    matches[next_match+3][4].ToString(),
                                    matches[next_match+3][5].ToString() };
                    update_players = new_players;
                }
                else
                {
                    nextround = new_round;
                    string[] new_players = { matches[this_match+3][2].ToString(),
                                    matches[this_match+3][3].ToString(),
                                    matches[this_match+3][4].ToString(),
                                    matches[this_match+3][5].ToString(),
                                    new_player1,
                                    new_player2,
                                    new_player3,
                                    new_player4 };
                    update_players = new_players;
                }

                player_info[] player_data = new player_info[8];
                for (int ii = 0; ii < 8; ii++)
                {
                    for (int i = 0; i <= global_values.roster_size; i++)
                    {
                        if (global_values.roster[i].tag == update_players[ii])
                        {
                            player_data[ii] = global_values.roster[i];
                            break;
                        }
                    }
                }

                // The new values to apply to the spreadsheet.
                var uoblist1 = new List<object>() { "Current Match", "TEAM1 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:", "",
                                                "Next Match", "TEAM1 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                var uoblist2 = new List<object>();
                var uoblist3 = new List<object>() { round, "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:", "",
                                                nextround, "TEAM1 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:"};
                var uoblist4 = new List<object>();
                var uoblist5 = new List<object>() { "", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:", "",
                                                "", "TEAM2 P1", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:" };
                var uoblist6 = new List<object>();
                var uoblist7 = new List<object>() { "", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:", "",
                                                "", "TEAM2 P2", "Tag:", "Twitter:", "Region:", "Sponsor:", "Character:"};
                var uoblist8 = new List<object>();

                if (this_match != next_match)
                {
                    uoblist2 = new List<object>() { "", "", player_data[0].tag, player_data[0].twitter, player_data[0].region, player_data[0].sponsor, player_data[0].character[0], "",
                                                "", "", player_data[4].tag, player_data[4].twitter, player_data[4].region, player_data[4].sponsor, player_data[4].character[0] };

                    uoblist4 = new List<object>() { "", "", player_data[1].tag, player_data[1].twitter, player_data[1].region, player_data[1].sponsor, player_data[1].character[0], "",
                                                "", "", player_data[5].tag, player_data[5].twitter, player_data[5].region, player_data[5].sponsor, player_data[5].character[0] };

                    uoblist6 = new List<object>() { "", "", player_data[2].tag, player_data[2].twitter, player_data[2].region, player_data[2].sponsor, player_data[2].character[0], "",
                                                "", "", player_data[6].tag, player_data[6].twitter, player_data[6].region, player_data[6].sponsor, player_data[6].character[0] };

                    uoblist8 = new List<object>() { "", "", player_data[3].tag, player_data[3].twitter, player_data[3].region, player_data[3].sponsor, player_data[3].character[0], "",
                                                "", "", player_data[7].tag, player_data[7].twitter, player_data[7].region, player_data[7].sponsor, player_data[7].character[0] };
                }
                else
                {
                    uoblist2 = new List<object>() { "", "", player_data[0].tag, player_data[0].twitter, player_data[0].region, player_data[0].sponsor, player_data[0].character[0], "",
                                                "", "", "", "", "", "", "" };
                    uoblist4 = new List<object>() { "", "", player_data[1].tag, player_data[1].twitter, player_data[1].region, player_data[1].sponsor, player_data[1].character[0], "",
                                                "", "", "", "", "", "", "" };
                    uoblist6 = new List<object>() { "", "", player_data[2].tag, player_data[2].twitter, player_data[2].region, player_data[2].sponsor, player_data[2].character[0], "",
                                                "", "", "", "", "", "", "" };
                    uoblist8 = new List<object>() { "", "", player_data[3].tag, player_data[3].twitter, player_data[3].region, player_data[3].sponsor, player_data[3].character[0], "",
                                                "", "", "", "", "", "", "" };
                }

                current_info.Values = new List<IList<object>>() { uoblist1, uoblist2, uoblist3, uoblist4 };
                current_info.Range = "Current Round Info!A4:H18";
                current_info.MajorDimension = "COLUMNS";

                data.Add(current_info);
            }


            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>>();

            var oblist = new List<object>() { new_round, new_player1, new_player2, new_player3, new_player4 };
            upcoming.Values.Add(oblist);

            string update_row = (4 + updating_match).ToString();

            upcoming.Range = "Upcoming Matches!B" + update_row + ":F" + update_row;
            upcoming.MajorDimension = "ROWS";

            data.Add(upcoming);

            // TODO: Assign values to desired properties of `requestBody`:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "RAW";
            requestBody.Data = data;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = service.Spreadsheets.Values.BatchUpdate(requestBody, frm_streamqueue.sheets_id);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();

            MessageBox.Show("Match #" + change_match + " updated.\n" +
                            new_round + " - " + new_player1 + " & " + new_player2 + " vs. " + new_player3 + " & " + new_player4);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void add_to_sheets(player_info new_player)
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

            //Set the range to be only the player information
            string range = "Player Information!A2:O" + (frm_main.MAX_PLAYERS + 1).ToString();

            //Set up the request for the sheet
            SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(frm_streamqueue.sheets_id, range);

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
            SpreadsheetsResource.ValuesResource.UpdateRequest update = service.Spreadsheets.Values.Update(data, frm_streamqueue.sheets_id, range2);
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
    }
}
