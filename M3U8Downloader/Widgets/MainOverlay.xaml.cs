using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace M3U8Downloader.Widgets
{
    public static class Store
    {
        public static double MainOverlayXoffset = 0.0;

        public static double MainOverlayYoffset = 0.0;

        public static double MainOverlayWidth = 320.0;

        public static double MainOverlayEffect1 = 0.8;

        public static double MainOverlayEffect2 = 0.59999999999999987;

        public static double MainOverlayEffect3 = 4.6;

        public static double MainOverlayEffect4 = 1.0;

        public static double MainOverlayFontsize = 18.667;

        public static double FullOverlayEffect1 = 400.0;

        public static double FullOverlayFontsize = 35.0;

        public static bool WtfEngineEnabled = true;
    }
    /// <summary>
    /// MainOverlay.xaml 的交互逻辑
    /// </summary>
    public partial class MainOverlay : Window
    {
        public MainOverlay()
        {
            InitializeComponent();
            Topmost = true;
            this.timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.ShowFront), base.Dispatcher);
            this.timer.Start();
            this.Loaded += MainOverlay_Loaded;
        }

        private void MainOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            Initail();
        }

        private void ShowFront(object sender, EventArgs eventArgs)
        {

            if (this != null)
            {
                this.Topmost = false;
                this.Topmost = true;
            }
        }
        private readonly DispatcherTimer timer;
        public void Initail()
        {
            this.OpenOverlay();
            this.Show();
            //			new Thread(new ThreadStart(() => {
            //			                           	while (true) {
            //			                           		this.AddDMText("LoveGaia", "我爱大哥我爱大哥我爱大哥我爱大哥我爱大哥我爱大哥", false, false);
            //			                           		Thread.Sleep(1000);
            //			                           	}
            //			                           })).Start();
        }
        
        private void overlay_Deactivated(object sender, EventArgs e)
        {
            if (sender is MainOverlay)
            {
                (sender as MainOverlay).Topmost = true;
            }
            //			overlay.Topmost=true;
        }
        [DllImport("user32")]
        private static extern uint SetWindowLong(IntPtr hwnd, int nIndex, uint dwNewLong);

        [DllImport("user32")]
        private static extern uint GetWindowLong(IntPtr hwnd, int nIndex);
        public void OpenOverlay()
        {
            //			this.overlay = new MainOverlay();
            this.Deactivated += new EventHandler(this.overlay_Deactivated);
            this.SourceInitialized += delegate (object sender, EventArgs e)
            {
                IntPtr expr_10 = new WindowInteropHelper(this).Handle;
                uint windowLong = GetWindowLong(expr_10, -20);
                SetWindowLong(expr_10, -20, windowLong | 32u);
            };
            this.Background = Brushes.Transparent;
            this.ShowInTaskbar = false;
            this.Topmost = true;
            this.Top = SystemParameters.WorkArea.Top + Store.MainOverlayXoffset;
            this.Left = SystemParameters.WorkArea.Right - Store.MainOverlayWidth + Store.MainOverlayYoffset;
            this.Height = SystemParameters.WorkArea.Height;
            this.Width = Store.MainOverlayWidth;
        }
        
        public void ToastMessage(string msgStr, MainWindow.TOAST_TYPE msgType)
        {
            string Title = "Message";
            System.Windows.Media.Color txtColor = Colors.Gray;
            System.Windows.Media.Color iconColor = Colors.White;
            MahApps.Metro.IconPacks.PackIconMaterialKind icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert;
            switch (msgType)
            {
                case MainWindow.TOAST_TYPE.MESSAGE:
                    Title = "Message";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert; ;
                    txtColor = Colors.Gray;
                    iconColor = Colors.White;
                    break;
                case MainWindow.TOAST_TYPE.ALERT:
                    Title = "Alert";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircleOutline; ;
                    txtColor = Colors.Orange;
                    iconColor = Colors.Wheat;
                    break;
                case MainWindow.TOAST_TYPE.ERROR:
                    Title = "Error";
                    iconColor = Colors.Red;
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.Alert; ;
                    txtColor = Colors.OrangeRed;
                    break;
            }
            if (Dispatcher.CheckAccess())
            {
                LogSnackBar toast = new LogSnackBar();
                toast.LogMsg.Text = msgStr;
                toast.msgIcon.Kind = icon;
                toast.msgIcon.Foreground = new SolidColorBrush(iconColor);
                toast.LogMsg.Foreground = new SolidColorBrush(txtColor);
                toast.LogType.Text = Title;
                toast.ChangeHeight();
                ((Storyboard)toast.Resources["Storyboard1"]).Completed += new EventHandler(this.Toast_Completed);
                this.LayoutRoot.Children.Add(toast);
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(delegate
                {
                    this.ToastMessage(msgStr, msgType);
                }));
            }
        }
        private void Toast_Completed(object sender, EventArgs e)
        {
            ClockGroup clockGroup = sender as ClockGroup;
            if (clockGroup == null)
            {
                return;
            }
            LogSnackBar toast = Storyboard.GetTarget(clockGroup.Children[2].Timeline) as LogSnackBar;
            if (toast != null)
            {
                this.LayoutRoot.Children.Remove(toast);
            }
        }

    }
}
