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
    public partial class Main_Menu : Form
    {
        public Main_Menu()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var streamer = new frm_main();
            streamer.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var top8 = new Results();
            top8.Show();
        }
    }
}
