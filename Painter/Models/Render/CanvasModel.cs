using Painter.DisplayManger;
using Painter.Models.CmdControl;
using Painter.Models.PainterModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.Models
{
    ///负责：
    ///1.图形绘制
    ///2.视图操作
    ///3.鼠标反馈
    [Serializable] 
    public class CanvasModel
    {
        public event Action<string> CmdMsgEvent;
        public void OnCmdMsg(string str)
        {
            CmdMsgEvent?.Invoke(str);
        }
        private PointGeo offset = new PointGeo();
        private PointGeo scale = new PointGeo();
        private CommandMgr CmdMgr;
        public CommandMgr GetCmdMgr()
        {
            return CmdMgr;
        }
        public PointGeo OffsetPoint
        {
            get
            {
                if (this.fixedLayerManager != null)
                {
                    offset = this.fixedLayerManager.GetPainter().OffsetPoint;
                }
                return offset;
            }
            set
            {
                //Debug.Print(offset.ToString());
                offset = value;
                if (this.fixedLayerManager != null)
                {
                    this.fixedLayerManager.GetPainter().OffsetPoint = offset;
                    this.freshLayerManager.GetPainter().OffsetPoint = offset;
                }
            }
        }
        public PointGeo Scale
        {
            get
            {
                if (this.fixedLayerManager != null)
                {
                    scale = this.fixedLayerManager.GetPainter().Scale;
                }
                return scale; 
            }
            set
            {
                scale = value;
                if (this.fixedLayerManager != null)
                {
                    this.fixedLayerManager.GetPainter().Scale = scale;
                    this.freshLayerManager.GetPainter().Scale = scale;
                }
            }
        }

        public float Width { get; set; }

        public void ShowPanel(bool v)
        {
            foreach (var item in this.geoControls)
            {
                item.IsVisible = v;
            }
        }

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
        public GraphicsLayerManager GetScreenManager()
        {
            return this.screenManager;
        }
        private GraphicsLayerManager fixedLayerManager;//画静止物体，标题之类的
        private GraphicsLayerManager freshLayerManager;//需要刷新的物体
        private GraphicsLayerManager screenManager=new GraphicsLayerManager();//本窗口自身
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
            MaxX = 2400;
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

            //geoControls.Clear();
            //this.screenManager.Clear();
        }
        public void Init()
        {
            SetLayerManagerGraphic(ref fixedLayerManager);
            SetLayerManagerGraphic(ref freshLayerManager);
            SetCanvasTranlate(this.Width, this.Height);
            screenManager.SetPainter(new WinFormPainter()); 
        }
        public   Color Background = Color.White;
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
        public static bool EnableTrack = false;
        private object lockObj = new object();
        public void DrawOnCanvas(Graphics graphics)
        {
            Action<GraphicsLayerManager> onPanit = (layer) =>
            {
                lock (lockObj)
                {
                    layer.GetPainter().Clear(Color.Transparent);
                    layer.Draw();
                    Image image = (layer.GetPainter() as WinFormPainter).GetCanvas();
                    graphics.SmoothingMode = SmoothingMode.HighQuality;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
                    graphics.DrawImage(image, 0, 0, image.Width, image.Height);
                }
            };
            //onPanit(this.fixedLayerManager);
            if (EnableTrack)
            {
                (this.fixedLayerManager.GetPainter() as WinFormPainter).graphics.FillRectangle(new SolidBrush(Color.FromArgb(50, Background)), 0, 0, this.Width, this.Height);
            }
            else
            {
                (this.fixedLayerManager.GetPainter() as WinFormPainter).graphics.Clear(Color.Transparent);
            }
            this.fixedLayerManager.Draw();
            graphics.DrawImage((this.fixedLayerManager.GetPainter() as WinFormPainter).GetCanvas(), 0, 0);
            onPanit(this.freshLayerManager); 

            (screenManager.GetPainter() as WinFormPainter).SetGraphics(graphics);
            graphics.SmoothingMode = SmoothingMode.HighQuality;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixel;
            screenManager.Draw();
            foreach (var item in this.ControlPoints)
            {
                Point p=this.ObjectToScreen(item.Clone());
                //Debug.Print(p.ToString());
                graphics.DrawRectangle(Pens.BlueViolet, new Rectangle(p.X-4, p.Y-4, 8, 8));
            }
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
             this.OffsetPoint = offsetPoint; 
            this.Scale = scaleP;
        }
        internal void SetCanvasTranlate(float width, float height)
        {
            //先缩放后平移  
            float ratioX = (float)((width - this.RectLeft * 2) * 1.0f / (MaxX - MinX));
            float ratioY = (float)((height - this.RectTop * 2) / (MaxY - MinY));
            float ratio = Math.Min(ratioY, ratioX);
            if (IsYUpDirection)
            {
                Scale = new PointGeo(ratio, -ratio);
            }
            else
            {
                Scale = new PointGeo(ratio, ratio);
            }
            if (IsYUpDirection)
            {
                OffsetPoint = new PointGeo(-(float)this.MinX * Scale.X + this.RectLeft, height - this.RectTop - (float)this.MinY * Scale.Y);
            }
            else
            {
                OffsetPoint = new PointGeo(-(float)this.MinX * Scale.X + this.RectLeft, -(float)this.MinY * Scale.Y + this.RectTop);
            }
        }
        public void OnLoad(object sender, EventArgs e)
        {
            this.Width = (sender as Form).ClientRectangle.Width;
            this.Height = (sender as Form).ClientRectangle.Height;

            SetCanvasTranlate(this.Width, this.Height);
            Init();
            CmdMgr = new CommandMgr(this);

        }
        private bool isError = false;
        public void OnPaint(object sender, PaintEventArgs e)
        {
            if (isError)
            {
                return;
            }
            try
            {
                DrawOnCanvas(e.Graphics); 
            }
            catch (OutOfMemoryException ee)
            {
                isError = true;
                //this.CmdMgr.Undo();
                Init();
                Thread.Sleep(20); 
                MessageBox.Show("Out of Memery. The Smallest shape has been disposed");
                isError = false;
            }
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                PointGeo newPoint = new PointGeo(e.X, e.Y);
                PointGeo offsetPoint = newPoint - oldPoint;
                this.OffsetPoint = oldOffsetPoint + offsetPoint;
                this.OffsetPoint = oldOffsetPoint + offsetPoint;
                if (this.Invalidate != null)
                {
                    this.Invalidate();
                }
            }
            CurObjectPoint = ScreenToObjectPos(e.X, e.Y);
            if (this.selectPoint != null)
            {
                this.selectPoint.X = CurObjectPoint.X;
                this.selectPoint.Y = CurObjectPoint.Y;
                foreach (var item in clickCeo)
                {
                    item.Initial();
                }
            }
            if (EnableClickTest)
            {
                MoveTest(new Point(e.X, e.Y));
                this.CmdMgr.OnMouseMove(sender, e);
            } 
            
        }
        public bool EnableClickTest = true;
        public bool EnableSelect = true;
        public void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                selectPoint = null;
            }
            this.CmdMgr.OnMouseUp(sender, e);
        }
        public void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift)
            {
                IsShiftDown = true;
            }
            else
            {
                IsShiftDown = false;
            }
            if (e.Modifiers == Keys.Control)
            {
                IsControlDown = true;
            }
            else
            {
                IsControlDown = false;
            }
        }
        public   bool IsControlDown = false;
        public   bool IsShiftDown = false;
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            foreach (var item in this.geoControls)
            {
                if (item is GeoEditBox)
                {
                    (item as GeoEditBox).OnKeyDown(sender, e);
                }
            }
            if (e.Modifiers == Keys.Shift)
            {
                IsShiftDown = true;
            }
            else
            {
                IsShiftDown = false;
            }
            if (e.Modifiers == Keys.Control)
            {
                IsControlDown = true;
            }
            else
            {
                IsControlDown = false;
            }
            this.CmdMgr.OnKeyDown(sender, e);
        }
        PointGeo oldPoint = new PointGeo(0, 0);
        PointGeo newPoint = new PointGeo(0, 0);
        PointGeo oldOffsetPoint = new PointGeo(0, 0);
        public PointGeo CurObjectPoint = new PointGeo();
        public Point  CurScreenPoint = new Point();
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            IsClickHandled = false;

            if (e.Button == MouseButtons.Middle)
            {
                oldPoint.X = e.X;
                oldPoint.Y = e.Y;
                oldOffsetPoint = this.fixedLayerManager.GetPainter().OffsetPoint.Clone();
            }
            CurObjectPoint = ScreenToObjectPos(e.X, e.Y);
            CurScreenPoint = e.Location;
            if (e.Button==MouseButtons.Left&&EnableClickTest)
            {
                ClickTest(new Point(e.X, e.Y)); 
            }
            if (!IsClickHandled)
            {
                this.CmdMgr.OnMouseDown(sender, e); 
            }
            this.Invalidate();
        }
        public float[] LineDashStyle = null;
        public event Action<List<DrawableObject>> ShapeClickEvent;
        private static int HitRangePixel = 5;
        private CircleGeo hitCircle = new CircleGeo();
        List<DrawableObject> clickCeo = new List<DrawableObject>();
        private List<GeoControl> geoControls = new List<GeoControl>();
        public void AddGeoControls(GeoControl geoControl)
        {
            if (geoControl is GeoPanel)
            {
                this.geoControls.AddRange((geoControl as GeoPanel).GetControls());
                this.geoControls.Add(geoControl);
            }
            else
            {
                this.geoControls.Add(geoControl);
            } 
            this.screenManager.AddRange(geoControl.GetElements(), true);
        }
        private void MoveTest(Point point)
        {
            hitCircle.FirstPoint = new PointGeo(point.X, point.Y);
            hitCircle.SecondPoint = hitCircle.FirstPoint + new PointGeo(HitRangePixel, HitRangePixel);
            foreach (var item in this.geoControls)
            {
                item.HitTest(hitCircle, false);
            }
            this.Invalidate();
        }
        private  List<PointGeo> ControlPoints=new List<PointGeo>();
        private PointGeo selectPoint; 
        private void ClickTest(Point point )
        {
            selectPoint = null;
            hitCircle.FirstPoint = new PointGeo(point.X, point.Y);
            hitCircle.SecondPoint = hitCircle.FirstPoint + new PointGeo(HitRangePixel, HitRangePixel);
            foreach (var item in this.geoControls)
            {
                if (item.HitTest(hitCircle, true))//优先显示Controls的信息
                {
                    IsClickHandled = true;
                }
            }
            if (IsClickHandled)
            {
                return;
            }
            if (!EnableSelect)
            {
                return;
            }
            float HitRange = HitRangePixel / this.fixedLayerManager.GetPainter().Scale.X;
            PointGeo objPoint = ScreenToObjectPos(point.X, point.Y);
            hitCircle.FirstPoint = objPoint;
            hitCircle.SecondPoint = objPoint + new PointGeo(HitRange, HitRange);
            foreach (var item in this.ControlPoints)
            {
                PointGeo dist = item - hitCircle.FirstPoint;
                if (dist.GetLength()< HitRange)
                {
                    selectPoint = item;
                    return;
                }
            }

            foreach (var item in clickCeo)
            {
                item.GetDrawMeta().DashLineStyle = LineDashStyle;
                if (item is DrawableText)
                { 
                    item.GetDrawMeta().ForeColor = item.GetDrawMeta().BackColor;
                }
            }
            clickCeo.Clear();
            ControlPoints.Clear();
         
            foreach (var item in this.freshLayerManager.GetDatas())
            {
                if (item is DrawableObject)
                {
                    DrawableObject shape = item as DrawableObject;
                     
                    if (shape.IsShow==false||shape.IsInVision==false)
                    {
                        continue;
                    }
                    if (hitCircle.IsOverlap(shape))
                    {
                        clickCeo.Add(shape);
                        ControlPoints.AddRange(shape.GetControlPoints());
                    }
                    
                }
            }
            foreach (var item in this.fixedLayerManager.GetDatas())
            {
                if (item is DrawableObject)
                {
                    DrawableObject shape = item as DrawableObject;
                    
                    if (shape.IsShow == false || shape.IsInVision == false)
                    {
                        continue;
                    }
                    if (hitCircle.IsOverlap(shape))
                    {
                        clickCeo.Add(shape);
                    }
                }
            }
           

            //if (clickCeo.GetShapes().Count!=0)
            {
                ShapeClickEvent?.Invoke(clickCeo);
            } 
            
            
        }
        private bool IsClickHandled = false;
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
                return;//太大会有偏移
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
