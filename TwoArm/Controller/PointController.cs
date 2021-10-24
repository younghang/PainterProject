 using System;
using System.CodeDom;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Media;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using TwoArm.Model;
using TwoArm.View;

namespace TwoArm
{
    //业务逻辑处理    
    //负责传递View和Model之间的中介，转发消息，同时方便两者解耦，内容能少则少
    public class PointController
    {
        public ObservableCollection<PointModel> models = new ObservableCollection<PointModel>();
        private MainWindow window3;
        private ArmGeomotryControl firstArm;
        private ArmGeomotryControl2 secondArm;
        public PointController(MainWindow w3)
        {
            models.Add(new PointModel("取料位置", 120, 120, 1));
            models.Add(new PointModel("放料位置", 100, 80, 1));
            models.Add(new PointModel("原点位置", 185, 0, 1));
            window3 = w3;
            firstArm = new ArmGeomotryControl();
            secondArm = new ArmGeomotryControl2();
            LoadView();
        }
        public void SetPoint(int index, int x, int y, double speed)
        {
            models[index].Speed = speed;
            models[index].X = x;
            models[index].Y = y;
        }
        public void SetPoint(string name, int x, int y, double speed)
        {
            var re = from m in models where m.PointName == name select m;
            var resArray = re.ToArray();
            PointModel model;
            if (resArray != null && resArray.Length > 0)
                model = (PointModel)(resArray[0]);
            else
                model = new PointModel();
            model.Speed = speed;
            model.X = x;
            model.Y = y;
        }
        private PointModel curPointModel;
        private double curFirstAngle;
        private double curSecondAngle;
        Ellipse ellipse = new Ellipse();
        public string MoveTo(int x, int y, double speed)
        {
            this.window3.mainCanvas.Children.Remove(ellipse);
            this.window3.mainCanvas.Children.Add(ellipse);
            Canvas.SetTop(ellipse, 300 + 50 - y - 2);
            Canvas.SetLeft(ellipse, x + 50);
            curPointModel = new PointModel("CUR", x, y, 1);
            curFirstAngle = TwoArmModel.FirstArmAngle;
            curSecondAngle = TwoArmModel.SecondArmAngle;
            TwoArmModel.SetPointModel(curPointModel);
            if (!TwoArmModel.CalculateTwoArmAngle())
            {
                return "error! out of range";
            }
            Storyboard storyboard = new Storyboard();
            storyboard.Children.Add(firstArm.RotateWithAnimation(curFirstAngle * 180 / Math.PI, TwoArmModel.FirstArmAngle * 180 / Math.PI, speed));
            Tuple<AnimationTimeline, AnimationTimeline> t = secondArm.TransferReferFirstWithAnimation(curFirstAngle, TwoArmModel.FirstArmAngle, speed);
            storyboard.Children.Add(t.Item1);
            storyboard.Children.Add(t.Item2);
            AnimationTimeline animation = secondArm.RotateWithAnimation(curSecondAngle * 180 / Math.PI, TwoArmModel.SecondArmAngle * 180 / Math.PI, speed);
            storyboard.Children.Add(animation);
            storyboard.Begin();
            return "Finished: theta1=" + (TwoArmModel.FirstArmAngle * 180 / Math.PI).ToString("F2") + " , theta2=" + (TwoArmModel.SecondArmAngle * 180 / Math.PI).ToString("F2");
        }
        public void Reset()
        {
        }
        private void LoadView()
        {
            this.window3.mainCanvas.Children.Add(firstArm);
            Canvas.SetTop(firstArm, 300);
            Canvas.SetLeft(firstArm, 0);
            this.window3.mainCanvas.Children.Add(secondArm);
            Canvas.SetTop(secondArm, 300);
            Canvas.SetLeft(secondArm, 0);
            secondArm.TransferReferFirst(0);
            ellipse.Width = 4;
            ellipse.Height = 4;
            ellipse.Fill = Brushes.Aqua;
        }
    }

}
