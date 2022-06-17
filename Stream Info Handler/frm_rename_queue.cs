using SqlDatabaseLibrary;
using System;
using System.Media;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_rename_queue : Form
    {
        int queue;
        public frm_rename_queue(int queueid, bool ontop)
        {
            InitializeComponent();
            this.CenterToScreen();
            this.TopMost = ontop;

            queue = queueid;
        }

        private void btn_rename_Click(object sender, EventArgs e)
        {
            if(txt_rename.Text != "")
            {
                StreamQueueManager.AdjustQueueSettings(queue, txt_rename.Text, StreamQueueManager.queueAdjustmentTypes.adjustName);
                StreamQueueManager.ImportStreamQueues();
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                SystemSounds.Beep.Play();
            }

        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }



        private void txt_rename_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.Enter)
            {
                btn_rename_Click(sender, e);
            }
        }
    }
}
