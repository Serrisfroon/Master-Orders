
namespace Stream_Info_Handler.ClipToTwitter
{
    partial class ClipToTwitterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClipToTwitterForm));
            this.btnConnect = new System.Windows.Forms.Button();
            this.btnGenerateClip = new System.Windows.Forms.Button();
            this.llbManageConnections = new System.Windows.Forms.LinkLabel();
            this.txtTweet = new System.Windows.Forms.TextBox();
            this.btnSendTweet = new System.Windows.Forms.Button();
            this.lblConsole = new System.Windows.Forms.Label();
            this.btnConfirmPublication = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(12, 12);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(145, 26);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // btnGenerateClip
            // 
            this.btnGenerateClip.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateClip.Location = new System.Drawing.Point(12, 57);
            this.btnGenerateClip.Name = "btnGenerateClip";
            this.btnGenerateClip.Size = new System.Drawing.Size(145, 55);
            this.btnGenerateClip.TabIndex = 2;
            this.btnGenerateClip.Text = "Generate Clip!";
            this.btnGenerateClip.UseVisualStyleBackColor = true;
            this.btnGenerateClip.Click += new System.EventHandler(this.btnGenerateClip_Click);
            // 
            // llbManageConnections
            // 
            this.llbManageConnections.AutoSize = true;
            this.llbManageConnections.Location = new System.Drawing.Point(40, 41);
            this.llbManageConnections.Name = "llbManageConnections";
            this.llbManageConnections.Size = new System.Drawing.Size(117, 13);
            this.llbManageConnections.TabIndex = 1;
            this.llbManageConnections.TabStop = true;
            this.llbManageConnections.Text = "Manage Connections...";
            this.llbManageConnections.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbManageConnections_LinkClicked);
            // 
            // txtTweet
            // 
            this.txtTweet.Location = new System.Drawing.Point(163, 12);
            this.txtTweet.Multiline = true;
            this.txtTweet.Name = "txtTweet";
            this.txtTweet.Size = new System.Drawing.Size(251, 100);
            this.txtTweet.TabIndex = 3;
            // 
            // btnSendTweet
            // 
            this.btnSendTweet.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.btnSendTweet.Location = new System.Drawing.Point(309, 118);
            this.btnSendTweet.Name = "btnSendTweet";
            this.btnSendTweet.Size = new System.Drawing.Size(105, 31);
            this.btnSendTweet.TabIndex = 4;
            this.btnSendTweet.Text = "Send Tweet";
            this.btnSendTweet.UseVisualStyleBackColor = true;
            this.btnSendTweet.Click += new System.EventHandler(this.btnSendTweet_Click);
            // 
            // lblConsole
            // 
            this.lblConsole.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblConsole.Location = new System.Drawing.Point(163, 118);
            this.lblConsole.Name = "lblConsole";
            this.lblConsole.Size = new System.Drawing.Size(140, 31);
            this.lblConsole.TabIndex = 5;
            this.lblConsole.Text = "label1";
            // 
            // btnConfirmPublication
            // 
            this.btnConfirmPublication.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirmPublication.Location = new System.Drawing.Point(12, 118);
            this.btnConfirmPublication.Name = "btnConfirmPublication";
            this.btnConfirmPublication.Size = new System.Drawing.Size(145, 31);
            this.btnConfirmPublication.TabIndex = 6;
            this.btnConfirmPublication.Text = "Confirm Publication";
            this.btnConfirmPublication.UseVisualStyleBackColor = true;
            this.btnConfirmPublication.Click += new System.EventHandler(this.btnConfirmPublication_Click);
            // 
            // ClipToTwitterForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(424, 164);
            this.Controls.Add(this.btnConfirmPublication);
            this.Controls.Add(this.lblConsole);
            this.Controls.Add(this.btnSendTweet);
            this.Controls.Add(this.txtTweet);
            this.Controls.Add(this.llbManageConnections);
            this.Controls.Add(this.btnGenerateClip);
            this.Controls.Add(this.btnConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "ClipToTwitterForm";
            this.Text = "Master Orders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnGenerateClip;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.LinkLabel llbManageConnections;
        private System.Windows.Forms.TextBox txtTweet;
        private System.Windows.Forms.Button btnSendTweet;
        private System.Windows.Forms.Label lblConsole;
        private System.Windows.Forms.Button btnConfirmPublication;
    }
}