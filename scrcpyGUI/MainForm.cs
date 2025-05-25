using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Windows.Forms;


namespace scrcpyGUI
{
    public partial class MainForm : Form
    {
        #region Start
        private Process scrcpyProcess;

        public MainForm()
        {
            InitializeComponent();
            this.Shown += MainForm_Shown;
        }
        #endregion

        #region Download Scrcpy
        private string[] requiredFiles = {
            "adb.exe",
            "AdbWinApi.dll",
            "AdbWinUsbApi.dll",
            "avcodec-61.dll",
            "avformat-61.dll",
            "avutil-59.dll",
            "icon.png",
            "libusb-1.0.dll",
            "open_a_terminal_here.bat",
            "scrcpy.exe",
            "scrcpy-console.bat",
            "scrcpy-noconsole.vbs",
            "scrcpy-server",
            "SDL2.dll",
            "swresample-5.dll"
        };

        private string downloadFolder = Path.Combine(Application.StartupPath, "cli_tools");
        private string zipFileName = "download.zip";

        private void CheckAndDownloadFiles()
        {
            if (!AreRequiredFilesPresent())
            {
                var result = MessageBox.Show(
                    "Required files are missing.\nDo you want to download them?",
                    "Missing Files",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    using (var downloader = new DownloadForm())
                    {
                        downloader.DownloadCompleted += () =>
                        {
                            ExtractZipAndCleanup();
                            MessageBox.Show("Files were successfully downloaded and extracted.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        };

                        downloader.DownloadCanceled += () =>
                        {
                            DeleteFileIfExists(Path.Combine(downloadFolder, zipFileName));
                            MessageBox.Show("Download canceled.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        };

                        downloader.ShowDialog();
                    }
                }
            }
        }

        private bool AreRequiredFilesPresent()
        {
            foreach (var file in requiredFiles)
            {
                if (!File.Exists(Path.Combine(downloadFolder, file)))
                    return false;
            }
            return true;
        }

        private void ExtractZipAndCleanup()
        {
            string zipPath = Path.Combine(downloadFolder, zipFileName);
            string tempExtractPath = Path.Combine(downloadFolder, "tempExtract");

            if (File.Exists(zipPath))
            {
                if (Directory.Exists(tempExtractPath))
                    Directory.Delete(tempExtractPath, true);

                ZipFile.ExtractToDirectory(zipPath, tempExtractPath);

                string[] directories = Directory.GetDirectories(tempExtractPath);
                if (directories.Length == 1)
                {
                    string innerFolder = directories[0];

                    foreach (var file in Directory.GetFiles(innerFolder, "*", SearchOption.AllDirectories))
                    {
                        string relativePath = file.Substring(innerFolder.Length + 1);
                        string destinationPath = Path.Combine(downloadFolder, relativePath);
                        string destinationDir = Path.GetDirectoryName(destinationPath);

                        if (!Directory.Exists(destinationDir))
                            Directory.CreateDirectory(destinationDir);

                        File.Copy(file, destinationPath, true);
                    }
                }
                else
                {
                    MessageBox.Show("Unexpected ZIP structure. A single folder was expected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Directory.Delete(tempExtractPath, true);
                File.Delete(zipPath);
            }
        }

        private void DeleteFileIfExists(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        #endregion

        #region Form Events
        private bool hasCheckedFiles = false;

        private void MainForm_Shown(object sender, EventArgs e)
        {
            if(!hasCheckedFiles)
            {
                hasCheckedFiles = true;
                BeginInvoke((MethodInvoker)CheckAndDownloadFiles);
            }
        }
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string adbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "cli_tools", "adb.exe");
                if (File.Exists(adbPath))
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

                if (scrcpyProcess != null && !scrcpyProcess.HasExited)
                {
                    scrcpyProcess.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while closing:\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (scrcpyProcess != null)
                {
                    scrcpyProcess.Dispose();
                    scrcpyProcess = null;
                }
            }
        }
        #endregion

        #region Arguments

        private string BuildArguments()
        {
            var arguments = new StringBuilder();

            #region CheckBox Options
            var checkBoxOptions = new Dictionary<CheckBox, string>
            {
                { alwaysOnTop, "--always-on-top" },
                { fullscreen, "--fullscreen" },
                { noAudio, "--no-audio" },
                { noAudioPlayback, "--no-audio-playback" },
                { noControl, "--no-control" }, //
                { printFps, "--print-fps" },
                { noVideo, "--no-video" },
                { noVdDestroyContent, "--no-vd-destroy-content" },
                { noVdSystemDecorations, "--no-vd-system-decorations" },
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
                { powerOffOnClose, "--no-power-off-on-close" },
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
                { listDisplays, "--list-displays" },
                { version, "--version" },
                { m, "--m" },
                { k, "--k" },
                { g, "--g" },

            };

            foreach (var option in checkBoxOptions)
            {
                if (option.Key.Checked)
                    arguments.Append(option.Value + " ");
            }
            #endregion

            #region TextBox Options
            var textOptions = new List<(TextBox box, string argName)>
            {
                (angle,             "--angle"),
                (maxFps,            "--max-fps"),
                (videoBitRate,      "--video-bit-rate"),
                (audioBitRate,      "--audio-bit-rate"),
                (audioBuffer,       "--audio-buffer"),
                (videoBuffer,       "--video-buffer"),
                (videoEncoder,      "--video-encoder"),
                (maxSize,           "--max-size"),
                (audioOutputBuffer, "--audio-output-buffer"),
                (screenOffTimeout,  "--screen-off-timeout"),
                (record,            "--record"),

                (windowX,           "--window-x"),
                (windowY,           "--window-y"),
                (windowWidth,       "--window-width"),
                (windowHeight,      "--window-height"),
                (cameraFps,         "--camera-fps"),
                (port,              "--port"),
                (timeLimit,         "--time-limit"),
                (tunnelPort,        "--tunnel-port"),
            };

            foreach (var (box, name) in textOptions)
            {
                var txt = box.Text.Trim();
                if (!string.IsNullOrEmpty(txt))
                    arguments.Append($"{name}={txt} ").Append(' ');
            }
            #endregion

            #region ComboBox Options
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
            #endregion
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
        // Open Preferences Form (AppSetting.cs)
        private void preferencesStrip_Click(object sender, EventArgs e)
        {
            AppSettings appSettings = new AppSettings();
            appSettings.ShowDialog();
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
            string version = "Version: 2025.05.24";
            string developer = "Ahrezan";
            string githubLink = "GitHub: github.com/Ahrezan";
            string license = "Licence: Apache License 2.0";
            string releaseDate = "Latest Update: 2025-05-24";
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
