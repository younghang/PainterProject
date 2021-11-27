using Painter.Models.Paint;
using Painter.Painters;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Utils;

namespace Painter.Models.PainterModel
{
    
    public class GeoControl
    {
        public virtual List<DrawableObject> GetElements()
        {
            List<DrawableObject> temp = new List<DrawableObject>();  
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
            else if (shape is ArcGeo)
            {
                ArcGeo arc = baseShape as ArcGeo;
                arc.FirstPoint = new PointGeo(0, height);
                arc.SecondPoint =new PointGeo(0, 0) ;
                arc.ThirdPoint =new PointGeo(width, height) ; 
            }
            else if (shape is RectangleGeo || shape is EllipseGeo)
            {
                baseShape.FirstPoint = new PointGeo(0, 0);
                baseShape.SecondPoint = new PointGeo(width, height);
            }
            else  
            {
                baseShape.FirstPoint = new PointGeo(0, 0);
                baseShape.SecondPoint = new PointGeo(width, height);
            }

            if (baseShape.GetDrawMeta() == null )
            {
                baseShape.SetDrawMeta(new ShapeMeta()
                {
                    BackColor = Color.Aquamarine,
                    IsFill = true
                });
            } 
            this.Width = width;
            this.Height = height;
        }

        public bool HitTest(CircleGeo circle,bool isMouseDown)
        {
            if (this.baseShape.IsShow == false || this.baseShape.IsInVision == false)
            {
                return false; 
            }
            if (circle.IsOverlap(baseShape))
            {
                if (isMouseDown)
                {
                    ClickEvent?.Invoke();
                    OnMouseClick();
                    return true;
                }
                else
                {
                    OnMouseHover(); 
                }
            } else
            {
                OnMouseLeave();
            }
            return false;
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
            get {
                if (baseShape is LineGeo|| baseShape.ShapeType==Shape.SHAPE_TYPE.ARC)
                {
                    return (this.baseShape.GetDrawMeta() as ShapeMeta).ForeColor;
                }
                else
                {
                    return (this.baseShape.GetDrawMeta() as ShapeMeta).BackColor;
                } 
            }
            set {
                if (baseShape is LineGeo || baseShape.ShapeType == Shape.SHAPE_TYPE.ARC)
                {
                    (this.baseShape.GetDrawMeta() as ShapeMeta).ForeColor = value;
                }else
                {
                    (this.baseShape.GetDrawMeta() as ShapeMeta).BackColor = value;
                }
            } 
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
        public GeoLabel(int width, int height, Shape shape, DrawableObject drawable) : base(width, height, shape)
        {
            this.text.SetDrawMeta(new TextMeta("123")
            {
                ForeColor = Color.Black,
                TEXTFONT = new Font("Arial", baseShape.Heigth / 3),
                stringFormat = new StringFormat() { Alignment = StringAlignment.Near }
            });
            ReplaceText();
            this.Drawable = drawable;
        }
        public void LoadDrawableFile(string filePath)
        {
            List<DrawableObject> drawables= FileUtils.LoadDrawableFile(filePath);
            this.Drawable = drawables[0];
        }
        protected DrawableObject Drawable=null;
         
        public Color TextColor
        {
            get { return text.GetDrawMeta().ForeColor; }
            set { text.GetDrawMeta().ForeColor=value; }
        }
        //文本对齐方式，在设置Text之前设置
        public StringAlignment Alignment = StringAlignment.Center;
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
            if (this.Drawable!=null)
            {
                Drawable.Translate(point);
            }
        }
        protected DrawableText text = new DrawableText();
        public override void Update()
        {
            base.Update();
            ReplaceText();//挂到属性上去
            text.IsShow = IsVisible;
            if (Drawable!=null)
            {
                Drawable.IsShow = IsVisible;
            }
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
            if (text.GetDrawMeta()!=null)
            {
                Size size= TextRenderer.MeasureText(text.GetTextMeta().Text, text.GetTextMeta().TEXTFONT);
                if (Alignment==StringAlignment.Near)
                {
                    text.pos = this.Pos + new PointGeo(0, (this.Height - size.Height) / 2.0f);
                }else
                {
                    text.pos = this.Pos + new PointGeo((this.Width - size.Width) / 2f, (this.Height - size.Height) / 2.0f);
                }
            } 
        }
        public Color ForeColor
        {
            get { return (this.text.GetDrawMeta()).ForeColor; }
            set { (this.text.GetDrawMeta()).ForeColor = value; }
        }
        
