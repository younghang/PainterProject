using Painter.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

using System.Windows.Controls;

using System.Windows.Shapes;
using Painter.Painters;
using System.Drawing.Text;

namespace Painter.Painters
{
    public abstract class PainterBase
    {
        public PointGeo Scale = new PointGeo(1, 1);
        //先缩放，后平移
        public PointGeo OffsetPoint = new PointGeo(0, 0);
        public virtual void Dispose()
        {

        }

        public virtual void Clear(System.Drawing.Color color)
        {

        }
        public abstract void DrawRectangle(RectangeGeo rect);
        public abstract void DrawLine(LineGeo line);
        public abstract void DrawCircle(CircleGeo circle);
        public abstract void DrawEllipse(EllipseGeo ellipse);
        //绘制圆弧只管起始角度终止角度，不管你从那边走向哪边
        public abstract void DrawArc(ArcGeo arc);
        public virtual void DrawText(DrawableText text)
        {
        }
        public virtual void DrawImage(string filePath, int x = 0, int y = 0)
        {
        }
        public virtual void DrawMultiLines(RandomLines randomLines)
        {
        }

        public virtual void DrawPolygon(PolygonGeo polygonGeo) { }
    }
    public abstract class WPFPainter : PainterBase
    {
    }
    public class ContextPainter : WPFPainter
    {
        private DrawingContext drawingContext;
        public ContextPainter(DrawingContext drawingContext)
        {
            this.drawingContext = drawingContext;
        }



        public override void DrawLine(LineGeo line)
        {
            DrawMeta meta = line.GetDrawMeta();
            if (meta == null)
            {
                return;
            }
            if (drawingContext != null)
            {
                ShapeMeta dm = line.GetDrawMeta() as ShapeMeta;
                System.Windows.Media.Color color = new System.Windows.Media.Color();
                color.A = dm.ForeColor.A;
                color.R = dm.ForeColor.R;
                color.G = dm.ForeColor.G;
                color.B = dm.ForeColor.B;

                drawingContext.DrawLine(new System.Windows.Media.Pen(
                    new SolidColorBrush(color), dm.LineWidth),
                    new System.Windows.Point(line.FirstPoint.X, line.FirstPoint.Y), new System.Windows.Point(line.SecondPoint.X, line.SecondPoint.Y));
            }
        }

        public override void DrawMultiLines(RandomLines rl)
        {
            throw new NotImplementedException();
        }

        public override void DrawRectangle(RectangeGeo rect)
        {
            return;
        }
        public override void DrawCircle(CircleGeo circle)
        {
            throw new NotImplementedException();
        }
        public override void DrawEllipse(EllipseGeo ellipse)
        {
            throw new NotImplementedException();
        }
        public override void DrawArc(ArcGeo arc)
        {
            throw new NotImplementedException();
        }

    }
    public class CanvasPainter : WPFPainter
    {
        private System.Windows.Controls.Canvas mCanvas;
        public CanvasPainter(Canvas canvas)
        {
            this.mCanvas = canvas;
        }
        public CanvasPainter()
        {

        }
        public override void DrawLine(LineGeo line)
        {
            DrawMeta dm = line.GetDrawMeta();
            if (dm == null)
            {
                return;
            }
            if (mCanvas != null)
            {
                Line mydrawline = new Line();
                System.Windows.Media.Color color = new System.Windows.Media.Color();
                color.A = dm.ForeColor.A;
                color.R = dm.ForeColor.R;
                color.G = dm.ForeColor.G;
                color.B = dm.ForeColor.B;
                mydrawline.Stroke = new SolidColorBrush(color);
                mydrawline.StrokeThickness = dm.LineWidth;//线宽度
                mydrawline.X1 = line.FirstPoint.X;
                mydrawline.Y1 = line.FirstPoint.Y;
                mydrawline.X2 = line.SecondPoint.X;
                mydrawline.Y2 = line.SecondPoint.Y;
                mCanvas.Children.Add(mydrawline);
            }
        }

        public override void DrawRectangle(RectangeGeo rect)
        {
            throw new NotImplementedException();
        }
        public override void DrawMultiLines(RandomLines rl)
        {
            throw new NotImplementedException();
        }

