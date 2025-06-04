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
            this.topMost = new System.Windows.Forms.CheckBox();
            this.keepScreenSharing = new System.Windows.Forms.CheckBox();
            this.LanguageAndThemeGB = new System.Windows.Forms.GroupBox();
            this.language = new System.Windows.Forms.ComboBox();
            this.theme = new System.Windows.Forms.ComboBox();
            this.updateApp = new System.Windows.Forms.Button();
            this.updateScrcpy = new System.Windows.Forms.Button();
            this.saveSettings = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.VariousGB.SuspendLayout();
            this.LanguageAndThemeGB.SuspendLayout();
            this.SuspendLayout();
            // 
            // VariousGB
            // 
            this.VariousGB.Controls.Add(this.topMost);
            this.VariousGB.Controls.Add(this.keepScreenSharing);
            this.VariousGB.Location = new System.Drawing.Point(12, 3);
            this.VariousGB.Name = "VariousGB";
            this.VariousGB.Size = new System.Drawing.Size(244, 67);
            this.VariousGB.TabIndex = 0;
            this.VariousGB.TabStop = false;
            this.VariousGB.Text = "Various";
            // 
            // topMost
            // 
            this.topMost.AutoSize = true;
            this.topMost.Location = new System.Drawing.Point(6, 42);
            this.topMost.Name = "topMost";
            this.topMost.Size = new System.Drawing.Size(144, 17);
            this.topMost.TabIndex = 1;
            this.topMost.Text = "Keep app window on top";
            this.topMost.UseVisualStyleBackColor = true;
            // 
            // keepScreenSharing
            // 
            this.keepScreenSharing.AutoSize = true;
            this.keepScreenSharing.Location = new System.Drawing.Point(6, 19);
            this.keepScreenSharing.Name = "keepScreenSharing";
            this.keepScreenSharing.Size = new System.Drawing.Size(201, 17);
            this.keepScreenSharing.TabIndex = 0;
            this.keepScreenSharing.Text = "Keep screen sharing after app closes";
            this.keepScreenSharing.UseVisualStyleBackColor = true;
            // 
            // LanguageAndThemeGB
            // 
            this.LanguageAndThemeGB.Controls.Add(this.language);
            this.LanguageAndThemeGB.Controls.Add(this.theme);
            this.LanguageAndThemeGB.Controls.Add(this.updateApp);
            this.LanguageAndThemeGB.Location = new System.Drawing.Point(12, 76);
            this.LanguageAndThemeGB.Name = "LanguageAndThemeGB";
            this.LanguageAndThemeGB.Size = new System.Drawing.Size(244, 76);
            this.LanguageAndThemeGB.TabIndex = 1;
            this.LanguageAndThemeGB.TabStop = false;
            this.LanguageAndThemeGB.Text = "Language, Theme && Updates";
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
            this.updateApp.Location = new System.Drawing.Point(6, 45);
            this.updateApp.Name = "updateApp";
            this.updateApp.Size = new System.Drawing.Size(120, 23);
            this.updateApp.TabIndex = 2;
            this.updateApp.Text = "Check for all updates";
            this.updateApp.UseVisualStyleBackColor = true;
            // 
            // updateScrcpy
            // 
            this.updateScrcpy.Location = new System.Drawing.Point(12, 158);
            this.updateScrcpy.Name = "updateScrcpy";
            this.updateScrcpy.Size = new System.Drawing.Size(170, 23);
            this.updateScrcpy.TabIndex = 3;
            this.updateScrcpy.Text = "Re-download scrcpy";
            this.updateScrcpy.UseVisualStyleBackColor = true;
            // 
            // saveSettings
            // 
            this.saveSettings.Location = new System.Drawing.Point(188, 158);
            this.saveSettings.Name = "saveSettings";
            this.saveSettings.Size = new System.Drawing.Size(68, 50);
            this.saveSettings.TabIndex = 4;
            this.saveSettings.Text = "Save Settings";
            this.saveSettings.UseVisualStyleBackColor = true;
            this.saveSettings.Click += new System.EventHandler(this.saveSettings_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 185);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Report a bug";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // AppSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 220);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.saveSettings);
            this.Controls.Add(this.updateScrcpy);
            this.Controls.Add(this.LanguageAndThemeGB);
            this.Controls.Add(this.VariousGB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "AppSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.VariousGB.ResumeLayout(false);
            this.VariousGB.PerformLayout();
            this.LanguageAndThemeGB.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox VariousGB;
        private System.Windows.Forms.CheckBox keepScreenSharing;
        private System.Windows.Forms.GroupBox LanguageAndThemeGB;
        private System.Windows.Forms.CheckBox topMost;
        private System.Windows.Forms.ComboBox language;
        private System.Windows.Forms.ComboBox theme;
        private System.Windows.Forms.Button updateApp;
        private System.Windows.Forms.Button updateScrcpy;
        private System.Windows.Forms.Button saveSettings;
        private System.Windows.Forms.Button button1;
    }
}