        public override List<DrawableObject> GetElements()
        {
            List<DrawableObject> temp = base.GetElements(); 
            temp.Add(text);
            if (Drawable!=null)
            {
                temp.Add(Drawable);
            } 
            return temp;
        }

    }
    public class GeoRadio : GeoLabel
    { 
        public Color BackColor = Color.Aquamarine;
        public Color CheckedColor = Color.Aqua;
        private bool isChecked = false;
        public bool IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                if (this.isChecked)
                { 
                    if (this.Drawable != null)
                    {
                        (this.Drawable.GetDrawMeta() as ShapeMeta).BackColor = CheckedColor;
                        (this.Drawable.GetDrawMeta() as ShapeMeta).ForeColor = CheckedColor;
                    }else
                    {
                        Background = CheckedColor;
                    }
                }else
                { 
                    if (this.Drawable != null)
                    {
                        (this.Drawable.GetDrawMeta() as ShapeMeta).BackColor = BackColor;
                        (this.Drawable.GetDrawMeta() as ShapeMeta).ForeColor = BackColor;
                    }
                    else
                    {
                        Background = BackColor;
                    }
                }
            }
        }
        public GeoRadio(int width, int height, Shape shape) : base(width, height, shape)
        {
            Text = "";
        }
        public GeoRadio(int width, int height, Shape shape,DrawableObject drawable) : base(width, height, shape, drawable)
        {
            Text = "";
            Drawable = drawable;
        }
        public event Action CheckedEvent;
        protected override void OnMouseClick()
        {
            base.OnMouseClick();
            IsChecked = !IsChecked;
            if (IsChecked)
            {
                CheckedEvent?.Invoke();
            }
        }
    }
    public class GeoButton: GeoLabel
    {
        public GeoButton(int width, int height, Shape shape, DrawableObject drawable) : base(width, height, shape, drawable)
        {
            Text = "";
        }
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
    public class GeoDrawButton : GeoButton
    {
        public GeoDrawButton(int width, int height, Shape shape) : base(width, height, shape)
        {
            Text = ""; 
        }
        protected List<DrawableObject> Drawables;
        public override void Move(PointGeo point)
        {
            base.Move(point);
            if (this.Drawables != null)
            {
                foreach (var item in Drawables)
                {
                    item.Translate(point);
                }
            }
        }
        public override void Update()
        {
            base.Update();
            if (this.Drawables!=null)
            {
                foreach (var item in Drawables)
                {
                    item.IsShow = IsVisible;
                }
            }
           
        }
        public override List<DrawableObject> GetElements()
        {
            List<DrawableObject> temp = base.GetElements();
            if (this.Drawables != null)
            {
                foreach (var item in Drawables)
                {
                    temp.Add(item);
                }
            }
            return temp;
        }
        public new void LoadDrawableFile(string filePath)
        {
            List<DrawableObject> drawables = FileUtils.LoadDrawableFile(filePath);
            drawables.Reverse();
            this.Drawables = drawables;
        }

    }
    public class GeoEditBox : GeoButton
    {
        public GeoEditBox(int width, int height, Shape shape) : base(width, height, shape)
        {
            ClickColor = HoverColor;
            BackColor = Color.White;
            Background = BackColor;
            shape.GetDrawMeta().ForeColor = Color.Orange;
            shape.GetDrawMeta().LineWidth = 2;

        }
        public event Action<string> TextChangeEvent;
        private bool isEditIn = true;
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            isEditIn = false;
        }
        protected override void OnMouseClick()
        {
            isEditIn = true;
            base.OnMouseClick();
            text.GetTextMeta().stringFormat.Alignment = StringAlignment.Near;
            Text ="";
        }
        public void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                text.GetTextMeta().stringFormat.Alignment = StringAlignment.Near;
                TextChangeEvent?.Invoke(Text);
                return;
            }
            if (isEditIn==false)
            {
                return;
            }
            else if (e.KeyCode == Keys.Back)
            {
                if (Text.Length>=1)
                {
                    Text = Text.Substring(0, Text.Length - 1);  
                } 
            }
            else
            {
                if (e.KeyCode>=Keys.NumPad0 && e.KeyCode <= Keys.NumPad9
                    || e.KeyCode >= Keys.D0 && e.KeyCode <= Keys.D9
                    || e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z
                    || e.KeyCode==Keys.Subtract
                    || e.KeyCode==Keys.OemMinus
                    )
                {
                    if (e.KeyCode >= Keys.NumPad0 && e.KeyCode <= Keys.NumPad9)
                    {
                        Text += (e.KeyCode - Keys.NumPad0) ;
                    }
                    else if (e.KeyCode == Keys.Subtract || e.KeyCode == Keys.OemMinus)
                    {
                        Text += "-";
                    }
                    else
                    {
                        Text += ((char)e.KeyValue); 
                    }
                }
               
            }
            TextChangeEvent?.Invoke(Text);

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
        public FLOW_DIRECTION flowDirection = FLOW_DIRECTION.HORIZONTAL;
        public void ClearRadioSelection()
        {
            foreach (var item in this.Controls)
            {
                if (item is GeoRadio)
                {
                    GeoRadio geoRadio = item as GeoRadio;
                    geoRadio.IsChecked = false;
                }
            }
        }
        public void AddControl(GeoControl control)
        {
            if (!Controls.Contains(control))
            {
                if (control is GeoRadio)
                {
                    GeoRadio geoRadio = control as GeoRadio;
                    geoRadio.ClickEvent += GeoRadio_ClickEvent;
                }
                this.Controls.Add(control);
            } 
        }

        private void GeoRadio_ClickEvent()
        {
            foreach (var item in this.Controls)
            {
                if (item is GeoRadio)
                {
                    GeoRadio geoRadio = item as GeoRadio;
                    geoRadio.IsChecked=false;
                }
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
            
            }
        }
        //Move之后一起执行
        public void Layout()
        {
            float length = 0;
            if (flowDirection==FLOW_DIRECTION.HORIZONTAL)
            {
                length = this.Controls[0].Width + padding;
                for (int i = 1; i < this.Controls.Count; i++)
                { 
                    this.Controls[i].Move(new PointGeo(length ,this.Height/2- this.Controls[i].Height/2));
                    length += this.Controls[i].Width + padding;
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
        public override List<DrawableObject> GetElements()
        {
            List<DrawableObject> temp = new List<DrawableObject>();
            temp.Add(baseShape);
            foreach (var item in this.Controls)
            {
                temp.AddRange(item.GetElements());
            } 
            return temp;
        }
    }
}
