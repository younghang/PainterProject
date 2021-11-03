using Painter.Models;
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
        Geo rightSide = new Geo();
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
            this.LoadElements();
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
                rightSide.Hide();
            }else
            {
                rightSide.Show();
            }
            foreach (var item in shapes)
            {
                info.Text = item.MSG;
            }
            this.Invalidate();
        }
        TextMeta info = new TextMeta("你好") { };
        private void LoadElements()
        {
            CircleGeo circleGeo = new CircleGeo() { FirstPoint = new PointGeo(500, 500), SecondPoint = new PointGeo(200, 200) };
            circleGeo.SetDrawMeta(new Painters.ShapeMeta() { IsFill = true, BackColor = Color.MediumAquamarine });
            this.canvasModel.GetScreenManager().Add(circleGeo);
            this.canvasModel.GetFreshLayerManager().Add(circleGeo);

            RectangeGeo rectangeBackground = new RectangeGeo(new PointGeo(0, 0), new PointGeo(200, 500));
            rectangeBackground.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Aquamarine, IsFill = true });
            
            RectangeGeo rectangeHeaderBG = new RectangeGeo(new PointGeo(0, 0), new PointGeo(200, 100));
            rectangeHeaderBG.SetDrawMeta(new Painters.ShapeMeta() { BackColor = Color.Orange, IsFill = true });

            DrawableText text = new DrawableText();
            info.ForeColor = Color.Black;
            info.TEXTFONT = new Font("宋体", 26f);
            info.stringFormat = new StringFormat() {Alignment=StringAlignment.Near};
            text.SetDrawMeta(info);
            text.pos = new PointGeo(0, 37);
            
            rightSide.AddShape(rectangeBackground);
            rightSide.AddShape(rectangeHeaderBG);  
            for (int i = 0; i < rightSide.GetShapes().Count; i++)
            {
                this.canvasModel.GetScreenManager().Add(rightSide.GetShapes()[i],true); 
            }
            
            this.canvasModel.GetScreenManager().Add(text, true);
        }
    }
}
