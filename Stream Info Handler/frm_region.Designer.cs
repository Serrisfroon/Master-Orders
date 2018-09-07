namespace Stream_Info_Handler
{
    partial class frm_region
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_region));
            this.label1 = new System.Windows.Forms.Label();
            this.txt_region = new System.Windows.Forms.TextBox();
            this.btn_okay = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter the player\'s region:";
            // 
            // txt_region
            // 
            this.txt_region.Location = new System.Drawing.Point(12, 25);
            this.txt_region.Name = "txt_region";
            this.txt_region.Size = new System.Drawing.Size(185, 20);
            this.txt_region.TabIndex = 1;
            this.txt_region.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txt_region_KeyDown);
            // 
            // btn_okay
            // 
            this.btn_okay.Location = new System.Drawing.Point(70, 51);
            this.btn_okay.Name = "btn_okay";
            this.btn_okay.Size = new System.Drawing.Size(75, 23);
            this.btn_okay.TabIndex = 2;
            this.btn_okay.Text = "Okay";
            this.btn_okay.UseVisualStyleBackColor = true;
            this.btn_okay.Click += new System.EventHandler(this.btn_okay_Click);
            // 
            // frm_region
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(208, 79);
            this.ControlBox = false;
            this.Controls.Add(this.btn_okay);
            this.Controls.Add(this.txt_region);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frm_region";
            this.Text = "Enter the Region";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_region;
        private System.Windows.Forms.Button btn_okay;
    }
}