using System;
using System.Collections.Generic;
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
            string adbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cli_tools", "adb.exe");
            ProcessStartInfo psi = new ProcessStartInfo
            {
                    FileName = adbPath,
                    Arguments = "kill-server",
                    CreateNoWindow = true,
                    UseShellExecute = false
            };
            Process.Start(psi);

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

            // CheckBox Options
            var checkBoxOptions = new Dictionary<CheckBox, string>
            {
                { alwaysOnTop, "--always-on-top" },
                { fullscreen, "--fullscreen" },
                { noAudio, "--no-audio" },
                { noControl, "--no-control" },
                { printFps, "--print-fps" },
                { noVideo, "--no-video" },
                { listEncoders, "--list-encoders" },
                { stayAwake, "--stay-awake" },
                { disableScreensaver, "--disable-screensaver" },
                { killAdbOnClose, "--kill-adb-on-close" },
                { legacyPaste, "--legacy-paste" },
                { turnScreenOff, "--turn-screen-off" },
                { showTouches, "--show-touches" },
                { forceAdbForward, "--force-adb-forward" },
                { cameraHighSpeed, "--camera-high-speed" },
                { audioDup, "--audio-dup" },
                { requireAudio, "--require-audio" },
                { noPlayback, "--no-playback" },
                { noCleanup, "--no-cleanup" },
                { noMipmaps, "--no-mipmaps" },
                { noKeyRepeat, "--no-key-repeat" },
                { noMouseHover, "--no-mouse-hover" },
                { noPowerOn, "--no-power-on" },
                { noClipboardAutosync, "--no-clipboard-autosync" },
                { noDownsizeOnError, "--no-downsize-on-error" },
                { noVideoPlayback, "--no-video-playback" },
                { noWindow, "--no-window" },
                { otg, "--otg" },
                { preferText, "--prefer-text" },
                { rawKeyEvents, "--raw-key-events" },
                { windowBorderless, "--window-borderless" },
                { listApps, "--list-apps" },
                { listCameras, "--list-cameras" },
                { listCameraSizes, "--list-camera-sizes" },
                { listDisplays, "--list-displays" }
            };

            foreach (var option in checkBoxOptions)
            {
                if (option.Key.Checked)
                    arguments.Append(option.Value + " ");
            }

            // Int TextBox Options (Aslında ayırmaya gerek yok)
            var intOptions = new List<(TextBox, string)>
            {
                (angle, "--angle"),
                (maxFps, "--max-fps"),
                (videoBitRate, "--video-bit-rate"),
                (audioBitRate, "--audio-bit-rate"),
                (audioBuffer, "--audio-buffer"),
                (audioOutputBuffer, "--audio-output-buffer"),
                (screenOffTimeout, "--screen-off-timeout"),
                (windowX, "--window-x"),
                (windowY, "--window-y"),
                (windowWidth, "--window-width"),
                (windowHeight, "--window-height"),
                (cameraFps, "--camera-fps"),
                (port, "--port"),
                (timeLimit, "--time-limit"),
                (tunnelPort, "--tunnel-port")
            };

            foreach (var (box, argName) in intOptions)
            {
                if (int.TryParse(box.Text.Trim(), out int value))
                    arguments.Append($"{argName}={value} ");
            }

            // Direct String Options
            var stringOptions = new List<(TextBox, string)>
            {
                // checkb { serial, "--serial" },
                (record, "--record"),
                (videoEncoder, "--video-encoder"),
                (startApp, "--start-app"),
                (windowTitle, "--window-title"),
                (cameraAr, "--camera-ar"),
                (cameraId, "--camera-id"),
                (displayId, "--display-id"),
                (pushTarget, "--push-target"),
                //ayarlanmadı (shortcutMod, "--shortcut-mod"),
                // text (tcpip, "--tcpip"),
                // (tunnelHost, "--tunnel-host"),
                // (videoCodecOptions, "--video-codec-options"),
                // (audioCodecOptions, "--audio-codec-options")
            };

            foreach (var (box, argName) in stringOptions)
            {
                string value = box.Text.Trim();
                if (!string.IsNullOrEmpty(value))
                    arguments.Append($"{argName}={value} ");
            }

            // ComboBox Options
            var comboBoxOptions = new Dictionary<ComboBox, string>
            {
                { audioCodec, "--audio-codec" },
                { audioEncoder, "--audio-encoder" },
                { audioSource, "--audio-source" },
                { videoCodec, "--video-codec" },
                { cameraFacing, "--camera-facing" },
                { recordFormat, "--record-format" },
                { videoSource, "--video-source" },
                { captureOrientation, "--capture-orientation" },
                { displayOrientation, "--display-orientation" },
                { displayImePolicy, "--display-ime-policy" },
                { keyboard, "--keyboard" },
                { gamepad, "--gamepad" },
                { mouse, "--mouse" },
                { recordOrientation, "--record-orientation" },
                { renderDriver, "--render-driver" },
                { verbosity, "--verbosity" }
            };

            foreach (var option in comboBoxOptions)
            {
                if (!string.IsNullOrWhiteSpace(option.Key.Text))
                    arguments.Append($"{option.Value}={option.Key.Text.Trim()} ");
            }

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

                string adbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cli_tools", "adb.exe");
                if (killAdbOnClose.Checked)
                {
                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = adbPath,
                        Arguments = "kill-server",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };

                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"scrcpy kapatılamadı:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
        // Open Help.txt
        private void helpconsoleStrip_Click(object sender, EventArgs e)
        {
            string helpFilePath = Path.Combine(Application.StartupPath, "cli_tools", "help.txt");
            try
            {
                Process.Start(helpFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File could not be opened: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Open Config Folder
        private void configfolderStrip_Click(object sender, EventArgs e)
        {
            string configFilePath = Path.Combine(Application.StartupPath, "config");
            try
            {
                Process.Start(configFilePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"File could not be opened: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
