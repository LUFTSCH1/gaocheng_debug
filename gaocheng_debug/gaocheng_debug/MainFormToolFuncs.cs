using System;
using System.IO;
using System.Text;
using System.Globalization;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        public void DoWhileEditCanceled()
        {
            rtxResultViewer.Rtf = tempContainerForResultViewer;
            EnableComponentAfterEdit();
            MutSync.BringToFrontAndFocus(this);
        }

        public void DoWhileEdited(in int groupNum)
        {
            dataGroupNum = groupNum;
            dataHash = MutSync.MD5Hash(absoluteTestDataPath);
            EditProjectGaocheng();
            GenerateAndCompare();
            EnableComponentAfterEdit();
        }

        // 私有工具函数
        private void LockProjectGaocheng() =>
            projectGaochengLock = MutSync.NewReadOnlyFileHandle(absoluteProjectGaochengPath);

        private void DisposeProjectGaochengLock()
        {
            if (projectGaochengLock != null)
            {
                projectGaochengLock.Close();
                projectGaochengLock = null;
            }
        }

        private void DisableComponentWhileEditing()
        {
            btnNewProject.Enabled = false;
            tsmiSettings.Enabled = false;
            cboProjectSelector.Enabled = false;
            btnDeleteProject.Enabled = false;
            btnOpenProjectDirectory.Enabled = false;
            btnBrowseDemoExe.Enabled = false;
            btnBrowseYourExe.Enabled = false;
            btnRetest.Enabled = false;
        }

        private void EnableComponentAfterEdit()
        {
            tsmiSettings.Enabled = true;
            cboProjectSelector.Enabled = true;
            btnDeleteProject.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
            btnBrowseDemoExe.Enabled = true;
            btnBrowseYourExe.Enabled = true;
            btnRetest.Enabled = true;
            btnNewProject.Enabled = true;
        }

        private void DisableComponent()
        {
            btnBrowseDemoExe.Enabled = false;
            btnBrowseYourExe.Enabled = false;
            btnRetest.Enabled = false;
            btnNewOrEditTestData.Enabled = false;
            btnOpenProjectDirectory.Enabled = false;
            btnDeleteProject.Enabled = false;
            txtDemoExePath.Text = string.Empty;
            txtYourExePath.Text = string.Empty;
            rtxResultViewer.Text = string.Empty;
            cboTrimSelector.SelectedIndex = 0;
            cboDisplaySelector.SelectedIndex = 0;
            cboTrimSelector.Enabled = false;
            cboDisplaySelector.Enabled = false;
            chkIsInterfaceProgramPause.Enabled = false;
        }

        private void EnableComponent()
        {
            btnBrowseDemoExe.Enabled = true;
            btnBrowseYourExe.Enabled = true;
            btnRetest.Enabled = true;
            btnNewOrEditTestData.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
            btnDeleteProject.Enabled = true;
            cboTrimSelector.Enabled = true;
            cboDisplaySelector.Enabled = true;
            chkIsInterfaceProgramPause.Enabled = true;
        }

        private void RefreshProjectList()
        {
            DateTime now = DateTime.Now;
            DateTime prj_time;

            cboProjectSelector.Items.Clear();
            cboProjectSelector.Items.Add(BlankItemStr);

            string[] directories = Directory.GetDirectories(Global.ProjectDirectoryRelativePath);
            int len = 0, i = 0, arr_len = directories.Length;
            for (string[] temp; i < arr_len; ++i)
            {
                string dir = Path.GetFileName(directories[i]);
                temp = dir.Split(ProjectSplitChar);
                if (temp.Length == 2 &&
                    Regex.IsMatch(temp[0], ProjectPattern) &&
                    DateTime.TryParseExact(temp[1],
                                           ProjectNameFormatStr,
                                           InvariantCulture,
                                           DateTimeStyles.None,
                                           out prj_time) &&
                    now >= prj_time)
                {
                    directories[len++] = dir;
                }
            }
            Array.Sort(directories, 0, len, CMP);
            for (i = 0; i < len; ++i)
            {
                cboProjectSelector.Items.Add(directories[i]);
            }
        }

        private bool ExeErrorFilter()
        {
            bool is_demo_exe_exist = File.Exists(txtDemoExePath.Text);
            bool is_your_exe_exist = File.Exists(txtYourExePath.Text);
            
            if (is_demo_exe_exist && is_your_exe_exist) 
            {
                return true;
            }
            else
            {
                if (!is_demo_exe_exist && !is_your_exe_exist)
                {
                    MutSync.ShowMessageToWarn("demo和作业exe文件均不存在");
                }
                else if (is_demo_exe_exist)
                {
                    MutSync.ShowMessageToWarn("作业exe文件不存在");
                }
                else
                {
                    MutSync.ShowMessageToWarn("demo文件不存在");
                }

                return false;
            }
        }

        private bool IsHaveCommonError()
        {
            if (txtDemoExePath.Text == DemoExePathTxtDefaultStr || txtYourExePath.Text == YourExePathTxtDefaultStr)
            {
                MutSync.ShowMessageToWarn("官方demo或作业exe路径为空");
                return true;
            }
            else if (!ExeErrorFilter())
            {
                return true;
            }
            else if (txtDemoExePath.Text == txtYourExePath.Text)
            {
                return !MutSync.CheckOperation("官方demo路径和作业exe路径相同\n你真的要测试同一程序对同一数据的两次输出吗？\n如需继续测试，请按确认");
            }
            else
            {
                return false;
            }
        }

        private void CheckProjectFileAccess()
        {
            while (true)
            {
                try
                {
                    new FileStream(absoluteTestDataPath, FileMode.Open, FileAccess.Read, FileShare.Read).Close();
                    if (File.Exists(absoluteCompareResultPath))
                    {
                        new FileStream(absoluteCompareResultPath, FileMode.Open, FileAccess.Write).Close();
                    }
                    if (File.Exists(absoluteDemoExeResultPath))
                    {
                        new FileStream(absoluteDemoExeResultPath, FileMode.Open, FileAccess.Write).Close();
                    }
                    if (File.Exists(absoluteYourExeResultPath))
                    {
                        new FileStream(absoluteYourExeResultPath, FileMode.Open, FileAccess.Write).Close();
                    }
                    return;
                }
                catch (Exception ex)
                {
                    if (!MutSync.CheckOperation($"项目 {projectDirName} 中文件测试需要的权限不满足，是否重试？\n错误信息：{ex.Message}",
                                                MessageBoxIcon.Error,
                                                Global.ErrorTitle,
                                                MessageBoxDefaultButton.Button1))
                    {
                        Environment.Exit((int)ErrorCode.FileAccessError);
                    }
                }
            }
        }

        private void PrintResultInfo(in string resultFile)
        {
            string time_info = $"{Global.CompareResult}文件创建/修改时间：{File.GetLastWriteTime(resultFile).ToString(Global.OperationTimeFormatStr)}";
            rtxResultViewer.Text = $"{time_info}{Global.NewLine}{MutSync.ReadAllText(resultFile, Global.GB18030)}";
            rtxResultViewer.Select(0, time_info.Length);
            rtxResultViewer.SelectionColor = TimeInfoColor;
            rtxResultViewer.DeselectAll();
        }

        private void PrintErrorInfo(in string errorStr)
        {
            rtxResultViewer.Text = errorStr;
            rtxResultViewer.SelectAll();
            rtxResultViewer.SelectionColor = ErrorInfoColor;
            rtxResultViewer.DeselectAll();
        }

        private void TryToGetProjectGaochengInfo()
        {
            absoluteProjectGaochengPath = $"{absoluteDirPath}\\{Global.ProjectGaocheng}";
            if (File.Exists(absoluteProjectGaochengPath))
            {
                string[] project_info = MutSync.ReadAllLines(absoluteProjectGaochengPath);
                if (project_info.Length == Global.ProjectGaochengLines)
                {
                    LockProjectGaocheng();

                    absoluteTestDataPath      = $"{absoluteDirPath}\\{Global.TestData}";
                    absoluteCompareResultPath = $"{absoluteDirPath}\\{Global.CompareResult}";
                    absoluteDemoExeResultPath = $"{absoluteDirPath}\\{Global.DemoExeResult}";
                    absoluteYourExeResultPath = $"{absoluteDirPath}\\{Global.YourExeResult}";

                    if (project_info[0] == MutSync.MD5HashWithSalt($"{project_info[1]}\n{project_info[2]}\n{project_info[3]}\n{project_info[4]}\n{project_info[5]}\n{project_info[6]}"))
                    {
                        txtDemoExePath.Text = projectDemoExePath = project_info[1];
                        txtYourExePath.Text = projectYourExePath = project_info[2];
                        dataHash                                 = project_info[3];
                        dataGroupNum                                   = Convert.ToInt32(project_info[4]);
                        cboTrimSelector.SelectedIndex = trimMode       = Convert.ToInt32(project_info[5]);
                        cboDisplaySelector.SelectedIndex = displayMode = Convert.ToInt32(project_info[6]);

                        if (File.Exists(absoluteCompareResultPath))
                        {
                            PrintResultInfo(absoluteCompareResultPath);
                        }
                        else
                        {
                            PrintErrorInfo($"运行过测试的项目 {projectDirName} {ResultTxtNotExistExceptionStr}");
                        }
                    }
                    else
                    {
                        dataGroupNum                                   = 0;
                        cboTrimSelector.SelectedIndex = trimMode       = 0;
                        cboDisplaySelector.SelectedIndex = displayMode = 0;

                        rtxResultViewer.Text = NewProjectStrSet[RND.Next(0, NewProjectStrSet.Length)];
                        txtDemoExePath.Text = DemoExePathTxtDefaultStr;
                        txtYourExePath.Text = YourExePathTxtDefaultStr;
                    }

                    if (!btnNewOrEditTestData.Enabled)
                    {
                        EnableComponent();
                    }

                    return;
                }
            }

            DisableComponent();
            MutSync.ShowMessageToWarn($"项目{projectDirName}的\n{Global.ProjectGaocheng}\n文件不存在或不合法");
            PrintErrorInfo($"项目 {projectDirName} 的{ProjectGaochengExceptionStr}");
            btnDeleteProject.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
        }

        private void OpenCmdAndTest()
        {
            string compare_cmd = $"{AbsoluteTxtComparePath} --file1 {Global.DemoExeResult} --file2 {Global.YourExeResult} {cboTrimSelector.SelectedItem} {cboDisplaySelector.SelectedItem}";

            while (true)
            {
                try
                {
                    InterfaceProgram.Start();
                    break;
                }
                catch
                {
                    if (!MutSync.CheckOperation("启动CMD失败，是否重试？", MessageBoxIcon.Error, Global.ErrorTitle, MessageBoxDefaultButton.Button1))
                    {
                        return;
                    }
                }
            }

            InterfaceProgram.StandardInput.WriteLine(
                  $"cd /d \"{absoluteDirPath}\"\n"
                + $"{AbsoluteGetInputDataPath} {Global.TestData} 1 | \"{txtDemoExePath.Text}\" 1>{Global.DemoExeResult}\n"
                + $"{AbsoluteGetInputDataPath} {Global.TestData} 1 | \"{txtYourExePath.Text}\" 1>{Global.YourExeResult}\n"
                + $"for /l %v in (2, 1, {dataGroupNum}) do "
                + $"{AbsoluteGetInputDataPath} {Global.TestData} %v | \"{txtDemoExePath.Text}\" 1>>{Global.DemoExeResult} & "
                + $"{AbsoluteGetInputDataPath} {Global.TestData} %v | \"{txtYourExePath.Text}\" 1>>{Global.YourExeResult}\n"
                + $"{compare_cmd} 1>{Global.CompareResult} 2>>&1"
            );
            if (chkIsInterfaceProgramPause.Checked)
            {
                InterfaceProgram.StandardInput.WriteLine("cls");
                InterfaceProgram.StandardInput.WriteLine(compare_cmd);
            }
            else
            {
                InterfaceProgram.StandardInput.WriteLine("exit");
            }
            InterfaceProgram.WaitForExit();

            InterfaceProgram.Close();
        }

        private void EditProjectGaocheng()
        {
            trimMode    = cboTrimSelector.SelectedIndex;
            displayMode = cboDisplaySelector.SelectedIndex;
            projectDemoExePath = txtDemoExePath.Text;
            projectYourExePath = txtYourExePath.Text;
            isModeChanged = false;
            isPathChanged = false;
            string prj_info = $"{projectDemoExePath}\n{projectYourExePath}\n{dataHash}\n{dataGroupNum}\n{cboTrimSelector.SelectedIndex}\n{cboDisplaySelector.SelectedIndex}";
            DisposeProjectGaochengLock();
            MutSync.WriteAllText(absoluteProjectGaochengPath, $"{MutSync.MD5HashWithSalt(prj_info)}\n{prj_info}", Encoding.UTF8);
            LockProjectGaocheng();
        }

        private void EditProjectGaochengWhileNecessary()
        {
            if (isModeChanged || isPathChanged)
            {
                trimMode    = cboTrimSelector.SelectedIndex;
                displayMode = cboDisplaySelector.SelectedIndex;
                projectDemoExePath = txtDemoExePath.Text;
                projectYourExePath = txtYourExePath.Text;
                isModeChanged = false;
                isPathChanged = false;
                EditProjectGaocheng();
            }
        }

        private void GenerateAndCompare()
        {
            CheckProjectFileAccess();
            FileStream demo_exe_lock = MutSync.NewReadOnlyFileHandle(txtDemoExePath.Text);
            FileStream your_exe_lock = MutSync.NewReadOnlyFileHandle(txtYourExePath.Text);

            if (OwnMD5CalculatorForm.Visible)
            {
                OwnMD5CalculatorForm.Hide();
            }
            Hide();
            Enabled = false;

            if (File.Exists(absoluteCompareResultPath))
            {
                File.Delete(absoluteCompareResultPath);
            }
            
            OpenCmdAndTest();
            
            demo_exe_lock.Close();
            your_exe_lock.Close();

            if (File.Exists(absoluteCompareResultPath))
            {
                PrintResultInfo(absoluteCompareResultPath);
            }
            else
            {
                PrintErrorInfo($"{TestProcessExceptionStr}{DateTime.Now.ToString(Global.OperationTimeFormatStr)}");
            }

            Enabled = true;
            MutSync.BringToFrontAndFocus(this);
        }
    }
}
