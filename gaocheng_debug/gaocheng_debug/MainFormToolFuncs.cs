using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        public void ChangeComponentEnabled()
        {
            tsmiSettings.Enabled = !tsmiSettings.Enabled;
            cboProjectSelector.Enabled = !cboProjectSelector.Enabled;
            // 由于timer延迟修改，btnNewProject.Enabled不确定，须手动更改为一个确定值
            btnDeleteProject.Enabled = !btnDeleteProject.Enabled;
            btnOpenProjectDirectory.Enabled = !btnOpenProjectDirectory.Enabled;
            btnBrowseDemoExe.Enabled = !btnBrowseDemoExe.Enabled;
            btnBrowseYourExe.Enabled = !btnBrowseYourExe.Enabled;
            // 无需改动 btnNewOrEditTestData.Enabled，已经做好判断
            btnRetest.Enabled = !btnRetest.Enabled;
        }

        public void ConstructAndTest()
        {
            dataHash = MutSync.PartHashWithSalt(absoluteDirPath + Global.TestDataFileName);
            ForceToEditLogAndBat();
            GenerateAndCompare();
        }
        
        public void RecoverResultViewerText() => rtxResultViewer.Text = tempContainerForResultViewer;

        public void FocusBackToMain()
        {
            if (Visible)
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Normal;
                }
                BringToFront();
                Focus();
            }
            else
            {
                Show();
            }
        }

        // 私有工具函数
        private bool CheckOperation(in string msg, in MessageBoxIcon icon) => MessageBox.Show(msg, "操作确认", MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

        private void LockProjectGaocheng() => projectGaochengLock = new FileStream(absoluteDirPath + Global.ProjectGaochengFileName, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Read);

        private void DisposeProjectGaochengLock()
        {
            if (projectGaochengLock != null)
            {
                projectGaochengLock.Close();
                projectGaochengLock = null;
            }
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
            chkIsCMDPause.Enabled = false;
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
            chkIsCMDPause.Enabled = true;
        }

        private void RefreshProjectList()
        {
            DateTime now = DateTime.Now;

            cboProjectSelector.Items.Clear();
            cboProjectSelector.Items.Add(BlankItemStr);

            string[] directories = Directory.GetDirectories(Global.ProjectDirectoryRelativePath);
            int len = 0, i = 0, arr_len = directories.Length;
            for (string temp; i < arr_len; ++i)
            {
                temp = Path.GetFileName(directories[i]);
                if (TryToConvertDateTime(temp, ProjectNameFormatStr) && now >= timeChecker)
                {
                    directories[len++] = temp;
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
                return !CheckOperation("官方demo路径和作业exe路径相同\n你真的要测试同一程序对同一数据的两次输出吗？\n如需继续测试，请按确认", MessageBoxIcon.Information);
            }
            else
            {
                return false;
            }
        }

        private void PrintResultInfo(in string resultFile) => rtxResultViewer.Text = $"{Global.CompareResult}文件创建/修改时间：{File.GetLastWriteTime(resultFile).ToString(Global.OperationTimeFormatStr)}{Global.NewLine}{File.ReadAllText(resultFile, Global.GB18030)}";

        // 注意：本函数会修改timeChecker
        private bool TryToConvertDateTime(in string timeString, in string timeFormat) => DateTime.TryParseExact(timeString, timeFormat, InvariantCulture, DateTimeStyles.None, out timeChecker);

        private void TryToGetProjectGaochengInfo()
        {
            if (File.Exists(absoluteDirPath + Global.ProjectGaochengFileName))
            {
                string[] project_info = File.ReadAllLines(absoluteDirPath + Global.ProjectGaochengFileName);
                if (project_info.Length == ProjectGaochengLines && TryToConvertDateTime(project_info[0], ProjectCheckTimeFormatStr) && timeChecker == File.GetLastWriteTime(absoluteDirPath + Global.ProjectGaochengFileName))
                {
                    // 锁
                    LockProjectGaocheng();

                    dataGroupNum = Convert.ToInt32(project_info[6]);
                    cboTrimSelector.SelectedIndex = trimMode = Convert.ToInt32(project_info[7]);
                    cboDisplaySelector.SelectedIndex = displayMode = Convert.ToInt32(project_info[8]);

                    if (project_info[1] == ProjectGeneratedFlagStr)
                    {
                        txtDemoExePath.Text = projectDemoExePath = project_info[2];
                        txtYourExePath.Text = projectYourExePath = project_info[3];
                        projectLatestTestPath = project_info[4];
                        dataHash = project_info[5];

                        if (File.Exists(absoluteDirPath + Global.CompareResultFileName))
                        {
                            PrintResultInfo(absoluteDirPath + Global.CompareResultFileName);
                        }
                        else
                        {
                            rtxResultViewer.Text = $"运行过测试的项目 {projectDirName} {ResultTxtNotExistExceptionStr}";
                        }
                    }
                    else
                    {
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
            MutSync.ShowMessageToWarn($"项目{projectDirName}的\n{Global.ProjectGaocheng}\n文件不存在、不合法或被篡改");
            rtxResultViewer.Text = $"项目 {projectDirName} 的{ProjectGaochengExceptionStr}";
            btnDeleteProject.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
        }

        private string ConstructTxtCompareCmd() => $"..\\..\\{Global.ResourceDirectory}\\{Global.TxtCompare} --file1 {Global.DemoExeResult} --file2 {Global.YourExeResult} {cboTrimSelector.SelectedItem} {cboDisplaySelector.SelectedItem}";

        private void ForceToEditLogAndBat()
        {
            if (isModeChanged)
            {
                trimMode = cboTrimSelector.SelectedIndex;
                displayMode = cboDisplaySelector.SelectedIndex;
                isModeChanged = false;
            }
            if (isPathChanged)
            {
                if (projectDemoExePath != txtDemoExePath.Text)
                {
                    projectDemoExePath = txtDemoExePath.Text;
                }
                if (projectYourExePath != txtYourExePath.Text)
                {
                    projectYourExePath = txtYourExePath.Text;
                }
                isPathChanged = false;
            }
            if (projectLatestTestPath != absoluteDirPath)
            {
                projectLatestTestPath = absoluteDirPath;
            }

            timeChecker = DateTime.Now;

            DisposeProjectGaochengLock();
            File.WriteAllText(absoluteDirPath + Global.ProjectGaochengFileName, $"{timeChecker.ToString(ProjectCheckTimeFormatStr)}\n{ProjectGeneratedFlagStr}\n{projectDemoExePath}\n{projectYourExePath}\n{absoluteDirPath}\n{dataHash}\n{dataGroupNum}\n{cboTrimSelector.SelectedIndex}\n{cboDisplaySelector.SelectedIndex}");
            File.SetLastWriteTime(absoluteDirPath + Global.ProjectGaochengFileName, timeChecker);
            LockProjectGaocheng();
            
            string content = $"cd /d \"{absoluteDirPath}\"{Global.NewLine}";
            content += $"..\\..\\{Global.ResourceDirectory}\\{Global.GetInputData} {Global.TestData} [1] | \"{txtDemoExePath.Text}\" 1>{Global.DemoExeResult}{Global.NewLine}";
            content += $"..\\..\\{Global.ResourceDirectory}\\{Global.GetInputData} {Global.TestData} [1] | \"{txtYourExePath.Text}\" 1>{Global.YourExeResult}{Global.NewLine}";
            content += $"for /l %%v in (2, 1, {dataGroupNum}) do ({Global.NewLine}";
            content += $"..\\..\\{Global.ResourceDirectory}\\{Global.GetInputData} {Global.TestData} [%%v] | \"{txtDemoExePath.Text}\" 1>>{Global.DemoExeResult}{Global.NewLine}";
            content += $"..\\..\\{Global.ResourceDirectory}\\{Global.GetInputData} {Global.TestData} [%%v] | \"{txtYourExePath.Text}\" 1>>{Global.YourExeResult}{Global.NewLine}){Global.NewLine}";
            content += $"{ConstructTxtCompareCmd()} 1>{Global.CompareResult} 2>&1";
            File.WriteAllText(absoluteDirPath + Global.TestBatFileName, content, Global.GB18030);
            File.SetLastWriteTime(absoluteDirPath + Global.TestBatFileName, timeChecker);
        }

        private void EditLogAndBatWhileModeOrPathChanged()
        {
            if (isModeChanged || isPathChanged || projectLatestTestPath != absoluteDirPath)
            {
                ForceToEditLogAndBat();
            }
        }

        private void GenerateAndCompare()
        {
            FileStream demo_exe_lock = MutSync.NewReadOnlyFileHandle(txtDemoExePath.Text);
            FileStream your_exe_lock = MutSync.NewReadOnlyFileHandle(txtYourExePath.Text);

            if (OwnMD5CalculatorForm.Visible)
            {
                OwnMD5CalculatorForm.Hide();
            }
            Hide();

            string compare_result_path = absoluteDirPath + Global.CompareResultFileName;

            if (File.Exists(compare_result_path))
            {
                File.Delete(compare_result_path);
            }

            CMD.Start();
            CMD.StandardInput.WriteLine($"\"{absoluteDirPath}{Global.TestBatFileName}\"");
            if (chkIsCMDPause.Checked)
            {
                CMD.StandardInput.WriteLine("cls");
                CMD.StandardInput.WriteLine(ConstructTxtCompareCmd());
            }
            else
            {
                CMD.StandardInput.WriteLine("cls&exit");
            }
            CMD.WaitForExit();
            CMD.Close();

            demo_exe_lock.Close();
            your_exe_lock.Close();

            if (File.Exists(compare_result_path))
            {
                PrintResultInfo(compare_result_path);
            }
            else
            {
                rtxResultViewer.Text = TestProcessExceptionStr + DateTime.Now.ToString(Global.OperationTimeFormatStr);
            }

            FocusBackToMain();
        }
    }
}
