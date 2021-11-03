using Painter.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter
{
    //统计项目行号
    public partial class FileTravaselForm : Form
    {
        public FileTravaselForm()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string filePath="";
            if (!FileUtils.SelectFilePath(ref filePath))
            {
                return;
            }
            string FileName = "this.txt";
            if (File.Exists(FileName))
            {
                File.Delete(FileName);
            }
            Func<FileInfo, bool> func = (file) =>
               {
                   if (file.Extension == ".cs"||file.Extension == ".xaml")
                   {
                       return true;
                   }
                   return false;
               };
            Action<FileInfo> action = (file) => {
                Debug.Print(file.FullName + "\n");
                using (FileStream fs=new FileStream(FileName,FileMode.Append))
                {
                    byte[] buffer = Encoding.UTF8.GetBytes(File.ReadAllText(file.FullName));
                    fs.Write(buffer, 0, buffer.Length);
                } 
            };
            FileUtils.FindFiles(filePath, func, action);
            MessageBox.Show("this.txt 文件生成完成");
        }
    }
}
