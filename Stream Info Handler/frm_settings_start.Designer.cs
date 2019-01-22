namespace Stream_Info_Handler
{
    partial class frm_settings_start
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_settings_start));
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_thumb_directory = new System.Windows.Forms.Button();
            this.txt_thumbnail_directory = new System.Windows.Forms.TextBox();
            this.btn_browse_roster = new System.Windows.Forms.Button();
            this.txt_roster_directory = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_stream_directory = new System.Windows.Forms.TextBox();
            this.btn_output = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdb_manual = new System.Windows.Forms.RadioButton();
            this.rdb_automatic = new System.Windows.Forms.RadioButton();
            this.btn_finish = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.btn_vods = new System.Windows.Forms.Button();
            this.txt_vods = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(29, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please configure Master Orders";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Cursor = System.Windows.Forms.Cursors.Default;
            this.label6.Location = new System.Drawing.Point(12, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(101, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "Thumbnail Directory";
            // 
            // btn_thumb_directory
            // 
            this.btn_thumb_directory.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_thumb_directory.Location = new System.Drawing.Point(258, 57);
            this.btn_thumb_directory.Name = "btn_thumb_directory";
            this.btn_thumb_directory.Size = new System.Drawing.Size(65, 22);
            this.btn_thumb_directory.TabIndex = 0;
            this.btn_thumb_directory.Text = "Browse";
            this.btn_thumb_directory.UseVisualStyleBackColor = true;
            this.btn_thumb_directory.Click += new System.EventHandler(this.btn_thumb_directory_Click);
            // 
            // txt_thumbnail_directory
            // 
            this.txt_thumbnail_directory.Cursor = System.Windows.Forms.Cursors.Default;
            this.txt_thumbnail_directory.Location = new System.Drawing.Point(12, 57);
            this.txt_thumbnail_directory.Name = "txt_thumbnail_directory";
            this.txt_thumbnail_directory.Size = new System.Drawing.Size(240, 20);
            this.txt_thumbnail_directory.TabIndex = 1;
            this.txt_thumbnail_directory.TabStop = false;
            this.txt_thumbnail_directory.TextChanged += new System.EventHandler(this.txt_thumbnail_directory_TextChanged);
            // 
            // btn_browse_roster
            // 
            this.btn_browse_roster.Location = new System.Drawing.Point(258, 135);
            this.btn_browse_roster.Name = "btn_browse_roster";
            this.btn_browse_roster.Size = new System.Drawing.Size(65, 22);
            this.btn_browse_roster.TabIndex = 4;
            this.btn_browse_roster.Text = "Browse";
            this.btn_browse_roster.UseVisualStyleBackColor = true;
            this.btn_browse_roster.Click += new System.EventHandler(this.btn_browse_roster_Click);
            // 
            // txt_roster_directory
            // 
            this.txt_roster_directory.Location = new System.Drawing.Point(12, 135);
            this.txt_roster_directory.Name = "txt_roster_directory";
            this.txt_roster_directory.Size = new System.Drawing.Size(240, 20);
            this.txt_roster_directory.TabIndex = 5;
            this.txt_roster_directory.TabStop = false;
            this.txt_roster_directory.TextChanged += new System.EventHandler(this.txt_roster_directory_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(132, 13);
            this.label5.TabIndex = 23;
            this.label5.Text = "Character Roster Directory";
            // 
            // txt_stream_directory
            // 
            this.txt_stream_directory.Cursor = System.Windows.Forms.Cursors.Default;
            this.txt_stream_directory.Location = new System.Drawing.Point(12, 175);
            this.txt_stream_directory.Name = "txt_stream_directory";
            this.txt_stream_directory.Size = new System.Drawing.Size(240, 20);
            this.txt_stream_directory.TabIndex = 7;
            this.txt_stream_directory.TabStop = false;
            this.txt_stream_directory.TextChanged += new System.EventHandler(this.txt_stream_directory_TextChanged);
            // 
            // btn_output
            // 
            this.btn_output.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_output.Location = new System.Drawing.Point(258, 173);
            this.btn_output.Name = "btn_output";
            this.btn_output.Size = new System.Drawing.Size(65, 22);
            this.btn_output.TabIndex = 6;
            this.btn_output.Text = "Browse";
            this.btn_output.UseVisualStyleBackColor = true;
            this.btn_output.Click += new System.EventHandler(this.btn_output_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Cursor = System.Windows.Forms.Cursors.Default;
            this.label7.Location = new System.Drawing.Point(12, 159);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(109, 13);
            this.label7.TabIndex = 26;
            this.label7.Text = "Stream Files Directory";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdb_manual);
            this.groupBox2.Controls.Add(this.rdb_automatic);
            this.groupBox2.Location = new System.Drawing.Point(12, 201);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(146, 71);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Scoreboard Updating";
            // 
            // rdb_manual
            // 
            this.rdb_manual.AutoSize = true;
            this.rdb_manual.Location = new System.Drawing.Point(21, 42);
            this.rdb_manual.Name = "rdb_manual";
            this.rdb_manual.Size = new System.Drawing.Size(103, 17);
            this.rdb_manual.TabIndex = 1;
            this.rdb_manual.Text = "Manual Updates";
            this.rdb_manual.UseVisualStyleBackColor = true;
            this.rdb_manual.CheckedChanged += new System.EventHandler(this.rdb_manual_CheckedChanged);
            // 
            // rdb_automatic
            // 
            this.rdb_automatic.AutoSize = true;
            this.rdb_automatic.Checked = true;
            this.rdb_automatic.Location = new System.Drawing.Point(21, 19);
            this.rdb_automatic.Name = "rdb_automatic";
            this.rdb_automatic.Size = new System.Drawing.Size(115, 17);
            this.rdb_automatic.TabIndex = 0;
            this.rdb_automatic.TabStop = true;
            this.rdb_automatic.Text = "Automatic Updates";
            this.rdb_automatic.UseVisualStyleBackColor = true;
            this.rdb_automatic.CheckedChanged += new System.EventHandler(this.rdb_automatic_CheckedChanged);
            // 
            // btn_finish
            // 
            this.btn_finish.Location = new System.Drawing.Point(109, 278);
            this.btn_finish.Name = "btn_finish";
            this.btn_finish.Size = new System.Drawing.Size(114, 39);
            this.btn_finish.TabIndex = 10;
            this.btn_finish.Text = "Finish";
            this.btn_finish.UseVisualStyleBackColor = true;
            this.btn_finish.Click += new System.EventHandler(this.btn_finish_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Filter = ".json Files|*.json";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Cursor = System.Windows.Forms.Cursors.Default;
            this.label9.Location = new System.Drawing.Point(12, 80);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(73, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "VoD Directory";
            // 
            // btn_vods
            // 
            this.btn_vods.Cursor = System.Windows.Forms.Cursors.Default;
            this.btn_vods.Location = new System.Drawing.Point(258, 96);
            this.btn_vods.Name = "btn_vods";
            this.btn_vods.Size = new System.Drawing.Size(65, 22);
            this.btn_vods.TabIndex = 2;
            this.btn_vods.Text = "Browse";
            this.btn_vods.UseVisualStyleBackColor = true;
            this.btn_vods.Click += new System.EventHandler(this.btn_vods_Click);
            // 
            // txt_vods
            // 
            this.txt_vods.Cursor = System.Windows.Forms.Cursors.Default;
            this.txt_vods.Location = new System.Drawing.Point(12, 96);
            this.txt_vods.Name = "txt_vods";
            this.txt_vods.Size = new System.Drawing.Size(240, 20);
            this.txt_vods.TabIndex = 3;
            this.txt_vods.TabStop = false;
            this.txt_vods.TextChanged += new System.EventHandler(this.txt_vods_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Location = new System.Drawing.Point(177, 201);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(146, 71);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Stream Software";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(21, 42);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(80, 17);
            this.radioButton1.TabIndex = 1;
            this.radioButton1.Text = "OBS Studio";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.CheckedChanged += new System.EventHandler(this.radioButton1_CheckedChanged);
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(21, 19);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(52, 17);
            this.radioButton2.TabIndex = 0;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "XSplit";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // frm_settings_start
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 324);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btn_vods);
            this.Controls.Add(this.txt_vods);
            this.Controls.Add(this.btn_finish);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.txt_stream_directory);
            this.Controls.Add(this.btn_output);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btn_browse_roster);
            this.Controls.Add(this.txt_roster_directory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_thumb_directory);
            this.Controls.Add(this.txt_thumbnail_directory);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_settings_start";
            this.Text = "Master Orders";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btn_thumb_directory;
        private System.Windows.Forms.TextBox txt_thumbnail_directory;
        private System.Windows.Forms.Button btn_browse_roster;
        private System.Windows.Forms.TextBox txt_roster_directory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_stream_directory;
        private System.Windows.Forms.Button btn_output;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdb_manual;
        private System.Windows.Forms.RadioButton rdb_automatic;
        private System.Windows.Forms.Button btn_finish;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btn_vods;
        private System.Windows.Forms.TextBox txt_vods;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
    }
}