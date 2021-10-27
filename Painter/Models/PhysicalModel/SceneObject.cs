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
    public enum SCENE_OBJECT_TYPE { ROLE,GROUND,PICTURE,OBSTACLE,WEAPON,EFFECT};
    public enum SCENE_OBJECT_STATUS {IN_AIR,IN_GROUND};
    public enum SCENE_OBJECT_INTERFER {NONE,INTERFER};
    class SceneObject
    {
        public Scene CurrenScene;
        public SCENE_OBJECT_STATUS Status;
        public SCENE_OBJECT_INTERFER Interfer;
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
        public virtual Shape GetOutShape()
        {
            return elements[0] as Shape;
        }
        public bool IsDisposed = false;

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
        protected List<IPrintAndOperatable> elements = new List<IPrintAndOperatable>();
        public PointGeo Pos = new PointGeo();
         
        public void Add(IPrintAndOperatable ele)
        {
            if (!this.elements.Contains(ele))
            {
                elements.Add(ele);
            }
        }

        internal List<IScreenPrintable> GetElements()
        {
            List<IScreenPrintable> temp = new List<IScreenPrintable>();
            foreach (var item in elements)
            {
                temp.Add(item);
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
        }
        public virtual void SetStatus()
        {

        }
    }
    class GroundObject : SceneObject
    {
        public float ReflectResistance=0.8f;

    }
    class Role : SceneObject
    {
        public int LifeLength = 100;
        public int Attack = 20;
        public int Level = 1;
        public PointGeo Speed = new PointGeo();
        public PointGeo Acc = new PointGeo();
        public PointGeo Force = new PointGeo();
        public float Mass = 1000;
        public bool IsInGround = false;
        public bool IsCollide = false; 
        public override void Move(PointGeo pos)
        {
            Pos += pos;
            if (MinY < -1000)
            {
                pos.Y = 2000;
                Speed.Y = 0;
            }
            foreach (var item in elements)
            {
                item.Translate(pos);
            }
        }
        
        public bool CheckCollision(SceneObject sceneObject)
        {
            Shape shapeSelf = GetOutShape();
            Shape other = sceneObject.GetOutShape();
            IsCollide= shapeSelf.IsOverlap(other);
            return IsCollide;
        }
    }
    class Enemy : Role
    {
        public Enemy()
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.ROLE;
            RectangeGeo rectange = new RectangeGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            rectange.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Red, LineWidth = 5,BackColor= System.Drawing.Color.OrangeRed,IsFill=true }); 
            elements.Add(rectange);
        }
        public void Reset()
        {
            PointGeo randomP = new PointGeo((float)(new Random(System.DateTime.UtcNow.Millisecond + 10).NextDouble()-0.5) * 10, (float)new Random(System.DateTime.UtcNow.Millisecond).NextDouble() * 5);

            PointGeo origin = new PointGeo((randomP.X + 5) * 200, (randomP.Y + 5) * 50);
            
            foreach (var item in elements)
            {
                item.Translate(new PointGeo(-MaxX,-MaxY)+origin);
            }
            Pos = new PointGeo();
            Speed = randomP;
        }
        public override void Move(PointGeo pos)
        { 
            if (MinY < -1000||MaxX>3000||MinX<-600)
            {
                Reset();
                //pos.Y = 2000; 
                //Speed.Y = 0;
                //Speed.X = (float)(new Random().NextDouble()*2-1)*5;
            }
            else
            {
                Pos += pos;
                foreach (var item in elements)
                {
                    item.Translate(pos);
                }
            }
        }
    }
    class MainCharacter: Role
    { 
        public MainCharacter()
        { 
            OBJECT_TYPE = SCENE_OBJECT_TYPE.ROLE; 
            LineGeo lineGeo = new LineGeo(new PointGeo(0, 0), new PointGeo(100, 100));
            lineGeo.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AliceBlue, LineWidth = 5 });
            LineGeo lineGeo2 = new LineGeo(new PointGeo(0, 0), new PointGeo(-100, 100));
            lineGeo2.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.AliceBlue, LineWidth = 5 });
            CircleGeo circle = new CircleGeo(new PointGeo(0, -50), new PointGeo(0, -100));
            circle.SetDrawMeta(new Painters.ShapeMeta() { ForeColor = System.Drawing.Color.Aquamarine, IsFill = true, BackColor = System.Drawing.Color.Bisque, LineWidth = 4 });
            elements.Add(lineGeo2);
            elements.Add(lineGeo);
            elements.Add(circle); 
        }
        private int StatusCount = 0;
        private int TickCount = 0;
        
        public override Shape GetOutShape()
        {
            return new RectangeGeo(new PointGeo(this.MinX, this.MinY), new PointGeo(this.MaxX, this.MaxY));//elements[2] as Shape;
        }
        public override void SetStatus()
        {
            
            if (Status==SCENE_OBJECT_STATUS.IN_AIR)
            {
                TickCount++;
                if (TickCount % 2 == 0)
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
            } 
            if (this.Interfer == SCENE_OBJECT_INTERFER.NONE)
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = false;
            }
            else
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
            }
            if (IsCollide)
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.Red;
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).IsFill = true;
                this.Speed = new PointGeo();
            }
            else
            {
                ((this.elements[2] as Shape).GetDrawMeta() as ShapeMeta).BackColor = System.Drawing.Color.Bisque;
            }

        }
        public void OnAttack()
        {

        }
        public void OnFire(PointGeo target)
        {
            this.CurrenScene.AddObject(new Weapon(new PointGeo(this.MaxX/2+this.MinX/2,this.MaxY/2+this.MinY/2), target),false);
        }
    }
    class Weapon:SceneObject
    {
        public int CreateTimeTick { get; set; }
        PointGeo fromPoint;
        public PointGeo toPoint { get; set; }
        PointGeo curPoint;
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
        private int _id = 0;
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public event Action<PointGeo, int> CreateParticles;
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
            set { _brightness = value;  }
        }
        private double _targetRadius;
        public double TargetRadius
        {
            get { return _targetRadius; }
            set { _targetRadius = value;  }
        }
        private static Random ran = new Random();
        public Weapon(PointGeo _fromPoint, PointGeo _toPoint)
        {
            OBJECT_TYPE = SCENE_OBJECT_TYPE.WEAPON;
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
            randomLines.SetDrawMeta(new ShapeMeta() { LineWidth = 3, ForeColor =  System.Drawing.Color.FromArgb(c.red, c.green, c.blue) });
            randomLines.IsHold = false;
            this.elements.Add(randomLines);
            CreateParticles += Weapon_CreateParticles;
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
        public override void SetStatus()
        {
            if (IsDisposed)
            {
                (elements[0] as RandomLines).IsShow = false;
                return; 
            }
            if (GetPointDistance(curPoint, fromPoint) >= Distance)
            {
                curPoint = toPoint;
                if (CreateParticles != null)
                {
                    CreateParticles(toPoint, Hue); 
                }
                IsDisposed = true;
            }
            if (this.listPoints.Count > 3) this.listPoints.RemoveAt(0);
            this.listPoints.Add(new PointGeo(curPoint.X, curPoint.Y));
            if (TargetRadius < 8) TargetRadius += 0.3;
            else TargetRadius = 1;
            Speed *= this.Accelerator;
            double deltaX = Math.Cos(this.Angle) * Speed;
            double deltaY = Math.Sin(this.Angle) * Speed;
            curPoint.X += (float)deltaX;
            curPoint.Y += (float)deltaY; 
        }
         
    }
    class Spark:SceneObject
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
            gravity = -0.98/2; 
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
                listPoints.Clear();
            }
            var color = CommonUtils.HslToRgb(Hue, 100, Brightness);
            System.Drawing.Color c = System.Drawing.Color.FromArgb(color.red, color.green, color.blue);
            (elements[0] as RandomLines).GetDrawMeta().ForeColor = c;
        } 
    }

}
