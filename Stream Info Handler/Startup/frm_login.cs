using SqlDatabaseLibrary;
using Stream_Info_Handler.AppSettings;
using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler.Startup
{
    public partial class frm_login : Form
    {

        public frm_login()
        {
            InitializeComponent();
            this.CenterToScreen();

            ckbRememberLogin.Checked = SettingsFile.Initialize(out string username, out string password);
            txtUsername.Text = username;
            txtPassword.Text = password;
        }

        private void btnLogin_click(object sender, EventArgs e)
        {
            string LogInMessage = UserSession.LogIn(txtUsername.Text.ToUpper(), txtUsername.Text.ToUpper(), txtPassword.Text);
            if(LogInMessage == "Success")
            {
                SettingsFile.LogInUpdate(ckbRememberLogin.Checked, txtUsername.Text.ToUpper(), txtPassword.Text);
                this.DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                MessageBox.Show(LogInMessage);
                lblErrorMessage.Text = "Login Failed! Make sure the credentials provided are correct.";
            }
        }

        private void llb_forgot_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show("If you've forgotten your Master Orders login credentials, please contact Serris via Twitter @serrisfroon");
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Enter:
                    btnLogin_click(new object(), new EventArgs());
                break;
                default:
                    return base.ProcessCmdKey(ref msg, keyData);
            }

            return true;
        }

        private void txt_field_Enter(object sender, EventArgs e)
        {
            TextBox field = (TextBox)sender;
            field.SelectionStart = 0;
            field.SelectionLength = field.Text.Length;
        }
    }
}
