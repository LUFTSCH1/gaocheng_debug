using System;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class MainForm : Form
    {
        // 私有工具函数
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
        }

        private void RefreshFileList()
        {
            cboProjectSelector.Items.Clear();
            cboProjectSelector.Items.Add(ConstValues.BlankItemStr);

            string[] directories = Directory.GetDirectories(ConstValues.TestLogRelativePath);
            int len = 0, i = 0, arr_len = directories.Length;
            for (string temp; i < arr_len; ++i)
            {
                temp = Path.GetFileName(directories[i]);
                if (MutSync.CheckProjectNameIegitimacy(temp))
                {
                    directories[len++] = temp;
                }
            }
            Array.Sort(directories, 0, len, ConstValues.CMP);
            for (i = 0; i < len; ++i)
            {
                cboProjectSelector.Items.Add(directories[i]);
            }
        }

        private bool ExeErrorFilter()
        {
            bool is_demo_exist = File.Exists(txtDemoExePath.Text);
            bool is_exe_exist = File.Exists(txtYourExePath.Text);
            
            if (is_demo_exist && is_exe_exist) 
            {
                return true;
            }
            else
            {
                if (!is_demo_exist && !is_exe_exist)
                {
                    MutSync.ShowMessageToWarn("demo和作业exe文件均不存在");
                }
                else if (!is_demo_exist)
                {
                    MutSync.ShowMessageToWarn("demo文件不存在");
                }
                else
                {
                    MutSync.ShowMessageToWarn("作业exe文件不存在");
                }

                return false;
            }
        }

        private bool IsHaveCommonError()
        {
            if (txtDemoExePath.Text == ConstValues.DemoExePathTxtDefaultStr || txtYourExePath.Text == ConstValues.YourExePathTxtDefaultStr)
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
                return MessageBox.Show("官方demo路径和作业exe路径相同\n你真的要测试同一程序对同一数据的两次输出吗？\n如需继续测试，请按确认", "操作确认", MessageBoxButtons.YesNo, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2) != DialogResult.Yes;
            }
            else
            {
                return false;
            }
        }

        private void PrintResultInfo(in string resultFile)
        {
            rtxResultViewer.Text = $"{ConstValues.CompareResult}文件创建/修改时间：{File.GetLastWriteTime(resultFile):yyyy/MM/dd HH:mm:ss.ffff}{ConstValues.NewLine}{File.ReadAllText(resultFile, ConstValues.GB18030)}";
        }

        private void TryToGetCompareResult()
        {
            if (File.Exists(absoluteDirPath + ConstValues.CompareResultFileName))
            {
                PrintResultInfo(absoluteDirPath + ConstValues.CompareResultFileName);
            }
            else
            {
                MutSync.ShowMessageToWarn($"项目{projectDirName}的\n{ConstValues.CompareResult}\n文件不存在");
                rtxResultViewer.Text = $"项目 {projectDirName} 的{ConstValues.ResultTxtNotExistExceptionStr}";
            }
        }

        private void TryToGetPathLogInfo()
        {
            if (File.Exists(absoluteDirPath + ConstValues.PathLogFileName))
            {
                string[] path_info = File.ReadAllLines(absoluteDirPath + ConstValues.PathLogFileName);
                if (path_info.Length == ConstValues.PathLogLines && DateTime.TryParseExact(path_info[0], ConstValues.ProjectCheckTimeFormat, ConstValues.InvariantCulture, DateTimeStyles.None, out logAndBatTimeChecker) && logAndBatTimeChecker == File.GetLastWriteTime(absoluteDirPath + ConstValues.PathLogFileName))
                {
                    dataGroupNum = Convert.ToInt32(path_info[6]);
                    cboTrimSelector.SelectedIndex = trimMode = Convert.ToInt32(path_info[7]);
                    cboDisplaySelector.SelectedIndex = displayMode = Convert.ToInt32(path_info[8]);

                    if (path_info[1] == ConstValues.ProjectGeneratedFlagStr)
                    {
                        txtDemoExePath.Text = projectDemoPath = path_info[2];
                        txtYourExePath.Text = projectExePath = path_info[3];
                        projectLatestTestPath = path_info[4];
                        dataHash = path_info[5];

                        TryToGetCompareResult();
                    }
                    else
                    {
                        rtxResultViewer.Text = ConstValues.NewProjectStrSet[ConstValues.Rnd.Next(0, ConstValues.NewProjectStrSet.Length)];
                        txtDemoExePath.Text = ConstValues.DemoExePathTxtDefaultStr;
                        txtYourExePath.Text = ConstValues.YourExePathTxtDefaultStr;
                    }

                    if (!btnNewOrEditTestData.Enabled)
                    {
                        EnableComponent();
                    }

                    return;
                }
            }

            DisableComponent();
            MutSync.ShowMessageToWarn($"项目{projectDirName}的\n{ConstValues.PathLog}\n文件不存在、不合法或被篡改");
            rtxResultViewer.Text = $"项目 {projectDirName} 的{ConstValues.PathLogExceptionStr}";
            btnDeleteProject.Enabled = true;
            btnOpenProjectDirectory.Enabled = true;
        }

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
                if (projectDemoPath != txtDemoExePath.Text)
                {
                    projectDemoPath = txtDemoExePath.Text;
                }
                if (projectExePath != txtYourExePath.Text)
                {
                    projectExePath = txtYourExePath.Text;
                }
                isPathChanged = false;
            }
            if (projectLatestTestPath != absoluteDirPath)
            {
                projectLatestTestPath = absoluteDirPath;
            }

            logAndBatTimeChecker = DateTime.Now;

            File.WriteAllText(absoluteDirPath + ConstValues.PathLogFileName, $"{logAndBatTimeChecker.ToString(ConstValues.ProjectCheckTimeFormat)}\n{ConstValues.ProjectGeneratedFlagStr}\n{projectDemoPath}\n{projectExePath}\n{absoluteDirPath}\n{dataHash}\n{dataGroupNum}\n{cboTrimSelector.SelectedIndex}\n{cboDisplaySelector.SelectedIndex}");
            File.SetLastWriteTime(absoluteDirPath + ConstValues.PathLogFileName, logAndBatTimeChecker);
            
            string content = $"cd /d \"{absoluteDirPath}\"{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\{ConstValues.GetInputData} {ConstValues.TestData} [1] | \"{txtDemoExePath.Text}\" 1>{ConstValues.DemoExeResult}{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\{ConstValues.GetInputData} {ConstValues.TestData} [1] | \"{txtYourExePath.Text}\" 1>{ConstValues.YourExeResult}{ConstValues.NewLine}";
            content += $"for /l %%v in (2, 1, {dataGroupNum}) do ({ConstValues.NewLine}";
            content += $"..\\..\\rsc\\{ConstValues.GetInputData} {ConstValues.TestData} [%%v] | \"{txtDemoExePath.Text}\" 1>>{ConstValues.DemoExeResult}{ConstValues.NewLine}";
            content += $"..\\..\\rsc\\{ConstValues.GetInputData} {ConstValues.TestData} [%%v] | \"{txtYourExePath.Text}\" 1>>{ConstValues.YourExeResult}{ConstValues.NewLine}){ConstValues.NewLine}";
            content += $"..\\..\\rsc\\{ConstValues.TxtCompare} --file1 {ConstValues.DemoExeResult} --file2 {ConstValues.YourExeResult} ";
            content += cboTrimSelector.SelectedItem.ToString();
            content += cboDisplaySelector.SelectedItem.ToString();
            content += $"1>{ConstValues.CompareResult} 2>&1";
            File.WriteAllText(absoluteDirPath + ConstValues.TestBatFileName, content, ConstValues.GB18030);
            File.SetLastWriteTime(absoluteDirPath + ConstValues.TestBatFileName, logAndBatTimeChecker);
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
            string compare_result_path = absoluteDirPath + ConstValues.CompareResultFileName;

            if (File.Exists(compare_result_path))
            {
                File.Delete(compare_result_path);
            }

            CMD.Start();
            CMD.StandardInput.WriteLine($"\"{absoluteDirPath}{ConstValues.TestBatFileName}\"");
            CMD.StandardInput.WriteLine("cls&exit");
            CMD.WaitForExit();
            CMD.Close();

            if (File.Exists(compare_result_path))
            {
                PrintResultInfo(compare_result_path);
            }
            else
            {
                MutSync.ShowMessageToWarn("cmd运行过程发生错误或被中断，生成失败");
                rtxResultViewer.Text = ConstValues.TestProcessExceptionStr;
            }
        }
    }
}
