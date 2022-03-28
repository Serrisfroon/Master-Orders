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
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(0, settingsForm.btnScoreboardPlayer1Image1, settingsForm.pic_score1_image1));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(1, settingsForm.btnScoreboardPlayer1Image2, settingsForm.pic_score1_image2));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(2, settingsForm.btnScoreboardPlayer1Image3, settingsForm.pic_score1_image3));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(3, settingsForm.btnScoreboardPlayer2Image1, settingsForm.pic_score2_image1));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(4, settingsForm.btnScoreboardPlayer2Image2, settingsForm.pic_score2_image2));
            settingsForm.editableSettings.scoreControls.Add(new ScoreControlModel(5, settingsForm.btnScoreboardPlayer2Image3, settingsForm.pic_score2_image3));
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

            settingsForm.txtCharacterDatabasesDirectory.Text = DirectoryManagement.gamesDirectory;
            if (settingsForm.txtCharacterDatabasesDirectory.Text != "" && Directory.Exists(settingsForm.txtCharacterDatabasesDirectory.Text))
            {
                if (Directory.Exists(DirectoryManagement.GetGameDirectory()))
                {
                    settingsForm.cbxCharacterRosters.SelectedIndex = settingsForm.cbxCharacterRosters.FindStringExact(GlobalSettings.selectedGame);
                }
            }
            if (StreamQueueManager.queueId == -1)
                settingsForm.cbxStreamQueues.SelectedIndex = 0;
            else
                settingsForm.cbxStreamQueues.SelectedIndex = StreamQueueManager.queueId;

            settingsForm.txtStreamFilesDirectory.Text = DirectoryManagement.outputDirectory;
            settingsForm.txt_thumbnails.Text = DirectoryManagement.thumbnailDirectory;
            settingsForm.ckbEnableSponsorImages.Checked = ImageManagement.enableSponsorImages;
            settingsForm.ckbEnableRegionImages.Checked = ImageManagement.enableRegionImages;
            settingsForm.txtSponsorImagesDirectory.Text = DirectoryManagement.sponsorDirectory;
            settingsForm.txtRegionImagesDirectory.Text = DirectoryManagement.regionDirectory;
            settingsForm.txtVodsDirectory.Text = DirectoryManagement.vodsDirectory;

            settingsForm.ckbEnableVodUploads.Checked = YoutubeController.enableYoutubeFunctions;
            settingsForm.editableSettings.playlistId = YoutubeController.playlistId;
            settingsForm.txt_description.Text = YoutubeController.videoDescription.Replace("\n", "\r\n");
            settingsForm.txt_tags.Text = YoutubeController.videoTags.Replace("\n", "\r\n");
            settingsForm.txtPlaylistName.Text = YoutubeController.playlistName;
            settingsForm.btnUpdatePlaylistName.Enabled = false;
            settingsForm.txt_titletemplate.Text = YoutubeController.titleTemplate;

            settingsForm.ckbEnableImageScoreboard.Checked = ImageManagement.enableImageScoreboard;
            
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

            settingsForm.txtThumbnailBackground.Text = ImageManagement.thumbnailConfiguration.backgroundImage;
            settingsForm.txtThumbnailForeground.Text = ImageManagement.thumbnailConfiguration.foregroundImage;
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

            settingsForm.rdbAutomaticStreamUpdates.Checked = DataOutputCaller.automaticUpdates;
            settingsForm.ckbKeepWindowOnTop.Checked = GlobalSettings.keepWindowsOnTop;

            if (YoutubeController.streamSoftware == "OBS")
                settingsForm.rdbStreamSoftwareObs.Checked = true;
            settingsForm.ckb_thumbnails.Checked = YoutubeController.enableVideoThumbnails;
            settingsForm.ckb_clipboard.Checked = YoutubeController.copyVideoTitle;
            settingsForm.cbx_shorten_video.SelectedIndex = (int)YoutubeController.enableVideoTitleShortening;
            settingsForm.cbx_format.Text = GlobalSettings.bracketFormat;
            settingsForm.txtBracketRoundsFile.Text = GlobalSettings.bracketRounds;
            settingsForm.txt_seperator.Text = TextFileManagement.sponsorSeperator;

            settingsForm.btnApplyChanges.Enabled = false;

            if (settingsForm.txtCharacterDatabasesDirectory.Text == "")
                InitialImportSettings();
        }

        /// <summary>
        /// Attempts to help the user import settings when there is no games directory selected.
        /// Typically this means it's the user's first time opening settings
        /// </summary>
        private static void InitialImportSettings()
        {
            if (MessageBox.Show("Do you want to import settings from a folder?", "Import Settings", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            if (settingsForm.fbdBrowserForDirectory.ShowDialog() != DialogResult.OK)
                return;

            string settings_dir = settingsForm.fbdBrowserForDirectory.SelectedPath + @"\";

            XDocument xml = XDocument.Load(settings_dir + @"configure_settings.xml");

            if (xml.Root.Element("characters").IsEmpty == false)
                settingsForm.txtCharacterDatabasesDirectory.Text = settings_dir + (string)xml.Root.Element("characters");
            if (xml.Root.Element("stream").IsEmpty == false)
                settingsForm.txtStreamFilesDirectory.Text = settings_dir + (string)xml.Root.Element("stream");
            if (xml.Root.Element("thumbnails").IsEmpty == false)
                settingsForm.txt_thumbnails.Text = settings_dir + (string)xml.Root.Element("thumbnails");
            if (xml.Root.Element("sponsors").IsEmpty == false)
                settingsForm.txtSponsorImagesDirectory.Text = settings_dir + (string)xml.Root.Element("sponsors");
            if (xml.Root.Element("regions").IsEmpty == false)
                settingsForm.txtRegionImagesDirectory.Text = settings_dir + (string)xml.Root.Element("regions");
            if (xml.Root.Element("vods").IsEmpty == false)
                settingsForm.txtVodsDirectory.Text = settings_dir + (string)xml.Root.Element("vods");
            if (xml.Root.Element("json").IsEmpty == false)
                settingsForm.ckbEnableVodUploads.Checked = true;
            if (xml.Root.Element("foreground").IsEmpty == false)
                settingsForm.txtThumbnailForeground.Text = settings_dir + (string)xml.Root.Element("foreground");
            if (xml.Root.Element("background").IsEmpty == false)
                settingsForm.txtThumbnailBackground.Text = settings_dir + (string)xml.Root.Element("background");
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
        }

    }
}
