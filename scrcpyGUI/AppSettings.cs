using System;
using System.Windows.Forms;

namespace scrcpyGUI
{
    public partial class AppSettings : Form
    {
        public AppSettings()
        {
            InitializeComponent();
            theme.SelectedIndex = 0; // Default to Light theme
            language.SelectedIndex = 0; // Default to English
        }

        private void saveSettings_Click(object sender, EventArgs e)
        {

        }
    }
}
