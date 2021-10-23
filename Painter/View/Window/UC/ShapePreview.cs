using Painter.Models;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.View.Window.UC
{
    public partial class ShapePreview : UserControl
    {
        Shape PreShape;
        Bitmap bitmap;
        Graphics graphics;
        WinFormPainter Painter;
        PointGeo oldPoint = new PointGeo(0, 0);
        PointGeo newPoint = new PointGeo(0, 0);
        PointGeo oldOffsetPoint = new PointGeo(0, 0);

        public ShapePreview()
        {
            this.DoubleBuffered = true;
            InitializeComponent();
            this.Load += OnLoad;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.MouseWheel += OnMouseWheel;
            this.SizeChanged += OnSizeChanged;
            this.Paint += OnPaint;
        }
        private void OnSizeChanged(object sender, EventArgs e)
        {
            SetLayerManagerGraphic();
            SetCanvasScale(this.Width, this.Height);
            this.Invalidate();
        }
        private void SetLayerManagerGraphic()
        {
            bitmap = new Bitmap(this.Width, this.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.Clip = new Region(new Rectangle(0, 0, this.Width, this.Height));
            graphics.Clear(Color.Transparent);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            if (Painter == null)
            {
                Painter = new WinFormPainter(graphics, bitmap);
            }
            else
            {
                Painter.SetGraphics(graphics);
                Painter.SetCanvas(bitmap);
            }
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            graphics.Clear(Color.Transparent);
            if (PreShape != null)
            {
                PreShape.Draw();
            }
            Graphics g = e.Graphics;
            g.DrawImage(this.bitmap, 0, 0);
            g.DrawRectangle(new Pen(Color.Snow,0.5f),new Rectangle(2,2,this.Width-5,this.Height-5));
        }
        public void SetCanvasScale(double width, double height)
        {
            if (PreShape==null)
            {
                return;
            }
            //先缩放后平移
            PointGeo offsetPoint;
            PointGeo scaleP;
            float ratioX = (float)((width * 0.9) * 1.0f / (PreShape.GetMaxX() - PreShape.GetMinX()));
            float ratioY = (float)((height * 0.8f) / (PreShape.GetMaxY() - PreShape.GetMinY()));
            float ratio = Math.Min(ratioY, ratioX);
            scaleP = new PointGeo(ratio, -ratio);
            offsetPoint = new PointGeo((float)(-PreShape.GetMinX() * scaleP.X+50), (float)(height - PreShape.GetMinY() * scaleP.Y));
            SetCanvasTranlate(offsetPoint, scaleP);
        }
        private void OnMouseWheel(object sender, MouseEventArgs e)
        {
            PointGeo point = ScreenToObjectPos(e.X, e.Y);
            PointGeo Scale = Painter.Scale;

            if (e.Delta > 0)
            {
                Scale = 1.2f * Scale;
                point = 1.2f * point;
            }
            else
            {
                Scale = 0.8f * Scale;
                point = 0.8f * point;
            }
            if (Scale.X > 1E6)
            {
                return;
            }
            if (Scale.X < 1E-3)
            {
                return;
            }
            Point newPoint = ObjectToScreen(point);
            SetCanvasTranlate(new PointGeo(e.X - newPoint.X, e.Y - newPoint.Y) + this.Painter.OffsetPoint, Scale);
            Invalidate(new System.Drawing.Rectangle(0, 0, this.Width, this.Height));

        }
        private void SetCanvasTranlate(PointGeo offsetPoint, PointGeo scaleP)
        {
            if (scaleP.X > 1E6)
            {
                return;
            }
            if (scaleP.X < 1E-3)
            {
                return;
            }
            Painter.OffsetPoint = offsetPoint;
            Painter.Scale = scaleP;
        }
        public PointGeo ScreenToObjectPos(int x, int y)
        {
            PointGeo point = new PointGeo(x, y);
            point.Offset(-1 * this.Painter.OffsetPoint);
            point.Scale(1 / this.Painter.Scale);
            return point;
        }
        public Point ObjectToScreen(PointGeo point)
        {
            point.Scale(Painter.Scale);
            point.Offset(Painter.OffsetPoint);
            //Point p = GraphicsPosToScreen(point);
            Point p = new Point((int)point.X, (int)point.Y);
            return p;
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                oldPoint.X = e.X;
                oldPoint.Y = e.Y;
                oldOffsetPoint = this.Painter.OffsetPoint.Clone();
            }
        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                PointGeo newPoint = new PointGeo(e.X, e.Y);
                PointGeo offsetPoint = newPoint - oldPoint;
                this.Painter.OffsetPoint = oldOffsetPoint + offsetPoint;
                this.Invalidate();
            }
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {

        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        public void SetShape(Shape shape)
        {
            this.PreShape = shape;
            PreShape.SetPainter(Painter); 
            SetCanvasScale(this.Width, this.Height); 
            Invalidate();
        }

        void OnLoad(object sender, EventArgs e)
        {
            bitmap = new Bitmap(this.Width, this.Height);
            graphics = Graphics.FromImage(bitmap);
            Painter = new WinFormPainter(graphics, bitmap);
            Painter.Scale = new PointGeo(1, 1);
            Painter.OffsetPoint = new PointGeo(0, 0);
            if (this.PreShape==null)
            {
                ArcGeo arcGeo = new ArcGeo();
                arcGeo.StartAngle = 0;
                arcGeo.EndAngle = 90;
                arcGeo.CenterX = this.Width / 2;
                arcGeo.CenterY = this.Height / 2;
                arcGeo.Radius = 100;
                PreShape = arcGeo;
            }
            if (PreShape != null)
            {
                PreShape.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Red, LineWidth = 2 });
                PreShape.SetPainter(Painter);
                SetCanvasScale(this.Width, this.Height);
            } 
        }
    }
}
