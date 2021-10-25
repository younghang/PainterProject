﻿using System;
using System.Windows.Forms;

namespace Painter
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Settings.LoadSettingFile();
            Application.Run(new GameJumpFrm());
        }
    }
}