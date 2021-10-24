 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Utils;
using Point = System.Windows.Point;
namespace FireWorks.Model
{
    class Fireworks : INotifyPropertyChanged
    {
        public int TimeTick { get; set; }
        Point fromPoint;
        public Point toPoint { get; set; }
        Point curPoint;
        public static int CurrentID = 0;
        private int particleCount = 100;
        public int ParticleCount
        {
            get { return particleCount; }
            set
            {
                particleCount = value;
                OnPropertyChanged();
            }
        }
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public event Action<Point, int> CreateParticles;
        public double Distance
        {
            get
            {
                return Math.Sqrt(1.0 * (toPoint.X - fromPoint.X) * (toPoint.X - fromPoint.X) +
(toPoint.Y - fromPoint.Y) * (toPoint.Y - fromPoint.Y) * 1.0);
            }
        }
        public static int MAX_POINTS = 3;
        public List<Point> listPoints;
        public int Hue = 40;
        public double Angle { get { return Math.Atan2((toPoint.Y - fromPoint.Y), (toPoint.X - fromPoint.X)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private double _speed;
        public double Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged();
            }
        }
        private double _accelerate;
        public double Accelerator
        {
            get { return _accelerate; }
            set
            {
                _accelerate = value;
                OnPropertyChanged();
            }
        }
        private int _brightness;
        public int Brightness
        {
            get { return _brightness; }
            set { _brightness = value; OnPropertyChanged(); }
        }
        private double _targetRadius;
        public double TargetRadius
        {
            get { return _targetRadius; }
            set { _targetRadius = value; OnPropertyChanged(); }
        }
        public Fireworks(Point _fromPoint, Point _toPoint)
        {
            CurrentID++;
            this.fromPoint = _fromPoint;
            this.toPoint = _toPoint;
            this.curPoint = new Point(fromPoint.X, fromPoint.Y);
            Speed = 5;
            Accelerator = 1.05;
            Brightness = ran.Next(60, 100);
            TargetRadius = 5;
            Hue = ran.Next(30, 360);
            listPoints = new List<Point>(MAX_POINTS);
            this.ID = CurrentID;
        }
        private static Random ran = new Random();
        public bool IsDisposed = false;
        public static double GetPointDistance(Point x1, Point x2)
        {
            return Math.Sqrt(1.0 * (x2.X - x1.X) * (x2.X - x1.X) + 1.0 * (x2.Y - x1.Y) * (x2.Y - x1.Y));
        }
        public void Update()
        {
            if (IsDisposed)
                return;
            //Debug.Print("distance:" + Distance + ",  curDistance:" + GetPointDistance(curPoint, fromPoint));
            if (GetPointDistance(curPoint, fromPoint) >= Distance)
            {
                curPoint = toPoint;
                CreateParticles(toPoint, Hue);
                IsDisposed = true;
            } 
            if (this.listPoints.Count > 3) this.listPoints.RemoveAt(0);
            this.listPoints.Add(new Point(curPoint.X, curPoint.Y));
            if (TargetRadius < 8) TargetRadius += 0.3;
            else TargetRadius = 1;
            Speed *= this.Accelerator;
            double deltaX = Math.Cos(this.Angle) * Speed;
            double deltaY = Math.Sin(this.Angle) * Speed;
            curPoint.X += (float)deltaX;
            curPoint.Y += (float)deltaY;

        }
        public void Draw(Graphics graphics)
        { 
            if (listPoints.Count < 2) return;
            var color=CommonUtils.HslToRgb(Hue, 100, Brightness);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
            System.Drawing.Pen pen = new System.Drawing.Pen(c);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.Width = 3;
            graphics.DrawLine(pen,
                (float)listPoints[listPoints.Count - 2].X, (float)listPoints[listPoints.Count - 2].Y,
                 (float)listPoints[listPoints.Count - 1].X, (float)listPoints[listPoints.Count - 1].Y);
            if (IsDisposed)
            {
                listPoints.RemoveAt(0);
            }
        }

    }
    class Particles
    {
        Point curPos;
        List<Point> listPoints;
        double Angle;
        double Speed;
        double friction = 0.95;
        public double gravity { get; set; }
        int Brightness;
        double Alpha = 1;
        int Hue;
        double Decay;
        static int MAX_COUNT = 10;
        public bool IsDisposed = false;
        public Particles(Point p, int hue)
        {
            this.curPos = p;
            listPoints = new List<Point>(MAX_COUNT);
            Angle = ran.NextDouble() * Math.PI * 2;
            //Debug.Print("Angle" + this.Angle);
            Speed = ran.NextDouble() * 10;//产生的随机数是一样的
            Hue = ran.Next(hue - 30, hue + 30);
            Brightness = ran.Next(60, 100);
            Decay = ran.NextDouble() * 0.0015 + 0.0015;
            gravity = 0.98;
        }
        static Random ran = new Random();


        public void Update()
        {
            //Debug.Print("Update:数量" + this.listPoints.Count);
            if (IsDisposed)
                return;
            if (this.listPoints.Count > 10) listPoints.RemoveAt(0);
            listPoints.Add(new Point(curPos.X, curPos.Y));
            Speed *= friction;
            double deltaX = Math.Cos(this.Angle) * Speed;
            double deltaY = Math.Sin(this.Angle) * Speed + gravity;
            curPos.X += (float)deltaX;
            curPos.Y += (float)deltaY;
            Alpha -= Decay;
            if (Alpha < Decay)
            {
                IsDisposed = true;
                listPoints.Clear();
            }
        }
        public void Draw(Graphics graphics)
        {
            
            //Debug.Print("Draw:数量" + this.listPoints.Count);
            if (listPoints.Count < 8) return;
            var color = CommonUtils.HslToRgb(Hue, 100, Brightness);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
            System.Drawing.Pen pen = new System.Drawing.Pen(c);
            pen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            pen.Width = 1;
            graphics.DrawLine(pen,
                (float)listPoints[listPoints.Count - 2].X, (float)listPoints[listPoints.Count - 2].Y,
                 (float)listPoints[listPoints.Count - 1].X, (float)listPoints[listPoints.Count - 1].Y);
            if (IsDisposed)
            {
                listPoints.Clear();
            }
        }
    }
}