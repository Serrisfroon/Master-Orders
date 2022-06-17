using Stream_Info_Handler.CharacterSelect;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using SqlDatabaseLibrary;
using CharacterLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;

namespace Stream_Info_Handler
{
    public partial class frm_playermanager : Form
    {
        public event closedform_event close_form;

        public static string secondary_text = "Add a Secondary";

        string character_directory;
        string roster_selection;
        SortOrder sorting;
        int lastcolumn = -1;
        bool enabled_modify = false;
        List<string> game_directories;
        string character_path = "";

        List<PlayerRecordModel> player_roster = new List<PlayerRecordModel>();
        PlayerRecordModel view_player;
        List<string> player_id;

        string characterName;
        int colorNumber;

        public frm_playermanager()
        {
            InitializeComponent();

         

            //Open the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            //Update the character roster box to contain correct info
            character_directory = (string)xml.Root.Element("directories").Element("character-directory");
            roster_selection = (string)xml.Root.Element("directories").Element("roster-selection");
            int roster_index = 0;
            if (Directory.Exists(character_directory))
            {
                string[] folders;
                try
                {
                    folders = Directory.GetDirectories(character_directory);
                }
                catch (UnauthorizedAccessException)
                {
                    MessageBox.Show("You do not have access to the Character Databases Directory. Please update this in the settings and try again.");
                    this.Close();
                    return;
                }
                game_directories = new List<string>();
                cbx_character_roster.BeginUpdate();                                      //Begin
                cbx_character_roster.Items.Clear();                                      //Empty the item list

                foreach (string folder in folders)
                {
                    int dirlength = character_directory.Length + 1;
                    string gamename = folder.Substring(dirlength, folder.Length - dirlength);
                    game_directories.Add(folder);
                    if (roster_selection == folder)
                        roster_index = cbx_character_roster.Items.Count;
                    cbx_character_roster.Items.Add(gamename);
                }
                cbx_character_roster.EndUpdate();                                        //End
                cbx_character_roster.SelectedIndex = roster_index;                                  //Set the combobox index to 0
            }

        }

        private void frm_playermanager_FormClosed(object sender, FormClosedEventArgs e)
        {
            global_values.playermanager_form = null;
            close_form(3);
        }

        private void cbx_character_roster_SelectedIndexChanged(object sender, EventArgs e)
        {
            character_path = game_directories[((ComboBox)sender).SelectedIndex];
            btn_character.BackgroundImage = null;
            btn_character.Text = "Click to Select a Character";

            update_players();


        }


        private void update_players()
        {
            this.lvw_players.ListViewItemSorter = null;
            string[] folders = DirectoryManagement.GetCharactersFromDirectory(character_path);
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            string CheckGameName = DirectoryManagement.VerifyGameDirectory(cbx_character_roster.Text, (string)xml.Root.Element("directories").Element("character-directory"));

            PlayerDatabase.LoadPlayers(CheckGameName);

            lvw_players.BeginUpdate();
            lvw_players.Items.Clear();
            foreach(PlayerRecordModel playerRecord in PlayerDatabase.playerRecords)
            {
                ListViewItem add_item = new ListViewItem(playerRecord.elo.ToString());
                add_item.SubItems.Add(playerRecord.tag);
                add_item.SubItems.Add(playerRecord.sponsor);
                add_item.SubItems.Add(playerRecord.twitter);
                add_item.SubItems.Add(playerRecord.fullName);
                add_item.SubItems.Add(playerRecord.region);
                add_item.SubItems.Add(playerRecord.characterName);
                add_item.SubItems.Add(playerRecord.misc);
                lvw_players.Items.Add(add_item);
            }
            lvw_players.EndUpdate();
        }


