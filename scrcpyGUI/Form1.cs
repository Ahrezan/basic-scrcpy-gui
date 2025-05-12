using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using static System.Net.WebRequestMethods;

namespace scrcpyGUI
{
    public partial class Form1 : Form
    {
        private Process scrcpyProcess;

        public Form1()
        {
            InitializeComponent();
        }

        #region Form Events
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (scrcpyProcess != null && !scrcpyProcess.HasExited)
            {
                scrcpyProcess.Kill();
                scrcpyProcess.Dispose();
                scrcpyProcess = null;
            }
        }
        #endregion

        #region Arguments

        private string BuildArguments()
        {
            var arguments = new StringBuilder();

            if (fullscreenCheckBox.Checked)
                arguments.Append("--fullscreen ");

            if (noaudioCheckBox.Checked)
                arguments.Append("--no-audio ");

            if (nocontrolCheckBox.Checked)
                arguments.Append("--no-control ");

            if (printfpsCheckBox.Checked)
                arguments.Append("--print-fps ");

            if (novideoCheckBox.Checked)
                arguments.Append("--no-video ");

            if (int.TryParse(angleTextBox.Text, out int angle) && angle >= 0 && angle <= 360)
                arguments.Append($"--angle={angle} ");
            else
                arguments.Append("--angle=0 ");

            if (int.TryParse(fpsTextBox.Text, out int fps) && fps > 0)
                arguments.Append($"--max-fps={fps} ");

            if (orientationComboBox.SelectedItem != null)
                arguments.Append($"--capture-orientation={orientationComboBox.SelectedItem} ");

            if (videocodecComboBox.SelectedItem != null)
                arguments.Append($"--video-codec={videocodecComboBox.SelectedItem} ");

            if (audiocodecComboBox.SelectedItem != null)
                arguments.Append($"--audio-codec={audiocodecComboBox.SelectedItem} ");

            if (listencodersCheckBox.Checked)
                arguments.Append("--list-encoders ");

            return arguments.ToString().Trim();
        }

        #endregion

        #region Start Scrcpy
            private void StartScrcpy()
            {
                string args = BuildArguments();
                string exePath = Path.Combine(Application.StartupPath, "cli_tools", "scrcpy.exe");

                var psi = new ProcessStartInfo
                {
                    FileName = exePath,
                    Arguments = args,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                scrcpyProcess = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true
                };

                scrcpyProcess.OutputDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        consoleOutputRichTextbox.Invoke(new Action(() =>
                        {
                            consoleOutputRichTextbox.AppendText(ev.Data + Environment.NewLine);
                            consoleOutputRichTextbox.ScrollToCaret();
                        }));
                    }
                };

                scrcpyProcess.ErrorDataReceived += (s, ev) =>
                {
                    if (ev.Data != null)
                    {
                        consoleOutputRichTextbox.Invoke(new Action(() =>
                            consoleOutputRichTextbox.AppendText("Error: " + ev.Data + Environment.NewLine)));
                    }
                };

                try
                {
                    scrcpyProcess.Start();
                    scrcpyProcess.BeginOutputReadLine();
                    scrcpyProcess.BeginErrorReadLine();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Scrcpy could not be started:\n{ex.Message}", "Error",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        #endregion

        #region Kill Scrcpy Function
        private void KillScrcpy()
        {
            try
            {
                if (scrcpyProcess != null && !scrcpyProcess.HasExited)
                {
                    scrcpyProcess.Kill();
                    scrcpyProcess.Dispose();
                    scrcpyProcess = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Scrcpy could not be closed:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region MenuStrip Command
        // Save Console Output
        private void saveoutputStrip_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog consoleoutputSaveFileDialog = new SaveFileDialog())
            {
                consoleoutputSaveFileDialog.FileName = "console_output.txt";

                consoleoutputSaveFileDialog.Filter = "All Files (*.*)|*.*|Text Files (*.txt)|*.txt";
                consoleoutputSaveFileDialog.FilterIndex = 2;

                if (consoleoutputSaveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    consoleOutputRichTextbox.SaveFile(consoleoutputSaveFileDialog.FileName, RichTextBoxStreamType.PlainText);
                }
            }

        }
        // Start Scrcpy
        private void startStrip_Click(object sender, EventArgs e)
        { 
            StartScrcpy();
        }
        // Stop Scrcpy
        private void stopStrip_Click(object sender, EventArgs e)
        {
            KillScrcpy();
        }
        // Clear Console Output
        private void clearoutputStrip_Click(object sender, EventArgs e)
        {
            consoleOutputRichTextbox.Clear();
        }
        // App Website
        private void websiteStrip_Click(object sender, EventArgs e)
        {
            string url = "https://github.com/Ahrezan/scrcpy-gui";

            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch
            {
                MessageBox.Show("Could not open the website", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // App Info
        private void aboutStrip_Click(object sender, EventArgs e)
        {
            string appName = "Scrcpy for Basic GUI";
            string version = "Version: 2025.05.12";
            string developer = "Ahrezan";
            string githubLink = "GitHub: github.com/Ahrezan";
            string license = "Licence: MIT";
            string releaseDate = "Latest Update: 2025-05-12";
            string scrcpyVersion = "Scrcpy Version: v3.2";

            string aboutMessage = $"{appName}\n{version}\n\n{developer}\n{githubLink}\n{license}\n{scrcpyVersion}\n{releaseDate}";

            MessageBox.Show(aboutMessage, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        private void helpconsoleStrip_Click(object sender, EventArgs e)
        {
            string helpFilePath = Path.Combine(Application.StartupPath, "cli_tools", "help.txt");

            // Dosyayı varsayılan uygulama (Notepad gibi) ile açmak
            try
            {
                Process.Start(helpFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File could not be opened: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
