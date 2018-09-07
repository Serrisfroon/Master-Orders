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
    public partial class frm_region : Form
    {
        public frm_region()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btn_okay_Click(object sender, EventArgs e)
        {
            frm_main.get_player_region = txt_region.Text;
            this.Close();
        }

        private void txt_region_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                frm_main.get_player_region = txt_region.Text;
                this.Close();
            }
        }
    }
}
