using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DailyClickCount
{
    internal static class Program
    {
        static Mutex mutex;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 创建一个命名互斥体
            bool createdNew;
            mutex = new Mutex(true, "DailyMouseCounter", out createdNew);

            // 检查互斥体是否已经存在，如果存在则表示已经有一个实例在运行
            if (!createdNew)
            {
                MessageBox.Show("程序已经在运行中。");
                return;
            }

            // 获取程序的路径
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;

            // 设置程序在启动时自动运行
            SetStartup(appPath);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
            // 释放互斥体
            mutex.ReleaseMutex();
            mutex.Close();
        }
        static void SetStartup(string appPath)
        {
            // 打开注册表项
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true))
            {
                // 添加程序到启动项
                key.SetValue("ClickCount", appPath);
            }
        }
    }

}
