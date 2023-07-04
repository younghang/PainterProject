using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.GameModel.SceneModel.OpticalModel
{
    // Define a struct for 2D vectors
    struct Vector2
    {
        public double x;
        public double y;

        public Vector2(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        // Add two vectors
        public static Vector2 operator +(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x + v2.x, v1.y + v2.y);
        }

        // Subtract two vectors
        public static Vector2 operator -(Vector2 v1, Vector2 v2)
        {
            return new Vector2(v1.x - v2.x, v1.y - v2.y);
        }

        // Multiply a vector by a scalar
        public static Vector2 operator *(Vector2 v, double s)
        {
            return new Vector2(v.x * s, v.y * s);
        }

        // Dot product of two vectors
        public static double Dot(Vector2 v1, Vector2 v2)
        {
            return v1.x * v2.x + v1.y * v2.y;
        }

        // Magnitude of a vector
        public static double Mag(Vector2 v)
        {
            return Math.Sqrt(Dot(v, v));
        }

        // Angle of a vector in radians
        public static double Angle(Vector2 v)
        {
            return Math.Atan2(v.y, v.x);
        }
    }

    // Define a struct for 2D rays
    struct Ray2
    {
        public Vector2 start; // Start point of the ray
        public Vector2 end; // End point of the ray
        public Vector2 dir; // Direction vector of the ray

        public Ray2(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            this.dir = (end - start) * (1 / (Vector2.Mag(end - start))); // Normalize the direction vector
        }

        // Define a method to check if a ray intersects an arc
        // If yes, return true and set outPoint to be the intersection point
        // If no, return false and set outPoint to be null
        public static bool RayArcIntersect(Ray2 ray, Arc2 arc, out Vector2? outPoint, out double t)
        {
            // Define some variables for convenience
            Vector2 d = ray.dir; // Direction vector of ray
            Vector2 f = ray.start - arc.center; // Vector from center of circle to ray start
            t = 0;
            // Solve the quadratic equation for t
            double a = Vector2.Dot(d, d);
            double b = 2 * Vector2.Dot(f, d);
            double c = Vector2.Dot(f, f) - arc.radius * arc.radius;
            double discriminant = b * b - 4 * a * c;
            double tolMin = 1E-1;
            // Check if there is any solution
            if (discriminant < 0)
            {
                // No intersection
                outPoint = null;
                return false;
            }
            else
            {
                // There are two possible solutions, t1 and t2
                discriminant = Math.Sqrt(discriminant);
                double t1 = (-b - discriminant) / (2 * a);
                double t2 = (-b + discriminant) / (2 * a);
                Func<double, bool> checkValue = (angle) => { return angle >= arc.startAngle && angle <= arc.endAngle; };
                // Check if t1 is a valid solution
                if (t1 > tolMin && t1 < t2)
                {
                    // Find the intersection point using t1
                    Vector2 p1 = ray.start + d * t1;

                    // Check if p1 is on the arc
                    double angle1 = Vector2.Angle(p1 - arc.center);

                    if (checkValue(angle1) || checkValue(angle1 + Math.PI * 2) || checkValue(angle1 - Math.PI * 2))
                    {
                        // p1 is the intersection point
                        outPoint = p1;
                        t = t1;
                        return true;
                    }
                }

                // Check if t2 is a valid solution
                if (t2 > tolMin)
                {
                    // Find the intersection point using t2
                    Vector2 p2 = ray.start + d * t2;

                    // Check if p2 is on the arc
                    double angle2 = Vector2.Angle(p2 - arc.center);
                    if (checkValue(angle2) || checkValue(angle2 + Math.PI * 2) || checkValue(angle2 - Math.PI * 2))
                    {
                        // p2 is the intersection point
                        outPoint = p2;
                        t = t2;
                        return true;
                    }
                }

                // Neither t1 nor t2 is a valid solution
                outPoint = null;
                return false;
            }
        }

    }

    // Define a struct for 2D circular arcs
    struct Arc2
    {
        public Vector2 center; // Center point of the arc
        public double radius; // Radius of the arc
        public double startAngle; // Start angle of the arc in radians
        public double endAngle; // End angle of the arc in radians

        public Arc2(Vector2 center, double radius, double startAngle, double endAngle)
        {
            this.center = center;
            this.radius = radius;
            this.startAngle = startAngle;
            this.endAngle = endAngle;
        }
    }

    public class LensMaterial {
        public string Name { get; set; }
        public string Equation;
    }
    public class MaterialDataBase
    {
        private MaterialDataBase(){
            Materials.Add(new LensMaterial()
            {
                Name = "NBK7",
                Equation = "fx(lambda)=1.03961212+0.00600069867/(pow(lambda,2)-0.0139332363)+0.231792344/(pow(lambda,2)-0.0200179144)+1.01046945/(pow(lambda,2)-103.560653)"
            });
            Materials.Add(new LensMaterial()
            {
                Name = "Mirror",
                Equation = ""
            });

        }
        private static MaterialDataBase _instance = null;
        public static MaterialDataBase Instance
        {
            get
            {
                if (_instance==null)
                {
                    _instance = new MaterialDataBase(); 
                }
                return _instance;
            }
        }
        public List<LensMaterial> Materials = new List<LensMaterial>();
        public LensMaterial LoadMaterialByName(string Name)
        {
            for (int i = 0; i < Materials.Count; i++)
            {
                if (Materials[i].Name== Name)
                {
                    return Materials[i];
                }
            }
            return null;
        }
        public void RemoveMaterial(string name)
        {
            for (int i = 0; i < this.Materials.Count; i++)
            {
                if (Materials[i].Name == name)
                {
                    Materials.RemoveAt(i);
                    return;
                }
            }
            }
        public void AddMaterail(string name,string equation)
        {
            for (int i = 0; i < this.Materials.Count; i++)
            {
                if (Materials[i].Name==name)
                {
                    Materials[i].Equation = equation;
                    return;
                }
            }
            this.Materials.Add(new LensMaterial() {Name=name,Equation=equation });
        }
        public int GetMaterialIndex(string name)
        {
            for (int i = 0; i < Materials.Count; i++)
            {
                if (Materials[i].Name == name)
                {
                    return i;
                }
            }
            return -1;
        }
    }
    public enum LEN_TYPE
    {
        POSITIVE,
        NEGETIVE,
        ARC,
        MIRROR
    }
    public class LensObject : Obstacle
    {
        protected PointGeo left_vertex = new PointGeo(0, 120);
        public virtual PointGeo LeftVertex {
            get { 
                return left_vertex; 
            }
            set
            {
                left_vertex = value;
                InitElement();
            }
        }
        public virtual void SetColor(Color FilledColor)
        {

        }
        public override void Move(PointGeo pos)
        {
            base.Move(pos);
            this.left_vertex += pos;
        }
        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value; 
                foreach (var item in elements)
                {
                    if (selected)
                    {
                        item.GetDrawMeta().DashLineStyle =new float[]{ 1f,2f,1f};
                    }else
                    {
                        item.GetDrawMeta().DashLineStyle = null;
                    }
                }
              
            }
        }
        public virtual PointGeo Normal
        {
            get {
                PointGeo direction = EndPos - StartPos; 
                // rotate the direction vector 90 degrees counterclockwise
                PointGeo normal = new PointGeo(-direction.Y, direction.X); 
                normal.Normalized();
                return normal; 
            }

        }
        public Color BackColor = Color.Aqua;
        public LEN_TYPE LenType=LEN_TYPE.ARC;
        public PointGeo StartPos=new PointGeo();
        public PointGeo EndPos=new PointGeo();
        public string Name;
        public LensObject()
        {

        }
        public LensObject(PointGeo startPos, PointGeo endPos, string name= "NBK7")
        { 
            StartPos = startPos;
            EndPos = endPos;
            Name = name;
            InitElement();
        }
        public virtual void InitElement()
        {
            for (int i = 0; i < elements.Count; i++)
            {
                elements[i].IsDisposed = true;
            }
            elements.Clear();
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.Black, BackColor = Color.Black, LineWidth = 3 };
            LineGeo lineGeo = new LineGeo();
            lineGeo.FirstPoint = this.StartPos;
            lineGeo.SecondPoint = this.EndPos;
            lineGeo.SetDrawMeta(lineMeta);
            this.Add(lineGeo);
        }
         
        public virtual PointGeo GetNewDir(float index0, float index1, PointGeo dir_ori, PointGeo normal)
        {
            if (dir_ori.Dot(normal) < 0)
            {
                normal = normal * -1;
            }
            PointGeo newDir = new PointGeo();
            
            float pvalue = (float)Math.Sqrt(index1 * index1 - index0 * index0 + Math.Pow(normal.Dot(dir_ori*index0), 2)) - normal.Dot(dir_ori*index0);
            newDir = dir_ori*index0 + pvalue * normal;
            newDir = newDir * (float)(1.0f / index1);

            //A'=A+p N 
            //p = n'cosI'-ncosI

            newDir.Normalized();
            return newDir;
        }
    }

    class PositiveLens : LensObject
    {
        public float Thick=2;
        public float RRight=50;
        public float RLeft=50;
        public float Height=40; 
        public float Angle=0;
         
        public PositiveLens(string name= "NBK7")
        {
            left_vertex = new PointGeo(120, 0);
            Name = name;
            InitElement();
        }
        public PositiveLens():this("NBK7")
        {

        }
        public PositiveLens(float thick, float rRight, float rLeft, float height, PointGeo leftVertex, float angle, string name = "NBK7")
        {
            Name = name;
            Thick = thick;
            RRight = rRight;
            RLeft = rLeft;
            Height = height;
            this.left_vertex = leftVertex;
            Angle = angle;
            InitElement();
        }
        public override void SetColor(Color FilledColor)
        {
            BackColor = FilledColor;
            for (int i = 0; i < this.GetElements().Count; i++)
            {
                DrawableObject drawableObject = this.GetElements()[i];
                if (drawableObject is PathGeo)
                {
                    drawableObject.GetDrawMeta().BackColor = FilledColor;
                }
            }
        }
        public override void InitElement()
        {
            foreach (var item in this.elements)
            {
                item.IsDisposed = true;
            }
            elements.Clear();
            LenType = LEN_TYPE.POSITIVE;
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.Black, BackColor = Color.Black, LineWidth = 3 };
            
            float leftDgree = (float)(Math.Asin(Height / 2/ RLeft)/Math.PI*180);
              
            ArcGeo leftArc = new ArcGeo();
            leftArc.StartAngle = (180 -leftDgree)+Angle;
            leftArc.EndAngle = (180 + leftDgree )+ Angle;
            PointGeo dir = new PointGeo(1,0);
            dir.RotateAroundOrigin(Angle, LeftVertex);
            PointGeo centerOfLeftArc = LeftVertex + dir * RLeft;
            leftArc.CenterX = centerOfLeftArc.x;
            leftArc.CenterY = centerOfLeftArc.y;
            leftArc.Radius = RLeft;
            leftArc.SetDrawMeta(lineMeta);

            LineGeo bottomLineGeo = new LineGeo();
            bottomLineGeo.FirstPoint = leftArc.EndPos;
            bottomLineGeo.SecondPoint = leftArc.EndPos+dir*Thick;
            bottomLineGeo.SetDrawMeta(lineMeta);

            float rightDgree = (float)(Math.Asin(Height / 2/ RRight) / Math.PI*180  );

            ArcGeo rightArc = new ArcGeo();
            rightArc.StartAngle = ( - rightDgree) + Angle;
            rightArc.EndAngle = (rightDgree) + Angle;
            float leftMargin = (float)(RLeft - Math.Sqrt(RLeft * RLeft - Height * Height / 4));
            float rightMargin = (float)(RRight - Math.Sqrt(RRight * RRight - Height * Height / 4));
            PointGeo RightVertex = LeftVertex + dir * (leftMargin + Thick + rightMargin);
            this.Center= (RightVertex + LeftVertex)*0.5f;
            PointGeo centerOfrightArc = LeftVertex + dir *( leftMargin + Thick+ rightMargin-RRight);
            rightArc.CenterX = centerOfrightArc.x;
            rightArc.CenterY = centerOfrightArc.y;
            rightArc.Radius = RRight;
            rightArc.SetDrawMeta(lineMeta);

            LineGeo topLineGeo = new LineGeo();
            topLineGeo.FirstPoint = rightArc.EndPos;
            topLineGeo.SecondPoint = rightArc.EndPos - dir * Thick;
            topLineGeo.SetDrawMeta(lineMeta);

            this.Add(leftArc);
            this.Add(bottomLineGeo);
            this.Add(rightArc);
            this.Add(topLineGeo);

            ShapeMeta pathMeta = new ShapeMeta() { ForeColor = Color.Black,IsFill=true, BackColor = this.BackColor, LineWidth = 0 };

            PathGeo path = new PathGeo();
            path.AddShape(bottomLineGeo, true);
            path.AddShape(rightArc, true);
            path.AddShape(topLineGeo, true);
            path.AddShape(leftArc, true);
            path.SetDrawMeta(pathMeta);
            this.Add(path);
        } 
    }
    class NegtiveLens : LensObject
    {
        public float Thick = 2;
        public float RRight = 50;
        public float RLeft = 50;
        public float Height = 40; 
        public float Angle = 0;
        public NegtiveLens() : this("NBK7")
        {

        }
 
        public NegtiveLens(string name = "NBK7")
        {
            Name = name;
            left_vertex = new PointGeo(190, 0);
            InitElement();
        }
        public NegtiveLens(float thick, float rRight, float rLeft, float height, PointGeo leftVertex, float angle, string name = "NBK7")
        {
            Name = name;
            Thick = thick;
            RRight = rRight;
            RLeft = rLeft;
            Height = height;
            left_vertex = leftVertex;
            Angle = angle;
            InitElement();
        }
        public override void SetColor(Color FilledColor)
        {
            for (int i = 0; i < this.GetElements().Count; i++)
            {
                DrawableObject drawableObject = this.GetElements()[i];
                if (drawableObject is PathGeo)
                {
                    drawableObject.GetDrawMeta().BackColor = FilledColor;
                }
            }
        }
        public override void InitElement()
        {
            foreach (var item in this.elements)
            {
                item.IsDisposed = true;
            }
            elements.Clear();
            LenType = LEN_TYPE.NEGETIVE;
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.Black, BackColor = BackColor, LineWidth = 3 };

            float leftDgree = (float)(Math.Asin(Height / 2 / RLeft) / Math.PI * 180);

            ArcGeo leftArc = new ArcGeo();
            leftArc.StartAngle = (-leftDgree) + Angle;
            leftArc.EndAngle = (leftDgree) + Angle;
            PointGeo dir = new PointGeo(1, 0);
            dir.RotateAroundOrigin(Angle, LeftVertex);
            PointGeo centerOfLeftArc = LeftVertex - dir * RLeft;
            leftArc.CenterX = centerOfLeftArc.x;
            leftArc.CenterY = centerOfLeftArc.y;
            leftArc.Radius = RLeft;
            leftArc.SetDrawMeta(lineMeta);

            float leftMargin = (float)(RLeft - Math.Sqrt(RLeft * RLeft - Height * Height / 4));
            float rightMargin = (float)(RRight - Math.Sqrt(RRight * RRight - Height * Height / 4));

            LineGeo bottomLineGeo = new LineGeo();
            bottomLineGeo.FirstPoint = leftArc.EndPos;
            bottomLineGeo.SecondPoint = leftArc.EndPos + dir * (Thick + leftMargin + rightMargin);
            bottomLineGeo.SetDrawMeta(lineMeta);

            float rightDgree = (float)(Math.Asin(Height / 2 / RRight) / Math.PI * 180);

            ArcGeo rightArc = new ArcGeo();
            rightArc.StartAngle = (180 - rightDgree) + Angle;
            rightArc.EndAngle = (180 + rightDgree) + Angle;

            PointGeo RightVertex = LeftVertex + dir * (leftMargin + Thick + rightMargin);
            this.Center = (RightVertex + LeftVertex) * 0.5f;
            PointGeo centerOfrightArc = LeftVertex + dir * (Thick + RRight);
            rightArc.CenterX = centerOfrightArc.x;
            rightArc.CenterY = centerOfrightArc.y;
            rightArc.Radius = RRight;
            rightArc.SetDrawMeta(lineMeta);

            LineGeo topLineGeo = new LineGeo();
            topLineGeo.FirstPoint = rightArc.EndPos;
            topLineGeo.SecondPoint = rightArc.EndPos - dir * (Thick + leftMargin + rightMargin);
            topLineGeo.SetDrawMeta(lineMeta);

            this.Add(leftArc);
            this.Add(bottomLineGeo);
            this.Add(rightArc);
            this.Add(topLineGeo);
            ShapeMeta pathMeta = new ShapeMeta() { ForeColor = Color.Black, IsFill = true, BackColor = Color.Aqua, LineWidth = 0 };
            PathGeo path = new PathGeo();
            path.SetDrawMeta(pathMeta);
            path.AddShape(bottomLineGeo, true);
            path.AddShape(rightArc, true);
            path.AddShape(topLineGeo, true);
            path.AddShape(leftArc, true);
            this.Add(path);

        }
    }
    class ArcLens : LensObject
    {
        public float Thick = 2;
        public float RRight = -50;
        public float RLeft = 50;
        public float Height = 40; 
        public float Angle = 0;
        public ArcLens(string name = "NBK7")
        {
            Name = name;
            left_vertex = new PointGeo(190, 0);
            InitElement();
        }
        
        public ArcLens(float thick, float rRight, float rLeft, float height, PointGeo leftVertex, float angle, string name = "NBK7")
        {
            Name = name;
            Thick = thick;
            RRight = rRight;
            RLeft = rLeft;
            Height = height;
            left_vertex = leftVertex;
            Angle = angle;
            InitElement();
        }

        public override void InitElement()
        {
            foreach (var item in this.elements)
            {
                item.IsDisposed = true;
            }
            elements.Clear();
            LenType = LEN_TYPE.ARC;
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.Black, BackColor = Color.Black, LineWidth = 3 };

            float leftDgree = (float)(Math.Asin(Height / 2 / (-RLeft)) / Math.PI * 180);

            ArcGeo leftArc = new ArcGeo();
            leftArc.StartAngle = ( -leftDgree) + Angle;
            leftArc.EndAngle = ( leftDgree) + Angle; 
            if (-RLeft<0)
            {
                leftArc.EndAngle  = (180-leftDgree) + Angle;
                leftArc.StartAngle= (180+leftDgree) + Angle;
            }
            PointGeo dir = new PointGeo(1, 0);
            dir.RotateAroundOrigin(Angle, LeftVertex);
            PointGeo centerOfLeftArc = LeftVertex - dir * (-RLeft);
            leftArc.CenterX = centerOfLeftArc.x;
            leftArc.CenterY = centerOfLeftArc.y;
            leftArc.Radius = Math.Abs(-RLeft);
            leftArc.SetDrawMeta(lineMeta);

            float leftMargin = (float)(Math.Abs(RLeft) - Math.Sqrt((-RLeft) * (-RLeft) - Height * Height / 4));
            float rightMargin = (float)(Math.Abs(RRight) - Math.Sqrt(RRight * RRight - Height * Height / 4));

            if (RRight < 0)
            {
                rightMargin = -rightMargin;
            }
            if (RLeft>0)
            {
                leftMargin = -leftMargin;
            }
            LineGeo bottomLineGeo = new LineGeo();
            if (RLeft>0)
            {
                bottomLineGeo.SecondPoint = leftArc.StartPos;
                bottomLineGeo.FirstPoint = leftArc.StartPos + dir * (Thick + leftMargin + rightMargin);
            }else
            {
                bottomLineGeo.FirstPoint = leftArc.EndPos;
                bottomLineGeo.SecondPoint = leftArc.EndPos + dir * (Thick + leftMargin+ rightMargin);

            }
            bottomLineGeo.SetDrawMeta(lineMeta);

            float rightDgree = (float)(Math.Asin(Height / 2 /  (RRight)) / Math.PI * 180);

            ArcGeo rightArc = new ArcGeo();
            rightArc.StartAngle = (180-rightDgree) + Angle;
            rightArc.EndAngle = (180+rightDgree) + Angle;
            if (RRight < 0)
            {
                rightArc.StartAngle = (rightDgree) + Angle;
                rightArc.EndAngle = (-rightDgree) + Angle;
            }
            PointGeo RightVertex = LeftVertex + dir * (leftMargin + Thick + rightMargin);
            this.Center = (RightVertex + LeftVertex) * 0.5f;
            PointGeo centerOfrightArc = LeftVertex + dir * (   Thick +RRight  );
            rightArc.CenterX = centerOfrightArc.x;
            rightArc.CenterY = centerOfrightArc.y;
            rightArc.Radius = Math.Abs(RRight);
            rightArc.SetDrawMeta(lineMeta);
    
            LineGeo topLineGeo = new LineGeo();
            if (RRight<0)
            {
                topLineGeo.SecondPoint = rightArc.StartPos - dir * (Thick + leftMargin + rightMargin);
                topLineGeo.FirstPoint = rightArc.StartPos;
            }else
            {
                topLineGeo.SecondPoint = rightArc.EndPos;
                topLineGeo.FirstPoint = rightArc.EndPos - dir * (Thick + leftMargin + rightMargin);  
            }
            topLineGeo.SetDrawMeta(lineMeta);

            this.Add(leftArc);
            this.Add(bottomLineGeo);
            this.Add(rightArc);
            this.Add(topLineGeo);
            ShapeMeta pathMeta = new ShapeMeta() { ForeColor = Color.Black, IsFill = true, BackColor = Color.Aqua, LineWidth = 0 };
            PathGeo path = new PathGeo();
            if (RRight<0)
            {
                path.SetDrawMeta(pathMeta);
                path.AddShape(bottomLineGeo);
                path.AddShape(rightArc);
                path.AddShape(topLineGeo);
                path.AddShape(leftArc);
            }else
            {
                path.SetDrawMeta(pathMeta);
                path.AddShape(bottomLineGeo, true);
                path.AddShape(rightArc, true);
                path.AddShape(topLineGeo, true);
                path.AddShape(leftArc, true);
            }
           
            this.Add(path);

        }
    }

    class MirrorObject : LensObject
    {
        public MirrorObject():this(new PointGeo(0,0),new PointGeo(100,100),"Mirror")
        {

        }
        public MirrorObject(PointGeo startPos, PointGeo endPos,string name ) : base(startPos, endPos,name)
        {

        }

        public override void InitElement()
        {
            foreach (var item in this.elements)
            {
                item.IsDisposed = true;
            }
            elements.Clear();
            LenType = LEN_TYPE.MIRROR;
            ShapeMeta lineMeta = new ShapeMeta() { ForeColor = Color.Black, BackColor = BackColor, LineWidth = 3 };

            LineGeo lineGeo = new LineGeo();
            lineGeo.FirstPoint = this.StartPos;
            lineGeo.SecondPoint = this.EndPos;
            lineGeo.SetDrawMeta(lineMeta);
            this.Add(lineGeo);
        }
        public override PointGeo Normal { 
            get
            {
                PointGeo direction = EndPos - StartPos;
                // rotate the direction vector 90 degrees counterclockwise
                PointGeo normal = new PointGeo(-direction.Y, direction.X);
                normal.Normalized();
                return normal;
            }
        }
        public override PointGeo GetNewDir(float index0, float index1, PointGeo dir_ori, PointGeo normal)
        { 
            if (dir_ori.Dot(normal) < 0)
            {
                normal = normal * -1;
            }
            PointGeo newDir = new PointGeo();

            //float pvalue =-2*(normal.Dot(dir_ori));
            //newDir = dir_ori + pvalue * normal;
            newDir =  dir_ori*(float)index0-2*normal*((float)(normal.Dot(dir_ori)));
            newDir = newDir * (float)(1.0f / index1);
            //A'=A+p N 
            //p = n'cosI'-ncosI

            newDir.Normalized();
            return newDir;
         
    }
    }
    public class RayLight : MainCharacter
    {
        private PointGeo ori;
        public PointGeo Ori
        {
            get { return this.ori; }
            set
            {
                this.ori = value; 
                stopInteractive = false;
                Reset();
            }
        }
        private PointGeo dir;

        public PointGeo Dir
        {
            get { return this.dir; }
            set
            {
                this.dir = value;  
                stopInteractive = false;
            }
        }

        private bool selected = false;
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                foreach (var item in elements)
                {
                    if (selected)
                    {
                        item.GetDrawMeta().DashLineStyle = new float[] {2f, 2f };
                    }
                    else
                    {
                        item.GetDrawMeta().DashLineStyle = null;
                    }
                }

            }
        }

        public void Reset()
        {
            tempPoints.Clear();
            this.curDir = dir.Clone();
            this.curPos = ori.Clone();
            CurIndex = 1;
            curTValue = -1; 
            stopInteractive = false;
            IsInLens = false;
            IsInteractWithLens = false; 
            tempPoints.Add(curPos);
        }
        double wave_length = 650;
        public double WaveLength { get { return wave_length; }
            set {
                wave_length = value;
                foreach (var item in elements)
                { 
                   item.GetDrawMeta().ForeColor = WavelengthToColor(wave_length);  
                }
            } }
        /// <summary>
        /// 创建一个射线
        /// </summary>
        /// <param name="ori">光源位置</param>
        /// <param name="degree">光源与+X夹角</param>
        /// <param name="waveLenght">波长nm</param>
        public RayLight(PointGeo ori, double degree, double waveLenght)
        {
            WaveLength = waveLenght;
            Color lightColor = WavelengthToColor(WaveLength);
            ShapeMeta meta = new ShapeMeta() { ForeColor = lightColor, BackColor = Color.White, LineWidth = 1 };
            InitData(ori, new PointGeo((float)Math.Cos(degree / 180 * Math.PI), (float)Math.Sin(degree / 180 * Math.PI)), meta);
        }
        public RayLight():this(new PointGeo(0,0),0,650)
        {
            
        }

        public enum RAY_COLOR
        {
            RED,ORANGE,YELLOW,GREEN,BLUE,INDIGO,VIOLET
        }
        public static RayLight CreatRayByColor(PointGeo ori, double degree, RAY_COLOR rayColor)
        {
            double wave_length = 400;
            switch (rayColor)
            {
                case RAY_COLOR.RED:
                    wave_length = 665;
                    break;
                case RAY_COLOR.ORANGE:
                    wave_length = 630;  
                    break;
                case RAY_COLOR.YELLOW:
                    wave_length = 600;
                    break;
                case RAY_COLOR.GREEN:
                    wave_length = 550;
                    break;
                case RAY_COLOR.BLUE:
                    wave_length = 470;
                    break;
                case RAY_COLOR.INDIGO:
                    wave_length = 425;
                    break;
                case RAY_COLOR.VIOLET:
                    wave_length = 400;
                    break;
                default:
                    break;
            }
            return new RayLight(ori, degree, wave_length);
        }
        private void InitData(PointGeo ori, PointGeo dir, ShapeMeta meta,float lightWidth=2)
        {
            this.IsAppliedGravity = false;
            this.AvoidFallingDown = false;
            tempPoints.Clear();
            this.ori = ori;
            this.dir = dir;
            this.curDir = dir.Clone();
            this.curPos = ori.Clone();
            CurIndex = 1;
            curTValue = -1;
            this.Trajectory = new RandomLines();
            Trajectory.IsHold = false;
            tempPoints.Add(curPos);
            //Trajectory.AddPoint(curPos);
            meta.LineWidth = lightWidth;
            Trajectory.SetDrawMeta(meta);
            this.elements.Clear();
            this.Add(Trajectory);
        }
        // 定义一个函数，接受以纳米为单位的波长（lambda）作为参数，并返回N-BK7玻璃的折射率（n）
        public static float RefractiveIndexFromName(double lambda,string name= "NBK7")
        {
            lambda = lambda / 1000;
            // 使用分数函数公式
            float n = 1;
            // 返回折射率
            if (name== "NBK7")
            {
                n = (float)(1.03961212 + 0.00600069867 / (Math.Pow(lambda, 2) - 0.0139332363) + 0.231792344 / (Math.Pow(lambda, 2) - 0.0200179144) + 1.01046945 / (Math.Pow(lambda, 2) - 103.560653));
            }
            else if (name=="Mirror")
            {
                n = -n;
            }
            return n;
        }
        // 定义一个函数，接受以纳米为单位的波长（lambda）作为参数，并返回一个RGB颜色值
        public static Color WavelengthToColor(double lambda)
        {
            // 定义一些变量来存储红绿蓝分量
            double r, g, b;
            // 根据波长范围来计算红绿蓝分量
            if (lambda >= 380 && lambda < 440)
            {
                r = -(lambda - 440) / (440 - 380);
                g = 0.0;
                b = 1.0;
            }
            else if (lambda >= 440 && lambda < 490)
            {
                r = 0.0;
                g = (lambda - 440) / (490 - 440);
                b = 1.0;
            }
            else if (lambda >= 490 && lambda < 510)
            {
                r = 0.0;
                g = 1.0;
                b = -(lambda - 510) / (510 - 490);
            }
            else if (lambda >= 510 && lambda < 580)
            {
                r = (lambda - 510) / (580 - 510);
                g = 1.0;
                b = 0.0;
            }
            else if (lambda >= 580 && lambda < 645)
            {
                r = 1.0;
                g = -(lambda - 645) / (645 - 580);
                b = 0.0;
            }
            else if (lambda >= 645 && lambda <= 780)
            {
                r = 1.0;
                g = 0.0;
                b = 0.0;
            }
            else
            {
                r = 0.0;
                g = 0.0;
                b = 0.0;
            }
            // 考虑到人眼对不同波长的光的敏感度，调整亮度
            double s;
            if (lambda > 700)
            {
                s = 0.3 + 0.7 * (780 - lambda) / (780 - 700);
            }
            else if (lambda < 420)
            {
                s = 0.3 + 0.7 * (lambda - 380) / (420 - 380);
            }
            else
            {
                s = 1.0;
            }
            // 将红绿蓝分量转换为0到255的整数值
            int ir = (int)(s * r * 255);
            int ig = (int)(s * g * 255);
            int ib = (int)(s * b * 255);
            // 将红绿蓝分量组合成一个RGB颜色值
            int color = (ir << 16) | (ig << 8) | ib;
            // 返回颜色值
            Color colorofWave =  Color.FromArgb(255,ir, ig,ib);
            return colorofWave;
        }

        public RayLight(PointGeo ori, PointGeo dir, double waveLenght)
        {
            WaveLength = waveLenght;
            Color lightColor = WavelengthToColor(WaveLength);
            ShapeMeta meta = new ShapeMeta() { ForeColor = lightColor, BackColor = Color.White, LineWidth = 1 };
            InitData(ori,dir,meta);
        }
        public float CurIndex;
        private PointGeo curPos;
        public PointGeo curDir;
        private float curTValue;
        private PointGeo nextPos;
 
        private PointGeo nextDir;
        RandomLines Trajectory;
        List<PointGeo> tempPoints = new List<PointGeo>();
        private bool stopInteractive = false;
        private bool IsInLens = false;
        private bool IsInteractWithLens = false;
        public static bool RaySegmentIntersection(PointGeo rayOrigin, PointGeo rayDirection, PointGeo point1, PointGeo point2, out PointGeo intersection,out float t1)
        {
            var v1 = rayOrigin - point1;
            var v2 = point2 - point1;
            var v3 = new PointGeo(-rayDirection.Y, rayDirection.X);

            var dot = v2 .Dot(v3);
            if (Math.Abs(dot) < 0.000001)
            {
                t1 = -1;
                intersection = PointGeo.zero;
                return false;
            }

            t1 =  v2.Cross( v1) / dot;
            var t2 = v1 .Dot( v3) / dot;
            //if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
            if (t1 > 0.0 && (t2 > 0.0 && t2 < 1.0))
            {
                intersection = rayOrigin + t1 * rayDirection;
                return true;
            }

            intersection = PointGeo.zero;
            return false;
        }


        bool visited = false;
        Stopwatch timeWatcher = new Stopwatch();
        public void InteractWithMirror(LensObject mirror)
        {
            if (curTValue<0)
            {
                curTValue = 0;
            }
            PointGeo interP=new PointGeo();
            float tempT=0;
            float mirrorIndex = RefractiveIndexFromName(this.WaveLength, mirror.Name);
            double minTol = 1E-2;
            List<DrawableObject> linesAndArcs = mirror.GetElements();
            foreach (var item in linesAndArcs)
            {
                bool intersect = false;
                if (item is LineGeo)
                {
                    LineGeo line = item as LineGeo;
                    intersect = RaySegmentIntersection(this.curPos, this.curDir, line.FirstPoint, line.SecondPoint, out interP, out tempT);
                  
                }else if (item is ArcGeo)//
                {
                    if (visited==true)
                    {
                        //continue;
                    }
                    visited = true;
                    ArcGeo arc = item as ArcGeo;
                    Ray2 ray = new Ray2(new Vector2(this.curPos.x, this.curPos.y), new Vector2(this.curPos.x+this.curDir.x,this.curPos.y+this.curDir.y));
                    Arc2 arc2 = new Arc2(new Vector2(arc.CenterX, arc.CenterY), arc.Radius, arc.StartAngle/180*Math.PI, arc.EndAngle / 180 * Math.PI);

                    // Call the method and print the result
                    Vector2? outPoint;
                    double t_t=0;
                    intersect = Ray2.RayArcIntersect(ray, arc2, out outPoint,out t_t);
                    if (intersect)
                    {
                        interP.X = (float)outPoint.Value.x;
                        interP.Y = (float)outPoint.Value.y;
                        tempT = (float)t_t;
                    }
                   
                }

                if (intersect)
                {
                    bool setNextState = false;
                    if (curTValue > minTol && tempT < curTValue && tempT > minTol)
                    {
                        curTValue = tempT;
                        setNextState = true;
                    }
                    if (curTValue <= 0 && tempT > minTol)
                    {
                        curTValue = tempT;
                        setNextState = true;
                    }
                    PointGeo normal = new PointGeo();
                    if (item is LineGeo)
                    {
                        LineGeo line = item as LineGeo;
                        normal = line.SecondPoint - line.FirstPoint;
                        normal.Normalized();
                        
                    }else if(item is ArcGeo)
                    {
                        ArcGeo arcGeo = item as ArcGeo;
                        normal = interP - new PointGeo(arcGeo.CenterX,arcGeo.CenterY);
                        normal.Normalized();
                    }
                    if (setNextState)
                    {
                        nextPos = interP;
                        float index1 = 1;
                        float index2 = 1;
                        if (IsInLens)
                        {
                            index1 = mirrorIndex;
                        }else
                        {
                            index2 = mirrorIndex;
                        }
                        CurIndex = index1;
                        nextDir = mirror.GetNewDir(index1, index2, curDir, normal);
                        //nextIndex = mirrorIndex;
                        if (mirror is MirrorObject)
                        {
                            IsInteractWithLens = false; 
                        }else
                        {
                            IsInteractWithLens = true;
                        }
                    }
                }
            }
          
        }
        public double DistanceToOri = -1;
        public event Action<double> FinishTrack; 
        public override void SetStatus()
        { 
            timeWatcher.Stop();
            //Debug.Print(timeWatcher.ElapsedTicks + "");
            //Debug.Print("ray setStatus "+timeWatcher.ElapsedMilliseconds + "");
            if (stopInteractive)
            {
                return;
            }
            if (curTValue<0)//haven't check interfer yet
            {
                return;
            }
            if (curTValue>0.1)
            {
                curPos = nextPos;
                curDir = nextDir; 
                //Trajectory.AddPoint(curPos);
                tempPoints.Add(curPos.Clone());
                if (IsInteractWithLens)
                {
                    IsInLens = !IsInLens;
                }
            }else
            {
                double distance_from_ori = -1;
                if (curDir.x!=float.NaN)
                {
                    //-y0 * u / v + x0
                    distance_from_ori = -(curDir.x / curDir.y) * curPos.y + curPos.x;
                    if (distance_from_ori<curPos.x)
                    {
                        distance_from_ori = -1;
                    }
                    curPos = curPos + curDir * 300; 
                    //Trajectory.AddPoint(curPos);
                    tempPoints.Add(curPos.Clone());
                    Trajectory.Clear();
                    Trajectory.AddPoints(Utils.MyUtils.Clone<PointGeo>(tempPoints));
                    tempPoints.Clear();
                }
                DistanceToOri = distance_from_ori;
                stopInteractive = true; 
                FinishTrack?.Invoke(distance_from_ori);
            } 
            curTValue = 0;
            IsInteractWithLens = false;
            //base.SetStatus();

            timeWatcher.Reset();
            timeWatcher.Start();
        }
        public override bool CheckCollision(SceneObject sceneObject)
        {
            if (stopInteractive)
            {
                return false;
            }
            switch (sceneObject.OBJECT_TYPE)
            {
                case SCENE_OBJECT_TYPE.ROLE:
                    break;
                case SCENE_OBJECT_TYPE.GROUND:
                    break;
                case SCENE_OBJECT_TYPE.PICTURE:
                    break;
                case SCENE_OBJECT_TYPE.OBSTACLE:
                    if (sceneObject is LensObject)
                    {
                        LensObject mirror = sceneObject as LensObject;
                        InteractWithMirror(mirror);
                    } 
                    break;
                case SCENE_OBJECT_TYPE.WEAPON:
                    break;
                case SCENE_OBJECT_TYPE.EFFECT:
                    break;
                case SCENE_OBJECT_TYPE.RAY_BEAM:
                    break;
                case SCENE_OBJECT_TYPE.MIRROR:

                    break;
                case SCENE_OBJECT_TYPE.LENS:
                    break;
                default:
                    break;
            }

            return true;
        }
    }
}
#if false
There are different ways to check whether a 2D ray and a 2D line segment intersect, and how to get the intersection point if they do. One common method is to use vector algebra and parametric equations to represent the ray and the line segment, and then solve for the parameters that make them equal. Here is a possible algorithm:

