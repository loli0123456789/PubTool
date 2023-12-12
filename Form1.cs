using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PubTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //将程序直接放到发布路径下，自动识别
            string appPath = Application.StartupPath;
            tbFrom.Text = appPath;

            DirectoryInfo directory = new DirectoryInfo(appPath);
            string appPathName = directory.Name;

            string parentPath = Directory.GetParent(appPath).FullName;
            //在父级路径下建立当前日期的子文件夹，用于拷贝文件
            string newPath = Path.Combine(parentPath, $"{DateTime.Now.ToString("yyyyMMdd")}_{appPathName}");
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }
            tbTo.Text = newPath;
        }



        private void button2_Click(object sender, EventArgs e)
        {
            string fromPath = tbFrom.Text;
            if (string.IsNullOrEmpty(fromPath))
            {
                MessageBox.Show("请选择发布路径");

                return;
            }
            if (!Directory.Exists(fromPath))
            {
                MessageBox.Show("发布路径不存在");

                return;
            }

            string toPath = tbTo.Text;
            if (string.IsNullOrEmpty(toPath))
            {
                MessageBox.Show("请选择提取路径");

                return;
            }
            if (!Directory.Exists(fromPath))
            {
                MessageBox.Show("提取路径不存在");

                return;
            }

            try
            {
                GetAndCopyFile(fromPath, fromPath, toPath);

                MessageBox.Show("获取并复制文件成功！");
            }
            catch (Exception ex)
            {
                throw new Exception("获取并复制文件出错", ex);
            }
        }

        protected void GetAndCopyFile(string rootPath, string fromPath, string toPath)
        {
            string[] paths = Directory.GetDirectories(fromPath);
            //递归遍历文件夹，去找文件
            foreach (string path in paths)
            {
                GetAndCopyFile(rootPath, path, toPath);
            }

            //找文件夹下的文件
            string[] filePaths = Directory.GetFiles(fromPath);
            //遍历文件，进行拷贝
            foreach (string filePath in filePaths)
            {
                FileInfo file = new FileInfo(filePath);
                //当天修改文件，进行拷贝
                if (file.LastWriteTime.ToShortDateString() == DateTime.Now.ToShortDateString())
                {
                    string fromFilePath = file.FullName;
                    string newFIlePath = fromFilePath.Replace(rootPath, toPath);
                    FileInfo newFile = new FileInfo(newFIlePath);
                    if (!newFile.Directory.Exists)
                    {
                        newFile.Directory.Create();
                    }
                    file.CopyTo(newFIlePath, true);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            fileFrom.ShowDialog();

            tbFrom.Text = fileFrom.SelectedPath;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            fileFrom.ShowDialog();

            tbTo.Text = fileFrom.SelectedPath;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string flag = "出版,";
            int dex = flag.IndexOf(null);
            MessageBox.Show(dex.ToString());
        }
    }
}
