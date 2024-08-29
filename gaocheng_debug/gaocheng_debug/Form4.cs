using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace gaocheng_debug
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private async Task<string> GetMD5HashFromFile(string fileName)
        {
            return await Task.Run(() => { 
                try
                {
                    System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                    FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    byte[] retVal = md5.ComputeHash(file);
                    file.Close();
 
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0, len = retVal.Length; i < len; ++i)
                    {
                        sb.Append(retVal[i].ToString("x2"));
                    }
                
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception("获取文件MD5失败：" + ex.Message);
                }
            }).ConfigureAwait(true);
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;
                textBox1.Text = $"文件：{path}{ConstValues.NewLine}计算中...";
                textBox1.Text = $"目录  ：{Path.GetDirectoryName(path)}{ConstValues.NewLine}文件名：{Path.GetFileName(path)}{ConstValues.NewLine}MD5   ：{await GetMD5HashFromFile(path)}";
            }
        }
    }
}
