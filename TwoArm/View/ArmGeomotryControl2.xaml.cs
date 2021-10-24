 
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
    public partial class ArmGeomotryControl2 : UserControl
    {
        public ArmGeomotryControl2()
        {
            InitializeComponent();
        }
        public AnimationTimeline RotateWithAnimation(double angleDegreeFrom, double angleDegreeTo, double speed)
        {
            RotateTransform rtf = new RotateTransform();
            this.RenderTransform = rtf;
            rtf.CenterX = ArmGeomotryControl2.MainJoint.X;
            rtf.CenterY = ArmGeomotryControl2.MainJoint.Y;
            DoubleAnimation doubleAnimation = new DoubleAnimation(360 - angleDegreeFrom, 360 - angleDegreeTo, TimeSpan.FromSeconds(speed));
            //doubleAnimation.EasingFunction = new CircleEase();
            Storyboard.SetTarget(doubleAnimation, this);
            Storyboard.SetTargetProperty(doubleAnimation, new PropertyPath("RenderTransform.Angle"));
            return doubleAnimation;
        }
        public void TransferReferFirst(double thetaOfFirst)
        {
            double offsetX = TwoArmModel.FirstArmLength * (Math.Cos(thetaOfFirst));
            double offsetY = TwoArmModel.FirstArmLength * Math.Sin(thetaOfFirst);
            //this.RenderTransform = new TranslateTransform(TwoArmModel.FirstArmLength * (1 - Math.Cos(thetaOfFirst)), TwoArmModel.FirstArmLength * Math.Sin(thetaOfFirst));
            Canvas.SetTop(this, 300 - offsetY + 50 - 25);
            Canvas.SetLeft(this, offsetX + 50 - 25);
        }
        public Tuple<AnimationTimeline, AnimationTimeline> TransferReferFirstWithAnimation(double thetaFrom, double thetaTo, double speed)
        {
            PointAnimationUsingPath pointPathAnimation = new PointAnimationUsingPath();
            double XFrom = TwoArmModel.FirstArmLength * Math.Cos(thetaFrom) + 50 - 25;
            double XTo = TwoArmModel.FirstArmLength * Math.Cos(thetaTo) + 50 - 25;
            double YFrom = 300 + 50 - 25 - TwoArmModel.FirstArmLength * Math.Sin(thetaFrom);
            double YTo = 300 + 50 - 25 - TwoArmModel.FirstArmLength * Math.Sin(thetaTo);
            XEaseMoveAnimation doubleAnimationX = new XEaseMoveAnimation();
            doubleAnimationX.To = thetaTo;
            doubleAnimationX.Duration = TimeSpan.FromSeconds(speed * 1);
            doubleAnimationX.From = thetaFrom;
            YEaseMoveAnimation doubleAnimationY = new YEaseMoveAnimation();
            doubleAnimationY.To = thetaTo;
            doubleAnimationY.Duration = TimeSpan.FromSeconds(speed * 1);
            doubleAnimationY.From = thetaFrom;
            //doubleAnimationX.EasingFunction = xChange; 
            //doubleAnimationY.EasingFunction = yChange;
            Storyboard.SetTarget(doubleAnimationX, this);
            Storyboard.SetTargetProperty(doubleAnimationX, new PropertyPath("(Canvas.Left)"));
            Storyboard.SetTarget(doubleAnimationY, this);
            Storyboard.SetTargetProperty(doubleAnimationY, new PropertyPath("(Canvas.Top)"));
            return new Tuple<AnimationTimeline, AnimationTimeline>(doubleAnimationX, doubleAnimationY);
        }
        public static Point SecondaryJoint { get { return new Point(75, 25); } }
        public static Point MainJoint { get { return new Point(25, 25); } }
    }

    public class YEaseMoveAnimation : XEaseMoveAnimation
    {
        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double delta = animationClock.CurrentProgress.Value;
            double currentValue = (double)((To - From) * delta + From);
            return 300 + 50 - 25 - TwoArmModel.FirstArmLength * Math.Sin(currentValue);
        }
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new YEaseMoveAnimation();
        }
    }
    public class XEaseMoveAnimation : DoubleAnimationBase
    {
        public static readonly DependencyProperty FromProperty = DependencyProperty.Register(
            "From", typeof(double?), typeof(XEaseMoveAnimation), new PropertyMetadata(null));
        public static readonly DependencyProperty ToProperty = DependencyProperty.Register(
            "To", typeof(double?), typeof(XEaseMoveAnimation), new PropertyMetadata(null));
        public double? From
        {
            get
            {
                return (double?)this.GetValue(XEaseMoveAnimation.FromProperty);
            }
            set
            {
                this.SetValue(XEaseMoveAnimation.FromProperty, value);
            }
        }
        public double? To
        {
            get
            {
                return (double?)this.GetValue(XEaseMoveAnimation.ToProperty);
            }
            set
            {
                this.SetValue(XEaseMoveAnimation.ToProperty, value);
            }
        }

        protected override double GetCurrentValueCore(double defaultOriginValue, double defaultDestinationValue, AnimationClock animationClock)
        {
            double delta = animationClock.CurrentProgress.Value;
            double currentValue = (double)((To - From) * delta + From);
            //Debug.Print(delta+"");
            return TwoArmModel.FirstArmLength * Math.Cos(currentValue) + 50 - 25;

        }
        protected override System.Windows.Freezable CreateInstanceCore()
        {
            return new XEaseMoveAnimation();
        }
    }
}