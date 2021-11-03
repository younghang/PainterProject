using Painter.Models;
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

namespace Painter
{
    public partial class TestForm : Form
    {
        public TestForm()
        {
            InitializeComponent(); 
            this.Paint += TestForm_Paint;
            this.MouseMove += TestForm_MouseMove;
            this.MouseDown += TestForm_MouseDown;
            this.DoubleBuffered = true; 
        }
        PointGeo oldPoint = new PointGeo(0,0);
        PointGeo oldOffsetPoint = new PointGeo(0,0);
        private void TestForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Middle)
            {
                oldPoint.X = e.X;
                oldPoint.Y = e.Y;
                oldPoint=ScreenPosToGraphics(e.X, e.Y);
                oldOffsetPoint = this.Painter.OffsetPoint.Clone();
            } 
        }

        private void TestForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button==MouseButtons.Middle)
            {
                PointGeo newPoint =ScreenPosToGraphics(e.X, e.Y);
                PointGeo offsetPoint = newPoint-oldPoint;
                this.Painter.OffsetPoint = oldOffsetPoint+ offsetPoint;
                this.Invalidate();
            }
            
            
        }
        ArcGeo arcGeo = new ArcGeo();
        Bitmap bitmap;
        Graphics graphics;
        WinFormPainter Painter;
        private void TestForm_Paint(object sender, PaintEventArgs e)
        {
            graphics.Clear(Color.Transparent);
            arcGeo.Draw(); 
            Graphics g = e.Graphics;
            g.DrawImage(this.bitmap,0,0);
            g.DrawRectangle(new Pen(Color.Black, 3), this.Width / 2, this.Height / 2, 100, 200);
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            bitmap = new Bitmap(this.Width, this.Height);
            graphics = Graphics.FromImage(bitmap);
            Painter = new WinFormPainter(graphics, bitmap);
            Painter.Scale = new PointGeo(1, 1);
            Painter.OffsetPoint = new PointGeo(0, 0);
            arcGeo.StartAngle = 0;
            arcGeo.EndAngle = 90;
            arcGeo.CenterX = this.Width / 2;
            arcGeo.CenterY = this.Height / 2;
            arcGeo.Radius = 100;
            arcGeo.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Red,LineWidth=2 });
            arcGeo.SetPainter(Painter);

        }
        float RotateAngle = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            RotateAngle += 15;
            Rotate(RotateAngle); 
            this.Invalidate();
        }
        private void Rotate(float angle)
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
        private PointGeo ScreenPosToGraphics(int x,int y)
        {
            PointGeo offset = new PointGeo();
            offset.X = x - this.Width / 2;
            offset.Y = y - this.Height / 2;
            double rad = -RotateAngle / 180 * Math.PI;
            PointGeo newPoint = new PointGeo(0, 0);
            newPoint.X = (float)(offset.X * Math.Cos(rad) - offset.Y * Math.Sin(rad));
            newPoint.Y = (float)(offset.X * Math.Sin(rad) + offset.Y * Math.Cos(rad));
            newPoint.X += this.Width / 2;
            newPoint.Y += this.Height / 2;  
            return newPoint;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
        }
    }
}
