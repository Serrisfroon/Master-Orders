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
    public partial class frm_edit_match : Form
    {
        string change_match;

        public frm_edit_match(string match, string round, string player1, string player2, ComboBox.ObjectCollection players)
        {
            InitializeComponent();
            cbx_round.Text = round;
            cbx_player1.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player2.Items.AddRange(players.Cast<Object>().ToArray());
            cbx_player1.Text = player1;
            cbx_player2.Text = player2;

            change_match = match;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btn_apply_Click(object sender, EventArgs e)
        {
            if (cbx_round.Text == "" || cbx_player1.Text == "" || cbx_player2.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return;
            }
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


            // The new values to apply to the spreadsheet.
            Google.Apis.Sheets.v4.Data.ValueRange upcoming = new Google.Apis.Sheets.v4.Data.ValueRange();
            upcoming.Values = new List<IList<object>>();

            var oblist = new List<object>() { new_round, new_player1, new_player2 };
            upcoming.Values.Add(oblist);

            string update_row = (4 + Int32.Parse(change_match)).ToString();

            upcoming.Range = "Upcoming Matches!B" + update_row + ":D" + update_row;
            upcoming.MajorDimension = "ROWS";

            List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>() { upcoming };  // TODO: Update placeholder value.

            // TODO: Assign values to desired properties of `requestBody`:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
            requestBody.ValueInputOption = "RAW";
            requestBody.Data = data;

            SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = service.Spreadsheets.Values.BatchUpdate(requestBody, frm_streamqueue.sheets_id);

            // To execute asynchronously in an async method, replace `request.Execute()` as shown:
            Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();

            MessageBox.Show("Match #" + change_match + " updated.\n" +
                            new_round + " - " + new_player1 + " vs. " + new_player2);
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
