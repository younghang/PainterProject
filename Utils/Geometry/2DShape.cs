/*
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
using System.Windows.Forms;
using Utils;

namespace Painter.Models
{
    /// <summary>
    /// Description of Shape.
    /// </summary>
    public interface IOperationable
    {
        void Translate(PointGeo offset);
        void Rotate(float degree, PointGeo center = null);
        void Scale(PointGeo offset);
    }
    public interface IPrintAndOperatable : IScreenPrintable, IOperationable
    {

    }
    [Serializable]
    public abstract class DrawableObject : IPrintAndOperatable
    {
        public abstract List<PointGeo> GetControlPoints();
        public abstract void Draw(PainterBase pb);
        public virtual void Rotate(float degree, PointGeo center = null)
        {
            this.Angle += degree;
        }
        public abstract void Scale(PointGeo offset);
        public abstract void Translate(PointGeo offset);
        public bool IsShow = true;
        public bool IsDisposed = false;
        public bool IsInVision = true;//减少gdi刷新的图像数量，超过视野的设置为不可看见
        public float Angle = 0;//定义旋转中心为左上角 旋转的矩形,顺时针为正
        public abstract DrawMeta GetDrawMeta();
        public abstract void SetDrawMeta(DrawMeta draw);

        public virtual bool IsOverlap(DrawableObject other)
        {
            return false;
        }

        public abstract void Initial() ;
        
    }

    [Serializable]
    public class PointGeo
    {
        public float X;

        public float Y;
        public float x { get { return X; } }
        public float y { get { return Y; } }
        public bool IsNeedDraw = true;
        public static PointGeo zero { get { return new PointGeo(0,0); } } 

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
        public float DistanceTo(PointGeo p)
        {
            return (this - p).GetLength();
        }
        public float Dot(PointGeo p)
        {
            return this.X * p.X + this.Y * p.Y;
        }
        public float Cross(PointGeo p)
        {
            return this.X * p.Y - p.X * this.Y;
        }
        public PointGeo Offset(PointGeo p)
        {
            this.X += p.X;
            this.Y += p.Y;
            return this;
        }
        public PointGeo(PointGeo p)
        {
            this.X = p.X;
            this.Y = p.Y;
        }

        public void Scale(PointGeo p)
        {
            this.X *= p.X;
            this.Y *= p.Y;
        }
        public float GetLength()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y);
        }
        public PointGeo Abs()
        {
            return new PointGeo(Math.Abs(X), Math.Abs(Y));
        }
        public override string ToString()
        {
            return "(" + this.X.ToString("f3") + ", " + this.Y.ToString("f3") + ")";
        }
        public void Normalized()
        {
            float l = GetLength();
            this.X = X / l;
            this.Y = Y / l;
        }
        public void RotateAroundOrigin(float degree, PointGeo refOrigin)
        {
            double rad = degree / 180 * Math.PI;
            float tempX, tempY;
            PointGeo deltaP = this - refOrigin;
            tempX = (float)((deltaP.X) * Math.Cos(rad) - (deltaP.Y) * Math.Sin(rad));
            tempY = (float)((deltaP.X) * Math.Sin(rad) + (deltaP.Y) * Math.Cos(rad));
            X = tempX + refOrigin.X;
            Y = tempY + refOrigin.Y;
        }
    }

    [Serializable]
    public abstract class Shape : DrawableObject
    {
        public override List<PointGeo> GetControlPoints()
        {
            return new List<PointGeo>() { this.FirstPoint,this.SecondPoint };
        }
        public enum SHAPE_TYPE { SHAPE, CIRCLE, ELLIPSE, RECTANGLE, ARC, LINE }
        public SHAPE_TYPE ShapeType = SHAPE_TYPE.SHAPE;

        public bool IsFinishedInitial = false;//减少离散点计算 

        public bool IsNonTrans = false;//temp use, delete sooon
        protected PointGeo shapeCenter;
        [NonSerialized]
        protected PainterBase Painter;
        public virtual void SetPainter(PainterBase painter)
        {
            this.Painter = painter;
        }
        public virtual float GetArea()
        {
            return this.Width * this.Heigth;
        }
        public virtual PointGeo GetShapeCenter()
        {
            shapeCenter = (this.FirstPoint + this.SecondPoint) / 2;

            return shapeCenter;
        }
        public string MSG { get; set; }
        private PointGeo _start = new PointGeo(0, 0);
        private PointGeo _end = new PointGeo(0, 0);
        public PointGeo FirstPoint//这是圆心，Radius只读，写不进去的
        {
            get { return _start; }
            set
            {
                this._start = value;
                Initial();
            }
        }
        public PointGeo SecondPoint
        {
            get { return _end; }
            set
            {
                this._end = value;
                Initial();
            }
        } 
        public override bool IsOverlap(DrawableObject shape)
        {
            return false;
        }

        protected double _minX = 1E6;
        protected double _minY = 1E6;
        protected double _maxX;
        protected double _maxY;
        public virtual float GetMaxY()
        {
            return Math.Max(this.FirstPoint.Y, this.SecondPoint.Y);
        }
        public virtual float GetMinY()
        {
            return Math.Min(this.FirstPoint.Y, this.SecondPoint.Y);
        }
        public virtual float GetMaxX()
        {
            return Math.Max(this.FirstPoint.X, this.SecondPoint.X);
        }
        public virtual float GetMinX()
        {
            return Math.Min(this.FirstPoint.X, this.SecondPoint.X);
        }
        public float Width
        {
            get;
            set;
        }
        public float Heigth
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
            this.FirstPoint = s;
            this.SecondPoint = e;
        }
        public override void Initial()
        {
            this.Width = Math.Abs(FirstPoint.X - SecondPoint.X);
            this.Heigth = Math.Abs(FirstPoint.Y - SecondPoint.Y);
        }
        public override void SetDrawMeta(DrawMeta dm)
        {
            this.drawMata = dm as ShapeMeta;
        }
        public Shape SetDrawMeta(ShapeMeta dm)
        {
            this.drawMata = dm as ShapeMeta;
            return this;
        }
        public override DrawMeta GetDrawMeta()
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
        public override void Translate(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }
            this.FirstPoint = this.FirstPoint + offset;
            this.SecondPoint = this.SecondPoint + offset;
        }
        public override void Rotate(float degree, PointGeo center)
        {

        }
        public override void Scale(PointGeo scale)
        {
            this.FirstPoint.Scale(scale);
            this.SecondPoint.Scale(scale);
        }
        public abstract void Draw();
        public override void Draw(PainterBase p)
        {
            //			if (this.Painter==null||this.Painter!=p) {就是这个导致没画出来图，搞了两个小时
            //
            //			}
            if (!IsShow || IsDisposed)
            {
                return;
            }
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
        public abstract double GetPerimeter();

        protected List<PointGeo> dispersedPoints = new List<PointGeo>();
        protected List<double> PerimeterList = new List<double>();


    }
    [Serializable]
    public class RectangleGeo : Shape
    {
        public override float GetMaxY()
        {
            return Math.Max(this.FirstPoint.Y, this.SecondPoint.Y);
        }
        public override float GetMinY()
        {
            return Math.Min(this.FirstPoint.Y, this.SecondPoint.Y);
        }
        public override float GetMaxX()
        {
            return Math.Max(this.FirstPoint.X, this.SecondPoint.X);
        }
        public override float GetMinX()
        {
            return Math.Min(this.FirstPoint.X, this.SecondPoint.X);
        }
        public RectangleGeo()
        {
            ShapeType = SHAPE_TYPE.RECTANGLE;

        }
        public RectangleGeo(PointGeo s, PointGeo e) : base(s, e)
        {
            ShapeType = SHAPE_TYPE.RECTANGLE;
        }
        public override void Rotate(float degree, PointGeo center)
        {
            Angle += degree;
        }
        
        public override void Initial()
        {
            base.Initial();
            LeftCorner = (int)Math.Min(SecondPoint.X, FirstPoint.X);
            TopCorner = (int)Math.Max(SecondPoint.Y, FirstPoint.Y);
        }
        public override void Draw()
        {
            float num = SecondPoint.X - FirstPoint.X;
            float num2 = SecondPoint.Y - FirstPoint.Y;
            MSG = String.Format("当前坐标：({0},{1})--坐标偏移量：({2},{3})", SecondPoint.X, SecondPoint.Y, num, num2);
            if (this.drawMata == null)
                return;
            if (Painter != null)
                Painter.DrawRectangle(this);
        }
        public override bool IsOverlap(DrawableObject other)
        {
            if (other is RectangleGeo)
            {
                RectangleGeo otherRec = other as RectangleGeo;
                PointGeo leftTop1 = new PointGeo(this.GetMinX(), this.GetMinY());
                PointGeo leftTop2 = new PointGeo(otherRec.GetMinX(), otherRec.GetMinY());
                if (Math.Abs((leftTop1.X + this.Width / 2) - (leftTop2.X + otherRec.Width / 2)) < (this.Width + otherRec.Width) / 2)
                {
                    if (Math.Abs((leftTop1.Y + this.Heigth / 2) - (leftTop2.Y + otherRec.Heigth / 2)) < (this.Heigth + otherRec.Heigth) / 2)
                    {
                        return true;
                    }
                }
            }
            if (other is CircleGeo)
            {
                CircleGeo otherCircle = other as CircleGeo;
                if (this.drawMata != null && this.drawMata.IsFill)
                {

                    PointGeo leftTop1 = new PointGeo(this.GetMinX(), this.GetMaxY());
                    PointGeo circleToLeftTop = new PointGeo(otherCircle.CenterX - leftTop1.X, otherCircle.CenterY - leftTop1.Y);
                    CommonUtils.PointRotateAroundOrigin(Angle, ref circleToLeftTop.X, ref circleToLeftTop.Y);
                    PointGeo nearestPointOnRectToCircle = new PointGeo();
                    PointGeo centerRelativeToRectCenter = new PointGeo(-this.Width / 2, this.Heigth / 2) + circleToLeftTop;
                    //以Rect的中心为原点，矩形上离圆的最近的点必然受限制，在矩形内，即-width/2<x<width/2；
                    nearestPointOnRectToCircle.X = Math.Min(centerRelativeToRectCenter.X, this.Width / 2);
                    nearestPointOnRectToCircle.X = Math.Max(nearestPointOnRectToCircle.X, -this.Width / 2);
                    nearestPointOnRectToCircle.Y = Math.Min(centerRelativeToRectCenter.Y, this.Heigth / 2);
                    nearestPointOnRectToCircle.Y = Math.Max(nearestPointOnRectToCircle.Y, -this.Heigth / 2);
                    PointGeo nearestPointRelateiveToCircleCenter = nearestPointOnRectToCircle - centerRelativeToRectCenter;
                    if (nearestPointRelateiveToCircleCenter.X * nearestPointRelateiveToCircleCenter.X + nearestPointRelateiveToCircleCenter.Y * nearestPointRelateiveToCircleCenter.Y < otherCircle.Radius * otherCircle.Radius)
                    {
                        return true;
                    }
                }
                else
                {
                    LineGeo line = new LineGeo();
                    line.FirstPoint = FirstPoint;
                    line.SecondPoint = FirstPoint + new PointGeo(SecondPoint.X - FirstPoint.X, 0);
                    if (otherCircle.IsOverlap(line))
                    {
                        return true;
                    }
                    line.FirstPoint = FirstPoint;
                    line.SecondPoint = FirstPoint + new PointGeo(0, SecondPoint.Y - FirstPoint.Y);
                    if (otherCircle.IsOverlap(line))
                    {
                        return true;
                    }
                    line.FirstPoint = SecondPoint;
                    line.SecondPoint = SecondPoint + new PointGeo(FirstPoint.X - SecondPoint.X, 0);
                    if (otherCircle.IsOverlap(line))
                    {
                        return true;
                    }
                    line.FirstPoint = SecondPoint;
                    line.SecondPoint = SecondPoint + new PointGeo(0, FirstPoint.Y - SecondPoint.Y);
                    if (otherCircle.IsOverlap(line))
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }


        public override void DispersedStartWithRightSide()
        {
            throw new NotImplementedException();
        }

        public override PointGeo GetXDirectionAtLength(float length)
        {
            throw new NotImplementedException();
        }

        public override double GetPerimeter()
        {
            return 2 * (GetMaxX() - GetMinX()) * (GetMaxY() - GetMinX());
        }

        public int LeftCorner { get; set; }
        public int TopCorner { get; set; }
    }
    [Serializable]
    public sealed class EllipseGeo : RectangleGeo
    {
        public EllipseGeo()
        {
            ShapeType = SHAPE_TYPE.ELLIPSE;

        }
        public override void Draw()
        {
            float num = SecondPoint.X - FirstPoint.X;
            float num2 = SecondPoint.Y - FirstPoint.Y;
            MSG = String.Format("当前坐标：({0},{1})--坐标偏移量：({2},{3})", SecondPoint.X, SecondPoint.Y, num, num2);
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
            ShapeType = SHAPE_TYPE.CIRCLE;

        }
        public override List<PointGeo> GetControlPoints()
        {
            return new List<PointGeo>() { this.FirstPoint, this.SecondPoint  };
        }
        public override PointGeo GetShapeCenter()
        {
            return FirstPoint;
        }
        public override float GetArea()
        {
            return this.Radius * this.Radius * (float)Math.PI;
        }
        public CircleGeo(PointGeo s, PointGeo e)
        {
            this.FirstPoint = s;
            this.SecondPoint = e;
        }
        public override void Initial()
        {//是这样的，第一个点是圆心，第二个点决定半径
            this.Width = Math.Abs(FirstPoint.X - SecondPoint.X);
            this.Heigth = Math.Abs(FirstPoint.Y - SecondPoint.Y);
            CenterX = FirstPoint.X;
            CenterY = FirstPoint.Y;
            if (SecondPoint.X == 0 && SecondPoint.Y == 0)
            {
                //Radius自由定义
            }
            else
                Radius = (float)(Math.Sqrt(1.0 * (FirstPoint.X - SecondPoint.X) * (FirstPoint.X - SecondPoint.X) + 1.0 * (FirstPoint.Y - SecondPoint.Y) * (FirstPoint.Y - SecondPoint.Y)));
            this.Width = 2 * Radius;
            this.Heigth = 2 * Radius;
        }
        public override void Translate(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }
            this.FirstPoint += offset;//会触发Init方法，而重写的Init里面会更新CenterX
            if (SecondPoint.X == 0 && SecondPoint.Y == 0)
            {
                //Radius自由定义
            }
            else
            {
                //由此定义Radius
                this.SecondPoint += offset;
            }
            //this.CenterX += offset.X;
            //this.CenterY += offset.Y;
        }
        public override float GetMaxX()
        {
            return (float)CenterX + Radius;
        }
        public override void Rotate(float degree, PointGeo center)
        {

        }
        public override float GetMaxY()
        {
            return (float)CenterY + Radius;
        }
        public override float GetMinX()
        {
            return (float)CenterX - Radius;
        }
        public override float GetMinY()
        {
            return (float)CenterY - Radius;
        }
        public override void Draw()
        {
            if (this.drawMata == null)
                return;

            if (Painter != null)
                Painter.DrawCircle(this);
            MSG = String.Format("圆心坐标：({0},{1})--半径：{2}", FirstPoint.X, FirstPoint.Y, Radius);
        }

        public override void DispersedStartWithRightSide()
        {
            return;
        }
        public override bool IsOverlap(DrawableObject other)
        {
            //注意每个分段必须return 如果不加else 的话
            if (other is RectangleGeo)
            {
                return other.IsOverlap(this);
            }
            if (other is CircleGeo)
            {
                CircleGeo otherCircle = other as CircleGeo;
                PointGeo centerToCenter = new PointGeo(this.CenterX - otherCircle.CenterX, this.CenterY - otherCircle.CenterY);
                if (otherCircle.drawMata != null && (otherCircle.GetDrawMeta() as ShapeMeta).IsFill)
                {
                    if (centerToCenter.X * centerToCenter.X + centerToCenter.Y * centerToCenter.Y < (this.Radius + otherCircle.Radius) * (this.Radius + otherCircle.Radius))
                    {
                        return true;
                    }
                }
                else
                {
                    if (centerToCenter.X * centerToCenter.X + centerToCenter.Y * centerToCenter.Y < (this.Radius + otherCircle.Radius) * (this.Radius + otherCircle.Radius) && (centerToCenter.X * centerToCenter.X + centerToCenter.Y * centerToCenter.Y) > (this.Radius - otherCircle.Radius) * (this.Radius - otherCircle.Radius))
                    {
                        return true;
                    }
                }
                return false;

            }
            if (other is LineGeo)
            {
                LineGeo line = other as LineGeo;
                PointGeo linevector = line.FirstPoint - line.SecondPoint;
                PointGeo lineFirstToCenterVector = this.FirstPoint - line.FirstPoint;
                PointGeo lineSecondToCenterVector = line.SecondPoint - this.FirstPoint;
                if (Math.Abs(lineSecondToCenterVector.GetLength()) < Radius)
                {
                    return true;
                }
                if (Math.Abs(lineFirstToCenterVector.GetLength()) < Radius)
                {
                    return true;
                }
                float distance = Math.Abs(linevector.Cross(lineFirstToCenterVector)) / linevector.GetLength();
                if (distance > this.Radius)
                {
                    return false;
                }
                float angle1 = linevector.Dot(-1 * lineFirstToCenterVector);
                float angle2 = -linevector.Dot(lineSecondToCenterVector);
                if (angle1 > 0 && angle2 > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            if (other is ArcGeo)
            {
                return other.IsOverlap(this);
            }
            if (other is RandomLines)
            {

                RandomLines lines = other as RandomLines;
                if (lines.GetDrawMeta() != null && (lines.GetDrawMeta() as ShapeMeta).IsFill)
                {//有填充
                    PointGeo point = this.FirstPoint;
                    int numCross = 0;
                    for (int i = 0; i < lines.points.Count; i++)
                    {
                        PointGeo p1 = lines.points[i];
                        PointGeo p2 = lines.points[(i + 1) % lines.points.Count];
                        if (p1.Y == p2.Y)
                        {
                            continue;
                        }
                        if (point.Y < Math.Min(p1.Y, p2.Y)) continue;
                        if (point.Y > Math.Max(p1.Y, p2.Y)) continue;
                        float x = (point.Y - p1.Y) * (p2.X - p1.X) / (p2.Y - p1.Y) + p1.X;
                        if (x == point.X)
                        {
                            return true;
                        }
                        if (x > point.X)
                        {
                            numCross++;
                        }
                    }
                    if (numCross % 2 == 1) return true;
                    else return false;
                }
                else//纯线框
                {
                    LineGeo line = new LineGeo();
                    for (int i = 0; i < lines.points.Count - 1; i++)
                    {
                        line.FirstPoint = lines.points[i];
                        line.SecondPoint = lines.points[i + 1];
                        if (line.IsOverlap(this))
                        {
                            return true;
                        }
                    }
                    if (other is PolygonGeo)
                    {
                        line.FirstPoint = lines.points[0];
                        line.SecondPoint = lines.points[lines.points.Count - 1];
                        if (line.IsOverlap(this))
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            if (other is DrawableText)
            {
                DrawableText text = other as DrawableText;
                Size size = TextRenderer.MeasureText(text.GetTextMeta().Text, text.GetTextMeta().TEXTFONT);
                RectangleGeo rectangle = new RectangleGeo(text.pos, text.pos + new PointGeo(size.Width, -size.Height));
                rectangle.SetDrawMeta(new ShapeMeta() { IsFill=true});
                return this.IsOverlap(rectangle);
            }
            if (other is DrawableImage)
            {
                DrawableImage image = other as DrawableImage; 
                RectangleGeo rectangle = new RectangleGeo(image.leftTop,image.RightBottom);
                rectangle.SetDrawMeta(new ShapeMeta() { IsFill = true });
                return this.IsOverlap(rectangle);
            }
            return false;
        }

    }
    [Serializable]
    public class ArcGeo : Shape
    { 
        public static int DISPERSED_COUNT = 500; 
        public ArcGeo()
        {
            ShapeType = SHAPE_TYPE.ARC;

        }
        public override List<PointGeo> GetControlPoints()
        {
            return new List<PointGeo>() { this.FirstPoint, this.SecondPoint,this.ThirdPoint};
        }
        public override bool IsOverlap(DrawableObject other)
        {
            if (other is CircleGeo)
            {
                CircleGeo circle = other as CircleGeo;
                PointGeo startP = new PointGeo((float)(CenterX + Radius * Math.Cos(Math.PI / 180 * StartAngle)),
                (float)(CenterY + Radius * Math.Sin(Math.PI / 180 * StartAngle)));
                PointGeo endP = new PointGeo((float)(CenterX + Radius * Math.Cos(Math.PI / 180 * EndAngle)),
                   (float)(CenterY + Radius * Math.Sin(Math.PI / 180 * EndAngle)));
                Func<PointGeo, bool> pIsInCircle = (point) =>
                {
                    PointGeo ptocenter = circle.FirstPoint - point;
                    if (ptocenter.GetLength() < circle.Radius)
                    {
                        return true;
                    }
                    return false;
                };
                if (pIsInCircle(startP))
                {
                    return true;
                }
                if (pIsInCircle(endP))
                {
                    return true;
                }
                PointGeo centerToCenter = circle.FirstPoint - new PointGeo(this.CenterX, this.CenterY);
                if (Math.Abs(centerToCenter.GetLength() - this.Radius) > circle.Radius)
                {
                    return false;
                }
                else
                {
                    double degree = 180 * Math.Atan2(centerToCenter.Y, centerToCenter.X) / Math.PI;
                    if (degree > StartAngle && degree < EndAngle)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;

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
                    _endAngle -= 360;
                }
                while (value < -180)
                {
                    value += 360;
                    _endAngle += 360;
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
                //if (_endAngle>360)
                //{

                //}
                DispersedStartWithRightSide();
            }
        }
        public override void Rotate(float degree, PointGeo center)
        {
            StartAngle -= degree;
            EndAngle -= degree;
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
       
        private PointGeo thirdp = new PointGeo();
        public PointGeo ThirdPoint
        {
            get { return thirdp; }
            set
            {
                this.thirdp = value;
                Initial();
            }
        }
        public PointGeo StartPos
        {
            get { 
                PointGeo center = new PointGeo(CenterX, CenterY);
                PointGeo center_front = new PointGeo(CenterX + Radius, CenterY);
                center_front.RotateAroundOrigin(StartAngle, center);
                 return center_front;
            } 
        }
        public PointGeo EndPos
        {
            get
            {
                PointGeo center = new PointGeo(CenterX, CenterY);
                PointGeo center_front = new PointGeo(CenterX + Radius, CenterY);
                center_front.RotateAroundOrigin(EndAngle, center);
                return center_front;
            }
        }
        public override void Initial()
        {
            this.CenterX = FirstPoint.X;
            this.CenterY = FirstPoint.Y;
            this.Radius = (FirstPoint - SecondPoint).GetLength();
            this.StartAngle = (float)(180 / Math.PI * Math.Atan2(SecondPoint.Y - FirstPoint.Y, SecondPoint.X - FirstPoint.X));
            this.EndAngle = (float)(180 / Math.PI * Math.Atan2(ThirdPoint.Y - FirstPoint.Y, ThirdPoint.X - FirstPoint.X));
            //纯粹为GeoControl而添加
            this.Width = (float)(_maxX - _minX);
            this.Heigth = (float)(_maxY - _minY); 
        }
        public override void Draw()
        {
            if (this.drawMata == null)
                return;
            if (Painter != null)
                Painter.DrawArc(this);
            MSG = String.Format("圆心：({0},{1})--半径：{2}，起始角{3},终止角{4},弧长{5}", CenterX, CenterY, Radius, StartAngle.ToString("f2"), EndAngle.ToString("f2"), GetPerimeter().ToString("f2"));

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

        public override double GetPerimeter()
        {
            return Radius * (EndAngle - StartAngle) / 180 * Math.PI;
        }
        public override void Translate(PointGeo offset)
        {
            if (IsNonTrans)
            {
                return;
            }
            //this.StartPoint += offset;
            //this.EndPoint += offset;
            //this.FirstPoint += offset;
            //this.SecondPoint += offset;
            //this.ThirdPoint += offset;
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
            ShapeType = SHAPE_TYPE.LINE;
        }
        public LineGeo(PointGeo s, PointGeo e)
            : base(s, e)
        {
            ShapeType = SHAPE_TYPE.LINE;
        }

        public override float GetArea()
        {
            return 0;
        }
        public override void Translate(PointGeo offset)
        {
            this.FirstPoint += offset;
            this.SecondPoint += offset;
        }
        public  void Rotate2(float degree, PointGeo center = null)
        {
            if (center == null)
            {
                center = FirstPoint.Clone();
            }
            float x = FirstPoint.X - center.X;
            float y = FirstPoint.Y - center.Y;
            CommonUtils.PointRotateAroundOrigin(-degree, ref x, ref y);
            FirstPoint.X = x + center.X;
            FirstPoint.Y = y + center.Y;
            x = SecondPoint.X - center.X;
            y = SecondPoint.Y - center.Y;
            CommonUtils.PointRotateAroundOrigin(-degree, ref x, ref y);
            SecondPoint.X = x + center.X;
            SecondPoint.Y = y + center.Y;
        }
        public override void Rotate(float degree, PointGeo center=null)
        {
            if (center == null)
            {
                center = this.FirstPoint.Clone();
            }
            this.FirstPoint.RotateAroundOrigin(-degree, center);
            this.SecondPoint.RotateAroundOrigin(-degree, center);
        }

        public static float MAX_SLOP = 66666;
        public static float INF = 1E20f;
        public float GetK()
        {
            if (Math.Abs(FirstPoint.X - SecondPoint.X) < 0.001)
                return MAX_SLOP;
            float tmpK = (SecondPoint.Y - FirstPoint.Y) / (SecondPoint.X - FirstPoint.X);
            return Math.Abs(tmpK) > MAX_SLOP ? MAX_SLOP : tmpK;
        }
        public float GetRad()
        {
            return (float)Math.Atan2(SecondPoint.Y - FirstPoint.Y, SecondPoint.X - FirstPoint.X);
        }
        public double GetB()
        {
            if (Math.Abs(GetK() - MAX_SLOP) < Math.Abs(GetK() / 1000))
                return INF;
            return FirstPoint.Y + GetK() * (0 - FirstPoint.X);

        }
        public static int DISPERSED_COUNT = 500;
        public override void DispersedStartWithRightSide()
        {
            dispersedPoints.Clear();
            PerimeterList.Clear();
            PointGeo tmp = FirstPoint.Clone();
            for (int i = 0; i < DISPERSED_COUNT; i++)
            {
                tmp.X = FirstPoint.X - (SecondPoint.X - FirstPoint.X) * i / DISPERSED_COUNT;
                tmp.Y = FirstPoint.Y - (SecondPoint.Y - FirstPoint.Y) * i / DISPERSED_COUNT;
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
                MSG = String.Format("按住Shift 画垂直水平线; Ctrl 连续画线，线条长度： {0}， dx={1} ,dy={2}, 角度={3}", GetLineLength().ToString("F2"), (SecondPoint.X - FirstPoint.X).ToString("F2"), (SecondPoint.Y - FirstPoint.Y).ToString("F2"), (Math.Atan(GetK()) / Math.PI * 180).ToString("f2"));
                //OnUpdateMessage(msg);
            }
        }

        private double GetLineLength()
        {
            return Math.Sqrt((FirstPoint.X - SecondPoint.X) * (FirstPoint.X - SecondPoint.X) + (FirstPoint.Y - SecondPoint.Y) * (FirstPoint.Y - SecondPoint.Y));
        }

        public override PointGeo GetXDirectionAtLength(float length)
        {
            return new PointGeo((float)(FirstPoint.X + (SecondPoint.X - FirstPoint.X) * length / GetLineLength()),
                (float)(FirstPoint.Y + (SecondPoint.Y - FirstPoint.Y) * length / GetLineLength()));
        }
        public override bool IsOverlap(DrawableObject shape)
        {
            if (shape is CircleGeo)
            {
                return shape.IsOverlap(this);
            }
            return base.IsOverlap(shape);
        }
        public override double GetPerimeter()
        {
            return GetLineLength();
        }
    }
    [Serializable]
    public class CurveGeo : PolygonGeo
    {
        public override void Draw(PainterBase Painter)
        {
            if (!IsShow)
            {
                return;
            }
            if (GetDrawMeta() == null)
                return;
            Painter.DrawCurve(this);
        }
    }
    [Serializable]
    public class PolygonGeo : RandomLines
    {
        public PolygonGeo()
        {
        }
        public override void Initial()
        {
            this.pointF.Clear();
            foreach (var item in this.points)
            {
                pointF.Add(new PointF(item.X,item.Y));
            }
        }

        public override void Rotate(float degree, PointGeo center = null)
        {
            base.Rotate(degree, center);
            for (int i = 0; i < this.points.Count; i++)
            {
                pointF[i]  = new PointF(this.points[i].X,this.points[i].Y); 
            }
        }
        public override void SamplePointByStep(double ratio)
        {
            int countSpan = (int)(this.points.Count * ratio);
            if (countSpan<1)
            {
                countSpan = 1;
            }
            for (int i = this.points.Count - 1; i >= 0; i--)
            {
                if (i % countSpan != 0)
                {
                    this.points.RemoveAt(i);
                    this.pointF.RemoveAt(i);
                }
            }
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
        public override void Translate(PointGeo offset)
        {
            base.Translate(offset);
            this.pointF.Clear();
            for (int i = 0; i < this.points.Count; i++)
            {
                this.pointF.Add(new PointF(points[i].X, points[i].Y));
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
            if (!IsShow)
            {
                return;
            }
            if (GetDrawMeta() == null)
                return;
            Painter.DrawPolygon(this);
        }
        protected List<PointF> pointF = new List<PointF>();
        public PointF[] GetPointF()
        {
            return this.pointF.ToArray();
        }

        public void RemovePoint()
        {
            this.points.RemoveAt(this.points.Count - 1);
            this.pointF.RemoveAt(this.pointF.Count - 1);
        }
         
    }
    [Serializable]
    public class RoundRec : PolygonGeo
    {
        public RoundRec()
        {
            MaxCount = 4;
        }
        private PointGeo first = new PointGeo();
        public PointGeo FirstPoint
        {
            get { return first; }
            set
            {
                first = value;
                // Init();
            }
        }
        private PointGeo second = new PointGeo();
        public PointGeo SecondPoint
        {
            get { return second; }
            set
            {
                second = value;
                //Init();
            }
        }
        private PointGeo third = new PointGeo();
        public PointGeo ThirdPoint
        {
            get { return third; }
            set
            {
                third = value;
                Init();
            }
        }
        public override void Draw(PainterBase Painter)
        {
            if (!IsShow)
            {
                return;
            }
            if (GetDrawMeta() == null)
                return; 
        }
        private void Init()
        {
            this.points.Clear();
            this.pointF.Clear();
            float Width = Math.Abs(SecondPoint.X - first.X);
            float Height = Math.Abs(second.Y - first.Y);
            float Radius = Math.Abs(ThirdPoint.Y - SecondPoint.Y);
            if (Radius > Height / 2)
            {
                Radius = Height / 2;
            }
            if (Radius > Width / 2)
            {
                Radius = Width / 2;
            }
            float MaxX = Math.Max(first.X, second.X);
            float MinX = Math.Min(first.X, second.X);
            float MaxY = Math.Max(first.Y, second.Y);
            float MinY = Math.Min(first.Y, second.Y);
            AddPoint(new PointGeo(MinX + Radius, MinY));
            AddPoint(new PointGeo(MaxX - Radius, MinY));
            AddPoint(new PointGeo(MaxX, MinY + Radius));
            AddPoint(new PointGeo(MaxX, MaxY - Radius));
            AddPoint(new PointGeo(MaxX - Radius, MaxY));
            AddPoint(new PointGeo(MinX + Radius, MaxY));
            AddPoint(new PointGeo(MinX, MaxY - Radius));
            AddPoint(new PointGeo(MinX, MinY + Radius));
            //AddPoint(new PointGeo(MinX + Radius, MinY)); 

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
    public class RandomLines : DrawableObject
    {
        public override List<PointGeo> GetControlPoints()
        {
            return this.points;
        }
        //这个没办法，一定要通过自己去调用AddPoint 添加新的点
        public void AddPoint(PointGeo point)
        {
            if (!this.points.Contains(point))
            {
                this.points.Add(point); 
            }
        }
        public  virtual void SamplePointByStep(double ratio)
        {
            int countSpan = (int)(this.points.Count * ratio);
            if (countSpan < 1)
            {
                countSpan = 1;
            }
            for (int i = this.points.Count-1; i >=0; i--)
            {
                if (i%countSpan!=0)
                {
                    this.points.RemoveAt(i);
                } 
            }
        }
        
        public void ResetPoints()
        {
            foreach (var element in this.points)
            {
                element.IsNeedDraw = true;
            }
        }
        public int MaxCount = -1;
        public override void Draw(PainterBase Painter)
        {
            if (!IsShow)
            {
                return;
            }
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
            for (int i = this.points.Count - 2; i >= 0; i--)
            {
                try
                {
                    if (this.points.Count < 1 || this.points[i] == null || this.points[i + 1] == null)
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
                catch (Exception)//此集合存在删除元素和遍历元素为不同线程的情况，
                {
                    //throw;
                }

            }
        }
        //是否仅可以不刷新，false就每次Invalidate都刷新
        public bool IsHold = true;
        public void Clear()
        {
            this.points.Clear();
        }
        public override DrawMeta GetDrawMeta()
        {
            return this.pointsMeta;
        }
        public override void SetDrawMeta(DrawMeta dm)
        {
            this.pointsMeta = dm as ShapeMeta;
        }
        protected ShapeMeta pointsMeta;
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

        public override void Translate(PointGeo offset)
        {
            for (int i = 0; i < this.points.Count; i++)
            {
                this.points[i] += offset;
            }
        }

        public override void Rotate(float degree, PointGeo center = null)
        {
            foreach (var item in this.points)
            {
                item.RotateAroundOrigin(-degree, center);
            }
        }

        public override void Scale(PointGeo offset)
        {

        }

        public override void Initial()
        {
            
        }

        public List<PointGeo> points = new List<PointGeo>();

    }
    [Serializable]
    public class DrawableImage : DrawableObject
    {
       
        
        private PointGeo _start = new PointGeo(0, 0);
        private PointGeo _end = new PointGeo(0, 0);
        public PointGeo leftTop 
        {
            get { return _start; }
            set
            {
                this._start = value;
                Initial();
            }
        }
        public PointGeo RightBottom
        {
            get { return _end; }
            set
            {
                this._end = value;
                Initial();
            }
        }
        public float Width
        {
            get;
            set;
        }
        public float Height
        {
            get;
            set;
        }
        public override List<PointGeo> GetControlPoints()
        {
            return new List<PointGeo>() { this.leftTop,this.RightBottom };
        }
        public override void Draw(PainterBase Painter)
        {
            if (Painter != null)
            {
                Painter.DrawImage(this);
            }
        }
         

        public override void Scale(PointGeo offset)
        {
             
        }

        public override void Translate(PointGeo offset)
        {
            this.leftTop += offset;
            this.RightBottom += offset;
        }

        public override DrawMeta GetDrawMeta()
        {
            return this.imageMeta;
        } 
        public override void SetDrawMeta(DrawMeta draw)
        {
            imageMeta = draw as ImageMeta; 
        }
        private ImageMeta imageMeta;
        
        public override void Initial()
        {
            this.Width = Math.Abs(leftTop.X - RightBottom.X);
            this.Height = Math.Abs(leftTop.Y - RightBottom.Y); 
        } 
       
    }
    [Serializable]
    public class DrawableText : DrawableObject
    {
        public override List<PointGeo> GetControlPoints()
        {
            return new List<PointGeo>() { this.pos };
        }
        public override void Draw(PainterBase dp)
        {
            if (!IsShow)
            {
                return;
            }
            if (this.pm == null)
                return;
            if (dp == null)
                return;
            dp.DrawText(this);
        }
        public override void SetDrawMeta(DrawMeta dm)
        {
            this.pm = dm as TextMeta;
        }
        public TextMeta GetTextMeta()
        {
            return this.pm;
        }
        public override DrawMeta GetDrawMeta()
        {
            return this.pm;
        }
        public override void Translate(PointGeo offset)
        {
            this.pos.Offset(offset);
        }
         
        public override void Scale(PointGeo p)
        {
            this.pos.Scale(p);
        }

        public override void Initial()
        {
            
        }

        public PointGeo pos { get; set; }
        private TextMeta pm;
    }
    [Serializable]
    public class PathGeo : RandomLines
    {
        public PathGeo()
        {
            MaxX = 0;
            MaxY = 0;
            MinX = 1E6f;
            MinY = 1E6f;
        }
        public void ClearShape()
        {
            this.shapes.Clear();
        }
        public List<Shape> GetShapes()
        {
            return this.shapes;
        }
        private List<Shape> shapes = new List<Shape>();
        public void AddShape(Shape shape, bool isReverse = false)
        {
            if (!shapes.Contains(shape))
            {
                if (isReverse)
                {
                    shapes.Insert(0, shape);
                }
                else
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
        public override void Draw(PainterBase Painter)
        {
            if (!IsShow)
            {
                return;
            }
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
            Painter.DrawPath(this);
        }
        double _premiter = 0;
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MinX { get; set; }
        public float MinY { get; set; }
        public void Show()
        {
            foreach (var item in this.shapes)
            {
                item.IsShow = true;
            }
        }

        public void Hide()
        {
            foreach (var item in this.shapes)
            {
                item.IsShow = false;
            }
        }
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
        public void AddShape(Shape shape, bool isReverse = false)
        {
            if (!shapes.Contains(shape))
            {
                if (isReverse)
                {
                    shapes.Insert(0, shape);
                }
                else
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

        public void Show()
        {
            foreach (var item in this.shapes)
            {
                item.IsShow = true;
            }
        }

        public void Hide()
        {
            foreach (var item in this.shapes)
            {
                item.IsShow = false;
            }
        }
    }

}