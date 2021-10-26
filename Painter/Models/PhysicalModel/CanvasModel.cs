﻿using Painter.DisplayManger;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Models
{
    class CanvasModel
    {
        PointGeo offsetPoint;
        PointGeo scaleP;
        public float Width { get; set; }
        public float Height { get; set; }
        public event Action Invalidate;
        public GraphicsLayerManager GetHoldLayerManager()
        {
            return fixedLayerManager;
        } 
        public GraphicsLayerManager GetFreshLayerManager()
        {
            return freshLayerManager;
        }
        private GraphicsLayerManager fixedLayerManager;//画静止物体，标题之类的
        private GraphicsLayerManager freshLayerManager;//需要刷新的物体
        public CanvasModel()
        {
            //图形区域
            this.Width = 1200;
            this.Height = 600;
            this.RectTop = 100;
            this.RectLeft = 100;
            IsYUpDirection = true;
            //数据区域
            MinX = 0;
            MaxX = 1200;
            MinY = 0;
            MaxY = 600;
        }
        public void OnInvalidate()//刷新前
        {
            if (this.Invalidate != null)
            {
                this.Invalidate();
            }
        }
        public void Clear()
        {
            this.fixedLayerManager.Clear();
            this.freshLayerManager.Clear(); 
        }
        public void Init()
        { 
            SetLayerManagerGraphic(ref fixedLayerManager);
            SetLayerManagerGraphic(ref freshLayerManager);
            SetCanvasTranlate(this.Width, this.Height); 
        }
        public void OnSizeChanged(object sender, EventArgs e)
        {
            this.Width = (sender as Form).ClientRectangle.Width;
            this.Height = (sender as Form).ClientRectangle.Height;
            if (fixedLayerManager != null)
            {
                //this.fixedLayerManager.Clear();
                Init();
            }

        }
        private object lockObj = new object();
        public void DrawOnCanvas(Graphics graphics)
        {
            Action<GraphicsLayerManager> onPanit = (layer) =>
            {
                lock (lockObj)
                {
                    layer.GetPainter().Clear();
                    layer.Draw();
                    Image image = (layer.GetPainter() as WinFormPainter).GetCanvas();
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                }
            };
            onPanit(this.fixedLayerManager);
            onPanit(this.freshLayerManager);
            //this.freshLayerManager.GetPainter().Clear();
            //this.freshLayerManager.Draw();
            //graphics.DrawImage((this.freshLayerManager.GetPainter() as WinFormPainter).GetCanvas(), 0, 0);
        }
        private void SetLayerManagerGraphic(ref GraphicsLayerManager layerGraphic)
        {
            if (layerGraphic != null)
            {
                if (this.Width * this.Height <= 3)
                {
                    return;
                }
                layerGraphic.Dispose();
                Bitmap canvasBitmap = null;
                canvasBitmap = new Bitmap((int)this.Width, (int)this.Height);
                canvasBitmap.SetResolution(96, 96);
                Graphics canGraphics = Graphics.FromImage(canvasBitmap);
                //canGraphics.Clip = new Region(new Rectangle(0, 0, (int)this.Width, (int)this.Height));
                canGraphics.Clear(Color.Transparent);
                canGraphics.SmoothingMode = SmoothingMode.HighQuality;
                canGraphics.CompositingQuality = CompositingQuality.HighQuality;
                canGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                canGraphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                if (layerGraphic.GetPainter() == null)
                {
                    layerGraphic.SetPainter(new WinFormPainter(canGraphics, canvasBitmap));
                }
                else
                {
                    (layerGraphic.GetPainter() as WinFormPainter).SetGraphics(canGraphics);
                    (layerGraphic.GetPainter() as WinFormPainter).SetCanvas(canvasBitmap);
                }
            }
            else
            {
                layerGraphic = new GraphicsLayerManager();
                //layerGraphic.Invalidate += this.Invalidate;
                SetLayerManagerGraphic(ref layerGraphic);
            }
        } 
        public bool IsYUpDirection { get; set; }
        private double _minX;
        private double _minY;
        private double _maxX;
        private double _maxY;
        public double MinX { get { return _minX; } set { _minX = value; } }
        public double MinY { get { return _minY; } set { _minY = value; } }
        public double MaxX { get { return _maxX; } set { _maxX = value; } }
        public double MaxY { get { return _maxY; } set { _maxY = value; } }
        public float RectTop { get; set; }
        public float RectLeft { get; set; }
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
            this.fixedLayerManager.GetPainter().OffsetPoint = offsetPoint;
            this.fixedLayerManager.GetPainter().Scale = scaleP;
            this.freshLayerManager.GetPainter().OffsetPoint = offsetPoint;
            this.freshLayerManager.GetPainter().Scale = scaleP;
        }
        internal void SetCanvasTranlate(float width, float height)
        {
            //先缩放后平移  
            float ratioX = (float)((width - this.RectLeft * 2) * 1.0f / (MaxX - MinX));
            float ratioY = (float)((height - this.RectTop * 2) / (MaxY - MinY));
            float ratio = Math.Min(ratioY, ratioX);
            if (IsYUpDirection)
            {
                scaleP = new PointGeo(ratio, -ratio);
            }
            else
            {
                scaleP = new PointGeo(ratio, ratio);
            }
            if (IsYUpDirection)
            {
                offsetPoint = new PointGeo(-(float)this.MinX * scaleP.X + this.RectLeft, height - this.RectTop - (float)this.MinY * scaleP.Y);
            }
            else
            {
                offsetPoint = new PointGeo(-(float)this.MinX * scaleP.X + this.RectLeft, -(float)this.MinY * scaleP.Y + this.RectTop);
            }
            if (this.fixedLayerManager != null)
            {
                this.fixedLayerManager.GetPainter().OffsetPoint = offsetPoint;
                this.fixedLayerManager.GetPainter().Scale = scaleP;
                this.freshLayerManager.GetPainter().OffsetPoint = offsetPoint;
                this.freshLayerManager.GetPainter().Scale = scaleP;
            }
        }
        public void OnLoad(object sender, EventArgs e)
        {
            this.Width = (sender as Form).ClientRectangle.Width;
            this.Height = (sender as Form).ClientRectangle.Height;

            SetCanvasTranlate(this.Width, this.Height);
            Init();

        }
        public void OnPaint(object sender, PaintEventArgs e)
        { 
            DrawOnCanvas(e.Graphics);
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                PointGeo newPoint = new PointGeo(e.X, e.Y);
                PointGeo offsetPoint = newPoint - oldPoint;
                this.GetHoldLayerManager().GetPainter().OffsetPoint = oldOffsetPoint + offsetPoint;
                this.GetFreshLayerManager().GetPainter().OffsetPoint = oldOffsetPoint + offsetPoint;
                if (this.Invalidate!=null)
                {
                    this.Invalidate(); 
                }
            }
        }
        public void OnMouseUp(object sender, MouseEventArgs e)
        {
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {

        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {

        }
        PointGeo oldPoint = new PointGeo(0, 0);
        PointGeo newPoint = new PointGeo(0, 0);
        PointGeo oldOffsetPoint = new PointGeo(0, 0);
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                oldPoint.X = e.X;
                oldPoint.Y = e.Y;
                oldOffsetPoint = this.fixedLayerManager.GetPainter().OffsetPoint.Clone();
            }
        }
        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            PointGeo point = ScreenToObjectPos(e.X, e.Y);
            PointGeo Scale = fixedLayerManager.GetPainter().Scale;

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
            SetCanvasTranlate(new PointGeo(e.X - newPoint.X, e.Y - newPoint.Y) + this.fixedLayerManager.GetPainter().OffsetPoint, Scale);
            Invalidate();
        }
        public PointGeo ScreenToObjectPos(int x, int y)
        {
            PointGeo point = new PointGeo(x, y);
            point.Offset(-1 * this.fixedLayerManager.GetPainter().OffsetPoint);
            point.Scale(1 / this.fixedLayerManager.GetPainter().Scale);
            return point;
        }
        public Point ObjectToScreen(PointGeo point)
        {
            point.Scale(fixedLayerManager.GetPainter().Scale);
            point.Offset(fixedLayerManager.GetPainter().OffsetPoint);
            //Point p = GraphicsPosToScreen(point);
            Point p = new Point((int)point.X, (int)point.Y);
            return p;
        }
    }

}