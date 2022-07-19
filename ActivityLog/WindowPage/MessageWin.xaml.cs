using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ActivityLog.WindowPage
{
    /// <summary>
    /// MessageWin.xaml 的交互逻辑
    /// </summary>
    public partial class MessageWin : Window
    {
        private MessageWin()
        {
            InitializeComponent();
            this.Loaded += MessageWin_Loaded;
        }

        private void MessageWin_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private static MessageWin _instance = null;
        private static MessageWin Instance
        {
            get
            {
                if (_instance == null)//|| (_instance != null&&_)
                {
                    _instance = new MessageWin();
                }
                return _instance;
            }
        }
        private bool IsConfirm = false;

        public static async Task<bool> Working(Func<bool> action)
        {
            var task = Task<bool>.Run(action);
            bool result = await task;
            Instance.Hide();
            return result;
        }
        public static void SetLoadingMsg(string msg)
        {
            Instance.txtLoadingMsg.Dispatcher.Invoke(
                ()=> { 
            Instance.txtLoadingMsg.Text = msg;
                });
        }
        public static bool LoadingAsync(Func<bool> action)
        {
            Instance.gridWaiting.Visibility = Visibility.Visible;
            Instance.gridMsg.Visibility = Visibility.Hidden;
            Storyboard storyboard = Instance.FindResource("loadingAnimation") as Storyboard;
            storyboard.Begin();
            var taskResult = Working(action);
            Instance.ShowAnimation();
            Instance.ShowDialog();
            bool result = taskResult.Result;
            storyboard.Stop();
            return result;
        }
        public static void MSG(string msg)
        {
            Instance.Left = SystemParameters.PrimaryScreenWidth / 2 - Instance.Width / 2;
            Instance.Top = SystemParameters.PrimaryScreenHeight / 2 - Instance.Height / 2;
            Instance.txtMsg.Text = msg;
            Instance.Title = "Message";
            Instance.msgIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.Bell;
            Instance.spConfirm.Visibility = Visibility.Hidden;
            Instance.spMsg.Visibility = Visibility.Visible;
            Instance.gridWaiting.Visibility = Visibility.Hidden;
            Instance.gridMsg.Visibility = Visibility.Visible;
            Instance.ShowAnimation();
            Instance.ShowDialog();
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close
            this.Hide();      // Programmatically hides the window
        }
        public static bool Confirm(string msg)
        {
            Instance.Left = SystemParameters.PrimaryScreenWidth / 2 - Instance.Width / 2;
            Instance.Top = SystemParameters.PrimaryScreenHeight / 2 - Instance.Height / 2;
            Instance.txtMsg.Text = msg;
            Instance.Title = "Confirm";
            Instance.msgIcon.Kind = MahApps.Metro.IconPacks.PackIconMaterialKind.HelpCircleOutline;
            Instance.spMsg.Visibility = Visibility.Hidden;
            Instance.spConfirm.Visibility = Visibility.Visible;
            Instance.gridWaiting.Visibility = Visibility.Hidden;
            Instance.gridMsg.Visibility = Visibility.Visible;
            Instance.ShowAnimation();
            Instance.ShowDialog();

            return Instance.IsConfirm;
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonEx_Click(object sender, RoutedEventArgs e)
        {
            IsConfirm = true;
            HideAnimation();
        }

        private void ButtonEx_Click_1(object sender, RoutedEventArgs e)
        {
            IsConfirm = false;
            HideAnimation();
        }

        private void ButtonEx_Click_2(object sender, RoutedEventArgs e)
        {
            HideAnimation();
        }

        private void closeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideAnimation();
        }
        private void HideAnimation()
        {
            Storyboard story = (Storyboard)FindResource("Hide");
            story.Begin();
        }
        private void ShowAnimation()
        {
            Storyboard story = (Storyboard)FindResource("Load");
            story.Begin();
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
