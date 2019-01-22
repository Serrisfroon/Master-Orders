namespace Stream_Info_Handler
{
    partial class frm_replace_character
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_replace_character));
            this.lbl_character = new System.Windows.Forms.Label();
            this.btn_dont_replace = new System.Windows.Forms.Button();
            this.btn_character1 = new System.Windows.Forms.Button();
            this.btn_character2 = new System.Windows.Forms.Button();
            this.btn_character3 = new System.Windows.Forms.Button();
            this.btn_character4 = new System.Windows.Forms.Button();
            this.btn_character5 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lbl_character
            // 
            this.lbl_character.Location = new System.Drawing.Point(12, 9);
            this.lbl_character.Name = "lbl_character";
            this.lbl_character.Size = new System.Drawing.Size(374, 40);
            this.lbl_character.TabIndex = 0;
            this.lbl_character.Text = "PLAYER already has 5 characters! Select one to replace with CHARACTER.";
            this.lbl_character.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_dont_replace
            // 
            this.btn_dont_replace.Location = new System.Drawing.Point(145, 140);
            this.btn_dont_replace.Name = "btn_dont_replace";
            this.btn_dont_replace.Size = new System.Drawing.Size(112, 32);
            this.btn_dont_replace.TabIndex = 5;
            this.btn_dont_replace.Text = "Do not replace";
            this.btn_dont_replace.UseVisualStyleBackColor = true;
            this.btn_dont_replace.Click += new System.EventHandler(this.btn_okay_Click);
            // 
            // btn_character1
            // 
            this.btn_character1.Location = new System.Drawing.Point(15, 62);
            this.btn_character1.Name = "btn_character1";
            this.btn_character1.Size = new System.Drawing.Size(70, 70);
            this.btn_character1.TabIndex = 0;
            this.btn_character1.UseVisualStyleBackColor = true;
            this.btn_character1.Click += new System.EventHandler(this.btn_character1_Click);
            // 
            // btn_character2
            // 
            this.btn_character2.Location = new System.Drawing.Point(91, 62);
            this.btn_character2.Name = "btn_character2";
            this.btn_character2.Size = new System.Drawing.Size(70, 70);
            this.btn_character2.TabIndex = 1;
            this.btn_character2.UseVisualStyleBackColor = true;
            this.btn_character2.Click += new System.EventHandler(this.btn_character2_Click);
            // 
            // btn_character3
            // 
            this.btn_character3.Location = new System.Drawing.Point(167, 62);
            this.btn_character3.Name = "btn_character3";
            this.btn_character3.Size = new System.Drawing.Size(70, 70);
            this.btn_character3.TabIndex = 2;
            this.btn_character3.UseVisualStyleBackColor = true;
            this.btn_character3.Click += new System.EventHandler(this.btn_character3_Click);
            // 
            // btn_character4
            // 
            this.btn_character4.Location = new System.Drawing.Point(243, 62);
            this.btn_character4.Name = "btn_character4";
            this.btn_character4.Size = new System.Drawing.Size(70, 70);
            this.btn_character4.TabIndex = 3;
            this.btn_character4.UseVisualStyleBackColor = true;
            this.btn_character4.Click += new System.EventHandler(this.btn_character4_Click);
            // 
            // btn_character5
            // 
            this.btn_character5.Location = new System.Drawing.Point(319, 62);
            this.btn_character5.Name = "btn_character5";
            this.btn_character5.Size = new System.Drawing.Size(70, 70);
            this.btn_character5.TabIndex = 4;
            this.btn_character5.UseVisualStyleBackColor = true;
            this.btn_character5.Click += new System.EventHandler(this.btn_character5_Click);
            // 
            // frm_replace_character
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 183);
            this.ControlBox = false;
            this.Controls.Add(this.btn_character5);
            this.Controls.Add(this.btn_character4);
            this.Controls.Add(this.btn_character3);
            this.Controls.Add(this.btn_character2);
            this.Controls.Add(this.btn_character1);
            this.Controls.Add(this.btn_dont_replace);
            this.Controls.Add(this.lbl_character);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_replace_character";
            this.Text = "Replace a Character";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_character;
        private System.Windows.Forms.Button btn_dont_replace;
        private System.Windows.Forms.Button btn_character1;
        private System.Windows.Forms.Button btn_character2;
        private System.Windows.Forms.Button btn_character3;
        private System.Windows.Forms.Button btn_character4;
        private System.Windows.Forms.Button btn_character5;
    }
}