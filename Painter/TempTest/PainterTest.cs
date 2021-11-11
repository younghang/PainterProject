using Painter.Models;
using Painter.Models.CmdControl;
using Painter.Models.PainterModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.TempTest
{
    public partial class PainterTest : Form
    {
        CanvasModel canvasModel = new CanvasModel();
        private TextBox textBox = new TextBox();
        private TextMeta textMeta = new TextMeta("请输入文本")  
        {
            ForeColor = Color.Black,
                TEXTFONT = new Font("Arial", 16f),
                stringFormat = new StringFormat() { Alignment = StringAlignment.Near }
        } ;
        PainterInputCmds inputs = new PainterInputCmds();
        static List<DrawableObject> selectDrawableObjects = new List<DrawableObject>();
        public PainterTest()
        {
            InitializeComponent();
            this.DoubleBuffered = true;//防止频闪
            this.Load += OnLoad;
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;
            this.MouseDown += OnMouseDown;
            this.MouseMove += OnMouseMove;
            this.MouseUp += OnMouseUp;
            this.SizeChanged += OnSizeChanged;
            this.Paint += OnPaint;
            this.MouseWheel += OnMouseWheel;
            this.FormClosing += (o, e) =>
            {
                if (this.canvasModel != null)
                {
                    this.canvasModel.Clear();
                } 
            };
            canvasModel.Invalidate += this.Invalidate;
            canvasModel.CmdMsgEvent += CanvasModel_CmdMsgEvent;
        }

        private void CanvasModel_CmdMsgEvent(string msg)
        {
            info.Text = msg;
        }

        void OnLoad(object sender, EventArgs e)
        {
            BackColor = Color.White;
            if (canvasModel == null)
            {
                return;
            }
            this.Controls.Add(textBox);
            textBox.Visible = false;
            canvasModel.OnLoad(sender, e);
            canvasModel.ShapeClickEvent += CanvasModel_ShapeClickEvent;
            inputs.CMDEvent += Inputs_CMDEvent;
            this.LoadElements();
            this.LoadControls();
        }

        private void Inputs_CMDEvent(PAINT_CMDS obj)
        {
            switch (obj)
            {
                case PAINT_CMDS.NONE:
                    break;
                case PAINT_CMDS.SHOWPANEL:
                    this.canvasModel.ShowPanel(true);
                    this.Invalidate();
                    break;
                case PAINT_CMDS.HIDEPANEL:
                    this.canvasModel.ShowPanel(false);
                    this.Invalidate(); 
                    break;
                case PAINT_CMDS.LINE:
                    this.canvasModel.GetCmdMgr().AddCmd(new LineCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));
                    break;
                case PAINT_CMDS.CIRCLE:
                    break;
                case PAINT_CMDS.ARC:
                    break;
                case PAINT_CMDS.RECT:
                    break;
                case PAINT_CMDS.SHOWGAME:
                    new GameJumpFrm().Show();
                    break;
                case PAINT_CMDS.CLEAR:
                    break;
                case PAINT_CMDS.POLY:
                    break;
                case PAINT_CMDS.SPLINE:
                    break;
                case PAINT_CMDS.ELLIPSE:
                    break;
                case PAINT_CMDS.TEXT:
                    break;
                case PAINT_CMDS.HELP:
                    new PainterForm().Show();
                    break;
                default:
                    break;
            }
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnSizeChanged(sender, e);
           
            info.Move(-1*info.Pos);
            info.Move(new PointGeo(0, this.ClientRectangle.Height - info.Height));
            this.Invalidate();
        }
        private void OnPaint(object sender, PaintEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnPaint(sender, e);
            //e.Graphics.DrawString("这个", new Font("宋体", 20f), new SolidBrush(Color.Black), new PointF(300, 300));

        }
        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnMouseMove(sender, e);
            this.Text = canvasModel.CurObjectPoint.ToString();
        }
        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnMouseUp(sender, e);
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnKeyUp(sender, e);
            inputs.OnKeyDown(sender, e);
        }
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnKeyDown(sender, e); 
            if (e.KeyCode == Keys.Enter)//无效 这里接收不到命令按键
            {

            }
            if (e.KeyCode==Keys.S&&ModifierKeys==Keys.Control)
            {
                canvasModel.GetCmdMgr().AddCmd(new SaveOrLoadCmd(canvasModel)); 
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:

                    break;
                case Keys.Left:

                    break;
                case Keys.Up:

                    break;
                case Keys.Space:
                    break;
                case Keys.Enter:
                    
                    break;
                case Keys.Down:
                    break;
            } 
            canvasModel.OnInvalidate();
            return base.ProcessCmdKey(ref msg, keyData);
        }
        public void OnMouseWheel(object sender, MouseEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnMouseWheel(sender, e);
        }
        private void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (canvasModel == null)
            {
                return;
            }
            if (e.Button == MouseButtons.Right)
            {
                //this.contextMenuStrip1.Show(new Point(e.X + this.Left, e.Y + this.Top + 20));
            }
            canvasModel.OnMouseDown(sender, e); 

        }
        private void PainterTest_Load(object sender, EventArgs e)
        {

        }
        private void CanvasModel_ShapeClickEvent(List<DrawableObject> obj)
        {
            selectDrawableObjects = obj; 
            if (selectDrawableObjects.Count==0)
            {
                info.Text ="";
            }
            else
            {
                foreach (var item in selectDrawableObjects)
                {
                    if (item is DrawableObject)
                    {
                        item.GetDrawMeta().DashLineStyle = new float[] { 0.5f, 1, 0.5f };
                        if (item is Shape)
                        {
                            info.Text = (item as Shape).MSG;
                        } else if(item is DrawableText)
                        {
                            item.GetDrawMeta().ForeColor = Color.Gray;
                        }
                    } 
                }
            }  
        }
        GeoLabel info = new GeoLabel(400,50,new RectangleGeo());
        static Painters.ShapeMeta CurShapeMeta = new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.MediumAquamarine };

        private Action<GeoButton> myColorSelectHandler = ( button ) => { 
            ColorDialog colorDialog = new ColorDialog();
            DialogResult dialogResult = colorDialog.ShowDialog();
            Console.WriteLine(dialogResult.ToString());
            Color color = new Color();
            if (dialogResult == System.Windows.Forms.DialogResult.OK)
            {
                colorDialog.Dispose();
                color = Color.FromArgb(colorDialog.Color.A, colorDialog.Color.R, colorDialog.Color.G, colorDialog.Color.B);
                if (button.Text.Contains("填充"))
                {
                    CurShapeMeta.BackColor = color;
                    foreach (var item in selectDrawableObjects)
                    {
                        ShapeMeta shapeMeta = item.GetDrawMeta() as ShapeMeta;
                        if (shapeMeta != null)
                        {
                            shapeMeta.BackColor = color;
                        }
                    }
                }
                if (button.Text.Contains("边框"))
                {
                    CurShapeMeta.ForeColor = color;
                    foreach (var item in selectDrawableObjects)
                    {
                        DrawMeta shapeMeta = item.GetDrawMeta() ;
                        if (shapeMeta != null)
                        {
                            shapeMeta.ForeColor = color;
                        }
                    }
                }
                button.BackColor = color;
                
            }
        };
        private void LoadControls()
        {
            info.Background = Color.FromArgb(220, 255, 255);
            info.Alignment = StringAlignment.Near;
            info.TextColor = Color.Blue;

            GeoPanel geoFilePanel = new GeoPanel(400, 50, new RectangleGeo());
            geoFilePanel.Padding = 10;

            GeoLabel fileLabel = new GeoLabel(100, 50, new RectangleGeo());
            fileLabel.Text = "文件操作";
            fileLabel.Background = Color.FromArgb(220, 255, 255);

            GeoButton geoButtonSave = new GeoButton(50, 30, new RectangleGeo());
            geoButtonSave.Text = "保存";
            geoButtonSave.ClickEvent += () => {
                info.Text = "保存  is Clicked";
                canvasModel.GetCmdMgr().AddCmd(new SaveOrLoadCmd(canvasModel,true));
            };
            GeoButton geoButtonLoad = new GeoButton(50, 30, new RectangleGeo());
            geoButtonLoad.Text = "加载";
            geoButtonLoad.ClickEvent += () => {
                info.Text = "加载 is Clicked";
                canvasModel.GetCmdMgr().AddCmd(new SaveOrLoadCmd(canvasModel, false)); 
            };
            GeoButton geoButtonUndo = new GeoButton(50, 30, new RectangleGeo());
            geoButtonUndo.Text = "撤销";
            geoButtonUndo.ClickEvent += () => {
                info.Text = "撤销  is Clicked";
                this.canvasModel.GetCmdMgr().Undo();

            };
            GeoButton geoButtonRedo = new GeoButton(50, 30, new RectangleGeo());
            geoButtonRedo.Text = "重做";
            geoButtonRedo.ClickEvent += () => {
                info.Text = "重做 is Clicked";
                this.canvasModel.GetCmdMgr().Redo(); 
            };
            GeoButton geoButtonClear = new GeoButton(50, 30, new RectangleGeo());
            geoButtonClear.Text = "清空";
            geoButtonClear.ClickEvent += () => {
                info.Text = "清空 is Clicked";
                canvasModel.GetCmdMgr().AddCmd(new ClearCmd(canvasModel));
            };
            geoFilePanel.AddControl(fileLabel);
            geoFilePanel.AddControl(geoButtonSave);
            geoFilePanel.AddControl(geoButtonLoad);
            geoFilePanel.AddControl(geoButtonUndo);
            geoFilePanel.AddControl(geoButtonRedo);
            geoFilePanel.AddControl(geoButtonClear);
            geoFilePanel.Move(new PointGeo(0, 0));

            GeoPanel geoOperatorPanel = new GeoPanel(400, 50, new RectangleGeo());
            geoOperatorPanel.Padding = 10;

            GeoLabel operateLabel = new GeoLabel(100, 50, new RectangleGeo());
            operateLabel.Text = "对象操作";
            operateLabel.Background = Color.FromArgb(220, 255, 255);

            GeoButton geoButtonRotate = new GeoButton(50, 30, new RectangleGeo());
            geoButtonRotate.Text = "旋转";
            geoButtonRotate.ClickEvent += () => {
                info.Text = "旋转  [not implement]";

            };
            GeoButton geoButtonScale = new GeoButton(50, 30, new RectangleGeo());
            geoButtonScale.Text = "缩放";
            geoButtonScale.ClickEvent += () => {
                info.Text = "缩放 [not implement]";
            };
            GeoButton geoButtonTranslate = new GeoButton(50, 30, new RectangleGeo());
            geoButtonTranslate.Text = "移动";
            geoButtonTranslate.ClickEvent += () => {
                info.Text = "移动  is Clicked";
                this.canvasModel.GetCmdMgr().AddCmd(new MoveCmd(this.canvasModel, selectDrawableObjects));
            };
            GeoButton geoButtonCopy = new GeoButton(50, 30, new RectangleGeo());
            geoButtonCopy.Text = "复制";
            geoButtonCopy.ClickEvent += () => {
                info.Text = "复制  is Clicked";
                this.canvasModel.GetCmdMgr().AddCmd(new CopyCmd(this.canvasModel, selectDrawableObjects));
            };
            GeoButton geoButtonDelete = new GeoButton(50, 30, new RectangleGeo());
            geoButtonDelete.Text = "删除";
            geoButtonDelete.ClickEvent += () => {
                info.Text = "删除 is Clicked";
                this.canvasModel.GetCmdMgr().AddCmd(new DeleteCmd(this.canvasModel, selectDrawableObjects));

            };
           
            geoOperatorPanel.AddControl(operateLabel);
            geoOperatorPanel.AddControl(geoButtonRotate);
            geoOperatorPanel.AddControl(geoButtonScale);
            geoOperatorPanel.AddControl(geoButtonTranslate); 
            geoOperatorPanel.AddControl(geoButtonCopy); 
            geoOperatorPanel.AddControl(geoButtonDelete);
            geoOperatorPanel.Move(new PointGeo(0, 50));
            canvasModel.AddGeoControls(geoOperatorPanel);

            GeoPanel geoTypePanel = new GeoPanel(400, 50, new RectangleGeo());
            GeoLabel typeLabel = new GeoLabel(100, 50, new RectangleGeo());
            typeLabel.Text = "绘图类型";
            typeLabel.Background = Color.FromArgb(220,255,255);
            LineGeo line = new LineGeo();
            line.SetDrawMeta(new Painters.ShapeMeta() {LineWidth=3,ForeColor=Color.Aquamarine });
            GeoRadio geoRadioLine = new GeoRadio(30, 30, line);
            geoRadioLine.CheckedEvent += () => {
                info.Text = "当前绘制：直线";
                this.canvasModel.GetCmdMgr().AddCmd(new LineCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            }; 

            ArcGeo arc = new ArcGeo();
            arc.SetDrawMeta(new Painters.ShapeMeta() { LineWidth = 3, ForeColor = Color.Aquamarine });
            GeoRadio geoRadioArc = new GeoRadio(30, 30, arc);
            geoRadioArc.CheckedEvent += () => {
                info.Text = "当前绘制：圆弧";
                this.canvasModel.GetCmdMgr().AddCmd(new ArcCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            };

            ArcGeo bezier = new ArcGeo();
            bezier.SetDrawMeta(new Painters.ShapeMeta() { LineWidth = 2, ForeColor = Color.Orange });
            GeoRadio geoRadioBezier = new GeoRadio(30, 30, bezier);
            geoRadioBezier.LoadDrawableFile("pen.dat");
            geoRadioBezier.CheckedEvent += () => {
                info.Text = "当前绘制：样条线";
                this.canvasModel.GetCmdMgr().AddCmd(new ArcCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            };

            GeoRadio geoRadioCircle = new GeoRadio(40, 40, new CircleGeo());
            geoRadioCircle.CheckedEvent += () => {
                info.Text ="当前绘制：圆圈";
                this.canvasModel.GetCmdMgr().AddCmd(new CircleCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            };
            GeoRadio geoRadioRect = new GeoRadio(36, 36, new RectangleGeo());
            geoRadioRect.CheckedEvent += () => {
                info.Text = "当前绘制：矩形";
                this.canvasModel.GetCmdMgr().AddCmd(new RectangleCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            };
            GeoRadio geoRadioEllipse = new GeoRadio(60, 30, new EllipseGeo());
            geoRadioEllipse.CheckedEvent += () => {
                info.Text = "当前绘制：椭圆";
                this.canvasModel.GetCmdMgr().AddCmd(new EllipseCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            };
            PolygonGeo polygon = new PolygonGeo();
            polygon.AddPoint(new PointGeo(0, 15));
            polygon.AddPoint(new PointGeo(15, 30));
            polygon.AddPoint(new PointGeo(30, 15));
            polygon.AddPoint(new PointGeo(15, 0));
            polygon.IsHold = false;
            polygon.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 2, ForeColor = Color.MediumAquamarine, BackColor = Color.Aquamarine });
            GeoRadio geoRadioPolygon = new GeoRadio(30, 30,new RectangleGeo(), polygon);
            geoRadioPolygon.CheckedEvent += () => {
                info.Text = "当前绘制：多边形，ESC取消输入"; 
                this.canvasModel.GetCmdMgr().AddCmd(new PolygonCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));

            }; 
            geoRadioPolygon.Background = Color.Transparent;

            geoRadioCircle.IsChecked = true; 

            geoTypePanel.AddControl(typeLabel); 
            geoTypePanel.AddControl(geoRadioLine);  
            geoTypePanel.AddControl(geoRadioArc);  
            geoTypePanel.AddControl(geoRadioBezier);  
            geoTypePanel.AddControl(geoRadioCircle);
            geoTypePanel.AddControl(geoRadioRect);
            geoTypePanel.AddControl(geoRadioEllipse);
            geoTypePanel.AddControl(geoRadioPolygon);
            geoTypePanel.Move(new PointGeo(0, 100));

            GeoPanel geoTypePanel2 = new GeoPanel(400, 50, new RectangleGeo());
            GeoLabel typeLabel2 = new GeoLabel(100, 50, new RectangleGeo());
            typeLabel2.Text = "";
            typeLabel2.Background = Color.FromArgb(220, 255, 255);
            geoTypePanel2.AddControl(typeLabel2);

            RectangleGeo circleGeoText = new RectangleGeo();
            circleGeoText.SetDrawMeta(new Painters.ShapeMeta() { LineWidth = 1, ForeColor = Color.Black, IsFill = true });
            GeoRadio geoRadioText = new GeoRadio(30, 30, circleGeoText);
            geoRadioText.CheckedEvent += () => {
                info.Text = "当前绘制：文本（ 轨迹 图像）";
                geoTypePanel.ClearRadioSelection();
                TextCmd textCmd = new TextCmd(this.canvasModel, textMeta);
                textMeta.ForeColor = CurShapeMeta.ForeColor;
                textCmd.ShowTextBox += (str) => {
                    textBox.Font = textMeta.TEXTFONT;
                    textBox.TextAlign = (HorizontalAlignment)textMeta.stringFormat.Alignment;
                    textBox.Text = str;
                    textBox.Location = canvasModel.CurScreenPoint;
                    textBox.Visible = true;
                };
                textCmd.GetTextBox += () =>
                {
                    textBox.Visible = false;
                    return textBox.Text;
                };
                this.canvasModel.GetCmdMgr().AddCmd(textCmd);
            };
            geoRadioText.Text = " T";
            geoRadioText.BackColor = Color.White;

            CurveGeo curve = new CurveGeo();
            curve.AddPoint(new PointGeo(0, 15));
            curve.AddPoint(new PointGeo(15, 30));
            curve.AddPoint(new PointGeo(30, 15));
            curve.AddPoint(new PointGeo(15, 0));
            curve.IsHold = false;
            curve.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 2, ForeColor = Color.MediumAquamarine, BackColor = Color.Aquamarine });
            GeoRadio geoRadioCurve = new GeoRadio(30, 30, new RectangleGeo(), curve);
            geoRadioCurve.CheckedEvent += () => {
                info.Text = "当前绘制：曲线，ESC取消输入";
                geoTypePanel.ClearRadioSelection();
                this.canvasModel.GetCmdMgr().AddCmd(new CurveCmd(this.canvasModel, CommonUtils.CloneObject<Painters.ShapeMeta>(CurShapeMeta)));
            };
            geoRadioCurve.Background = Color.Transparent;

            geoTypePanel2.AddControl(geoRadioCurve); 
            geoTypePanel2.AddControl(geoRadioText);


            geoTypePanel2.Move(new PointGeo(0, 150));

            GeoPanel geoColorPanel = new GeoPanel(400, 50, new RectangleGeo());
            GeoLabel colorLabel = new GeoLabel(100, 50, new RectangleGeo());
            colorLabel.Text = "颜色设置";
            colorLabel.Background = Color.FromArgb(220, 255, 255);

            GeoButton geoButton = new GeoButton(60, 30, new RectangleGeo());
            geoButton.Text = "边框色";
            geoButton.ClickEvent += () => {
                info.Text = "边框色  is Clicked";
                myColorSelectHandler(geoButton);
            };
            GeoButton geoButton2 = new GeoButton(60, 30, new RectangleGeo());
            geoButton2.Text = "填充色";
            geoButton2.ClickEvent += () => {
                myColorSelectHandler(geoButton2);
            };
            GeoLabel fillLabel = new GeoLabel(60, 30, new RectangleGeo());
            fillLabel.Text = "填充";
            fillLabel.Background = Color.FromArgb(220, 255, 255);

            CircleGeo circleGeo = new CircleGeo();
            circleGeo.SetDrawMeta(new Painters.ShapeMeta() { LineWidth = 2, ForeColor = Color.LimeGreen, IsFill = true });
            CircleGeo circleGeo2 = new CircleGeo();
            circleGeo2.SetDrawMeta(new Painters.ShapeMeta() { LineWidth = 2, ForeColor = Color.Red, IsFill = true });
            GeoRadio geoRadioY = new GeoRadio(30, 30, circleGeo);
            GeoRadio geoRadioN = new GeoRadio(30, 30, circleGeo2);
            geoRadioY.CheckedEvent += () => {
                info.Text = "当前绘制： 填充";
                CurShapeMeta.IsFill = true;
            };
            geoRadioN.CheckedEvent += () => {
                info.Text = "当前绘制：不填充";
                CurShapeMeta.IsFill = false; 
            };
            geoRadioY.Text = " Y";
            geoRadioY.BackColor = Color.White;
            geoRadioY.IsChecked = true;
            geoRadioN.Text = " N";
            geoRadioN.BackColor = Color.White;
            geoRadioN.Move(new PointGeo(10, 0));

            geoColorPanel.AddControl(colorLabel);
            geoColorPanel.AddControl(geoButton);
            geoColorPanel.AddControl(geoButton2); 
            geoColorPanel.AddControl(fillLabel); 
            geoColorPanel.AddControl(geoRadioY); 
            geoColorPanel.AddControl(geoRadioN); 
            geoColorPanel.Move(new PointGeo(0, 200));

            GeoPanel geoPenPanel = new GeoPanel(400, 50, new RectangleGeo());
            GeoLabel penLabel = new GeoLabel(100, 50, new RectangleGeo());
            penLabel.Text = "画笔设置";
            penLabel.Background = Color.FromArgb(220, 255, 255);

            GeoLabel lineWidthLabel = new GeoLabel(60, 40, new RectangleGeo());
            lineWidthLabel.Text = "线宽";
            lineWidthLabel.Background = Color.Transparent;
            lineWidthLabel.Move(new PointGeo(0, 0));

            GeoEditBox geoEditBox = new GeoEditBox(60, 30, new RectangleGeo());
            geoEditBox.Text = "2";
            geoEditBox.Move(new PointGeo(0, 0));
            geoEditBox.TextChangeEvent += (str) =>
            {
                float lw = CurShapeMeta.LineWidth;
                float.TryParse(str, out lw);
                CurShapeMeta.LineWidth = lw;
                foreach (var item in selectDrawableObjects)
                {
                    ShapeMeta shapeMeta = item.GetDrawMeta() as ShapeMeta;
                    if (shapeMeta != null)
                    {
                        shapeMeta.LineWidth = lw==0?1:lw;
                    }
                }
            };

            GeoLabel lineStyleLabel = new GeoLabel(60, 40, new RectangleGeo());
            lineStyleLabel.Text = "线型";
            lineStyleLabel.Background = Color.Transparent;
            lineStyleLabel.Move(new PointGeo(0, 0));


            geoPenPanel.AddControl(penLabel); 
            geoPenPanel.AddControl(lineWidthLabel);
            geoPenPanel.AddControl(geoEditBox);
            geoPenPanel.AddControl(lineStyleLabel);
            geoPenPanel.Move(new PointGeo(0, 250));
            this.canvasModel.AddGeoControls(geoTypePanel);
            this.canvasModel.AddGeoControls(geoTypePanel2);
            this.canvasModel.AddGeoControls(geoColorPanel);
            this.canvasModel.AddGeoControls(geoPenPanel);
            info.Move(new PointGeo(0, this.ClientRectangle.Height - info.Height));



            this.canvasModel.AddGeoControls(geoFilePanel);

            this.canvasModel.AddGeoControls(info);

        }
        private void LoadElements()
        {
            
            CircleGeo circleGeo = new CircleGeo() { FirstPoint = new PointGeo(100, 100), SecondPoint = new PointGeo(200, 200) };
            circleGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, BackColor = Color.MediumAquamarine });
            this.canvasModel.GetFreshLayerManager().Add(circleGeo); 
            
            LineGeo lineGeo=new LineGeo() { FirstPoint = new PointGeo(300, 100), SecondPoint = new PointGeo(1000, 300) };
            lineGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true,LineWidth=5, ForeColor = Color.LimeGreen });
            this.canvasModel.GetFreshLayerManager().Add(lineGeo);
            
            LineGeo lineGeo2 = new LineGeo() { FirstPoint = new PointGeo(300, 300), SecondPoint = new PointGeo(800, 300) };
            lineGeo2.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.Orange });
            this.canvasModel.GetFreshLayerManager().Add(lineGeo2);

            LineGeo lineGeo3 = new LineGeo() { FirstPoint = new PointGeo(400, 500), SecondPoint = new PointGeo(400, 1000) };
            lineGeo3.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.MediumAquamarine });
            this.canvasModel.GetFreshLayerManager().Add(lineGeo3);

            ArcGeo arcGeo = new ArcGeo() { FirstPoint = new PointGeo(300, 100), SecondPoint = new PointGeo(1000, 300) };
            arcGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.MediumAquamarine });
            arcGeo.StartAngle = 0;
            arcGeo.EndAngle = 72;
            arcGeo.CenterX = 400;
            arcGeo.CenterY = 600;
            arcGeo.Radius = 300;
            this.canvasModel.GetFreshLayerManager().Add(arcGeo);

            PolygonGeo polygonGeo = new PolygonGeo();
            polygonGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.MediumAquamarine,BackColor=Color.GreenYellow });
            polygonGeo.AddPoint(new PointGeo(1000,0));
            polygonGeo.AddPoint(new PointGeo(1100,-100));
            polygonGeo.AddPoint(new PointGeo(1300,-180)); 
            polygonGeo.AddPoint(new PointGeo(1130,50));
            this.canvasModel.GetFreshLayerManager().Add(polygonGeo);


            RoundRec roundRect = new RoundRec();
            roundRect.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, LineWidth = 5, ForeColor = Color.MediumAquamarine, BackColor = Color.GreenYellow });
            roundRect.FirstPoint = new PointGeo(600, 600);
            roundRect.SecondPoint = new PointGeo(1200, 1000);
            roundRect.ThirdPoint = new PointGeo(1200, 1020);
            this.canvasModel.GetFreshLayerManager().Add(roundRect);
            roundRect.Translate(new PointGeo(200, 200));


            RectangleGeo rectangeBackground = new RectangleGeo(new PointGeo(0, 0), new PointGeo(200, 500));
            rectangeBackground.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Aquamarine, IsFill = true });
            
            RectangleGeo rectangeHeaderBG = new RectangleGeo(new PointGeo(0, 0), new PointGeo(200, 100));
            rectangeHeaderBG.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Orange, IsFill = true });


             

            GeoPanel geoPanel = new GeoPanel(500,300,new RectangleGeo());
            
            GeoButton geoButton = new GeoButton(100, 50,new RectangleGeo()); 
            geoButton.Text = "按钮"; 
            geoButton.ClickEvent += () =>{
                info.Text = "Button1 is Clicked";
                //geoButton.IsVisible = false;
                //MessageBox.Show("Button is Clicked");
            };
            GeoButton geoButton2 = new GeoButton(100, 50, new RectangleGeo());
            geoButton2.Text = "按钮";
            geoButton2.ClickEvent += () => {
                info.Text = "Button2 is Clicked"; 
            };
            GeoButton geoButton3 = new GeoButton(50, 50, new CircleGeo()); 
      
            GeoButton geoButton4 = new GeoButton(200, 100, new RectangleGeo());

            GeoRadio geoRadio = new GeoRadio(50, 50, new CircleGeo());
            GeoRadio geoRadio2 = new GeoRadio(50, 50, new CircleGeo());

            GeoEditBox geoEditBox = new GeoEditBox(100, 50, new RectangleGeo());
            geoPanel.AddControl(geoButton);
            geoPanel.AddControl(geoButton2);
            geoPanel.AddControl(geoButton3);
            geoPanel.AddControl(geoRadio);
            geoPanel.AddControl(geoRadio2);
            geoPanel.AddControl(geoEditBox);
            geoPanel.Move(new PointGeo(1000, 800));
            this.canvasModel.AddGeoControls(geoPanel);
            //this.canvasModel.OffsetPoint = new PointGeo(500,350);
        }
    }
}
