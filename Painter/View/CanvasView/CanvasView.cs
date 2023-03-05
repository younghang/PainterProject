using Painter.Models;
using Painter.Models.CameraModel;
using Painter.Models.CmdControl;
using Painter.Models.GameModel.StageModel;
using Painter.Models.Paint;
using Painter.Models.PainterModel;
using Painter.Models.StageModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.View.CanvasView
{
    public partial class CanvasView : UserControl
    {
        CanvasModel canvasModel = new CanvasModel();
        GameInputCmds inputs = new GameInputCmds();
        ScreenCamera camera;
        StageManager stageManager = new StageManager();//多场景下使用的
        MainCharacter character; 
        StageController stageController=null;
        StageScene curStage;
        public CanvasView()
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
            this.Disposed += (o, e) =>
            {
                if (this.canvasModel != null)
                {
                    this.canvasModel.Clear();
                }
                if (stageController != null)
                {
                    stageController.Dispose();
                }
            };
            canvasModel.Invalidate += this.Invalidate;
            canvasModel.EnableClickTest = true;
            canvasModel.EnableSelect = false;
        }
        void OnLoad(object sender, EventArgs e)
        {
            BackColor = Color.White;
            if (canvasModel == null)
            {
                return;
            }
            canvasModel.OnLoad(sender, e);

            stageController = new StageController(this.canvasModel);
            stageController.Invalidate += this.Invalidate;
            //FirstStageScene firstScene = new FirstStageScene();
            //SecondStageScene second = new SecondStageScene();
            //SnakeStage snakeStage = new SnakeStage();
            //RussiaBlock russia = new RussiaBlock();
            //WeldStage weld = new WeldStage();
            //MarioStage mario = new MarioStage();
            //stageManager.AddStage(firstScene);
            //stageManager.AddStage(weld);
            //stageManager.AddStage(russia);
            //stageManager.AddStage(second);
            //stageManager.AddStage(snakeStage);
            //stageManager.AddStage(mario);
            //SetCurStage(russia);

            inputs.CMDEvent += Inputs_CMDEvent;
            isFirst = false;
             
        }
        public void AddControl(GeoControl geoControl)
        {
            this.canvasModel.AddGeoControls(geoControl);
        }
       
        float MoveDistance = 10;
        bool isGraph = false;
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
                    Pause_Click();
                    break;
                case Keys.Down:
                    break;
            }
            curStage.OnKeyDown(keyData);
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
            if (e.Button == MouseButtons.Left && character != null && character.CurrenScene != null)
            {
                character.OnFire(canvasModel.CurObjectPoint);
            }
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
        bool isFirst = true;

        public void SetCurStage(StageScene stage)
        {
            if (stage == null)
            {
                return;
            }
            curStage = stage;
            character = curStage.GetMainCharacter();
            if (!isFirst&&camera!=null)
            {
                stageController.Stop();
            }
            else
            {
                camera = new ScreenCamera(canvasModel);
            }
            stageController.Start(curStage.CreateScene());
            camera.SetFocusObject(character);
            this.canvasModel.MaxX = stage.MaxX;
            this.canvasModel.MaxY = stage.MaxY;
            this.canvasModel.MinX = stage.MinX;
            this.canvasModel.MinY = stage.MinY;
            this.Width = stage.GetWidth();
            this.Height = stage.GetHeight();
            //this.StartPosition = FormStartPosition.CenterScreen;
        }
        private void Inputs_CMDEvent(GAME_CMDS cmd)
        {
            switch (cmd)
            {
                case GAME_CMDS.NONE:
                    break;
                case GAME_CMDS.AUTO_FOCUS:
                    camera.EnableFucus = true;
                    break;
                case GAME_CMDS.DISABLE_FOCUS:
                    camera.EnableFucus = false;
                    break;
                case GAME_CMDS.INTERFER_ON:
                    character.EnableCheckCollision = true;
                    break;
                case GAME_CMDS.INTERFER_OFF:
                    character.EnableCheckCollision = false;
                    break;
                case GAME_CMDS.MOMENTA_ON:
                    StageController.EnableMomenta = true;
                    break;
                case GAME_CMDS.MOMENTA_OFF:
                    StageController.EnableMomenta = false;
                    break;
                case GAME_CMDS.TRACK_ON:
                    CanvasModel.EnableTrack = true;
                    break;
                case GAME_CMDS.TRACK_OFF:
                    CanvasModel.EnableTrack = false;
                    break;
                case GAME_CMDS.NEXT_SCENE:
                    if (curStage != null)
                    {
                        curStage.Clear();
                    }
                    StageScene next = stageManager.GetNextStage();
                    SetCurStage(next);
                    break;
                case GAME_CMDS.FOR_SCENE:
                    StageScene stage = stageManager.GetPreViewStage();
                    SetCurStage(stage);
                    break;
                case GAME_CMDS.EDIT: 
                    break;
                case GAME_CMDS.GCODE:
                    new GCodeLaserHeadForm().Show();
                    break;
                default:
                    break;
            }
        }
    }
}
