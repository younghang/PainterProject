using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ActivityLog.Model;
using ActivityLog.Model.ViewModel;
using ActivityLog.Widgets;
using ActivityLog.WindowPage;
using ActivityLog.WindowPage.UserControls;

namespace ActivityLog
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            VMActivity.Instance.LoadDataFromJson();
            
        }  
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ucOverview = new UCOverview();
            UCActivity = new UCActivity();
            UCRecord = new UCRecord();
            ucOverview.AttachNextEvent += AttachNextEventHandle;
            UCActivity.AttachNextEvent += AttachNextEventHandle;
            UCRecord.AttachNextEvent += AttachNextEventHandle;
            ActivityLog.Model.Activity activity = new Model.Activity();
            SwitchToUC(UCActivity);
        }

        private void AttachNextEventHandle()
        {
            this.mainFrm.Children.Clear();
            if (currentUC != null)
            {
                if (!this.mainFrm.Children.Contains(currentUC.GetUC()))
                {
                    this.mainFrm.Children.Add(currentUC.GetUC());
                }
            }
        }
         
        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            VMActivity.Instance.SaveData(); 
            Application.Current.Shutdown();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void ButtonEx_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void MinWindow(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private UCOverview ucOverview = null;
        private UCActivity UCActivity = null;
        private UCRecord UCRecord = null;
        private IUCSwitch currentUC = null;
        private void SwitchToUC(IUCSwitch uc)
        {
            if (currentUC != null && currentUC != uc)
            {
                currentUC.Detach();
            }
            if (currentUC == null)
            {
                currentUC = uc;
                AttachNextEventHandle();
            }
            else
            {
                currentUC = uc;
            }

        }
        private void MainFrmSwitchClick(object sender, RoutedEventArgs e)
        {
            ButtonEx button = sender as ButtonEx;
            foreach (var item in spSwitchMain.Children)
            {
                ButtonEx buttonEx = item as ButtonEx;
                buttonEx.IsChecked = false;
            }
            button.IsChecked = true;
            switch (button.Name)
            {
                case "btnOverview":
                    SwitchToUC(ucOverview);
                    break;
                case "btnActivity":
                    SwitchToUC(UCActivity);
                    break;
                case "btnRecord":
                    SwitchToUC(UCRecord);
                    break;
            }
        }
        public enum TOAST_TYPE { MESSAGE,ERROR,ALERT}
        public void ToastMessage(string msgStr,TOAST_TYPE msgType )
        {
            string Title = "Message";
            System.Windows.Media.Color txtColor = Colors.Gray;
            System.Windows.Media.Color iconColor = Colors.White;
            MahApps.Metro.IconPacks.PackIconMaterialKind icon= MahApps.Metro.IconPacks.PackIconMaterialKind.BellAlert;  
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
                this.SnackBar.Children.Add(toast); 
            }
            else
            {
                Dispatcher.BeginInvoke(new Action(delegate {
                    this.ToastMessage( msgStr,msgType );
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
                this.SnackBar.Children.Remove(toast);
            }
        }
    }
}
