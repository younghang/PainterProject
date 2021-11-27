using Painter.Models;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Painter.Models.GeoModel
{
    public class Vertex 
    {
        public float X;

        public float Y;
        public float Z;

        public bool IsNeedDraw = true;
        public Vertex(float x, float y,float z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        public Vertex()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public static Vertex operator +(Vertex p1, Vertex p2)
        {
            return new Vertex(p1.X + p2.X, p1.Y + p2.Y,p1.Z +p2.Z);
        }
        public static Vertex operator -(Vertex p1, Vertex p2)
        {
            return new Vertex(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);
        }
        public static Vertex operator *(Vertex p1, float num)
        {
            return new Vertex(p1.X * num, p1.Y * num,p1.Z*num);
        }
        public static Vertex operator *(float num, Vertex p1)
        {
            return new Vertex(p1.X * num, p1.Y * num, p1.Z * num);
        }
        public static Vertex operator /(Vertex p1, float num)
        {
            return new Vertex(p1.X / num, p1.Y / num,p1.Z/num);
        }
        public static Vertex operator /(float num, Vertex p1)
        {
            return new Vertex(num / p1.X, num / p1.Y,num/p1.Z);
        }
        public Vertex Clone()
        {
            return new Vertex(this.X, this.Y,this.Z);
        }
        public float Dot(Vertex p)
        {
            return this.X * p.X + this.Y * p.Y+this.Z*p.Z;
        }
        public Vertex Cross(Vertex p)
        {
            return new Vertex(this.Y*p.Z-this.Z*p.Y,-this.X*p.Z+this.Z*p.X,this.X*p.Y-this.Y*p.X);
        }
        public Vertex Offset(Vertex p)
        {
            this.X += p.X;
            this.Y += p.Y;
            this.Z += p.Z;
            return this;
        }
        public Vertex(Vertex p)
        {
            this.X = p.X;
            this.Y = p.Y;
            this.Z = p.Z;
        }
        public Vertex Normalize()
        {
            float s = GetLength();
            this.X = X / s;
            this.Y = Y / s;
            this.Z = Z / s;
            return this;
        }
        public void Scale(Vertex p)
        {
            this.X *= p.X;
            this.Y *= p.Y;
            this.Z *= p.Z;
        }
        public float GetLength()
        {
            return (float)Math.Sqrt(this.X * this.X + this.Y * this.Y+this.Z*this.Z);
        }
        public Vertex Abs()
        {
            return new Vertex(Math.Abs(X), Math.Abs(Y),Math.Abs(Z));
        }
        public override string ToString()
        {
            return "(" + this.X.ToString("f3") + ", " + this.Y.ToString("f3") + ", " + this.Z.ToString("f3")+ ")";
        }

        public void Rotate(float degree, Vertex point,Vertex direction)
        {
            //这里没有实现，先写一个简单的绕Y轴的旋转
            PointGeo pointGeo = new PointGeo(this.X, this.Z);
            pointGeo.RotateAroundOrigin(degree,new PointGeo());
            this.X = pointGeo.X;
            this.Z = pointGeo.Y;
        }
    }
    public class Edge
    {
        public Vertex StartVertex = new Vertex();
        public Vertex EndVertex = new Vertex();
    }
    
    public class Surface
    {
        public HSLColor FaceColor = new HSLColor();
        public Surface()
        {
            FaceColor.Hue = 150;
            FaceColor.Saturation = 80;
            FaceColor.Lightness = 80;
        }
        public void Move(Vertex v)
        {
            foreach (var item in this.Vertices)
            {
                item.Offset(v);
            }
        }
        public Vertex GetNormalizeVertex()
        {
            Vertex vertex1 = Vertices[1] - Vertices[0];
            Vertex vertex2 = Vertices[2] - Vertices[0];
            return vertex1.Cross(vertex2).Normalize();
        }
        public Vertex[] Vertices = new Vertex[3];
        private Vertex NormalizeVertex = new Vertex();

        public void Rotate(float degree, Vertex point,Vertex direction)
        {
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vertex v = Vertices[i];
                v.Rotate(degree, point,direction);
            }
        }
    }
    public class Object3D
    {
        public List<Surface> surfaces = new List<Surface>();

        public void Rotate(float degree, Vertex point, Vertex direction)
        {
            foreach (var item in this.surfaces)
            {
                item.Rotate(degree, point, direction);
            }
        }
        public void Move(Vertex v)
        {
            foreach (var item in this.surfaces)
            {
                item.Move(v);
            }
        }
        public void AddSurface(Surface s)
        {
            if (!surfaces.Contains(s))
            {
                surfaces.Add(s);
            }
        }
    }
    public class Light
    {
        public float IlluminateStrength = 1;
    }
    public class Camera3D
    {
        float angleOfView = 60;//以高度为视觉基准
        float viewHeight = 200;
        float viewWidth = 200;
        float zFar = -5000;
        float zNear = 500;

        public Vertex EyeToObjectVertex = new Vertex(0, 0, 1);

        public PointGeo ObjectToPlane(Vertex vertex)
        {
            double centerZ = zNear + viewHeight / Math.Tan(angleOfView / Math.PI);
            PointGeo p = new PointGeo();
            p.Y = (float)(vertex.Y / (centerZ - vertex.Z) * (centerZ - zNear));
            p.X = (float)(vertex.X / (centerZ - vertex.Z) * (centerZ - zNear));
            return p; 
        }
    }
    public class Render3D
    {

        CanvasModel render2D;
        Scene3D curScene;
        Camera3D camera;

        public Render3D(CanvasModel render2D, Scene3D curScene, Camera3D camera)
        {
            this.render2D = render2D;
            this.curScene = curScene;
            this.camera = camera;
        }

        public void Render()
        { 
            foreach (var item in curScene.GetObjects())
            {
                foreach (var surface in item.surfaces)
                {
                    RenderSurface(surface);
                }
            }
        }
        private void RenderSurface(Surface surface)
        {
            List<PointGeo> ps = new List<PointGeo>();
            for (int i = 0; i < surface.Vertices.Length; i++)
            {
                ps.Add(camera.ObjectToPlane(surface.Vertices[i]));
            }
            float LightnessRatio = surface.GetNormalizeVertex().Dot(camera.EyeToObjectVertex);
            if (LightnessRatio<0)
            {
                LightnessRatio = -LightnessRatio;
            }
            HSLColor tempColor = surface.FaceColor.Clone();
            tempColor.Lightness = (int) (surface.FaceColor.Lightness * LightnessRatio);
            if (tempColor.Lightness<=0)
            {
                tempColor.Lightness = 0;
            }
            if (tempColor.Lightness >=100)
            {
                tempColor.Lightness = 100;
            }
            PolygonGeo polygonGeo = new PolygonGeo();
            polygonGeo.AddPoints(ps);
            Color color = tempColor.ToColor();
            polygonGeo.SetDrawMeta(new ShapeMeta() {IsFill=true,BackColor=color,ForeColor= Color.Wheat, LineWidth=1});
            render2D.GetFreshLayerManager().Add(polygonGeo);
        }
    }
    public class Scene3D
    {
        List<Object3D> object3Ds = new List<Object3D>();

        public List<Object3D> GetObjects()
        {
            return this.object3Ds;
        }
        public void Rotate(float degree)
        {
            foreach (var item in this.object3Ds)
            {
                item.Rotate(degree,new Vertex(0,0,0), new Vertex(0, 1, 0));
            }
        }
        public Object3D GetOctahedral()
        {
            return this.object3Ds[0];
        }
        public Object3D GetCube()
        {
            return this.object3Ds[1];
        }
        public void Init()
        {
            Object3D octahedral = new Object3D();
            Surface surface = new Surface();
            surface.Vertices[0] = new Vertex(0, 0, 0);
            surface.Vertices[1] = new Vertex(100, 0, 0);
            surface.Vertices[2] = new Vertex(50,150 ,50 );

            Surface surface2 = new Surface();
            surface2.Vertices[0] = new Vertex(0, 0, 0);
            surface2.Vertices[1] = new Vertex(0, 0, 100);
            surface2.Vertices[2] = new Vertex(50, 150, 50);

            Surface surface3 = new Surface();
            surface3.Vertices[0] = new Vertex(100, 0, 0);
            surface3.Vertices[1] = new Vertex(100, 0, 100);
            surface3.Vertices[2] = new Vertex(50, 150, 50);

            Surface surface4 = new Surface();
            surface4.Vertices[0] = new Vertex(0, 0, 100);
            surface4.Vertices[1] = new Vertex(100, 0, 100);
            surface4.Vertices[2] = new Vertex(50, 150, 50);

            Surface surface5 = new Surface();
            surface5.Vertices[0] = new Vertex(0, 0, 0);
            surface5.Vertices[1] = new Vertex(100, 0, 0);
            surface5.Vertices[2] = new Vertex(50, -150, 50);

            Surface surface6 = new Surface();
            surface6.Vertices[0] = new Vertex(0, 0, 0);
            surface6.Vertices[1] = new Vertex(0, 0, 100);
            surface6.Vertices[2] = new Vertex(50, -150, 50);

            Surface surface7 = new Surface();
            surface7.Vertices[0] = new Vertex(100, 0, 0);
            surface7.Vertices[1] = new Vertex(100, 0, 100);
            surface7.Vertices[2] = new Vertex(50, -150, 50);

            Surface surface8 = new Surface();
            surface8.Vertices[0] = new Vertex(0, 0, 100);
            surface8.Vertices[1] = new Vertex(100, 0, 100);
            surface8.Vertices[2] = new Vertex(50, -150, 50);

            octahedral.surfaces.Add(surface);
            octahedral.surfaces.Add(surface2);
            octahedral.surfaces.Add(surface3);
            octahedral.surfaces.Add(surface4);
            octahedral.surfaces.Add(surface5);
            octahedral.surfaces.Add(surface6);
            octahedral.surfaces.Add(surface7);
            octahedral.surfaces.Add(surface8);

            Object3D cube = new Object3D();
            Surface s1 = new Surface();
            s1.Vertices = new Vertex[4];
            s1.Vertices[0] = new Vertex(0, 0, 0);
            s1.Vertices[1] = new Vertex(100, 0, 0);
            s1.Vertices[2] = new Vertex(100, 100, 0);
            s1.Vertices[3] = new Vertex(0, 100, 0);

            Surface s2 = new Surface();
            s2.Vertices = new Vertex[4];
            s2.Vertices[0] = new Vertex(0, 0, 100);
            s2.Vertices[1] =new Vertex(0, 100, 100);
            s2.Vertices[2] =  new Vertex(100, 100, 100);
            s2.Vertices[3] = new Vertex(100, 0, 100);

            Surface s3 = new Surface();
            s3.Vertices = new Vertex[4];
            s3.Vertices[0] = new Vertex(0, 0, 0);
            s3.Vertices[1] = new Vertex(100, 0, 0);
            s3.Vertices[2] = new Vertex(100, 0, 100);
            s3.Vertices[3] = new Vertex(0, 0, 100);

            Surface s4 = new Surface();
            s4.Vertices = new Vertex[4];
            s4.Vertices[0] = new Vertex(0, 100, 0);
            s4.Vertices[1] = new Vertex(100, 100, 0);
            s4.Vertices[2] = new Vertex(100, 100, 100);
            s4.Vertices[3] = new Vertex(0, 100, 100);

            Surface s5 = new Surface();
            s5.Vertices = new Vertex[4];
            s5.Vertices[0] = new Vertex(0, 0, 0);
            s5.Vertices[1] = new Vertex(0, 100, 0);
            s5.Vertices[2] = new Vertex(0, 100,100);
            s5.Vertices[3] = new Vertex(0, 0, 100);

            Surface s6 = new Surface();
            s6.Vertices = new Vertex[4];
            s6.Vertices[0] = new Vertex(100, 0, 0);
            s6.Vertices[1] = new Vertex(100, 100, 0);
            s6.Vertices[2] = new Vertex(100, 100, 100);
            s6.Vertices[3] = new Vertex(100, 0, 100);

            cube.surfaces.Add(s1);
            cube.surfaces.Add(s2);
            cube.surfaces.Add(s3);
            cube.surfaces.Add(s4);
            cube.surfaces.Add(s5);
            cube.surfaces.Add(s6);
            cube.Move(new Vertex(200, -50, 0));

            object3Ds.Add(octahedral);
            object3Ds.Add(cube);
        }
    }
}
