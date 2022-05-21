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
using System.Windows.Shapes;

namespace ActivityLog.WindowPage
{
    /// <summary>
    /// AddNewActivity.xaml 的交互逻辑
    /// </summary>
    public partial class AddNewActivity : Window
    {
        public AddNewActivity()
        {
            InitializeComponent();
        }
        private void CloseWindow(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
        private void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }

}
