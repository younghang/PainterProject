using Painter.Models.Paint;
using Painter.Models.StageModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.Models.GameModel.StageModel
{
    class WeldStage : StageScene
    {
        public WeldStage()
        {
            MaxX = 2400;
            MinX = 0;
            MinY = 0;
            MaxY = 600;
        }
        public override void Clear()
        {
            this.scene.Clear();
            气槽工字板.IsDisposed = true; 
            CanvasModel.EnableTrack = false;
        }
        MainCharacter character = new MainCharacter();
        MainCharacter board = new MainCharacter();
        MainCharacter graph = new MainCharacter();
        DrawableText scoreText = new DrawableText();
        Obstacle 气槽工字板 = new Obstacle();
         

        public override Scene CreateScene()
        {
            CanvasModel.EnableTrack = false;
            //scene.Background = Color.Black;
            this.Add气槽工字板();
            气槽工字板.Move(new PointGeo(0, -20));


            Obstacle TextObject = new Obstacle();
            scoreText.pos = new PointGeo(1200, 500);
            scoreText.SetDrawMeta(new TextMeta("你好呀 Hello") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            TextObject.Add(rect);
            TextObject.Add(scoreText);

            if (board.GetElements().Count<6)
            {
                RectangleGeo rectangleBoard = new RectangleGeo(new PointGeo(0, 1220), new PointGeo(-7050, 0));
                rectangleBoard.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = true, BackColor = Color.FromArgb(50, Color.Gray) });
                board.Add(rectangleBoard);
                board.IsAppliedGravity = false;
                board.AvoidFallingDown = false; 
            } 

            this.LoadGraph();
            graph.IsAppliedGravity = false;
            graph.AvoidFallingDown = false;

            scene.AddObject(气槽工字板, false);
            scene.AddObject(graph);
            scene.AddObject(board); 
            return scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            return character;
        }
        public void LoadGraph()
        {
            if (this.graph.GetElements().Count<6)
            {
                List<DrawableObject> drawableObjects = FileUtils.LoadDrawableFile("./shape.dat");
                foreach (var item in drawableObjects)
                {
                    graph.Add(item);
                }
            } 
        }
        public void Add气槽工字板()
        {
            List<DrawableObject> drawableObjects = FileUtils.LoadDrawableFile("./gongziban.dat");
            foreach (var item in drawableObjects)
            {
                气槽工字板.Add(item);
            }
        }
        public override Scene GetScene()
        {
            return scene;
        }

        public override void OnKeyDown(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    board.Speed.X = 1; 
                    break;
                case Keys.Left:
                    board.Speed.X = -1; 
                    break;
                case Keys.Up:
                    graph.Speed.X = 1;
                    break;
                case Keys.Space:
                    board.Speed.X = 0;
                    board.Move(new PointGeo(1, 0));
                    graph.Speed.X = 0;
                    graph.Move(new PointGeo(1, 0));
                    break;
                case Keys.Enter:
                    break;
                case Keys.Down:
                    graph.Speed.X = -1;
                    break;

            }
        }

       
        public override int GetWidth()
        {
            return width;
        }

        public override int GetHeight()
        {
            return height;
        }

        public void AnimateDistance(float moveDistance, int timespan, bool isGraph)
        {
            if (isGraph)
            {
                graph.AnimateTo(new PointGeo(moveDistance, 0), timespan);
            }else
            {
                board.AnimateTo(new PointGeo(moveDistance, 0), timespan); 
            }
        }
    }
}
