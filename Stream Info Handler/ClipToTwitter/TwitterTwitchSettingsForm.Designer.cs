
namespace Stream_Info_Handler.ClipToTwitter
{
    partial class TwitterTwitchSettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TwitterTwitchSettingsForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblConnectedTwitter = new System.Windows.Forms.Label();
            this.btnRevokeTwitter = new System.Windows.Forms.Button();
            this.btnConnectToTwitter = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblConnectedTwitch = new System.Windows.Forms.Label();
            this.txtChannelToClip = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRevokeTwitch = new System.Windows.Forms.Button();
            this.btnConnectToTwitch = new System.Windows.Forms.Button();
            this.btnDone = new System.Windows.Forms.Button();
            this.ckbRemind = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblConnectedTwitter);
            this.groupBox1.Controls.Add(this.btnRevokeTwitter);
            this.groupBox1.Controls.Add(this.btnConnectToTwitter);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(294, 78);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Twitter Connection";
            // 
            // lblConnectedTwitter
            // 
            this.lblConnectedTwitter.AutoSize = true;
            this.lblConnectedTwitter.Location = new System.Drawing.Point(6, 50);
            this.lblConnectedTwitter.Name = "lblConnectedTwitter";
            this.lblConnectedTwitter.Size = new System.Drawing.Size(108, 13);
            this.lblConnectedTwitter.TabIndex = 7;
            this.lblConnectedTwitter.Text = "Connected Account: ";
            // 
            // btnRevokeTwitter
            // 
            this.btnRevokeTwitter.Location = new System.Drawing.Point(161, 19);
            this.btnRevokeTwitter.Name = "btnRevokeTwitter";
            this.btnRevokeTwitter.Size = new System.Drawing.Size(126, 28);
            this.btnRevokeTwitter.TabIndex = 1;
            this.btnRevokeTwitter.Text = "Revoke Twitter Access";
            this.btnRevokeTwitter.UseVisualStyleBackColor = true;
            this.btnRevokeTwitter.Click += new System.EventHandler(this.btnRevokeTwitter_Click);
            // 
            // btnConnectToTwitter
            // 
            this.btnConnectToTwitter.Location = new System.Drawing.Point(6, 19);
            this.btnConnectToTwitter.Name = "btnConnectToTwitter";
            this.btnConnectToTwitter.Size = new System.Drawing.Size(126, 28);
            this.btnConnectToTwitter.TabIndex = 0;
            this.btnConnectToTwitter.Text = "Connect to Twitter";
            this.btnConnectToTwitter.UseVisualStyleBackColor = true;
            this.btnConnectToTwitter.Click += new System.EventHandler(this.btnConnectToTwitter_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblConnectedTwitch);
            this.groupBox2.Controls.Add(this.txtChannelToClip);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.btnRevokeTwitch);
            this.groupBox2.Controls.Add(this.btnConnectToTwitch);
            this.groupBox2.Location = new System.Drawing.Point(12, 96);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(294, 120);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Twitch Connection";
            // 
            // lblConnectedTwitch
            // 
            this.lblConnectedTwitch.AutoSize = true;
            this.lblConnectedTwitch.Location = new System.Drawing.Point(6, 50);
            this.lblConnectedTwitch.Name = "lblConnectedTwitch";
            this.lblConnectedTwitch.Size = new System.Drawing.Size(108, 13);
            this.lblConnectedTwitch.TabIndex = 4;
            this.lblConnectedTwitch.Text = "Connected Account: ";
            // 
            // txtChannelToClip
            // 
            this.txtChannelToClip.Location = new System.Drawing.Point(6, 91);
            this.txtChannelToClip.Name = "txtChannelToClip";
            this.txtChannelToClip.Size = new System.Drawing.Size(281, 20);
            this.txtChannelToClip.TabIndex = 3;
            this.txtChannelToClip.TextChanged += new System.EventHandler(this.txtChannelToClip_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Channel to Clip From:";
            // 
            // btnRevokeTwitch
            // 
            this.btnRevokeTwitch.Location = new System.Drawing.Point(161, 19);
            this.btnRevokeTwitch.Name = "btnRevokeTwitch";
            this.btnRevokeTwitch.Size = new System.Drawing.Size(126, 28);
            this.btnRevokeTwitch.TabIndex = 1;
            this.btnRevokeTwitch.Text = "Revoke Twitch Access";
            this.btnRevokeTwitch.UseVisualStyleBackColor = true;
            this.btnRevokeTwitch.Click += new System.EventHandler(this.btnRevokeTwitch_Click);
            // 
            // btnConnectToTwitch
            // 
            this.btnConnectToTwitch.Location = new System.Drawing.Point(6, 19);
            this.btnConnectToTwitch.Name = "btnConnectToTwitch";
            this.btnConnectToTwitch.Size = new System.Drawing.Size(126, 28);
            this.btnConnectToTwitch.TabIndex = 0;
            this.btnConnectToTwitch.Text = "Connect to Twitch";
            this.btnConnectToTwitch.UseVisualStyleBackColor = true;
            this.btnConnectToTwitch.Click += new System.EventHandler(this.btnConnectToTwitch_Click);
            // 
            // btnDone
            // 
            this.btnDone.Location = new System.Drawing.Point(95, 245);
            this.btnDone.Name = "btnDone";
            this.btnDone.Size = new System.Drawing.Size(126, 28);
            this.btnDone.TabIndex = 5;
            this.btnDone.Text = "Done";
            this.btnDone.UseVisualStyleBackColor = true;
            this.btnDone.Click += new System.EventHandler(this.btnDone_Click);
            // 
            // ckbRemind
            // 
            this.ckbRemind.AutoSize = true;
            this.ckbRemind.Location = new System.Drawing.Point(51, 222);
            this.ckbRemind.Name = "ckbRemind";
            this.ckbRemind.Size = new System.Drawing.Size(217, 17);
            this.ckbRemind.TabIndex = 6;
            this.ckbRemind.Text = "Remind to publish the first clip generated";
            this.ckbRemind.UseVisualStyleBackColor = true;
            this.ckbRemind.CheckedChanged += new System.EventHandler(this.ckbRemind_CheckedChanged);
            // 
            // TwitterTwitchSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(317, 286);
            this.Controls.Add(this.ckbRemind);
            this.Controls.Add(this.btnDone);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TwitterTwitchSettingsForm";
            this.Text = "Master Orders";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TwitterTwitchSettingsForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnRevokeTwitter;
        private System.Windows.Forms.Button btnConnectToTwitter;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtChannelToClip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRevokeTwitch;
        private System.Windows.Forms.Button btnConnectToTwitch;
        private System.Windows.Forms.Label lblConnectedTwitter;
        private System.Windows.Forms.Label lblConnectedTwitch;
        private System.Windows.Forms.Button btnDone;
        private System.Windows.Forms.CheckBox ckbRemind;
    }
}