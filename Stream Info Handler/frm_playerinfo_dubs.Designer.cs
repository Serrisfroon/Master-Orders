namespace Stream_Info_Handler
{
    partial class frm_playerinfo_dubs
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
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Name",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem(new string[] {
            "Twitter",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem3 = new System.Windows.Forms.ListViewItem(new string[] {
            "Region",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem4 = new System.Windows.Forms.ListViewItem(new string[] {
            "Sponsor",
            "",
            "",
            "",
            ""}, -1);
            System.Windows.Forms.ListViewItem listViewItem5 = new System.Windows.Forms.ListViewItem(new string[] {
            "Character",
            "",
            "",
            "",
            ""}, -1);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_playerinfo_dubs));
            this.btn_player2 = new System.Windows.Forms.Button();
            this.btn_player1 = new System.Windows.Forms.Button();
            this.lvw_playerinfo = new System.Windows.Forms.ListView();
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_player3 = new System.Windows.Forms.Button();
            this.btn_player4 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_player2
            // 
            this.btn_player2.Location = new System.Drawing.Point(323, 169);
            this.btn_player2.Name = "btn_player2";
            this.btn_player2.Size = new System.Drawing.Size(152, 29);
            this.btn_player2.TabIndex = 1;
            this.btn_player2.Text = "Edit Player 2 Information";
            this.btn_player2.UseVisualStyleBackColor = true;
            this.btn_player2.Click += new System.EventHandler(this.btn_player2_Click);
            // 
            // btn_player1
            // 
            this.btn_player1.Location = new System.Drawing.Point(128, 169);
            this.btn_player1.Name = "btn_player1";
            this.btn_player1.Size = new System.Drawing.Size(152, 29);
            this.btn_player1.TabIndex = 0;
            this.btn_player1.Text = "Edit Player 1 Information";
            this.btn_player1.UseVisualStyleBackColor = true;
            this.btn_player1.Click += new System.EventHandler(this.btn_player1_Click);
            // 
            // lvw_playerinfo
            // 
            this.lvw_playerinfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader1,
            this.columnHeader5});
            this.lvw_playerinfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvw_playerinfo.FullRowSelect = true;
            this.lvw_playerinfo.GridLines = true;
            this.lvw_playerinfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvw_playerinfo.HideSelection = false;
            this.lvw_playerinfo.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1,
            listViewItem2,
            listViewItem3,
            listViewItem4,
            listViewItem5});
            this.lvw_playerinfo.Location = new System.Drawing.Point(12, 12);
            this.lvw_playerinfo.Name = "lvw_playerinfo";
            this.lvw_playerinfo.Size = new System.Drawing.Size(871, 151);
            this.lvw_playerinfo.TabIndex = 4;
            this.lvw_playerinfo.UseCompatibleStateImageBehavior = false;
            this.lvw_playerinfo.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Team 1 Player 1";
            this.columnHeader3.Width = 191;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Team1 Player 2";
            this.columnHeader4.Width = 191;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Team 2 Player 1";
            this.columnHeader1.Width = 191;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Team 2 Player 2";
            this.columnHeader5.Width = 191;
            // 
            // btn_player3
            // 
            this.btn_player3.Location = new System.Drawing.Point(513, 169);
            this.btn_player3.Name = "btn_player3";
            this.btn_player3.Size = new System.Drawing.Size(152, 29);
            this.btn_player3.TabIndex = 2;
            this.btn_player3.Text = "Edit Player 1 Information";
            this.btn_player3.UseVisualStyleBackColor = true;
            this.btn_player3.Click += new System.EventHandler(this.btn_player3_Click);
            // 
            // btn_player4
            // 
            this.btn_player4.Location = new System.Drawing.Point(706, 169);
            this.btn_player4.Name = "btn_player4";
            this.btn_player4.Size = new System.Drawing.Size(152, 29);
            this.btn_player4.TabIndex = 3;
            this.btn_player4.Text = "Edit Player 2 Information";
            this.btn_player4.UseVisualStyleBackColor = true;
            this.btn_player4.Click += new System.EventHandler(this.btn_player4_Click);
            // 
            // frm_playerinfo_dubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(896, 206);
            this.ControlBox = false;
            this.Controls.Add(this.btn_player4);
            this.Controls.Add(this.btn_player3);
            this.Controls.Add(this.btn_player2);
            this.Controls.Add(this.btn_player1);
            this.Controls.Add(this.lvw_playerinfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_playerinfo_dubs";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Player Information";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_player2;
        private System.Windows.Forms.Button btn_player1;
        private System.Windows.Forms.ListView lvw_playerinfo;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Button btn_player3;
        private System.Windows.Forms.Button btn_player4;
    }
}