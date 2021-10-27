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
            if (e.KeyCode==Keys.Enter)
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
                    if (character.Status==SCENE_OBJECT_STATUS.IN_GROUND)
                    {
                        character.Speed.Y = 2; 
                    }
                    break;
                case Keys.Space:
                    character.Speed.Y = 2;
                    break;
                case Keys.Enter:
                    Pause_Click();
                    break;
                case Keys.Down:
                    character.Move(new PointGeo(0,-10));
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
            if (e.Button == MouseButtons.Left&&character != null && character.CurrenScene != null)
            {
                character.OnFire(canvasModel.ClickPoint); 
            }
        }
        Scene scene;
        MainCharacter character = new MainCharacter();
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
            //Debug.Print(character.Speed.Y + "");
        }

        private void InitScene()
        {
            GroundObject groundObj = new GroundObject();
            RectangeGeo groundRec = new RectangeGeo(new PointGeo(0, 0), new PointGeo(1200, 100));
            groundRec.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2,IsFill=false,BackColor=Color.BlueViolet });
            groundObj.Add(groundRec);
            groundObj.ReflectResistance = 1E-6f;

            GroundObject groundObj2 = new GroundObject();
            RectangeGeo groundRec2 = new RectangeGeo(new PointGeo(1500, 0), new PointGeo(2700, 100));
            groundRec2.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj2.Add(groundRec2);

            GroundObject groundObj3 = new GroundObject();
            RectangeGeo groundRec3 = new RectangeGeo(new PointGeo(1500, 300), new PointGeo(2700, 400));
            groundRec3.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj3.Add(groundRec3);

            character.Move(new PointGeo(100, 200));

            Enemy enemy = new Enemy();
            enemy.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble()*5, (float) new Random(System.DateTime.UtcNow.Millisecond).NextDouble()*5);
            enemy.Move(enemy.Speed*100);
            scene.AddObject(groundObj);
            scene.AddObject(groundObj2);
            scene.AddObject(groundObj3);
            scene.AddObject(enemy, true); 
            scene.AddObject(character,true);
        }
        bool isPause = false;
        private void Pause_Click()
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

        private void GameJumpFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
