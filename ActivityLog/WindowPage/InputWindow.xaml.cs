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
    /// InputWindow.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindow : Window
    {
        public InputWindow()
        {
            InitializeComponent();
            this.Loaded += InputWindow_Loaded;
        }

        private void InputWindow_Loaded(object sender, RoutedEventArgs e)
        {
             
        }
        private static InputWindow _instance = null;
        private static InputWindow Instance
        {
            get
            {
                if (_instance == null)//|| (_instance != null&&_)
                {
                    _instance = new InputWindow();
                }
                return _instance;
            }
        }
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;  // cancels the window close
            this.Hide();      // Programmatically hides the window
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }
        private string _inputText = "Hello";
        public string InputTxt { get { return _inputText; } set { _inputText = value; } } 
        public static string PleaseInput(string msg,string defautlStr="")
        {
            Instance.Left = SystemParameters.PrimaryScreenWidth / 2 - Instance.Width / 2;
            Instance.Top = SystemParameters.PrimaryScreenHeight / 2 - Instance.Height / 2; 
            Instance.txtMsg.Text = msg; 
            Instance.txtInputText.Text = defautlStr;
            Instance.ShowAnimation();
            Instance.ShowDialog();
            return Instance.txtInputText.Text;
        }
        private void ShowAnimation()
        {
            Storyboard story = (Storyboard)FindResource("Load");
            story.Begin();
        }
        private void HideAnimation()
        {
            Storyboard story = (Storyboard)FindResource("Hide");
            story.Begin();
        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void closeButton_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            HideAnimation();
        }

        private void ButtonEx_Click_2(object sender, RoutedEventArgs e)
        {
            HideAnimation();
        }
    }
}
