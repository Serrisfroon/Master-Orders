﻿using SqlDatabaseLibrary;
using Stream_Info_Handler.StreamAssistant;
using System;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_newplayer : Form
    {
        public frm_newplayer()
        {
            this.CenterToScreen();

            InitializeComponent();
            this.TopMost = global_values.keepWindowsOnTop;

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            if(PlayerDatabase.RecordExistsFromString(txt_tag.Text, PlayerDatabase.SearchProperty.tag))
            {
                MessageBox.Show("A player with this tag already exists! Please enter a different tag.");
                return;
            }

            StreamAssistantForm.save_name = txt_tag.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
