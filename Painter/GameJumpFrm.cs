using Painter.Models;
using Painter.Models.Paint;
using Painter.Models.PhysicalModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter
{
    public partial class GameJumpFrm : Form
    {
        CanvasModel canvasModel = new CanvasModel();
        public GameJumpFrm()
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
                timer.Stop();
                physicalField.Dispose();
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
            if (e.KeyCode==Keys.Left)
            {
               
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch(keyData)
            {
                case Keys.Right:
                    character.Speed.X = 5;
                    //character.Move( new PointGeo(10, 0)); 
                    break;
                case Keys.Left:
                    character.Speed.X = -5; 
                    //character.Move(new PointGeo(-10, 0));
                    break;
                case Keys.Up:
                    character.Speed.Y = 2;
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
        Scene scene;
        Character character = new Character();
        PhysicalField physicalField = new PhysicalField();
        Timer timer = new Timer();
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (scene != null)
            {
                scene.Clear();
            }
            scene = new Scene(canvasModel.GetHoldLayerManager(),canvasModel.GetFreshLayerManager());
            InitScene();
            physicalField.AddScene(scene);
            physicalField.Rules += (sceneObject) => { 
                 
            };
            physicalField.ApplyField();
            btnStart.Visible = false;
            this.Focus();
            Invalidate();
            timer.Interval = 15;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            this.Invalidate();
            Debug.Print(character.Speed.Y + "");
        }

        private void InitScene()
        {
            SceneObject groundObj = new SceneObject();
            RectangeGeo groundRec = new RectangeGeo(new PointGeo(0, 0), new PointGeo(1200, 100));
            groundRec.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2,IsFill=false,BackColor=Color.BlueViolet });
            groundObj.Add(groundRec);

            SceneObject groundObj2 = new SceneObject();
            RectangeGeo groundRec2 = new RectangeGeo(new PointGeo(1500, 0), new PointGeo(2700, 100));
            groundRec2.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj2.Add(groundRec2);

            character.Move(new PointGeo(100, 200));
 
            scene.AddObject(groundObj);
            scene.AddObject(groundObj2);
            scene.AddObject(character);
        }
        bool isPause = false;
        private void btnPause_Click(object sender, EventArgs e)
        {
            isPause = !isPause;
            if (isPause)
            {
                physicalField.Pause();
            }else
            {
                physicalField.Start();
            }
        }
    }
}
