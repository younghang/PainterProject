//2021.10.26 yanghang
using Painter.Models.PhysicalModel;
using Painter.Models.StageModel;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Painter.Models.Paint
{
    public enum SCENE_OBJECT_TYPE
    {
        ROLE,//角色类 ：主角、敌人
        GROUND,//仅边界有效，而且不仅针对Y速度向下的时候
        PICTURE,
        OBSTACLE,//障碍物，物体
        WEAPON,//武器 归属Role类对象所有
        EFFECT,//效果类：粒子Particle
        RAY_BEAM,
        MIRROR,
        LENS
    };
    public enum SCENE_OBJECT_STATUS { IN_AIR, IN_GROUND };
    public enum SCENE_OBJECT_INTERFER { NONE, INTERFER };
    public class SceneObject
    {
        public event Action StatusUpdateEvent; 
        protected PointGeo Center = new PointGeo();//这个不准啊，不知为啥，GetOuterShape().GetShapeCenter()比较好
        public Scene CurrenScene;
        public SCENE_OBJECT_STATUS Status;
        public SCENE_OBJECT_INTERFER Interfer;
        public float Mass = 1;
        public bool EnableCheckInterfer = true;
        public bool EnableCheckCollision = true;//启用与Role对象碰撞
        public bool IsHaveTrack = false;
        public PointGeo GetCenter()
        {
            return Center;
        }
        public SceneObject()
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.GROUND;
            MaxX = 0;
            MaxY = 0;
            MinX = 1E6f;
            MinY = 1E6f;
            Status = SCENE_OBJECT_STATUS.IN_AIR;
            Interfer = SCENE_OBJECT_INTERFER.NONE;
        }
        public virtual void BeenHit(Weapon weapon)
        {

        }
        public virtual bool CheckCollision(SceneObject sceneObject)
        {
            if (!this.EnableCheckCollision)
            {
                return false;
            }
            DrawableObject shapeSelf = GetOutShape();
            DrawableObject other = sceneObject.GetOutShape();
            if (shapeSelf.IsOverlap(other))
            {
                this.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                return true;
            }
            return false;
        }
        public virtual DrawableObject GetOutShape()
        {
            return elements[0];
        }
        private bool _isDisposed = false;
        public bool IsDisposed
        {
            get { return this._isDisposed; }
            set
            {
                this._isDisposed = value;
                if (_isDisposed)
                {
                    foreach (var item in this.elements)
                    { 
                        item.IsDisposed = true; 
                    }
                }
            }
        }

        public void CalMaxMin()
        {
            MaxX = -1E6f;
            MaxY = -1E6f;
            MinX = 1E6f;
            MinY = 1E6f;
            foreach (var item in elements)
            {
                if (item is Shape)
                {
                    Shape shape = item as Shape;
                    if (this.MinX > shape.GetMinX())
                    {
                        this.MinX = shape.GetMinX();
                    }
                    if (this.MaxX < shape.GetMaxX())
                    {
                        this.MaxX = shape.GetMaxX();
                    }
                    if (this.MinY > shape.GetMinY())
                    {
                        this.MinY = shape.GetMinY();
                    }
                    if (this.MaxY < shape.GetMaxY())
                    {
                        this.MaxY = shape.GetMaxY();
                    }
                }
            } 
        }
        public float MaxX { get; set; }
        public float MaxY { get; set; }
        public float MinX { get; set; }
        public float MinY { get; set; }
        public SCENE_OBJECT_TYPE OBJECT_TYPE;
        protected List<DrawableObject> elements = new List<DrawableObject>();
        public PointGeo Pos = new PointGeo();
        public float Area = 0;

        public void Add(DrawableObject ele)
        {
            if (!this.elements.Contains(ele))
            {
                elements.Add(ele);
                if (ele is Shape)
                {
                    Shape shape = ele as Shape;
                    Center.X = (Center.X * this.Area + shape.GetShapeCenter().X * shape.GetArea()) / (this.Area + shape.GetArea());
                    Center.Y = (Center.Y * this.Area + shape.GetShapeCenter().Y * shape.GetArea()) / (this.Area + shape.GetArea());
                    this.Area += shape.GetArea();
                }
            }
            CalMaxMin();
        }

        internal List<DrawableObject> GetElements()
        {
            List<DrawableObject> temp = new List<DrawableObject>();
            for (int i = 0; i < elements.Count; i++)
            {
                temp.Add(elements[i]);
            } 
            return temp;
        }

        public virtual void Move(PointGeo pos)
        {
            Pos = pos;
            foreach (var item in elements)
            {
                item.Translate(pos);
            }
            Center += pos;
        }
        public virtual void SetStatus()
        {
            if (InAnimation)
            {
                AnimateStep();
            }
            StatusUpdateEvent?.Invoke();
        }
        private bool InAnimation = false;
        public bool IsInAnimate
        {
            get { return InAnimation; }
        }
        private PointGeo curPoint;
        private PointGeo endPoint;
        private PointGeo stepPoint;
        public void AnimateTo(PointGeo pointGeo, float timespan)
        {
            curPoint = new PointGeo();
            endPoint = pointGeo;
            int count = (int)(timespan / PhysicalField.TICK_TIME);
            stepPoint = pointGeo / count;
            InAnimation = true;
        }
        private void AnimateStep()
        {
            if (Math.Abs(this.curPoint.X + this.stepPoint.X) < Math.Abs(endPoint.X))
            {
                this.Move(stepPoint);
                this.curPoint += this.stepPoint;
            }
            else
            {
                this.Move(endPoint - curPoint);
                InAnimation = false;
            } 
        }
    }
    public enum DIRECTION { UP, DOWN, LEFT, RIGHT }

    class GroundObject : SceneObject
    {
        //最好定义为一个矩形,越小物体越无法弹起来（1E-6f就弹不动了）
        public float ReflectResistance = 0.8f;
        public int CarrayCount = 0;
        public GroundObject()
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.GROUND;
            ReflectionDirection = DIRECTION.UP;
        }
        private LineGeo ReflectedLine;
        public LineGeo GetReflectionLine()//仅仅是用来确定一个旋转的参考点，还需结合矩形角度
        {
            RectangleGeo rectange = elements[0] as RectangleGeo;
            if (rectange == null)
            {
                return null;
            }
            //if (ReflectedLine == null)
            //{
                switch (ReflectionDirection)
                {
                    case DIRECTION.UP:
                        ReflectedLine = new LineGeo(new PointGeo(rectange.GetMinX(), rectange.GetMaxY()), new PointGeo(rectange.GetMaxX(), rectange.GetMaxY()));
                        break;
                    case DIRECTION.DOWN:
                        ReflectedLine = new LineGeo(new PointGeo(rectange.GetMinX(), rectange.GetMinY()), new PointGeo(rectange.GetMaxX(), rectange.GetMinY()));
                        break;
                    case DIRECTION.LEFT:
                        ReflectedLine = new LineGeo(new PointGeo(rectange.GetMinX(), rectange.GetMinY()), new PointGeo(rectange.GetMinX(), rectange.GetMaxY()));
                        break;
                    case DIRECTION.RIGHT:
                        ReflectedLine = new LineGeo(new PointGeo(rectange.GetMaxX(), rectange.GetMinY()), new PointGeo(rectange.GetMaxX(), rectange.GetMaxY()));
                        break;
                    default:
                        break;
                }
            //}
            return ReflectedLine;
        }
        public DIRECTION ReflectionDirection;

        public float GroundFrictionRatio = 3f;


        public override void SetStatus()
        {
            if (CarrayCount > 0)
            {
                (this.GetOutShape().GetDrawMeta() as ShapeMeta).IsFill = true;
            }
            else
            {
                (this.GetOutShape().GetDrawMeta() as ShapeMeta).IsFill = false;
            }
            CarrayCount = 0;
            //if (ReflectedLine == null)
            //{
            //    RectangeGeo rectange = elements[0] as RectangeGeo;
            //    double x = rectange.GetMinX()+rectange.Width*Math.Cos(-rectange.Angle/180*Math.PI);
            //    double y = rectange.GetMinY() + rectange.Heigth * Math.Sin(-rectange.Angle / 180 * Math.PI);
            //    ReflectedLine = new LineGeo(new PointGeo(rectange.GetMinX(), rectange.GetMinY()), new PointGeo());
            //}
            base.SetStatus();
        }
    }
    public class Role : SceneObject
    {
        public bool IsAppliedGravity = true;
        public int LifeLength = 100;
        public int Attack = 20;
        public int Level = 1;
        public PointGeo Speed = new PointGeo();
        public PointGeo Acc = new PointGeo();
        public PointGeo Force = new PointGeo();
        public bool IsInGround = false;

        public override void Move(PointGeo pos)
        {
            
            if (MinY < -1000)
            {
                pos.Y = 2000;
                Speed.Y = 0;
            }
            base.Move(pos);
            //Pos += pos;
            //foreach (var item in elements)
            //{
            //    item.Translate(pos);
            //}
            //Center += pos;
        }

        internal void CheckInterfer(SceneObject sceneObject)
        {
            if (!sceneObject.EnableCheckInterfer)
            {
                return;
            }
            if (this== sceneObject)
            {
                return;
            }
            if (sceneObject.IsDisposed==true)
            {
                return;
            }
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    if (sceneObject is Enemy)
                    {
                        Enemy enemy = sceneObject as Enemy;
                        enemy.CalMaxMin();
                        if (CheckCollision(enemy))
                        {
                            if (StageController.EnableMomenta)
                            {
                                //if (this is MainCharacter)
                                //{
                                //和Enemy对象发生碰撞了 动量计算
                                if (!(this.GetOutShape() is Shape&& enemy.GetOutShape() is Shape))
                                {
                                    return;
                                }
                                PointGeo thisCenter = (this.GetOutShape() as Shape).GetShapeCenter();
                                PointGeo enemyCenter = (enemy.GetOutShape() as Shape).GetShapeCenter();
                                LineGeo line = new LineGeo(enemyCenter, thisCenter);//这里不是this.Center
                                PointGeo delta = this.Center - enemyCenter;
                                float angle = (float)(line.GetRad()/Math.PI*180);
                                CommonUtils.PointRotateAroundOrigin(angle, ref delta.X, ref delta.Y);
                                CommonUtils.PointRotateAroundOrigin(angle, ref this.Speed.X, ref this.Speed.Y);
                                CommonUtils.PointRotateAroundOrigin(angle, ref enemy.Speed.X, ref enemy.Speed.Y);
                                PointGeo speed = enemy.Speed - this.Speed;
                                enemy.Speed.X = ((enemy.Mass - this.Mass) * enemy.Speed.X + 2 * this.Mass * this.Speed.X) / (this.Mass + enemy.Mass);
                                this.Speed.X = speed.X + enemy.Speed.X;
                                CommonUtils.PointRotateAroundOrigin(-angle, ref this.Speed.X, ref this.Speed.Y);
                                CommonUtils.PointRotateAroundOrigin(-angle, ref enemy.Speed.X, ref enemy.Speed.Y);
                                //}
                            }
                        }

                    }
                    break;
                case SCENE_OBJECT_TYPE.OBSTACLE:
                    Obstacle obstacle = sceneObject as Obstacle;
                    obstacle.CalMaxMin();
                    if (this is MainCharacter)
                    {
                        if (this.CheckCollision(obstacle))
                        {
                            if (obstacle is FallingObstacle)
                            {
                                (this as MainCharacter).OnHitEnemy(-50);
                                obstacle.IsDisposed = true;
                            }

                        }
                    }
                    break;
                case SCENE_OBJECT_TYPE.GROUND:
                    GroundObject ground = sceneObject as GroundObject;
                    RectangleGeo rectange = ground.GetOutShape() as RectangleGeo;
                    LineGeo lineGeo = ground.GetReflectionLine();
                    if (lineGeo==null)
                    {
                        return;
                    }
                    if (!(this.GetOutShape() is Shape ))
                    {
                        return;
                    }
                    float radius = ((this.GetOutShape() as Shape).GetMaxX() - (this.GetOutShape() as Shape).GetMinX()) / 2;
                    PointGeo checkPoint = (this.GetOutShape() as Shape).GetShapeCenter().Clone();
                    PointGeo checkPRelativeToLine = checkPoint - lineGeo.FirstPoint;
                    PointGeo geoTemp = checkPRelativeToLine.Clone();
                    PointGeo force = this.Force.Clone();
                    CommonUtils.PointRotateAroundOrigin(rectange.Angle, ref checkPRelativeToLine.X, ref checkPRelativeToLine.Y);
                    CommonUtils.PointRotateAroundOrigin(rectange.Angle, ref this.Speed.X, ref this.Speed.Y);
                    CommonUtils.PointRotateAroundOrigin(rectange.Angle, ref this.Force.X, ref this.Force.Y);
                    float flash = Math.Abs(PhysicalField.TICK_TIME * this.Speed.Y);
                    if (flash < 5)
                    {
                        flash = 5;//不这么写，停下来的时候速度=0 flash=0,就直接穿过去了
                    }
                    if (Math.Abs(checkPRelativeToLine.Y - radius) < flash)
                    {
                        if (checkPRelativeToLine.X > 0 && checkPRelativeToLine.X < rectange.Width)
                        {
                            this.Status = SCENE_OBJECT_STATUS.IN_GROUND;
                            ground.CarrayCount++;
                            if (this.Speed.Y < 0.1)// 
                            {
                                //考虑斜面动静摩擦的版本，比较复杂，效果也没好多少
                                checkPRelativeToLine.Y = radius;
                                this.Speed.Y *= -ground.ReflectResistance;
                                CommonUtils.PointRotateAroundOrigin(-rectange.Angle, ref checkPRelativeToLine.X, ref checkPRelativeToLine.Y);
                                this.Move(checkPRelativeToLine - geoTemp);

                                if (Math.Abs(this.Speed.Y) < 0.3)
                                {
                                    this.Speed.Y = 0;
                                }
                                if (Math.Abs(this.Force.X) < 0.1)
                                {
                                    this.Force.X = 0;
                                }
                                if (Math.Abs(this.Speed.X) < 0.3)
                                {
                                    this.Speed.X = 0;
                                }
                                if (this.Force.X == 0)
                                {
                                    if (Math.Abs(this.Speed.X) > 0.01)
                                    {
                                        this.Force.X = -(this.Speed.X) / Math.Abs(this.Speed.X) * Math.Abs(Force.Y) * ground.GroundFrictionRatio;
                                    }
                                }
                                else if (this.Force.X * this.Speed.X > 0)
                                {
                                    this.Force.X = this.Force.X + Math.Abs(Force.Y) * ground.GroundFrictionRatio;
                                }
                                else
                                {
                                    if (Math.Abs(this.Force.X) > Math.Abs(Force.Y) * ground.GroundFrictionRatio)
                                    {
                                        this.Force.X = this.Force.X - Math.Abs(Force.Y) * ground.GroundFrictionRatio;
                                    }
                                    else
                                    {
                                        this.Force.X = 0;
                                    }
                                }
                                this.Force.Y = 0;
                                if (Math.Abs(this.Force.X) < 0.1)
                                {
                                    this.Force.X = 0;
                                    //this.Speed.X = 0;
                                    if (this is Enemy)
                                    {
                                        (this as Enemy).OnStopOnGround();
                                    }
                                }
                            }
                        }
                    }
                    CommonUtils.PointRotateAroundOrigin(-rectange.Angle, ref this.Speed.X, ref this.Speed.Y);
                    CommonUtils.PointRotateAroundOrigin(-rectange.Angle, ref this.Force.X, ref this.Force.Y);



                    //if (this.MinY - ground.MaxY > -flash && this.MinY - ground.MaxY < flash)
                    //{
                    //    if (this.MaxX < ground.MaxX + width && this.MinX >= ground.MinX - width)
                    //    {
                    //        this.Status = SCENE_OBJECT_STATUS.IN_GROUND;
                    //        ground.CarrayCount++;
                    //        //重置物体位置，使得贴近ground表面
                    //        if (this.Speed.Y < 0)
                    //        {
                    //            float deltaY = ground.MaxY - this.MinY;
                    //            float deltaX = -deltaY / this.Speed.Y * this.Speed.X;
                    //            this.Move(new PointGeo(deltaX, deltaY));
                    //            this.Speed.Y *= -ground.ReflectResistance;
                    //        }
                    //        break;
                    //    }
                    //}
                    break;
                case SCENE_OBJECT_TYPE.PICTURE:
                    break;

                case SCENE_OBJECT_TYPE.WEAPON:
                    break;
                default:
                    break;
            }
        }
    }
    class Enemy : Role
    {
        public Enemy()
        {
            Mass = 500;
            LifeLength = 30;
            OBJECT_TYPE = SCENE_OBJECT_TYPE.ROLE;
        }
        public event Action<Enemy> EnemyDisposedEvent;
        public override void BeenHit(Weapon weapon)
        {
            this.Speed.X += (float)(weapon.Speed * Math.Cos(weapon.Angle)) * weapon.Mass / this.Mass;
            this.Speed.Y += (float)(weapon.Speed * Math.Sin(weapon.Angle)) * weapon.Mass / this.Mass;
            this.LifeLength -= 10;
            if (LifeLength<=0)
            {
                this.IsDisposed = true;
                EnemyDisposedEvent?.Invoke(this);
            }
        }
        public void Reset()
        {
            PointGeo randomP = new PointGeo((float)(new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble() - 0.5) * 10, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);

            PointGeo origin = new PointGeo((randomP.X + 5) * 200, (randomP.Y + 5) * 50);

            foreach (var item in elements)
            {
                item.Translate(new PointGeo(-MaxX, -MaxY) + origin);
                if (item is Shape)
                {
                    Shape shape = item as Shape;
                    Center.X = (Center.X * this.Area + shape.GetShapeCenter().X * shape.GetArea()) / (this.Area + shape.GetArea());
                    Center.Y = (Center.Y * this.Area + shape.GetShapeCenter().Y * shape.GetArea()) / (this.Area + shape.GetArea());
                }
            }

            Pos = new PointGeo();
            Speed = randomP;
        }
        public override void Move(PointGeo pos)
        {
            if (MinY < -1000 || MaxX > 5000 || MinX < -600)
            {
                Reset();
                //pos.Y = 2000; 
                //Speed.Y = 0;
                //Speed.X = (float)(new Random().NextDouble()*2-1)*5;
            }
            else
            {
                base.Move(pos);
                //Pos += pos;
                //foreach (var item in elements)
                //{
                //    item.Translate(pos);
                //}
                //Center += pos;
            }
        }
        public event Action<Enemy> StopEvent;
        public void OnStopOnGround()
        {
            StopEvent?.Invoke(this);
            //this.Speed.X = ((float)new Random().NextDouble() * 2 - 1) * 8;
        }
    }
    public class MainCharacter : Role
    {
        public double HitCount = 0;
        public event Action HitEnemyEvent;
        public event Action PositionChange;
        public void OnHitEnemy(double score)
        {
            HitCount += score;
            if (HitEnemyEvent != null)
            {
                HitEnemyEvent();
            }
        }
        public MainCharacter()
        {
            Mass = 1000;
            OBJECT_TYPE = SCENE_OBJECT_TYPE.ROLE;
            LineGeo lineGeo = new LineGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            lineGeo.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AliceBlue, LineWidth = 5 });
            LineGeo lineGeo2 = new LineGeo(new PointGeo(0, 0), new PointGeo(-100, 100));
            lineGeo2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AliceBlue, LineWidth = 5 });
            CircleGeo circle = new CircleGeo(new PointGeo(0, -50), new PointGeo(0, -100));
            circle.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Aquamarine, IsFill = true, BackColor = System.Drawing.Color.Bisque, LineWidth = 4 });
            CircleGeo circle1 = new CircleGeo(new PointGeo(-40, -20), new PointGeo(-40, -50));
            circle1.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AntiqueWhite, IsFill = false, BackColor = System.Drawing.Color.Bisque, LineWidth = 4 });
            CircleGeo circle2 = new CircleGeo(new PointGeo(40, -20), new PointGeo(40, -50));
            circle2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AntiqueWhite, IsFill = false, BackColor = System.Drawing.Color.Bisque, LineWidth = 4 });

            elements.Add(lineGeo2);
            elements.Add(lineGeo);
            elements.Add(circle);
            elements.Add(circle1);
            elements.Add(circle2);
            //lineGeo2.IsShow = false;
            //lineGeo.IsShow = false;
            //circle.IsShow = false;
            //circle1.IsShow = false;
            //circle2.IsShow = false;

        }
        private int StatusCount = 0;
        private int TickCount = 0;

        public override DrawableObject GetOutShape()
        {
            return elements[2] as Shape;
            //return new RectangeGeo(new PointGeo(this.MinX, this.MinY), new PointGeo(this.MaxX, this.MaxY));
        }
        public override void SetStatus()
        { 
            if (Status == SCENE_OBJECT_STATUS.IN_AIR)
            {
                TickCount++;
                if (TickCount % 1 == 0)
                {
                    LineGeo line = elements[0] as LineGeo;
                    LineGeo line2 = elements[1] as LineGeo;
                    StatusCount++;
                    if (StatusCount < 20)
                    {
                        line.Rotate(1);
                        line2.Rotate(-1);
                    }
                    else
                    {
                        line.Rotate(-1);
                        line2.Rotate(1);
                        if (StatusCount > 40)
                        {
                            StatusCount = 0;
                        }
                    }
                }
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = false;
            }
            else
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
            }
            if (this.Interfer == SCENE_OBJECT_INTERFER.NONE)
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.Bisque;
                //this.Speed = new PointGeo();
            }
            else
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.Red;
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
            }
            base.SetStatus();

        }
        public void OnAttack()
        {

        }
        public void OnFire(PointGeo target)
        {
            Weapon weapon = new Weapon(new PointGeo(this.MaxX / 2 + this.MinX / 2, this.MaxY / 2 + this.MinY / 2), target);
            weapon.Owner = this;
            this.Speed.X -= (float)(weapon.Speed * Math.Cos(weapon.Angle)) * weapon.Mass / this.Mass;
            this.Speed.Y -= (float)(weapon.Speed * Math.Sin(weapon.Angle)) * weapon.Mass / this.Mass;
            this.CurrenScene.AddObject(weapon, false);
        }
        public bool AvoidFallingDown = true;
        public override void Move(PointGeo pos)
        {
            Pos += pos;
            if (AvoidFallingDown)
            {
                if (MinY < -1000)
                {
                    pos.Y = 2000;
                    this.HitCount -= 100;
                    Speed.Y = 0;
                }
            } 
            foreach (var item in elements)
            {
                item.Translate(pos);
            }
            Center += pos;
            PositionChange?.Invoke();
        }
       
    }
    class FallingObstacle : Obstacle
    {
        private static Random ran = new Random();
        private int ObNo = 0;
        public FallingObstacle()
        {
            No++;
            ObNo = No;
            Brightness = ran.Next(60, 100);
            Hue = ran.Next(30, 360);
            Speed = new PointGeo(ran.Next(8) - 4, -ran.Next(5, 100) / 200.0f);
        }
        public override void BeenHit(Weapon weapon)
        {
            base.BeenHit(weapon);
            IsDisposed = true;
            for (int i = 0; i < elements.Count; i++)
            {
                (elements[i] as Shape).IsDisposed = true;
            }
        }
        private int TickCount = 0;
        private int Hue = 30;
        private int Brightness = 60;
        public override void Move(PointGeo pos)
        {
            if (MinY < -500)
            {
                pos.Y = 2000;
                Speed = new PointGeo(ran.Next(8) - 4, -ran.Next(5, 100) / 200.0f);
            }
            if (MaxX < -500)
            {
                Speed.X = Math.Abs(Speed.X);
            }
            if (MaxX > 3000)
            {

                Speed.X = -Math.Abs(Speed.X);
            }
            foreach (var item in elements)
            {
                item.Translate(pos);
            }
            Center += pos;
        }
        private static int No = 0;
        public override void SetStatus()
        {
            base.SetStatus();
            if (IsDisposed)
            {
                for (int i = 0; i < elements.Count; i++)
                {
                    (elements[i] as Shape).IsDisposed = true;
                }
            }
            TickCount++;
            if (TickCount % 10 == 0)
            {
                ReciprocateValue(ref Hue, ref reverseHue, 30, 360);
                ReciprocateValue(ref Brightness, ref reverseBrightness, 75, 100);
            }
            var color = CommonUtils.HslToRgb(Hue, 100, Brightness);
            ((this.elements[0] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
            //if (ObNo == 3)
            //{
            //    Debug.Print("Hue" + Hue);
            //}

        }
        bool reverseHue = true;
        bool reverseBrightness = true;
        private void ReciprocateValue(ref int num, ref bool revere, int min = 0, int max = 100)
        {

            if (num > max && num < min)
            {
                num = new Random().Next(min, max);
            }
            if (num == max)
            {
                revere = false;
            }
            else if (num == min)
            {
                revere = true;
            }
            if (revere)
            {
                num++;
            }
            else
            {
                num--;
            }
        }
    }
    public class Obstacle : SceneObject
    {
        public event Action UpdateEvent;
        public override void SetStatus()
        {
            base.SetStatus();
            UpdateEvent?.Invoke();
        }
        public PointGeo Speed = new PointGeo();
        public Obstacle()
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.OBSTACLE;
        }
        public override void BeenHit(Weapon weapon)
        {
            if (EnabledRotate)
            {
                if (weapon.Angle < Math.PI / 2 && weapon.Angle > -Math.PI / 2)
                {
                    elements[0].Rotate(-1);
                }
                else
                {
                    elements[0].Rotate(1);
                }
            }
        }
        public bool EnabledRotate = false;
    }
    public class Weapon : SceneObject
    {
        public int CreateTimeTick { get; set; }
        PointGeo fromPoint;
        public PointGeo toPoint { get; set; }
        PointGeo curPoint;
        public double MaxRange = 5000;
        public static int CurrentID = 0;
        private int particleCount = 20;
        public int ParticleCount
        {
            get { return particleCount; }
            set
            {
                particleCount = value;
            }
        }
        private CircleGeo HitCircle = new CircleGeo();
        public override DrawableObject GetOutShape()
        {
            return HitCircle;
        }
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public event Action<PointGeo, int> ExposedEvent;
        public double Distance
        {
            get
            {
                return Math.Sqrt(1.0 * (toPoint.X - fromPoint.X) * (toPoint.X - fromPoint.X) +
(toPoint.Y - fromPoint.Y) * (toPoint.Y - fromPoint.Y) * 1.0);
            }
        }
        public static int MAX_POINTS = 3;
        public List<PointGeo> listPoints;
        public int Hue = 40;
        public double Angle { get { return Math.Atan2((toPoint.Y - fromPoint.Y), (toPoint.X - fromPoint.X)); } }

        private double _speed;
        public double Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
            }
        }
        private double _accelerate;
        public double Accelerator
        {
            get { return _accelerate; }
            set
            {
                _accelerate = value;
            }
        }
        private int _brightness;
        public int Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }
        private double _targetRadius;
        public double TargetRadius
        {
            get { return _targetRadius; }
            set { _targetRadius = value; }
        }
        private static Random ran = new Random();
        public Role Owner;
        public Weapon(PointGeo _fromPoint, PointGeo _toPoint)
        {
            Mass = 100;
            OBJECT_TYPE = SCENE_OBJECT_TYPE.WEAPON;
            Interfer = SCENE_OBJECT_INTERFER.NONE;
            CurrentID++;
            this.fromPoint = _fromPoint;
            this.toPoint = _toPoint;
            this.curPoint = new PointGeo(fromPoint.X, fromPoint.Y);
            Speed = 5;
            Accelerator = 1.05;
            Brightness = ran.Next(60, 100);
            TargetRadius = 5;
            Hue = ran.Next(30, 360);
            listPoints = new List<PointGeo>(MAX_POINTS);
            this.ID = CurrentID;
            RandomLines randomLines = new RandomLines();
            randomLines.SetPoints(listPoints);
            var c = CommonUtils.HslToRgb(Hue, 100, Brightness);
            randomLines.SetDrawMeta(new ShapeMeta() { LineWidth = 3, ForeColor = System.Drawing.Color.FromArgb(c.red, c.green, c.blue) });
            randomLines.IsHold = false;
            this.elements.Add(randomLines);
            ExposedEvent += Weapon_CreateParticles;
            HitCircle.FirstPoint = fromPoint.Clone();
        }

        private void Weapon_CreateParticles(PointGeo point, int hue)
        {
            for (int i = 0; i < ParticleCount; i++)
            {
                Spark p = new Spark(point, hue);
                this.CurrenScene.AddObject(p, true);
            }
        }

        public static double GetPointDistance(PointGeo x1, PointGeo x2)
        {
            return Math.Sqrt(1.0 * (x2.X - x1.X) * (x2.X - x1.X) + 1.0 * (x2.Y - x1.Y) * (x2.Y - x1.Y));
        }
        private SceneObject hitObject;
        public override void SetStatus()
        {
            if (listPoints.Count > 1)
            {
                HitCircle.FirstPoint = listPoints[listPoints.Count - 1];
                HitCircle.Radius = 5;
            }
            #region 这样能够停在指定点 
            if (curPoint == toPoint)
            {
                IsDisposed = true;//这样最后一个点能够加上去 并显示出来
            }
            if (IsDisposed)
            {
                (elements[0] as RandomLines).IsShow = false;//这样能够在Scene移除前，在canvas里面也移除掉
                return;
            }
            #endregion
            if (!IsDisposed)
            { 
                if (this.listPoints.Count > 3) this.listPoints.RemoveAt(0);
                this.listPoints.Add(curPoint.Clone());
                if (TargetRadius < 8) TargetRadius += 0.3;
                else TargetRadius = 1;
                Speed *= this.Accelerator;
                double deltaX = Math.Cos(this.Angle) * Speed;
                double deltaY = Math.Sin(this.Angle) * Speed;
                curPoint.X += (float)deltaX;
                curPoint.Y += (float)deltaY;

            }  
            
            if (GetPointDistance(curPoint, fromPoint) >= MaxRange || Interfer == SCENE_OBJECT_INTERFER.INTERFER)
            {
                toPoint = curPoint; 
                this.listPoints.Add(curPoint.Clone());
                if (Interfer == SCENE_OBJECT_INTERFER.INTERFER)
                {
                    MainCharacter ch = this.Owner as MainCharacter;
                    if (ch != null && hitObject != null)
                    {
                        if (hitObject is Enemy)
                        {
                            if (hitObject.GetOutShape() is CircleGeo)
                            {
                                ch.OnHitEnemy(this.Speed);
                            }
                            else
                            {
                                ch.OnHitEnemy(Math.Log(this.Speed + 1));
                            }
                        }
                        else if (hitObject is FallingObstacle)
                        {
                            ch.OnHitEnemy(20);
                        }
                        else
                        {
                            ch.OnHitEnemy(-Math.Sqrt(this.Speed));
                        }
                    };
                }
                if (ExposedEvent != null)
                {
                    ExposedEvent(curPoint, Hue);
                } 
            }

        }

        internal void CheckInterfer(SceneObject sceneObject)
        {
            if (this.Owner == sceneObject)
            {
                return;
            }
            if (!sceneObject.EnableCheckInterfer)
            {
                return;
            }
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    if (sceneObject.CheckCollision(this))
                    {
                        this.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                        hitObject = sceneObject;
                        sceneObject.BeenHit(this);
                    }
                    break;
                case SCENE_OBJECT_TYPE.GROUND:
                    if (sceneObject.CheckCollision(this))
                    {
                        //sceneObject.CheckCollision(this);
                        this.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                        hitObject = sceneObject;
                        sceneObject.BeenHit(this);
                    }

                    break;
                case SCENE_OBJECT_TYPE.PICTURE:
                    break;
                case SCENE_OBJECT_TYPE.OBSTACLE:
                    if (sceneObject.CheckCollision(this))
                    {
                        this.Interfer = SCENE_OBJECT_INTERFER.INTERFER;
                        hitObject = sceneObject;
                        sceneObject.BeenHit(this);
                    }
                    break;
                case SCENE_OBJECT_TYPE.WEAPON:
                    break;
                case SCENE_OBJECT_TYPE.EFFECT:
                    break;
                default:
                    break;
            }
        }
    }
    class Spark : SceneObject
    {
        PointGeo curPos;
        List<PointGeo> listPoints;
        double Angle;
        double Speed;
        double friction = 0.95;
        public double gravity { get; set; }
        int Brightness;
        double Alpha = 1;
        int Hue;
        double Decay;
        static int MAX_COUNT = 5;
        public Spark(PointGeo p, int hue)
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.EFFECT;
            this.curPos = p.Clone();
            listPoints = new List<PointGeo>(MAX_COUNT);
            Angle = ran.NextDouble() * Math.PI * 2;

            Speed = ran.NextDouble() * 10;//产生的随机数是一样的
            //Debug.Print("Speed" + this.Speed);
            Hue = ran.Next(hue - 30, hue + 30);
            Brightness = ran.Next(60, 100);
            Decay = ran.NextDouble() * 0.0045 + 0.0045;
            gravity = -0.98 / 2;
            RandomLines randomLines = new RandomLines();
            randomLines.SetPoints(listPoints);
            var c = CommonUtils.HslToRgb(Hue, 100, Brightness);
            randomLines.SetDrawMeta(new ShapeMeta() { LineWidth = 1, ForeColor = System.Drawing.Color.FromArgb(c.red, c.green, c.blue) });
            randomLines.IsHold = false;
            this.elements.Add(randomLines);
        }
        static Random ran = new Random(DateTime.Now.Millisecond);
        public override void SetStatus()
        {
            if (IsDisposed)
            {
                (elements[0] as RandomLines).IsShow = false;
                listPoints.Clear();
                return;
            }
            if (this.listPoints.Count > MAX_COUNT) listPoints.RemoveAt(0);
            listPoints.Add(new PointGeo(curPos.X, curPos.Y));
            Speed *= friction;
            double deltaX = Math.Cos(this.Angle) * Speed;
            double deltaY = Math.Sin(this.Angle) * Speed + gravity;
            curPos.X += (float)deltaX;
            curPos.Y += (float)deltaY;
            Alpha -= Decay;
            if (Alpha < Decay)
            {
                IsDisposed = true;
                (elements[0] as RandomLines).IsShow = false;
                listPoints.Clear();
            }
            var color = CommonUtils.HslToRgb(Hue, 100, Brightness);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
            (elements[0] as RandomLines).GetDrawMeta().ForeColor = c;
        }
    }

}
