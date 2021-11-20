using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.StreamAssistant.DataManagement;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using YoutubeLibrary;

namespace Stream_Info_Handler.AppSettings.GeneralSettings
{
    public static class LoadSettingsFormControls

    {
        public static GeneralSettingsForm settingsForm;
        public static void InitializeImageScoreControls()
        {
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(0, settingsForm.btn_score1_image1, settingsForm.pic_score1_image1));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(1, settingsForm.btn_score1_image2, settingsForm.pic_score1_image2));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(2, settingsForm.btn_score1_image3, settingsForm.pic_score1_image3));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(3, settingsForm.btn_score2_image1, settingsForm.pic_score2_image1));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(4, settingsForm.btn_score2_image2, settingsForm.pic_score2_image2));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(5, settingsForm.btn_score2_image3, settingsForm.pic_score2_image3));
        }
        public static void LoadStreamQueues(ComboBox queueComboBox, List<StreamQueueModel> streamQueues)
        {
            //Load queue names
            queueComboBox.BeginUpdate();
            queueComboBox.Items.Clear();                                            //Empty the item list
            queueComboBox.Items.Add("None");
            for (int i = 0; i < streamQueues.Count; i++)
            {
                queueComboBox.Items.Add(streamQueues[i].queueName);
            }
            queueComboBox.EndUpdate();
        }

        public static void LoadGameTitles(ComboBox gameComboBox, string[] gameNames, string firstItem)
        {
            gameComboBox.BeginUpdate();
            gameComboBox.Items.Clear();                                            //Empty the item list
            gameComboBox.Items.Add(firstItem);

            foreach (string gameName in gameNames)
            {
                gameComboBox.Items.Add(gameName);
            }
            gameComboBox.EndUpdate();
        }

        public static void LoadSettingsFields()
        {
            SettingsFile.LoadSettings(Startup.FormManagement.FormNames.Settings);
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);

            settingsForm.txt_characters.Text = DirectoryManagement.characterRostersDirectory;
            if (settingsForm.txt_characters.Text != "" && Directory.Exists(settingsForm.txt_characters.Text))
            {
                //This method should not load directly from the settings file
                //Use SettingsFile.LoadSettings to read the setting file and just save it to the appropriate values
                //DONE FINALLY LOL

                //Read those values and adjust the fields to the form here.
                //THIS IS DONE NOW TOO

                //create/update the method to verify that a selected directory holds all appropriate information
                //DirectoryManagement.VerifyGameDirectory
                //DO NOT FORGET TO DO THIS
                //Check all references to see what they are doing and ensure the method is performing proper checks

                //Do the tooptips function next
                if (Directory.Exists(DirectoryManagement.GetGameDirectory()))
                {
                    settingsForm.cbx_characters.SelectedIndex = settingsForm.cbx_characters.FindStringExact(GlobalSettings.selectedGame);
                }
            }
            if (StreamQueue.queueId == -1)
                settingsForm.cbx_queues.SelectedIndex = 0;
            else
                settingsForm.cbx_queues.SelectedIndex = StreamQueue.queueId;

            settingsForm.txt_streamfiles.Text = DirectoryManagement.outputDirectory;
            settingsForm.txt_thumbnails.Text = DirectoryManagement.thumbnailDirectory;
            settingsForm.ckb_sponsors.Checked = ImageManagement.enableSponsorImages;
            settingsForm.ckb_regions.Checked = ImageManagement.enableRegionImages;
            settingsForm.txt_sponsors.Text = DirectoryManagement.sponsorDirectory;
            settingsForm.txt_regions.Text = DirectoryManagement.regionDirectory;
            settingsForm.txt_vods.Text = DirectoryManagement.vodsDirectory;

            settingsForm.ckb_vod_uploads.Checked = YoutubeController.enableYoutubeFunctions;
            settingsForm.editableSettings.playlistId = YoutubeController.playlistId;
            settingsForm.txt_description.Text = YoutubeController.videoDescription.Replace("\n", "\r\n");
            settingsForm.txt_tags.Text = YoutubeController.videoTags.Replace("\n", "\r\n");
            settingsForm.txt_playlist.Text = YoutubeController.playlistName;
            settingsForm.btn_playlist.Enabled = false;
            settingsForm.txt_titletemplate.Text = YoutubeController.titleTemplate;

            settingsForm.ckb_scoreboad.Checked = ImageManagement.enableImageScoreboard;
            
            for (int i = 0; i < 2; i++)
            {
                for (int ii = 0; ii < 3; ii++)
                {
                    string scoreImage = ImageManagement.scoreboardImages[i, ii];
                    if (File.Exists(scoreImage) == true)
                    {
                        settingsForm.editableSettings.scoreControls[(i*3)+ii].UpdateImage(scoreImage);
                    }
                }
            }

            settingsForm.txt_background.Text = ImageManagement.thumbnailConfiguration.backgroundImage;
            settingsForm.txt_foreground.Text = ImageManagement.thumbnailConfiguration.foregroundImage;
            settingsForm.editableSettings.thumbnailFont = new Font(ImageManagement.thumbnailConfiguration.thumbnailFont, 12, FontStyle.Regular);
            settingsForm.txt_char1_xoffset.Text = ImageManagement.thumbnailConfiguration.characterXOffset[0].ToString();
            settingsForm.txt_char1_yoffset.Text = ImageManagement.thumbnailConfiguration.characterYOffset[0].ToString();
            settingsForm.txt_char2_xoffset.Text = ImageManagement.thumbnailConfiguration.characterXOffset[1].ToString();
            settingsForm.txt_char2_yoffset.Text = ImageManagement.thumbnailConfiguration.characterYOffset[1].ToString();
            settingsForm.txt_name1_xoffset.Text = ImageManagement.thumbnailConfiguration.playerNameXOffset[0].ToString();
            settingsForm.txt_name1_yoffset.Text = ImageManagement.thumbnailConfiguration.playerNameYOffset[0].ToString();
            settingsForm.txt_name1_size.Text = ImageManagement.thumbnailConfiguration.playerNameSize[0].ToString();
            settingsForm.txt_name2_xoffset.Text = ImageManagement.thumbnailConfiguration.playerNameXOffset[1].ToString();
            settingsForm.txt_name2_yoffset.Text = ImageManagement.thumbnailConfiguration.playerNameYOffset[1].ToString();
            settingsForm.txt_name2_size.Text = ImageManagement.thumbnailConfiguration.playerNameSize[1].ToString();
            settingsForm.txt_round_xoffset.Text = ImageManagement.thumbnailConfiguration.roundXOffset.ToString();
            settingsForm.txt_round_yoffset.Text = ImageManagement.thumbnailConfiguration.roundYOffset.ToString();
            settingsForm.txt_round_size.Text = ImageManagement.thumbnailConfiguration.roundSize.ToString();
            settingsForm.ckb_date.Checked = ImageManagement.thumbnailConfiguration.showDateOnThumbnail;
            settingsForm.txt_date_xoffset.Text = ImageManagement.thumbnailConfiguration.dateXOffset.ToString();
            settingsForm.txt_date_yoffset.Text = ImageManagement.thumbnailConfiguration.dateYOffset.ToString();
            settingsForm.txt_date_size.Text = ImageManagement.thumbnailConfiguration.dateSize.ToString();
            settingsForm.txt_version.Text = ImageManagement.thumbnailConfiguration.patchVersion.ToString();
            settingsForm.txt_patch_xoffset.Text = ImageManagement.thumbnailConfiguration.patchXOffset.ToString();
            settingsForm.txt_patch_yoffset.Text = ImageManagement.thumbnailConfiguration.patchYOffset.ToString();
            settingsForm.txt_patch_size.Text = ImageManagement.thumbnailConfiguration.patchSize.ToString();

            settingsForm.rdb_automatic.Checked = DataOutputCaller.automaticUpdates;
            settingsForm.ckb_ontop.Checked = GlobalSettings.keepWindowsOnTop;

            if (YoutubeController.streamSoftware == "OBS")
                settingsForm.rdb_obs.Checked = true;
            settingsForm.ckb_thumbnails.Checked = YoutubeController.enableVideoThumbnails;
            settingsForm.ckb_clipboard.Checked = YoutubeController.copyVideoTitle;
            settingsForm.cbx_shorten_video.SelectedIndex = (int)YoutubeController.enableVideoTitleShortening;
            settingsForm.cbx_format.Text = GlobalSettings.bracketFormat;
            settingsForm.txt_bracketrounds.Text = GlobalSettings.bracketRounds;
            settingsForm.txt_seperator.Text = TextFileManagement.sponsorSeperator;

            settingsForm.btn_apply.Enabled = false;

            if (settingsForm.txt_characters.Text == "")
                InitialImportSettings();
        }

        public static void LoadToolTips()
        {

        }

        private static void InitialImportSettings()
        {
            if (MessageBox.Show("Do you want to import settings from a folder?", "Import Settings", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            if (settingsForm.fbd_directory.ShowDialog() != DialogResult.OK)
                return;

            string settings_dir = settingsForm.fbd_directory.SelectedPath + @"\";

            XDocument xml = XDocument.Load(settings_dir + @"configure_settings.xml");

            if (xml.Root.Element("characters").IsEmpty == false)
                settingsForm.txt_characters.Text = settings_dir + (string)xml.Root.Element("characters");
            if (xml.Root.Element("stream").IsEmpty == false)
                settingsForm.txt_streamfiles.Text = settings_dir + (string)xml.Root.Element("stream");
            if (xml.Root.Element("thumbnails").IsEmpty == false)
                settingsForm.txt_thumbnails.Text = settings_dir + (string)xml.Root.Element("thumbnails");
            if (xml.Root.Element("sponsors").IsEmpty == false)
                settingsForm.txt_sponsors.Text = settings_dir + (string)xml.Root.Element("sponsors");
            if (xml.Root.Element("regions").IsEmpty == false)
                settingsForm.txt_regions.Text = settings_dir + (string)xml.Root.Element("regions");
            if (xml.Root.Element("vods").IsEmpty == false)
                settingsForm.txt_vods.Text = settings_dir + (string)xml.Root.Element("vods");
            if (xml.Root.Element("json").IsEmpty == false)
                settingsForm.ckb_vod_uploads.Checked = true;
            if (xml.Root.Element("foreground").IsEmpty == false)
                settingsForm.txt_foreground.Text = settings_dir + (string)xml.Root.Element("foreground");
            if (xml.Root.Element("background").IsEmpty == false)
                settingsForm.txt_background.Text = settings_dir + (string)xml.Root.Element("background");
            if (xml.Root.Element("patch").IsEmpty == false)
                settingsForm.txt_version.Text = settings_dir + (string)xml.Root.Element("patch");


            settingsForm.ckb_thumbnails.Checked = true;
            settingsForm.ckb_date.Checked = true;

            if (xml.Root.Element("scores").IsEmpty == false)
            {

                for (int i = 0; i < 2; i++)
                {
                    for (int ii = 0; ii < 3; ii++)
                    {
                        string scoreImage = settings_dir + (string)xml.Root.Element("scores") + @"\player{ i + 1 }_{ ii + 1 }";
                        if (File.Exists(scoreImage) == true)
                        {
                            settingsForm.editableSettings.scoreControls[(i * 3) + ii].UpdateImage(scoreImage);
                        }
                    }
                }
            }
            MessageBox.Show((string)xml.Root.Element("message"));

            settingsForm.btn_oauth_Click(new object(), new EventArgs());
        }

    }
}
