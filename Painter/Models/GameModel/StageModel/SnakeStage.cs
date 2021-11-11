using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.Models.StageModel
{
    class SnakeStage : StageScene
    {
        DIRECTION curDirection = DIRECTION.RIGHT;
        MainCharacter character = new MainCharacter();
        DrawableText scoreText = new DrawableText();
        Scene scene = new Scene();
        public override void Clear()
        {
            scene.Clear();
        }

        ShapeMeta emptyMeta = new ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 1, BackColor = System.Drawing.Color.OrangeRed, IsFill = false ,DashLineStyle=new float[] { 4,8,4} }  ;
        ShapeMeta fillMeta = new ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 1, BackColor = System.Drawing.Color.OrangeRed, IsFill = true }  ;
        Obstacle obstacle=null;
        Obstacle food = null; 
        List<Point> points = new List<Point>();
        private static Random rand = new Random();
        private PointGeo CreateFoodPos()
        {

            while (true)
            {
                PointGeo pointGeo = new PointGeo(rand.Next(25), rand.Next(10));
                bool isOverLap = false;
                foreach (var item in points)
                {
                    if (item.X==pointGeo.X&&item.Y== pointGeo.Y)
                    {
                        isOverLap = true;
                    }
                }
                if (!isOverLap)
                {
                    return pointGeo;
                }
            }
        }
        int blockWidth = 100;
        PointGeo FoodPos = new PointGeo();
        public override Scene CreateScene()
        {
            Score = 0;
            IsEndGame = false;
            points.Clear();
            points.Add(new Point(rand.Next(24), rand.Next(9)));
            if (points[0].X>12)
            {
                curDirection = DIRECTION.LEFT;
            }else
            {
                curDirection = DIRECTION.RIGHT; 
            }
            obstacle = new Obstacle();
            FoodPos= CreateFoodPos();
            food = new Obstacle();
            CircleGeo circle = new CircleGeo(new PointGeo(FoodPos.X*blockWidth+blockWidth/2.0f,FoodPos.Y * blockWidth + blockWidth / 2.0f), new PointGeo( FoodPos.X * blockWidth +  blockWidth, FoodPos.Y * blockWidth + blockWidth / 2.0f));
            circle.SetDrawMeta(new ShapeMeta() { BackColor=System.Drawing.Color.Aquamarine,IsFill=true,ForeColor=System.Drawing.Color.AntiqueWhite,LineWidth=1 });
            food.Add(circle); 
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    RectangleGeo rectange = new RectangleGeo(new PointGeo( j * blockWidth,i * blockWidth), new PointGeo((j + 1) * blockWidth, (i + 1) * blockWidth));
                    rectange.SetDrawMeta(emptyMeta);
                    obstacle.Add(rectange);
                }
            }
            RectangleGeo rectangeGeo = new RectangleGeo(new PointGeo(0, 0), new PointGeo(25 * blockWidth, 10 * blockWidth));
            rectangeGeo.SetDrawMeta(new ShapeMeta() {ForeColor=System.Drawing.Color.OrangeRed,LineWidth=3 });
            obstacle.Add(rectangeGeo);

            Obstacle obstacleTex = new Obstacle(); 
            scoreText.pos = new PointGeo(1350, 1100);
            scoreText.SetDrawMeta(new TextMeta("Score: 0") { IsScaleble = true, ForeColor = Color.LimeGreen, TEXTFONT = new Font("Consolas Bold", 48f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangleGeo rect = new RectangleGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleTex.Add(rect);
            obstacleTex.Add(scoreText);

            scene.AddObject(obstacle);
            scene.AddObject(food);
            scene.AddObject(obstacleTex);
            obstacle.UpdateEvent += Obstacle_UpdateEvent;
            return scene;
        }
        private int TickCount = -1;
        private void Obstacle_UpdateEvent()
        { 
            if (IsEndGame)
            {
                return;
            }
            TickCount++;
            if (TickCount%20==0)
            { 
                UpdatePosition();
            }
             
        }
        bool IsEndGame = false;
        int Score = 0;
        private void UpdatePosition()
        {
           
            #region 添加一个新点
            switch (curDirection)
            {
                case DIRECTION.UP:
                    points.Add(new Point(points[points.Count - 1].X, points[points.Count - 1].Y + 1));
                    break;
                case DIRECTION.DOWN:
                    points.Add(new Point(points[points.Count - 1].X, points[points.Count - 1].Y - 1));

                    break;
                case DIRECTION.LEFT:
                    points.Add(new Point(points[points.Count - 1].X-1, points[points.Count - 1].Y )); 
                    break;
                case DIRECTION.RIGHT:
                    points.Add(new Point(points[points.Count - 1].X+1, points[points.Count - 1].Y )); 
                    break;
                default:
                    break;
            }
            #region 检查游戏结束
            List<int> Result = new List<int>();
            foreach (var item in points)
            {
                if (item.X < 0 || item.Y < 0 || item.Y > 10 || item.X > 25)
                {
                    IsEndGame = true;
                }
                int k = item.X + item.Y * 25;
                if (Result.Contains(k))
                {
                    IsEndGame = true;
                    break;
                }
                else
                {
                    Result.Add(k);
                }
            }
            //Debug.Print(points[0]+"");
            if (IsEndGame)
            {
                scoreText.GetTextMeta().Text = "Game Over";
                return;
            }
            #endregion
            #region 吃掉了食物
            bool isEatFood = false;
            if (points[points.Count-1].X==FoodPos.X&& points[points.Count - 1].Y == FoodPos.Y)
            {
                isEatFood = true;
                Score += 10;
                scoreText.GetTextMeta().Text = "Score: "+ Score;
                PointGeo newPos = CreateFoodPos();
                food.Move(newPos*blockWidth-FoodPos*blockWidth);
                this.fillMeta.BackColor = ((food.GetElements()[0] as Shape).GetDrawMeta() as ShapeMeta).BackColor;
                int Hue = rand.Next(30, 360);
                var color = CommonUtils.HslToRgb(Hue, 100, 100);
                ((food.GetElements()[0] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
                FoodPos = newPos;
            }
            if (!isEatFood)
            {
                points.RemoveAt(0);
            }
            #endregion
            List<DrawableObject> elements = obstacle.GetElements();
            #endregion
            for (int i = 0; i < elements.Count-1; i++)
            {
                Shape shape = elements[i] as Shape;
                int row = i / 25;
                int col =i % 25;
                bool isInPoints = false;
                foreach (var item in points)
                {
                    if (item.X == col && item.Y == row)
                    {
                        isInPoints = true; 
                        break;
                    }
                }
                if (isInPoints)
                {
                    shape.SetDrawMeta(fillMeta);  
                }
                else
                {
                    shape.SetDrawMeta(emptyMeta); 
                }
            }
        }
        public override MainCharacter GetMainCharacter()
        {
            return character;
        }

        public override Scene GetScene()
        {
            return scene; 
        }
        private int Width = 1200;
        private int height = 600;

        public override int GetWidth()
        {
            return Width;
        }

        public override int GetHeight()
        {
            return height;
        }
        public override void OnKeyDown(Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Right:
                    if (curDirection != DIRECTION.LEFT)
                    {
                        curDirection = DIRECTION.RIGHT;
                    }
                    break;
                case Keys.Left:
                    if (curDirection!= DIRECTION.RIGHT)
                    {
                        curDirection = DIRECTION.LEFT;
                    } 
                    break;
                case Keys.Up:
                    if (curDirection != DIRECTION.DOWN)
                    {
                        curDirection = DIRECTION.UP;
                    }
                    break;
                case Keys.Space: 
                    break;
                case Keys.Enter:
                    break;
                case Keys.Down:
                    if (curDirection != DIRECTION.UP)
                    {
                        curDirection = DIRECTION.DOWN;
                    }
                    break; 
            }
        }
    }
}
