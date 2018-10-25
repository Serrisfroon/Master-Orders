using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_newplayer : Form
    {
        public frm_newplayer()
        {
            InitializeComponent();
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
            this.DialogResult = DialogResult.Cancel;
        }

        private void btn_create_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= global_values.roster_size; i++)
            {
                if (global_values.roster[i].tag == txt_tag.Text)
                {
                    MessageBox.Show("A player with this tag already exists! Please enter a different tag.");
                    return;
                }
            }

            frm_main.save_name = txt_tag.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
