using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stream_Info_Handler.AppSettings.GeneralSettings
{
    public static class LoadToolTips
    {
        public static void Initialize(GeneralSettingsForm settingsForm)
        {
            LoadGeneralToolTips(settingsForm);
            LoadStreamGeneralToolTips(settingsForm);
            LoadStreamYouTubeToolTips(settingsForm);
            LoadStreamImagesToolTips(settingsForm);
            LoadStreamThumbnailToolTips(settingsForm);

            //
            //Set the tooltips for the YouTube subtab
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_playlist,
            "Enter the name of the YouTube playlist. Leave this\n" +
            "empty to disable playlists.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_playlist,
            "Update the YouTube playlist to the entered playlist name.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_description,
            "Set the default description for uploaded YouTube VoDs.\n" +
            "You can use the following keywords to insert data from\n" +
            "Master Orders:\n" +
            "INFO_TOURNAMENT INFO_DATE INFO_BRACKET INFO_ROUND\n" +
            "INFO_PLAYER1 INFO_PLAYER2 INFO_CHARACTER1 INFO_CHARACTER2\n" +
            "INFO_TWITTER1 INFO_TWITTER2");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_xsplit,
            "Set the streaming application to XSplit.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_obs,
            "Set the streaming application to OBS Studio.");
        }

        public static void LoadGeneralToolTips(GeneralSettingsForm settingsForm)
        {
            //Set the tooltips for the General tab
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtCharacterDatabasesDirectory,
            "Set the directory location of the Character\n" +
            "Roster Directories.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnBrowseCharacterRostersDirectory,
            "Choose the directory location of the Character\n" +
            "Roster Directories.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbxCharacterRosters,
            "Select the game to use with Master Orders. If\n" +
            "the game hasn't been configured yet, you'll be\n" +
            "asked to select a directory for the game's character\n" +
            "roster.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnReassignCharacterDirectory,
            "Reassign the directory containing the character roster\n" +
            "for this game.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_queues,
            "Set the stream queue to use for the Stream and Bracket\n" +
            "Assistants.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_queue_rename,
            "Rename the currently selected queue. Only used identify\n" +
            "each queue.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_queuegame,
            "Set the game that's assigned to this queue. Changing this\n" +
            "requires the queue to be reset.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_format,
            "Set the current tournament format. Stream and Bracket\n" +
            "Assistants will change to accomodate the format.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_bracketrounds,
            "Set the text file to read bracket rounds from. You can\n" +
            "create your own using notepad and creating a .txt file\n" +
            "with one bracket round name per line.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_bracketrounds,
            "Choose the text file to read bracket rounds from. You can\n" +
            "create your own using notepad and creating a .txt file\n" +
            "with one bracket round name per line.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_seperator,
            "Set the delimiter used to seperate a player's sponsor from\n" +
            "their tag. Can be any number of characters, but it's\n" +
            "suggested to use a single character with a space on\n" +
            "each side (ex: ' | ').");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_ontop,
            "Enable this to prevent any other windows from overlapping\n" +
            "Master Orders windows.");
        }
        public static void LoadStreamGeneralToolTips(GeneralSettingsForm settingsForm)
        {
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtStreamFilesDirectory,
            "Set the directory for the Stream Assitant to export files to.\n" +
            "A folder structure will be created at the location to organize\n" +
            "files exports for the match, players, and commentators.\n" +
            "An XML file containing all information will also be generated,\n" +
            "which can be used when creating dynamic overlays.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnBrowseStreamFilesDirectory,
            "Choose the directory for the Stream Assitant to export files to.\n" +
            "A folder structure will be created at the location to organize\n" +
            "files exports for the match, players, and commentators.\n" +
            "An XML file containing all information will also be generated,\n" +
            "which can be used when creating dynamic overlays.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_automatic,
            "Use automatic stream file updating. Changes to\n" +
            "player and tournament information will automatically\n" +
            "be pushed to the Stream Files Directory.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_manual,
            "Use manual stream file updating. Changes to player\n" +
            "and tournament information will only be pushed to\n" +
            "the Stream Files Directory when clicking Update.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_thumbnails,
            "Enabling will provide functionality to generate thumbnails in\n" +
            "the Stream Assistant. Thumbnail settings will need to be\n" +
            "configured in the Thumbnails tab.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_clipboard,
            "Enabling will provide functionality to generate and copy a\n" +
            "video title to the clipboard in the Stream Assistant. Can be\n" +
            "used with or without thumbnail generation or automatic uploads.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_shorten_video,
            "Select when video titles will be shortened to conserve space.\n" +
            "Be aware that YouTube has a 100-character limit on titles, but\n" +
            "titles over 70 characters will be truncated in most search results.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_titletemplate,
            "The template used to generate video titles. Use the following\n" +
            "tokens to use data from the given match:\n" +
            "*tournament* : The tournament name\n" +
            "*round* : The round in bracket\n" +
            "*date* : The date set for the tournament\n" +
            "*player1* / *player2* : The player or team of players\n" +
            "*character1* / *character2* : The characters used by each player/team");
        }
        public static void LoadStreamYouTubeToolTips(GeneralSettingsForm settingsForm)
        {
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckbEnableVodUploads,
            "Enabling will allow VoDs to be uploaded directly from the Stream\n" +
            "Assistant. A title will be automatically generated based on the Video\n" +
            "Title Template, and description will be generated based on the Default\n" +
            "Video Description, tags will be added based on Video Tags, and, if enabled,\n" +
            "a thumbnail will be generated based on the settings in the Thumbnails tab.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_playlist,
            "Enter the name of the YouTube Playlist to add new VoD uploads to. Remember\n" +
            "to click the Update button after to process the change. If needed, the\n" +
            "playlist will be created. Leaving this field empty will cause VoD uploads\n" +
            "to not be assigned to any playlist.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_playlist,
            "Click to commit to any playlist changes.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtVodsDirectory,
            "Set the directory for the Stream Assitant to look to for any VoD changes.\n" +
            "Configuring this setting correctly, along with selecting the appropriate\n" +
            "stream software, will cause the Stream Assistant to automatically pull\n" +
            "in the most recent local recording when uploading a VoD to Youtube.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnBrowseVodsDirectory,
            "Choose the directory for the Stream Assitant to look to for any VoD changes.\n" +
            "Configuring this setting correctly, along with selecting the appropriate\n" +
            "stream software, will cause the Stream Assistant to automatically pull\n" +
            "in the most recent local recording when uploading a VoD to Youtube.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_xsplit,
            "Select the stream software you are using to record VoDs.\n" +
            "Configuring this setting correctly, along with selecting the appropriate\n" +
            "stream software, will cause the Stream Assistant to automatically pull\n" +
            "in the most recent local recording when uploading a VoD to Youtube.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.rdb_obs,
            "Select the stream software you are using to record VoDs.\n" +
            "Configuring this setting correctly, along with selecting the appropriate\n" +
            "stream software, will cause the Stream Assistant to automatically pull\n" +
            "in the most recent local recording when uploading a VoD to Youtube.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_description,
            "The template used to generate video descriptions. Use the following\n" +
            "tokens to use data from the given match:\n" +
            "*tournament* : The tournament name\n" +
            "*round* : The round in bracket\n" +
            "*bracket* : The bracket URL for the tournament page\n" +
            "*date* : The date set for the tournament\n" +
            "*player1* / *player2* : The player or team of players\n" +
            "*character1* / *character2* : The characters used by each player/team\n" +
            "*twitter1* / *twitter2* : The twitter handles of each player/team"); 
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_tags,
            "Set the tags that will be assigned to VoD uplaods on YouTube.\n" +
            "One tag per line.");
        }
        public static void LoadStreamImagesToolTips(GeneralSettingsForm settingsForm)
        {
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_scoreboad,
            "Toggle the use of image scoreboard. Enabling it\n" +
            "limits the max score to 3 but updates images in the\n" +
            "Stream File Directory to reflect each player's score.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score1_image1,
            "Change the image for Player 1's score at 1 point.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score1_image2,
            "Change the image for Player 1's score at 2 points.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score1_image3,
            "Change the image for Player 1's score at 3 points.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score2_image1,
            "Change the image for Player 2's score at 1 point.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score2_image2,
            "Change the image for Player 2's score at 2 points.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_score2_image3,
            "Change the image for Player 2's score at 3 points.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckbEnableSponsorImages,
            "Toggle whether or not sponsor images should be added for players\n" +
            "in the Stream Output directory.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtSponsorImagesDirectory,
            "Set the directory for the Stream Assitant to look to for player\n" +
            "sponsor images. Typically, these are sponsor/team/school logos.\n" +
            "This setting is also used for generating Top 8 graphics, and\n" +
            "it will be used for that even if Enable Sponsor Images is\n" +
            "unchecked.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnBrowseSponsorImagesDirectory,
            "Choose the directory for the Stream Assitant to look to for player\n" +
            "sponsor images. Typically, these are sponsor/team/school logos.\n" +
            "This setting is also used for generating Top 8 graphics, and\n" +
            "it will be used for that even if Enable Sponsor Images is\n" +
            "unchecked.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckbEnableRegionImages,
            "Toggle whether or not region images should be added for players\n" +
            "in the Stream Output directory.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtRegionImagesDirectory,
            "Set the directory for the Stream Assitant to look to for region\n" +
            "images. These are typically country or state flags, but they can\n" +
            "be anything needed.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnBrowseRegionImagesDirectory,
            "Choose the directory for the Stream Assitant to look to for region\n" +
            "images. These are typically country or state flags, but they can\n" +
            "be anything needed.");
        }
        public static void LoadStreamThumbnailToolTips(GeneralSettingsForm settingsForm)
        {
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_thumbnails,
            "Toggle to enable thumbnail generation. By default, generated\n" +
            "thumbnails are added to the Thumbnails sub-directory in the\n" +
            "Stream Files directory. If Automatic YouTube VoD Uploads is\n" +
            "enabled, thumbnails will be generated and added to the uploads\n" +
            "automatically.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_preview,
            "Generate a sample thumbnail based on the current settings.");

            //Templates and Font
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtThumbnailBackground,
            "Set the image file(.JPG or .PNG) to use for the thumbnail\n" +
            "background. 1920x1080 is the recommended size. All other\n" +
            "images and text appear over this image.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_background,
            "Choose the image file(.JPG or .PNG) to use for the thumbnail\n" +
            "background. 1920x1080 is the recommended size. All other\n" +
            "images and text appear over this image.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txtThumbnailForeground,
            "Set the image file(.PNG) to use for the thumbnail foreground.\n" +
            "1920x1080 is the recommended size. The thumbnail background and\n" +
            "character images appear below it, but all other information\n" +
            "appears over this image.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btn_foreground,
            "Choose the image file(.PNG) to use for the thumbnail foreground.\n" +
            "1920x1080 is the recommended size. The thumbnail background and\n" +
            "character images appear below it, but all other information\n" +
            "appears over this image.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.btnSelectThumbnailFont,
            "Select the font to be used for all text on the thumbnail. Only\n" +
            "the font itself will be used. Size and style will not be applied.");

            //Characters
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_char1_xoffset,
            "Set the X offset from the default position for player 1's character.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_char1_yoffset,
            "Set the Y offset from the default position for player 1's character.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_char1,
            "Select a character to use for player 1 when previewing the thumbnail.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_char2_xoffset,
            "Set the X offset from the default position for player 2's character.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_char2_yoffset,
            "Set the Y offset from the default position for player 1's character.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.cbx_char2,
            "Select a character to use for player 2 when previewing the thumbnail.");

            //Names
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name1_xoffset,
            "Set the X offset from the default position for player 1's name.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name1_yoffset,
            "Set the Y offset from the default position for player 1's name.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name1_size,
            "Set the starting font size for player 1's name. This size will decrease\n" +
            "as needed to ensure both names can fit in their space.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name1,
            "Set a sample name to use when previewing the thumbnail.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name2_xoffset,
            "Set the X offset from the default position for player 2's name.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name2_yoffset,
            "Set the Y offset from the default position for player 2's name.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name2_size,
            "Set the starting font size for player 2's name. This size will decrease\n" +
            "as needed to ensure both names can fit in their space.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_name2,
            "Set a sample name to use when previewing the thumbnail.");

            //Round, Date, and Patch
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_round_xoffset,
            "Set the X offset from the default position for the bracket round.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_round_yoffset,
            "Set the Y offset from the default position for the bracket round.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_round_size,
            "Set the font size for the bracket round.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.ckb_date,
            "Toggle to enable adding the date onto the thumbnail.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_date_xoffset,
            "Set the X offset from the default position for the date.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_date_yoffset,
            "Set the Y offset from the default position for the date.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_date_size,
            "Set the font size for the date.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_version,
            "Set the patch version to display on thumbnails.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_patch_xoffset,
            "Set the X offset from the default position for the patch.\n" +
            "Negative moves left, positive moves right.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_patch_yoffset,
            "Set the Y offset from the default position for the patch.\n" +
            "Negative moves up, positive moves down.");
            settingsForm.ttp_tooltip.SetToolTip(settingsForm.txt_patch_size,
            "Set the font size for the patch.");
        }
    }
}
