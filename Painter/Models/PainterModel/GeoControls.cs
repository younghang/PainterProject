using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Painter.Models.PainterModel
{
    
    public class GeoControl
    {
        public virtual List<IScreenPrintable> GetElements()
        {
            List<IScreenPrintable> temp = new List<IScreenPrintable>();  
            temp.Add(baseShape); 
            return temp;
        }
        protected Shape baseShape;
        public virtual void  Update()
        {

        }
        protected virtual void OnMouseHover()
        {

        }
        protected virtual void OnMouseClick()
        {

        }
        protected virtual void OnMouseLeave()
        {

        }
        public event Action ClickEvent;
        private int width;
        private int height;
        public GeoControl()
        {

        }
        public void SetBaseShape(Shape shape)
        {
            this.baseShape = shape;
        }
        public GeoControl(int width, int height,Shape shape)
        {
            this.baseShape = shape;
            if (shape is CircleGeo)
            {
                baseShape.FirstPoint = new PointGeo(width/2, height/2);
                baseShape.SecondPoint = new PointGeo(width, height/2);
            }
            else if (shape is RectangeGeo || shape is EllipseGeo)
            {
                baseShape.FirstPoint = new PointGeo(0, 0);
                baseShape.SecondPoint = new PointGeo(width, height);
            }
            else  
            {
                baseShape.FirstPoint = new PointGeo(0, 0);
                baseShape.SecondPoint = new PointGeo(width, height);
            }
            
           
            baseShape.SetDrawMeta(new ShapeMeta()
            {
                BackColor = Color.Aquamarine,
                IsFill = true
            }); 
            this.Width = width;
            this.Height = height;
        }
        public void HitTest(CircleGeo circle,bool isMouseDown)
        {
            if (this.baseShape.IsOverlap(circle))
            {
                if (isMouseDown)
                {
                    ClickEvent?.Invoke();
                    OnMouseClick();
                }
                else
                {
                    OnMouseHover(); 
                }
            } else
            {
                OnMouseLeave();
            }
        }
        private bool isVisible = true;
        public bool IsVisible
        {
            get { 
                return this.isVisible; }
            set
            {
                this.isVisible = value;
                this.baseShape.IsShow = value;
                Update();
            }
        } 
        public Color Background { 
            get { return (this.baseShape.GetDrawMeta() as ShapeMeta).BackColor; }
            set { (this.baseShape.GetDrawMeta() as ShapeMeta).BackColor = value; } 
        }
        public PointGeo Pos = new PointGeo();
         
        public virtual void Move(PointGeo point)
        {
            this.Pos += point;
            this.baseShape.Translate(point);
        }
        public int Width { get => width; set{ width = value;
                Update();
            } }
        public int Height { get => height; set { height = value;
                Update();
            } }
    }
    public class GeoLabel : GeoControl
    { 
        public string Text
        {
            get { return this.text.GetTextMeta().Text; }
            set {   this.text.GetTextMeta().Text=value;
                ReplaceText();
            }
        }
        public override void Move(PointGeo point)
        {
            base.Move(point);
            this.text.Translate(point);
        }
        protected DrawableText text = new DrawableText();
        public override void Update()
        {
            base.Update();
            ReplaceText();//挂到属性上去
            text.IsShow = IsVisible;
        }
        public GeoLabel(int width, int height, Shape shape) : base(width, height, shape)
        {
            this.text.SetDrawMeta(new TextMeta("123")
            {
                ForeColor = Color.Black,
                TEXTFONT = new Font("Arial", baseShape.Heigth /3),
                stringFormat = new StringFormat() { Alignment = StringAlignment.Near }
            });
            ReplaceText();
        }
        private void ReplaceText()
        {
            if (text.GetTextMeta()!=null)
            {
                Size size= TextRenderer.MeasureText(text.GetTextMeta().Text, text.GetTextMeta().TEXTFONT);
                text.pos = new PointGeo((this.Width - size.Width) / 2.0f, (this.Height - size.Height) / 2.0f);
            } 
        }
        public Color ForeColor
        {
            get { return (this.text.GetTextMeta()).ForeColor; }
            set { (this.text.GetTextMeta()).ForeColor = value; }
        }
        public override List<IScreenPrintable> GetElements()
        {
            List<IScreenPrintable> temp = new List<IScreenPrintable>();
            temp.Add(baseShape); 
            temp.Add(text); 
            return temp;
        }

    }
    public class GeoButton: GeoLabel
    {
        public GeoButton(int width, int height, Shape shape) : base(width, height, shape)
        {
            Text = "123";
        }
        public Color HoverColor = Color.AntiqueWhite; 
        public Color BackColor = Color.Aquamarine;
        public Color ClickColor = Color.Aqua;
        protected override void OnMouseHover()
        {
            base.OnMouseHover();
            Background = HoverColor;
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            Background = BackColor;
        }
        protected override void OnMouseClick()
        {
            base.OnMouseClick();
            Background = ClickColor;
        }
    }
    public class GeoEditBox : GeoButton
    {
        public GeoEditBox(int width, int height, Shape shape) : base(width, height, shape)
        {
            ClickColor = HoverColor;
        }
        protected override void OnMouseClick()
        {
            base.OnMouseClick();
            text.GetTextMeta().stringFormat.Alignment = StringAlignment.Near;
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                text.GetTextMeta().stringFormat.Alignment = StringAlignment.Center;
                return;
            }
            else if (e.KeyCode == Keys.Back)
            {
                Text = Text.Substring(0, Text.Length - 2); ;
            }
            else
            {
                Text += (char)e.KeyValue;
            }

        }
    }
    public enum FLOW_DIRECTION { HORIZONTAL,VERTICAL}
    public class GeoPanel : GeoControl
    {
        public GeoPanel(int width, int height, Shape shape) : base(width, height, shape)
        {
            Background = Color.Azure;

        }
        public List<GeoControl> GetControls()
        {
            return this.Controls;
        }
        private List<GeoControl> Controls = new List<GeoControl>();
        FLOW_DIRECTION flowDirection = FLOW_DIRECTION.HORIZONTAL;
        public void AddControl(GeoControl control)
        {
            if (!Controls.Contains(control))
            {
                this.Controls.Add(control);
            }
        }
        public override void Move(PointGeo point)
        {
            base.Move(point); 
            foreach (var item in this.Controls)
            {
                item.Move(point);
            }
            Layout();
        }
        private int padding = 5;
        public int Padding
        {
            get { return this.padding; }
            set {
                this.padding = value;
                Layout();
            }
        }

        public void Layout()
        {
            float length = 0;
            if (flowDirection==FLOW_DIRECTION.HORIZONTAL)
            {
                length = this.Controls[0].Width + padding;
                for (int i = 1; i < this.Controls.Count; i++)
                { 
                    this.Controls[i].Move(new PointGeo(length ,0 ));
                    length += this.Controls[i-1].Width + padding;
                }
            }else if (flowDirection == FLOW_DIRECTION.VERTICAL)
            {
                length = this.Controls[0].Height + padding;
                for (int i = 1; i < this.Controls.Count; i++)
                {
                    this.Controls[i].Move(new PointGeo(0, length));
                    length += this.Controls[i - 1].Height + padding;
                }
            }
        }
        public override void Update()
        {
            base.Update();
            foreach (var item in this.Controls)
            {
                item.IsVisible = this.IsVisible;
                item.Update();
            }
        }
        public override List<IScreenPrintable> GetElements()
        {
            List<IScreenPrintable> temp = new List<IScreenPrintable>();
            temp.Add(baseShape);
            foreach (var item in this.Controls)
            {
                temp.AddRange(item.GetElements());
            } 
            return temp;
        }
    }
}
