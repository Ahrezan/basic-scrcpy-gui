using System;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace scrcpyGUI
{
    public partial class DownloadForm : Form
    {
        public event Action DownloadCompleted;
        public event Action DownloadCanceled;

        private WebClient webClient;
        private string cliToolsPath = Path.Combine(Application.StartupPath, "cli_tools");
        private string zipFileName = "download.zip";

        public DownloadForm()
        {
            InitializeComponent();
            this.FormClosing += DownloadForm_FormClosing;
            StartDownload();
        }

        private void StartDownload()
        {
            bool Architecture = Environment.Is64BitOperatingSystem;
            string downloadUrl = Architecture
                ? "https://github.com/Genymobile/scrcpy/releases/download/v3.2/scrcpy-win64-v3.2.zip"
                : "https://github.com/Genymobile/scrcpy/releases/download/v3.2/scrcpy-win32-v3.2.zip";

            this.Text = $"Downloading Required Files ({(Architecture ? "x64" : "x86")})";

            if (!Directory.Exists(cliToolsPath))
                Directory.CreateDirectory(cliToolsPath);

            string destinationPath = Path.Combine(cliToolsPath, zipFileName);

            webClient = new WebClient();
            webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
            webClient.DownloadFileCompleted += WebClient_DownloadFileCompleted;

            try
            {
                webClient.DownloadFileAsync(new Uri(downloadUrl), destinationPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Download could not be started: " + ex.Message);
                DownloadCanceled?.Invoke();
                this.Close();
            }
        }

        private void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void WebClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Cancelled || e.Error != null)
            {
                string zipPath = Path.Combine(cliToolsPath, zipFileName);
                if (File.Exists(zipPath)) File.Delete(zipPath);
                DownloadCanceled?.Invoke();
            }
            else
            {
                DownloadCompleted?.Invoke();
            }

            this.Close();
        }

        private void DownloadForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (webClient != null && webClient.IsBusy)
            {
                webClient.CancelAsync();
            }
        }
    }
}
