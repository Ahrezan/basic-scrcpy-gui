using System;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace scrcpyGUI
{
    public partial class Form1 : Form
    {
        // scrcpy işlemini burada saklıyoruz
        private Process scrcpyProcess;

        public Form1()
        {
            InitializeComponent();
        }

        private void startBtn_Click(object sender, EventArgs e)
        {
            var arguments = new StringBuilder();

            if (fullscreenCheckBox.Checked)
            {
                arguments.Append("--fullscreen ");
            }

            if (noaudioCheckBox.Checked)
            {
                arguments.Append("--no-audio ");
            }
            
            if (nocontrolCheckBox.Checked)
            {
                arguments.Append("--no-control ");
            }
            
            if (printfpsCheckBox.Checked)
            {
                arguments.Append("--print-fps ");
            }
                        
            if (novideoCheckBox.Checked)
            {
                arguments.Append("--no-video ");
            }

            if (int.TryParse(angleTextBox.Text, out int angle) && angle >= 0 && angle <= 360)
            {
                arguments.Append($"--angle={angle} ");
            }
            else
            {
                arguments.Append($"--angle=0 ");
            }

            if (int.TryParse(fpsTextBox.Text, out int fps) && fps > 0)
            {
                arguments.Append($"--max-fps={fps} ");
            }
            else
            {
                arguments.Append($"--max-fps=60 "); // Default 60 FPS
            }

            if (orientationComboBox.SelectedItem != null)
            {
                string orientation = orientationComboBox.SelectedItem.ToString();

                // Eğer orientation "locked" seçeneği ise
                if (orientation == "Locked to the initial orientation")
                {
                    arguments.Append("--capture-orientation=@ ");
                }
                else if (orientation.StartsWith("@"))
                {
                    // Eğer @ ile başlıyorsa, @ parametresini kullanıyoruz
                    arguments.Append($"--capture-orientation={orientation} ");
                }
                else
                {
                    // Normal orientation parametreleri
                    switch (orientation)
                    {
                        case "0°":
                            arguments.Append(" --capture-orientation=0 ");
                            break;
                        case "90°":
                            arguments.Append(" --capture-orientation=90 ");
                            break;
                        case "180°":
                            arguments.Append(" --capture-orientation=180 ");
                            break;
                        case "270°":
                            arguments.Append(" --capture-orientation=270 ");
                            break;
                        case "Flip 0°":
                            arguments.Append(" --capture-orientation=flip0 ");
                            break;
                        case "Flip 90°":
                            arguments.Append(" --capture-orientation=flip90 ");
                            break;
                        case "Flip 180°":
                            arguments.Append(" --capture-orientation=flip180 ");
                            break;
                        case "Flip 270°":
                            arguments.Append(" --capture-orientation=flip270 ");
                            break;
                    }
                }
            }


            string args = arguments.ToString().Trim();
            string exePath = Application.StartupPath + @"\cli_tools\scrcpy.exe";

            var psi = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = args,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = false,
                RedirectStandardError = false
            };

            try
            {
                scrcpyProcess = Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"scrcpy başlatılamadı:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void stopBtn_Click(object sender, EventArgs e)
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
                MessageBox.Show($"scrcpy kapatılamadı:\n{ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
