namespace Stream_Info_Handler
{
    partial class frm_characters
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_characters));
            this.lbx_characters = new System.Windows.Forms.ListBox();
            this.cbx_colors = new System.Windows.Forms.ComboBox();
            this.btn_updatecharacter = new System.Windows.Forms.Button();
            this.lbl_color = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbx_characters
            // 
            this.lbx_characters.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbx_characters.FormattingEnabled = true;
            this.lbx_characters.ItemHeight = 18;
            this.lbx_characters.Location = new System.Drawing.Point(12, 12);
            this.lbx_characters.Name = "lbx_characters";
            this.lbx_characters.ScrollAlwaysVisible = true;
            this.lbx_characters.Size = new System.Drawing.Size(178, 184);
            this.lbx_characters.TabIndex = 0;
            this.lbx_characters.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // cbx_colors
            // 
            this.cbx_colors.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbx_colors.Font = new System.Drawing.Font("Microsoft Sans Serif", 54.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbx_colors.FormattingEnabled = true;
            this.cbx_colors.Location = new System.Drawing.Point(196, 12);
            this.cbx_colors.Name = "cbx_colors";
            this.cbx_colors.Size = new System.Drawing.Size(121, 91);
            this.cbx_colors.TabIndex = 1;
            this.cbx_colors.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // btn_updatecharacter
            // 
            this.btn_updatecharacter.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_updatecharacter.Location = new System.Drawing.Point(196, 140);
            this.btn_updatecharacter.Name = "btn_updatecharacter";
            this.btn_updatecharacter.Size = new System.Drawing.Size(121, 56);
            this.btn_updatecharacter.TabIndex = 2;
            this.btn_updatecharacter.Text = "Update";
            this.btn_updatecharacter.UseVisualStyleBackColor = true;
            this.btn_updatecharacter.Click += new System.EventHandler(this.btn_updatecharacter_Click);
            // 
            // lbl_color
            // 
            this.lbl_color.AutoSize = true;
            this.lbl_color.Location = new System.Drawing.Point(243, 115);
            this.lbl_color.Name = "lbl_color";
            this.lbl_color.Size = new System.Drawing.Size(31, 13);
            this.lbl_color.TabIndex = 3;
            this.lbl_color.Text = "Color";
            this.lbl_color.Click += new System.EventHandler(this.lbl_color_Click);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 208);
            this.Controls.Add(this.lbl_color);
            this.Controls.Add(this.btn_updatecharacter);
            this.Controls.Add(this.cbx_colors);
            this.Controls.Add(this.lbx_characters);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Character Selection";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lbx_characters;
        private System.Windows.Forms.ComboBox cbx_colors;
        private System.Windows.Forms.Button btn_updatecharacter;
        private System.Windows.Forms.Label lbl_color;
    }
}