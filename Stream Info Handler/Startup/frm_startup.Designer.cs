namespace Stream_Info_Handler
{
    partial class frm_startup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_startup));
            this.btn_stream = new System.Windows.Forms.Button();
            this.btn_bracket = new System.Windows.Forms.Button();
            this.ckb_keep_open = new System.Windows.Forms.CheckBox();
            this.btn_settings = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.sbn_othertools = new SplitButtonDLL.SplitButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_stream
            // 
            this.btn_stream.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_stream.Location = new System.Drawing.Point(71, 137);
            this.btn_stream.Name = "btn_stream";
            this.btn_stream.Size = new System.Drawing.Size(188, 58);
            this.btn_stream.TabIndex = 1;
            this.btn_stream.Text = "Stream Assistant";
            this.btn_stream.UseVisualStyleBackColor = true;
            this.btn_stream.Click += new System.EventHandler(this.btn_stream_Click);
            // 
            // btn_bracket
            // 
            this.btn_bracket.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_bracket.Location = new System.Drawing.Point(71, 215);
            this.btn_bracket.Name = "btn_bracket";
            this.btn_bracket.Size = new System.Drawing.Size(188, 58);
            this.btn_bracket.TabIndex = 2;
            this.btn_bracket.Text = "Bracket Assistant";
            this.btn_bracket.UseVisualStyleBackColor = true;
            this.btn_bracket.Click += new System.EventHandler(this.btn_bracket_Click);
            // 
            // ckb_keep_open
            // 
            this.ckb_keep_open.AutoSize = true;
            this.ckb_keep_open.Location = new System.Drawing.Point(39, 419);
            this.ckb_keep_open.Name = "ckb_keep_open";
            this.ckb_keep_open.Size = new System.Drawing.Size(260, 17);
            this.ckb_keep_open.TabIndex = 3;
            this.ckb_keep_open.Text = "Keep this window open after opening an assistant";
            this.ckb_keep_open.UseVisualStyleBackColor = true;
            this.ckb_keep_open.CheckedChanged += new System.EventHandler(this.ckb_keep_open_CheckedChanged);
            // 
            // btn_settings
            // 
            this.btn_settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_settings.Location = new System.Drawing.Point(121, 378);
            this.btn_settings.Name = "btn_settings";
            this.btn_settings.Size = new System.Drawing.Size(86, 31);
            this.btn_settings.TabIndex = 4;
            this.btn_settings.Text = "Settings";
            this.btn_settings.UseVisualStyleBackColor = true;
            this.btn_settings.Click += new System.EventHandler(this.btn_settings_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 454);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(241, 26);
            this.label1.TabIndex = 6;
            this.label1.Text = "Master Orders Version 1.1.0\r\nCopyright 2019, Dan Sanchez, All rights reserved.\r\n";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::Stream_Info_Handler.Properties.Resources.MasterOrders;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(-8, -4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(344, 104);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // sbn_othertools
            // 
            this.sbn_othertools.ColeItems = new string[] {
        "Player Manager",
        "Top 8 Generator",
        "Clip-Tweeter"};
            this.sbn_othertools.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sbn_othertools.Image = ((System.Drawing.Image)(resources.GetObject("sbn_othertools.Image")));
            this.sbn_othertools.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.sbn_othertools.Location = new System.Drawing.Point(71, 293);
            this.sbn_othertools.Name = "sbn_othertools";
            this.sbn_othertools.Size = new System.Drawing.Size(188, 58);
            this.sbn_othertools.TabIndex = 8;
            this.sbn_othertools.Text = "Other Tools";
            this.sbn_othertools.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.sbn_othertools.UseVisualStyleBackColor = true;
            // 
            // frm_startup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(331, 489);
            this.Controls.Add(this.sbn_othertools);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btn_settings);
            this.Controls.Add(this.ckb_keep_open);
            this.Controls.Add(this.btn_bracket);
            this.Controls.Add(this.btn_stream);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_startup";
            this.Text = "Master Orders";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btn_stream;
        private System.Windows.Forms.Button btn_bracket;
        private System.Windows.Forms.CheckBox ckb_keep_open;
        private System.Windows.Forms.Button btn_settings;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private SplitButtonDLL.SplitButton sbn_othertools;
    }
}