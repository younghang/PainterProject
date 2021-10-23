﻿/*
 * Created by SharpDevelop.
 * User: YangH7
 * Date: 09/07/2020
 * Time: 11:08
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace Painter.Models
{
    /// <summary>
    /// Description of Shape.
    /// </summary>
    public interface IOperationable
    {
        void Translate(PointGeo offset);
        void Rotate();
        void Scale(PointGeo offset);
    }
    [Serializable]
    public class PointGeo
    {
        public float X
        {
            get;
            set;
        }
        public float Y
        {
            get;
            set;
        }
        public bool IsNeedDraw = true;
        public PointGeo(float x, float y)
        {
            X = x;
            Y = y;
        }
        public PointGeo()
        {
            X = 0;
            Y = 0;
        }
        public static PointGeo operator +(PointGeo p1, PointGeo p2)
        {
            return new PointGeo(p1.X + p2.X, p1.Y + p2.Y);
        }
        public static PointGeo operator -(PointGeo p1, PointGeo p2)
        {
            return new PointGeo(p1.X - p2.X, p1.Y - p2.Y);
        }
        public static PointGeo operator *(PointGeo p1, float num)
        {
            return new PointGeo(p1.X * num, p1.Y * num);
        }
        public static PointGeo operator *(float num, PointGeo p1)
        {
            return new PointGeo(p1.X * num, p1.Y * num);
        }
        public static PointGeo operator /(PointGeo p1, float num)
        {
            return new PointGeo(p1.X / num, p1.Y / num);
        }
        public static PointGeo operator /(float num, PointGeo p1)
        {
            return new PointGeo(num / p1.X, num / p1.Y);
        }
        public PointGeo Clone()
        {
            return new PointGeo(this.X, this.Y);
        }
        public PointGeo Offset(PointGeo p)
        {
            this.X += p.X;
            this.Y += p.Y;
            return this;
        }

        internal void Scale(PointGeo p)
        {
            this.X *= p.X;
            this.Y *= p.Y; ;
        }
        public PointGeo GetAbs()
        {
            return new PointGeo(Math.Abs(X), Math.Abs(Y));
        }
        public override string ToString()
        {
            return "(" + this.X.ToString("f3") + ", " + this.Y.ToString("f3") + ")";
        }
    }

    [Serializable]
    public abstract class Shape : IOperationable, IScreenPrintable
    {
        public bool IsFinishedInitial = false;//减少离散点计算
        public bool IsShow = true;
        public bool IsNonTrans = false;//temp use, delete sooon
        [NonSerialized]
        protected PainterBase Painter;
        public virtual void SetPainter(PainterBase painter)
        {
            this.Painter = painter;
        }
        public string MSG { get; set; }
        private PointGeo _start = new PointGeo(0, 0);
        private PointGeo _end = new PointGeo(0, 0);
        public PointGeo StartPoint
        {
            get { return _start; }
            set
            {
                this._start = value;
                Initial();
            }
        }
        public PointGeo EndPoint
        {
            get { return _end; }
            set
            {
                this._end = value;
                Initial();
            }
        }

        public virtual float GetMaxY()
        {
            return Math.Max(this.StartPoint.Y, this.EndPoint.Y);
        }
        protected double _minX = 1E6;
        protected double _minY = 1E6;
        protected double _maxX;
        protected double _maxY;
        public virtual float GetMinY()
        {
            return Math.Min(this.StartPoint.Y, this.EndPoint.Y);
        }
        public virtual float GetMaxX()
        {
            return Math.Max(this.StartPoint.X, this.EndPoint.X);
        }
        public virtual float GetMinX()
        {
            return Math.Min(this.StartPoint.X, this.EndPoint.X);
        }
        public int Width
        {
            get;
            set;
        }
        public int Heigth
        {
            get;
            set;
        }
        protected ShapeMeta drawMata = null;
        //包含绘制图形的基本信息
        [NonSerialized]
        public Action<string> UpdateMessage = null;
        public virtual void OnUpdateMessage(string msg)
        {
            if (UpdateMessage != null)
            {
                UpdateMessage(msg);
            }
        }
        public Shape()
        {
        }
        public Shape(PointGeo s, PointGeo e)
        {
            this.StartPoint = s;
            this.EndPoint = e;
        }
        protected virtual void Initial()
        {
            this.Width = (int)Math.Abs(StartPoint.X - EndPoint.X);
            this.Heigth = (int)Math.Abs(StartPoint.Y - EndPoint.Y);
        }
        public Shape SetDrawMeta(ShapeMeta dm)
        {
            this.drawMata = dm;
            return this;
        }
        public DrawMeta GetDrawMeta()
        {
            return this.drawMata;
        }
        public bool HasDrawMeta()
        {
            if (drawMata == null)
            {
                return false;
            }
            return true;
        }
        public void Translate(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }
            this.StartPoint = this.StartPoint + offset;
            this.EndPoint = this.EndPoint + offset;
        }
        public void Rotate()
        {
            throw new NotImplementedException();
        }
        public void Scale(PointGeo scale)
        {
            this.StartPoint.Scale(scale);
            this.EndPoint.Scale(scale);
        }
        public abstract void Draw();
        public void Draw(PainterBase p)
        {
            //			if (this.Painter==null||this.Painter!=p) {就是这个导致没画出来图，搞了两个小时
            //
            //			}
            this.Painter = p;
            this.Draw();
        }
        //这样写更简单
        public abstract PointGeo GetXDirectionAtLength(float length);
        //这样也可以但是太麻烦了，需要计算很多的点而且还需要内存和时间，不如上面直接算出来
        public double GetXDirectionAtLength(double v)
        {
            if (PerimeterList.Count == 0)
            {
                DispersedStartWithRightSide();
            }
            int count = PerimeterList.Count;
            if (PerimeterList[count - 1] < v)
            {
                return dispersedPoints[count - 1].X;
            }
            if (PerimeterList[0] > v)
            {
                return dispersedPoints[0].X;
            }
            double tmp = PerimeterList[count / 2];
            int left = 0;
            int right = count;
            int center = (right + left) / 2;
            while (true)
            {
                if (Math.Abs(tmp - v) < 1E-1)
                {
                    break;
                }
                if (tmp < v)
                {
                    left = center;
                }
                else
                {
                    right = center;
                }
                center = (right + left) / 2;
                tmp = PerimeterList[center];
            }
            return dispersedPoints[center].X;

        }
        public abstract void DispersedStartWithRightSide();
        internal abstract double GetPerimeter();

        protected List<PointGeo> dispersedPoints = new List<PointGeo>();
        protected List<double> PerimeterList = new List<double>();


    }
    [Serializable]
    public class RectangeGeo : Shape
    {
        public RectangeGeo()
        {
        }
        protected override void Initial()
        {
            base.Initial();
            LeftCorner = (int)Math.Min(EndPoint.X, StartPoint.X);
            TopCorner = (int)Math.Min(EndPoint.Y, StartPoint.Y);
        }
        public override void Draw()
        {
            float num = EndPoint.X - StartPoint.X;
            float num2 = EndPoint.Y - StartPoint.Y;
            MSG = String.Format("当前坐标：({0},{1})--坐标偏移量：({2},{3})", EndPoint.X, EndPoint.Y, num, num2);
            if (this.drawMata == null)
                return;
            if (Painter != null)
                Painter.DrawRectangle(this);
        }



        public override void DispersedStartWithRightSide()
        {
            throw new NotImplementedException();
        }

        public override PointGeo GetXDirectionAtLength(float length)
        {
            throw new NotImplementedException();
        }

        internal override double GetPerimeter()
        {
            return 2 * (GetMaxX() - GetMinX()) * (GetMaxY() - GetMinX());
        }

        public int LeftCorner { get; set; }
        public int TopCorner { get; set; }
    }
    [Serializable]
    public sealed class EllipseGeo : RectangeGeo
    {
        public EllipseGeo()
        {
        }
        public override void Draw()
        {
            float num = EndPoint.X - StartPoint.X;
            float num2 = EndPoint.Y - StartPoint.Y;
            MSG = String.Format("当前坐标：({0},{1})--坐标偏移量：({2},{3})", EndPoint.X, EndPoint.Y, num, num2);
            if (this.drawMata == null)
                return;
            if (Painter != null)
                Painter.DrawEllipse(this);
        }
    }
    [Serializable]
    public class CircleGeo : ArcGeo
    {
        public CircleGeo()
        {
        }
        protected override void Initial()
        {
            base.Initial();
            CenterX = StartPoint.X;
            CenterY = StartPoint.Y;
            Radius = (int)(Math.Sqrt(1.0 * (StartPoint.X - EndPoint.X) * (StartPoint.X - EndPoint.X) + 1.0 * (StartPoint.Y - EndPoint.Y) * (StartPoint.Y - EndPoint.Y)));
        }
        public override void Draw()
        {
            if (this.drawMata == null)
                return;
            if (!IsShow)
            {
                return;
            }
            if (Painter != null)
                Painter.DrawCircle(this);
            MSG = String.Format("圆心坐标：({0},{1})--半径：{2}", StartPoint.X, StartPoint.Y, Radius);
        }

        public override void DispersedStartWithRightSide()
        {
            return;
        }

    }
    [Serializable]
    public class ArcGeo : Shape
    {
        public static int DISPERSED_COUNT = 500;

        public ArcGeo()
        {
        }
        public override float GetMaxX()
        {
            return (float)_maxX;
        }
        public override float GetMaxY()
        {
            return (float)_maxY;
        }
        public override float GetMinX()
        {
            return (float)_minX;
        }
        public override float GetMinY()
        {
            return (float)_minY;
        }
        public bool IsClockwise { get; set; }
        //终点角度始终大于起点角度，逆时针：从起始角到终止角，顺时针：从终止角到起始角
        private float _startAngle = 0;
        private float _endAngle = 0;
        private float _radius = 0;
        private float _centerX = 0;
        private float _centerY = 0;
        //-180→180
        public float StartAngle
        {
            get { return _startAngle; }
            set
            {
                while (value > 180)
                {
                    value -= 360;
                }
                while (value < -180)
                {
                    value += 360;
                }
                _startAngle = value;
                DispersedStartWithRightSide();
            }
        }
        //大于StartAngle: -180→180+
        public float EndAngle
        {
            get { return _endAngle; }
            set
            {
                _endAngle = value;
                if (_endAngle <= _startAngle)
                {
                    _endAngle += 360;
                }
                DispersedStartWithRightSide();
            }
        }

        public float Radius
        {
            get { return _radius; }
            set
            {
                _radius = value;
                DispersedStartWithRightSide();
            }
        }
        public float CenterX
        {
            get { return _centerX; }
            set
            {
                _centerX = value;
                DispersedStartWithRightSide();
            }
        }
        public float CenterY
        {
            get { return _centerY; }
            set
            {
                _centerY = value;
                DispersedStartWithRightSide();
            }
        }
        public override void Draw()
        {
            if (this.drawMata == null)
                return;
            if (Painter != null)
                Painter.DrawArc(this);
            MSG = String.Format("圆心坐标：({0},{1})--半径：{2}", CenterX, CenterY, Radius);

        }
        public override void DispersedStartWithRightSide()
        {
            if (IsFinishedInitial)
            {
                return;
            }
            this._minX = 1E6;
            this._minY = 1E6;
            this._maxX = -1E6;
            this._maxY = -1E6;
            float curDegree = StartAngle;
            dispersedPoints.Clear();
            PerimeterList.Clear();
            PointGeo tempPoint = null;
            for (int i = 0; i < DISPERSED_COUNT; i++)
            {
                if (!IsClockwise)
                {//逆时针，从StartAngle到EndAngle
                    curDegree = StartAngle + (EndAngle - StartAngle) / DISPERSED_COUNT * i;
                    tempPoint = new PointGeo((float)(CenterX + Radius * Math.Cos(Math.PI / 180 * curDegree)), (float)(CenterY + Radius * Math.Sin(Math.PI / 180 * curDegree)));
                    dispersedPoints.Add(tempPoint);
                    PerimeterList.Add(Math.Abs(curDegree - StartAngle) / 180 * Math.PI * Radius);
                }
                else
                {
                    curDegree = EndAngle + (StartAngle - EndAngle) / DISPERSED_COUNT * i;
                    tempPoint = new PointGeo((float)(CenterX + Radius * Math.Cos(Math.PI / 180 * curDegree)), (float)(CenterY + Radius * Math.Sin(Math.PI / 180 * curDegree)));
                    dispersedPoints.Add(tempPoint);
                    PerimeterList.Add(Math.Abs(curDegree - EndAngle) / 180 * Math.PI * Radius);
                }
                if (tempPoint.X < this.GetMinX())
                {
                    this._minX = tempPoint.X;
                }
                if (tempPoint.X > this.GetMaxX())
                {
                    this._maxX = tempPoint.X;
                }
                if (tempPoint.Y < this.GetMinY())
                {
                    this._minY = tempPoint.Y;
                }
                if (tempPoint.Y > this.GetMaxY())
                {
                    this._maxY = tempPoint.Y;
                }
            }
        }

        public override PointGeo GetXDirectionAtLength(float length)
        {
            double durationAngle = length / (Radius * 2 * Math.PI) * 360;
            double curDegree = 0;
            if (!IsClockwise)
            {
                curDegree = StartAngle + durationAngle;
            }
            else
            {
                curDegree = EndAngle - durationAngle;
            }
            return new PointGeo((float)(CenterX + Radius * Math.Cos(Math.PI / 180 * curDegree)),
                (float)(CenterY + Radius * Math.Sin(Math.PI / 180 * curDegree)));
        }

        internal override double GetPerimeter()
        {
            return Radius * (EndAngle - StartAngle) / 180 * Math.PI;
        }
        public new void Translate(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }
            //this.StartPoint += offset;
            //this.EndPoint += offset;
            this.StartPoint += offset;
            this.EndPoint += offset;
            this.CenterX += offset.X;
            this.CenterY += offset.Y;
        }
        public new void Scale(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }

        }
    }
    [Serializable]
    public class LineGeo : Shape
    {
        public LineGeo()
        {
        }
        public LineGeo(PointGeo s, PointGeo e)
            : base(s, e)
        {

        }
        public new void Translate(PointGeo offset)
        {
            this.StartPoint += offset;
            this.EndPoint += offset;
        }
        public static float MAX_SLOP = 66666;
        public static float INF = 1E20f;
        public float GetK()
        {
            if (Math.Abs(StartPoint.X - EndPoint.X) < 0.001)
                return MAX_SLOP;
            float tmpK = (EndPoint.Y - StartPoint.Y) / (EndPoint.X - StartPoint.X);
            return Math.Abs(tmpK) > MAX_SLOP ? MAX_SLOP : tmpK;
        }
        public double GetB()
        {
            if (Math.Abs(GetK() - MAX_SLOP) < Math.Abs(GetK() / 1000))
                return INF;
            return StartPoint.Y + GetK() * (0 - StartPoint.X);

        }
        public static int DISPERSED_COUNT = 500;
        public override void DispersedStartWithRightSide()
        {
            dispersedPoints.Clear();
            PerimeterList.Clear();
            PointGeo tmp = StartPoint.Clone();
            for (int i = 0; i < DISPERSED_COUNT; i++)
            {
                tmp.X = StartPoint.X - (EndPoint.X - StartPoint.X) * i / DISPERSED_COUNT;
                tmp.Y = StartPoint.Y - (EndPoint.Y - StartPoint.Y) * i / DISPERSED_COUNT;
                this.dispersedPoints.Add(tmp.Clone());
                this.PerimeterList.Add(this.GetLineLength() / DISPERSED_COUNT * i);
            }
        }

        public override void Draw()
        {
            if (this.drawMata == null)
                return;
            if (Painter != null)
            {
                Painter.DrawLine(this);
                MSG = String.Format("按住Shift 画垂直水平线; Ctrl 连续画线，线条长度： {0}， dx={1} ,dy={2}, 角度={3}", GetLineLength().ToString("F2"), (EndPoint.X - StartPoint.X).ToString("F2"), (EndPoint.Y - StartPoint.Y).ToString("F2"), (Math.Atan(GetK()) / Math.PI * 180).ToString("f2"));
                //OnUpdateMessage(msg);
            }
        }

        private double GetLineLength()
        {
            return Math.Sqrt((StartPoint.X - EndPoint.X) * (StartPoint.X - EndPoint.X) + (StartPoint.Y - EndPoint.Y) * (StartPoint.Y - EndPoint.Y));
        }

        public override PointGeo GetXDirectionAtLength(float length)
        {
            return new PointGeo((float)(StartPoint.X + (EndPoint.X - StartPoint.X) * length / GetLineLength()),
                (float)(StartPoint.Y + (EndPoint.Y - StartPoint.Y) * length / GetLineLength()));
        }

        internal override double GetPerimeter()
        {
            return GetLineLength();
        }
    }
    [Serializable]
    public class PolygonGeo : RandomLines
    {
        public PolygonGeo()
        { 
        }
        //这个没办法，一定要通过自己去调用AddPoint 添加新的点
        public void AddPoint(PointGeo point)
        {
            if (!this.points.Contains(point))
            {
                this.points.Add(point);
                this.pointF.Add(new PointF(point.X, point.Y));
            }
        }
        public override void AddPoints(List<PointGeo> ps)
        {
            if (MaxCount > 0 && ps.Count > MaxCount)
            {
                return;
            }
            foreach (var item in ps)
            {
                AddPoint(item);
            }            
        }

        public override void Draw(PainterBase Painter)
        {
            if (GetDrawMeta() == null)
                return;
            Painter.DrawPolygon(this);
        }
        private List<PointF> pointF = new List<PointF>();
        public PointF[] GetPointF()
        {
            return this.pointF.ToArray();
        }

    }
    [Serializable]
    public class TriangleGeo : PolygonGeo
    {
        public TriangleGeo()
        {
            MaxCount = 3;
        } 
    }
    [Serializable]
    public class RandomLines : IScreenPrintable
    {
        public void ResetPoints()
        {
            foreach (var element in this.points)
            {
                element.IsNeedDraw = true;
            }
        }

        public int MaxCount = -1;
        public virtual void Draw(PainterBase Painter)
        {
            if (pointsMeta == null)
            {
                pointsMeta = new ShapeMeta();
                this.pointsMeta.LineWidth = 1;
                this.pointsMeta.ForeColor = Color.Black;
                this.pointsMeta.DashLineStyle = new float[] { 2f, 4f, 1f };
                pointsMeta.CapStyle = System.Drawing.Drawing2D.LineCap.Round;
            }

            if (Painter == null)
                return;
            for (int i = 0; i < this.points.Count - 1; i++)
            {
                if (this.points[i] == null || this.points[i + 1] == null)
                {
                    continue;
                }
                if (this.points[i].IsNeedDraw)
                {
                    LineGeo line = new LineGeo(this.points[i], points[i + 1]);
                    line.SetDrawMeta(pointsMeta);
                    Painter.DrawLine(line);
                    if (IsHold)
                    {
                        this.points[i].IsNeedDraw = false;
                    }

                }
            }
        }
        //是否仅可以不刷新，false就每次Invalidate都刷新
        public bool IsHold = true;
        public void Clear()
        {
            this.points.Clear();
        }
        public ShapeMeta GetDrawMeta()
        {
            return this.pointsMeta;
        }
        public void SetDrawMeta(ShapeMeta dm)
        {
            this.pointsMeta = dm;
        }
        private ShapeMeta pointsMeta;
        public virtual void AddPoints(List<PointGeo> ps)
        {
            if (MaxCount > 0 && ps.Count > MaxCount)
            {
                return;
            }
            points.AddRange(ps);
        }
        public void SetPoints(List<PointGeo> ps)
        {
            this.points = ps;
        }
        public List<PointGeo> points = new List<PointGeo>();
    }
    [Serializable]
    public class DrawableImage : IScreenPrintable
    {
        public void Draw(PainterBase Painter)
        {
            if (Painter != null)
            {
                Painter.DrawImage(FilePath);
            }
        }
        public string FilePath;

    }
    [Serializable]
    public class DrawableText : IScreenPrintable, IOperationable
    {
        public void Draw(PainterBase dp)
        {
            if (this.pm == null)
                return;
            if (dp == null)
                return;
            dp.DrawText(this);
        }
        public void SetDrawMeta(TextMeta dm)
        {
            this.pm = dm;
        }
        public TextMeta GetTextMeta()
        {
            return this.pm;
        }

        public void Translate(PointGeo offset)
        {
            this.pos.Offset(offset);
        }

        public void Rotate()
        {
            throw new NotImplementedException();
        }

        public void Scale(PointGeo p)
        {
            this.pos.Scale(p);
        }

        public PointGeo pos { get; set; }
        private TextMeta pm;
    }
    //连续加工的最小单位
    public class Geo
    {
        double _premiter = 0;
        public void Draw(PainterBase painter)
        {
            foreach (var element in this.shapes)
            {
                element.Draw(painter);
            }
        }
        public double GetPerimeter()
        {
            return this._premiter;
        }
        public Geo()
        {
            MaxX = 0;
            MaxY = 0;
            MinX = 1E6f;
            MinY = 1E6f;
        }
        private List<Shape> shapes = new List<Shape>();
        public void AddShape(Shape shape)
        {
            if (!shapes.Contains(shape))
            {
                shapes.Add(shape);
                if (this.MinX > shape.GetMinX())
                {
                    this.MinX = shape.GetMinX();
                }
                if (this.MaxX < shape.GetMaxX())
                {
                    this.MaxX = shape.GetMaxX();
                }
                if (this.MinY > shape.GetMinY())
                {
                    this.MinY = shape.GetMinY();
                }
                if (this.MaxY < shape.GetMaxY())
                {
                    this.MaxY = shape.GetMaxY();
                }

                _premiter += shape.GetPerimeter();
            }
        }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MinX { get; set; }
        public float MinY { get; set; }
        public void ClearShape()
        {
            this.shapes.Clear();
        }
        public List<Shape> GetShapes()
        {
            return this.shapes;
        }
    }

}