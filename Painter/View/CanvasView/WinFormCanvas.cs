/*
 * Created by SharpDevelop.
 * User: DELL
 * Date: 2021/9/11
 * Time: 23:28
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Text;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using Painter.Models;
using Painter.DisplayManger;
using Painter.Painters;
using Painter.Controller;
using Painter.Utils;

namespace Painter.View.CanvasView
{
    /// <summary>
    /// Description of WinFormCanvas.
    /// 负责基本图形，轨迹及其他示意标记的绘制
    /// </summary>
    public sealed partial class WinFormCanvas : UserControl, IPainterUC
    {
        public void OnMessage(string msg)
        {
            return;
        }

        public void OnUpdatePos(double offset)
        {
            return;
        }

        public GraphicsLayerManager GetMoveableLayerManager()
        {
            return this.moveableLayerManager;
        }

        public GraphicsLayerManager GetHoldLayerManager()
        {
            return this.holdLayerManager;
        }

        public GraphicsLayerManager GetFreshLayerManager()
        {
            return this.freshLayerManager;
        }


        public WinFormCanvas()
        {
            //
            // The InitializeComponent() call is required for Windows Forms designer support.
            //
            InitializeComponent();

            //
            // TODO: Add constructor code after the InitializeComponent() call.
            //

            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.Opaque, true);

            SetLayerManagerGraphic(ref moveableLayerManager);
            SetLayerManagerGraphic(ref holdLayerManager);
            SetLayerManagerGraphic(ref freshLayerManager);

            this.Load += OnLoad;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.SizeChanged += OnSizeChanged;
            this.Paint += OnPaint;
            this.MouseWheel += OnMouseWheel;
            //this.Resize += OnSizeChanged;
        }

        #region Extended Feature 
        DRAW_TYPE currentType= DRAW_TYPE.LINE; 
        private Array lineCapStyle = Enum.GetValues(typeof(LineCap));

        internal void SetCanvasTranlate(PointGeo offsetPoint, PointGeo scaleP)
        {
            if (scaleP.X>1E6)
            {
                return;
            }
            this.freshLayerManager.GetPainter().OffsetPoint = offsetPoint;
            this.freshLayerManager.GetPainter().Scale = scaleP;
            this.moveableLayerManager.GetPainter().OffsetPoint = offsetPoint;
            this.moveableLayerManager.GetPainter().Scale = scaleP;
            this.holdLayerManager.GetPainter().OffsetPoint = offsetPoint;
            this.holdLayerManager.GetPainter().Scale = scaleP;
        }

        static Color ShapeBackColor = Color.LimeGreen;
        static Color ShapeForeColor = Color.OrangeRed;
        static Font DrawTextFont = new Font("黑体", 18, FontStyle.Bold);
        bool IsTextAdded = false;
        bool IsSaveFile = false;
        Dictionary<string, DRAW_TYPE> dicDrawTypes = new Dictionary<string, DRAW_TYPE>() { {
                "圆",
                DRAW_TYPE.CIRCLE
            }
            , { "矩形", DRAW_TYPE.RECTANGLE }, { "直线", DRAW_TYPE.LINE }, {
                "椭圆",
                DRAW_TYPE.ELLIPSE
            },
            { "铅笔", DRAW_TYPE.RANDOMLINES }
        };
        bool IsStraightLine = false;
        bool IsContinusLine = false;
        List<PointGeo> points = new List<PointGeo>();
        private static TextBox tx = null;
        #endregion


        GraphicsLayerManager moveableLayerManager = null;
        //平移的
        public GraphicsLayerManager MoveableLayerManager
        {
            get
            {
                return moveableLayerManager;
            }
        }

        //禁止不动，不刷新的,只需要在原图上继续追加
        GraphicsLayerManager holdLayerManager = null;
        public GraphicsLayerManager HoldLayerManager
        {
            get
            {
                return holdLayerManager;
            }
        }
        //会刷新，但是固定不动
        GraphicsLayerManager freshLayerManager = null;
        public GraphicsLayerManager FreshLayerManager
        {
            get
            {
                return freshLayerManager;
            }
        }
         
        PointGeo oldPoint;
        PointGeo newPoint;
        bool IsMouseDown = false;
        IScreenPrintable ips = null;

        public GraphicsLayerManager GetTrackLayerManager()
        {
            return this.holdLayerManager;
        }
        public GraphicsLayerManager GetShapeLayerManager()
        {
            return this.moveableLayerManager;
        }
        public GraphicsLayerManager GetFixedLayerManager()
        {
            return this.freshLayerManager;
        }
        private Geo AxisGeo = new Geo();
        private void AddAxis()
        {
            AxisGeo.ClearShape();
            float length = Math.Min(this.Width, this.Height);
            length = 24.0f / this.freshLayerManager.GetPainter().Scale.X;
            //X轴
            ShapeMeta XMeta = new ShapeMeta();
            XMeta.LineWidth = 3;
            XMeta.ForeColor = Color.Green;
            AxisGeo.AddShape(new LineGeo(new PointGeo(length * 3, 0), new PointGeo(length * 2, length / 6)).SetDrawMeta(XMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(length * 2, length / 6), new PointGeo(length * 2, -length / 6)).SetDrawMeta(XMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(length * 2, -length / 6), new PointGeo(length * 3, 0)).SetDrawMeta(XMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(0, 0), new PointGeo(length * 2, 0)).SetDrawMeta(XMeta));

            //Y轴
            ShapeMeta YMeta = new ShapeMeta();
            YMeta.LineWidth = 3;
            YMeta.ForeColor = Color.Red;
            AxisGeo.AddShape(new LineGeo(new PointGeo(0, length * 3), new PointGeo(length / 6, length * 2)).SetDrawMeta(YMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(length / 6, length * 2), new PointGeo(-length / 6, length * 2)).SetDrawMeta(YMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(-length / 6, length * 2), new PointGeo(0, length * 3)).SetDrawMeta(YMeta));
            AxisGeo.AddShape(new LineGeo(new PointGeo(0, 0), new PointGeo(0, length * 2)).SetDrawMeta(YMeta));

            foreach (var element in AxisGeo.GetShapes())
            {
                element.Translate(new PointGeo((float)(1.0f * ProgramController.Instance.MinX) - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X), (float)(1f * ProgramController.Instance.MinY - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X))));
                //element.Scale(1.0f/ this.freshLayerManager.GetPainter().Scale.GetAbs());
            }


            AxisGeo.Draw(this.freshLayerManager.GetPainter());
            DrawableText xlabel = new DrawableText();
            TextMeta xtextMeta = new TextMeta("X");
            xtextMeta.ForeColor = Color.Green;
            xtextMeta.TEXTFONT = new Font(DrawTextFont, DrawTextFont.Style);
            xlabel.SetDrawMeta(xtextMeta);
            xlabel.pos = new PointGeo(4 * length, -DrawTextFont.Height / 2);
            xlabel.Translate(new PointGeo((float)ProgramController.Instance.MinX - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X), (float)(ProgramController.Instance.MinY) - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X)));
            xlabel.Draw(this.freshLayerManager.GetPainter());

            DrawableText ylabel = new DrawableText();
            TextMeta ytextMeta = new TextMeta("Y");
            ytextMeta.ForeColor = Color.Red;
            ytextMeta.TEXTFONT = new Font(DrawTextFont, DrawTextFont.Style);
            ylabel.SetDrawMeta(ytextMeta);
            ylabel.pos = new PointGeo(0, 4 * length);
            ylabel.Translate(new PointGeo((float)(ProgramController.Instance.MinX - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X)), (float)(ProgramController.Instance.MinY - (float)(25.0 / this.freshLayerManager.GetPainter().Scale.X))));
            ylabel.Draw(this.freshLayerManager.GetPainter());

        }
        private void SetLayerManagerGraphic(ref GraphicsLayerManager layerGraphic)
        {
            if (layerGraphic != null)
            {
                layerGraphic.Dispose();
                Bitmap canvasBitmap = null;
                //                if (layerGraphic.GetPainter()!=null&&(layerGraphic.GetPainter() as WinFormPainter).canvas!=null) {
                //                	canvasBitmap =new Bitmap((layerGraphic.GetPainter() as WinFormPainter).canvas, this.Width, this.Height);
                //                }else{
                //                	canvasBitmap =new Bitmap( this.Width, this.Height);
                //                } 
                canvasBitmap = new Bitmap(this.Width, this.Height);
                Graphics canGraphics = Graphics.FromImage(canvasBitmap);
                canGraphics.Clip = new Region(new Rectangle(0, 0, this.Width, this.Height));
                canGraphics.Clear(Color.Transparent);
                canGraphics.SmoothingMode = SmoothingMode.AntiAlias;
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
                layerGraphic.Invalidate += Invalidate;
                SetLayerManagerGraphic(ref layerGraphic);
            }
        }
        void OnLoad(object sender, EventArgs e)
        {

            BackColor = Color.White;
            this.绘制ToolStripMenuItem.DropDownItemClicked += 绘制ToolStripMenuItem_DropDownItemClicked;
            //			lblInfo.Text = "";
            //			lblMarginColor.Text = "";
            //			lblFillColor.Text = "";
            //			lblMarginColor.BackColor = ShapeForeColor;
            //			lblFillColor.BackColor = ShapeBackColor;
            //			lblFillColor.Click += this.myColorSelectHandler;
            //			lblMarginColor.Click += this.myColorSelectHandler;
            //			tx = new TextBox();
            //			comBoxLineCap.DataSource = Enum.GetNames(typeof(LineCap));
        }

        private void 绘制ToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            foreach (var item in this.绘制ToolStripMenuItem.DropDownItems)
            {
                (item as ToolStripMenuItem).Checked = false;
            }
            (e.ClickedItem as ToolStripMenuItem).Checked = true;
            switch (e.ClickedItem.Text)
            {
                case "直线":
                    currentType = DRAW_TYPE.LINE; 
                    break;
                case "矩形":
                    currentType = DRAW_TYPE.RECTANGLE;
                    break;
                case "圆":
                    currentType = DRAW_TYPE.CIRCLE; 
                    break;
                case "椭圆":
                    currentType = DRAW_TYPE.ELLIPSE;

                    break;
                case "轨迹":
                    currentType = DRAW_TYPE.RANDOMLINES; 
                    break;
                case "文字":
                    currentType = DRAW_TYPE.TEXT;
                    break;
                default:
                    break;
            }
        }

        public void Clear()
        {
            this.moveableLayerManager.Clear();
            this.holdLayerManager.Clear();
            this.freshLayerManager.Clear();
            this.points.Clear();
            if (ips != null)
            {
                if (ips is RandomLines)
                {
                    (ips as RandomLines).Clear();

                }
                ips = null;
            }
            this.Invalidate();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                var cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        private void OnSizeChanged(object sender, EventArgs e)
        {
            RefreshCanvas();
        }
        private void RefreshCanvas()
        {
            if (freshLayerManager != null)
            {
                SetLayerManagerGraphic(ref moveableLayerManager);
                SetLayerManagerGraphic(ref holdLayerManager);//这里要记得更新，不然会截掉一部风
                SetLayerManagerGraphic(ref freshLayerManager);
                //恢复轨迹
                //                foreach (var element in this.holdLayerManager.GetDatas()) {
                //                	if (element is RandomLines) {
                //                		(element as RandomLines).ResetPoints();
                //                	}
                //                }
            }
            PainterUCManager.Instance.SetCanvasScale(this.Width, this.Height);

        }

        private void ScreenPaint()
        {
            (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().Clear(Color.Transparent);
            if (ImageFilePath != "")
            {
                try
                {
                    if (BGBitmap == null)
                    {
                        System.Drawing.Size s = new Size(this.Width, this.Height);
                        using (Image image = Bitmap.FromFile(ImageFilePath))
                        {
                            Bitmap btm = new Bitmap(image, s);
                            (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                            (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().DrawImage(btm, new PointF(0, 0));
                            btm.Dispose();
                            image.Dispose();
                        }
                    }
                    else
                    {
                        (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
                        (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().DrawImage(BGBitmap, new PointF(0, 0));
                    }

                }
                catch (Exception)
                {
                    //this.lblInfo.Text = ("图片加载失败");
                }
            }
                (freshLayerManager.GetPainter() as WinFormPainter).GetGraphics().SmoothingMode = SmoothingMode.AntiAlias;
            AddAxis();
            freshLayerManager.Draw();

        }
        object lockObj = new object();
        private void OnPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Settings.BG_COLOR);
            //if (IsLabelPaint) return; //Paint--> Draw--> OnMessageUpdate--> Paint --> Draw;
            lock (lockObj)
            {
                //先绘制底层移动部分
                moveableLayerManager.GetPainter().Clear();
                moveableLayerManager.Draw();
                e.Graphics.DrawImage((moveableLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));

                //再绘制不需要刷新的部分（只需要在原图上继续追加）
                holdLayerManager.Draw();
                e.Graphics.DrawImage((holdLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));

                //最后绘制需要实时刷新的部分
                #region freshLayer.Draw 
                ScreenPaint();
                if (currentType == DRAW_TYPE.RANDOMLINES)
                {
                    DrawMeta dm = new DrawMeta();
                    dm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
                    dm.LineWidth = 1;// float.Parse(txtLineWidth.Text);
                    dm.CapStyle = (LineCap)lineCapStyle.GetValue(0);//comBoxLineCap.SelectedIndex);
                    Pen p = new Pen(dm.ForeColor, dm.LineWidth);
                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        e.Graphics.DrawLine(p, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);

                    }
                }
                else if (currentType == DRAW_TYPE.TEXT)
                {
                }
                else
                {
                    if (IsMouseDown && ips != null)
                    {
                        DrawRealTimeShape(e);
                    }
                }
                #endregion
                e.Graphics.DrawImage((freshLayerManager.GetPainter() as WinFormPainter).GetCanvas(), new PointF(0, 0));
            }

        }
        private void DrawRealTimeShape(PaintEventArgs e)
        {
            //(shapeLayerManager.GetPainter() as WinFormPainter).GetGraphics().Clear(Color.White);
            Shape s = ips as Shape;
            if (IsStraightLine && currentType == DRAW_TYPE.LINE)
            {
                if (Math.Abs(newPoint.X - oldPoint.X) < Math.Abs(newPoint.Y - oldPoint.Y))
                {
                    newPoint.X = oldPoint.X;
                }
                else
                {
                    newPoint.Y = oldPoint.Y;
                }
            }
            Func<float, float> transXBack = (value) => (float)(value - GetFixedLayerManager().GetPainter().OffsetPoint.X) / GetFixedLayerManager().GetPainter().Scale.X;
            Func<float, float> transYBack = (value) => (float)(value - GetFixedLayerManager().GetPainter().OffsetPoint.Y) / GetFixedLayerManager().GetPainter().Scale.Y;

            s.SecondPoint = new PointGeo(transXBack(newPoint.X), transYBack(newPoint.Y));
            if (!s.HasDrawMeta())
            {
                s.FirstPoint = new PointGeo(transXBack(oldPoint.X), transYBack(oldPoint.Y));
                ShapeMeta sm = new ShapeMeta();
                sm.IsFill = false;//fillChecked.Checked;
                sm.BackColor = Color.FromArgb(ShapeBackColor.A, ShapeBackColor.R, ShapeBackColor.G, ShapeBackColor.B);
                sm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
                sm.LineWidth = float.Parse("1");//txtLineWidth.Text);
                sm.CapStyle = (LineCap)lineCapStyle.GetValue(0);//comBoxLineCap.SelectedIndex);
                s.SetDrawMeta(sm);
            }
            if (s.UpdateMessage == null)
                s.UpdateMessage += MsgHandler;

            ips.Draw(freshLayerManager.GetPainter());
            this.statusMsg.Text = s.MSG;
        }

        public void LoadShapes(List<Shape> shapes)
        {
            if (shapes != null && shapes.Count > 0)
            {
                this.moveableLayerManager.LoadShape(shapes);
            }
            this.ips = null;
            moveableLayerManager.GetPainter().Clear();
            moveableLayerManager.Draw();
            this.Invalidate();
        }

        public void AddTrack(RandomLines rl)
        {
            if (rl != null)
            {
                this.holdLayerManager.Add(rl);
            }
        }
        PointGeo oldOffSetPoint;
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            newPoint = ScreenPosToGraphics(e.X, e.Y);
            if (e.Button == MouseButtons.Middle)
            {
                PointGeo pointGeo = new PointGeo(newPoint.X - oldPoint.X, newPoint.Y - oldPoint.Y);
                this.SetCanvasTranlate(pointGeo + oldOffSetPoint, this.freshLayerManager.GetPainter().Scale);
            }
            if (IsMouseDown)
            {
                if (currentType == DRAW_TYPE.RANDOMLINES)
                    points.Add(new PointGeo(newPoint.X, newPoint.Y));

            }
            else
            {
                this.statusMsg.Text = "当前位置：" + ScreenToObjectPos(e.X, e.Y);
            }
            Invalidate(new System.Drawing.Rectangle(0, 0, this.Width, this.Height));
        }
        public float GraphicsWithScreenAngle = 0;
        public PointGeo ScreenToObjectPos(int x, int y)
        {
            //PointGeo point =ScreenPosToGraphics(x, y); 
            PointGeo point = new PointGeo(x, y);
            point.Offset(-1 * this.freshLayerManager.GetPainter().OffsetPoint);
            point.Scale(1 / this.freshLayerManager.GetPainter().Scale); 
            return point;
        }
        public Point ObjectToScreen(PointGeo point) 
        { 
            point.Scale(this.freshLayerManager.GetPainter().Scale); 
            point.Offset(this.freshLayerManager.GetPainter().OffsetPoint);
            //Point p = GraphicsPosToScreen(point);
            Point p = new Point((int)point.X,(int)point.Y);
            return p;
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (IsMouseDown && ips != null)
            {
                if (ips is Shape && (ips as Shape).HasDrawMeta())
                {
                    freshLayerManager.Add(ips);
                    ips = null;
                }
                else if (ips is RandomLines)
                {
                    RandomLines rl = (ips as RandomLines);
                    rl.IsHold = false;
                    ShapeMeta pm = new ShapeMeta();
                    pm.ForeColor = Color.FromArgb(ShapeForeColor.A, ShapeForeColor.R, ShapeForeColor.G, ShapeForeColor.B);
                    pm.LineWidth = 1;//float.Parse(txtLineWidth.Text);
                    rl.SetDrawMeta(pm);
                    List<PointGeo> listP= CommonUtils.Clone<PointGeo>(points);
                    for (int i = 0; i < listP.Count; i++)
                    {
                        listP[i] = ScreenToObjectPos((int)listP[i].X,(int)listP[i].Y);
                    }
                    rl.AddPoints(listP);
                    freshLayerManager.Add(rl);
                    ips = null;
                    points.Clear();
                }
            }
            if (IsContinusLine && currentType == DRAW_TYPE.LINE)
            {
                IsMouseDown = true;
                oldPoint = ScreenPosToGraphics(e.X, e.Y);
                currentType = DRAW_TYPE.LINE;//dicDrawTypes[(string)comBoxSelectShape.SelectedItem];
                ips = ScreenElementCreator.CreateElement(currentType);
            }
            else
            {
                IsMouseDown = false;
                //ScreenPaint();
                Invalidate();
            }
        }






        #region Extended Feature
        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    if (GetImage != null) this.GetImage(canvasBitmap);
        //    base.OnClosing(e);
        //}
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //				if (this.freshLayerManager.GetCurPos() > 5) {
                //					this.btnSave.Text = "Save";
                //					IsSaveFile = true;
                //				} else {
                //					this.btnSave.Text = "Load";
                //					IsSaveFile = false;
                //				}
                //currentType = DRAW_TYPE.LINE;//dicDrawTypes[(string)comBoxSelectShape.SelectedItem];
                if (currentType == DRAW_TYPE.RANDOMLINES)
                {
                    IsMouseDown = true;
                    ips = ScreenElementCreator.CreateElement(currentType);
                    oldPoint = ScreenPosToGraphics(e.X, e.Y);
                    points.Clear();
                    points.Add(new PointGeo(oldPoint.X, oldPoint.Y));
                    //this.lblInfo.Text = "铅笔绘制模式";
                }
                if (IsContinusLine && currentType == DRAW_TYPE.LINE && ips != null && (ips as Shape).HasDrawMeta())
                {
                    freshLayerManager.Add(ips);
                    ips = null;
                    IsMouseDown = false;
                    Invalidate();
                    //ScreenPaint();
                }
                else
                {
                    IsMouseDown = true;
                    oldPoint = ScreenPosToGraphics(e.X, e.Y);
                    if (IsDrawText)
                    {
                        currentType = DRAW_TYPE.TEXT;
                        if (IsTextAdded)
                        {
                            ips = ScreenElementCreator.CreateElement(currentType);
                            DrawableText dt = ips as DrawableText;
                            dt.pos = new PointGeo(tx.Location.X, tx.Location.Y);
                            TextMeta dm = new TextMeta(tx.Text);
                            dm.TEXTFONT = new Font(DrawTextFont, DrawTextFont.Style);
                            dm.ForeColor = ShapeForeColor;
                            dt.SetDrawMeta(dm);
                            freshLayerManager.Add(ips);
                            this.Controls.Remove(tx);
                            ips = null;
                            IsTextAdded = false;
                            return;
                        }
                        else
                        {
                            CreateTextView();
                            this.Invalidate();
                        }
                    }
                    ips = ScreenElementCreator.CreateElement(currentType);
                }
            }
            else
            {
                if (e.Button == MouseButtons.Middle)
                {
                    oldPoint = ScreenPosToGraphics(e.X, e.Y);
                    oldOffSetPoint = this.freshLayerManager.GetPainter().OffsetPoint.Clone();
                }else
                {
                    
                    this.contextMenuStrip1.Show(new System.Drawing.Point(e.Location.X +this.Parent.Parent.Parent.Location.X+ this.FindForm().Location.X, e.Location.Y + this.Parent.Parent.Parent.Location.Y+ this.FindForm().Location.Y));
                }
              
            }
        }
        public void OnMouseWheel(object sender, MouseEventArgs e)
        { 
            PointGeo point = ScreenToObjectPos(e.X, e.Y);  
            PointGeo Scale = this.freshLayerManager.GetPainter().Scale; 
            
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
            this.SetCanvasTranlate(new PointGeo(e.X-newPoint.X,e.Y-newPoint.Y) + this.freshLayerManager.GetPainter().OffsetPoint, Scale);
            Invalidate(new System.Drawing.Rectangle(0, 0, this.Width, this.Height));

        }
        private void CreateTextView()
        {
            if (tx != null)
            {
                this.Controls.Remove(tx);
            }
            tx.Location = new System.Drawing.Point((int)oldPoint.X, (int)oldPoint.Y);
            tx.Font = DrawTextFont;
            int fontSize = (int)(DrawTextFont.Size);
            tx.Size = new System.Drawing.Size(10 * fontSize, fontSize);
            tx.TabIndex = 2;
            tx.Text = "在此输入文本";
            this.Controls.Add(tx);
            tx.Focus();
            tx.SelectAll();
            IsTextAdded = true;
        }


        private Bitmap BGBitmap = null;
        public void SetImagePath(string image_path)
        {
            this.ImageFilePath = image_path;
        }
        public void SetImage(Bitmap b)
        {
            BGBitmap = b.Clone() as Bitmap;
        }
        private void MsgHandler(string msg)
        {
            this.statusMsg.Text = msg;
        }
        private void btnUndo_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            this.freshLayerManager.Undo();
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {
            this.Invalidate();
            this.freshLayerManager.Redo();
        }
        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            IsStraightLine = false;
            IsContinusLine = false;
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift)
            {
                IsStraightLine = true;
            }
            else
            {
                IsStraightLine = false;
            }
            if (e.Modifiers == Keys.Control)
            {
                IsContinusLine = true;
            }
            else
            {
                IsContinusLine = false;
            }
        }
        private EventHandler myColorSelectHandler = (o, e) =>
        {
            Label l = o as Label;
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog();
            Console.WriteLine(dialogResult.ToString());
            Color color = new Color();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                colorDialog.Dispose();
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                if (l.Tag.ToString() == "Fill")
                {
                    ShapeBackColor = color;
                }
                if (l.Tag.ToString() == "Margin")
                {
                    ShapeForeColor = color;
                }
                l.BackColor = color;
            }
        };
        bool IsDrawText = false;
        private void label4_Click(object sender, EventArgs e)
        {
            IsDrawText = !IsDrawText;
            if (IsDrawText)
            {
                this.statusMsg.Text = "插入文本, 双击[A]修改字体";
                //this.lblTextFont.BackColor = Color.Wheat;
            }
            else
            {
                this.statusMsg.Text = "绘图，点击色块修改颜色";
                //this.lblTextFont.BackColor = Color.Transparent;
            }
        }
        private void label4_DoubleClick(object sender, EventArgs e)
        {
            IsDrawText = !IsDrawText;
            if (IsDrawText)
            {
                this.statusMsg.Text = "插入文本, 双击[A]修改字体";
                //this.lblTextFont.BackColor = Color.Wheat;
            }
            else
            {
                this.statusMsg.Text = "绘图，点击色块修改颜色";
                //this.lblTextFont.BackColor = Color.Transparent;
            }
            FontDialog fd = new FontDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                DrawTextFont = fd.Font;
            }
        }
       
        string ImageFilePath = "";
        private void lblImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog() { Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp" })
            {
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (!String.IsNullOrEmpty(ofd.FileName))
                    {
                        this.ImageFilePath = ofd.FileName;
                        //ips = ScreenElementCreator.CreateElement(DRAW_TYPE.IMAGE);
                        //DrawableImage di=ips as DrawableImage;
                        //di.SetDrawMeta(new ImageMeta(this.ImageFilePath));
                        //screenManager.Add(ips);
                        //ips = null;
                        //Invalidate();
                    }
                }
            }
        }
        private void SaveImage()
        {
            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.DefaultExt = "png";
                sfd.AddExtension = true;
                sfd.Filter = "图片|*.jpg;*.png;*.gif;*.jpeg;*.bmp";
                if (sfd.ShowDialog() == DialogResult.OK && (moveableLayerManager.GetPainter() as WinFormPainter).GetCanvas() != null)
                {
                    Bitmap bitmap = new Bitmap(this.Width, this.Height);
                    this.DrawToBitmap(bitmap, this.ClientRectangle);
                    bitmap.Save(sfd.FileName);
                }
            }
        }
 
        bool IsScreenCut = false;
        private void 截取区域ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IsScreenCut = true;
        }
       
        #endregion
         
        private void ConfirmRotate_Click(object sender, EventArgs e)
        {
            float.TryParse(this.txtRotateAngle.Text ,out GraphicsWithScreenAngle);
            Rotate((this.freshLayerManager.GetPainter() as WinFormPainter).GetGraphics(), GraphicsWithScreenAngle);
            Rotate((this.holdLayerManager.GetPainter() as WinFormPainter).GetGraphics(), GraphicsWithScreenAngle);
            Rotate((this.moveableLayerManager.GetPainter() as WinFormPainter).GetGraphics(), GraphicsWithScreenAngle);
        }
        private void Rotate(Graphics graphics, float angle)
        {
            graphics.ResetTransform();
            float x = this.Width / 2;
            float y = this.Height / 2;
            double rad = angle / 180 * Math.PI;
            PointGeo newPoint = new PointGeo(0, 0);
            newPoint.X = (float)(x * Math.Cos(rad) - y * Math.Sin(rad));
            newPoint.Y = (float)(x * Math.Sin(rad) + y * Math.Cos(rad));
            PointGeo offsetPoint = new PointGeo();
            offsetPoint.X = x - newPoint.X;
            offsetPoint.Y = y - newPoint.Y;
            graphics.TranslateTransform(offsetPoint.X, offsetPoint.Y);
            graphics.RotateTransform(angle);
        }
        private PointGeo ScreenPosToGraphics(int x, int y)
        {
            PointGeo offset = new PointGeo();
            offset.X = x - this.Width / 2;
            offset.Y = y - this.Height / 2;
            double rad = -GraphicsWithScreenAngle / 180 * Math.PI;
            PointGeo newPoint = new PointGeo(0, 0);
            newPoint.X = (float)(offset.X * Math.Cos(rad) - offset.Y * Math.Sin(rad));
            newPoint.Y = (float)(offset.X * Math.Sin(rad) + offset.Y * Math.Cos(rad));
            newPoint.X += this.Width / 2;
            newPoint.Y += this.Height / 2;
            return newPoint;
        }
        private Point GraphicsPosToScreen(PointGeo pointGeo)
        {
            PointGeo offset = new PointGeo();
            offset.X = pointGeo.X - this.Width / 2;
            offset.Y = pointGeo.Y - this.Height / 2;
            double rad = GraphicsWithScreenAngle / 180 * Math.PI;
            PointGeo newPoint = new PointGeo(0, 0);
            newPoint.X = (float)(offset.X * Math.Cos(rad) - offset.Y * Math.Sin(rad));
            newPoint.Y = (float)(offset.X * Math.Sin(rad) + offset.Y * Math.Cos(rad));
            newPoint.X += this.Width / 2;
            newPoint.Y += this.Height / 2;
            return new Point((int)newPoint.X,(int)newPoint.Y);
        }
        private void SaveAsPNG_Click(object sender, EventArgs e)
        {
            SaveImage();
        }

        private void SaveAsDAT_Click(object sender, EventArgs e)
        {
            this.freshLayerManager.SaveOrLoadFile(true);
            this.ips = null;
            this.Invalidate();
        }

        private void ReadFromFile_Click(object sender, EventArgs e)
        {
            this.freshLayerManager.SaveOrLoadFile(false);
            this.ips = null;
            this.Invalidate();
        }
    }
}
