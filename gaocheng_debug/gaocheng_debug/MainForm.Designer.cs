namespace gaocheng_debug
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.cboProjectSelector = new System.Windows.Forms.ComboBox();
            this.lblProjectTip = new System.Windows.Forms.Label();
            this.mnsAdditionalFunctions = new System.Windows.Forms.MenuStrip();
            this.tsmiSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiMD5Calculator = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiHelp = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNewProject = new System.Windows.Forms.Button();
            this.txtDemoExePath = new System.Windows.Forms.TextBox();
            this.lblDemoExeTip = new System.Windows.Forms.Label();
            this.btnBrowseDemoExe = new System.Windows.Forms.Button();
            this.btnBrowseYourExe = new System.Windows.Forms.Button();
            this.txtYourExePath = new System.Windows.Forms.TextBox();
            this.lblYourExeTip = new System.Windows.Forms.Label();
            this.btnRetest = new System.Windows.Forms.Button();
            this.cboDisplaySelector = new System.Windows.Forms.ComboBox();
            this.cboTrimSelector = new System.Windows.Forms.ComboBox();
            this.btnNewOrEditTestData = new System.Windows.Forms.Button();
            this.rtxResultViewer = new System.Windows.Forms.RichTextBox();
            this.tmrWarrantyOfProjectUniqueness = new System.Windows.Forms.Timer(this.components);
            this.ofdDemoAndYourExeSelector = new System.Windows.Forms.OpenFileDialog();
            this.btnOpenProjectDirectory = new System.Windows.Forms.Button();
            this.btnDeleteProject = new System.Windows.Forms.Button();
            this.mnsAdditionalFunctions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cboProjectSelector
            // 
            this.cboProjectSelector.BackColor = System.Drawing.SystemColors.Window;
            this.cboProjectSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProjectSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboProjectSelector.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboProjectSelector.FormattingEnabled = true;
            this.cboProjectSelector.Items.AddRange(new object[] {
            "blank"});
            this.cboProjectSelector.Location = new System.Drawing.Point(97, 30);
            this.cboProjectSelector.Margin = new System.Windows.Forms.Padding(2);
            this.cboProjectSelector.Name = "cboProjectSelector";
            this.cboProjectSelector.Size = new System.Drawing.Size(272, 26);
            this.cboProjectSelector.TabIndex = 0;
            this.cboProjectSelector.TabStop = false;
            this.cboProjectSelector.SelectedIndexChanged += new System.EventHandler(this.CboProjectSelectorSelectedIndexChanged);
            // 
            // lblProjectTip
            // 
            this.lblProjectTip.AutoSize = true;
            this.lblProjectTip.Location = new System.Drawing.Point(10, 31);
            this.lblProjectTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblProjectTip.Name = "lblProjectTip";
            this.lblProjectTip.Size = new System.Drawing.Size(69, 20);
            this.lblProjectTip.TabIndex = 1;
            this.lblProjectTip.Text = "测试项目";
            // 
            // mnsAdditionalFunctions
            // 
            this.mnsAdditionalFunctions.BackColor = System.Drawing.Color.Transparent;
            this.mnsAdditionalFunctions.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.mnsAdditionalFunctions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSettings,
            this.tsmiMD5Calculator,
            this.tsmiHelp});
            this.mnsAdditionalFunctions.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.mnsAdditionalFunctions.Location = new System.Drawing.Point(0, 0);
            this.mnsAdditionalFunctions.Name = "mnsAdditionalFunctions";
            this.mnsAdditionalFunctions.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.mnsAdditionalFunctions.Size = new System.Drawing.Size(927, 28);
            this.mnsAdditionalFunctions.TabIndex = 2;
            this.mnsAdditionalFunctions.Text = "menuStrip1";
            // 
            // tsmiSettings
            // 
            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(53, 24);
            this.tsmiSettings.Text = "设置";
            this.tsmiSettings.Click += new System.EventHandler(this.TsmiSettingsClick);
            // 
            // tsmiMD5Calculator
            // 
            this.tsmiMD5Calculator.Name = "tsmiMD5Calculator";
            this.tsmiMD5Calculator.Size = new System.Drawing.Size(118, 24);
            this.tsmiMD5Calculator.Text = "计算文件MD5";
            this.tsmiMD5Calculator.Click += new System.EventHandler(this.TsmiMD5CalculatorClick);
            // 
            // tsmiHelp
            // 
            this.tsmiHelp.Name = "tsmiHelp";
            this.tsmiHelp.Size = new System.Drawing.Size(83, 24);
            this.tsmiHelp.Text = "使用说明";
            this.tsmiHelp.Click += new System.EventHandler(this.TsmiHelpClick);
            // 
            // btnNewProject
            // 
            this.btnNewProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(190)))), ((int)(((byte)(138)))));
            this.btnNewProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewProject.Location = new System.Drawing.Point(373, 26);
            this.btnNewProject.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewProject.Name = "btnNewProject";
            this.btnNewProject.Size = new System.Drawing.Size(52, 30);
            this.btnNewProject.TabIndex = 3;
            this.btnNewProject.TabStop = false;
            this.btnNewProject.Text = "新建";
            this.btnNewProject.UseVisualStyleBackColor = false;
            this.btnNewProject.Click += new System.EventHandler(this.BtnNewProjectClick);
            // 
            // txtDemoExePath
            // 
            this.txtDemoExePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtDemoExePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDemoExePath.Location = new System.Drawing.Point(97, 66);
            this.txtDemoExePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtDemoExePath.Name = "txtDemoExePath";
            this.txtDemoExePath.ReadOnly = true;
            this.txtDemoExePath.Size = new System.Drawing.Size(454, 25);
            this.txtDemoExePath.TabIndex = 4;
            this.txtDemoExePath.TabStop = false;
            this.txtDemoExePath.TextChanged += new System.EventHandler(this.TxtDemoExePathOrTxtYourExePathTextChanged);
            // 
            // lblDemoExeTip
            // 
            this.lblDemoExeTip.AutoSize = true;
            this.lblDemoExeTip.Location = new System.Drawing.Point(10, 68);
            this.lblDemoExeTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDemoExeTip.Name = "lblDemoExeTip";
            this.lblDemoExeTip.Size = new System.Drawing.Size(82, 20);
            this.lblDemoExeTip.TabIndex = 5;
            this.lblDemoExeTip.Text = "官方demo";
            // 
            // btnBrowseDemoExe
            // 
            this.btnBrowseDemoExe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(187)))), ((int)(((byte)(208)))));
            this.btnBrowseDemoExe.Enabled = false;
            this.btnBrowseDemoExe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseDemoExe.Location = new System.Drawing.Point(555, 63);
            this.btnBrowseDemoExe.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowseDemoExe.Name = "btnBrowseDemoExe";
            this.btnBrowseDemoExe.Size = new System.Drawing.Size(52, 30);
            this.btnBrowseDemoExe.TabIndex = 6;
            this.btnBrowseDemoExe.TabStop = false;
            this.btnBrowseDemoExe.Text = "浏览";
            this.btnBrowseDemoExe.UseVisualStyleBackColor = false;
            this.btnBrowseDemoExe.Click += new System.EventHandler(this.BtnBrowseDemoExeClick);
            // 
            // btnBrowseYourExe
            // 
            this.btnBrowseYourExe.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(187)))), ((int)(((byte)(208)))));
            this.btnBrowseYourExe.Enabled = false;
            this.btnBrowseYourExe.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrowseYourExe.Location = new System.Drawing.Point(555, 100);
            this.btnBrowseYourExe.Margin = new System.Windows.Forms.Padding(2);
            this.btnBrowseYourExe.Name = "btnBrowseYourExe";
            this.btnBrowseYourExe.Size = new System.Drawing.Size(52, 30);
            this.btnBrowseYourExe.TabIndex = 7;
            this.btnBrowseYourExe.TabStop = false;
            this.btnBrowseYourExe.Text = "浏览";
            this.btnBrowseYourExe.UseVisualStyleBackColor = false;
            this.btnBrowseYourExe.Click += new System.EventHandler(this.BtnBrowseYourExeClick);
            // 
            // txtYourExePath
            // 
            this.txtYourExePath.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtYourExePath.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtYourExePath.Location = new System.Drawing.Point(97, 103);
            this.txtYourExePath.Margin = new System.Windows.Forms.Padding(2);
            this.txtYourExePath.Name = "txtYourExePath";
            this.txtYourExePath.ReadOnly = true;
            this.txtYourExePath.Size = new System.Drawing.Size(454, 25);
            this.txtYourExePath.TabIndex = 8;
            this.txtYourExePath.TabStop = false;
            this.txtYourExePath.TextChanged += new System.EventHandler(this.TxtDemoExePathOrTxtYourExePathTextChanged);
            // 
            // lblYourExeTip
            // 
            this.lblYourExeTip.AutoSize = true;
            this.lblYourExeTip.Location = new System.Drawing.Point(10, 105);
            this.lblYourExeTip.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblYourExeTip.Name = "lblYourExeTip";
            this.lblYourExeTip.Size = new System.Drawing.Size(65, 20);
            this.lblYourExeTip.TabIndex = 9;
            this.lblYourExeTip.Text = "作业exe";
            // 
            // btnRetest
            // 
            this.btnRetest.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnRetest.Enabled = false;
            this.btnRetest.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRetest.Location = new System.Drawing.Point(713, 100);
            this.btnRetest.Margin = new System.Windows.Forms.Padding(2);
            this.btnRetest.Name = "btnRetest";
            this.btnRetest.Size = new System.Drawing.Size(102, 30);
            this.btnRetest.TabIndex = 0;
            this.btnRetest.TabStop = false;
            this.btnRetest.Text = "重复测试";
            this.btnRetest.UseVisualStyleBackColor = false;
            this.btnRetest.Click += new System.EventHandler(this.BtnRetestClick);
            // 
            // cboDisplaySelector
            // 
            this.cboDisplaySelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDisplaySelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboDisplaySelector.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboDisplaySelector.FormattingEnabled = true;
            this.cboDisplaySelector.Items.AddRange(new object[] {
            "--display normal ",
            "--display detailed "});
            this.cboDisplaySelector.Location = new System.Drawing.Point(753, 28);
            this.cboDisplaySelector.Margin = new System.Windows.Forms.Padding(2);
            this.cboDisplaySelector.Name = "cboDisplaySelector";
            this.cboDisplaySelector.Size = new System.Drawing.Size(163, 28);
            this.cboDisplaySelector.TabIndex = 11;
            this.cboDisplaySelector.TabStop = false;
            this.cboDisplaySelector.SelectedIndexChanged += new System.EventHandler(this.CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged);
            // 
            // cboTrimSelector
            // 
            this.cboTrimSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTrimSelector.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboTrimSelector.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cboTrimSelector.FormattingEnabled = true;
            this.cboTrimSelector.Items.AddRange(new object[] {
            "--trim none ",
            "--trim right ",
            "--trim left "});
            this.cboTrimSelector.Location = new System.Drawing.Point(619, 28);
            this.cboTrimSelector.Margin = new System.Windows.Forms.Padding(2);
            this.cboTrimSelector.Name = "cboTrimSelector";
            this.cboTrimSelector.Size = new System.Drawing.Size(130, 28);
            this.cboTrimSelector.TabIndex = 0;
            this.cboTrimSelector.TabStop = false;
            this.cboTrimSelector.SelectedIndexChanged += new System.EventHandler(this.CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged);
            // 
            // btnNewOrEditTestData
            // 
            this.btnNewOrEditTestData.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(93)))), ((int)(((byte)(190)))), ((int)(((byte)(138)))));
            this.btnNewOrEditTestData.Enabled = false;
            this.btnNewOrEditTestData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewOrEditTestData.Location = new System.Drawing.Point(679, 63);
            this.btnNewOrEditTestData.Margin = new System.Windows.Forms.Padding(2);
            this.btnNewOrEditTestData.Name = "btnNewOrEditTestData";
            this.btnNewOrEditTestData.Size = new System.Drawing.Size(168, 30);
            this.btnNewOrEditTestData.TabIndex = 12;
            this.btnNewOrEditTestData.TabStop = false;
            this.btnNewOrEditTestData.Text = "创建/修改测试数据";
            this.btnNewOrEditTestData.UseVisualStyleBackColor = false;
            this.btnNewOrEditTestData.Click += new System.EventHandler(this.BtnNewOrEditTestDataClick);
            // 
            // rtxResultViewer
            // 
            this.rtxResultViewer.BackColor = System.Drawing.SystemColors.HotTrack;
            this.rtxResultViewer.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtxResultViewer.Font = new System.Drawing.Font("宋体", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtxResultViewer.ForeColor = System.Drawing.Color.White;
            this.rtxResultViewer.Location = new System.Drawing.Point(10, 144);
            this.rtxResultViewer.Margin = new System.Windows.Forms.Padding(2);
            this.rtxResultViewer.Name = "rtxResultViewer";
            this.rtxResultViewer.ReadOnly = true;
            this.rtxResultViewer.Size = new System.Drawing.Size(906, 323);
            this.rtxResultViewer.TabIndex = 14;
            this.rtxResultViewer.TabStop = false;
            this.rtxResultViewer.Text = "";
            // 
            // tmrWarrantyOfProjectUniqueness
            // 
            this.tmrWarrantyOfProjectUniqueness.Interval = 1000;
            this.tmrWarrantyOfProjectUniqueness.Tick += new System.EventHandler(this.TmrWarrantyOfProjectUniquenessTick);
            // 
            // ofdDemoAndYourExeSelector
            // 
            this.ofdDemoAndYourExeSelector.DefaultExt = "exe";
            this.ofdDemoAndYourExeSelector.Filter = "可执行文件|*.exe";
            this.ofdDemoAndYourExeSelector.Title = "选择exe文件";
            // 
            // btnOpenProjectDirectory
            // 
            this.btnOpenProjectDirectory.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(114)))), ((int)(((byte)(61)))));
            this.btnOpenProjectDirectory.Enabled = false;
            this.btnOpenProjectDirectory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenProjectDirectory.Location = new System.Drawing.Point(488, 26);
            this.btnOpenProjectDirectory.Margin = new System.Windows.Forms.Padding(2);
            this.btnOpenProjectDirectory.Name = "btnOpenProjectDirectory";
            this.btnOpenProjectDirectory.Size = new System.Drawing.Size(119, 30);
            this.btnOpenProjectDirectory.TabIndex = 15;
            this.btnOpenProjectDirectory.TabStop = false;
            this.btnOpenProjectDirectory.Text = "打开项目目录";
            this.btnOpenProjectDirectory.UseVisualStyleBackColor = false;
            this.btnOpenProjectDirectory.Click += new System.EventHandler(this.BtnOpenProjectDirectoryClick);
            // 
            // btnDeleteProject
            // 
            this.btnDeleteProject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(28)))), ((int)(((byte)(49)))));
            this.btnDeleteProject.Enabled = false;
            this.btnDeleteProject.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteProject.Location = new System.Drawing.Point(431, 26);
            this.btnDeleteProject.Margin = new System.Windows.Forms.Padding(2);
            this.btnDeleteProject.Name = "btnDeleteProject";
            this.btnDeleteProject.Size = new System.Drawing.Size(52, 30);
            this.btnDeleteProject.TabIndex = 16;
            this.btnDeleteProject.TabStop = false;
            this.btnDeleteProject.Text = "删除";
            this.btnDeleteProject.UseVisualStyleBackColor = false;
            this.btnDeleteProject.Click += new System.EventHandler(this.BtnDeleteProjectClick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(234)))), ((int)(((byte)(239)))));
            this.ClientSize = new System.Drawing.Size(927, 478);
            this.Controls.Add(this.cboDisplaySelector);
            this.Controls.Add(this.btnDeleteProject);
            this.Controls.Add(this.cboTrimSelector);
            this.Controls.Add(this.btnOpenProjectDirectory);
            this.Controls.Add(this.rtxResultViewer);
            this.Controls.Add(this.btnNewOrEditTestData);
            this.Controls.Add(this.btnRetest);
            this.Controls.Add(this.lblYourExeTip);
            this.Controls.Add(this.txtYourExePath);
            this.Controls.Add(this.btnBrowseYourExe);
            this.Controls.Add(this.btnBrowseDemoExe);
            this.Controls.Add(this.lblDemoExeTip);
            this.Controls.Add(this.txtDemoExePath);
            this.Controls.Add(this.btnNewProject);
            this.Controls.Add(this.lblProjectTip);
            this.Controls.Add(this.cboProjectSelector);
            this.Controls.Add(this.mnsAdditionalFunctions);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnsAdditionalFunctions;
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "战地小薯条";
            this.mnsAdditionalFunctions.ResumeLayout(false);
            this.mnsAdditionalFunctions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboProjectSelector;
        private System.Windows.Forms.Label lblProjectTip;
        private System.Windows.Forms.MenuStrip mnsAdditionalFunctions;
        private System.Windows.Forms.ToolStripMenuItem tsmiSettings;
        private System.Windows.Forms.ToolStripMenuItem tsmiHelp;
        private System.Windows.Forms.Button btnNewProject;
        private System.Windows.Forms.TextBox txtDemoExePath;
        private System.Windows.Forms.Label lblDemoExeTip;
        private System.Windows.Forms.Button btnBrowseDemoExe;
        private System.Windows.Forms.Button btnBrowseYourExe;
        private System.Windows.Forms.TextBox txtYourExePath;
        private System.Windows.Forms.Label lblYourExeTip;
        private System.Windows.Forms.Button btnRetest;
        private System.Windows.Forms.ComboBox cboTrimSelector;
        private System.Windows.Forms.ComboBox cboDisplaySelector;
        private System.Windows.Forms.Button btnNewOrEditTestData;
        private System.Windows.Forms.RichTextBox rtxResultViewer;
        private System.Windows.Forms.Timer tmrWarrantyOfProjectUniqueness;
        private System.Windows.Forms.OpenFileDialog ofdDemoAndYourExeSelector;
        private System.Windows.Forms.Button btnOpenProjectDirectory;
        private System.Windows.Forms.Button btnDeleteProject;
        private System.Windows.Forms.ToolStripMenuItem tsmiMD5Calculator;
    }
}

