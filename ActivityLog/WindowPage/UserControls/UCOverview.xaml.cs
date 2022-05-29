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
using System.Windows.Media.Animation;
namespace ActivityLog.WindowPage.UserControls
{
    public interface IUCSwitch
    {
        UserControl GetUC();
        void Detach();
        void AfterDetach();
        event Action AttachNextEvent;
    }
    /// <summary>
    /// UCOverview.xaml 的交互逻辑
    /// </summary>
    public partial class UCOverview : UserControl, IUCSwitch
    {
        public event Action AttachNextEvent;

        public UserControl GetUC() { return this; }
        public void Detach() {
            Storyboard sb = (Storyboard)App.Current.FindResource("closeDW1");
            if (sb.IsFrozen)
            {
                sb = sb.Clone();
            }
            sb.Completed += Sb_Completed;
            sb.Begin(MainBorder);
        }

        private void Sb_Completed(object sender, EventArgs e)
        {
            AfterDetach();
        }

        public void AfterDetach() {
            if (AttachNextEvent!=null)
            {
                AttachNextEvent();
            }
        }
        public UCOverview()
        {
            InitializeComponent();
        }
    }
}
