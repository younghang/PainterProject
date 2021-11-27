using Painter.Models;
using Painter.Models.CameraModel;
using Painter.Models.CmdControl;
using Painter.Models.GameModel.StageModel;
using Painter.Models.GeoModel;
using Painter.Models.Paint;
using Painter.Models.PainterModel;
using Painter.Models.PhysicalModel;
using Painter.Models.StageModel;
using Painter.Painters;
using Painter.TempTest;
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
    public partial class Object3DForm : Form
    {
  
        CanvasModel canvasModel = new CanvasModel();
        GameInputCmds inputs = new GameInputCmds();
        ScreenCamera camera;
        StageManager stageManager = new StageManager();//多场景下使用的
        MainCharacter character;
        StageController stageController;
        StageScene curStage;

        Scene3D scene3D = new Scene3D();
        Render3D render3D;
        Camera3D camera3D = new Camera3D();
        Timer timer = new Timer();
        public Object3DForm()
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
            FirstStageScene firstScene = new FirstStageScene();
            SecondStageScene second = new SecondStageScene();
            SnakeStage snakeStage = new SnakeStage();
            RussiaBlock russia = new RussiaBlock(); 
            MarioStage mario = new MarioStage();
 
            stageManager.AddStage(firstScene);
            stageManager.AddStage(russia);
            stageManager.AddStage(second);
            stageManager.AddStage(snakeStage);
            stageManager.AddStage(mario);
            //SetCurStage(firstScene);
            inputs.CMDEvent += Inputs_CMDEvent;
            isFirst = false;
            this.LoadControls();
            canvasModel.MaxX = 200;
            canvasModel.MaxY = 200;
            canvasModel.MinX = -100;
            canvasModel.MinY = -100;

            scene3D.Init();
            render3D = new Render3D(this.canvasModel, scene3D, camera3D);
            timer.Interval = 50;
            timer.Tick += Timer_Tick;
            timer.Start();
        }
        int degree = 1;
        private void Timer_Tick(object sender, EventArgs e)
        {
            canvasModel.Clear();
            if (render3D!=null)
            {
                render3D.Render();
            }
            scene3D.GetOctahedral().Rotate(degree,new Vertex(0,0,0),new Vertex(0,1,0));
            scene3D.GetCube().Rotate(degree/2.0f, new Vertex(0,0,0),new Vertex(0,1,0));
            this.Invalidate();
        }

        void LoadControls()
        {

            GeoPanel geoTypePanel = new GeoPanel(400, 50, new RectangleGeo());
            geoTypePanel.Padding = 10;
            GeoLabel typeLabel = new GeoLabel(100, 50, new RectangleGeo());
            typeLabel.Text = "操作对象";
            typeLabel.Background = Color.FromArgb(220, 255, 255);

            GeoRadio geoRadioLine = new GeoRadio(50, 35, new RectangleGeo());
            geoRadioLine.CheckedEvent += () => {
                this.Text = "当前：板材";
                isGraph = false;
            };
            geoRadioLine.Text = "板材";


            GeoRadio geoRadioArc = new GeoRadio(50, 35, new RectangleGeo());
            geoRadioArc.CheckedEvent += () => {
                this.Text = "当前：图形";
                isGraph = true;
            };
            geoRadioArc.Text = "图形";
            geoRadioLine.IsChecked = true;

            GeoLabel lineWidthLabel = new GeoLabel(50, 35, new RectangleGeo());
            lineWidthLabel.Text = "距离";
            lineWidthLabel.Background = Color.Transparent;
            lineWidthLabel.Move(new PointGeo(0, 0));

            GeoEditBox geoEditBox = new GeoEditBox(50, 30, new RectangleGeo());
            geoEditBox.Text = "10";
            geoEditBox.Move(new PointGeo(0, 0));
            geoEditBox.TextChangeEvent += (str) =>
            {
                float lw = MoveDistance;
                float.TryParse(str, out lw);
                MoveDistance = lw;
            };

            GeoButton geoButtonLineStyle = new GeoButton(50, 30, new RectangleGeo());
            geoButtonLineStyle.Text = "步进";
            geoButtonLineStyle.ClickEvent += () => {
                if (this.curStage is WeldStage)
                {
                    (this.curStage as WeldStage).AnimateDistance(MoveDistance, 500, isGraph);
                }
            };


            geoTypePanel.AddControl(typeLabel);
            geoTypePanel.AddControl(geoRadioLine);
            geoTypePanel.AddControl(geoRadioArc);
            geoTypePanel.AddControl(lineWidthLabel);
            geoTypePanel.AddControl(geoEditBox);
            geoTypePanel.AddControl(geoButtonLineStyle);
            geoTypePanel.Move(new PointGeo(0, 0));
            this.canvasModel.AddGeoControls(geoTypePanel);
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

        private void SetCurStage(StageScene stage)
        {
            if (stage == null)
            {
                return;
            }
            curStage = stage;
            character = curStage.GetMainCharacter();
            if (!isFirst)
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
            this.StartPosition = FormStartPosition.CenterScreen;
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
                    new PainterTest().Show();
                    break;
                case GAME_CMDS.GCODE:
                    new GCodeLaserHeadForm().Show();
                    break;
                default:
                    break;
            }
        }

        private void Object3DForm_Load(object sender, EventArgs e)
        {

        }
    }
}

