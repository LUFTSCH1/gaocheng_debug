using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form4 : Form
    {
        private readonly string NewLine = Environment.NewLine;

        public Form4()
        {
            InitializeComponent();
        }

        private string GetMD5HashFromFile(in string fileName)
        {
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
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            BackColor = Color.FromArgb(234, 234, 239);
            button1.BackColor = Color.FromArgb(93, 190, 138);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string path = openFileDialog1.FileName;

                textBox1.Text = string.Format("文件：{0}{1}计算中...", path, NewLine);
                Application.DoEvents();

                string content = "目录  ：" + Path.GetDirectoryName(path) + NewLine;
                content += "文件名：" + Path.GetFileName(path) + NewLine;
                content += "MD5   ：" + GetMD5HashFromFile(path);
                textBox1.Text = content;
            }
        }
    }
}
