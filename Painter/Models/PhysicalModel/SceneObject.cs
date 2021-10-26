using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.Paint
{
    public enum SCENE_OBJECT_TYPE { CHARACTER,GROUND,PICTURE};
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

        public void Move(PointGeo pos)
        {
            Pos = pos;
            foreach (var item in elements)
            {
                item.Translate(pos);
            }
        }
    }
    class Character:SceneObject
    {
        public int LifeLength = 100;
        public int Attack = 20;
        public int Level = 1;
        public PointGeo Speed = new PointGeo();
        public PointGeo Acc = new PointGeo();
        public PointGeo Force = new PointGeo(); 
        public float Mass = 1000;
        public bool IsInGround = false;
        public Character()
        { 
            OBJECT_TYPE = SCENE_OBJECT_TYPE.CHARACTER; 
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
       

        public void OnAttack()
        {

        }
    }

}
