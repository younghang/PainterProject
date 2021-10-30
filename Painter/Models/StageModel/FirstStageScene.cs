using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.StageModel
{
    interface IStageScene
    {
        MainCharacter GetMainCharacter();
        Scene GetScene();
        Scene CreateScene();
        void Clear();

    }
    class FirstStageScene: IStageScene
    {
        public void Clear()
        {
            this.scene.Clear();
        }
        MainCharacter character = new MainCharacter();
        DrawableText scoreText = new DrawableText();
        DrawableText illustrateText = new DrawableText(); 
        Scene scene = new Scene();
        //获取场景中的键鼠操作对象
        public MainCharacter GetMainCharacter()
        {
            return character;
        }
        public Scene GetScene()
        {
            return this.scene;
        }
        public Scene CreateScene()
        { 
            GroundObject groundObj = new GroundObject();
            RectangeGeo groundRec = new RectangeGeo(new PointGeo(0, 0), new PointGeo(1200, 100));
            groundRec.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.BlueViolet });
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

            GroundObject groundObj4 = new GroundObject();
            RectangeGeo groundRec4 = new RectangeGeo(new PointGeo(0, 1000), new PointGeo(1200, 1100));
            groundRec4.Angle = 30;
            groundRec4.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj4.Add(groundRec4);

            Obstacle obstacleTex = new Obstacle();

            scoreText.pos = new PointGeo(1350, 1000);
            scoreText.SetDrawMeta(new TextMeta("Score:") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangeGeo rect = new RectangeGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleTex.Add(rect);
            obstacleTex.Add(scoreText);

            Obstacle obstacleIllustionTex = new Obstacle();

            illustrateText.pos = new PointGeo(1700, 1000);
            illustrateText.SetDrawMeta(new TextMeta("Illustation:") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 16f), stringFormat = new StringFormat() { Alignment = StringAlignment.Near } });
            RectangeGeo rectTxt = new RectangeGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleIllustionTex.Add(rectTxt);
            obstacleIllustionTex.Add(illustrateText);
            illustrateText.GetTextMeta().Text += "\n\tMove: ↑↓←→";
            illustrateText.GetTextMeta().Text += "\n\tJump: Space";
            illustrateText.GetTextMeta().Text += "\n\tLoad Enemys: Q";
            illustrateText.GetTextMeta().Text += "\n\tLoad Falling Obstacle: W";
            illustrateText.GetTextMeta().Text += "\n\tPause/Resume: Enter";
            illustrateText.GetTextMeta().Text += "\n\t Command \"TRACK/DETRACK\" enable/disable move track";
            illustrateText.GetTextMeta().Text += "\n\t Command \"FOCUS/DEFOCUS\" enable/disable camera following";
            illustrateText.GetTextMeta().Text += "\n\t Command \"MOMENTA/DEMOMENTA\" enable/disable object collision";
            illustrateText.GetTextMeta().Text += "\n\t Command \"INTERFER/DEINTERFER\" enable/disable main character collision";

            character.Move(new PointGeo(100, 200));
            character.HitEnemyEvent += () => {
                scoreText.GetTextMeta().Text = "Score: " + (character.HitCount).ToString("f1");
            };
            character.EnableCheckCollision = false; 

            GroundObject groundObj5 = new GroundObject();
            RectangeGeo groundRec5 = new RectangeGeo(new PointGeo(3000, 0), new PointGeo(4000, 100));
            groundRec5.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj5.Add(groundRec5);

            GroundObject groundObj6 = new GroundObject();
            RectangeGeo groundRec6 = new RectangeGeo(new PointGeo(3000, 300), new PointGeo(3400, 400));
            groundRec6.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundRec6.Angle = 90;
            groundObj6.Add(groundRec6);

            Obstacle obstacle = new Obstacle();
            RectangeGeo rectRotate = new RectangeGeo(new PointGeo(4000, 100), new PointGeo(4020, 400));
            rectRotate.Angle = 0;
            rectRotate.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = true, BackColor = Color.GreenYellow });
            obstacle.Add(rectRotate);
            obstacle.EnabledRotate = true;

            GroundObject groundObj7 = new GroundObject();
            RectangeGeo groundRec7 = new RectangeGeo(new PointGeo(4000, 0), new PointGeo(6500, 100));
            groundRec7.Angle = -20;
            groundRec7.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Gray, LineWidth = 2, IsFill = false, BackColor = Color.BlanchedAlmond });
            groundObj7.Add(groundRec7);
            groundObj7.GroundFrictionRatio = 0.1f;

            scene.AddObject(groundObj);
            scene.AddObject(groundObj2);
            scene.AddObject(groundObj3);
            scene.AddObject(groundObj4);
            scene.AddObject(groundObj5);
            scene.AddObject(groundObj6);
            scene.AddObject(groundObj7);
            scene.AddObject(character, false);
            scene.AddObject(obstacleTex, true); 
            scene.AddObject(obstacleIllustionTex); 
            scene.AddObject(obstacle, true); 
            return scene;
        }
        public void LoadFallingObstacles()
        {
            for (int i = 0; i < 10; i++)
            {
                FallingObstacle enemy1 = new FallingObstacle();
                CircleGeo rectange2 = new CircleGeo(new PointGeo(0, 0), new PointGeo(20, 20));
                rectange2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Transparent, LineWidth = 1, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
                enemy1.Add(rectange2); 
                enemy1.Move(new PointGeo(rand.Next(3000),1500));
                scene.AddObject(enemy1, false);
            }
        }
        public void LoadEnemys()
        {
            Enemy enemy = new Enemy();
            RectangeGeo rectange = new RectangeGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            rectange.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
            enemy.Add(rectange);
            enemy.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() * 5, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);
            enemy.Move(enemy.Speed * 100);
            scene.AddObject(enemy, true);

            for (int i = 0; i < 6; i++)
            {
                Enemy enemy1 = new Enemy();
                RectangeGeo rectange2 = new RectangeGeo(new PointGeo(0, 0), new PointGeo(50, 50));
                rectange2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
                enemy1.Add(rectange2);
                enemy1.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() * 5, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);
                enemy1.Move(new PointGeo(rand.Next(200), rand.Next(200)));
                scene.AddObject(enemy1, false);
            }
            for (int i = 0; i < 6; i++)
            {
                Enemy enemy1 = new Enemy();
                CircleGeo rectange2 = new CircleGeo(new PointGeo(0, 0), new PointGeo(20, 20));
                rectange2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.IndianRed, LineWidth = 3, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
                enemy1.Add(rectange2);
                enemy1.Speed = new PointGeo((float)rand.NextDouble() * 5, (float)rand.NextDouble() * 5);
                enemy1.Move(new PointGeo(rand.Next(200), rand.Next(200))); 
                scene.AddObject(enemy1, true);
            }
        }
        static Random rand = new Random();

    }
}
