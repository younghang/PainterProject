using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using ActivityLog.WindowPage;

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
            ActivityLog.Model.Activity activity = new Model.Activity();
            this.textInfo.Text = ActivityLog.Model.Activity.ToJsonStr(activity);
            Activity.LoadFromJson(this.textInfo.Text);
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
            if (addnew.DialogResult==true)
            {
                Activity activity = addnew.CurActivity;
            } 
        }

        private void MinWindow(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
