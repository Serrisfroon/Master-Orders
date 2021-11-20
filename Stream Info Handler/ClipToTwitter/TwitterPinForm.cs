using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stream_Info_Handler.ClipToTwitter
{
    public partial class TwitterPinForm : Form
    {
        public TwitterPinForm()
        {
            InitializeComponent();
        }

        private void btnVerify_Click(object sender, EventArgs e)
        {
            this.Tag = txtPin.Text;
            this.Close();
        }
    }
}
