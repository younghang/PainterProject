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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ActivityLog.WindowPage.UserControls
{
    /// <summary>
    /// UCRecord.xaml 的交互逻辑
    /// </summary>
    public partial class UCRecord : UserControl,IUCSwitch
    {
        public UCRecord()
        {
            InitializeComponent();
            this.Loaded += UCRecord_Loaded;
        }

        private void UCRecord_Loaded(object sender, RoutedEventArgs e)
        {
             
        }

        public event Action AttachNextEvent;

        public UserControl GetUC() { return this; }
        public void Detach()
        {
            Storyboard sb = (Storyboard)App.Current.FindResource("closeDW0");
            if (sb.IsFrozen)
            {
                sb = sb.Clone();
            }
            sb.Completed += Sb_Completed;
            sb.Begin(this);
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            AfterDetach();
        }

        public void AfterDetach()
        {
            if (AttachNextEvent != null)
            {
                AttachNextEvent();
            }
        }

        private void AddNewRecord(object sender, RoutedEventArgs e)
        {

        }
    }
}
