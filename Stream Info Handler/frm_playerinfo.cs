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
using System.Xml.Linq;

namespace Stream_Info_Handler
{
    public partial class frm_playerinfo : Form
    {
        string sheet_id;

        private static frm_playerinfo instance;
        public frm_playerinfo()
        {
            InitializeComponent();
            XDocument xml = XDocument.Load(global_values.settings_file);
            sheet_id = (string)xml.Root.Element("google-sheets").Element("sheets-id");
        }

        public static frm_playerinfo GetInstance
        {
            get
            {
                if (instance == null || instance.IsDisposed)
                {
                    instance = new frm_playerinfo();
                }
                return instance;
            }
        }

        private player_info _player1_name = new player_info();
        public player_info player1_name
        {
            get { return _player1_name; }
            set
            {
                _player1_name = value;
                lvw_playerinfo.Items[0].SubItems[1].Text = _player1_name.tag;
                lvw_playerinfo.Items[1].SubItems[1].Text = _player1_name.twitter;
                lvw_playerinfo.Items[2].SubItems[1].Text = _player1_name.region;
                lvw_playerinfo.Items[3].SubItems[1].Text = _player1_name.sponsor;
                lvw_playerinfo.Items[4].SubItems[1].Text = _player1_name.character[0];
            }
        }

        private player_info _player2_name = new player_info();
        public player_info player2_name
        {
            get { return _player1_name; }
            set
            {
                _player2_name = value;
                lvw_playerinfo.Items[0].SubItems[2].Text = _player2_name.tag;
                lvw_playerinfo.Items[1].SubItems[2].Text = _player2_name.twitter;
                lvw_playerinfo.Items[2].SubItems[2].Text = _player2_name.region;
                lvw_playerinfo.Items[3].SubItems[2].Text = _player2_name.sponsor;
                lvw_playerinfo.Items[4].SubItems[2].Text = _player2_name.character[0];
            }
        }

        private void btn_player1_Click(object sender, EventArgs e)
        {
            player1_name = update_player(_player1_name);
        }

        private void btn_player2_Click(object sender, EventArgs e)
        {
            player2_name = update_player(_player2_name);
        }

        private player_info update_player(player_info updating_player)
        {
            int change_roster = -1;

            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == updating_player.tag)
                {
                    change_roster = i;
                    break;
                }
            }

            var player_info_box = new frm_save_player(updating_player);
            if (player_info_box.ShowDialog() == DialogResult.OK)
            {
                global_values.roster[change_roster] = frm_main.get_new_player;

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

                // Define request parameters.
                String spreadsheetId = sheet_id;

                //Set the range to be only the player information
                string range = "Current Round Info!A6:D18";

                //Set up the request for the sheet
                SpreadsheetsResource.ValuesResource.GetRequest request = service.Spreadsheets.Values.Get(spreadsheetId, range);

                //Receive the player information from the request
                Google.Apis.Sheets.v4.Data.ValueRange response = request.Execute();
                //Place the information in an array
                IList<IList<Object>> player_information = response.Values;

                //Add the new player's information to an array.
                var oblist = new List<object>() { global_values.roster[change_roster].tag,
                                                global_values.roster[change_roster].twitter,
                                                global_values.roster[change_roster].region,
                                                global_values.roster[change_roster].sponsor,
                                                global_values.roster[change_roster].character[0],
                                                global_values.roster[change_roster].character[1],
                                                global_values.roster[change_roster].character[2],
                                                global_values.roster[change_roster].character[3],
                                                global_values.roster[change_roster].character[4],
                                                global_values.roster[change_roster].color[0],
                                                global_values.roster[change_roster].color[1],
                                                global_values.roster[change_roster].color[2],
                                                global_values.roster[change_roster].color[3],
                                                global_values.roster[change_roster].color[4]};

                var player_oblist = new List<object>() { global_values.roster[change_roster].tag,
                                                global_values.roster[change_roster].twitter,
                                                global_values.roster[change_roster].region,
                                                global_values.roster[change_roster].sponsor,
                                                global_values.roster[change_roster].character[0] };

                List<Google.Apis.Sheets.v4.Data.ValueRange> data = new List<Google.Apis.Sheets.v4.Data.ValueRange>();

                if (player_information[0][1].ToString() == global_values.roster[change_roster].tag)
                {
                    Google.Apis.Sheets.v4.Data.ValueRange update_player_range = new Google.Apis.Sheets.v4.Data.ValueRange();
                    update_player_range.Values = new List<IList<object>> { player_oblist };
                    //Set the range's row to the player index"
                    update_player_range.Range = "Current Round Info!B6:B10";
                    update_player_range.MajorDimension = "COLUMNS";

                    data.Add(update_player_range);
                }

                if (player_information[0][3].ToString() == global_values.roster[change_roster].tag)
                {
                    Google.Apis.Sheets.v4.Data.ValueRange update_player_range = new Google.Apis.Sheets.v4.Data.ValueRange();
                    update_player_range.Values = new List<IList<object>> { player_oblist };
                    //Set the range's row to the player index"
                    update_player_range.Range = "Current Round Info!D6:D10";
                    update_player_range.MajorDimension = "COLUMNS";

                    data.Add(update_player_range);
                }

                if (player_information[8][1].ToString() == global_values.roster[change_roster].tag)
                {
                    Google.Apis.Sheets.v4.Data.ValueRange update_player_range = new Google.Apis.Sheets.v4.Data.ValueRange();
                    update_player_range.Values = new List<IList<object>> { player_oblist };
                    //Set the range's row to the player index"
                    update_player_range.Range = "Current Round Info!B14:B18";
                    update_player_range.MajorDimension = "COLUMNS";

                    data.Add(update_player_range);
                }

                if (player_information[8][3].ToString() == global_values.roster[change_roster].tag)
                {
                    Google.Apis.Sheets.v4.Data.ValueRange update_player_range = new Google.Apis.Sheets.v4.Data.ValueRange();
                    update_player_range.Values = new List<IList<object>> { player_oblist };
                    //Set the range's row to the player index"
                    update_player_range.Range = "Current Round Info!D14:D18";
                    update_player_range.MajorDimension = "COLUMNS";

                    data.Add(update_player_range);
                }


                //Create a data set from the array
                Google.Apis.Sheets.v4.Data.ValueRange data_range = new Google.Apis.Sheets.v4.Data.ValueRange();
                data_range.Values = new List<IList<object>> { oblist };
                //Set the range's row to the player index"
                data_range.Range = "Player Information!A" + (change_roster + 2).ToString() + ":N" + (change_roster + 2).ToString();
                data_range.MajorDimension = "ROWS";

                data.Add(data_range);

                // TODO: Assign values to desired properties of `requestBody`:
                Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest requestBody = new Google.Apis.Sheets.v4.Data.BatchUpdateValuesRequest();
                requestBody.ValueInputOption = "RAW";
                requestBody.Data = data;

                SpreadsheetsResource.ValuesResource.BatchUpdateRequest request2 = service.Spreadsheets.Values.BatchUpdate(requestBody, sheet_id);

                // To execute asynchronously in an async method, replace `request.Execute()` as shown:
                Google.Apis.Sheets.v4.Data.BatchUpdateValuesResponse response2 = request2.Execute();

                return frm_main.get_new_player;
            }
            else
            {
                return updating_player;
            }
        }
    }
}
