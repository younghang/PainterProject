﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Windows.Threading;

namespace ShowDanmu.FullScreen
{
    /// <summary>
    /// Window penetration
    /// </summary>
    public static class WindowsServices
    {
        const int WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);

        [DllImport("user32.dll")]
        static extern int GetWindowLong(IntPtr hwnd, int index);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            var extendedStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT);
        }
    }
    /// <summary>
    /// Interactive logic of Danmaku.xaml
    /// </summary>
    public partial class DanmakuCurtain : Window
    {
        /// <summary>
        /// Window penetration
        /// </summary>
        #region Code of window penetration
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            WindowsServices.SetWindowExTransparent(hwnd);
        }
        #endregion
        public double screenHeight = SystemParameters.PrimaryScreenHeight;
        public double screenWidth = SystemParameters.PrimaryScreenWidth;

        private bool enableShadowEffect = false;

        private DanmakuManager dm = null;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }


        private DanmakuCurtain(bool enableShadow)
        {
            InitializeComponent();

            Top = 0;
            Left = 0;
            Width = screenWidth;
            Height = screenHeight;

            enableShadowEffect = enableShadow;
        }
        public static DanmakuCurtain Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DanmakuCurtain(false);
                    _instance.Show();
                }
                return _instance;
            }
        }
        private static DanmakuCurtain _instance = null;
        //public void ShootDanmaku(string text)
        //{
        //    Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
        //        this.Shoot(text);

        //    }));
        //    //dmkCurt.Shoot(text);
        //}
        public void Shoot(string text)
        { 
            if (dm == null)
            {
                dm = new DanmakuManager(curtain, enableShadowEffect);
            }
            this.Dispatcher.BeginInvoke(new Action(() => dm.Shoot(text)));

        }
        //       	public void ShootDanmaku(string text) {
        //            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
        //                 Shoot(text);
        //                
        //            }));
        //        }
    }
}