-Let the ray start from point P (x, y) and have direction vector r (cos θ, sin θ), where θ is the angle from the horizontal. Then any point on the ray can be written as P + t r, where t is a scalar parameter that is greater than or equal to zero.
- Let the line segment have endpoints Q (x1, y1) and Q + s (x2, y2), where s(x2 - x1, y2 - y1) is the vector from Q to Q + s.Then any point on the line segment can be written as Q + u s, where u is a scalar parameter that is between zero and one.
- The ray and the line segment intersect if there exist values of t and u such that P + t r = Q + u s. This is equivalent to solving the following system of equations:

```
x + t cos θ = x1 + u(x2 - x1)
y + t sin θ = y1 + u (y2 - y1)
```

-To solve this system, we can use the cross product of vectors, which is defined as follows:

```
a × b = a.x * b.y - a.y * b.x
```

- We can rewrite the system of equations as:

```
(x1 - x) + u (x2 - x1) = t cos θ
(y1 - y) + u (y2 - y1) = t sin θ
```

- And then cross both sides with r:

```
(x1 - x) × r + u (x2 - x1) × r = t cos θ × r
(y1 - y) × r + u (y2 - y1) × r = t sin θ × r
```

- Since r × r = 0, we can simplify this to:

```
(x1 - x) × r + u (x2 - x1) × r = 0
(y1 - y) × r + u (y2 - y1) × r = 0
```

