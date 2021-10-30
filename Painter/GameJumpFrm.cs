using Painter.Models;
using Painter.Models.Paint;
using Painter.Models.PhysicalModel; 
using Painter.Models.StageModel;
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
using Utils;

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
                stageController.Stop();
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
                character.OnFire(canvasModel.CurObjectPoint); 
            }
        } 
        StageManager stageManager = new StageManager();//多场景下使用的
        MainCharacter character ; 
        StageController stageController;
        IStageScene scene =new FirstStageScene();//现在只有一个场景，先不管
        private void btnStart_Click(object sender, EventArgs e)
        { 
            if (scene != null)
            {
                scene.Clear();
            } 
            stageController = new StageController(scene.CreateScene(),this.canvasModel);
            stageController.Invalidate += this.Invalidate;
            character = scene.GetMainCharacter();
            btnStart.Visible = false;
            this.Focus();
            stageController.Start();
        } 
        private bool isPause = false;
        private void Pause_Click()
        {
            isPause = !isPause;
            if (isPause)
            {
                stageController.Pause();
            }
            else
            {
                stageController.Resume();
            } 
        }

        private void GameJumpFrm_Load(object sender, EventArgs e)
        {

        }
    }
}
