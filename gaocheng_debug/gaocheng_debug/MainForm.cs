using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.Collections.Generic;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        // 私有常量
        private const string ProjectNameFormatStr = "yyyy-MM-dd-HH-mm-ss";

        private const string BlankItemStr = "blank";

        private const string DemoExePathTxtDefaultStr = "Demo Exe File Path";
        private const string YourExePathTxtDefaultStr = "Your Exe File Path";

        // 私有静态只读成员
        private static readonly Color TimeInfoColor  = Color.FromArgb(144, 238, 144);
        private static readonly Color ErrorInfoColor = Color.FromArgb(255, 99, 71);
        private static readonly Color TipColor       = Color.FromArgb(255, 165, 0);

        private static readonly Comparer<string> CMP = Comparer<string>.Create((x, y) => y.CompareTo(x));

        private static readonly Random RND = new Random();

        private static readonly CultureInfo InvariantCulture = CultureInfo.InvariantCulture;

        private static readonly ProcessStartInfo ReadMeHtmlStartInfo = new ProcessStartInfo { FileName = Global.ReadMeHtmlRelativePath, UseShellExecute = true };
        private static readonly ProcessStartInfo RepositoryAddressStartInfo = new ProcessStartInfo { FileName = "https://github.com/LUFTSCH1/gaocheng_debug", UseShellExecute = true };

        private static readonly string NewOrEditTestDataFormOpenTipStr =   $"创建/修改测试数据 窗口已打开{Global.NewLine}{Global.NewLine}"
                                                                         + $"你仍可以计算文件MD5、查看使用说明、修改--trim和--display参数{Global.NewLine}"
                                                                         + $"但如果想进行其它操作，请继续完成 创建/修改测试数据 操作或将 创建/修改测试数据 窗体关闭";
        private static readonly string ResultTxtNotExistExceptionStr   =   $"{Global.CompareResult}文件不存在{Global.NewLine}{Global.NewLine}"
                                                                         + $"导致本异常的原因可能是：{Global.NewLine}"
                                                                         + $"{Global.CompareResult}被删除{Global.NewLine}"
                                                                         + $"上次测试时遇到异常，导致{Global.CompareResult}未能生成，但用户忽略了该情况{Global.NewLine}{Global.NewLine}"
                                                                         + $"本异常不影响您继续使用该项目继续测试";
        private static readonly string ProjectGaochengExceptionStr     =   $"{Global.ProjectGaocheng}文件不存在或不合法{Global.NewLine}{Global.NewLine}"
                                                                         + $"导致本异常的原因可能是：{Global.NewLine}"
                                                                         + $"{Global.ProjectGaocheng}被删除{Global.NewLine}"
                                                                         + $"您在{Global.ProjectDirectory}中手动创建了该文件夹{Global.NewLine}"
                                                                         + $"您将1.7.0版本之前的项目放进了{Global.ProjectDirectory}{Global.NewLine}{Global.NewLine}"
                                                                         + $"解决方法：{Global.NewLine}"
                                                                         + $"尝试在回收站中寻找本项目的{Global.ProjectGaocheng}文件并恢复{Global.NewLine}"
                                                                         + $"删除本项目";
        private static readonly string TestProcessExceptionStr         =   $"cmd运行过程发生错误或被用户中断，生成{Global.CompareResult}失败{Global.NewLine}{Global.NewLine}"
                                                                         + $"建议检查源程序逻辑问题 以及 每组测试数据是否合法{Global.NewLine}{Global.NewLine}"
                                                                         + $"Tips: 你或许可以从cmd窗口中最后尝试执行命令中的测试数据序号入手{Global.NewLine}{Global.NewLine}"
                                                                         + $"最后尝试结束时间：";

        private static readonly string[] NewProjectStrSet = new string[] {
            " Ciallo～(∠・ω< )⌒★", " ( ｀･ω･´)ゞ", $"{Global.NewLine}| ᐕ)⁾⁾",
            " ٩( ╹▿╹ )۶", " ミ(ﾉ-∀-)ﾉ", " (灬╹ω╹灬)"
        };

        // 私有只读成员，在构造函数中初始化
        private readonly string AbsoluteProjectDirectoryPath;

        private readonly string AbsoluteGetInputDataPath;
        private readonly string AbsoluteTxtComparePath;

        private readonly Process InterfaceProgram;

        private readonly SettingForm OwnSettingForm;
        private readonly NewOrEditTestDataForm OwnNewOrEditTestDataForm;
        private readonly MD5CalculatorForm OwnMD5CalculatorForm;

        // 私有成员变量
        private bool isModeChanged, isPathChanged;

        private int dataGroupNum;

        private int trimMode, displayMode;

        private string dataHash;

        private string defaultDemoExeDirectory, defaultYourExeDirectory;

        private string absoluteDirPath, projectDirName;
        private string projectDemoExePath, projectYourExePath;

        private string absoluteProjectGaochengPath;
        private string absoluteTestDataPath;
        private string absoluteDemoExeResultPath;
        private string absoluteYourExeResultPath;
        private string absoluteCompareResultPath;

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

        public string AbsoluteTestDataPath
        {
            get { return absoluteTestDataPath; }
        }

        // 构造函数
        public MainForm()
        {
            {
                string app_path = Directory.GetCurrentDirectory();
                AbsoluteProjectDirectoryPath = $"{app_path}\\{Global.ProjectDirectory}\\";
                AbsoluteGetInputDataPath     = $"\"{app_path}\\{Global.ResourceDirectory}\\{Global.GetInputData}\"";
                AbsoluteTxtComparePath       = $"\"{app_path}\\{Global.ResourceDirectory}\\{Global.TxtCompare}\"";
            }

            InitializeComponent();

            {
                string[] defaults = MutSync.ReadAllLines(Global.DefaultSettingsRelativePath);
                if (defaults.Length == Global.DefaultSettingsLines)
                {
                    defaultDemoExeDirectory = defaults[0];
                    defaultYourExeDirectory = defaults[1];
                }
                else
                {
                    defaultDemoExeDirectory = Global.DefaultDirectory;
                    defaultYourExeDirectory = Global.DefaultDirectory;
                    MutSync.WriteAllText(Global.DefaultSettingsRelativePath, $"{Global.DefaultDirectory}\n{Global.DefaultDirectory}", Encoding.UTF8);
                    MutSync.ShowMessageToWarn($"由于{Global.DefaultSettings}非法，已重置设置");
                }
            }

            InterfaceProgram = new Process();
            InterfaceProgram.StartInfo.FileName = "cmd.exe";
            InterfaceProgram.StartInfo.UseShellExecute = false;
            InterfaceProgram.StartInfo.RedirectStandardInput = true;

            {
                string[] form1_names = {
                    "校对工具", "oop，启动！", "高程，启动！",
                    "(✿╹◡╹)", "Ciallo～(∠・ω< )⌒★", "兄弟，写多久了？",
                    "EA的🐎似了（Oct 22, 2024）", "让我康康你的小红车", "٩( ╹▿╹ )۶"
                };
                Text = form1_names[RND.Next(0, form1_names.Length)];
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
            if ((OwnNewOrEditTestDataForm.Visible || OwnMD5CalculatorForm.Visible) &&
                !MutSync.CheckOperation("有其他窗口还在开启状态，你要现在退出应用吗？", MessageBoxIcon.Warning))
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
        private void CboTrimSelectorOrCboDisplaySelectorSelectedIndexChanged(object sender, EventArgs e) =>
            isModeChanged = (cboTrimSelector.SelectedIndex != trimMode || cboDisplaySelector.SelectedIndex != displayMode);

        // TextBox事件共用函数
        private void TxtDemoExePathOrTxtYourExePathTextChanged(object sender, EventArgs e) =>
            isPathChanged = (txtDemoExePath.Text != projectDemoExePath || txtYourExePath.Text != projectYourExePath);

        // ToolStripMenuItem事件处理函数
        private void TsmiSettingsClick(object sender, EventArgs e) =>
            OwnSettingForm.ShowDialog();

        private void TsmiMD5CalculatorClick(object sender, EventArgs e) =>
            MutSync.BringToFrontAndFocus(OwnMD5CalculatorForm);

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

        private void TsmiRepositoryAddressClick(object sender, EventArgs e)
        {
            Process.Start(RepositoryAddressStartInfo);
        }

        // Button事件处理函数
        private void BtnNewProjectClick(object sender, EventArgs e)
        {
            string new_path = $"{AbsoluteProjectDirectoryPath}{DateTime.Now.ToString(ProjectNameFormatStr)}";
            Directory.CreateDirectory(new_path);

            MutSync.WriteAllText($"{new_path}\\{Global.ProjectGaocheng}",
                                 $"awa\nQAQ\nTAT\nOvO\n0\n0\n0",
                                 Encoding.UTF8);

            RefreshProjectList();
            cboProjectSelector.SelectedIndex = 1;

            btnNewProject.Enabled = false;
            tmrWarrantyOfProjectUniqueness.Enabled = true;
        }

        private void BtnDeleteProjectClick(object sender, EventArgs e)
        {
            if (MutSync.CheckOperation($"注意：本操作为永久删除，无法撤销\n是否要删除项目：{projectDirName}",
                                       MessageBoxIcon.Warning))
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

            if (!OwnNewOrEditTestDataForm.Visible)
            {
                DisableComponentWhileEditing();
                tempContainerForResultViewer = rtxResultViewer.Rtf;

                rtxResultViewer.Text = NewOrEditTestDataFormOpenTipStr;
                rtxResultViewer.SelectAll();
                rtxResultViewer.SelectionColor = TipColor;
                rtxResultViewer.DeselectAll();

                OwnNewOrEditTestDataForm.LoadTestDataContent();
            }

            MutSync.BringToFrontAndFocus(OwnNewOrEditTestDataForm);
        }

        private void BtnRetestClick(object sender, EventArgs e)
        {
            if (IsHaveCommonError())
            {
                return;
            }

            if (!File.Exists(absoluteTestDataPath))
            {
                MutSync.ShowMessageToWarn("测试数据文件未生成\n请先完成创建/修改测试数据");
            }
            else if (dataHash != MutSync.MD5Hash(absoluteTestDataPath))
            {
                MutSync.ShowMessageToWarn("测试数据文件被更改\n请进入 创建/修改测试数据 读取该文件并再次生成以保证合法");
            }
            else if (!isPathChanged ||
                     MutSync.CheckOperation("官方demo路径或作业exe路径已变更\n请确认本项目的测试数据适用于对应的exe\n如需继续测试，请按确认"))
            {
                EditProjectGaochengWhileNecessary();
                GenerateAndCompare();
            }
        }

        // ComboBox事件处理函数
        private void CboProjectSelectorSelectedIndexChanged(object sender, EventArgs e)
        {
            DisposeProjectGaochengLock();
            chkIsInterfaceProgramPause.Checked = false;

            if ((projectDirName = cboProjectSelector.SelectedItem.ToString()) == BlankItemStr)
            {
                DisableComponent();
            }
            else
            {
                absoluteDirPath = $"{AbsoluteProjectDirectoryPath}{projectDirName}";
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
