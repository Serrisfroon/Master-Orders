using Stream_Info_Handler.AppSettings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler.ClipToTwitter
{
    public partial class TwitterTwitchSettingsForm : Form
    {
        public TwitterTwitchSettingsForm()
        {
            InitializeComponent();
            btnRevokeTwitch.Enabled = false;
            btnRevokeTwitter.Enabled = false;
            txtChannelToClip.Text = ClipToTwitterConnection.channelToClip;

            if (ClipToTwitterConnection.twitchEnabled)
            {
                lblConnectedTwitch.Text = "Connected Account: " + ClipToTwitterConnection.GetTwitchUser();
                btnRevokeTwitch.Enabled = true;
            }
            if (ClipToTwitterConnection.twitterEnabled)
            {
                lblConnectedTwitter.Text = "Connected Account: " + ClipToTwitterConnection.GetTwitterUser();
                btnRevokeTwitter.Enabled = true;
            }
            ckbRemind.Checked = (bool)XDocument.Load(SettingsFile.settingsFile).Root.Element("twitch").Element("remind-to-publish");

        }



        private void btnConnectToTwitter_Click(object sender, EventArgs e)
        {
            ClipToTwitterConnection.ConnectToTwitter();
            lblConnectedTwitter.Text = "Connected Account: " + ClipToTwitterConnection.GetTwitterUser();
            btnRevokeTwitter.Enabled = true;
        }

        private void btnRevokeTwitter_Click(object sender, EventArgs e)
        {
            ClipToTwitterConnection.DisconnectTwitter();
            lblConnectedTwitter.Text = "Connected Account: ";
        }

        private void btnConnectToTwitch_Click(object sender, EventArgs e)
        {
            ClipToTwitterConnection.ConnectToTwitch();
            lblConnectedTwitch.Text = "Connected Account: " + ClipToTwitterConnection.GetTwitchUser();
            btnRevokeTwitch.Enabled = true;
        }

        private void btnRevokeTwitch_Click(object sender, EventArgs e)
        {
            ClipToTwitterConnection.DisconnectTwitch();
            lblConnectedTwitch.Text = "Connected Account: ";
        }

        private void txtChannelToClip_TextChanged(object sender, EventArgs e)
        {
            ClipToTwitterConnection.UpdateChannelToClip(((TextBox)sender).Text);
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ckbRemind_CheckedChanged(object sender, EventArgs e)
        {
            ClipToTwitterConnection.UpdateNotifyToPublish(ckbRemind.Checked);
        }

        private void TwitterTwitchSettingsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (ClipToTwitterConnection.wasUserNotifiedToPublish == false && ckbRemind.Checked == false)
            {
                ClipToTwitterConnection.wasUserNotifiedToPublish = true;
            }
        }
    }
}
