using System;
using System.Drawing;
using System.Windows.Forms;
using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;

namespace Stream_Info_Handler.SavePlayer
{
    public partial class SavePlayerForm : Form
    {
        updatePlayerInformation updateProcesses;
        string gameDirectory = DirectoryManagement.GetGameDirectory();

        public bool outputIsNewPlayer;
        public PlayerRecordModel outputPlayer;

        public SavePlayerForm(PlayerRecordModel playerToSave, string originalCharacter, int originalColor)
        {
            this.CenterToScreen();

            InitializeComponent();
            this.TopMost = global_values.keepWindowsOnTop;

            updateProcesses = new updatePlayerInformation(playerToSave, gameDirectory, originalCharacter, originalColor);

            //Load in sponsors
            updateProcesses.load_sponsors();
            cbx_fullsponsor.BeginUpdate();
            foreach (string sponsor in updateProcesses.sponsorNames)
            {
                cbx_fullsponsor.Items.Add(sponsor);
            }
            cbx_fullsponsor.EndUpdate();

            //Update text fields with information coming from the player file
            txt_tag.Text = playerToSave.tag;
            cbx_region.Text = playerToSave.region;
            txt_sponsor.Text = playerToSave.sponsor;
            txt_twitter.Text = playerToSave.twitter;
            cbx_fullsponsor.Text = playerToSave.fullSponsor;
            txt_name.Text = playerToSave.fullName;
            txt_elo.Text = playerToSave.elo.ToString();
            txt_misc.Text = playerToSave.misc;
            ckb_wireless.Checked = playerToSave.usingWirelessController;

            //Set playerid and ownerid
            if(playerToSave.id != null && playerToSave.id != "")
            {

                if (updateProcesses.ownerId != global_values.user_id.ToString())
                {
                    btn_save.Text = "Create Local Copy";
                }
                else
                {
                    btn_create.Enabled = true;
                    btn_create.Visible = true;
                    btn_create.Left += 55;
                    btn_save.Left += 55;
                    btn_cancel.Left += 55;
                }
                lbl_playerid.Text = "Player ID: " + playerToSave.id;
                lbl_ownerid.Text = "Owner: " + database_tools.get_owner_name(playerToSave.owningUserId);
                ckb_character.Enabled = true;
            }
            else
            {
                btn_save.Text = "Create New Player";
                lbl_playerid.Text = "No Player ID assigned";
                lbl_ownerid.Text = "Owner: " + database_tools.get_owner_name(global_values.user_id.ToString());
            }

            //Assign the character image to the button
            if(playerToSave.characterName == null)
            {
                playerToSave.characterName = "Random";
                playerToSave.colorNumber = 1;
            }
            string character_path = updateProcesses.gamePath + @"\" + playerToSave.characterName + @"\" + playerToSave.colorNumber + @"\stamp.png";
            btn_character.BackgroundImage = Image.FromFile(character_path);
        }

        public SavePlayerForm(string newName)
        {
            this.CenterToScreen();

            InitializeComponent();
            this.TopMost = global_values.keepWindowsOnTop;

            updateProcesses = new updatePlayerInformation(newName, gameDirectory);

            //Load in sponsors
            updateProcesses.load_sponsors();
            cbx_fullsponsor.BeginUpdate();
            foreach (string sponsor in updateProcesses.sponsorNames)
            {
                cbx_fullsponsor.Items.Add(sponsor);
            }
            cbx_fullsponsor.EndUpdate();

            txt_tag.Text = newName;
            txt_elo.Text = PlayerRecordModel.defaultElo.ToString();

            lbl_playerid.Text = "No Player ID assigned";
            lbl_ownerid.Text = "Owner: " + database_tools.get_owner_name(global_values.user_id.ToString());

            btn_save.Text = "Create New Player";

            string character_path = updateProcesses.gamePath + @"\Random\1\stamp.png";
            btn_character.BackgroundImage = Image.FromFile(character_path);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            if(checkPlayer(updateProcesses.newPlayer))
                updateProcesses.savePlayer(buildPlayer(), updateProcesses.newPlayer, ckb_character.Checked, this);
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if (checkPlayer(updateProcesses.newPlayer))
                updateProcesses.savePlayer(buildPlayer(), true, ckb_character.Checked, this);
        }

        private bool checkPlayer(bool newPlayer)
        {
            if (txt_tag.Text == "")
            {
                System.Media.SystemSounds.Asterisk.Play();
                return false;
            }

            if (newPlayer == true)
                if (database_tools.player_exists(txt_tag.Text, txt_name.Text, updateProcesses.playerGame))
                {
                    MessageBox.Show("A player already exists with this Tag and Name. Please change one of these fields and try again.");
                    return false;
                }
            return true;
        }

        private PlayerRecordModel buildPlayer()
        {
            PlayerRecordModel savePlayer = new PlayerRecordModel();

            savePlayer.tag = txt_tag.Text;
            if (updateProcesses.needsUnique == true)
                savePlayer.uniqueTag = txt_tag.Text + "(" + txt_name.Text + ")";
            else
                savePlayer.uniqueTag = txt_tag.Text;
            savePlayer.twitter = txt_twitter.Text;
            savePlayer.region = cbx_region.Text;
            savePlayer.sponsor = txt_sponsor.Text;
            savePlayer.fullName = txt_name.Text;
            savePlayer.fullSponsor = cbx_fullsponsor.Text;
            savePlayer.elo = Int32.Parse(txt_elo.Text);
            savePlayer.misc = txt_misc.Text;
            savePlayer.usingWirelessController = ckb_wireless.Checked;

            return savePlayer;
        }
        private void txt_tag_TextChanged(object sender, EventArgs e)
        {
            if(txt_tag.Text != "")
            {
                txt_tag.BackColor = Color.White;
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void Cbx_fullsponsor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbx_fullsponsor.SelectedIndex > -1)
            {
                txt_sponsor.Text = updateProcesses.sponsorPrefixes[cbx_fullsponsor.SelectedIndex];
            }
        }

        private void btn_character_Click(object sender, EventArgs e)
        {
            Image newcharacter = updateProcesses.selectCharacter();
            if (newcharacter != null)
                btn_character.BackgroundImage = newcharacter;
        }
    }
}
