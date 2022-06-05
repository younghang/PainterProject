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

namespace ActivityLog.Widgets
{
    /// <summary>
    /// LogSnackBar.xaml 的交互逻辑
    /// </summary>
    public partial class LogSnackBar : UserControl
    {
        public LogSnackBar()
        {
            InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.DanmakuTextControl_Loaded);
            Storyboard story = (Storyboard)base.Resources["Storyboard1"];
            Storyboard.SetTarget(story.Children[2], this);
        }
        public void ChangeHeight()
        {
            this.border.Measure(new Size(360, 2147483647.0));
            (((Storyboard)base.Resources["Storyboard1"]).Children[0] as DoubleAnimationUsingKeyFrames).KeyFrames[1].Value = this.border.DesiredSize.Height;
        }
        private void DanmakuTextControl_Loaded(object sender, RoutedEventArgs e)
        {
            base.Loaded -= new RoutedEventHandler(this.DanmakuTextControl_Loaded);
        }
    }

}
