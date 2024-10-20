using ShowDanmu;
using ShowDanmu.RightSide.Control;
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

namespace ShowDanmu.RightSide
{
    static class Store
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
    /// //1.first init
    /// MainOverlay.Instance.Initial();
    /// //2.second show your message
    /// MainOverlay.Instance.ToastMessage("information",MainOverlay.TOAST_TYPE.MESSAGE)
    /// or MainOverlay.Instance.AddText();
    /// //3.close 
    /// MainOverlay.Instance.Close();
    /// </summary>
    public partial class MainOverlay : Window
    {
        private MainOverlay()
        {
            InitializeComponent();
            Topmost = true;
            this.timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, new EventHandler(this.ShowFront), base.Dispatcher);
            this.timer.Start();
            this.Loaded += MainOverlay_Loaded;
            this.Closing += MainOverlay_Closing;
        }
        public static MainOverlay Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new MainOverlay();
                    _instance.Show();
                }
                return _instance;
            }
        }
        private static MainOverlay _instance=null;
        private void MainOverlay_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            timer.Stop();

        }

        private void MainOverlay_Loaded(object sender, RoutedEventArgs e)
        {
            Initial();
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
        private void Initial()
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
        private void OpenOverlay()
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
        public enum TOAST_TYPE { MESSAGE, ERROR, ALERT }
        public void ToastMessage(string msgStr, TOAST_TYPE msgType)
        {
            string Title = "Message";
            System.Windows.Media.Color txtColor = Colors.Gray;
            System.Windows.Media.Color iconColor = Colors.White;
            MahApps.Metro.IconPacks.PackIconMaterialKind icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert;
            switch (msgType)
            {
                case TOAST_TYPE.MESSAGE:
                    Title = "Message";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert; ;
                    txtColor = Colors.Gray;
                    iconColor = Colors.White;
                    break;
                case TOAST_TYPE.ALERT:
                    Title = "Alert";
                    icon = MahApps.Metro.IconPacks.PackIconMaterialKind.AlertCircleOutline; ;
                    txtColor = Colors.Orange;
                    iconColor = Colors.Wheat;
                    break;
                case TOAST_TYPE.ERROR:
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
        public int  GetTextCount()
        {
            return this.LayoutRoot.Children.Count;
        }
        public void AddText(string user, string text, bool warn = false, bool foreceenablefullscreen = false)
        {

            if (Dispatcher.CheckAccess())
            {
                //				if (this.SideBar.IsChecked == true)
                //				{
                DanmakuTextControl danmakuTextControl = new DanmakuTextControl();
                danmakuTextControl.UserName.Text = user;
                if (warn)
                {
                    danmakuTextControl.UserName.Foreground = Brushes.Red;
                }
                danmakuTextControl.Text.Text = text;
                danmakuTextControl.ChangeHeight();
                ((Storyboard)danmakuTextControl.Resources["Storyboard1"]).Completed += new EventHandler(this.Toast_Completed);
                this.LayoutRoot.Children.Add(danmakuTextControl);
                //				}
                //				if (this.Full.IsChecked == true && (!warn | foreceenablefullscreen))
                //				{
                //				this.fulloverlay.AddDanmaku(DanmakuType.Scrolling, text, 4294967295u);
                //				return;
                //				}
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    this.AddText(user, text, false, false);
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
            var item =Storyboard.GetTarget(clockGroup.Children[2].Timeline);
             
            if (item != null)
            {
                this.LayoutRoot.Children.Remove((UIElement)item);
            }
        }

    }
}
