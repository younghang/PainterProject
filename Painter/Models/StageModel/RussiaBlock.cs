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
    class RussiaBlock : StageScene
    {
        public RussiaBlock()
        {
            MinX = 0;
            MaxX = 1200;
            MinY = 100;
            MaxY = 2700;
        }
        private int Width = 800;
        private int Height = 1000;

        DIRECTION curDirection = DIRECTION.RIGHT;
        MainCharacter character = new MainCharacter();
        DrawableText scoreText = new DrawableText();
        Scene scene = new Scene();
        public override void Clear()
        {
            scene.Clear();
        }


        ShapeMeta emptyMeta = new ShapeMeta() { ForeColor = System.Drawing.Color.Lavender, LineWidth = 1, BackColor = System.Drawing.Color.OrangeRed, IsFill = false, DashLineStyle = new float[] { 1, 2, 1 } };
        ShapeMeta fillMeta = new ShapeMeta() { ForeColor = System.Drawing.Color.Lavender, LineWidth = 1, BackColor = System.Drawing.Color.OrangeRed, IsFill = true };
        Obstacle obstacle = null;
        Obstacle nextBlock = null;
        List<PointGeo> points = new List<PointGeo>();
        List<PointGeo> nextPoints = new List<PointGeo>();
        private static Random rand = new Random();
       
        int blockWidth = 100;
        
        public override Scene CreateScene()
        {
            Score = 0;
            InitBlocks();
            IsEndGame = false;
            result.Clear();
            points.Clear();
            nextPoints = GenerateNextBlock();
            points = GenerateABlock();
            nextPoints = GenerateNextBlock();

            obstacle = new Obstacle(); 
            nextBlock = new Obstacle();
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    RectangeGeo rectange = new RectangeGeo(new PointGeo(j * blockWidth, i * blockWidth), new PointGeo((j + 1) * blockWidth, (i + 1) * blockWidth));
                    rectange.SetDrawMeta(new ShapeMeta() { ForeColor=Color.OliveDrab,LineWidth=0.5f});
                    nextBlock.Add(rectange);
                }
            }
    
            for (int i = 0; i < 27; i++)
            {
                for (int j = 0; j < 14; j++)
                {
                    RectangeGeo rectange = new RectangeGeo(new PointGeo(j * blockWidth, i * blockWidth), new PointGeo((j + 1) * blockWidth, (i + 1) * blockWidth));
                    rectange.SetDrawMeta(emptyMeta);
                    obstacle.Add(rectange);
                }
            }
            RectangeGeo rectangeGeo = new RectangeGeo(new PointGeo(0, 0), new PointGeo(14 * blockWidth, 27 * blockWidth));
            rectangeGeo.SetDrawMeta(new ShapeMeta() { ForeColor = System.Drawing.Color.OrangeRed, LineWidth = 3 });
            obstacle.Add(rectangeGeo);

            Obstacle obstacleTex = new Obstacle();
            scoreText.pos = new PointGeo(1800, 2000);
            scoreText.SetDrawMeta(new TextMeta("Score: 0") { IsScaleble = true, ForeColor = Color.Goldenrod, TEXTFONT = new Font("Consolas Bold", 62f), stringFormat = new StringFormat() { Alignment = StringAlignment.Center } });
            RectangeGeo rect = new RectangeGeo(scoreText.pos, scoreText.pos - new PointGeo(100, 100));
            obstacleTex.Add(rect);
            obstacleTex.Add(scoreText);

            scene.AddObject(obstacle);
            scene.AddObject(nextBlock);
            scene.AddObject(obstacleTex);
            nextBlock.Move(new PointGeo(1600, 2100));
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
            if (TickCount % 20 == 0)
            {
                UpdatePosition();
            }
            #region 上色
            List<IScreenPrintable> elements = obstacle.GetElements(); 
            for (int i = 0; i < elements.Count - 1; i++)
            {
                Shape shape = elements[i] as Shape;
                int row = i / 14;
                int col = i % 14;
                bool isInPoints = false;
                foreach (var item in points)
                {
                    if (item.X == col && item.Y == row)
                    {
                        isInPoints = true;
                        break;
                    }
                }
                foreach (var item in result)
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

            List<IScreenPrintable> nextElements = nextBlock.GetElements();
            for (int i = 0; i < nextElements.Count; i++)
            {
                Shape shape = nextElements[i] as Shape;
                int row = i / 4;
                int col = i % 4;
                bool isInPoints = false;
                foreach (var item in nextPoints)
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
            #endregion
            #region 检查消除整行
            bool hasWholeLine = true;
            int lineIndex = 0;
            while (hasWholeLine)
            {
                hasWholeLine = false;
                for (int i = 0; i < 27; i++)
                {
                    bool isLine = true;
                    for (int j = 0; j < 14; j++)
                    {
                        if (!IsPointInResult(j, i))
                        {
                            isLine = false;
                            break;
                        }
                    }
                    if (isLine)
                    {
                        hasWholeLine = true;
                        lineIndex = i;
                    }
                }
                if (hasWholeLine)
                {
                    for (int i = result.Count - 1; i >= 0; i--)
                    {
                        if (result[i].Y == lineIndex)
                        {
                            result.RemoveAt(i);
                        }
                        else if (result[i].Y > lineIndex)
                        {
                            result[i].Y--;
                        }
                    }
                    Score += 20;
                    scoreText.GetTextMeta().Text = "Score: " + Score; ;
                }
            }
            #endregion
        }
        bool IsEndGame = false;
        int Score = 0;
        private List<PointGeo> result = new List<PointGeo>();
        private bool IsPointInResult(int x, int y)
        {
            foreach (var item in result)
            {
                if (item.X == x && item.Y == y)
                {
                    return true;
                }
            }
            return false;
        }
        private void UpdatePosition()
        {
            CurrentBottom--;
            for (int i = 0; i < points.Count; i++)
            {
                PointGeo p = points[i];
                p.Y -= 1;
            }
            #region 检查游戏结束
            foreach (var item in result)
            {
                if (item.Y == 27)
                {
                    IsEndGame = true;
                }
            }
            if (IsEndGame)
            {
                scoreText.GetTextMeta().Text = "Game Over";
                return;
            }
            #endregion
            #region 碰到底
            bool isStop = false;
            bool isStopOnGround = false;
            if (CurrentBottom <= 0)
            {
                isStopOnGround = true;
            }
            foreach (var point in points)
            {
                if (IsPointInResult((int)point.X, (int)point.Y))
                {
                    isStop = true;
                }
                if (point.Y<=0)
                {
                    isStopOnGround = true;
                }
            }
            if (isStopOnGround)
            {
                this.result.AddRange(CommonUtils.Clone<PointGeo>(points));
            }
            else
            if (isStop)
            {
                List<PointGeo> ps = CommonUtils.Clone<PointGeo>(points);
                foreach (var item in ps)
                {
                    item.Y += 1;
                }
                this.result.AddRange(ps);
            }
            if (isStop || isStopOnGround)
            {
                this.points = GenerateABlock() ;
                this.nextPoints = GenerateNextBlock();
            }
            #endregion 
        }
        public override MainCharacter GetMainCharacter()
        {
            return character;
        }

        public override Scene GetScene()
        {
            return scene;
        }

        public override int GetWidth()
        {
            return Width;
        }

        public override int GetHeight()
        {
            return Height;
        }
        List<List<List<PointGeo>>> blocks = new List<List<List<PointGeo>>>();
        private void InitBlocks()
        {
            blocks.Clear();
            //正方形
            List<List<PointGeo>> squres = new List<List<PointGeo>>();
            List<PointGeo> square = new List<PointGeo>();
            square.Add(new PointGeo(0, 0));
            square.Add(new PointGeo(0, 1));
            square.Add(new PointGeo(1, 1));
            square.Add(new PointGeo(1, 0));
            squres.Add(square);
            blocks.Add(squres);

            //长条
            List<List<PointGeo>> rods = new List<List<PointGeo>>();
            List<PointGeo> rod1 = new List<PointGeo>();
            rod1.Add(new PointGeo(0, 0));
            rod1.Add(new PointGeo(1, 0));
            rod1.Add(new PointGeo(2, 0));
            rod1.Add(new PointGeo(3, 0));
            rods.Add(rod1);
            List<PointGeo> rod2 = new List<PointGeo>();
            rod2.Add(new PointGeo(2, 0));
            rod2.Add(new PointGeo(2, 1));
            rod2.Add(new PointGeo(2, 2));
            rod2.Add(new PointGeo(2, 3));
            rods.Add(rod2);
            blocks.Add(rods);

            //L条
            List<List<PointGeo>> ls = new List<List<PointGeo>>();
            List<PointGeo> l1 = new List<PointGeo>();
            l1.Add(new PointGeo(0, 0));
            l1.Add(new PointGeo(1, 0));
            l1.Add(new PointGeo(2, 0));
            l1.Add(new PointGeo(2, 1));
            ls.Add(l1);
            List<PointGeo> l2 = new List<PointGeo>();
            l2.Add(new PointGeo(1, 0));
            l2.Add(new PointGeo(2, 0));
            l2.Add(new PointGeo(1, 1));
            l2.Add(new PointGeo(1, 2));
            ls.Add(l2);
            List<PointGeo> l3 = new List<PointGeo>();
            l3.Add(new PointGeo(1, 0));
            l3.Add(new PointGeo(1, 1));
            l3.Add(new PointGeo(2, 1));
            l3.Add(new PointGeo(3, 1));
            ls.Add(l3);
            List<PointGeo> l4 = new List<PointGeo>();
            l4.Add(new PointGeo(2, 0));
            l4.Add(new PointGeo(2, 1));
            l4.Add(new PointGeo(2, 2));
            l4.Add(new PointGeo(1, 2));
            ls.Add(l4);
            blocks.Add(ls);

            //Z条
            List<List<PointGeo>> zs = new List<List<PointGeo>>();
            List<PointGeo> z1 = new List<PointGeo>();
            z1.Add(new PointGeo(0, 0));
            z1.Add(new PointGeo(1, 0));
            z1.Add(new PointGeo(1, 1));
            z1.Add(new PointGeo(2, 1));
            zs.Add(z1);
            List<PointGeo> z2 = new List<PointGeo>();
            z2.Add(new PointGeo(2, 0));
            z2.Add(new PointGeo(2, 1));
            z2.Add(new PointGeo(1, 1));
            z2.Add(new PointGeo(1, 2));
            zs.Add(z2);
            blocks.Add(zs);
        }
        private int CurrentType = 0;
        private int NextType = 0;
        private int NextSequence = 0;
        private int CurrentSequence = 0;
        private int CurrentBottom = 27;
        private int CurrentLeft = 8;
        private List<PointGeo> GenerateNextBlock()
        {
            NextType =  rand.Next(blocks.Count);
            List<List<PointGeo>> style = blocks[NextType];
            NextSequence = rand.Next(style.Count);
            List<PointGeo> temp = CommonUtils.Clone<PointGeo>(style[NextSequence]);
            return temp;
        }
        private List<PointGeo> GenerateABlock()
        {
            CurrentBottom = 27;
            CurrentLeft = 8;
            List<PointGeo> temp = CommonUtils.Clone<PointGeo>(nextPoints);
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Y += CurrentBottom;
                temp[i].X += CurrentLeft;
            }
            CurrentType = NextType;
            CurrentSequence = NextSequence;
            return temp;
        }
        private List<PointGeo> TransferShape()
        {
            List<List<PointGeo>> style = blocks[CurrentType];
            CurrentSequence = (++CurrentSequence) % style.Count;
            List<PointGeo> temp = CommonUtils.Clone<PointGeo>(style[CurrentSequence]);
            for (int i = 0; i < temp.Count; i++)
            {
                temp[i].Y += CurrentBottom;
                temp[i].X += CurrentLeft;
            }
            return temp;
        }
        private object lockobj = new object();
        bool isDealing = true;
        public override void OnKeyDown(Keys keyData)
        { 
            if (isDealing)
            {
                isDealing = false;
                switch (keyData)
                {
                    case Keys.Right:
                        curDirection = DIRECTION.RIGHT;
                        bool isWrong = false;
                        int i = 0;
                        for (i = 0; i < points.Count; i++)
                        {
                            PointGeo p = points[i];
                            p.X += 1;
                            if (p.X >= 14)
                            {
                                isWrong = true;
                                break;
                            }
                            if (IsPointInResult((int)p.X, (int)p.Y))
                            {
                                isWrong = true;
                                break;
                            }
                        }
                        if (isWrong)
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                PointGeo p = points[j];
                                p.X -= 1;
                            }
                        }
                        else
                        {
                            CurrentLeft++;
                        }
                        break;
                    case Keys.Left:
                        curDirection = DIRECTION.LEFT;
                        isWrong = false;
                        for (i = 0; i < points.Count; i++)
                        {
                            PointGeo p = points[i];
                            p.X -= 1;
                            if (p.X < 0)
                            {
                                isWrong = true;
                                break;
                            }
                            if (IsPointInResult((int)p.X, (int)p.Y))
                            {
                                isWrong = true;
                                break;
                            }
                        }
                        if (isWrong)
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                PointGeo p = points[j];
                                p.X += 1;
                            }
                        }
                        else
                        {
                            CurrentLeft--;
                        }
                        break;
                    case Keys.Up:

                        curDirection = DIRECTION.UP;
                        points = TransferShape();
                        break;
                    case Keys.Space:
                        break;
                    case Keys.Enter:
                        break;
                    case Keys.Down:
                        curDirection = DIRECTION.DOWN;
                        isWrong = false;
                        for (i = 0; i < points.Count; i++)
                        {
                            PointGeo p = points[i];
                            p.Y -= 1;
                            if (p.Y < 0)
                            {
                                isWrong = true;
                                break;
                            }
                            if (IsPointInResult((int)p.X, (int)p.Y))
                            {
                                isWrong = true;
                                break;
                            }
                        }
                        if (isWrong)
                        {
                            for (int j = 0; j <= i; j++)
                            {
                                PointGeo p = points[j];
                                p.Y += 1;
                            }
                        }
                        else
                        {
                            CurrentBottom--;
                        }
                        break;
                }
                isDealing = true;
            }
        }
    }
}
