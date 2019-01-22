namespace Stream_Info_Handler
{
    partial class frm_login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_login));
            this.txt_username = new System.Windows.Forms.TextBox();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llb_forgot = new System.Windows.Forms.LinkLabel();
            this.ckb_remember = new System.Windows.Forms.CheckBox();
            this.btn_login = new System.Windows.Forms.Button();
            this.lbl_error = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txt_username
            // 
            this.txt_username.Location = new System.Drawing.Point(12, 25);
            this.txt_username.Name = "txt_username";
            this.txt_username.Size = new System.Drawing.Size(230, 20);
            this.txt_username.TabIndex = 0;
            this.txt_username.Enter += new System.EventHandler(this.txt_field_Enter);
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(12, 72);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(230, 20);
            this.txt_password.TabIndex = 1;
            this.txt_password.UseSystemPasswordChar = true;
            this.txt_password.Enter += new System.EventHandler(this.txt_field_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // llb_forgot
            // 
            this.llb_forgot.AutoSize = true;
            this.llb_forgot.Location = new System.Drawing.Point(12, 95);
            this.llb_forgot.Name = "llb_forgot";
            this.llb_forgot.Size = new System.Drawing.Size(145, 13);
            this.llb_forgot.TabIndex = 2;
            this.llb_forgot.TabStop = true;
            this.llb_forgot.Text = "Forgot Username/Password?";
            this.llb_forgot.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_forgot_LinkClicked);
            // 
            // ckb_remember
            // 
            this.ckb_remember.AutoSize = true;
            this.ckb_remember.Location = new System.Drawing.Point(12, 165);
            this.ckb_remember.Name = "ckb_remember";
            this.ckb_remember.Size = new System.Drawing.Size(172, 17);
            this.ckb_remember.TabIndex = 3;
            this.ckb_remember.Text = "Remember my login information";
            this.ckb_remember.UseVisualStyleBackColor = true;
            // 
            // btn_login
            // 
            this.btn_login.Location = new System.Drawing.Point(73, 188);
            this.btn_login.Name = "btn_login";
            this.btn_login.Size = new System.Drawing.Size(111, 36);
            this.btn_login.TabIndex = 4;
            this.btn_login.Text = "Log In";
            this.btn_login.UseVisualStyleBackColor = true;
            this.btn_login.Click += new System.EventHandler(this.btn_login_Click);
            // 
            // lbl_error
            // 
            this.lbl_error.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_error.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lbl_error.Location = new System.Drawing.Point(10, 122);
            this.lbl_error.Name = "lbl_error";
            this.lbl_error.Size = new System.Drawing.Size(232, 30);
            this.lbl_error.TabIndex = 7;
            this.lbl_error.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 236);
            this.Controls.Add(this.lbl_error);
            this.Controls.Add(this.btn_login);
            this.Controls.Add(this.ckb_remember);
            this.Controls.Add(this.llb_forgot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.txt_username);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_login";
            this.Text = "Log into Master Orders";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_username;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel llb_forgot;
        private System.Windows.Forms.CheckBox ckb_remember;
        private System.Windows.Forms.Button btn_login;
        private System.Windows.Forms.Label lbl_error;
    }
}