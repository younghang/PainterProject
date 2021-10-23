using Painter.Models;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.DisplayManger.Axis
{
    /// <summary>
    /// 就是基本的chart绘图,基于Bitmap的（双缓存）
    /// </summary>
    public class AxisModel
    {
        public event Action Invalidate;
        private GraphicsLayerManager fixedLayerManager;//画坐标轴，刻度，标尺，标题之类的
        private GraphicsLayerManager graphicLayerManager;//绘制图形[相同比例尺下的图形]
       
        private Geo UnitVector = new Geo();
        public Font DrawLabelFont { get; set; }
        public Font DrawAxesFont { get; set; }
        public Font DrawTitleFont { get; set; }
        public string Title { get; set; } 
        public string XLabel { get; set; } 
        public string YLabel { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float RectTop { get; set; }
        public float RectLeft { get; set; }

        public int XIntervalCount { get; set; }
        public int YIntervalCount { get; set; }
        public ShapeMeta IntervalMeta { get; set; }
        public bool IsYUpDirection { get; set; }
        private Dictionary<string, RandomLines> lines;
        private double _minX;
        private double _minY;
        private double _maxX;
        private double _maxY;
        public double MinX { get { return _minX; } set { _minX = value; } }
        public double MinY { get { return _minY; } set { _minY = value; } }
        public double MaxX { get { return _maxX; } set { _maxX = value; } }
        public double MaxY { get { return _maxY; } set { _maxY = value; } }
        public AxisModel()
        {
            //图形区域
            this.Width = 600;
            this.Height = 600;
            this.RectTop = 100;
            this.RectLeft = 100;
            IsYUpDirection = true;
            //数据区域
            MinX = 100;
            MaxX = 2000;
            MinY = -500;
            MaxY = 500;
            this.DrawLabelFont = new Font("Consolas", 24, FontStyle.Bold);
            this.DrawAxesFont = new Font("Times New Roman", 12, FontStyle.Regular);
            this.DrawTitleFont = new Font("宋体", 24, FontStyle.Bold);
            XIntervalCount = 7;
            YIntervalCount = 7;

            IntervalMeta = new ShapeMeta();
            IntervalMeta.ForeColor = Color.Black;
            IntervalMeta.LineWidth = 1;
            lines = new Dictionary<string, RandomLines>();
            
            Title= "This is a title";
            XLabel= "This is X Label.";
        }
        public bool AddNewLine(string name)
        {
            if (!this.lines.Keys.Contains(name))
            {
                RandomLines randomLine = new RandomLines();
                randomLine.SetDrawMeta(new ShapeMeta() { LineWidth = 1, ForeColor = Color.Black });
                randomLine.IsHold = false;
                this.lines[name] = randomLine;
                DrawLines();
                return true;
            }
            return false;
        }
        public List<PointGeo> GetData()
        {
            if (this.lines.Count>0)
            {
                return this.lines.Values.ToList<RandomLines>()[0].points; 
            }
            return null;
        }
        public static int MAX_COUNT = 2000;
        private object LockObj = new object();
        public void AddPoint(string name, PointGeo p)
        {
            if (this.lines.Keys.Contains(name))
            {
                lock (lockObj)
                {
                    this.lines[name].points.Add(p.Clone());
                    if (this.lines[name].points.Count > MAX_COUNT)
                    {
                        this.MinX = this.lines[name].points[0].X;
                        this.lines[name].points.RemoveAt(0);
                    }
                    if (p.X < this.MinX)
                    {
                        this.MinX = p.X;
                    }
                    if (p.X > this.MaxX)
                    {
                        this.MaxX = p.X;
                    }
                    if (p.Y < this.MinY)
                    {
                        this.MinY = p.Y;
                    }
                    if (p.Y > this.MaxY)
                    {
                        this.MaxY = p.Y;
                    }
                    fixedLayerManager.Clear();
                    SetCanvasTranlate(this.Width, this.Height);
                    DrawBorder();
                    DrawInterva();
                    DrawTickText();
                    DrawTitleText();
                }
                //内部调用，外部刷新就用OnInvalidate
                //fixedLayerManager.Clear();
                //SetCanvasTranlate(this.Width, this.Height);
                //DrawBorder();
                //DrawInterva();
                //DrawTickText();
                //DrawTitleText();
                //if (this.Invalidate != null)
                //{
                //    this.Invalidate();
                //}
            }
            else
            {
                AddNewLine(name);
            }
        }
        public void OnInvalidate()//刷新前
        {
            if (this.Invalidate != null)
            {
                this.Invalidate();
            }
        }
        private Action<Font> SetFont = (font) =>
        {
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                font = fd.Font;
            }
        };
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
            this.graphicLayerManager.GetPainter().Clear();
            this.graphicLayerManager.Draw();
            graphics.DrawImage((this.graphicLayerManager.GetPainter() as WinFormPainter).GetCanvas(), 0, 0);
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
        PointGeo offsetPoint;
        PointGeo scaleP;
        internal void SetCanvasTranlate(float width, float height)
        {
            //先缩放后平移  
            float ratioX = (float)((width - this.RectLeft * 2) * 1.0f / (MaxX - MinX));
            float ratioY = (float)((height - this.RectTop * 2) / (MaxY - MinY));
            //float ratio = Math.Min(ratioY, ratioX);
            if (IsYUpDirection)
            {
                scaleP = new PointGeo(ratioX, -ratioY);
            }
            else
            {
                scaleP = new PointGeo(ratioX, ratioY);
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
                this.graphicLayerManager.GetPainter().OffsetPoint = offsetPoint;
                this.graphicLayerManager.GetPainter().Scale = scaleP;
            }
        }
        private void DrawLines()
        {
            this.graphicLayerManager.Clear();
            foreach (var line in this.lines)
            {
                line.Value.ResetPoints();
                this.graphicLayerManager.Add(line.Value);
            }
        }
        public void Clear()
        {
            this.fixedLayerManager.Clear();
            this.graphicLayerManager.Clear();
            foreach (var item in this.lines)
            {
                lines[item.Key].Clear();
            }
            this.lines.Clear();
        }
        public void Init()
        {
            if (MaxY == MinY)
            {
                MaxY = 1000;
                MinY = 0;
            }
            SetLayerManagerGraphic(ref fixedLayerManager);
            SetLayerManagerGraphic(ref graphicLayerManager);
            SetCanvasTranlate(this.Width, this.Height);
            DrawBorder();
            DrawInterva();
            DrawTickText();
            DrawTitleText();
            DrawLines();
        }
        private void DrawBorder()
        {
            PointGeo leftTop = new PointGeo((float)MinX, (float)MaxY);
            PointGeo leftBottom = new PointGeo((float)MinX, (float)MinY);
            PointGeo rightTop = new PointGeo((float)MaxX, (float)MaxY);
            PointGeo rightBottom = new PointGeo((float)MaxX, (float)MinY);
            LineGeo BottomLine = new LineGeo(leftBottom, rightBottom);
            LineGeo TopLine = new LineGeo(leftTop, rightTop);
            LineGeo LeftLine = new LineGeo(leftBottom, leftTop);
            LineGeo RightLine = new LineGeo(rightBottom, rightTop);

            //RectangeGeo borderRect = new RectangeGeo();
            //borderRect.StartPoint = new PointGeo((float)MinX, (float)MaxY);
            //borderRect.EndPoint = new PointGeo((float)MaxX, (float)MinY);
            ShapeMeta borderStyle = new ShapeMeta();
            borderStyle.ForeColor = Color.Black;
            //borderStyle.BackColor = Color.Black;
            //borderStyle.IsFill = true;
            borderStyle.LineWidth = 2;
            BottomLine.SetDrawMeta(borderStyle);
            TopLine.SetDrawMeta(borderStyle);
            LeftLine.SetDrawMeta(borderStyle);
            RightLine.SetDrawMeta(borderStyle);
            this.fixedLayerManager.Add(BottomLine);
            this.fixedLayerManager.Add(TopLine);
            this.fixedLayerManager.Add(LeftLine);
            this.fixedLayerManager.Add(RightLine);
        }
        private void DrawInterva()
        {
            for (int i = 0; i < XIntervalCount; i++)
            {
                PointGeo xs = new PointGeo((float)(this.MinX + (MaxX - MinX) * i / XIntervalCount), (float)MinY);
                PointGeo xe = new PointGeo((float)(this.MinX + (MaxX - MinX) * i / XIntervalCount), (float)MinY + (float)Math.Abs(5.0 / scaleP.Y));
                LineGeo xline = new LineGeo(xs, xe);
                xline.SetDrawMeta(IntervalMeta);
                this.fixedLayerManager.Add(xline);
            }
            for (int i = 0; i < YIntervalCount; i++)
            {
                PointGeo ys = new PointGeo((float)MinX, (float)(this.MinY + (MaxY - MinY) * i / YIntervalCount));
                PointGeo ye = new PointGeo(-(float)(5.0 / scaleP.X) + (float)MinX, (float)(this.MinY + (MaxY - MinY) * i / YIntervalCount));
                LineGeo yline = new LineGeo(ys, ye);
                yline.SetDrawMeta(IntervalMeta);
                this.fixedLayerManager.Add(yline);
            }
        }
        private void DrawTickText()
        {
            for (int i = 0; i < XIntervalCount; i++)
            {
                PointGeo xs = new PointGeo((float)(this.MinX + (MaxX - MinX) * i / XIntervalCount), (float)MinY - Math.Abs(DrawAxesFont.Height / 2 / scaleP.Y));
                DrawableText drawableText = new DrawableText();
                drawableText.pos = xs;
                TextMeta textMeta = new TextMeta(xs.X.ToString("f2"));
                textMeta.TEXTFONT = DrawAxesFont;
                textMeta.ForeColor = Color.Black;
                drawableText.SetDrawMeta(textMeta);
                this.fixedLayerManager.Add(drawableText);
            }
            for (int i = 0; i < YIntervalCount; i++)
            {
                PointGeo ys = new PointGeo((float)MinX - 60 / scaleP.X, (float)(this.MinY + (MaxY - MinY) * i / YIntervalCount));
                PointGeo pos = new PointGeo((float)MinX - 60 / scaleP.X, (float)(this.MinY - DrawAxesFont.Height / 2 / fixedLayerManager.GetPainter().Scale.Y + (MaxY - MinY) * i / YIntervalCount));
                DrawableText drawableText = new DrawableText();
                drawableText.pos = pos;
                TextMeta textMeta = new TextMeta(ys.Y.ToString("f2"));
                textMeta.TEXTFONT = DrawAxesFont;
                textMeta.stringFormat.Alignment = StringAlignment.Near;
                textMeta.ForeColor = Color.Black;
                drawableText.SetDrawMeta(textMeta);
                this.fixedLayerManager.Add(drawableText);
            }
        }
        private void DrawTitleText()
        {
            DrawableText title = new DrawableText();
            TextMeta text = new TextMeta(Title) { TEXTFONT = DrawTitleFont, ForeColor = Color.Black, stringFormat = new StringFormat() { Alignment = StringAlignment.Center } };
            title.SetDrawMeta(text);
            title.pos = new PointGeo((float)(this.MaxX + this.MinX) / 2, (float)(this.MaxY + Math.Abs(DrawTitleFont.Height * 1.0f / this.fixedLayerManager.GetPainter().Scale.Y)));
            this.fixedLayerManager.Add(title);

            DrawableText xLablel = new DrawableText();
            TextMeta xLabelText = new TextMeta(XLabel) { TEXTFONT = DrawLabelFont, ForeColor = Color.Black, stringFormat = new StringFormat() { Alignment = StringAlignment.Center } };
            xLablel.SetDrawMeta(xLabelText);
            xLablel.pos = new PointGeo((float)(this.MaxX + this.MinX) / 2, (float)(this.MinY - Math.Abs(DrawLabelFont.Height * 1.0f / this.fixedLayerManager.GetPainter().Scale.Y)));
            this.fixedLayerManager.Add(xLablel);
        }
        public void OnLoad(object sender, EventArgs e)
        {
            this.Width = (sender as Form).ClientRectangle.Width;
            this.Height = (sender as Form).ClientRectangle.Height;

            SetCanvasTranlate(this.Width, this.Height);
            Init();

        }
        public void OnSizeChanged(object sender, EventArgs e)
        {
            this.Width = (sender as Form).ClientRectangle.Width;
            this.Height = (sender as Form).ClientRectangle.Height;
            if (fixedLayerManager != null)
            {
                this.fixedLayerManager.Clear();
                Init();
            }

        }
        public void OnPaint(object sender, PaintEventArgs e)
        {
            DrawLegend(e.Graphics);
            DrawOnCanvas(e.Graphics);
        }
        private void DrawLegend(Graphics g)
        {
            int SpanHeight = 30;
            if (lines.Keys.Count == 0)
            {
                return;
            }
            g.DrawRectangle(new Pen(Color.Black, 2), new Rectangle(0, 0, 200, SpanHeight * (this.lines.Count + 1)));
            int index = 0;
            foreach (var item in lines)
            {
                g.DrawLine(new Pen(item.Value.GetDrawMeta().ForeColor, 1), new Point(5, SpanHeight / 2 + SpanHeight * index), new Point(30, SpanHeight / 2 + SpanHeight * index));
                g.DrawString(item.Key, DrawAxesFont, new SolidBrush(Color.Black), new PointF(35, SpanHeight * index));
                index++;
            }
        }
        public void OnMouseMove(object sender, MouseEventArgs e)
        {
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
        public void OnMouseDown(object sender, MouseEventArgs e)
        {
            //this.AddNewLine("测试");
            //this.AddPoint("测试", new PointGeo(e.X, e.Y));
        }
        public void DrawUnitVector()
        {
            UnitVector.ClearShape();
            float length = Math.Min(this.Width, this.Height);
            length = length / 12;
            //X轴
            ShapeMeta XMeta = new ShapeMeta();
            XMeta.LineWidth = 3;
            XMeta.ForeColor = Color.Green;
            UnitVector.AddShape(new LineGeo(new PointGeo(length * 3, 0), new PointGeo(length * 2, length / 6)).SetDrawMeta(XMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(length * 2, length / 6), new PointGeo(length * 2, -length / 6)).SetDrawMeta(XMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(length * 2, -length / 6), new PointGeo(length * 3, 0)).SetDrawMeta(XMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(0, 0), new PointGeo(length * 2, 0)).SetDrawMeta(XMeta));


            //Y轴
            ShapeMeta YMeta = new ShapeMeta();
            YMeta.LineWidth = 3;
            YMeta.ForeColor = Color.Red;
            UnitVector.AddShape(new LineGeo(new PointGeo(0, length * 3), new PointGeo(length / 6, length * 2)).SetDrawMeta(YMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(length / 6, length * 2), new PointGeo(-length / 6, length * 2)).SetDrawMeta(YMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(-length / 6, length * 2), new PointGeo(0, length * 3)).SetDrawMeta(YMeta));
            UnitVector.AddShape(new LineGeo(new PointGeo(0, 0), new PointGeo(0, length * 2)).SetDrawMeta(YMeta));

            //			foreach (var element in AxisGeo.GetShapes()) {
            //				element.Translate(WinFormPainter.OffsetPoint);
            //			}
            UnitVector.Draw(this.graphicLayerManager.GetPainter());
            DrawableText xlabel = new DrawableText();
            TextMeta xtextMeta = new TextMeta("X");
            xtextMeta.ForeColor = Color.Green;
            xtextMeta.TEXTFONT = new Font(DrawLabelFont, DrawLabelFont.Style);
            xlabel.SetDrawMeta(xtextMeta);
            xlabel.pos = new PointGeo(4 * length, 0);
            xlabel.Draw(this.graphicLayerManager.GetPainter());

            DrawableText ylabel = new DrawableText();
            TextMeta ytextMeta = new TextMeta("Y");
            ytextMeta.ForeColor = Color.Red;
            ytextMeta.TEXTFONT = new Font(DrawLabelFont, DrawLabelFont.Style);
            ylabel.SetDrawMeta(ytextMeta);
            ylabel.pos = new PointGeo(0, 4 * length);
            ylabel.Draw(this.graphicLayerManager.GetPainter());
        }
    }
}
