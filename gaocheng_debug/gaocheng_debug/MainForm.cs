using System;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        // 私有只读成员，在构造函数中初始化
        private readonly string AbsoluteTestLogPath;

        private readonly Process CMD;

        private readonly SettingForm OwnSettingForm;
        private readonly NewOrEditTestDataForm OwnNewOrEditTestDataForm;
        private readonly MD5CalculatorForm OwnMD5CalculatorForm;

        // 私有成员变量
        private bool isModeChanged, isPathChanged;

        private int dataGroupNum;

        private int trimMode, displayMode;

        private DateTime logAndBatTimeChecker;
        private string dataHash;

        private string defaultDemoPath, defaultExePath;

        private string absoluteDirPath, projectDirName;
        private string projectDemoPath, projectExePath;
        private string projectLatestTestPath;

        // 属性
        public string DefaultDemoPath
        {
            get { return defaultDemoPath; }
            set { defaultDemoPath = value; }
        }

        public string DefaultExePath
        {
            get { return defaultExePath; }
            set { defaultExePath = value; }
        }

        public int DataGroupNum
        {
            set { dataGroupNum = value; }
        }

        // 构造函数
        public MainForm()
        { 
            AbsoluteTestLogPath = Directory.GetCurrentDirectory() + @"\test_log\";

            CMD = new Process();
            CMD.StartInfo.FileName = "cmd.exe";
            CMD.StartInfo.UseShellExecute = false;
            CMD.StartInfo.RedirectStandardInput = true;

            InitializeComponent();

            {
                string[] form1_names = { "校对工具", "oop，启动！", "高程，启动！", "QAQ", "Ciallo～(∠・ω< )⌒★", "兄弟，写多久了？", "是兄弟，就来田野打架1捞我", "让我康康你的小红车" , "(✿╹◡╹)", "٩( ╹▿╹ )۶" };
                Text = form1_names[ConstValues.Rnd.Next(0, form1_names.Length)];
            }

            {
                string[] paths = File.ReadAllLines(ConstValues.InitialDirectoriesConfigRelativePath);
                if (paths.Length == ConstValues.InitialDirectoriesConfigLines)
                {
                    defaultDemoPath = paths[0];
                    defaultExePath = paths[1];
                }
                else
                {
                    defaultDemoPath = ConstValues.DefaultDirectory;
                    defaultExePath = ConstValues.DefaultDirectory;
                    File.WriteAllText(ConstValues.InitialDirectoriesConfigRelativePath, $"{defaultDemoPath}\n{defaultExePath}");
                    MutSync.ShowMessageToWarn($"由于{ConstValues.InitialDirectoriesConfig}非法，已重置该项设置");
                }
            }

            OwnSettingForm = new SettingForm(defaultDemoPath, defaultExePath);
            OwnNewOrEditTestDataForm = new NewOrEditTestDataForm();
            OwnMD5CalculatorForm = new MD5CalculatorForm();

            RefreshFileList();
            cboProjectSelector.SelectedIndex = 0;
        }

        // ComboBox事件共用函数
        private void CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            isModeChanged = (cboTrimSelector.SelectedIndex != trimMode || cboDisplaySelector.SelectedIndex != displayMode);
        }

        // TextBox事件共用函数
        private void TxtDemoExePathOrTxtYourExePathTextChanged(object sender, EventArgs e)
        {
            isPathChanged = (txtDemoExePath.Text != projectDemoPath || txtYourExePath.Text != projectExePath);
        }

        // ToolStripMenuItem事件处理函数
        private void TsmiSettingsClick(object sender, EventArgs e)
        {
            OwnSettingForm.ShowDialog(this);
        }

        private void TsmiMD5CalculatorClick(object sender, EventArgs e)
        {
            OwnMD5CalculatorForm.ShowDialog(this);
        }

        private void TsmiHelpClick(object sender, EventArgs e)
        {
            if (File.Exists(ConstValues.ReadMeHtmlRelativePath))
            {
                Process.Start(ConstValues.ReadMeHtmlRelativePath);
            }
            else
            {
                MutSync.ShowMessageToWarn($"说明文件\n{ConstValues.ReadMeHtml}\n已被删除");
            }
        }

        // Button事件处理函数
        private void BtnNewProjectClick(object sender, EventArgs e)
        {
            string new_path = AbsoluteTestLogPath + DateTime.Now.ToString(ConstValues.ProjectNameFormatStr);
            Directory.CreateDirectory(new_path);

            logAndBatTimeChecker = DateTime.Now;
            File.WriteAllText(new_path + ConstValues.PathLogFileName, $"{logAndBatTimeChecker.ToString(ConstValues.ProjectCheckTimeFormat)}\nnew\nnull\nnull\nnull\nnull\n0\n0\n0");
            File.SetLastWriteTime(new_path + ConstValues.PathLogFileName, logAndBatTimeChecker);

            RefreshFileList();
            cboProjectSelector.SelectedIndex = 1;

            btnNewProject.Enabled = false;
            tmrWarrantyOfProjectUniqueness.Enabled = true;
        }

        private void BtnDeleteProjectClick(object sender, EventArgs e)
        {
            if (MessageBox.Show($"注意：本操作为永久删除，无法撤销\n是否要删除项目：{projectDirName}", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                if (Directory.Exists(absoluteDirPath))
                {
                    Directory.Delete(absoluteDirPath, true);
                }

                RefreshFileList();
                cboProjectSelector.SelectedIndex = 0;
            }
        }

        private void BtnOpenProjectDirectoryClick(object sender, EventArgs e)
        {
            if (Directory.Exists(absoluteDirPath))
            {
                MutSync.OpenFolder(absoluteDirPath);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n不存在");
                RefreshFileList();
                cboProjectSelector.SelectedIndex = 0;
            }
        }

        private void BtnBrowseDemoExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultDemoPath))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultDemoPath;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原demo默认浏览目录：\n{defaultDemoPath}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = ConstValues.DefaultDirectory;
            }
            ofdDemoAndYourExeSelector.Title = "选择demo文件";
            if (ofdDemoAndYourExeSelector.ShowDialog() == DialogResult.OK)
            {
                txtDemoExePath.Text = ofdDemoAndYourExeSelector.FileName;
            }
        }

        private void BtnBrowseYourExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultExePath))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultExePath;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原作业exe默认浏览目录：\n{defaultExePath}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = ConstValues.DefaultDirectory;
            }
            ofdDemoAndYourExeSelector.Title = "选择作业exe文件";
            if (ofdDemoAndYourExeSelector.ShowDialog() == DialogResult.OK)
            {
                txtYourExePath.Text = ofdDemoAndYourExeSelector.FileName;
            }
        }

        private void BtnNewOrEditTestDataClick(object sender, EventArgs e)
        {
            if (IsHaveCommonError())
            {
                return;
            }

            OwnNewOrEditTestDataForm.SetPath(absoluteDirPath);
            if (OwnNewOrEditTestDataForm.ShowDialog(this) == DialogResult.OK)
            {
                dataHash = MutSync.PartHashWithSalt(absoluteDirPath + ConstValues.TestDataFileName);
                ForceToEditLogAndBat();
                GenerateAndCompare();
            }
        }

        private void BtnRetestClick(object sender, EventArgs e)
        {
            if (IsHaveCommonError())
            {
                return;
            }

            if (!File.Exists(absoluteDirPath + ConstValues.TestDataFileName))
            {
                MutSync.ShowMessageToWarn("测试数据文件未生成\n请先完成创建/修改测试数据");
            }
            else if (dataHash != MutSync.PartHashWithSalt(absoluteDirPath + ConstValues.TestDataFileName))
            {
                MutSync.ShowMessageToWarn("测试数据文件被更改\n请进入 创建/修改测试数据 读取该文件并再次生成以保证合法");
            }
            else if (!File.Exists(absoluteDirPath + ConstValues.TestBatFileName))
            {
                MutSync.ShowMessageToWarn("测试批处理文件未生成\n请先完成创建/修改测试数据");
            }
            else if (isPathChanged)
            {
                if (MessageBox.Show("官方demo路径或作业exe路径已变更\n请确认本项目的测试数据适用于对应的exe\n如需继续测试，请按确认", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                {
                    EditLogAndBatWhileModeOrPathChanged();
                    GenerateAndCompare();
                }
            }
            else
            {
                if (logAndBatTimeChecker == File.GetLastWriteTime(absoluteDirPath + ConstValues.TestBatFileName))
                {
                    EditLogAndBatWhileModeOrPathChanged();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"{ConstValues.TestBat}被篡改，将自动重新生成");
                    ForceToEditLogAndBat();
                }

                GenerateAndCompare();
            }
        }

        // ComboBox事件处理函数
        private void CboProjectSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            if ((projectDirName = cboProjectSelector.SelectedItem.ToString()) == ConstValues.BlankItemStr)
            {
                DisableComponent();
            }
            else
            {
                absoluteDirPath = AbsoluteTestLogPath + projectDirName;
                if (Directory.Exists(absoluteDirPath))
                {
                    TryToGetPathLogInfo();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n已被删除，即将刷新项目列表");
                    RefreshFileList();
                    cboProjectSelector.SelectedIndex = 0;
                }
            }
        }

        // Timer事件处理函数
        private void TmrWarrantyOfProjectUniquenessTick(object sender, EventArgs e)
        {
            tmrWarrantyOfProjectUniqueness.Enabled = false;
            btnNewProject.Enabled = true;
        }
    }
}
