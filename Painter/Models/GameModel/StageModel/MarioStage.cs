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
    class MarioStage : StageScene
    {
        public MarioStage()
        {
            MaxX = 2400;
            MinX = 0;
            MinY = 0;
            MaxY = 600;
        }
        public override void Clear()
        {
            this.scene.Clear();
 
            CanvasModel.EnableTrack = false;
        }
        MainCharacter character = new MainCharacter();
        MainCharacter board = new MainCharacter();
      
        DrawableText scoreText = new DrawableText();
 
    
        public override Scene CreateScene()
        {
            CanvasModel.EnableTrack = false;
            this.LoadGround();

            GroundObject floatGround = new GroundObject();
            RectangleGeo rectangle = new RectangleGeo(new PointGeo(3400, 350), new PointGeo(4000, 450));
            rectangle.SetDrawMeta(new ShapeMeta() { IsFill=false,LineWidth=2,ForeColor=Color.ForestGreen});
            floatGround.Add(rectangle);
            floatGround.ReflectResistance = 1E-5f;
            scene.AddObject(floatGround);
            floatGround.StatusUpdateEvent += () => {
                if (rectangle.FirstPoint.X<3900&&floatGround.IsInAnimate==false)
                {
                    floatGround.AnimateTo(new PointGeo(500, 400), 5000);
                }
                if (rectangle.FirstPoint.X >= 3900 && floatGround.IsInAnimate == false)
                {
                    floatGround.AnimateTo(new PointGeo(-500, -400), 5000);
                }
            };

            Obstacle TextObject = new Obstacle();
            scoreText.pos = new PointGeo(1200, 500);
            scoreText.SetDrawMeta(new TextMeta("你好呀 Hello") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            TextObject.Add(rect);
            TextObject.Add(scoreText);

            if (board.GetElements().Count < 6)
            {
                RectangleGeo rectangleBoard = new RectangleGeo(new PointGeo(0, 1220), new PointGeo(-7050, 0));
                rectangleBoard.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = true, BackColor = Color.FromArgb(50, Color.Gray) });
                board.Add(rectangleBoard);
                board.IsAppliedGravity = false;
                board.AvoidFallingDown = false;
            }
             
            scene.AddObject(character);
            return scene;
        }

        public override MainCharacter GetMainCharacter()
        {
            return character;
        }
        
        public void LoadGround()
        {
            List<DrawableObject> drawableObjects = FileUtils.LoadDrawableFile("./mario_ground.dat");
            foreach (var item in drawableObjects)
            {
                if (item is RectangleGeo)
                {
                    GroundObject groundObject = new GroundObject();
                    groundObject.Add(item);
                    this.scene.AddObject(groundObject);
                }else
                {
                    Obstacle obstacle = new Obstacle();
                    obstacle.Add(item);
                    this.scene.AddObject(obstacle);
                }
               
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
                    character.Speed.X = 3;
                    break;
                case Keys.Left:
                    character.Speed.X = -3;
                    break;
                case Keys.Up:
                    character.Speed.Y =  2;
                    break;
                case Keys.Space:
                    character.Speed.Y = 2;
                   
                 
                    break;
                case Keys.Enter:
                    break;
                case Keys.Down:
                    
                    break;

            }
        }
 

        public void AnimateDistance(float moveDistance, int timespan, bool isGraph)
        {
            if (isGraph)
            {
                 
            }
            else
            {
                board.AnimateTo(new PointGeo(moveDistance, 0), timespan);
            }
        }
    }
}
