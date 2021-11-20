using System;
using System.Windows.Forms;

namespace Stream_Info_Handler
{
    public partial class frm_modern : Form
    {
        public frm_modern()
        {
            InitializeComponent();
            closeMenus();
        }

        private void closeMenus()
        {
            if (panelStream.Height != 0)
                panelStream.Height = 0;
            if (panelBracket.Height != 0)
                panelBracket.Height = 0;
            if (panelResults.Height != 0)
                panelResults.Height = 0;
            if (panelTools.Height != 0)
                panelTools.Height = 0;
            if (panelSettings.Height != 0)
                panelSettings.Height = 0;
        }

        private Panel previousPanel = null;
        private void openMenu(Panel openPanel)
        {
            if (previousPanel != null &&
                previousPanel != openPanel)
                previousPanel.Height = 0;
            previousPanel = openPanel;

            if (openPanel.Height == 0)
            {
                openPanel.Height = openPanel.Controls.Count * 50;

            }
            else
            {
                openPanel.Height = 0;
            }
        }

        private void ButtonStream_Click(object sender, EventArgs e)
        {
            openMenu(panelStream);
        }

        private void ButtonBracket_Click(object sender, EventArgs e)
        {
            openMenu(panelBracket);
        }

        private void ButtonResults_Click(object sender, EventArgs e)
        {
            openMenu(panelResults);
        }

        private void ButtonTools_Click(object sender, EventArgs e)
        {
            openMenu(panelTools);
        }

        private void ButtonSettings_Click(object sender, EventArgs e)
        {
            openMenu(panelSettings);
        }
    }
}