        private void lvw_players_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.lvw_players.ListViewItemSorter == null)
            {
                this.lvw_players.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending);
                sorting = SortOrder.Ascending;
            }
            else
            {
                if(e.Column != lastcolumn)
                {
                    this.lvw_players.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending);
                    sorting = SortOrder.Ascending;
                }
                else
                {
                    if(sorting == SortOrder.Ascending)
                    {
                        this.lvw_players.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Descending);
                        sorting = SortOrder.Descending;
                    }
                    else
                    {
                        this.lvw_players.ListViewItemSorter = new ListViewItemComparer(e.Column, SortOrder.Ascending);
                        sorting = SortOrder.Ascending;
                    }
                }
            }
            lastcolumn = e.Column;
        }


        // Implements the manual sorting of items by columns.
        class ListViewItemComparer : IComparer
        {
            private int col;
            public SortOrder order;
            public ListViewItemComparer()
            {
                col = 0;
                order = SortOrder.Ascending;
            }
            public ListViewItemComparer(int column, SortOrder order)
            {
                col = column;
                this.order = order;
            }
            public int Compare(object x, object y)
            {
                int returnVal =  String.Compare(((ListViewItem)x).SubItems[col].Text, ((ListViewItem)y).SubItems[col].Text);

                // Determine whether the sort order is descending.
                if (order == SortOrder.Descending)
                {
                    // Invert the value returned by String.Compare.
                    returnVal *= -1;
                }

                return returnVal;
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            update_players();
        }

        private void lvw_players_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvw_players.SelectedIndices.Count == 0)
                return;

            btn_editplayer.Enabled = true;
            btn_removeplayer.Enabled = true;

            view_player = player_roster[lvw_players.SelectedIndices[0]];

            txt_tag.Text = view_player.tag;
            txt_twitter.Text = view_player.twitter;
            txt_elo.Text = view_player.elo.ToString();
            txt_fullsponsor.Text = view_player.fullSponsor;
            txt_sponsor.Text = view_player.sponsor;
            txt_misc.Text = view_player.misc;
            txt_name.Text = view_player.fullName;
            lbl_playerid.Text = "Player ID: " + view_player.id;
            lbl_ownerid.Text = "Owner: " + PlayerDatabase.GetOwnerName(view_player.owningUserId);
            if (view_player.characterName != "")
            {
                characterName = view_player.characterName;
                colorNumber = view_player.colorNumber;
                btn_character.BackgroundImage = Image.FromFile(character_path + @"\" + characterName + @"\" + colorNumber + @"\stamp.png");
                btn_character.Text = "";
            }
            else
            {
                btn_character.BackgroundImage = null;
                btn_character.Text = "Click to Select a Character";
            }
        }

        private void load_combobox_images(ref ComboBox sender, string character_name)
        {
            sender.Items.Clear();
            string colors_path = character_path + @"\" + character_name;
            int colors_count = Directory.GetDirectories(colors_path).Length;
            Image[] colors = new Image[colors_count];

            for (int i = 0; i < colors_count; i++)
            {
                colors[i] = Image.FromFile(colors_path + @"\" + (i + 1).ToString() + @"\stamp.png");
            }

            sender.DisplayImages(colors, 65);
            sender.SelectedIndex = 0;
            sender.DropDownHeight = 400;
        }

        private void enable_boxes(bool enable)
        {
            enabled_modify = enable;
            txt_elo.Enabled = enable;
            txt_fullsponsor.Enabled = enable;
            txt_misc.Enabled = enable;
            txt_name.Enabled = enable;
            txt_sponsor.Enabled = enable;
            txt_tag.Enabled = enable;
            txt_twitter.Enabled = enable;
            cbx_region.Enabled = enable;
            btn_character.Enabled = enable;
            if (enable == true)
            {
                lbl_playerid.Text = "Player ID: " + view_player.id;
                lbl_ownerid.Text = "Owner: " + PlayerDatabase.GetOwnerName(view_player.owningUserId);
            }   
            
            btn_cancel.Enabled = enable;
            btn_update.Enabled = enable;

            btn_addplayer.Enabled = !enable;
            btn_editplayer.Enabled = !enable;
            btn_removeplayer.Enabled = !enable;
            lvw_players.Enabled = !enable;
            cbx_character_roster.Enabled = !enable;
            btn_refresh.Enabled = !enable;
            btn_update.Text = "Update";
        }

        private void Btn_editplayer_Click(object sender, EventArgs e)
        {
            enable_boxes(true);
        }

        private void Btn_cancel_Click(object sender, EventArgs e)
        {
            enable_boxes(false);
            lvw_players_SelectedIndexChanged(sender, e);
        }

        private void Btn_removeplayer_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show("Are you sure you want to delete the player '" + view_player.uniqueTag + "' from the database?", "Delete Player", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                PlayerDatabase.RemovePlayer(view_player);
                update_players();
            }
        }

        private void btn_addplayer_Click(object sender, EventArgs e)
        {
            view_player = new PlayerRecordModel();
            view_player.id = "";
            view_player.owningUserId = UserSession.userId.ToString();
            enable_boxes(true);
            lbl_playerid.Text = "No Player ID assigned";
            txt_elo.Text = PlayerRecordModel.defaultElo.ToString();
            txt_fullsponsor.Text = "";
            txt_misc.Text = "";
            txt_name.Text = "";
            txt_sponsor.Text = "";
            txt_tag.Text = "";
            txt_twitter.Text = "";
            cbx_region.Text = "";
            btn_character.BackgroundImage = null;
            btn_character.Text = "Click to Select a Character";

            btn_update.Text = "Create Player";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (txt_tag.Text == "")
            {
                MessageBox.Show("A player needs a tag to be saved into Master Orders.");
                return;
            }


            PlayerRecordModel save_player = new PlayerRecordModel();
            string ownerid = view_player.owningUserId;
            bool new_player = false;

            //Generate a new playerid if this is a new player
            if (btn_update.Text == "Create Player")
            {
                new_player = true;
                if (PlayerDatabase.PlayerExists(txt_tag.Text, txt_name.Text, cbx_character_roster.Text))
                {
                    MessageBox.Show("A player already exists with this Tag and Name. Please change one of these fields and try again.");
                    return;
                }
            }

            save_player.owningUserId = ownerid;

            //Check if this is being saved as a copy
            if (ownerid == UserSession.userId.ToString())
                save_player.duplicateRecord = false;
            else
                save_player.duplicateRecord = true;
            save_player.tag = txt_tag.Text;
            save_player.twitter = txt_twitter.Text;
            save_player.region = cbx_region.Text;
            save_player.sponsor = txt_sponsor.Text;
            save_player.fullName = txt_name.Text;
            save_player.fullSponsor = txt_fullsponsor.Text;
            save_player.elo = Int32.Parse(txt_elo.Text);
            save_player.misc = txt_misc.Text;
            save_player.game = cbx_character_roster.Text;
            save_player.usingWirelessController = ckb_wireless.Checked;

            save_player.characterName = characterName;
            save_player.colorNumber = colorNumber;

            PlayerDatabase.AddPlayer(save_player, new_player);
            update_players();
            enable_boxes(false);
        }

        private void btn_character_Click(object sender, EventArgs e)
        {
            Image newcharacter = selectCharacter();
            if (newcharacter != null)
                btn_character.BackgroundImage = newcharacter;
        }

        public Image selectCharacter()
        {
            CharacterData playerCharacter = new CharacterData(characterName, colorNumber);
            playerCharacter = GenerateCharacters.GetCharacter(playerCharacter, DirectoryManagement.GetGameDirectory());
            characterName = playerCharacter.characterName;
            colorNumber = playerCharacter.characterColor;

            if (characterName != null && colorNumber > 0)
            {
                Image newCharacter = Image.FromFile(character_path + @"\" +
                                        characterName + @"\" + colorNumber.ToString() + @"\stamp.png");

                return newCharacter;
            }
            else
                return null;
        }
    }
}


