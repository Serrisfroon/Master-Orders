namespace Stream_Info_Handler.Startup
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
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.llbForgot = new System.Windows.Forms.LinkLabel();
            this.ckbRememberLogin = new System.Windows.Forms.CheckBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtUsername
            // 
            this.txtUsername.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtUsername.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtUsername.Location = new System.Drawing.Point(12, 25);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(230, 20);
            this.txtUsername.TabIndex = 0;
            this.txtUsername.Enter += new System.EventHandler(this.txt_field_Enter);
            // 
            // txtPassword
            // 
            this.txtPassword.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtPassword.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtPassword.Location = new System.Drawing.Point(12, 72);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(230, 20);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Enter += new System.EventHandler(this.txt_field_Enter);
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
            // llbForgot
            // 
            this.llbForgot.AutoSize = true;
            this.llbForgot.Location = new System.Drawing.Point(12, 95);
            this.llbForgot.Name = "llbForgot";
            this.llbForgot.Size = new System.Drawing.Size(145, 13);
            this.llbForgot.TabIndex = 2;
            this.llbForgot.TabStop = true;
            this.llbForgot.Text = "Forgot Username/Password?";
            this.llbForgot.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llb_forgot_LinkClicked);
            // 
            // ckbRememberLogin
            // 
            this.ckbRememberLogin.AutoSize = true;
            this.ckbRememberLogin.Location = new System.Drawing.Point(12, 165);
            this.ckbRememberLogin.Name = "ckbRememberLogin";
            this.ckbRememberLogin.Size = new System.Drawing.Size(172, 17);
            this.ckbRememberLogin.TabIndex = 3;
            this.ckbRememberLogin.Text = "Remember my login information";
            this.ckbRememberLogin.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(73, 188);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(111, 36);
            this.btnLogin.TabIndex = 4;
            this.btnLogin.Text = "Log In";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_click);
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblErrorMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.lblErrorMessage.Location = new System.Drawing.Point(10, 122);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(232, 30);
            this.lblErrorMessage.TabIndex = 7;
            this.lblErrorMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frm_login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(256, 236);
            this.Controls.Add(this.lblErrorMessage);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.ckbRememberLogin);
            this.Controls.Add(this.llbForgot);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.txtUsername);
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

        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel llbForgot;
        private System.Windows.Forms.CheckBox ckbRememberLogin;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lblErrorMessage;
    }
}