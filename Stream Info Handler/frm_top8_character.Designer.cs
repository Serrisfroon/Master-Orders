namespace Stream_Info_Handler
{
    partial class frm_top8_character
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_top8_character));
            this.cbx_character = new System.Windows.Forms.ComboBox();
            this.cbx_color = new System.Windows.Forms.ComboBox();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_okay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbx_character
            // 
            this.cbx_character.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.cbx_character.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbx_character.FormattingEnabled = true;
            this.cbx_character.Location = new System.Drawing.Point(26, 12);
            this.cbx_character.Name = "cbx_character";
            this.cbx_character.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.cbx_character.Size = new System.Drawing.Size(183, 21);
            this.cbx_character.TabIndex = 0;
            this.cbx_character.SelectedIndexChanged += new System.EventHandler(this.cbx_character_SelectedIndexChanged);
            this.cbx_character.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cbx_character_KeyUp);
            this.cbx_character.Leave += new System.EventHandler(this.cbx_character_Leave);
            // 
            // cbx_color
            // 
            this.cbx_color.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_color.Font = new System.Drawing.Font("Microsoft Sans Serif", 54.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_color.FormattingEnabled = true;
            this.cbx_color.Location = new System.Drawing.Point(61, 39);
            this.cbx_color.Name = "cbx_color";
            this.cbx_color.Size = new System.Drawing.Size(109, 91);
            this.cbx_color.TabIndex = 1;
            // 
            // btn_cancel
            // 
            this.btn_cancel.Location = new System.Drawing.Point(125, 136);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(94, 29);
            this.btn_cancel.TabIndex = 3;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_okay
            // 
            this.btn_okay.Location = new System.Drawing.Point(12, 136);
            this.btn_okay.Name = "btn_okay";
            this.btn_okay.Size = new System.Drawing.Size(94, 29);
            this.btn_okay.TabIndex = 2;
            this.btn_okay.Text = "Okay";
            this.btn_okay.UseVisualStyleBackColor = true;
            this.btn_okay.Click += new System.EventHandler(this.btn_okay_Click);
            // 
            // frm_top8_character
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(231, 177);
            this.Controls.Add(this.btn_okay);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.cbx_character);
            this.Controls.Add(this.cbx_color);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frm_top8_character";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Set the Character";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbx_character;
        private System.Windows.Forms.ComboBox cbx_color;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_okay;
    }
}