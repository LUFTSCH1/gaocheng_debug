using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        public void DoWhileEditCanceled()
        {
            rtxResultViewer.Text = tempContainerForResultViewer;
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
        private bool CheckOperation(in string msg, in MessageBoxIcon icon) => MessageBox.Show(msg, "操作确认", MessageBoxButtons.YesNo, icon, MessageBoxDefaultButton.Button2) == DialogResult.Yes;

        private void LockProjectGaocheng() => projectGaochengLock = MutSync.NewReadOnlyFileHandle(absoluteProjectGaochengPath);

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
            for (string temp; i < arr_len; ++i)
            {
                temp = Path.GetFileName(directories[i]);
                if (DateTime.TryParseExact(temp, ProjectNameFormatStr, InvariantCulture, DateTimeStyles.None, out prj_time) && now >= prj_time)
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

        private void TryToGetProjectGaochengInfo()
        {
            absoluteProjectGaochengPath = $"{absoluteDirPath}\\{Global.ProjectGaocheng}";
            if (File.Exists(absoluteProjectGaochengPath))
            {
                string[] project_info = File.ReadAllLines(absoluteProjectGaochengPath);
                if (project_info.Length == Global.ProjectGaochengLines)
                {
                    LockProjectGaocheng();

                    absoluteTestDataPath      = $"{absoluteDirPath}\\{Global.TestData}";
                    absoluteCompareResultPath = $"{absoluteDirPath}\\{Global.CompareResult}";

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
                            rtxResultViewer.Text = $"运行过测试的项目 {projectDirName} {ResultTxtNotExistExceptionStr}";
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
            MutSync.ShowMessageToWarn($"项目{projectDirName}的\n{Global.ProjectGaocheng}\n文件不存在、不合法或被篡改");
            rtxResultViewer.Text = $"项目 {projectDirName} 的{ProjectGaochengExceptionStr}";
            btnDeleteProject.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
        }

        private void OpenCmdAndTest()
        {
            string compare_cmd = $"{AbsoluteTxtComparePath} --file1 {Global.DemoExeResult} --file2 {Global.YourExeResult} {cboTrimSelector.SelectedItem} {cboDisplaySelector.SelectedItem}";

            InterfaceProgram.Start();

            InterfaceProgram.StandardInput.WriteLine(
                  $"cd /d \"{absoluteDirPath}\"\n"
                + $"{AbsoluteGetInputDataPath} {Global.TestData} [1] | \"{txtDemoExePath.Text}\" 1>{Global.DemoExeResult}\n"
                + $"{AbsoluteGetInputDataPath} {Global.TestData} [1] | \"{txtYourExePath.Text}\" 1>{Global.YourExeResult}\n"
                + $"for /l %v in (2, 1, {dataGroupNum}) do "
                + $"{AbsoluteGetInputDataPath} {Global.TestData} [%v] | \"{txtDemoExePath.Text}\" 1>>{Global.DemoExeResult} & "
                + $"{AbsoluteGetInputDataPath} {Global.TestData} [%v] | \"{txtYourExePath.Text}\" 1>>{Global.YourExeResult}\n"
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
            File.WriteAllText(absoluteProjectGaochengPath, $"{MutSync.MD5HashWithSalt(prj_info)}\n{prj_info}");
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
            FileStream demo_exe_lock = MutSync.NewReadOnlyFileHandle(txtDemoExePath.Text);
            FileStream your_exe_lock = MutSync.NewReadOnlyFileHandle(txtYourExePath.Text);

            if (OwnMD5CalculatorForm.Visible)
            {
                OwnMD5CalculatorForm.Hide();
            }
            Hide();

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
                rtxResultViewer.Text = $"{TestProcessExceptionStr}{DateTime.Now.ToString(Global.OperationTimeFormatStr)}";
            }

            MutSync.BringToFrontAndFocus(this);
        }
    }
}
