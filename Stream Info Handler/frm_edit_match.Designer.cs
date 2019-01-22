namespace Stream_Info_Handler
{
    partial class frm_edit_match
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_edit_match));
            this.btn_apply = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbx_player2 = new System.Windows.Forms.ComboBox();
            this.cbx_round = new System.Windows.Forms.ComboBox();
            this.cbx_player1 = new System.Windows.Forms.ComboBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_apply
            // 
            this.btn_apply.Location = new System.Drawing.Point(183, 59);
            this.btn_apply.Name = "btn_apply";
            this.btn_apply.Size = new System.Drawing.Size(125, 28);
            this.btn_apply.TabIndex = 3;
            this.btn_apply.Text = "Apply Changes";
            this.btn_apply.UseVisualStyleBackColor = true;
            this.btn_apply.Click += new System.EventHandler(this.btn_apply_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(416, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 14;
            this.label3.Text = "Player 2";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Player 1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Round in Bracket";
            // 
            // cbx_player2
            // 
            this.cbx_player2.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbx_player2.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_player2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_player2.FormattingEnabled = true;
            this.cbx_player2.Location = new System.Drawing.Point(419, 25);
            this.cbx_player2.Name = "cbx_player2";
            this.cbx_player2.Size = new System.Drawing.Size(204, 28);
            this.cbx_player2.TabIndex = 2;
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
            this.cbx_round.Location = new System.Drawing.Point(12, 25);
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
            this.cbx_player1.Location = new System.Drawing.Point(212, 25);
            this.cbx_player1.Name = "cbx_player1";
            this.cbx_player1.Size = new System.Drawing.Size(201, 28);
            this.cbx_player1.TabIndex = 1;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(314, 59);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(125, 28);
            this.btn_cancel.TabIndex = 4;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_edit_match
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 98);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_apply);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbx_player2);
            this.Controls.Add(this.cbx_round);
            this.Controls.Add(this.cbx_player1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_edit_match";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Edit Match Information";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_apply;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbx_player2;
        private System.Windows.Forms.ComboBox cbx_round;
        private System.Windows.Forms.ComboBox cbx_player1;
        private System.Windows.Forms.Button btn_cancel;
    }
}