- And then solve for u:

```
u = -(x1 - x) × r / (x2 - x1) × r
u = -(y1 - y) × r / (y2 - y1) × r
```

- If these two values of u are equal and between 0 and 1, then we have a valid intersection. We can then plug u into Q + u s to get the intersection point.If they are not equal or not between 0 and 1, then there is no intersection.
-Alternatively, we can solve for t:

```
t = (x1 - x) / cos θ + u(x2 - x1) / cos θ
t = (y1 - y) / sin θ + u(y2 - y1) / sin θ
```


-If these two values of t are equal and greater than or equal to 0, then we have a valid intersection.We can then plug t into P + t r to get the intersection point.If they are not equal or not greater than or equal to 0, then there is no intersection.


Here is an example of C# code that implements this algorithm²:

```csharp
/// <summary>
/// Returns the distance from the ray origin to the intersection point or null if there is no intersection.
/// </summary>
public double? GetRayToLineSegmentIntersection(Point rayOrigin, Vector rayDirection, Point point1, Point point2)
    {
        var v1 = rayOrigin - point1;
        var v2 = point2 - point1;
        var v3 = new Vector(-rayDirection.Y, rayDirection.X);

        var dot = v2 * v3;
        if (Math.Abs(dot) < 0.000001)
            return null;

        var t1 = Vector.CrossProduct(v2, v1) / dot;
        var t2 = (v1 * v3) / dot;

        if (t1 >= 0.0 && (t2 >= 0.0 && t2 <= 1.0))
            return t1;

        return null;
    }
```

You can also find other algorithms that use different approaches, such as solving for the intersection of two lines and then checking if the point is on the ray and the line segment³⁴, or using a determinant formula to find the parameters of the ray and the line segment¹. You may want to compare the performance and accuracy of different algorithms for your specific use case.

I hope this helps you with your problem. 😊

源: 与必应的对话， 2023 / 6 / 21
(1) c# - 2D Segment and Infinite Line Intersection Algorithm - Stack Overflow. https://stackoverflow.com/questions/70755385/2d-segment-and-infinite-line-intersection-algorithm.
(2) How do you detect where two line segments intersect?. https://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect.
(3) c# - Algorithm for intersection of 2 lines? - Stack Overflow. https://stackoverflow.com/questions/4543506/algorithm-for-intersection-of-2-lines.
(4) How do you check for intersection between a line segment and a line ray....https://stackoverflow.com/questions/14307158/how-do-you-check-for-intersection-between-a-line-segment-and-a-line-ray-emanatin.

#endif