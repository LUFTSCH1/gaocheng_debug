using System;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        // 私有常量
        private const int InitialDirectoriesConfigLines = 2;
        private const int ProjectGaochengLines = 9;

        private const string ProjectNameFormatStr = "yyyy-MM-dd-HH-mm-ss";
        private const string ProjectCheckTimeFormatStr = "yyyy_MM_dd_HH_mm_ss_fffffff";

        private const string BlankItemStr = "blank";
        private const string ProjectGeneratedFlagStr = "tested";

        private const string DemoExePathTxtDefaultStr = "Demo Exe File Path";
        private const string YourExePathTxtDefaultStr = "Your Exe File Path";

        // 私有静态只读成员
        private static readonly Comparer<string> CMP = Comparer<string>.Create((x, y) => y.CompareTo(x));

        private static readonly Random RND = new Random();

        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        private static readonly ProcessStartInfo ReadMeHtmlStartInfo = new ProcessStartInfo { FileName = Global.ReadMeHtmlRelativePath, UseShellExecute = true };

        private static readonly string AbsoluteProjectDirectoryPath = $"{Directory.GetCurrentDirectory()}\\{Global.ProjectDirectory}\\";

        private static readonly string NewOrEditTestDataFormOpenTipStr = $"创建/修改测试数据 窗口已打开{Global.NewLine}{Global.NewLine}你仍可以计算文件MD5、查看使用说明、修改--trim和--display参数{Global.NewLine}但如果想进行其它操作，请继续完成 创建/修改测试数据 操作或将 创建/修改测试数据 窗体关闭";
        private static readonly string ResultTxtNotExistExceptionStr = $"{Global.CompareResult}文件不存在{Global.NewLine}{Global.NewLine}导致本异常的原因可能是：{Global.NewLine}{Global.CompareResult}被删除{Global.NewLine}上次测试时遇到异常，导致{Global.CompareResult}未能生成，但用户忽略了该情况{Global.NewLine}{Global.NewLine}本异常不影响您继续使用该项目继续测试";
        private static readonly string ProjectGaochengExceptionStr = $"{Global.ProjectGaocheng}文件不存在、不合法或被篡改{Global.NewLine}{Global.NewLine}导致本异常的原因可能是：{Global.NewLine}{Global.ProjectGaocheng}被删除{Global.NewLine}您在{Global.ProjectDirectory}中手动创建了该文件夹{Global.NewLine}您将1.6.0版本之前的项目放进了{Global.ProjectDirectory}{Global.NewLine}{Global.ProjectGaocheng}被篡改{Global.NewLine}{Global.NewLine}解决方法：{Global.NewLine}尝试在回收站中寻找本项目的{Global.ProjectGaocheng}文件并恢复{Global.NewLine}删除本项目";
        private static readonly string TestProcessExceptionStr = $"cmd运行过程发生错误或被用户中断，生成{Global.CompareResult}失败{Global.NewLine}{Global.NewLine}建议检查源程序逻辑问题 以及 每组测试数据是否合法{Global.NewLine}{Global.NewLine}Tips: 你或许可以从cmd窗口中最后尝试执行命令中的测试数据序号入手{Global.NewLine}{Global.NewLine}最后尝试结束时间：";

        private static readonly string[] NewProjectStrSet = { " Ciallo～(∠・ω< )⌒★", " ( ｀･ω･´)ゞ", $"{Global.NewLine}| ᐕ)⁾⁾", " ٩( ╹▿╹ )۶", " ミ(ﾉ-∀-)ﾉ", " (δωδ)」", " (灬╹ω╹灬)" };

        // 私有只读成员，在构造函数中初始化
        private readonly Process CMD;

        private readonly SettingForm OwnSettingForm;
        private readonly NewOrEditTestDataForm OwnNewOrEditTestDataForm;
        private readonly MD5CalculatorForm OwnMD5CalculatorForm;

        // 私有成员变量
        private bool isModeChanged, isPathChanged;

        private int dataGroupNum;

        private int trimMode, displayMode;

        private DateTime timeChecker;

        private string dataHash;

        private string defaultDemoExeDirectory, defaultYourExeDirectory;

        private string absoluteDirPath, projectDirName;
        private string projectDemoExePath, projectYourExePath;
        private string projectLatestTestPath;

        private string tempContainerForResultViewer;

        private FileStream projectGaochengLock;

        // 属性
        public string DefaultDemoExeDirectory
        {
            set { defaultDemoExeDirectory = value; }
        }

        public string DefaultYourExeDirectory
        {
            set { defaultYourExeDirectory = value; }
        }

        public int DataGroupNum
        {
            set { dataGroupNum = value; }
        }

        public string AbsoluteDirPath
        {
            get { return absoluteDirPath; }
        }

        public bool BtnNewProjectEnabled
        {
            set { btnNewProject.Enabled = value; }
        }

        // 构造函数
        public MainForm()
        {
            CMD = new Process();
            CMD.StartInfo.FileName = "cmd.exe";
            CMD.StartInfo.UseShellExecute = false;
            CMD.StartInfo.RedirectStandardInput = true;

            InitializeComponent();

            {
                string[] form1_names = { "校对工具", "oop，启动！", "高程，启动！", "QAQ", "Ciallo～(∠・ω< )⌒★", "兄弟，写多久了？", "是兄弟，就来田野打架1捞我", "让我康康你的小红车" , "(✿╹◡╹)", "٩( ╹▿╹ )۶" };
                Text = form1_names[RND.Next(0, form1_names.Length)];
            }

            {
                string[] paths = File.ReadAllLines(Global.InitialDirectoriesConfigRelativePath);
                if (paths.Length == InitialDirectoriesConfigLines)
                {
                    defaultDemoExeDirectory = paths[0];
                    defaultYourExeDirectory = paths[1];
                }
                else
                {
                    defaultDemoExeDirectory = Global.DefaultDirectory;
                    defaultYourExeDirectory = Global.DefaultDirectory;
                    File.WriteAllText(Global.InitialDirectoriesConfigRelativePath, $"{defaultDemoExeDirectory}\n{defaultYourExeDirectory}");
                    MutSync.ShowMessageToWarn($"由于{Global.InitialDirectoriesConfig}非法，已重置该项设置");
                }
            }

            OwnSettingForm = new SettingForm(this, defaultDemoExeDirectory, defaultYourExeDirectory);
            OwnNewOrEditTestDataForm = new NewOrEditTestDataForm(this);
            OwnMD5CalculatorForm = new MD5CalculatorForm(this);

            RefreshProjectList();
            cboProjectSelector.SelectedIndex = 0;
        }

        // 窗体关闭释放资源
        private void MainFormClosing(object sender, FormClosingEventArgs e)
        {
            if ((OwnNewOrEditTestDataForm.Visible || OwnMD5CalculatorForm.Visible) && !CheckOperation("有其他窗口还在开启状态，你要现在退出应用吗？", MessageBoxIcon.Warning))
            {
                e.Cancel = true;
            }
            else
            {
                OwnSettingForm.Dispose();
                OwnNewOrEditTestDataForm.Dispose();
                OwnMD5CalculatorForm.Dispose();
                DisposeProjectGaochengLock();
                Application.Exit();
            }
        }

        // ComboBox事件共用函数
        private void CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            isModeChanged = (cboTrimSelector.SelectedIndex != trimMode || cboDisplaySelector.SelectedIndex != displayMode);
        }

        // TextBox事件共用函数
        private void TxtDemoExePathOrTxtYourExePathTextChanged(object sender, EventArgs e)
        {
            isPathChanged = (txtDemoExePath.Text != projectDemoExePath || txtYourExePath.Text != projectYourExePath);
        }

        // ToolStripMenuItem事件处理函数
        private void TsmiSettingsClick(object sender, EventArgs e)
        {
            OwnSettingForm.ShowDialog();
        }

        private void TsmiMD5CalculatorClick(object sender, EventArgs e)
        {
            if (OwnMD5CalculatorForm.Visible)
            {
                if (OwnMD5CalculatorForm.WindowState == FormWindowState.Minimized)
                {
                    OwnMD5CalculatorForm.WindowState = FormWindowState.Normal;
                }
                OwnMD5CalculatorForm.BringToFront();
                OwnMD5CalculatorForm.Focus();
            }
            else
            {
                OwnMD5CalculatorForm.Show();
            }
        }

        private void TsmiHelpClick(object sender, EventArgs e)
        {
            if (File.Exists(Global.ReadMeHtmlRelativePath))
            {
                Process.Start(ReadMeHtmlStartInfo);
            }
            else
            {
                MutSync.ShowMessageToWarn($"说明文件\n{Global.ReadMeHtml}\n已被删除");
            }
        }

        // Button事件处理函数
        private void BtnNewProjectClick(object sender, EventArgs e)
        {
            string new_path = AbsoluteProjectDirectoryPath + DateTime.Now.ToString(ProjectNameFormatStr);
            Directory.CreateDirectory(new_path);

            timeChecker = DateTime.Now;
            File.WriteAllText(new_path + Global.ProjectGaochengFileName, $"{timeChecker.ToString(ProjectCheckTimeFormatStr)}\nnew\nnull\nnull\nnull\nnull\n0\n0\n0");
            File.SetLastWriteTime(new_path + Global.ProjectGaochengFileName, timeChecker);

            RefreshProjectList();
            cboProjectSelector.SelectedIndex = 1;

            btnNewProject.Enabled = false;
            tmrWarrantyOfProjectUniqueness.Enabled = true;
        }

        private void BtnDeleteProjectClick(object sender, EventArgs e)
        {
            if (CheckOperation($"注意：本操作为永久删除，无法撤销\n是否要删除项目：{projectDirName}", MessageBoxIcon.Warning))
            {
                if (Directory.Exists(absoluteDirPath))
                {
                    DisposeProjectGaochengLock();
                    try
                    {
                        Directory.Delete(absoluteDirPath, true);
                    }
                    catch (Exception ex)
                    {
                        MutSync.ShowMessageToWarn(ex.Message);
                    }
                }

                RefreshProjectList();
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
                MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n已被删除，即将刷新项目列表");
                RefreshProjectList();
                cboProjectSelector.SelectedIndex = 0;
            }
        }

        private void BtnBrowseDemoExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultDemoExeDirectory))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultDemoExeDirectory;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原demo默认浏览目录：\n{defaultDemoExeDirectory}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = Global.DefaultDirectory;
            }
            ofdDemoAndYourExeSelector.Title = "选择官方demo文件";
            if (ofdDemoAndYourExeSelector.ShowDialog() == DialogResult.OK)
            {
                txtDemoExePath.Text = ofdDemoAndYourExeSelector.FileName;
            }
        }

        private void BtnBrowseYourExeClick(object sender, EventArgs e)
        {
            if (Directory.Exists(defaultYourExeDirectory))
            {
                ofdDemoAndYourExeSelector.InitialDirectory = defaultYourExeDirectory;
            }
            else
            {
                MutSync.ShowMessageToWarn($"原作业exe默认浏览目录：\n{defaultYourExeDirectory}\n不存在，建议点击“设置”进行更改");
                ofdDemoAndYourExeSelector.InitialDirectory = Global.DefaultDirectory;
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

            if (OwnNewOrEditTestDataForm.Visible)
            {
                if (OwnNewOrEditTestDataForm.WindowState == FormWindowState.Minimized)
                {
                    OwnNewOrEditTestDataForm.WindowState = FormWindowState.Normal;
                }
                OwnNewOrEditTestDataForm.BringToFront();
                OwnNewOrEditTestDataForm.Focus();
            }
            else
            {
                btnNewProject.Enabled = false;
                ChangeComponentEnabled();
                tempContainerForResultViewer = rtxResultViewer.Text;
                rtxResultViewer.Text = NewOrEditTestDataFormOpenTipStr;
                OwnNewOrEditTestDataForm.LoadTestDataContent();
                OwnNewOrEditTestDataForm.Show();
            }
        }

        private void BtnRetestClick(object sender, EventArgs e)
        {
            if (IsHaveCommonError())
            {
                return;
            }

            if (!Directory.Exists(absoluteDirPath))
            {
                MutSync.ShowMessageToWarn($"测试项目 {projectDirName} 被删除\n你可以尝试：\n1.从回收站中恢复项目\n使用 创建/修改测试数据 自动重新建立这个项目，但测试数据需要重新构造");
                return;
            }

            if (!File.Exists(absoluteDirPath + Global.TestDataFileName))
            {
                MutSync.ShowMessageToWarn("测试数据文件未生成\n请先完成创建/修改测试数据");
            }
            else if (dataHash != MutSync.PartHashWithSalt(absoluteDirPath + Global.TestDataFileName))
            {
                MutSync.ShowMessageToWarn("测试数据文件被更改\n请进入 创建/修改测试数据 读取该文件并再次生成以保证合法");
            }
            else if (!File.Exists(absoluteDirPath + Global.TestBatFileName))
            {
                MutSync.ShowMessageToWarn("测试批处理文件未生成\n请先完成创建/修改测试数据");
            }
            else if (isPathChanged)
            {
                if (CheckOperation("官方demo路径或作业exe路径已变更\n请确认本项目的测试数据适用于对应的exe\n如需继续测试，请按确认", MessageBoxIcon.Information))
                {
                    EditLogAndBatWhileModeOrPathChanged();
                    GenerateAndCompare();
                }
            }
            else
            {
                if (timeChecker == File.GetLastWriteTime(absoluteDirPath + Global.TestBatFileName))
                {
                    EditLogAndBatWhileModeOrPathChanged();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"{Global.TestBat}被篡改，将自动重新生成");
                    ForceToEditLogAndBat();
                }

                GenerateAndCompare();
            }
        }

        // ComboBox事件处理函数
        private void CboProjectSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            DisposeProjectGaochengLock();

            if ((projectDirName = cboProjectSelector.SelectedItem.ToString()) == BlankItemStr)
            {
                DisableComponent();
            }
            else
            {
                absoluteDirPath = AbsoluteProjectDirectoryPath + projectDirName;
                if (Directory.Exists(absoluteDirPath))
                {
                    TryToGetProjectGaochengInfo();
                }
                else
                {
                    MutSync.ShowMessageToWarn($"项目\n{projectDirName}\n已被删除，即将刷新项目列表");
                    RefreshProjectList();
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
