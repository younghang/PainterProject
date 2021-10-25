using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Painter.Models.Paint
{
   
    class SceneObject
    {
        public SceneObject()
        { 
        }
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
        public Character()
        {
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
