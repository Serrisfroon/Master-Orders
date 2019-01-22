namespace Stream_Info_Handler
{
    partial class frm_streamqueue_dubs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_streamqueue_dubs));
            this.lvw_matches = new System.Windows.Forms.ListView();
            this.clm_match = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_round = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clm_player4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btn_movedown = new System.Windows.Forms.Button();
            this.btn_moveup = new System.Windows.Forms.Button();
            this.btn_push = new System.Windows.Forms.Button();
            this.btn_remove = new System.Windows.Forms.Button();
            this.btn_edit = new System.Windows.Forms.Button();
            this.btn_reset = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbx_player4 = new System.Windows.Forms.ComboBox();
            this.cbx_player3 = new System.Windows.Forms.ComboBox();
            this.btn_add = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbx_player2 = new System.Windows.Forms.ComboBox();
            this.cbx_round = new System.Windows.Forms.ComboBox();
            this.cbx_player1 = new System.Windows.Forms.ComboBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvw_matches
            // 
            this.lvw_matches.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clm_match,
            this.clm_round,
            this.clm_player1,
            this.clm_player2,
            this.clm_player3,
            this.clm_player4});
            this.lvw_matches.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvw_matches.FullRowSelect = true;
            this.lvw_matches.GridLines = true;
            this.lvw_matches.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvw_matches.HideSelection = false;
            this.lvw_matches.Location = new System.Drawing.Point(12, 82);
            this.lvw_matches.Name = "lvw_matches";
            this.lvw_matches.Size = new System.Drawing.Size(1058, 292);
            this.lvw_matches.TabIndex = 1;
            this.lvw_matches.UseCompatibleStateImageBehavior = false;
            this.lvw_matches.View = System.Windows.Forms.View.Details;
            this.lvw_matches.SelectedIndexChanged += new System.EventHandler(this.lvw_matches_SelectedIndexChanged);
            // 
            // clm_match
            // 
            this.clm_match.Text = "Match #";
            this.clm_match.Width = 73;
            // 
            // clm_round
            // 
            this.clm_round.Text = "Round in Bracket";
            this.clm_round.Width = 191;
            // 
            // clm_player1
            // 
            this.clm_player1.Text = "Team 1 Player 1";
            this.clm_player1.Width = 191;
            // 
            // clm_player2
            // 
            this.clm_player2.Text = "Team 1 Player 2";
            this.clm_player2.Width = 191;
            // 
            // clm_player3
            // 
            this.clm_player3.Text = "Team 2 Player 1";
            this.clm_player3.Width = 191;
            // 
            // clm_player4
            // 
            this.clm_player4.Text = "Team 2 Player 2";
            this.clm_player4.Width = 191;
            // 
            // btn_movedown
            // 
            this.btn_movedown.Enabled = false;
            this.btn_movedown.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_movedown.Location = new System.Drawing.Point(1076, 240);
            this.btn_movedown.Name = "btn_movedown";
            this.btn_movedown.Size = new System.Drawing.Size(34, 134);
            this.btn_movedown.TabIndex = 3;
            this.btn_movedown.Text = "↓";
            this.btn_movedown.UseVisualStyleBackColor = true;
            this.btn_movedown.Click += new System.EventHandler(this.btn_movedown_Click);
            // 
            // btn_moveup
            // 
            this.btn_moveup.Enabled = false;
            this.btn_moveup.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn_moveup.Location = new System.Drawing.Point(1076, 82);
            this.btn_moveup.Name = "btn_moveup";
            this.btn_moveup.Size = new System.Drawing.Size(34, 134);
            this.btn_moveup.TabIndex = 2;
            this.btn_moveup.Text = "↑";
            this.btn_moveup.UseVisualStyleBackColor = true;
            this.btn_moveup.Click += new System.EventHandler(this.btn_moveup_Click);
            // 
            // btn_push
            // 
            this.btn_push.Enabled = false;
            this.btn_push.Location = new System.Drawing.Point(897, 380);
            this.btn_push.Name = "btn_push";
            this.btn_push.Size = new System.Drawing.Size(173, 37);
            this.btn_push.TabIndex = 4;
            this.btn_push.Text = "Push Selected Match to Stream";
            this.btn_push.UseVisualStyleBackColor = true;
            this.btn_push.Click += new System.EventHandler(this.btn_push_Click);
            // 
            // btn_remove
            // 
            this.btn_remove.Enabled = false;
            this.btn_remove.Location = new System.Drawing.Point(539, 380);
            this.btn_remove.Name = "btn_remove";
            this.btn_remove.Size = new System.Drawing.Size(173, 37);
            this.btn_remove.TabIndex = 6;
            this.btn_remove.Text = "Remove Selected Match";
            this.btn_remove.UseVisualStyleBackColor = true;
            this.btn_remove.Click += new System.EventHandler(this.btn_remove_Click);
            // 
            // btn_edit
            // 
            this.btn_edit.Enabled = false;
            this.btn_edit.Location = new System.Drawing.Point(718, 380);
            this.btn_edit.Name = "btn_edit";
            this.btn_edit.Size = new System.Drawing.Size(173, 37);
            this.btn_edit.TabIndex = 5;
            this.btn_edit.Text = "Edit Selected Match";
            this.btn_edit.UseVisualStyleBackColor = true;
            this.btn_edit.Click += new System.EventHandler(this.btn_edit_Click);
            // 
            // btn_reset
            // 
            this.btn_reset.Location = new System.Drawing.Point(12, 380);
            this.btn_reset.Name = "btn_reset";
            this.btn_reset.Size = new System.Drawing.Size(96, 37);
            this.btn_reset.TabIndex = 7;
            this.btn_reset.Text = "Reset Queue";
            this.btn_reset.UseVisualStyleBackColor = true;
            this.btn_reset.Click += new System.EventHandler(this.btn_reset_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbx_player4);
            this.groupBox1.Controls.Add(this.cbx_player3);
            this.groupBox1.Controls.Add(this.btn_add);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cbx_player2);
            this.groupBox1.Controls.Add(this.cbx_round);
            this.groupBox1.Controls.Add(this.cbx_player1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1098, 64);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Add a Match to Stream Queue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(803, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Team 2 Player 2";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(604, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Team 2 Player 1";
            // 
            // cbx_player4
            // 
            this.cbx_player4.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player4.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player4.FormattingEnabled = true;
            this.cbx_player4.Location = new System.Drawing.Point(806, 32);
            this.cbx_player4.Name = "cbx_player4";
            this.cbx_player4.Size = new System.Drawing.Size(196, 28);
            this.cbx_player4.TabIndex = 4;
            this.cbx_player4.TextChanged += new System.EventHandler(this.cbx_player4_TextChanged);
            this.cbx_player4.Enter += new System.EventHandler(this.cbx_player4_Enter);
            // 
            // cbx_player3
            // 
            this.cbx_player3.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player3.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player3.FormattingEnabled = true;
            this.cbx_player3.Location = new System.Drawing.Point(607, 32);
            this.cbx_player3.Name = "cbx_player3";
            this.cbx_player3.Size = new System.Drawing.Size(193, 28);
            this.cbx_player3.TabIndex = 3;
            this.cbx_player3.TextChanged += new System.EventHandler(this.cbx_player3_TextChanged);
            this.cbx_player3.Enter += new System.EventHandler(this.cbx_player3_Enter);
            // 
            // btn_add
            // 
            this.btn_add.Location = new System.Drawing.Point(1015, 30);
            this.btn_add.Name = "btn_add";
            this.btn_add.Size = new System.Drawing.Size(77, 28);
            this.btn_add.TabIndex = 5;
            this.btn_add.Text = "Add Match";
            this.btn_add.UseVisualStyleBackColor = true;
            this.btn_add.Click += new System.EventHandler(this.btn_add_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(402, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Team 1 Player 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Team 1 Player 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Round in Bracket";
            // 
            // cbx_player2
            // 
            this.cbx_player2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player2.FormattingEnabled = true;
            this.cbx_player2.Location = new System.Drawing.Point(405, 32);
            this.cbx_player2.Name = "cbx_player2";
            this.cbx_player2.Size = new System.Drawing.Size(196, 28);
            this.cbx_player2.TabIndex = 2;
            this.cbx_player2.TextChanged += new System.EventHandler(this.cbx_player2_TextChanged);
            this.cbx_player2.Enter += new System.EventHandler(this.cbx_player2_Enter);
            // 
            // cbx_round
            // 
            this.cbx_round.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_round.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_round.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_round.FormattingEnabled = true;
            this.cbx_round.Items.AddRange(new object[] {
            "Grand Finals",
            "Winners Finals",
            "Winners Semifinals",
            "Winners Quarters",
            "Losers Finals",
            "Losers Semifinals",
            "Losers Quarters",
            "Losers Top 8",
            "Winners Round 1",
            "Winners Round 2",
            "Winners Round 3",
            "Winners Round 4",
            "Losers Round 1",
            "Losers Round 2",
            "Losers Round 3",
            "Losers Round 4",
            "Winners Top 16",
            "Winners Top 24",
            "Winners Top 32",
            "Winners Top 48",
            "Losers Top 16",
            "Losers Top 24",
            "Losers Top 32",
            "Losers Top 48"});
            this.cbx_round.Location = new System.Drawing.Point(6, 32);
            this.cbx_round.Name = "cbx_round";
            this.cbx_round.Size = new System.Drawing.Size(194, 28);
            this.cbx_round.TabIndex = 0;
            // 
            // cbx_player1
            // 
            this.cbx_player1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player1.FormattingEnabled = true;
            this.cbx_player1.Location = new System.Drawing.Point(206, 32);
            this.cbx_player1.Name = "cbx_player1";
            this.cbx_player1.Size = new System.Drawing.Size(193, 28);
            this.cbx_player1.TabIndex = 1;
            this.cbx_player1.TextChanged += new System.EventHandler(this.cbx_player1_TextChanged);
            this.cbx_player1.Enter += new System.EventHandler(this.cbx_player1_Enter);
            // 
            // frm_streamqueue_dubs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1122, 431);
            this.Controls.Add(this.lvw_matches);
            this.Controls.Add(this.btn_movedown);
            this.Controls.Add(this.btn_moveup);
            this.Controls.Add(this.btn_push);
            this.Controls.Add(this.btn_remove);
            this.Controls.Add(this.btn_edit);
            this.Controls.Add(this.btn_reset);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_streamqueue_dubs";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Stream Queue Dashboard";
            this.Activated += new System.EventHandler(this.frm_streamqueue_dubs_Activated);
            this.Deactivate += new System.EventHandler(this.frm_streamqueue_dubs_Deactivate);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frm_streamqueue_dubs_FormClosed);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvw_matches;
        private System.Windows.Forms.ColumnHeader clm_match;
        private System.Windows.Forms.ColumnHeader clm_round;
        private System.Windows.Forms.ColumnHeader clm_player1;
        private System.Windows.Forms.ColumnHeader clm_player2;
        private System.Windows.Forms.Button btn_movedown;
        private System.Windows.Forms.Button btn_moveup;
        private System.Windows.Forms.Button btn_push;
        private System.Windows.Forms.Button btn_remove;
        private System.Windows.Forms.Button btn_edit;
        private System.Windows.Forms.Button btn_reset;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btn_add;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbx_player2;
        private System.Windows.Forms.ComboBox cbx_round;
        private System.Windows.Forms.ComboBox cbx_player1;
        private System.Windows.Forms.ColumnHeader clm_player3;
        private System.Windows.Forms.ColumnHeader clm_player4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbx_player4;
        private System.Windows.Forms.ComboBox cbx_player3;
    }
}