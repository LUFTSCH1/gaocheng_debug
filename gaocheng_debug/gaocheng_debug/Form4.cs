using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace gaocheng_debug
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private string GetMD5HashFromFile(in string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
 
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
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
                string txt = "文件所在目录：" + Path.GetDirectoryName(path) + Environment.NewLine;
                txt += "文件名：" + Path.GetFileName(path) + Environment.NewLine;
                txt += "MD5：" + GetMD5HashFromFile(path);
                textBox1.Text = txt;
            }
        }
    }
}
