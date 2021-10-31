using Painter.Models;
using Painter.Models.CameraModel;
using Painter.Models.CmdControl;
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
        InputCmds inputs = new InputCmds();
        Camera camera;
        StageManager stageManager = new StageManager();//多场景下使用的
        MainCharacter character;
        StageController stageController;
        IStageScene curStage;//现在只有一个场景 
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
                if (stageController!=null)
                {
                    stageController.Stop();
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
            inputs.OnKeyDown(sender, e);
            if (e.KeyCode == Keys.Enter)//无效 这里接收不到命令按键
            {

            }
            if (e.KeyCode == Keys.Q) 
            {
               ( curStage as FirstStageScene).LoadEnemys();
            }
            if (e.KeyCode == Keys.W) 
            {
                (curStage as FirstStageScene).LoadFallingObstacles();
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
        private void btnStart_Click(object sender, EventArgs e)
        { 
            if (curStage != null)
            {
                curStage.Clear();
            }
            btnStart.Visible = false;
            this.Focus();

            stageController.Start(curStage.CreateScene());
            camera = new Camera(canvasModel);
            camera.SetFocusObject(character); 
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
            stageController = new StageController(this.canvasModel);
            stageController.Invalidate += this.Invalidate;
            FirstStageScene firstScene= new FirstStageScene();
            SecondStageScene second = new SecondStageScene(); 
            stageManager.AddStage(firstScene);
            stageManager.AddStage(second); 
            curStage = stageManager.GetStageAt(0);
            character = curStage.GetMainCharacter();
            inputs.CMDEvent += Inputs_CMDEvent;
        }
        private void SetCurStage(IStageScene stage)
        {
            if (stage == null)
            {
                return;
            }
            curStage = stage;
            character = curStage.GetMainCharacter();
            stageController.Stop();
            stageController.Start(curStage.CreateScene());
        }
        private void Inputs_CMDEvent(CMDS cmd)
        {
            switch (cmd)
            {
                case CMDS.NONE:
                    break;
                case CMDS.AUTO_FOCUS:
                    camera.EnableFucus = true;
                    break;
                case CMDS.DISABLE_FOCUS:
                    camera.EnableFucus = false; 
                    break;
                case CMDS.INTERFER_ON:
                    character.EnableCheckCollision = true;
                    break;
                case CMDS.INTERFER_OFF:
                    character.EnableCheckCollision = false;
                    break;
                case CMDS.MOMENTA_ON:
                    StageController.EnableMomenta = true;
                    break;
                case CMDS.MOMENTA_OFF:
                    StageController.EnableMomenta = false;
                    break;
                case CMDS.TRACK_ON:
                    CanvasModel.EnableTrack = true;
                    break;
                case CMDS.TRACK_OFF:
                    CanvasModel.EnableTrack = false; 
                    break;
                case CMDS.NEXT_SCENE:
                    IStageScene next = stageManager.GetNextStage();
                    SetCurStage(next);
                    break;
                case CMDS.FOR_SCENE:
                    IStageScene stage = stageManager.GetPreViewStage();
                    SetCurStage(stage);
                    break;
                default:
                    break;
            }
        }
    }
    
}
