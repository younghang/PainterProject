
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TwoArm.Model;
namespace TwoArm.View
{
    /// <summary>
    /// ArmGeomotryControl.xaml 的交互逻辑
    /// </summary>
    public partial class ArmGeomotryControl : UserControl
    {
        private ArmGeomotry arm;

        public AnimationTimeline RotateWithAnimation(double angleDegreeFrom, double angleDegreeTo, double speed)
        {
            RotateTransform rtf = new RotateTransform();
            this.RenderTransform = rtf;
            rtf.CenterX = ArmGeomotryControl.MainJoint.X;
            rtf.CenterY = ArmGeomotryControl.MainJoint.Y;
            DoubleAnimation doubleAnimation = new DoubleAnimation(360 - angleDegreeFrom, 360 - angleDegreeTo, TimeSpan.FromSeconds(speed));
            //doubleAnimation.EasingFunction=new CircleEase();
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Angle"));
            return doubleAnimation;
        }
        public ArmGeomotryControl()
        {
            InitializeComponent();
            if (arm == null) arm = new ArmGeomotry();
            this.DataContext = arm;
        }
        public static Point SecondaryJoint { get { return new Point(175, 50); } }
        public static Point MainJoint { get { return new Point(50, 50); } }
    }
    public class ArmGeomotry : INotifyPropertyChanged
    {
        public int ArmLength
        {
            get { return GeoLength - (int)(3.0 * _width / 4); }
        }
        public int SmallCircleDiameter
        {
            get { return GeoWidth / 2; }
        }
        public int BigCircleDiameter
        {
            get { return GeoWidth; }
        }
        int _length = 200;
        int _width = 100;
        public int GeoWidth
        {
            get { return _width; }
            set { _width = value; OnPropertyChanged("GeoWidth"); }
        }
        public int GeoLength
        {
            get { return _length; }
            set
            {
                _length = value; OnPropertyChanged("GeoLength"); OnPropertyChanged("ArmLength");
                OnPropertyChanged("SmallCircleDiameter"); OnPropertyChanged("BigCircleDiameter");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        double angle;
        public double Angle
        {
            get { return angle; }
            set { angle = value; OnPropertyChanged("Angle"); }
        }
    }
}