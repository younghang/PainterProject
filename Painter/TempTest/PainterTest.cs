using Painter.Models;
using Painter.Models.CmdControl;
using Painter.Models.PainterModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.TempTest
{
    public partial class PainterTest : Form
    {
        CanvasModel canvasModel = new CanvasModel();
        InputCmds inputs = new InputCmds();
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
        }
        void OnLoad(object sender, EventArgs e)
        {
            BackColor = Color.White;
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnLoad(sender, e);
            canvasModel.ShapeClickEvent += CanvasModel_ShapeClickEvent;
            inputs.CMDEvent += Inputs_CMDEvent;
            this.LoadElements();
        }

        private void Inputs_CMDEvent(CMDS obj)
        {
            switch (obj)
            {
                case CMDS.NONE:
                    break;
                case CMDS.AUTO_FOCUS:
                    new GameJumpFrm().Show();
                    break;
                case CMDS.DISABLE_FOCUS:
                    break;
                case CMDS.INTERFER_ON:
                    break;
                case CMDS.INTERFER_OFF:
                    break;
                case CMDS.MOMENTA_ON:
                    break;
                case CMDS.MOMENTA_OFF:
                    break;
                case CMDS.TRACK_ON:
                    break;
                case CMDS.TRACK_OFF:
                    break;
                case CMDS.NEXT_SCENE:
                    break;
                case CMDS.FOR_SCENE:
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
        private void CanvasModel_ShapeClickEvent(Geo obj)
        {
            List<Shape> shapes = obj.GetShapes();
            if (shapes.Count==0)
            {
               
            }else
            {
                
            }
            foreach (var item in shapes)
            {
                info.Text = item.MSG;
            }
            this.Invalidate();
        }
        GeoLabel info = new GeoLabel(100,50,new RectangeGeo());
      
        private void LoadElements()
        {
            CircleGeo circleGeo = new CircleGeo() { FirstPoint = new PointGeo(100, 100), SecondPoint = new PointGeo(200, 200) };
            circleGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, BackColor = Color.MediumAquamarine });
            this.canvasModel.GetFreshLayerManager().Add(circleGeo); 

            RectangeGeo rectangeBackground = new RectangeGeo(new PointGeo(0, 0), new PointGeo(200, 500));
            rectangeBackground.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Aquamarine, IsFill = true });
            
            RectangeGeo rectangeHeaderBG = new RectangeGeo(new PointGeo(0, 0), new PointGeo(200, 100));
            rectangeHeaderBG.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Orange, IsFill = true });

             

            GeoPanel geoPanel = new GeoPanel(500,300,new RectangeGeo());
            
            GeoButton geoButton = new GeoButton(100, 50,new RectangeGeo()); 
            geoButton.Text = "按钮"; 
            geoButton.ClickEvent += () =>{
                info.Text = "Button1 is Clicked";
                //geoButton.IsVisible = false;
                //MessageBox.Show("Button is Clicked");
            };
            GeoButton geoButton2 = new GeoButton(100, 50, new RectangeGeo());
            geoButton2.Text = "按钮";
            geoButton2.ClickEvent += () => {
                info.Text = "Button2 is Clicked"; 
            };
            GeoButton geoButton3 = new GeoButton(50, 50, new CircleGeo());
            GeoButton geoButton4 = new GeoButton(200, 100, new RectangeGeo());
            geoPanel.AddControl(geoButton);
            geoPanel.AddControl(geoButton2);
            geoPanel.AddControl(geoButton3);
            geoPanel.Move(new PointGeo(500, 200));
            this.canvasModel.AddGeoControls(geoPanel);
            this.canvasModel.AddGeoControls(geoButton4);

        }
    }
}
