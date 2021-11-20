using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler.ClipToTwitter
{
    public partial class ClipToTwitterForm : Form
    {
        public ClipToTwitterForm()
        {
            InitializeComponent();
            EnableButtons(ClipToTwitterConnection.isInitialized);
            ClipToTwitterConnection.Initialize();
        }

        private void EnableButtons(bool isInitialized)
        {
            if(!isInitialized)
            {
                btnGenerateClip.Enabled = false;
                lblConsole.Text = "";
            }
            else
            {
                btnGenerateClip.Enabled = true;
                lblConsole.Text = "Connected.";
            }
            txtTweet.Enabled = false;
            btnSendTweet.Enabled = false;
            btnConfirmPublication.Enabled = false;
        }

        private void llbManageConnections_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using(TwitterTwitchSettingsForm settingsForm = new TwitterTwitchSettingsForm())
            {
                settingsForm.ShowDialog();
            }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            lblConsole.Text = "Connecting to Twitch and Twitter...";
            ClipToTwitterConnection.ConnectToTwitter();
            ClipToTwitterConnection.ConnectToTwitch();
            btnGenerateClip.Enabled = true;
            ClipToTwitterConnection.isInitialized = true;
            lblConsole.Text = "Connected.";
        }

        private void btnGenerateClip_Click(object sender, EventArgs e)
        {
            btnGenerateClip.Enabled = false;
            try
            { 
                ClipToTwitterConnection.GenerateClip();
            }
            catch
            {
                MessageBox.Show($"The channel name provided is either does not exist or is not live. Click 'Manage Connections' and update the channel name: { ClipToTwitterConnection.channelToClip }");
                btnGenerateClip.Enabled = true;
                return;
            }
            btnGenerateClip.Enabled = true;
            btnConfirmPublication.Enabled = true;
            txtTweet.Enabled = true;
            btnSendTweet.Enabled = false;
            txtTweet.Text = "";

            if (!ClipToTwitterConnection.wasUserNotifiedToPublish)
            {
                ClipToTwitterConnection.wasUserNotifiedToPublish = true;
                MessageBox.Show("Publish the clip from the native site and ensure it is less than 25 seconds in length. After you're finished, click 'Confirm Publication' to proceed.");
            }
            lblConsole.Text = "Click 'Confirm Publication' after publishing the clip.";
        }

        private void btnConfirmPublication_Click(object sender, EventArgs e)
        {
            btnSendTweet.Enabled = true;
            btnConfirmPublication.Enabled = false;
            ClipToTwitterConnection.DownloadClip();
            lblConsole.Text = "Type your message and click 'Send Tweet'!";
       }

        private void btnSendTweet_Click(object sender, EventArgs e)
        {
            lblConsole.Text = "Sending Tweet. This may take a minute or so.";
            btnSendTweet.Enabled = false;
            txtTweet.Enabled = false;
            Thread videoThread = new Thread(() =>
            {
                ClipToTwitterConnection.SendTweet(txtTweet.Text);
                lblConsole.Invoke((MethodInvoker)delegate {
                    lblConsole.Text = "Tweet sent successfully!";
                });
                btnGenerateClip.Invoke((MethodInvoker)delegate {
                    btnGenerateClip.Enabled = true;
                });
            });
            videoThread.IsBackground = true;
            videoThread.Start();
        }


    }
}
