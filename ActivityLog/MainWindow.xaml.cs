using System;
using System.Collections.Generic;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ActivityLog.Model;
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
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ucOverview = new UCOverview();
            UCActivity = new UCActivity();
            ucOverview.AttachNextEvent += AttachNextEventHandle;
            UCActivity.AttachNextEvent += AttachNextEventHandle;
            ActivityLog.Model.Activity activity = new Model.Activity();
            SwitchToUC(ucOverview);
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
            AddNewActivity addnew = new AddNewActivity();

            addnew.ShowDialog();
            if (addnew.DialogResult == true)
            {
                Activity activity = addnew.CurActivity;
            }
        }

        private void MinWindow(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private UCOverview ucOverview = null;
        private UCActivity UCActivity = null;
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
                    SwitchToUC(null);
                    break;
            }
        }
    }
}
