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
    public partial class Form3 : Form
    {
        public Form3(string message)
        {
            InitializeComponent();
            lbl_message.Text = message;
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }
    }
}
