namespace scrcpyGUI
{
    partial class AppSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AppSettings));
            this.VariousGB = new System.Windows.Forms.GroupBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.LanguageAndThemeGB = new System.Windows.Forms.GroupBox();
            this.language = new System.Windows.Forms.ComboBox();
            this.theme = new System.Windows.Forms.ComboBox();
            this.updateApp = new System.Windows.Forms.Button();
            this.updateScrcpy = new System.Windows.Forms.Button();
            this.saveSettings = new System.Windows.Forms.Button();
            this.VariousGB.SuspendLayout();
            this.LanguageAndThemeGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // VariousGB
            // 
            this.VariousGB.Controls.Add(this.checkBox2);
            this.VariousGB.Controls.Add(this.checkBox1);
            this.VariousGB.Location = new System.Drawing.Point(12, 3);
            this.VariousGB.Name = "VariousGB";
            this.VariousGB.Size = new System.Drawing.Size(244, 67);
            this.VariousGB.TabIndex = 0;
            this.VariousGB.TabStop = false;
            this.VariousGB.Text = "Various";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(6, 42);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(144, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Keep app window on top";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(6, 19);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(201, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Keep screen sharing after app closes";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // LanguageAndThemeGB
            // 
            this.LanguageAndThemeGB.Controls.Add(this.language);
            this.LanguageAndThemeGB.Controls.Add(this.theme);
            this.LanguageAndThemeGB.Location = new System.Drawing.Point(12, 76);
            this.LanguageAndThemeGB.Name = "LanguageAndThemeGB";
            this.LanguageAndThemeGB.Size = new System.Drawing.Size(244, 81);
            this.LanguageAndThemeGB.TabIndex = 1;
            this.LanguageAndThemeGB.TabStop = false;
            this.LanguageAndThemeGB.Text = "Language && Theme";
            // 
            // language
            // 
            this.language.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.language.FormattingEnabled = true;
            this.language.Items.AddRange(new object[] {
            "English",
            "Türkçe",
            "Español",
            "中国人"});
            this.language.Location = new System.Drawing.Point(132, 46);
            this.language.Name = "language";
            this.language.Size = new System.Drawing.Size(106, 21);
            this.language.TabIndex = 1;
            // 
            // theme
            // 
            this.theme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.theme.FormattingEnabled = true;
            this.theme.Items.AddRange(new object[] {
            "System default theme (Light)",
            "Dark theme"});
            this.theme.Location = new System.Drawing.Point(6, 19);
            this.theme.Name = "theme";
            this.theme.Size = new System.Drawing.Size(232, 21);
            this.theme.TabIndex = 0;
            // 
            // updateApp
            // 
            this.updateApp.Location = new System.Drawing.Point(12, 163);
            this.updateApp.Name = "updateApp";
            this.updateApp.Size = new System.Drawing.Size(172, 23);
            this.updateApp.TabIndex = 2;
            this.updateApp.Text = "Check for application updates";
            this.updateApp.UseVisualStyleBackColor = true;
            // 
            // updateScrcpy
            // 
            this.updateScrcpy.Location = new System.Drawing.Point(12, 192);
            this.updateScrcpy.Name = "updateScrcpy";
            this.updateScrcpy.Size = new System.Drawing.Size(172, 23);
            this.updateScrcpy.TabIndex = 3;
            this.updateScrcpy.Text = "Check for Scrcpy updates";
            this.updateScrcpy.UseVisualStyleBackColor = true;
            // 
            // saveSettings
            // 
            this.saveSettings.Location = new System.Drawing.Point(190, 163);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(66, 52);
            this.saveSettings.TabIndex = 4;
            this.saveSettings.Text = "Save Settings";
            this.saveSettings.UseVisualStyleBackColor = true;
            // 
            // AppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 223);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.updateScrcpy);
            this.Controls.Add(this.updateApp);
            this.Controls.Add(this.LanguageAndThemeGB);
            this.Controls.Add(this.VariousGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AppSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.AppSettings_Load);
            this.VariousGB.ResumeLayout(false);
            this.VariousGB.PerformLayout();
            this.LanguageAndThemeGB.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox VariousGB;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox LanguageAndThemeGB;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox language;
        private System.Windows.Forms.ComboBox theme;
        private System.Windows.Forms.Button updateApp;
        private System.Windows.Forms.Button updateScrcpy;
        private System.Windows.Forms.Button saveSettings;
    }
}