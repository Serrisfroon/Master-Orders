using SqlDatabaseLibrary;
using SqlDatabaseLibrary.Models;
using Stream_Info_Handler.AppSettings;
using Stream_Info_Handler.AppSettings.GeneralSettings;
using Stream_Info_Handler.Startup;
using Stream_Info_Handler.StreamAssistant;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Stream_Info_Handler
{
    //Create a new event to be delegated to this form
    public delegate void closedform_event(int form_type);

    public partial class frm_startup : Form
    {
        public frm_startup()
        {
            InitializeComponent();
            this.CenterToScreen();

            YoutubeLibrary.YoutubeController.enableYoutubeFunctions = true;

            InitializeOtherToolsDropDown();
            DisplayLoginForm();

            //Check if the login was set to be remembered
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            ckb_keep_open.Checked = Convert.ToBoolean((string)xml.Root.Element("login").Element("keep-open"));
        }
        /// <summary>
        /// Opens the form associated with the provided form name
        /// </summary>
        /// <param name="formName">The name of the form</param>
        private void OpenForm(FormManagement.FormNames formName)
        {

            if (FormManagement.openForms.ContainsKey(formName) == false)
            {
                FormManagement.openForms.Add(formName, FormManagement.OpenForm(formName));
                FormManagement.openForms[formName].Show();
                this.Visible = ckb_keep_open.Checked;
            }
            else
            {
                FormManagement.openForms[formName].BringToFront();
            }
        }
        private void btn_stream_Click(object sender, EventArgs e)
        {
            if (SettingsFile.LoadSettings(FormManagement.FormNames.StreamAssistant))
            {
                OpenForm(FormManagement.FormNames.StreamAssistant);
            }
        }
        private void btn_settings_Click(object sender, EventArgs e)
        {
            if (SettingsFile.LoadSettings(FormManagement.FormNames.Settings))
            {
                var settings = new GeneralSettingsForm(0);
                settings.ShowDialog();
            }
        }

        private void ckb_keep_open_CheckedChanged(object sender, EventArgs e)
        {
            //Update the settings file
            XDocument xml = XDocument.Load(SettingsFile.settingsFile);
            xml.Root.Element("login").Element("keep-open").ReplaceWith(new XElement("keep-open", ckb_keep_open.Checked.ToString()));
            xml.Save(SettingsFile.settingsFile);
        }

        private void btn_bracket_Click(object sender, EventArgs e)
        {
            if (SettingsFile.LoadSettings(FormManagement.FormNames.BracketAssistant))
            {
                OpenForm(FormManagement.FormNames.BracketAssistant);
            }
        }

        private void clickPlayerManager(object sender, EventArgs e)
        {
            if (SettingsFile.LoadSettings(FormManagement.FormNames.PlayerManager))
            {
                OpenForm(FormManagement.FormNames.PlayerManager);
            }
        }

        private void clickTop8Generator(object sender, EventArgs e)
        {
            if (SettingsFile.LoadSettings(FormManagement.FormNames.Top8Generator))
            {
                OpenForm(FormManagement.FormNames.Top8Generator);
            }
        }

        private void clickClipTweeter(object sender, EventArgs e)
        {
            OpenForm(FormManagement.FormNames.ClipToTweet);
        }


        private void InitializeOtherToolsDropDown()
        {
            sbn_othertools.Cms.Items[0].Click += new EventHandler(clickPlayerManager);
            sbn_othertools.Cms.Items[1].Click += new EventHandler(clickTop8Generator);
            sbn_othertools.Cms.Items[2].Click += new EventHandler(clickClipTweeter);
        }

        private void DisplayLoginForm()
        {
            //Show the login box
            var loginForm = new Startup.frm_login();
            loginForm.ShowDialog();
            //Check if logged in successfully
            switch (loginForm.DialogResult)
            {
                case DialogResult.OK:

                    break;
                case DialogResult.Yes:

                    break;
                default:
                    //If not logged in, close the program
                    if (System.Windows.Forms.Application.MessageLoop)
                    {
                        // WinForms app
                        System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        // Console app
                        System.Environment.Exit(1);
                    }
                    return;
            }
        }
    }
}