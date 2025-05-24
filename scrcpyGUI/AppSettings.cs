using System;
using System.Windows.Forms;

namespace scrcpyGUI
{
    public partial class AppSettings : Form
    {
        public AppSettings()
        {
            InitializeComponent();
        }

        private void AppSettings_Load(object sender, EventArgs e)
        {
            theme.SelectedIndex = 0;
            language.SelectedIndex = 0;
        }
    }
}
