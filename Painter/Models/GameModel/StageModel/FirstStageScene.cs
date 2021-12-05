using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Models.StageModel
{
    public abstract class StageScene
    { 
        public double MaxX = 2400;
        public double MinX = 0;
        public double MinY = 0;
        public double MaxY = 600;
        protected int width = 1200;
        protected int height = 600;
        protected Scene scene = new Scene();
        public abstract MainCharacter GetMainCharacter();
        public virtual Scene GetScene() { return this.scene; }
        public abstract Scene CreateScene();
        public virtual void Clear() { this.scene.Clear(); }
        public virtual void OnKeyDown(Keys keyData) { }
        public virtual int GetWidth() { return this.width; }
        public virtual int GetHeight() { return this.height; }
    }
    class FirstStageScene: StageScene
    {
        
        MainCharacter character = new MainCharacter();
        DrawableText scoreText = new DrawableText();
        DrawableText illustrateText = new DrawableText(); 
      
        //获取场景中的键鼠操作对象
        public override MainCharacter GetMainCharacter()
        {
            return character;
        }
        
        public override Scene CreateScene()
        { 
            GroundObject groundObj = new GroundObject();
            RectangleGeo groundRec = new RectangleGeo(new PointGeo(0, 0), new PointGeo(1200, 100));
            groundRec.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.BlueViolet });
            groundObj.Add(groundRec);
            groundObj.ReflectResistance = 1E-6f;

            GroundObject groundObj2 = new GroundObject();
            RectangleGeo groundRec2 = new RectangleGeo(new PointGeo(1500, 0), new PointGeo(2700, 100));
            groundRec2.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj2.Add(groundRec2);

            GroundObject groundObj3 = new GroundObject();
            RectangleGeo groundRec3 = new RectangleGeo(new PointGeo(1500, 300), new PointGeo(2700, 400));
            groundRec3.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj3.Add(groundRec3);

            GroundObject groundObj4 = new GroundObject();
            RectangleGeo groundRec4 = new RectangleGeo(new PointGeo(0, 1000), new PointGeo(1200, 1100));
            groundRec4.Angle = 30;
            groundRec4.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj4.Add(groundRec4);

            Obstacle obstacleTex = new Obstacle();

            scoreText.pos = new PointGeo(1350, 1000);
            scoreText.SetDrawMeta(new TextMeta("Score:0.0") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 36f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleTex.Add(rect);
            obstacleTex.Add(scoreText);

            Obstacle obstacleIllustionTex = new Obstacle();

            illustrateText.pos = new PointGeo(1700, 1000);
            illustrateText.SetDrawMeta(new TextMeta("Illustation:") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 16f), stringFormat = new StringFormat() { Alignment = StringAlignment.Near } });
            RectangleGeo rectTxt = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleIllustionTex.Add(rectTxt);
            obstacleIllustionTex.Add(illustrateText);
            illustrateText.GetTextMeta().Text += "\n\tMove: ↑↓←→";
            illustrateText.GetTextMeta().Text += "\n\tJump: Space";
            illustrateText.GetTextMeta().Text += "\n\tLoad Enemys: Q";
            illustrateText.GetTextMeta().Text += "\n\tLoad Falling Obstacle: W";
            illustrateText.GetTextMeta().Text += "\n\tPause/Resume: Enter";
            illustrateText.GetTextMeta().Text += "\n\t Command \"NEXT/FOR\" switch to next/previous scene";
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
            RectangleGeo groundRec5 = new RectangleGeo(new PointGeo(3000, 0), new PointGeo(4000, 100));
            groundRec5.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundObj5.Add(groundRec5);

            GroundObject groundObj6 = new GroundObject();
            RectangleGeo groundRec6 = new RectangleGeo(new PointGeo(3000, 300), new PointGeo(3400, 400));
            groundRec6.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = false, BackColor = Color.Gray });
            groundRec6.Angle = 90;
            groundObj6.Add(groundRec6);

            Obstacle obstacle = new Obstacle();
            RectangleGeo rectRotate = new RectangleGeo(new PointGeo(4000, 100), new PointGeo(4020, 400));
            rectRotate.Angle = 0;
            rectRotate.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Black, LineWidth = 2, IsFill = true, BackColor = Color.GreenYellow });
            obstacle.Add(rectRotate);
            obstacle.EnabledRotate = true;

            GroundObject groundObj7 = new GroundObject();
            RectangleGeo groundRec7 = new RectangleGeo(new PointGeo(4000, 0), new PointGeo(6500, 100));
            groundRec7.Angle = -20;
            groundRec7.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Gray, LineWidth = 2, IsFill = false, BackColor = Color.BlanchedAlmond });
            groundObj7.Add(groundRec7);
            groundObj7.GroundFrictionRatio = 0.1f;

            GroundObject groundObj8 = new GroundObject();
            RectangleGeo groundRec8 = new RectangleGeo(new PointGeo(6500, 0), new PointGeo(10500, 100));
            groundRec8.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Gray, LineWidth = 2, IsFill = false, BackColor = Color.BlanchedAlmond });
            groundObj8.Add(groundRec8);

            GroundObject groundObj9 = new GroundObject();
            RectangleGeo groundRec9 = new RectangleGeo(new PointGeo(10500, 200), new PointGeo(14500, 300));
            groundRec9.SetDrawMeta(new ShapeMeta() { ForeColor = Color.Gray, LineWidth = 2, IsFill = false, BackColor = Color.BlanchedAlmond });
            groundRec9.Angle = 180;
            groundObj9.Add(groundRec9);

            scene.AddObject(groundObj);
            scene.AddObject(groundObj2);
            scene.AddObject(groundObj3);
            scene.AddObject(groundObj4);
            scene.AddObject(groundObj5);
            scene.AddObject(groundObj6);
            scene.AddObject(groundObj7);
            scene.AddObject(groundObj8);
            scene.AddObject(groundObj9);
            scene.AddObject(character, false);
            scene.AddObject(obstacleTex, true); 
            scene.AddObject(obstacleIllustionTex); 
            scene.AddObject(obstacle, true);

            Enemy enemy = new Enemy();
            enemy.LifeLength = 60;
            RectangleGeo rectange = new RectangleGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            rectange.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
            enemy.Add(rectange);
            enemy.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() * 5, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);
            enemy.Move(enemy.Speed * 100);
            scene.AddObject(enemy, true);
            enemy.EnemyDisposedEvent += Enemy_EnemyDisposedEvent;
            return scene;
        }

        private void Enemy_EnemyDisposedEvent(Enemy obj)
        {
            for (int i = 0; i < 4; i++)
            {
                Enemy enemy1 = new Enemy();
                RectangleGeo rectange2 = new RectangleGeo(new PointGeo(0, 0), new PointGeo(30, 30));
                rectange2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
                enemy1.Add(rectange2);
                enemy1.Speed = new PointGeo((float)rand.NextDouble() * 2, (float)rand.NextDouble() * 2);
                enemy1.Move(new PointGeo((obj.GetOutShape() as Shape).GetShapeCenter()));
                enemy1.StopEvent += OnEnemyStop;
                scene.AddObject(enemy1, false);
            }
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
        private void OnEnemyStop(Enemy enemy)
        {
            enemy.Speed.X = ((float)new Random().NextDouble() * 2 - 1) * 8;
        }
        public void LoadEnemys()
        {
            Enemy enemy = new Enemy();
            RectangleGeo rectange = new RectangleGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            rectange.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
            enemy.Add(rectange);
            enemy.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() * 5, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);
            enemy.Move(enemy.Speed * 100);
            scene.AddObject(enemy, true);
            enemy.StopEvent += OnEnemyStop;
            for (int i = 0; i < 6; i++)
            {
                Enemy enemy1 = new Enemy();
                RectangleGeo rectange2 = new RectangleGeo(new PointGeo(0, 0), new PointGeo(50, 50));
                rectange2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5, BackColor = System.Drawing.Color.OrangeRed, IsFill = true });
                enemy1.Add(rectange2);
                enemy1.Speed = new PointGeo((float)new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() * 5, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);
                enemy1.Move(new PointGeo(rand.Next(200), rand.Next(200)));
                enemy1.StopEvent += OnEnemyStop; 
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
                enemy1.StopEvent += OnEnemyStop; 
                scene.AddObject(enemy1, true);
            }
        }

        public override void OnKeyDown(Keys keyData)
        {
            switch (keyData)
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
                    if (character.Status == SCENE_OBJECT_STATUS.IN_GROUND)
                    {
                        character.Speed.Y = 2;
                    }
                    break;
                case Keys.Space:
                    character.Speed.Y = 2;
                    break;
                case Keys.Enter:
                    break;
                case Keys.Down:
                    character.Move(new PointGeo(0, -10));
                    break;
                case Keys.Q:
                    LoadEnemys();
                    break;
                case Keys.W:
                    LoadFallingObstacles();
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

        static Random rand = new Random();

    }
   
}
