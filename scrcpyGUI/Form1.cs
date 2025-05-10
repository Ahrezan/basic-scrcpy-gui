using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

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

        private void StartScrcpy()
        {
            string args = BuildArguments();
            string exePath = Application.StartupPath + @"\cli_tools\scrcpy.exe";

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
        private void startStrip_Click(object sender, EventArgs e)
        {
            StartScrcpy();
        } 

        private void clearoutputStripMenuItem_Click(object sender, EventArgs e)
        {
            consoleOutputRichTextbox.Clear();
        }
        #endregion

        private void stopStrip_Click(object sender, EventArgs e)
        {
            KillScrcpy();
        }
    }
}
