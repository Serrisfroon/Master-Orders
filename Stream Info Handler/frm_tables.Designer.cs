namespace Stream_Info_Handler
{
    partial class frm_tables
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_tables));
            this.lbl_text = new System.Windows.Forms.Label();
            this.cbx_tables = new System.Windows.Forms.ComboBox();
            this.btn_okay = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_text
            // 
            this.lbl_text.Location = new System.Drawing.Point(12, 9);
            this.lbl_text.Name = "lbl_text";
            this.lbl_text.Size = new System.Drawing.Size(315, 47);
            this.lbl_text.TabIndex = 0;
            this.lbl_text.Text = "Select the character data directory for the game to use.";
            this.lbl_text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cbx_tables
            // 
            this.cbx_tables.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_tables.FormattingEnabled = true;
            this.cbx_tables.Location = new System.Drawing.Point(55, 59);
            this.cbx_tables.Name = "cbx_tables";
            this.cbx_tables.Size = new System.Drawing.Size(220, 21);
            this.cbx_tables.TabIndex = 1;
            // 
            // btn_okay
            // 
            this.btn_okay.Location = new System.Drawing.Point(55, 97);
            this.btn_okay.Name = "btn_okay";
            this.btn_okay.Size = new System.Drawing.Size(107, 27);
            this.btn_okay.TabIndex = 2;
            this.btn_okay.Text = "OK";
            this.btn_okay.UseVisualStyleBackColor = true;
            this.btn_okay.Click += new System.EventHandler(this.btn_okay_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(168, 97);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(107, 27);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_tables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 136);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_okay);
            this.Controls.Add(this.cbx_tables);
            this.Controls.Add(this.lbl_text);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_tables";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Select a Database";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_text;
        private System.Windows.Forms.ComboBox cbx_tables;
        private System.Windows.Forms.Button btn_okay;
        private System.Windows.Forms.Button btn_cancel;
    }
}