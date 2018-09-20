namespace Stream_Info_Handler
{
    partial class frm_uploading
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_uploading));
            this.pic_thumbnail = new System.Windows.Forms.PictureBox();
            this.txt_videotitle = new System.Windows.Forms.TextBox();
            this.lbl_videotitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_description = new System.Windows.Forms.TextBox();
            this.btn_upload_cancel = new System.Windows.Forms.Button();
            this.btn_upload_video = new System.Windows.Forms.Button();
            this.txt_videofile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btn_videofile = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pgb_upload = new System.Windows.Forms.ProgressBar();
            this.lbl_progress = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pic_thumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // pic_thumbnail
            // 
            this.pic_thumbnail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pic_thumbnail.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pic_thumbnail.Location = new System.Drawing.Point(11, 94);
            this.pic_thumbnail.Name = "pic_thumbnail";
            this.pic_thumbnail.Size = new System.Drawing.Size(240, 135);
            this.pic_thumbnail.TabIndex = 0;
            this.pic_thumbnail.TabStop = false;
            // 
            // txt_videotitle
            // 
            this.txt_videotitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_videotitle.Location = new System.Drawing.Point(11, 25);
            this.txt_videotitle.Name = "txt_videotitle";
            this.txt_videotitle.Size = new System.Drawing.Size(493, 22);
            this.txt_videotitle.TabIndex = 1;
            // 
            // lbl_videotitle
            // 
            this.lbl_videotitle.AutoSize = true;
            this.lbl_videotitle.Location = new System.Drawing.Point(12, 9);
            this.lbl_videotitle.Name = "lbl_videotitle";
            this.lbl_videotitle.Size = new System.Drawing.Size(57, 13);
            this.lbl_videotitle.TabIndex = 2;
            this.lbl_videotitle.Text = "Video Title";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(257, 91);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Description";
            // 
            // txt_description
            // 
            this.txt_description.Location = new System.Drawing.Point(257, 107);
            this.txt_description.Multiline = true;
            this.txt_description.Name = "txt_description";
            this.txt_description.Size = new System.Drawing.Size(247, 122);
            this.txt_description.TabIndex = 4;
            // 
            // btn_upload_cancel
            // 
            this.btn_upload_cancel.Location = new System.Drawing.Point(406, 235);
            this.btn_upload_cancel.Name = "btn_upload_cancel";
            this.btn_upload_cancel.Size = new System.Drawing.Size(98, 40);
            this.btn_upload_cancel.TabIndex = 5;
            this.btn_upload_cancel.Text = "Cancel";
            this.btn_upload_cancel.UseVisualStyleBackColor = true;
            this.btn_upload_cancel.Click += new System.EventHandler(this.button1_Click);
            // 
            // btn_upload_video
            // 
            this.btn_upload_video.Enabled = false;
            this.btn_upload_video.Location = new System.Drawing.Point(302, 235);
            this.btn_upload_video.Name = "btn_upload_video";
            this.btn_upload_video.Size = new System.Drawing.Size(98, 40);
            this.btn_upload_video.TabIndex = 6;
            this.btn_upload_video.Text = "Upload Video";
            this.btn_upload_video.UseVisualStyleBackColor = true;
            this.btn_upload_video.Click += new System.EventHandler(this.btn_upload_video_Click);
            // 
            // txt_videofile
            // 
            this.txt_videofile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txt_videofile.Location = new System.Drawing.Point(11, 66);
            this.txt_videofile.Name = "txt_videofile";
            this.txt_videofile.Size = new System.Drawing.Size(409, 22);
            this.txt_videofile.TabIndex = 7;
            this.txt_videofile.TextChanged += new System.EventHandler(this.txt_videofile_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Video File";
            // 
            // btn_videofile
            // 
            this.btn_videofile.Location = new System.Drawing.Point(426, 66);
            this.btn_videofile.Name = "btn_videofile";
            this.btn_videofile.Size = new System.Drawing.Size(79, 22);
            this.btn_videofile.TabIndex = 9;
            this.btn_videofile.Text = "Browse";
            this.btn_videofile.UseVisualStyleBackColor = true;
            this.btn_videofile.Click += new System.EventHandler(this.btn_videofile_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "Accepted Youtube Formats (*.mov;*.mpeg4;*.mp4;*.avi;*.wmv;*.mpegps;*.flv)|*.mov;*" +
    ".mpeg4;*.mp4;*.avi;*.wmv;*.mpegps;*.flv";
            // 
            // pgb_upload
            // 
            this.pgb_upload.Location = new System.Drawing.Point(11, 251);
            this.pgb_upload.Name = "pgb_upload";
            this.pgb_upload.Size = new System.Drawing.Size(285, 20);
            this.pgb_upload.TabIndex = 10;
            // 
            // lbl_progress
            // 
            this.lbl_progress.Location = new System.Drawing.Point(12, 232);
            this.lbl_progress.Name = "lbl_progress";
            this.lbl_progress.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lbl_progress.Size = new System.Drawing.Size(284, 16);
            this.lbl_progress.TabIndex = 11;
            // 
            // frm_uploading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(517, 282);
            this.Controls.Add(this.lbl_progress);
            this.Controls.Add(this.pgb_upload);
            this.Controls.Add(this.btn_videofile);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_videofile);
            this.Controls.Add(this.btn_upload_video);
            this.Controls.Add(this.btn_upload_cancel);
            this.Controls.Add(this.txt_description);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_videotitle);
            this.Controls.Add(this.txt_videotitle);
            this.Controls.Add(this.pic_thumbnail);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_uploading";
            this.Text = "Upload to YouTube";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form4_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.pic_thumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic_thumbnail;
        private System.Windows.Forms.TextBox txt_videotitle;
        private System.Windows.Forms.Label lbl_videotitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_description;
        private System.Windows.Forms.Button btn_upload_cancel;
        private System.Windows.Forms.Button btn_upload_video;
        private System.Windows.Forms.TextBox txt_videofile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_videofile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ProgressBar pgb_upload;
        private System.Windows.Forms.Label lbl_progress;
    }
}