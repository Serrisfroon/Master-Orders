namespace Stream_Info_Handler
{
    partial class frm_error
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_error));
            this.lbl_message = new System.Windows.Forms.Label();
            this.btn_error_okay = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl_message
            // 
            this.lbl_message.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lbl_message.Location = new System.Drawing.Point(12, 33);
            this.lbl_message.Name = "lbl_message";
            this.lbl_message.Size = new System.Drawing.Size(298, 65);
            this.lbl_message.TabIndex = 0;
            this.lbl_message.Text = "message";
            this.lbl_message.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btn_error_okay
            // 
            this.btn_error_okay.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_error_okay.Location = new System.Drawing.Point(118, 101);
            this.btn_error_okay.Name = "btn_error_okay";
            this.btn_error_okay.Size = new System.Drawing.Size(82, 37);
            this.btn_error_okay.TabIndex = 1;
            this.btn_error_okay.Text = "Okay";
            this.btn_error_okay.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(128, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "ERROR!";
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(322, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_error_okay);
            this.Controls.Add(this.lbl_message);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form3";
            this.Text = "Error";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_message;
        private System.Windows.Forms.Button btn_error_okay;
        private System.Windows.Forms.Label label1;
    }
}