        public override void DrawCircle(CircleGeo circle)
        {
            throw new NotImplementedException();
        }
        public override void DrawEllipse(EllipseGeo ellipse)
        {
            throw new NotImplementedException();
        }
        public override void DrawArc(ArcGeo arc)
        {
            throw new NotImplementedException();
        }


    }
    /// <summary>
    /// 负责绘制图形，文字，图片。包含绘图方法和绘图画布
    /// </summary>
    public class WinFormPainter : PainterBase
    {
        public static System.Drawing.Pen pen = Pens.LimeGreen;
        public Graphics graphics = null;
        public Bitmap canvas = null;
        private Func<double, float> TransformX;
        private Func<double, float> TransformY;

        public WinFormPainter(Graphics g, Bitmap b)
        {
            SetGraphics(g);
            SetCanvas(b);
        }

        public WinFormPainter()
        {
        }

        private object lockObj = new object();
        public override void Clear(System.Drawing.Color color)
        {
            lock (lockObj)
            {
                base.Clear(color);
                this.graphics.Clear(color);
            }

        }
        public void SetCanvas(Bitmap bmp)
        {
            this.canvas = bmp;
        }
        public Bitmap GetCanvas()
        {
            return this.canvas;
        }
        public Graphics GetGraphics()
        {
            return this.graphics;
        }
        public void SetGraphics(Graphics g)
        {
            this.graphics = g;
            TransformX = (value) => (float)(Scale.X * value + OffsetPoint.X);
            TransformY = (value) => (float)(Scale.Y * value + OffsetPoint.Y);
        }
        public override void DrawText(DrawableText t)
        {
            if (graphics != null)
            {

                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                if (t.GetTextMeta().stringFormat == null)
                {
                    t.GetTextMeta().stringFormat = new StringFormat() { Alignment = StringAlignment.Center };
                }
                Font font;
                if (t.GetTextMeta().IsScaleble)
                {
                    font = new Font(t.GetTextMeta().TEXTFONT.FontFamily, t.GetTextMeta().TEXTFONT.Size * (float)Math.Abs(Scale.X));
                }
                else
                {
                    font = t.GetTextMeta().TEXTFONT;
                }
                //负区域外就不显示了
                float x = TransformX(t.pos.X);
                float y = TransformY(t.pos.Y);
                SizeF size = graphics.MeasureString(t.GetTextMeta().Text, font);
                if (x+size.Width<0||y+size.Height<0) 
                {
                    return;
                }
                if (canvas != null&&( x >canvas.Width|| y >canvas.Height))
                {
                    return;
                }
                graphics.DrawString(t.GetTextMeta().Text, font, new SolidBrush(t.GetTextMeta().ForeColor),x, y, t.GetTextMeta().stringFormat);
            }

        }
        #region implemented abstract members of PainterBase
        public override void DrawPolygon(PolygonGeo polygonGeo)
        {
            if (graphics != null & pen != null)
            {
                ShapeMeta sm = (ShapeMeta)polygonGeo.GetDrawMeta();
                pen = new System.Drawing.Pen(sm.ForeColor, sm.LineWidth);
                if (sm.DashLineStyle != null)
                {
                    pen.DashPattern = sm.DashLineStyle;
                }
                PointF[] points = polygonGeo.GetPointF();
                for (int i = 0; i < points.Length; i++)
                {
                    points[i].X = (float)TransformX(points[i].X);
                    points[i].Y = (float)TransformY(points[i].Y);
                }
                if (sm.IsFill)
                    graphics.FillPolygon(new SolidBrush(sm.BackColor), points);
                graphics.DrawPolygon(pen, points);
            }
        }
        public override void DrawRectangle(RectangeGeo rect)
        {
            if (graphics != null & pen != null)
            {
                ShapeMeta sm = (ShapeMeta)rect.GetDrawMeta();
                pen = new System.Drawing.Pen(sm.ForeColor, sm.LineWidth);
                if (sm.DashLineStyle != null)
                {
                    pen.DashPattern = sm.DashLineStyle;
                }
                //负区域外就不显示了  
                if (TransformX(rect.GetMaxX()+rect.Width+rect.Heigth) < 0    )
                {
                    rect.IsInVision = false;
                    return;
                }
                if (canvas != null && (TransformX(rect.GetMinX()-rect.Width-rect.Heigth) > canvas.Width  ))
                {
                    rect.IsInVision = false; 
                    return;
                }
                if (canvas != null && (TransformX(rect.GetMinX() - rect.Width - rect.Heigth) <0&& TransformX(rect.GetMaxX() - rect.Width - rect.Heigth) > canvas.Width))
                {
                    rect.IsInVision = false;
                    return;
                }
                if (Math.Abs((rect.Width) * Scale.X)<1|| Math.Abs((rect.Heigth) * Scale.Y)<1)
                {
                    rect.IsInVision = false;
                    return;
                }
                GraphicsState state = graphics.Save();
                if (this.Scale.Y > 0)
                {
                    graphics.TranslateTransform(TransformX(rect.GetMinX()), TransformY(rect.GetMinY()));
                }
                else
                {
                    graphics.TranslateTransform(TransformX(rect.GetMinX()), TransformY(rect.GetMaxY()));
                } 
                graphics.RotateTransform(rect.Angle);
                //graphics.DrawRectangle 的X,Y 是指的屏幕的左上角，
                //对于Y👆 ，则是minX maxY
                //对于Y👇 ，则是minX minY
                try
                {
                    if (sm.IsFill)
                        //graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 255, 255, 255)), x, y, width, height);
                        graphics.FillRectangle(new SolidBrush(sm.BackColor), 0, 0, Math.Abs((rect.Width) * Scale.X), Math.Abs((rect.Heigth) * Scale.Y));
                    graphics.DrawRectangle(pen, 0, 0, Math.Abs((rect.Width) * Scale.X), Math.Abs((rect.Heigth) * Scale.Y));
                }
                catch (Exception)
                {
                    rect.IsDisposed = true;
                    throw;
                } 
                graphics.Restore(state); 
            }
        }
        public override void DrawLine(LineGeo line)
        {
            DrawMeta dm = line.GetDrawMeta();

            if (graphics != null & pen != null)
            {
                pen = new System.Drawing.Pen(dm.ForeColor, dm.LineWidth);
                pen.StartCap = dm.CapStyle;
                pen.EndCap = dm.CapStyle;
                if (dm.DashLineStyle != null)
                {
                    pen.DashPattern = dm.DashLineStyle;
                }
                //负区域外就不显示了  
                if (TransformX(line.GetMaxX()) < 0  )
                {
                    return;
                }
                if (canvas != null && (TransformX(line.GetMinX()) > canvas.Width  ))
                {
                    return;
                }
                graphics.DrawLine(pen, TransformX(line.FirstPoint.X), TransformY(line.FirstPoint.Y), TransformX(line.SecondPoint.X), TransformY(line.SecondPoint.Y));
            }
        }
        public override void DrawCircle(CircleGeo circle)
        {
            if (graphics != null && pen != null)
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int leftcorner = (int)(TransformX(circle.CenterX) - circle.Radius * Math.Abs(Scale.X));
                int topcorner = (int)(TransformY(circle.CenterY) - circle.Radius * Math.Abs(Scale.Y));
                //leftcorner = leftcorner < 0 ? 0 : leftcorner;
                //topcorner = topcorner < 0 ? 0 : topcorner;
                //负区域外就不显示了  
                if (leftcorner + Math.Abs((circle.Radius * 2) * Scale.X) < 0 || topcorner + Math.Abs((circle.Radius * 2) * Scale.Y) < 0)
                {
                    circle.IsInVision = false;
                    return;
                }
                if (canvas != null && (leftcorner > canvas.Width || topcorner > canvas.Height))
                {
                    circle.IsInVision = false; 
                    return;
                }
                if (Math.Abs((circle.Radius * 2) * Scale.Y)<1)
                {
                    circle.IsInVision = false;
                    return;
                }
                circle.IsInVision = true; 
                ShapeMeta sm = circle.GetDrawMeta() as ShapeMeta;
                pen = new System.Drawing.Pen(sm.ForeColor, sm.LineWidth);
                if (sm.DashLineStyle != null)
                {
                    pen.DashPattern = sm.DashLineStyle;
                }
                try
                {
                    if (sm.IsFill)
                        graphics.FillEllipse(new SolidBrush(sm.BackColor), leftcorner, topcorner, Math.Abs((circle.Radius * 2) * Scale.X), Math.Abs((circle.Radius * 2) * Scale.Y));
                    graphics.DrawEllipse(pen, leftcorner, topcorner, Math.Abs((circle.Radius * 2) * Scale.X), Math.Abs((circle.Radius * 2) * Scale.Y));
                }
                catch (Exception)
                {
                    circle.IsDisposed = true;
                    throw;
                }
                
            }
        }
        public override void DrawEllipse(EllipseGeo ellipse)
        {
            if (graphics != null & pen != null)
            {
                ShapeMeta sm = ellipse.GetDrawMeta() as ShapeMeta;
                pen = new System.Drawing.Pen(sm.ForeColor, sm.LineWidth);
                if (sm.DashLineStyle != null)
                {
                    pen.DashPattern = sm.DashLineStyle;
                }
                GraphicsState state = graphics.Save();
                if (this.Scale.Y > 0)
                {
                    graphics.TranslateTransform(TransformX(ellipse.GetMinX()), TransformY(ellipse.GetMinY()));
                }
                else
                {
                    graphics.TranslateTransform(TransformX(ellipse.GetMinX()), TransformY(ellipse.GetMaxY()));
                }
                graphics.RotateTransform(ellipse.Angle);
                //graphics.DrawRectangle 的X,Y 是指的屏幕的左上角，
                //对于Y👆 ，则是minX maxY
                //对于Y👇 ，则是minX minY
                graphics.DrawEllipse(pen, 0, 0, Math.Abs((ellipse.Width) * Scale.X), Math.Abs((ellipse.Heigth) * Scale.Y));
                if (sm.IsFill)
                    //graphics.FillRectangle(new SolidBrush(Color.FromArgb(200, 255, 255, 255)), x, y, width, height);
                    graphics.FillEllipse(new SolidBrush(sm.BackColor), 0, 0, Math.Abs((ellipse.Width) * Scale.X), Math.Abs((ellipse.Heigth) * Scale.Y));
                graphics.Restore(state);
            }
        }
        public override void DrawArc(ArcGeo arc)
        {
            if (graphics != null && pen != null)
            {
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                int leftcorner = (int)(TransformX(arc.CenterX) - arc.Radius * Math.Abs(Scale.X));
                int topcorner = (int)(TransformY(arc.CenterY) - arc.Radius * Math.Abs(Scale.Y));
                ShapeMeta sm = arc.GetDrawMeta() as ShapeMeta;
                pen = new System.Drawing.Pen(sm.ForeColor, sm.LineWidth);
                if (sm.DashLineStyle != null)
                {
                    pen.DashPattern = sm.DashLineStyle;
                }
                double sa = arc.StartAngle;
                double ea = arc.EndAngle;
                if (Scale.Y < 0)
                {
                    sa = -arc.EndAngle;
                    ea = -arc.StartAngle;
                }

                if (Scale.X < 0)
                {
                    sa = 180 - arc.EndAngle;
                    ea = 180 - arc.StartAngle;
                }
                //他这个是顺时针为正
                try
                {
                    graphics.DrawArc(pen, (leftcorner), (topcorner), Math.Abs((arc.Radius * 2) * Scale.X), Math.Abs((arc.Radius * 2) * Scale.Y), (float)sa, (float)(ea - sa));
                }
                catch (Exception)
                {
                    return;
                }
            }
        }
        #endregion
        public override void Dispose()
        {
            base.Dispose();
            if (this.canvas != null)
            {
                this.canvas.Dispose();
                this.canvas = null;
            }
            if (this.graphics != null)
            {
                this.graphics.Dispose();
                this.graphics = null;
            }
        }

    }

    public enum DRAW_TYPE
    {
        CIRCLE,
        RECTANGLE,
        TEXT,
        LINE,
        RANDOMLINES,
        IMAGE,
        ELLIPSE
    }
    [Serializable]
    public class DrawMeta
    {
        //包含绘制的内容和样式信息
        public System.Drawing.Color ForeColor { get; set; }
        private float _lineWidth = 1;
        public float LineWidth
        {
            get { return _lineWidth; }
            set
            {
                _lineWidth = value;
                if (_lineWidth <= 0)
                    LineWidth = 1;
            }
        }
        public LineCap CapStyle = LineCap.Round;
        public float[] DashLineStyle { get; set; }
        //设置线型比例e.g.[1f,0.5f,2f]
    }
    [Serializable]
    public class ShapeMeta : DrawMeta  //for shape element
    {
        public bool IsFill { get; set; }
        public System.Drawing.Color BackColor { get; set; }
        public ShapeMeta()
        {
            LineWidth = 1;
        }
    }

}

[Serializable]
public class TextMeta : DrawMeta
{
    public TextMeta(string text)
    {
        LineWidth = 1;
        Text = text;
        TEXTFONT = new Font("黑体", 12, FontStyle.Bold);
        stringFormat = new StringFormat();
        stringFormat.Alignment = StringAlignment.Center;
    }
    public bool IsScaleble = false;
    public StringFormat stringFormat { get; set; }
    public Font TEXTFONT { get; set; }
    public string Text { get; set; }
}

public interface IScreenPrintable
{
    void Draw(PainterBase pb);
}

